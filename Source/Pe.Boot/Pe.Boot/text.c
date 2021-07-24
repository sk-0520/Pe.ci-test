#include "debug.h"
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
        assert_debug(!text->library.need_release);
        return false;
    }
    if (!text->value) {
        return false;
    }

    return true;
}

TEXT RC_HEAP_FUNC(new_text_with_length, const TCHAR* source, size_t length)
{
    TCHAR* buffer = RC_HEAP_CALL(allocate_string, length);
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

TEXT RC_HEAP_FUNC(new_text, const TCHAR* source)
{
    if (!source) {
        return create_invalid_text();
    }

    size_t length = get_string_length(source);
    return RC_HEAP_CALL(new_text_with_length, source, length);
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

TEXT RC_HEAP_FUNC(clone_text, const TEXT* source)
{
    if (!is_enabled_text(source)) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, source->length);
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

bool RC_HEAP_FUNC(free_text, TEXT* text)
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

    RC_HEAP_CALL(free_string, text->value);
    text->value = 0;
    text->length = 0;

    text->library.released = true;
    text->library.need_release = false;

    return true;
}
