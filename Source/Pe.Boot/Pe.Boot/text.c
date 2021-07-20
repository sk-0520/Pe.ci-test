#include <assert.h>

#include "text.h"

TEXT create_invalid_text()
{
    TEXT result = {
        .value = NULL,
        .length = 0,
        .library = {
            .need_release = false,
            .released = false,
        },
    };

    return result;
}

bool is_enabled_text(const TEXT* text)
{
    if (!text) {
        return false;
    }
    if (text->library.released) {
        assert(!text->library.need_release);
        return false;
    }
    if (!text->value) {
        return false;
    }

    return true;
}

#ifdef MEM_CHECK
TEXT mem_check__new_text_with_length(const TCHAR* source, size_t length, const TCHAR* callerFile, size_t callerLine)
#else
TEXT new_text_with_length(const TCHAR* source, size_t length)
#endif
{
#ifdef MEM_CHECK
    TCHAR* buffer = mem_check__allocate_string(length, callerFile, callerLine);
#else
    TCHAR* buffer = allocate_string(length);
#endif
    copy_memory(buffer, (void*)source, length * sizeof(TCHAR));
    buffer[length] = 0;

    TEXT result = {
        .value = buffer,
        .length = length,
        .library = {
            .need_release = true,
            .released = false,
        },
    };

    return result;
}

#ifdef MEM_CHECK
TEXT mem_check__new_text(const TCHAR* source, const TCHAR* callerFile, size_t callerLine)
#else
TEXT new_text(const TCHAR* source)
#endif
{
    if (!source) {
        return create_invalid_text();
    }

    size_t length = get_string_length(source);
#ifdef MEM_CHECK
    return mem_check__new_text_with_length(source, length, callerFile, callerLine);
#else
    return new_text_with_length(source, length);
#endif
}


TEXT wrap_text_with_length(const TCHAR* source, size_t length, bool need_release)
{
    if (!source) {
        return create_invalid_text();
    }

    TEXT result = {
        .value = source,
        .length = length,
        .library = {
            .need_release = need_release,
            .released = false,
        },
    };

    return result;
}

TEXT wrap_text(const TCHAR* source)
{
    if (!source) {
        return create_invalid_text();
    }

    size_t length = get_string_length(source);

    return wrap_text_with_length(source, length, false);
}

#ifdef MEM_CHECK
TEXT mem_check__clone_text(const TEXT* source, MEM_CHECK_HEAD_ARGS)
#else
TEXT clone_text(const TEXT* source)
#endif
{
    if (!is_enabled_text(source)) {
        return create_invalid_text();
    }

    TCHAR* buffer =
#ifdef MEM_CHECK
        mem_check__allocate_string(source->length, MEM_CHECK_CALL_ARGS)
#else
        allocate_string(source->length)
#endif
        ;

    copy_memory(buffer, (void*)source->value, source->length * sizeof(TCHAR));
    buffer[source->length] = 0;

    TEXT result = {
        .value = buffer,
        .length = source->length,
        .library = {
            .need_release = true,
            .released = false,
        },
    };

    return result;
}

TEXT reference_text(const TEXT* source)
{
    if (!source->library.need_release) {
        return *source;
    }

    TEXT result = {
        .value = source->value,
        .length = source->length,
        .library = {
            .need_release = false,
            .released = false,
        }
    };

    return result;
}

#ifdef MEM_CHECK
bool mem_check__free_text(TEXT* text, const TCHAR* callerFile, size_t callerLine)
#else
bool free_text(TEXT* text)
#endif
{
    if (!is_enabled_text(text)) {
        return false;
    }

    if (!text->library.need_release) {
        return false;
    }

    if (!text->value) {
        return false;
    }

    free_string(text->value);
    text->value = 0;
    text->length = 0;

    text->library.released = true;

    return true;
}
