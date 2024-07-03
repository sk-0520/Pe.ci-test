#include <Windows.h>

#include "debug.h"
#include "text.h"
#include "tcharacter.h"

TCHAR get_relative_character(const TEXT* text, size_t base_index, ssize_t next_position)
{
    assert(text);

    size_t index = base_index + next_position;
    if (index < text->length) {
        return text->value[index];
    }

    return '\0';
}

static TEXT search_text_case_core(const TEXT* haystack, const TEXT* needle)
{
    assert(is_enabled_text(haystack));
    assert(is_enabled_text(needle));

    assert(!(haystack->length < needle->length));
    assert(!(haystack->length == needle->length && haystack->value == needle->value));

    for (size_t haystack_index = 0; haystack_index < haystack->length - 1; haystack_index++) {
        if (haystack->value[haystack_index] == needle->value[0]) {
            if (needle->length == 1) {
                return wrap_text_with_length(haystack->value + haystack_index, haystack->length - haystack_index, false, NULL);
            }

            const TCHAR* haystack_character = haystack->value + haystack_index + 1;
            const TCHAR* needle_character = needle->value + 1;

            bool is_equal = !compare_memory(haystack_character, needle_character, (needle->length - 1) * sizeof(TCHAR));
            if (is_equal) {
                return wrap_text_with_length(haystack->value + haystack_index, haystack->length - haystack_index, false, NULL);
            }
        }
    }

    return create_invalid_text();
}

static TEXT search_text_ignore_case_core(const TEXT* haystack, const TEXT* needle)
{
    assert(is_enabled_text(haystack));
    assert(is_enabled_text(needle));

    assert(!(haystack->length < needle->length));
    assert(!(haystack->length == needle->length && haystack->value == needle->value));

    for (size_t haystack_index = 0; haystack_index < haystack->length - 1; haystack_index++) {
        if (to_lower_character(haystack->value[haystack_index]) == to_lower_character(needle->value[0])) {
            if (needle->length == 1) {
                return wrap_text_with_length(haystack->value + haystack_index, haystack->length - haystack_index, false, NULL);
            }

            const TCHAR* haystack_character = haystack->value + haystack_index + 1;
            const TCHAR* needle_character = needle->value + 1;

            bool is_equal = false;
            for (size_t i = 1; i < haystack->length; i++) {
                if (to_lower_character(*haystack_character++) == to_lower_character(*needle_character++)) {
                    is_equal = true;
                    continue;
                }
                break;
            }

            if (is_equal) {
                return wrap_text_with_length(haystack->value + haystack_index, haystack->length - haystack_index, false, NULL);
            }
        }
    }

    return create_invalid_text();
}

TEXT search_text(const TEXT* haystack, const TEXT* needle, bool ignore_case)
{
    if (!is_enabled_text(haystack)) {
        return create_invalid_text();
    }
    if (!is_enabled_text(needle)) {
        return create_invalid_text();
    }

    if (haystack->length < needle->length) {
        return create_invalid_text();
    }
    if (haystack->length == needle->length && haystack->value == needle->value) {
        return create_invalid_text();
    }

    return ignore_case
        ? search_text_ignore_case_core(haystack, needle)
        : search_text_case_core(haystack, needle)
        ;
}

static const TCHAR* search_character_core(const TEXT* haystack, TCHAR needle, INDEX_START_POSITION index_start_position)
{
    if (!haystack->length) {
        return NULL;
    }

    switch (index_start_position) {
        case INDEX_START_POSITION_HEAD:
            for (size_t i = 0; i < haystack->length; i++) {
                const TCHAR* c = haystack->value + i;
                if (*c == needle) {
                    return c;
                }
            }
            break;

        case INDEX_START_POSITION_TAIL:
            for (size_t i = 0; i < haystack->length; i++) {
                const TCHAR* c = haystack->value + (haystack->length - i) - 1;
                if (*c == needle) {
                    return c;
                }
            }
            break;

        default:
            assert(false);
    }

    return NULL;
}

TEXT search_character(const TEXT* haystack, TCHAR needle, INDEX_START_POSITION index_start_position)
{
    ssize_t index = index_of_character(haystack, needle, index_start_position);
    if (index < 0) {
        return create_invalid_text();
    }

    return wrap_text_with_length(haystack->value + index, haystack->length - index, false, NULL);
}

ssize_t index_of_character(const TEXT* haystack, TCHAR needle, INDEX_START_POSITION index_start_position)
{
    const TCHAR* s = search_character_core(haystack, needle, index_start_position);
    if (!s) {
        return -1;
    }

    return s - haystack->value;
}

bool is_equals_text(const TEXT* a, const TEXT* b, bool ignore_case)
{
    if (!a && !b) {
        return true;
    }
    if ((a && !b) || (!a && b)) {
        return false;
    }

    if (a->length != b->length) {
        return false;
    }

    if (ignore_case) {
        for (size_t i = 0; i < a->length; i++) {
            TCHAR a1 = a->value[i];
            TCHAR b1 = b->value[i];
            if ('a' <= a1 && a1 <= 'z') {
                a1 = a1 - 'a' + 'A';
            }
            if ('a' <= b1 && b1 <= 'z') {
                b1 = b1 - 'a' + 'A';
            }
            if (a1 != b1) {
                return false;
            }
        }
        return true;
    }

    return !compare_memory(a->value, b->value, a->length * sizeof(TCHAR));
}

int compare_text(const TEXT* a, const TEXT* b, bool ignore_case)
{
    if (a->library.sentinel && b->library.sentinel) {
        // 番兵あり
        return ignore_case
            ? lstrcmpi(a->value, b->value)
            : lstrcmp(a->value, b->value)
            ;
    }

    // 番兵なし
    return compare_text_detail(a, b, -1, ignore_case ? TEXT_COMPARE_MODE_IGNORE_CASE : TEXT_COMPARE_MODE_NONE, LOCALE_TYPE_INVARIANT).compare;
}

static int get_compare_text_length(const TEXT* text, ssize_t width)
{
    assert(width);

    if (0 < width) {
        return MIN((int)text->length, (int)width);
    } else {
        return (int)text->length;
    }
}

static int get_compare_text_minimum_length(const TEXT* a, const TEXT* b)
{
    return (int)MIN(a->length, b->length);
}

TEXT_COMPARE_RESULT compare_text_detail(const TEXT* a, const TEXT* b, ssize_t width, TEXT_COMPARE_MODE mode, LOCALE_TYPE locale)
{
    if (!a->length && !b->length) {
        TEXT_COMPARE_RESULT none = {
            .compare = 0,
            .success = true,
        };
        return none;
    }

    int a_length = width ? get_compare_text_length(a, width) : get_compare_text_minimum_length(a, b);
    int b_length = width ? get_compare_text_length(b, width) : get_compare_text_minimum_length(a, b);

    int result = CompareString(locale, mode, a->value, a_length, b->value, b_length);
    if (!result) {
        TEXT_COMPARE_RESULT none = {
            .compare = 0,
            .success = false,
        };
        return none;
    }

    TEXT_COMPARE_RESULT success = {
        .success = true,
    };

    switch (result) {
        case CSTR_LESS_THAN:
            success.compare = -1;
            break;

        case CSTR_EQUAL:
            success.compare = 0;
            break;

        case CSTR_GREATER_THAN:
            success.compare = +1;
            break;

        default:
            assert(false);
    }

    return success;
}

bool starts_with_text(const TEXT* text, const TEXT* word)
{
    if (text->length < word->length) {
        return false;
    }

    return !compare_memory(text->value, word->value, word->length * sizeof(TCHAR));
}
