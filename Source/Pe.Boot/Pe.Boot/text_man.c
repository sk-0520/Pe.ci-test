#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"


size_t get_text_length(const TEXT* text)
{
    if (!is_enabled_text(text)) {
        return 0;
    }

    return text->length;
}

TEXT add_text(const TEXT* source, const TEXT* text)
{
    bool enabled_source = is_enabled_text(source);
    bool enabled_text = is_enabled_text(text);

    if (!enabled_source && !enabled_text) {
        return create_invalid_text();
    }
    if (!enabled_source) {
        return clone_text(text);
    }
    if (!enabled_text) {
        return clone_text(source);
    }

    size_t buffer_length = source->length + text->length;
    TCHAR* buffer = allocate_string(buffer_length);
    copy_memory(buffer, source->value, source->length * sizeof(TCHAR));
    copy_memory(buffer + source->length, text->value, text->length * sizeof(TCHAR));
    buffer[buffer_length] = 0;

    return wrap_text_with_length(buffer, buffer_length, true);
}

TEXT join_text(const TEXT* separator, const TEXT texts[], size_t count, IGNORE_EMPTY ignore_empty)
{
    size_t total_length = separator->length ? separator->length * count - 1 : 0;
    for (size_t i = 0; i < count; i++) {
        total_length += (texts + i)->length;
    }

    TCHAR* buffer = allocate_string(total_length);
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

    return wrap_text_with_length(buffer, current_position, true);
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

    TCHAR whitespace[] = { ' ', '\t', };

    for (size_t i = 0; i < text->length; i++) {
        TCHAR c = text->value[i];
        bool existsWhiteSpace = false;
        for (size_t j = 0; j < SIZEOF_ARRAY(whitespace); j++) {
            if (whitespace[j] == c) {
                existsWhiteSpace = true;
                break;
            }
        }
        if (!existsWhiteSpace) {
            return false;
        }
    }

    return true;
}

static bool contains_characters(TCHAR c, const TCHAR* characters, size_t count)
{
    for (size_t i = 0; i < count; i++) {
        if (c == characters[i]) {
            return true;
        }
    }

    return false;
}

TEXT trim_text(const TEXT* text, bool start, bool end, const TCHAR* characters, size_t count)
{
    assert(text);
    if ((!start && !end) || !count) {
        return clone_text(text);
    }
    assert(characters);

    size_t begin_index = 0;
    for (size_t i = 0; start && i < text->length; i++) {
        bool find = contains_characters(text->value[i], characters, count);
        if (!find) {
            begin_index = i;
            break;
        }
        if (i == text->length - 1) {
            // 最後まで行っちゃった
            return new_text(_T(""));
        }
    }

    size_t end_index = text->length - 1;
    for (size_t i = end_index; end; i--) {
        bool find = contains_characters(text->value[i], characters, count);
        if (!find) {
            end_index = i;
            break;
        }
        if (!i) {
            // 最初まで行っちゃった
            return new_text(_T(""));
        }
    }

    return new_text_with_length(text->value + begin_index, end_index - begin_index + 1);
}

TEXT trim_whitespace_text(const TEXT* text)
{
    TCHAR characters[] = { _T(' '), _T('\t') };
    return trim_text(text, true, true, characters, SIZEOF_ARRAY(characters));
}
