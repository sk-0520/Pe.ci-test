#include "debug.h"
#include "tstring.h"
#include "string_builder.h"
#include "writer.h"

STRING_BUILDER RC_HEAP_FUNC(new_string_builder, size_t capacity, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    assert(capacity);

    PRIMITIVE_LIST_TCHAR list = RC_HEAP_CALL(new_primitive_list, PRIMITIVE_LIST_TYPE_TCHAR, capacity, memory_arena_resource);

    STRING_BUILDER result = {
        .newline = NEWLINE_TEXT,
        .library = {
            .list = list,
        }
    };

    return result;
}

bool RC_HEAP_FUNC(release_string_builder, STRING_BUILDER* string_builder)
{
    if (!string_builder) {
        return false;
    }

    RC_HEAP_CALL(release_text, &string_builder->newline);

    return RC_HEAP_CALL(release_primitive_list, &string_builder->library.list);
}

static STRING_BUILDER* append_string_core(STRING_BUILDER* string_builder, const TCHAR* s, size_t length)
{
    add_range_list_tchar(&string_builder->library.list, s, length);

    return string_builder;
}

TEXT RC_HEAP_FUNC(build_text_string_builder, const STRING_BUILDER* string_builder)
{
    if (!string_builder) {
        return create_invalid_text();
    }

    if (!string_builder->library.list.length) {
        return RC_HEAP_CALL(new_text, _T(""), string_builder->library.list.library.memory_arena_resource);
    }

    TCHAR* s = RC_HEAP_CALL(allocate_string, string_builder->library.list.length, string_builder->library.list.library.memory_arena_resource);
    const TCHAR* buffer = reference_list_tchar(&string_builder->library.list);
    copy_memory(s, buffer, string_builder->library.list.length * sizeof(TCHAR));
    return wrap_text_with_length(s, string_builder->library.list.length, true, string_builder->library.list.library.memory_arena_resource);
}

TEXT reference_text_string_builder(STRING_BUILDER* string_builder)
{
    TCHAR* buffer;
    if (string_builder->library.list.length < string_builder->library.list.library.capacity_bytes * sizeof(TCHAR)) {
        buffer = reference_list_tchar(&string_builder->library.list);
        buffer[string_builder->library.list.length] = 0;
    } else {
        append_builder_character(string_builder, _T('0'), false);
        buffer = reference_list_tchar(&string_builder->library.list);
    }
    return wrap_text_with_length(buffer, string_builder->library.list.length, false, NULL);
}

STRING_BUILDER* clear_builder(STRING_BUILDER* string_builder)
{
    clear_primitive_list(&string_builder->library.list);

    return string_builder;
}


STRING_BUILDER* append_builder_newline(STRING_BUILDER* string_builder)
{
    return append_string_core(string_builder, string_builder->newline.value, string_builder->newline.length);
}

STRING_BUILDER* append_builder_string(STRING_BUILDER* string_builder, const TCHAR* s, bool newline)
{
    if (!s) {
        return string_builder;
    }

    size_t length = get_string_length(s);
    if (!length) {
        return string_builder;
    }

    append_string_core(string_builder, s, length);
    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

STRING_BUILDER* append_builder_text(STRING_BUILDER* string_builder, const TEXT* text, bool newline)
{
    if (!text) {
        return string_builder;
    }

    if (!text->length) {
        return string_builder;
    }

    append_string_core(string_builder, text->value, text->length);
    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

STRING_BUILDER* append_builder_character(STRING_BUILDER* string_builder, TCHAR c, bool newline)
{
    push_list_tchar(&string_builder->library.list, c);

    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

static WRITE_RESULT write_character(const WRITE_CHARACTER_DATA* data)
{
    STRING_BUILDER* string_builder = (STRING_BUILDER*)data->receiver;
    append_builder_character(string_builder, data->value, false);
    return write_success(1);
}

static WRITE_RESULT write_string(const WRITE_STRING_DATA* data)
{
    STRING_BUILDER* string_builder = (STRING_BUILDER*)data->receiver;
    append_string_core(string_builder, data->value, data->length);
    return write_success(data->length);
}

STRING_BUILDER* append_builder_int(STRING_BUILDER* string_builder, ssize_t value, bool newline)
{
    if (0 <= value && value <= 9) {
        return append_builder_character(string_builder, (uint8_t)value + '0', newline);
    }

    write_primitive_integer(write_string, string_builder, string_builder->library.list.library.memory_arena_resource, value, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(' '));
    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

STRING_BUILDER* append_builder_uint(STRING_BUILDER* string_builder, size_t value, bool newline)
{
    if (0 <= value && value <= 9) {
        return append_builder_character(string_builder, (uint8_t)value + '0', newline);
    }

    write_primitive_uinteger(write_string, string_builder, string_builder->library.list.library.memory_arena_resource, value, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(' '));
    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

STRING_BUILDER* append_builder_bool(STRING_BUILDER* string_builder, bool value, bool newline)
{
    write_primitive_boolean(write_string, string_builder, value, false);
    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

STRING_BUILDER* append_builder_pointer(STRING_BUILDER* string_builder, const void* pointer, bool newline)
{
    write_primitive_pointer(write_string, string_builder, pointer);
    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

STRING_BUILDER* append_builder_vformat(STRING_BUILDER* string_builder, const TEXT* format, va_list ap)
{
    write_vformat(write_string, write_character, string_builder, string_builder->library.list.library.memory_arena_resource, format, ap);
    return string_builder;
}

STRING_BUILDER* append_builder_format(STRING_BUILDER* string_builder, const TEXT* format, ...)
{
    va_list ap;
    va_start(ap, format);

    append_builder_vformat(string_builder, format, ap);

    va_end(ap);

    return string_builder;
}
