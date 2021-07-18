#include <assert.h>

#include "text.h"

TEXT createInvalidText()
{
    TEXT result = {
        .value = NULL,
        .length = 0,
        .library = {
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
    if (text->library.released) {
        assert(!text->library.needRelease);
        return false;
    }
    if (!text->value) {
        return false;
    }

    return true;
}

#ifdef MEM_CHECK
TEXT mem_check__newTextWithLength(const TCHAR* source, size_t length, const TCHAR* callerFile, size_t callerLine)
#   define newTextWithLength(source, length) mem_check__newTextWithLength((source), (length), __FILEW__, __LINE__)
#else
TEXT newTextWithLength(const TCHAR* source, size_t length)
#endif
{
#ifdef MEM_CHECK
    TCHAR* buffer = mem_check__allocateString(length, callerFile, callerLine);
#else
    TCHAR* buffer = allocateString(length);
#endif
    copyMemory(buffer, (void*)source, length * sizeof(TCHAR));
    buffer[length] = 0;

    TEXT result = {
        .value = buffer,
        .length = length,
        .library = {
            .needRelease = true,
            .released = false,
        },
    };

    return result;
}

#ifdef MEM_CHECK
TEXT mem_check__newText(const TCHAR* source, const TCHAR* callerFile, size_t callerLine)
#else
TEXT newText(const TCHAR* source)
#endif
{
    if (!source) {
        return createInvalidText();
    }

    size_t length = getStringLength(source);
#ifdef MEM_CHECK
    return mem_check__newTextWithLength(source, length, callerFile, callerLine);
#else
    return newTextWithLength(source, length);
#endif
}

TEXT wrapTextWithLength(const TCHAR* source, size_t length, bool needRelease)
{
    if (!source) {
        return createInvalidText();
    }

    TEXT result = {
        .value = source,
        .length = length,
        .library = {
            .needRelease = needRelease,
            .released = false,
        },
    };

    return result;
}

TEXT wrapText(const TCHAR* source)
{
    if (!source) {
        return createInvalidText();
    }

    size_t length = getStringLength(source);

    return wrapTextWithLength(source, length, false);
}

TEXT cloneText(const TEXT* source)
{
    if (!isEnabledText(source)) {
        return createInvalidText();
    }

    TCHAR* buffer = allocateString(source->length);
    copyMemory(buffer, (void*)source->value, source->length * sizeof(TCHAR));
    buffer[source->length] = 0;

    TEXT result = {
        .value = buffer,
        .length = source->length,
        .library = {
            .needRelease = true,
            .released = false,
        },
    };

    return result;
}

TEXT referenceText(const TEXT* source)
{
    if (!source->library.needRelease) {
        return *source;
    }

    TEXT result = {
        .value = source->value,
        .length = source->length,
        .library = {
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

    if (!text->library.needRelease) {
        return false;
    }

    if (!text->value) {
        return false;
    }

    freeString(text->value);
    text->value = 0;
    text->length = 0;

    text->library.released = true;

    return true;
}
