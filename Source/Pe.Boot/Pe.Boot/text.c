#include "text.h"

static TEXT _createEmptyText()
{
    TEXT result = {
        NULL,
        0,
        false,
        false,
    };

    return result;
}

TEXT createText(const TCHAR* source)
{
    if (!source) {
        return _createEmptyText();
    }

    size_t length = getStringLength(source);
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

TEXT wrapText(const TCHAR* source)
{
    if (!source) {
        return _createEmptyText();
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

    text->_released = true;

    return true;
}
