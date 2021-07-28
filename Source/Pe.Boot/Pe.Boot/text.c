#include "debug.h"
#include "text.h"
#include "writer.h"
#include "string_builder.h"

#define FORMAT_LENGTH (256)


TEXT create_invalid_text()
{
    TEXT result = {
        .value = NULL,
        .length = 0,
        .library = {
            .need_release = false,
            .sentinel = false,
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
            .sentinel = true,
            .released = false,
        },
    };

    return result;
}

TEXT RC_HEAP_FUNC(new_empty_text)
{
    return RC_HEAP_CALL(new_text_with_length, _T(""), 0);
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
            .sentinel = false,
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

    TEXT result = wrap_text_with_length(source, length, false);
    result.library.sentinel = true;
    return result;
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
            .sentinel = true,
            .released = false,
        },
    };

    return result;
}

TEXT reference_text_width_length(const TEXT* source, size_t index, size_t length)
{
    if (!source->library.need_release && !index && source->length == length) {
        return *source;
    }

    if(source->length <= index + length) {
        return create_invalid_text();
    }
    if (!length) {
        length = source->length - index;
    }

    TEXT result = {
        .value = source->value + index,
        .length = length,
        .library = {
            .need_release = false,
            .sentinel = false, //TODO: 番兵判定できるはず
            .released = false,
        }
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
            //.sentinel = source->library.sentinel,
            .released = false,
        }
    };
    result.library.sentinel = source->library.sentinel;

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

TEXT RC_HEAP_FUNC(format_text, const TEXT* format, ...)
{
    STRING_BUILDER sb = RC_HEAP_CALL(create_string_builder, FORMAT_LENGTH);
    va_list ap;
    va_start(ap, format);

    append_builder_vformat(&sb, format, ap);

    va_end(ap);

    TEXT result = RC_HEAP_CALL(build_text_string_builder, &sb);

    RC_HEAP_CALL(free_string_builder, &sb);

    return result;
}
