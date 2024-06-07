#include <Windows.h>

#include "tcharacter.h"
#include "tstring.h"
#include "path.h"
#include "logging.h"

static const TCHAR DIRECTORY_SEPARATORS[] = {
    DIRECTORY_SEPARATOR_CHARACTER,
    ALT_DIRECTORY_SEPARATOR_CHARACTER,
};

static PATH_INFO create_invalid_path_info(void)
{
    return (PATH_INFO)
    {
        .parent = create_invalid_text(),
            .name = create_invalid_text(),
            .name_without_extension = create_invalid_text(),
            .extension = create_invalid_text(),
            .library = {
            .need_release = false,
        },
    };
}

bool is_directory_separator(TCHAR c)
{
    return contains_characters(c, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));
}

bool has_root_path(const TEXT* text)
{
    assert(text);

    if (text->length) {
        if (is_directory_separator(text->value[0])) {
            return true;
        }

        if (2 <= text->length) {
            return text->value[1] == ':' && is_alphabet_character(text->value[0]);
        }

        //uncとか知らんし
    }

    return false;
}

TEXT RC_HEAP_FUNC(get_parent_directory_path, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    bool hittingSeparator = false;
    for (size_t i = 0; i < path->length; i++) {
        const TCHAR* tail = path->value + (path->length - i - 1);

        bool isSeparator = is_directory_separator(*tail);
        if (isSeparator) {
            if (hittingSeparator) {
                continue;
            }
            hittingSeparator = true;
            continue;
        }

        if (hittingSeparator) {
            size_t length = tail - path->value + 1;

            // ドライブの場合、セパレータを付与
            if (length == 2 && has_root_path(path)) {
                return new_text_with_length(path->value, length + 1, memory_arena_resource);
            }

            return new_text_with_length(path->value, length, memory_arena_resource);
        }
    }

    return new_empty_text(memory_arena_resource);
}

static TEXT split_path_core(const TEXT* source, size_t* next_index, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    size_t skip_index = 0;
    for (size_t i = 0; i < source->length; i++) {
        TCHAR current = source->value[i];

        if (is_directory_separator(current)) {
            if (!i || skip_index == i) {
                skip_index = i + 1;
                continue;
            }
            *next_index = i + 1;
            return wrap_text_with_length(source->value + skip_index, i - skip_index, false, NULL);
        }
    }

    *next_index = source->length;
    if (source->length && 0 < source->length - skip_index) {
        return reference_text_width_length(source, skip_index, source->length - skip_index);
    }

    return create_invalid_text();
}

OBJECT_LIST RC_HEAP_FUNC(split_path, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!path || !path->length) {
        return RC_HEAP_CALL(new_object_list, sizeof(TEXT), 0, NULL, compare_object_list_value_text, release_object_list_value_text, memory_arena_resource);
    }

    OBJECT_LIST list = RC_HEAP_CALL(split_text, path, split_path_core, memory_arena_resource);

    return list;
}

TEXT RC_HEAP_FUNC(canonicalize_path, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    OBJECT_LIST list = RC_HEAP_CALL(split_path, path, memory_arena_resource);

    const TEXT currentName = wrap_text(_T("."));
    const TEXT parentName = wrap_text(_T(".."));

    const TEXT** items = (TEXT**)RC_HEAP_CALL(new_memory, list.length, sizeof(TEXT*), memory_arena_resource);
    size_t item_length = 0;
    size_t capacity = list.length;
    for (size_t i = 0; i < list.length; i++) {
        const TEXT* value = (TEXT*)(void*)list.items + i;
        if (is_equals_text(value, &currentName, false)) {
            continue;
        }

        if (is_equals_text(value, &parentName, false) && item_length) {
            items[item_length - 1] = NULL;
            item_length -= 1;
            continue;
        } else {
            items[item_length++] = value;
            capacity += value->length;
        }
    }

    STRING_BUILDER string_builder = RC_HEAP_CALL(new_string_builder, capacity, memory_arena_resource);
    for (size_t i = 0; i < item_length; i++) {
        if (i) {
            append_builder_character_word(&string_builder, DIRECTORY_SEPARATOR_CHARACTER);
        }
        append_builder_text_word(&string_builder, items[i]);
    }

    RC_HEAP_CALL(release_memory, (TEXT**)items, memory_arena_resource);
    RC_HEAP_CALL(release_object_list, &list, true);

    TEXT result = RC_HEAP_CALL(build_text_string_builder, &string_builder);

    RC_HEAP_CALL(release_string_builder, &string_builder);

    return result;
}

TEXT RC_HEAP_FUNC(combine_path, const TEXT* base_path, const TEXT* relative_path, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    // 相対パスが絶対パスっぽければ相対パス自身を返す
    if (relative_path->length && is_directory_separator(relative_path->value[0])) {
        return RC_HEAP_CALL(trim_text, relative_path, TRIM_TARGETS_TAIL, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS), memory_arena_resource);
    }

    TEXT trimmed_base_path = trim_text_stack(base_path, TRIM_TARGETS_TAIL, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));
    TEXT trimmed_relative_path = trim_text_stack(relative_path, TRIM_TARGETS_BOTH, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));

    if (!trimmed_base_path.length || !trimmed_relative_path.length) {
        if (trimmed_base_path.length) {
            return clone_text(&trimmed_base_path, memory_arena_resource);
        }

        return clone_text(&trimmed_relative_path, memory_arena_resource);
    }

    size_t total_length = trimmed_base_path.length + sizeof(TCHAR/* \ */) + trimmed_relative_path.length;
    TCHAR* buffer = RC_HEAP_CALL(allocate_string, total_length, memory_arena_resource);

    copy_memory(buffer, trimmed_base_path.value, trimmed_base_path.length * sizeof(TCHAR));
    if (trimmed_relative_path.length) {
        copy_memory(buffer + trimmed_base_path.length + 1, trimmed_relative_path.value, trimmed_relative_path.length * sizeof(TCHAR));
        buffer[trimmed_base_path.length] = DIRECTORY_SEPARATOR_CHARACTER;
    }

    return wrap_text_with_length(buffer, total_length, true, memory_arena_resource);
}

TEXT RC_HEAP_FUNC(join_path, const TEXT* base_path, const TEXT_LIST paths, size_t count, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    size_t total_path_length = (size_t)base_path->length + 1 + count; // ディレクトリ区切り

    for (size_t i = 0; i < count; i++) {
        const TEXT* path = &paths[i];
        total_path_length += path->length;
    }

    STRING_BUILDER string_builder = RC_HEAP_CALL(new_string_builder, total_path_length, memory_arena_resource);
    TEXT trimmed_base_path = trim_text_stack(base_path, TRIM_TARGETS_TAIL, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));
    append_builder_text_word(&string_builder, &trimmed_base_path);

    for (size_t i = 0; i < count; i++) {
        TEXT trimmed_path = trim_text_stack(paths + i, TRIM_TARGETS_BOTH, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));
        if (trimmed_path.length) {
            append_builder_character_word(&string_builder, DIRECTORY_SEPARATOR_CHARACTER);
            append_builder_text_word(&string_builder, &trimmed_path);
        }
    }

    TEXT joined_path = RC_HEAP_CALL(build_text_string_builder, &string_builder);
    RC_HEAP_CALL(release_string_builder, &string_builder);

    TEXT result = RC_HEAP_CALL(canonicalize_path, &joined_path, memory_arena_resource);
    RC_HEAP_CALL(release_text, &joined_path);

    return result;
}

TEXT RC_HEAP_FUNC(get_module_path, HINSTANCE hInstance, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    size_t length = MAX_PATH;
    size_t path_length = 0;
    TCHAR* path = NULL;

    while (!path) {
        path = RC_HEAP_CALL(allocate_string, length, memory_arena_resource);
        if (!path) {
            return create_invalid_text();
        }

        DWORD module_path_length = GetModuleFileName(hInstance, path, (DWORD)length);
        if (!module_path_length) {
            path_length = 0;
            break;
        } else if (module_path_length >= length - 1) {
            RC_HEAP_CALL(release_string, path, memory_arena_resource);
            path = NULL;
            length *= 2;
        } else {
            path_length = module_path_length;
            break;
        }
    }

    TEXT result = path_length
        ? RC_HEAP_CALL(new_text_with_length, path, path_length, memory_arena_resource)
        : create_invalid_text()
        ;
    RC_HEAP_CALL(release_string, path, memory_arena_resource);

    return result;
}

PATH_INFO get_path_info_stack(const TEXT* path)
{
    if (!path || !is_enabled_text(path) || is_empty_text(path)) {
        return create_invalid_path_info();
    }

    TEXT trim_path = trim_text_stack(path, TRIM_TARGETS_TAIL, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));

    ssize_t last_sep_index = index_of_character(&trim_path, DIRECTORY_SEPARATOR_CHARACTER, INDEX_START_POSITION_TAIL);
    if (last_sep_index < 0) {
        last_sep_index = index_of_character(&trim_path, ALT_DIRECTORY_SEPARATOR_CHARACTER, INDEX_START_POSITION_TAIL);
    }
    if (last_sep_index < 0) {
        return create_invalid_path_info();
    }

    TEXT parent_path = reference_text_width_length(&trim_path, 0, last_sep_index);
    TEXT name = reference_text_width_length(&trim_path, last_sep_index + 1, 0);

    ssize_t last_ext_index = index_of_character(&name, _T('.'), INDEX_START_POSITION_TAIL);
    //TODO: あまあま処理なのできちんと修正が必要
    TEXT name_without_extension = last_ext_index < 0
        ? name
        : (last_ext_index
            ? reference_text_width_length(&name, 0, last_ext_index)
            : create_invalid_text()
        )
        ;
    TEXT extension = last_ext_index < 0 ? create_invalid_text() : reference_text_width_length(&name, last_ext_index + 1, 0);

    return (PATH_INFO)
    {
        .parent = parent_path,
            .name = name,
            .name_without_extension = name_without_extension,
            .extension = extension,
            .library = {
            .need_release = false,
        }
    };
}

PATH_INFO RC_HEAP_FUNC(clone_path_info, const PATH_INFO* path_info, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!path_info) {
        return create_invalid_path_info();
    }

    return (PATH_INFO)
    {
        .parent = RC_HEAP_CALL(clone_text, &path_info->parent, memory_arena_resource),
            .name = RC_HEAP_CALL(clone_text, &path_info->name, memory_arena_resource),
            .name_without_extension = RC_HEAP_CALL(clone_text, &path_info->name_without_extension, memory_arena_resource),
            .extension = RC_HEAP_CALL(clone_text, &path_info->extension, memory_arena_resource),
            .library = {
            .need_release = true,
        }
    };
}

bool RC_HEAP_FUNC(release_path_info, PATH_INFO* path_info)
{
    if (!path_info) {
        return false;
    }
    if (!path_info->library.need_release) {
        return false;
    }

    RC_HEAP_CALL(release_text, &path_info->parent);
    RC_HEAP_CALL(release_text, &path_info->name);
    RC_HEAP_CALL(release_text, &path_info->name_without_extension);
    RC_HEAP_CALL(release_text, &path_info->extension);
    path_info->library.need_release = false;

    return true;
}
