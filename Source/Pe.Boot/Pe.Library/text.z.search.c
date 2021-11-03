#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"


TEXT find_text(const TEXT* haystack, const TEXT* needle, bool ignore_case)
{
    //TODO: 番兵なし対応
    TCHAR* s = ignore_case
        ? StrStrI(haystack->value, needle->value)
        : StrStr(haystack->value, needle->value)
        ;

    if (!s) {
        return create_invalid_text();
    }

    return wrap_text(s);
}

static TCHAR* find_character_core(const TCHAR* haystack, TCHAR needle)
{
    while (*haystack != needle) {
        if (!*haystack) {
            return NULL;
        }
        haystack++;
    }

    return (TCHAR*)haystack;
}

TEXT find_character(const TEXT* haystack, TCHAR needle)
{
    //TODO: 番兵なし対応

    TCHAR* s = find_character_core(haystack->value, needle);
    if (!s) {
        return create_invalid_text();
    }

    return wrap_text(s);
}

ssize_t index_of_character(const TEXT* haystack, TCHAR needle)
{
    //TODO: 番兵なし対応
    TCHAR* s = find_character_core(haystack->value, needle);
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

    return !compare_memory(a->value, b->value, a->length);
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
