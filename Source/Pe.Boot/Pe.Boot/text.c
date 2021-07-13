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

TEXT wrapText(const TCHAR* source)
{
    if (!source) {
        return createEmptyText();
    }

    size_t length = getStringLength(source);

    TEXT result = {
        source,
        length,
        false,
        false,
    };

    return result;
}

TEXT cloneText(const TEXT* source)
{
    if (!source) {
        return createEmptyText();
    }

    if (source->_released) {
        return createEmptyText();
    }

    if (!source->value) {
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
    if (!text) {
        return false;
    }

    if (!text->_needRelease) {
        return false;
    }

    if (text->_released) {
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
