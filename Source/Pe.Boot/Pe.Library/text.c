#include "debug.h"
#include "text.h"
#include "writer.h"
#include "string_builder.h"

#define FORMAT_LENGTH (256)

const TEXT NEWLINE_CR_TEXT = static_text(NEWLINE_CR);
const TEXT NEWLINE_LF_TEXT = static_text(NEWLINE_LF);
const TEXT NEWLINE_CRLF_TEXT = static_text(NEWLINE_CRLF);
const TEXT NEWLINE_TEXT = static_text(NEWLINE);

TEXT create_invalid_text()
{
    TEXT result = {
        .value = NULL,
        .length = 0,
        .library = {
            .memory_arena_resource = NULL,
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
        assert(!text->library.need_release);
        return false;
    }
    if (!text->value) {
        return false;
    }

    return true;
}

bool check_text_length(size_t length)
{
    return length <= TEXT_MAX;
}


TEXT RC_HEAP_FUNC(new_text_with_length, const TCHAR* source, size_t length, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!check_text_length(length)) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, length, memory_arena_resource);
    copy_memory(buffer, (void*)source, length * sizeof(TCHAR));
    buffer[length] = 0;

    TEXT result = {
        .value = buffer,
        .length = (text_t)length,
        .library = {
            .memory_arena_resource = memory_arena_resource,
            .need_release = true,
            .sentinel = true,
            .released = false,
    },
    };

    return result;
}

TEXT RC_HEAP_FUNC(new_text, const TCHAR* source, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!source) {
        return create_invalid_text();
    }

    size_t length = get_string_length(source);
    return RC_HEAP_CALL(new_text_with_length, source, length, memory_arena_resource);
}

TEXT RC_HEAP_FUNC(new_empty_text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return RC_HEAP_CALL(new_text_with_length, _T(""), 0, memory_arena_resource);
}

TEXT wrap_text_with_length(const TCHAR* source, size_t length, bool need_release, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!source) {
        return create_invalid_text();
    }
    if (!check_text_length(length)) {
        return create_invalid_text();
    }
    if (need_release && !is_enabled_memory_arena_resource(memory_arena_resource)) {
        return create_invalid_text();
    }

    TEXT result = {
        .value = source,
        .length = (text_t)length,
        .library = {
            .memory_arena_resource = (need_release && memory_arena_resource) ? memory_arena_resource : NULL,
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

    TEXT result = wrap_text_with_length(source, length, false, NULL);
    result.library.sentinel = true;
    return result;
}

TEXT RC_HEAP_FUNC(clone_text, const TEXT* source, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!is_enabled_text(source)) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, source->length, memory_arena_resource);
    copy_memory(buffer, (void*)source->value, source->length * sizeof(TCHAR));
    buffer[source->length] = 0;

    TEXT result = {
        .value = buffer,
        .length = source->length,
        .library = {
            .memory_arena_resource = memory_arena_resource,
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

    if (source->length < index + length) {
        return create_invalid_text();
    }
    if (!length) {
        length = source->length - index;
    }
    if (!check_text_length(length)) {
        return create_invalid_text();
    }

    TEXT result = {
        .value = source->value + index,
        .length = (text_t)length,
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

bool RC_HEAP_FUNC(release_text, TEXT* text)
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

    RC_HEAP_CALL(release_string, text->value, text->library.memory_arena_resource);
    text->value = 0;
    text->length = 0;

    text->library.released = true;
    text->library.need_release = false;
    text->library.memory_arena_resource = NULL;

    return true;
}

TEXT RC_HEAP_FUNC(format_text, const MEMORY_ARENA_RESOURCE* memory_arena_resource, const TEXT* format, ...)
{
    STRING_BUILDER sb = RC_HEAP_CALL(new_string_builder, FORMAT_LENGTH, memory_arena_resource);
    va_list ap;
    va_start(ap, format);

    append_builder_vformat(&sb, format, ap);

    va_end(ap);

    TEXT result = RC_HEAP_CALL(build_text_string_builder, &sb);

    RC_HEAP_CALL(release_string_builder, &sb);

    return result;
}

TCHAR* RC_HEAP_FUNC(text_to_string, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!is_enabled_text(text)) {
        return RC_HEAP_CALL(allocate_string, 0, memory_arena_resource);
    }
    if (!text->length) {
        return RC_HEAP_CALL(allocate_string, 0, memory_arena_resource);
    }

    return RC_HEAP_CALL(clone_string_with_length, text->value, text->length, memory_arena_resource);
}

TEXT get_sentinel_text(const TEXT* text)
{
    assert(!text->library.sentinel);
    assert(is_enabled_text(text));

    const MEMORY_ARENA_RESOURCE* memory_arena_resource = text->library.memory_arena_resource
        ? text->library.memory_arena_resource
        : get_default_memory_arena_resource()
        ;

    return clone_text(text, memory_arena_resource);
}
