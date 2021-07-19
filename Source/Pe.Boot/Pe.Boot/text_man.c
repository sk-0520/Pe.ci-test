#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"


size_t getTextLength(const TEXT* text)
{
    if (!isEnabledText(text)) {
        return 0;
    }

    return text->length;
}

TEXT addText(const TEXT* source, const TEXT* text)
{
    bool enabledSource = isEnabledText(source);
    bool enabledText = isEnabledText(text);

    if (!enabledSource && !enabledText) {
        return createInvalidText();
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

static bool containsCharacters(TCHAR c, const TCHAR* characters, size_t count)
{
    for (size_t i = 0; i < count; i++) {
        if (c == characters[i]) {
            return true;
        }
    }

    return false;
}

TEXT trimText(const TEXT* text, bool start, bool end, const TCHAR* characters, size_t count)
{
    assert(text);
    if ((!start && !end) || !count) {
        return cloneText(text);
    }
    assert(characters);

    size_t beginIndex = 0;
    for (size_t i = 0; start && i < text->length; i++) {
        bool find = containsCharacters(text->value[i], characters, count);
        if (!find) {
            beginIndex = i;
            break;
        }
        if (i == text->length - 1) {
            // 最後まで行っちゃった
            return newText(_T(""));
        }
    }

    size_t endIndex = text->length - 1;
    for (size_t i = endIndex; end; i--) {
        bool find = containsCharacters(text->value[i], characters, count);
        if (!find) {
            endIndex = i;
            break;
        }
        if (!i) {
            // 最初まで行っちゃった
            return newText(_T(""));
        }
    }

    return newTextWithLength(text->value + beginIndex, endIndex - beginIndex + 1);
}

TEXT trimWhiteSpaceText(const TEXT* text)
{
    TCHAR characters[] = { _T(' '), _T('\t') };
    return trimText(text, true, true, characters, SIZEOF_ARRAY(characters));
}
