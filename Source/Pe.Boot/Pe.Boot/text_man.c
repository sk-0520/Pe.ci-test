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

    TEXT_PARSED_INT32_RESULT result;
    result.success = StrToIntEx(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;
}

TEXT_PARSED_INT64_RESULT parseLong(const TEXT* input, bool supportHex)
{
    if (!isEnabledText(input)) {
        return createFailedLongParseResult();
    }

    TEXT_PARSED_INT64_RESULT result;
    result.success = StrToInt64Ex(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;

}

