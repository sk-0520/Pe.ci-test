#include <assert.h>

#include "text.h"

TEXT createEmptyText()
{
    TEXT result = {
        .value = NULL,
        .length = 0,
        ._mng = {
            .needRelease = false,
            .released = false,
        },
    };

    return result;
}

bool isEnabledText(const TEXT* text)
{
    if (!text) {
        return false;
    }
    if (text->_mng.released) {
        assert(!text->_mng.needRelease);
        return false;
    }
    if (!text->value) {
        return false;
    }

    return true;
}

TEXT newTextWithLength(const TCHAR* source, size_t length)
{
    TCHAR* buffer = allocateString(length);
    copyMemory(buffer, (void*)source, length * sizeof(TCHAR));
    buffer[length] = 0;

    TEXT result = {
        .value = buffer,
        .length = length,
        ._mng = {
            .needRelease = true,
            .released = false,
        },
    };

    return result;
}

TEXT newText(const TCHAR* source)
{
    if (!source) {
        return createEmptyText();
    }

    size_t length = getStringLength(source);
    return newTextWithLength(source, length);
}

TEXT wrapTextWithLength(const TCHAR* source, size_t length, bool needRelease)
{
    if (!source) {
        return createEmptyText();
    }

    TEXT result = {
        .value = source,
        .length = length,
        ._mng = {
            .needRelease = needRelease,
            .released = false,
        },
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
    if (!isEnabledText(source)) {
        return createEmptyText();
    }

    TCHAR* buffer = allocateString(source->length);
    copyMemory(buffer, (void*)source->value, source->length * sizeof(TCHAR));
    buffer[source->length] = 0;

    TEXT result = {
        .value = buffer,
        .length = source->length,
        ._mng = {
            .needRelease = true,
            .released = false,
        },
    };

    return result;
}

TEXT referenceText(const TEXT* source)
{
    if (!source->_mng.needRelease) {
        return *source;
    }

    TEXT result = {
        .value = source->value,
        .length = source->length,
        ._mng = {
            .needRelease = false,
            .released = false,
        }
    };

    return result;
}

bool freeText(TEXT* text)
{
    if (!isEnabledText(text)) {
        return false;
    }

    if (!text->_mng.needRelease) {
        return false;
    }

    if (!text->value) {
        return false;
    }

    freeString(text->value);
    text->value = 0;
    text->length = 0;

    text->_mng.released = true;

    return true;
}
