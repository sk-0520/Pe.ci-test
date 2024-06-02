#include <Windows.h>

#include "debug.h"
#include "tcharacter.h"
#include "text.h"
#include "object_list.h"

typedef struct tag_TRIM_RESULT
{
    size_t index;
    text_t length;
} TRIM_RESULT;

size_t get_text_length(const TEXT* text)
{
    if (!is_enabled_text(text)) {
        return 0;
    }

    return text->length;
}

TEXT RC_HEAP_FUNC(add_text, const TEXT* source, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    bool enabled_source = is_enabled_text(source);
    bool enabled_text = is_enabled_text(text);

    if (!enabled_source && !enabled_text) {
        return create_invalid_text();
    }
    if (!enabled_source) {
        return clone_text(text, memory_arena_resource);
    }
    if (!enabled_text) {
        return clone_text(source, memory_arena_resource);
    }

    size_t buffer_length = (size_t)(source->length) + text->length;
    TCHAR* buffer = allocate_string(buffer_length, memory_arena_resource);
    copy_memory(buffer, source->value, source->length * sizeof(TCHAR));
    copy_memory(buffer + source->length, text->value, text->length * sizeof(TCHAR));
    buffer[buffer_length] = 0;

    return wrap_text_with_length(buffer, buffer_length, true, memory_arena_resource);
}

TEXT RC_HEAP_FUNC(join_text, const TEXT* separator, const TEXT_LIST texts, size_t count, IGNORE_EMPTY ignore_empty, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    size_t total_length = separator->length ? separator->length * count - 1 : 0;
    for (size_t i = 0; i < count; i++) {
        total_length += (texts + i)->length;
    }

    TCHAR* buffer = allocate_string(total_length, memory_arena_resource);
    size_t current_position = 0;
    bool add_separator = false;
    for (size_t i = 0; i < count; i++) {
        const TEXT* text = texts + i;
        switch (ignore_empty) {
            case IGNORE_EMPTY_NONE:
                break;

            case IGNORE_EMPTY_ONLY:
                if (is_empty_text(text)) {
                    continue;
                }
                break;

            case IGNORE_EMPTY_WHITESPACE:
                if (is_whitespace_text(text)) {
                    continue;
                }
                break;
        }

        if (add_separator) {
            copy_memory(buffer + current_position, separator->value, separator->length * sizeof(TCHAR));
            current_position += separator->length;
        }

        copy_memory(buffer + current_position, text->value, text->length * sizeof(TCHAR));
        current_position += text->length;

        add_separator = true;
    }
    buffer[current_position] = 0;

    return wrap_text_with_length(buffer, current_position, true, memory_arena_resource);
}

bool is_empty_text(const TEXT* text)
{
    if (!is_enabled_text(text)) {
        return true;
    }

    return !text->length;
}

bool is_whitespace_text(const TEXT* text)
{
    if (!is_enabled_text(text)) {
        return true;
    }

    if (!text->length) {
        return true;
    }

    for (size_t i = 0; i < text->length; i++) {
        TCHAR c = text->value[i];
        bool existsWhiteSpace = contains_characters(c, library_whitespace_characters, SIZEOF_ARRAY(library_whitespace_characters));
        if (!existsWhiteSpace) {
            return false;
        }
    }

    return true;
}

/// <summary>
/// トリム内部処理。
/// </summary>
/// <param name="result">結果データ。</param>
/// <param name="text"></param>
/// <param name="start"></param>
/// <param name="end"></param>
/// <param name="characters"></param>
/// <param name="count"></param>
/// <returns>トリム可能か</returns>
static bool trim_core(TRIM_RESULT* result, const TEXT* text, TRIM_TARGETS targets, const TCHAR* characters, size_t count)
{
    assert(text);
    if (!targets || !count) {
        result->index = 0;
        result->length = text->length;
        return true;
    }
    assert(characters);

    size_t begin_index = 0;
    if ((targets & TRIM_TARGETS_HEAD) == TRIM_TARGETS_HEAD) {
        for (size_t i = 0; i < text->length; i++) {
            bool find = contains_characters(text->value[i], characters, count);
            if (!find) {
                begin_index = i;
                break;
            }
            if (i == (size_t)text->length - 1) {
                // 最後まで行っちゃった
                return false;
            }
        }
    }

    size_t end_index = (size_t)text->length - 1;
    if ((targets & TRIM_TARGETS_TAIL) == TRIM_TARGETS_TAIL) {
        for (size_t i = end_index; true; i--) {
            bool find = contains_characters(text->value[i], characters, count);
            if (!find) {
                end_index = i;
                break;
            }
            if (!i) {
                // 最初まで行っちゃった
                return false;
            }
        }
    }

    result->index = begin_index;
    result->length = (text_t)(end_index - begin_index + 1);

    return true;
}

TEXT RC_HEAP_FUNC(trim_text, const TEXT* text, TRIM_TARGETS targets, const TCHAR* characters, size_t count, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    TRIM_RESULT trim_result;
    if (trim_core(&trim_result, text, targets, characters, count)) {
        return new_text_with_length(text->value + trim_result.index, trim_result.length, memory_arena_resource);
    }

    return new_text(_T(""), memory_arena_resource);
}

TEXT RC_HEAP_FUNC(trim_whitespace_text, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return trim_text(text, TRIM_TARGETS_BOTH, library_whitespace_characters, SIZEOF_ARRAY(library_whitespace_characters), memory_arena_resource);
}

TEXT trim_text_stack(const TEXT* text, TRIM_TARGETS targets, const TCHAR* characters, size_t count)
{
    TRIM_RESULT trim_result;
    if (trim_core(&trim_result, text, targets, characters, count)) {
        return wrap_text_with_length(text->value + trim_result.index, trim_result.length, false, NULL);
    }

    return wrap_text_with_length(text->value, 0, false, NULL);
}

TEXT trim_whitespace_text_stack(const TEXT* text)
{
    return trim_text_stack(text, TRIM_TARGETS_BOTH, library_whitespace_characters, SIZEOF_ARRAY(library_whitespace_characters));
}

OBJECT_LIST RC_HEAP_FUNC(split_text, const TEXT* text, func_split_text function, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!text) {
        OBJECT_LIST none = RC_HEAP_CALL(new_object_list, sizeof(TEXT), 2, NULL, compare_object_list_value_text, release_object_list_value_text, memory_arena_resource);
        return none;
    }

    OBJECT_LIST result = RC_HEAP_CALL(new_object_list, sizeof(TEXT), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, NULL, compare_object_list_value_text, release_object_list_value_text, memory_arena_resource);

    TEXT source = *text;
    size_t prev_index = 0;
    size_t next_index = 0;
    while (true) {
        size_t current_next_index = 0;
        TEXT item = function(&source, &current_next_index, memory_arena_resource);
        if (!is_enabled_text(&item)) {
            break;
        }
        assert(!item.library.need_release);

        TEXT part_text = RC_HEAP_CALL(clone_text, &item, memory_arena_resource);
        push_object_list(&result, &part_text);

        next_index += current_next_index;
        if (next_index == prev_index && text->length <= next_index) {
            break;
        }
        prev_index = next_index;
        source = wrap_text_with_length(text->value + next_index, text->length - next_index, false, NULL);
    }

    return result;
}

static TEXT split_newline_text_core(const TEXT* source, size_t* next_index, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    for (size_t i = 0; i < source->length; i++) {
        TCHAR current = source->value[i];

        if (current == '\n') {
            *next_index = i + 1;
            return wrap_text_with_length(source->value, i, false, NULL);
        }

        if (current == '\r') {
            bool last = i + 1 == source->length;
            if (last) {
                *next_index = i + 1;
                return wrap_text_with_length(source->value, i, false, NULL);
            }

            TCHAR next = source->value[i + 1];
            if (next == '\n') {
                *next_index = i + 2;
                return wrap_text_with_length(source->value, i, false, NULL);
            }
            *next_index = i + 1;
            return wrap_text_with_length(source->value, i, false, NULL);
        }
    }

    *next_index = source->length;
    return reference_text_width_length(source, 0, source->length);
}

OBJECT_LIST RC_HEAP_FUNC(split_newline_text, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return RC_HEAP_CALL(split_text, text, split_newline_text_core, memory_arena_resource);
}
