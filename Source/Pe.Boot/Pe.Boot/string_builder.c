#include "debug.h"
#include "tstring.h"
#include "string_builder.h"
#include "writer.h"

STRING_BUILDER RC_HEAP_FUNC(initialize_string_builder, const TCHAR* s, size_t capacity)
{
    assert_debug(s);

    size_t length = get_string_length(s);
    size_t bytes = length * sizeof(TCHAR);

    TCHAR* buffer = RC_HEAP_CALL(allocate_memory, bytes, false);
    copy_memory(buffer, s, bytes);

    STRING_BUILDER result = {
        .buffer = buffer,
        .length = length,
        .library = {
            .capacity = MAX(length, capacity),
            .newline = NEWLINET,
    }
    };

    return result;
}

STRING_BUILDER RC_HEAP_FUNC(create_string_builder, size_t capacity)
{
    assert_debug(capacity);

    size_t bytes = capacity * sizeof(TCHAR);
    TCHAR* buffer = RC_HEAP_CALL(allocate_memory, bytes, false);
    buffer[0] = 0;

    STRING_BUILDER result = {
        .buffer = buffer,
        .length = 0,
        .library = {
            .capacity = capacity,
            .newline = NEWLINET,
    }
    };

    return result;
}

bool RC_HEAP_FUNC(free_string_builder, STRING_BUILDER* string_builder)
{
    if (!string_builder) {
        return false;
    }
    if (!string_builder->buffer) {
        return false;
    }

    RC_HEAP_CALL(free_memory, string_builder->buffer);
    string_builder->buffer = NULL;
    string_builder->length = 0;
    string_builder->library.capacity = 0;

    return true;
}



/// <summary>
/// 必要に応じて<c>STRING_BUILDER</c>予約領域を拡張。
/// </summary>
/// <param name="string_builder">対象の<c>STRING_BUILDER</c></param>
/// <param name="need_length">追加に必要なサイズ。</param>
/// <returns>拡張したサイズ。拡張しなかった場合は0。</returns>
static size_t extend_capacity_if_not_enough_string_builder(STRING_BUILDER* string_builder, size_t need_length)
{
    // まだ大丈夫なら何もしない
    size_t need_total_length = string_builder->length + need_length;
    if (need_total_length <= string_builder->library.capacity) {
        return 0;
    }

    // 必要な領域まで拡張
    size_t old_capacity = string_builder->library.capacity;
    size_t new_capacity = string_builder->library.capacity;
    do {
        new_capacity *= 2;
    } while (new_capacity < need_total_length);

    size_t new_bytes = new_capacity * sizeof(TCHAR);
    TCHAR* new_buffer = allocate_memory(new_bytes, false);
    TCHAR* old_buffer = string_builder->buffer;

    copy_memory(new_buffer, old_buffer, new_bytes);
    free_memory(old_buffer);

    string_builder->buffer = new_buffer;
    string_builder->library.capacity = new_capacity;

    return new_capacity - old_capacity;
}

static STRING_BUILDER* append_string_core(STRING_BUILDER* string_builder, const TCHAR* s, size_t length)
{
    extend_capacity_if_not_enough_string_builder(string_builder, length);
    copy_memory(string_builder->buffer + string_builder->length, s, length * sizeof(TCHAR));
    string_builder->length += length;
    assert_debug(string_builder->length <= string_builder->library.capacity);

    return string_builder;
}

TEXT RC_HEAP_FUNC(build_text_string_builder, const STRING_BUILDER* string_builder)
{
    if (!string_builder) {
        return create_invalid_text();
    }

    if (!string_builder->length) {
        return RC_HEAP_CALL(new_text, _T(""));
    }

    TCHAR* s = RC_HEAP_CALL(allocate_string, string_builder->length);
    copy_memory(s, string_builder->buffer, string_builder->length * sizeof(TCHAR));
    return wrap_text_with_length(s, string_builder->length, true);
}

TEXT reference_text_string_builder(STRING_BUILDER* string_builder)
{
    if (string_builder->length < string_builder->library.capacity) {
        string_builder->buffer[string_builder->length] = 0;
    } else {
        append_builder_character(string_builder, _T('0'), false);
    }
    return wrap_text_with_length(string_builder->buffer, string_builder->length, false);
}

STRING_BUILDER* append_builder_newline(STRING_BUILDER* string_builder)
{
    return append_string_core(string_builder, string_builder->library.newline, get_string_length(string_builder->library.newline));
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
    extend_capacity_if_not_enough_string_builder(string_builder, 1);
    string_builder->buffer[string_builder->length++] = c;

    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}

static bool write_string(void* receiver, const WRITE_STRING_DATA* data)
{
    STRING_BUILDER* string_builder = (STRING_BUILDER*)receiver;
    append_string_core(string_builder, data->value, data->length);
    return true;
}

STRING_BUILDER* append_builder_int(STRING_BUILDER* string_builder, ssize_t value, bool newline)
{
    if (0 <= value && value <= 9) {
        return append_builder_character(string_builder, (uint8_t)value + '0', newline);
    }

    write_primitive_integer(write_string, string_builder, value, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(' '));
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

    write_primitive_uinteger(write_string, string_builder, value, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(' '));
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

STRING_BUILDER* append_builder_pointer(STRING_BUILDER* string_builder, void* pointer, bool newline)
{
    write_primitive_pointer(write_string, string_builder, pointer);
    if (newline) {
        append_builder_newline(string_builder);
    }
    return string_builder;
}
