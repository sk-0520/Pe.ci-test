#include <windows.h>


#include "tcharacter.h"
#include "tstring.h"
#include "path.h"
#include "logging.h"

static TCHAR DIRECTORY_SEPARATORS[] = { DIRECTORY_SEPARATOR_CHARACTER, ALT_DIRECTORY_SEPARATOR_CHARACTER, };

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

TEXT RC_HEAP_FUNC(get_parent_directory_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource)
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
                return new_text_with_length(path->value, length + 1, memory_resource);
            }

            return new_text_with_length(path->value, length, memory_resource);
        }
    }

    return new_empty_text(memory_resource);
}

OBJECT_LIST RC_HEAP_FUNC(split_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource)
{
    if (!path || !path->length) {
        return RC_HEAP_CALL(new_object_list, sizeof(TEXT), 0, NULL, compare_object_list_value_text, release_object_list_value_text, memory_resource);
    }

    OBJECT_LIST list = RC_HEAP_CALL(new_object_list, sizeof(TEXT), 16, NULL, compare_object_list_value_text, release_object_list_value_text, memory_resource);

    return list;
}

TEXT RC_HEAP_FUNC(combine_path, const TEXT* base_path, const TEXT* relative_path, const MEMORY_RESOURCE* memory_resource)
{
    // 相対パスが絶対パスっぽければ相対パス自身を返す
    if (relative_path->length && is_directory_separator(relative_path->value[0])) {
        return RC_HEAP_CALL(trim_text, relative_path, false, true, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS), memory_resource);
    }

    TEXT trimmed_base_path = trim_text_stack(base_path, false, true, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));
    TEXT trimmed_relative_path = trim_text_stack(relative_path, true, true, DIRECTORY_SEPARATORS, SIZEOF_ARRAY(DIRECTORY_SEPARATORS));

    if (!trimmed_base_path.length || !trimmed_relative_path.length) {
        if (trimmed_base_path.length) {
            return clone_text(&trimmed_base_path, memory_resource);
        }

        return clone_text(&trimmed_relative_path, memory_resource);
    }

    size_t total_length = trimmed_base_path.length + sizeof(TCHAR/* \ */) + trimmed_relative_path.length;
    TCHAR* buffer = RC_HEAP_CALL(allocate_string, total_length, memory_resource);

    copy_memory(buffer, trimmed_base_path.value, trimmed_base_path.length * sizeof(TCHAR));
    if (trimmed_relative_path.length) {
        copy_memory(buffer + trimmed_base_path.length + 1, trimmed_relative_path.value, trimmed_relative_path.length * sizeof(TCHAR));
        buffer[trimmed_base_path.length] = DIRECTORY_SEPARATOR_CHARACTER;
    }

    return wrap_text_with_length(buffer, total_length, true, memory_resource);
}

TEXT RC_HEAP_FUNC(join_path, const TEXT* base_path, const TEXT_LIST paths, size_t count, const MEMORY_RESOURCE* memory_resource)
{
    size_t total_path_length = (size_t)base_path->length + 1 + count; // ディレクトリ区切り

    for (size_t i = 0; i < count; i++) {
        const TEXT* path = &paths[i];
        total_path_length += path->length;
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, total_path_length, memory_resource);
    copy_string(buffer, base_path->value);

    for (size_t i = 0; i < count; i++) {
        const TEXT* path = &paths[i];
        PathCombine(buffer, buffer, path->value);
    }
    TCHAR* temp_buffer = clone_string(buffer, memory_resource);
    PathCanonicalize(buffer, temp_buffer);
    release_string(temp_buffer, memory_resource);

    return wrap_text_with_length(buffer, get_string_length(buffer), true, memory_resource);
}

TEXT RC_HEAP_FUNC(canonicalize_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource)
{
    TCHAR* buffer = RC_HEAP_CALL(allocate_string, path->length, memory_resource);
    PathCanonicalize(buffer, path->value);

    return wrap_text_with_length(buffer, get_string_length(buffer), true, memory_resource);
}

TEXT RC_HEAP_FUNC(get_module_path, HINSTANCE hInstance, const MEMORY_RESOURCE* memory_resource)
{
    size_t length = MAX_PATH;
    size_t path_length = 0;
    TCHAR* path = NULL;

    while (!path) {
        path = allocate_string(length, memory_resource);
        if (!path) {
            return create_invalid_text();
        }

        DWORD module_path_length = GetModuleFileName(hInstance, path, (DWORD)length);
        if (!module_path_length) {
            path_length = 0;
            break;
        } else if (module_path_length >= length - 1) {
            release_string(path, memory_resource);
            path = NULL;
            length *= 2;
        } else {
            path_length = module_path_length;
            break;
        }
    }

    TEXT result = path_length
        ? new_text_with_length(path, path_length, memory_resource)
        : create_invalid_text()
        ;
    release_string(path, memory_resource);

    return result;
}

