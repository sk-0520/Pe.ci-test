#include <assert.h>

#include <windows.h>
#include <shlwapi.h>

#include "text.h"


size_t getTextLength(const TEXT* text)
{
    if (!isEnabledText(text)) {
        return 0;
    }

    return text->length;
}

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

    int a_length = width ? getCompareTextLength(a, width): getCompareTextMinimumLength(a, b);
    int b_length = width ? getCompareTextLength(b, width): getCompareTextMinimumLength(a, b);

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

static TEXT_PARSED_INT32_RESULT createFailedIntegerParseResult()
{
    TEXT_PARSED_INT32_RESULT result = {
        .success = false,
    };

    return result;
}

static TEXT_PARSED_INT64_RESULT createFailedLongParseResult()
{
    TEXT_PARSED_INT64_RESULT result = {
        .success = false,
    };

    return result;
}

TEXT_PARSED_INT32_RESULT parseInteger(const TEXT* input, bool supportHex)
{
    if (!isEnabledText(input)) {
        return createFailedIntegerParseResult();
    }

//#pragma warning(push)
//#pragma warning(disable:6001)
    TEXT_PARSED_INT32_RESULT result;
//#pragma warning(pop)
    result.success = StrToIntEx(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;
}

TEXT_PARSED_INT64_RESULT parseLong(const TEXT* input, bool supportHex)
{
    if (!isEnabledText(input)) {
        return createFailedLongParseResult();
    }

//#pragma warning(push)
//#pragma warning(disable:6001)
    TEXT_PARSED_INT64_RESULT result;
//#pragma warning(pop)
    result.success = StrToInt64Ex(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;

}

TEXT addText(const TEXT* source, const TEXT* text)
{
    bool enabledSource = isEnabledText(source);
    bool enabledText = isEnabledText(text);

    if (!enabledSource && !enabledText) {
        return createEmptyText();
    }
    if (!enabledSource) {
        return cloneText(text);
    }
    if (!enabledText) {
        return cloneText(source);
    }

    size_t bufferLength = source->length + text->length;
    TCHAR* buffer = allocateString(bufferLength);
    copyMemory(buffer, source->value, source->length * sizeof(TCHAR));
    copyMemory(buffer + source->length, text->value, text->length * sizeof(TCHAR));
    buffer[bufferLength] = 0;

    return wrapTextWithLength(buffer, bufferLength, true);
}

TEXT joinText(const TEXT* separator, const TEXT texts[], size_t count, IGNORE_EMPTY ignoreEmpty)
{
    size_t totalLength = separator->length ? separator->length * count - 1 : 0;
    for (size_t i = 0; i < count; i++) {
        totalLength += (texts + i)->length;
    }

    TCHAR* buffer = allocateString(totalLength);
    size_t currentPosition = 0;
    bool addSeparator = false;
    for (size_t i = 0; i < count; i++) {
        const TEXT* text = texts + i;
        switch (ignoreEmpty) {
            case IGNORE_EMPTY_NONE:
                break;

            case IGNORE_EMPTY_ONLY:
                if (isEmptyText(text)) {
                    continue;
                }
                break;

            case IGNORE_EMPTY_WHITESPACE:
                if (isWhiteSpaceText(text)) {
                    continue;
                }
                break;
        }

        if (addSeparator) {
            copyMemory(buffer + currentPosition, separator->value, separator->length * sizeof(TCHAR));
            currentPosition += separator->length;
        }

        copyMemory(buffer + currentPosition, text->value, text->length * sizeof(TCHAR));
        currentPosition += text->length;

        addSeparator = true;
    }
    buffer[currentPosition] = 0;

    return wrapTextWithLength(buffer, currentPosition, true);
}

bool isEmptyText(const TEXT* text)
{
    if (!isEnabledText(text)) {
        return true;
    }

    return !text->length;
}

bool isWhiteSpaceText(const TEXT* text)
{
    if (!isEnabledText(text)) {
        return true;
    }

    if (!text->length) {
        return true;
    }

    TCHAR whiteSpace[] = { ' ', '\t', };

    for (size_t i = 0; i < text->length; i++) {
        TCHAR c = text->value[i];
        bool existsWhiteSpace = false;
        for (size_t j = 0; j < SIZEOF_ARRAY(whiteSpace); j++) {
            if (whiteSpace[j] == c) {
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
