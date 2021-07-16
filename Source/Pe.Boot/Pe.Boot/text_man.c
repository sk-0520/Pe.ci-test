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

TEXT joinText(const TEXT* separator, const TEXT texts[], size_t count)
{
    size_t totalLength = separator->length ? separator->length * count - 1 : 0;
    for (size_t i = 0; i < count; i++) {
        totalLength += (texts + i)->length;
    }

    TCHAR* buffer = allocateString(totalLength);
    size_t currentPosition = 0;
    for (size_t i = 0; i < count; i++) {
        if (i) {
            copyMemory(buffer + currentPosition, separator->value, separator->length * sizeof(TCHAR));
            currentPosition += separator->length;
        }

        const TEXT* text = texts + i;
        copyMemory(buffer + currentPosition, text->value, text->length * sizeof(TCHAR));
        currentPosition += text->length;
    }
    buffer[totalLength] = 0;

    return wrapTextWithLength(buffer, totalLength, 0);
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
