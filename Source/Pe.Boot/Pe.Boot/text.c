#include <assert.h>

#include "text.h"

TEXT createEmptyText()
{
    TEXT result = {
        NULL,
        0,
        false,
        false,
    };

    return result;
}

bool isEnableText(const TEXT* text)
{
    if (!text) {
        return false;
    }
    if (text->_released) {
        assert(!text->_needRelease);
        return false;
    }
    if (!text->value) {
        return false;
    }


    return true;
}

TEXT createTextWithLength(const TCHAR* source, size_t length)
{
    TCHAR* buffer = allocateMemory((length * sizeof(TCHAR)) + sizeof(TCHAR), false);
    copyMemory(buffer, (void*)source, length * sizeof(TCHAR));
    buffer[length] = 0;

    TEXT result = {
        buffer,
        length,
        true,
        false,
    };

    return result;
}

TEXT createText(const TCHAR* source)
{
    if (!source) {
        return createEmptyText();
    }

    size_t length = getStringLength(source);
    return createTextWithLength(source, length);
}

TEXT wrapTextWithLength(const TCHAR* source, size_t length, bool needRelease)
{
    if (!source) {
        return createEmptyText();
    }

    TEXT result = {
        source,
        length,
        needRelease,
        false,
    };

    return result;
}

TEXT wrapText(const TCHAR* source)
{
    if (!source) {
        return createEmptyText();
    }

    size_t length = getStringLength(source);

    return wrapTextWithLength(source, length, false);
}

TEXT cloneText(const TEXT* source)
{
    if (!isEnableText(source)) {
        return createEmptyText();
    }

    TCHAR* buffer = allocateMemory((source->length * sizeof(TCHAR)) + sizeof(TCHAR), false);
    copyMemory(buffer, (void*)source->value, source->length * sizeof(TCHAR));
    buffer[source->length] = 0;

    TEXT result = {
        buffer,
        source->length,
        true,
        false,
    };

    return result;
}

bool freeText(TEXT* text)
{
    if (!isEnableText(text)) {
        return false;
    }

    if (!text->_needRelease) {
        return false;
    }

    if (!text->value) {
        return false;
    }

    freeString(text->value);
    text->value = 0;
    text->length = 0;

    text->_released = true;

    return true;
}
