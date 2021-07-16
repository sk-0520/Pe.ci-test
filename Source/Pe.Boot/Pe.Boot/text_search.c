#include <assert.h>

#include <windows.h>
#include <shlwapi.h>

#include "text.h"


TEXT findText(const TEXT* haystack, const TEXT* needle, bool ignoreCase)
{
    TCHAR* s = ignoreCase
        ? StrStrI(haystack->value, needle->value)
        : StrStr(haystack->value, needle->value)
        ;

    if (!s) {
        return createEmptyText();
    }

    return wrapText(s);
}

static TCHAR* findCharacterCore(const TCHAR* haystack, TCHAR needle)
{
    while (*haystack != needle) {
        if (!*haystack) {
            return NULL;
        }
        haystack++;
    }

    return (TCHAR*)haystack;
}

TEXT findCharacter2(const TEXT* haystack, TCHAR needle)
{
    TCHAR* s = findCharacterCore(haystack->value, needle);
    if (!s) {
        return createEmptyText();
    }

    return wrapText(s);
}

ssize_t indexOfCharacter(const TEXT* haystack, TCHAR needle)
{
    TCHAR* s = findCharacterCore(haystack->value, needle);
    if (!s) {
        return -1;
    }

    return s - haystack->value;
}

int compareText(const TEXT* a, const TEXT* b, bool ignoreCase)
{
    return ignoreCase
        ? lstrcmpi(a->value, b->value)
        : lstrcmp(a->value, b->value)
        ;
}

static int getCompareTextLength(const TEXT* text, ssize_t width)
{
    assert(width);

    if (0 < width) {
        return MIN((int)text->length, (int)width);
    } else {
        return (int)text->length;
    }
}

static int getCompareTextMinimumLength(const TEXT* a, const TEXT* b)
{
    return (int)MIN(a->length, b->length);
}

TEXT_COMPARE_RESULT compareTextDetail(const TEXT* a, const TEXT* b, ssize_t width, TEXT_COMPARE_MODE mode, LOCALE_TYPE locale)
{
    if (!a->length && !b->length) {
        TEXT_COMPARE_RESULT none = {
            .compare = 0,
            .success = true,
        };
        return none;
    }

    int a_length = width ? getCompareTextLength(a, width) : getCompareTextMinimumLength(a, b);
    int b_length = width ? getCompareTextLength(b, width) : getCompareTextMinimumLength(a, b);

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

bool startsWithText(const TEXT* text, const TEXT* word)
{
    if (text->length < word->length) {
        return false;
    }

    return !compareMemory(text->value, word->value, word->length * sizeof(TCHAR));
}
