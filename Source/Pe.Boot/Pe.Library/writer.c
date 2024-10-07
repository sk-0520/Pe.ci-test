#include <stddef.h>
#include <stdarg.h>

#include "debug.h"
#include "memory.h"
#include "tstring.h"
#include "writer.h"

#define TRUE_UPPER "TRUE"
#define TRUE_LOWER "true"
#define FALSE_UPPER "FALSE"
#define FALSE_LOWER "false"

/// スタック上に確保する領域の上限(これにTCHARを掛けるので普通使用:UNICODE環境では2倍)
#define TEXT_STACK_COUNT (256)

#define HEX_L_INDEX (0)
#define HEX_U_INDEX (1)

static const TCHAR decimals[] = _T("0123456789");
static const TCHAR* hexes[] = { _T("0123456789abcdef"), _T("0123456789ABCDEF"), };

typedef enum tag_FORMAT_KIND
{
    FORMAT_KIND_UNKNOWN,
    FORMAT_KIND_INTEGER,
    FORMAT_KIND_UINTEGER,
    FORMAT_KIND_HEX,
    FORMAT_KIND_CHARACTER,
    FORMAT_KIND_POINTER,
    FORMAT_KIND_STRING,
    FORMAT_KIND_BOOLEAN,
    FORMAT_KIND_TEXT,
} FORMAT_KIND;

typedef struct tag_FORMAT_WIDTH
{
    size_t width;
    /// <summary>
    /// パラメータからの取得が必要(未実装)
    /// </summary>
    bool need_parameter;
} FORMAT_WIDTH;

bool is_success_write(const WRITE_RESULT* write_result)
{
    return write_result && write_result->error == WRITE_ERROR_KIND_NONE;
}

WRITE_RESULT write_success(size_t length)
{
    WRITE_RESULT result = {
        .write_length = length,
        .error = WRITE_ERROR_KIND_NONE,
    };

    return result;
}

WRITE_RESULT write_failed(WRITE_ERROR_KIND kind)
{
    WRITE_RESULT result = {
        .write_length = 0,
        .error = kind,
    };

    return result;
}


WRITE_RESULT write_primitive_boolean(func_string_writer writer, void* receiver, bool value, bool is_upper)
{
    WRITE_STRING_DATA data = {
        .receiver = receiver,
    };
    if (value) {
        if (is_upper) {
            data.value = _T(TRUE_UPPER);
            data.length = sizeof(TRUE_UPPER) - 1;
        } else {
            data.value = _T(TRUE_LOWER);
            data.length = sizeof(TRUE_LOWER) - 1;
        }
    } else {
        if (is_upper) {
            data.value = _T(FALSE_UPPER);
            data.length = sizeof(FALSE_UPPER) - 1;
        } else {
            data.value = _T(FALSE_LOWER);
            data.length = sizeof(FALSE_LOWER) - 1;
        }
    }

    return writer(&data);
}

#define number_bytes(is_hex, width) \
(is_hex \
    ? (sizeof(size_t) * 2 + 2 + width) \
    : (sizeof(size_t) * 8 + 1 + width) \
)

TCHAR get_fill_character(WRITE_PADDING write_padding)
{
    switch (write_padding) {
        case WRITE_PADDING_ZERO:
            return _T('0');

        case WRITE_PADDING_SPACE:
            return _T(' ');

        default:
            assert(false);
    }

    return _T('\0');
}

static size_t set_sign_and_fill(TCHAR* buffer, size_t buffer_length, size_t fill_buffer_length, size_t fill_buffer_index, bool is_sign, bool is_negative, WRITE_PADDING write_padding, WRITE_ALIGN write_align)
{
    if (buffer_length != fill_buffer_length) {
        if (write_align == WRITE_ALIGN_RIGHT || (write_align == WRITE_ALIGN_LEFT && write_padding == WRITE_PADDING_ZERO)) {
            TCHAR padding = get_fill_character(write_padding);
            if (padding) {
                for (size_t i = fill_buffer_index; i < fill_buffer_length; i++) {
                    buffer[i] = padding;
                }
                if (is_sign) {
                    if (write_padding == WRITE_PADDING_ZERO) {
                        buffer[fill_buffer_length - 1] = is_negative ? _T('-') : _T('+');
                    } else {
                        buffer[buffer_length - 1] = is_negative ? _T('-') : _T('+');
                    }
                }
            }
        } else {
            if (is_sign) {
                buffer[buffer_length - 1] = is_negative ? _T('-') : _T('+');
            }
            fill_buffer_length = buffer_length;
        }
    } else {
        if (is_sign) {
            buffer[buffer_length - 1] = is_negative ? _T('-') : _T('+');
        }
    }

    return fill_buffer_length;
}

static void reverse_buffer(TCHAR* buffer, size_t length)
{
    for (size_t left = 0, right = length - 1; left < right; left++, right--) {
        TCHAR c = buffer[left];
        buffer[left] = buffer[right];
        buffer[right] = c;
    }
}

static size_t fill_last(TCHAR* buffer, size_t fill_buffer_length, size_t width, WRITE_PADDING write_padding, WRITE_ALIGN write_align)
{
    if (write_align == WRITE_ALIGN_LEFT && write_padding == WRITE_PADDING_SPACE && fill_buffer_length < width) {
        TCHAR padding = get_fill_character(write_padding);
        for (size_t i = fill_buffer_length; i < width; i++) {
            buffer[i] = padding;
        }
        fill_buffer_length = width;
    }

    return fill_buffer_length;
}

WRITE_RESULT write_primitive_integer(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, ssize_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator)
{
    new_stack_or_heap_array(buffer, array, TCHAR, number_bytes(false, width), TEXT_STACK_COUNT, memory_arena_resource);

    size_t buffer_length = 0;
    bool is_negative = value < 0;
    ssize_t abs_value = is_negative ? -value : value;
    ssize_t number_of_digits = 0;
    bool is_sign = show_sign || is_negative;

    do {
        int n = abs_value % 10;
        buffer[buffer_length++] = decimals[n];
        number_of_digits += 1;
        if (n && separator && (number_of_digits % 3) == 0) {
            buffer[buffer_length++] = separator;
        }
        abs_value /= 10;
    } while (abs_value != 0);

    size_t fill_buffer_index = buffer_length;
    if (is_sign) {
        buffer_length += 1; // +-
    }
    size_t fill_buffer_length = MAX(buffer_length, width);

    fill_buffer_length = set_sign_and_fill(buffer, buffer_length, fill_buffer_length, fill_buffer_index, is_sign, is_negative, write_padding, write_align);

    reverse_buffer(buffer, fill_buffer_length);

    fill_buffer_length = fill_last(buffer, fill_buffer_length, width, write_padding, write_align);

    WRITE_STRING_DATA data = {
        .receiver = receiver,
        .value = buffer,
        .length = fill_buffer_length,
    };

    WRITE_RESULT result = writer(&data);

    release_stack_or_heap_array(array);

    return result;
}

WRITE_RESULT write_primitive_uinteger(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, size_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator)
{
    new_stack_or_heap_array(buffer, array, TCHAR, number_bytes(false, width), TEXT_STACK_COUNT, memory_arena_resource);
    size_t buffer_length = 0;
    bool is_negative = false;
    size_t abs_value = value;
    size_t number_of_digits = 0;
    bool is_sign = show_sign || is_negative;

    do {
        int n = abs_value % 10;
        buffer[buffer_length++] = decimals[n];
        number_of_digits += 1;
        if (separator && (number_of_digits % 3) == 0) {
            buffer[buffer_length++] = separator;
        }
        abs_value /= 10;
    } while (abs_value != 0);

    size_t fill_buffer_index = buffer_length;
    if (is_sign) {
        buffer_length += 1; // +-
    }
    size_t fill_buffer_length = MAX(buffer_length, width);

    fill_buffer_length = set_sign_and_fill(buffer, buffer_length, fill_buffer_length, fill_buffer_index, is_sign, is_negative, write_padding, write_align);

    reverse_buffer(buffer, fill_buffer_length);

    fill_buffer_length = fill_last(buffer, fill_buffer_length, width, write_padding, write_align);

    WRITE_STRING_DATA data = {
        .receiver = receiver,
        .value = buffer,
        .length = fill_buffer_length,
    };
    WRITE_RESULT result = writer(&data);

    release_stack_or_heap_array(array);

    return result;
}

//TODO: 諸々間違ってる
WRITE_RESULT write_primitive_hex(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, ssize_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool is_upper, bool alternate_form, size_t width)
{
    new_stack_or_heap_array(buffer, array, TCHAR, number_bytes(true, width), TEXT_STACK_COUNT, memory_arena_resource);
    ssize_t work_value = value;
    size_t buffer_length = 0;
    do {
        int n = work_value % 16;
        buffer[buffer_length++] = hexes[is_upper ? HEX_U_INDEX : HEX_L_INDEX][n];
        work_value /= 16;
    } while (work_value != 0);

    if (alternate_form) {
        buffer[buffer_length++] = is_upper ? _T('X') : _T('x');
        buffer[buffer_length++] = _T('0');
    }

    size_t fill_buffer_length = MAX(buffer_length, width);

    fill_buffer_length = set_sign_and_fill(buffer, buffer_length, fill_buffer_length, buffer_length, false, false, write_padding, write_align);

    reverse_buffer(buffer, fill_buffer_length);

    fill_buffer_length = fill_last(buffer, fill_buffer_length, width, write_padding, write_align);

    WRITE_STRING_DATA data = {
        .receiver = receiver,
        .value = buffer,
        .length = fill_buffer_length,
    };
    WRITE_RESULT result = writer(&data);

    release_stack_or_heap_array(array);

    return result;
}

//TODO: 諸々間違ってる
WRITE_RESULT write_primitive_uhex(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, size_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool is_upper, bool alternate_form, size_t width)
{
    new_stack_or_heap_array(buffer, array, TCHAR, number_bytes(true, width), TEXT_STACK_COUNT, memory_arena_resource);
    size_t work_value = value;
    size_t buffer_length = 0;
    do {
        int n = work_value % 16;
        buffer[buffer_length++] = hexes[is_upper ? HEX_U_INDEX : HEX_L_INDEX][n];
        work_value /= 16;
    } while (work_value != 0);

    if (alternate_form) {
        buffer[buffer_length++] = is_upper ? _T('X') : _T('x');
        buffer[buffer_length++] = _T('0');
    }

    size_t fill_buffer_length = MAX(buffer_length, width);

    fill_buffer_length = set_sign_and_fill(buffer, buffer_length, fill_buffer_length, buffer_length, false, false, write_padding, write_align);

    reverse_buffer(buffer, fill_buffer_length);

    fill_buffer_length = fill_last(buffer, fill_buffer_length, width, write_padding, write_align);

    WRITE_STRING_DATA data = {
        .receiver = receiver,
        .value = buffer,
        .length = fill_buffer_length,
    };
    WRITE_RESULT result = writer(&data);

    release_stack_or_heap_array(array);

    return result;
}

WRITE_RESULT write_primitive_character(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, TCHAR character, WRITE_ALIGN write_align, size_t width)
{
    size_t buffer_length = width ? width : 1;
    TCHAR buffer_core[TEXT_STACK_COUNT * sizeof(TCHAR)];
    TCHAR* buffer = buffer_length < TEXT_STACK_COUNT
        ? buffer_core
        : new_memory(buffer_length, sizeof(TCHAR), memory_arena_resource)
    ;
    buffer[0] = character;

    size_t fill_buffer_length = set_sign_and_fill(buffer, 1, buffer_length, 1, false, false, WRITE_PADDING_SPACE, write_align);

    reverse_buffer(buffer, fill_buffer_length);

    fill_buffer_length = fill_last(buffer, fill_buffer_length, width, WRITE_PADDING_SPACE, write_align);

    WRITE_STRING_DATA data = {
        .receiver = receiver,
        .value = buffer,
        .length = fill_buffer_length,
    };
    WRITE_RESULT result = writer(&data);

    if (buffer != buffer_core) {
        release_string(buffer, memory_arena_resource);
    }

    return result;
}

WRITE_RESULT write_primitive_pointer(func_string_writer writer, void* receiver, const void* pointer)
{
    TCHAR buffer[sizeof(void*) * 2];
    for (size_t i = 0; i < SIZEOF_ARRAY(buffer); i++) {
        buffer[i] = _T('0');
    }

    ptrdiff_t work_value = (ptrdiff_t)pointer;
    size_t buffer_length = 0;
    do {
        int n = work_value % 16;
        buffer[buffer_length++] = hexes[HEX_L_INDEX][n];
        work_value /= 16;
    } while (work_value != 0);

    reverse_buffer(buffer, sizeof(buffer) / sizeof(TCHAR));

    WRITE_STRING_DATA data = {
        .receiver = receiver,
        .value = buffer,
        .length = sizeof(buffer) / sizeof(TCHAR),
    };
    return writer(&data);
}

WRITE_RESULT write_address_string(func_string_writer writer, void* receiver, const TCHAR* string, WRITE_ALIGN write_align, size_t width)
{
    TEXT text = wrap_text(string);
    return write_address_text(writer, receiver, &text, write_align, width);
}

WRITE_RESULT write_address_text(func_string_writer writer, void* receiver, const TEXT* text, WRITE_ALIGN write_align, size_t width)
{
    WRITE_STRING_DATA data = {
        .receiver = receiver,
        .value = text->value,
        .length = text->length,
    };
    return writer(&data);
}

size_t get_write_format_flags(WRITE_FORMAT_FLAGS* result, const TEXT* format)
{
    result->align = WRITE_ALIGN_LEFT;
    result->padding = WRITE_PADDING_SPACE;
    result->alternate_form = false;
    result->show_sign = false;

    struct
    {
        bool align;
        bool show_sign;
        bool padding;
        bool alternate_form;
    } checked = { false };

    size_t read_length = 0;
    for (size_t i = 0; i < format->length; i++, read_length++) {
        TCHAR c = format->value[i];

        switch (c) {
            case _T('-'):
                if (!checked.align) {
                    result->align = WRITE_ALIGN_LEFT;
                    checked.align = true;
                }
                break;

            case _T('+'):
                if (!checked.show_sign) {
                    result->show_sign = true;
                    checked.show_sign = true;
                }
                break;

            case _T(' '):
                if (!checked.align && !checked.padding) {
                    // 違うんよなぁ
                    result->align = WRITE_ALIGN_RIGHT;
                    result->padding = WRITE_PADDING_SPACE;
                    checked.align = checked.padding = true;
                }
                break;

            case _T('#'):
                if (!checked.alternate_form) {
                    result->alternate_form = true;
                    checked.alternate_form = true;
                }
                break;

            case _T('0'):
                if (!checked.padding) {
                    result->padding = WRITE_PADDING_ZERO;
                    checked.padding = true;
                }
                break;

            default:
                return i;
        }
    }

    return read_length;
}

static FORMAT_KIND get_write_format_kind(TCHAR c)
{
    switch (c) {
        case _T('d'):
        case _T('i'):
            return FORMAT_KIND_INTEGER;

        case _T('u'):
            return FORMAT_KIND_UINTEGER;

        case _T('x'):
        case _T('X'):
            return FORMAT_KIND_HEX;

        case _T('c'):
            return FORMAT_KIND_CHARACTER;

        case _T('p'):
            return FORMAT_KIND_POINTER;

        case _T('s'):
            return FORMAT_KIND_STRING;

        case _T('b'):
        case _T('B'):
            return FORMAT_KIND_BOOLEAN;

        case _T('t'):
            return FORMAT_KIND_TEXT;

        default:
            return FORMAT_KIND_UNKNOWN;
    }
}

static size_t get_write_format_width(FORMAT_WIDTH* result, const TEXT* format, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    result->width = 0;
    result->need_parameter = false;

    for (size_t i = 0; i < format->length; i++) {
        TCHAR c = format->value[i];
        if (!i && c == _T('*')) {
            result->need_parameter = true;
            return i;
        }
        if (!i && _T('1') <= c && c <= _T('9')) {
            continue;
        }

        if (_T('0') <= c && c <= _T('9')) {
            continue;
        }

        TEXT number = reference_text_width_length(format, 0, i);
        TEXT_PARSED_I32_RESULT parsed_result = parse_i32_from_text(&number, PARSE_BASE_NUMBER_D);

        result->width = parsed_result.value;
        return i;
    }

    return 0;
}


static WRITE_RESULT write_format_simple_character(func_character_writer character_writer, void* receiver, TCHAR c)
{
    WRITE_CHARACTER_DATA char_data = {
        .receiver = receiver,
        .value = c,
    };
    return character_writer(&char_data);
}

static WRITE_RESULT write_format_value(func_string_writer string_writer, func_character_writer character_writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, FORMAT_KIND format_kind, const TEXT* format, va_list* ap)
{
    WRITE_FORMAT_FLAGS flags;
    size_t flag_skip_index = get_write_format_flags(&flags, format);
    TEXT convert_format = reference_text_width_length(format, flag_skip_index, 0);

    // 幅
    FORMAT_WIDTH format_waidth;
    size_t width_skip_index = get_write_format_width(&format_waidth, &convert_format, memory_arena_resource);

    // 長さ
    TEXT format_length = reference_text_width_length(&convert_format, width_skip_index, convert_format.length - width_skip_index - 1);

    // 変換指定子
    TCHAR format_type = convert_format.value[convert_format.length - 1];

    WRITE_RESULT result = { 0 };

    switch (format_kind) {
        case FORMAT_KIND_INTEGER:
        {
            if (format_length.length && format_length.value[format_length.length - 1] == _T('z')) {
#ifdef _WIN64
                ssize_t value = (ssize_t)va_arg(*ap, ssize_t);
#else
                ssize_t value = (ssize_t)va_arg(*ap, ssize_t);
#endif
                result = write_primitive_integer(string_writer, receiver, memory_arena_resource, value, flags.padding, flags.align, flags.show_sign, format_waidth.width, _T('\0'));
            } else {
#ifdef _WIN64
                long value = (long)va_arg(*ap, ssize_t);
#else
                ssize_t value = (ssize_t)va_arg(*ap, ssize_t);
#endif
                result = write_primitive_integer(string_writer, receiver, memory_arena_resource, value, flags.padding, flags.align, flags.show_sign, format_waidth.width, _T('\0'));
            }
        }
        break;

        case FORMAT_KIND_UINTEGER:
        {
            size_t value = (size_t)va_arg(*ap, size_t);
            result = write_primitive_uinteger(string_writer, receiver, memory_arena_resource, value, flags.padding, flags.align, flags.show_sign, format_waidth.width, _T('\0'));
        }
        break;

        case FORMAT_KIND_HEX:
        {
            // 16進数は size_t を強制
            bool is_upper = format_type == _T('X');
            size_t value = va_arg(*ap, size_t);
            result = write_primitive_hex(string_writer, receiver, memory_arena_resource, value, flags.padding, flags.align, is_upper, flags.alternate_form, format_waidth.width);
        }
        break;

        case FORMAT_KIND_CHARACTER:
        {
            TCHAR value = va_arg(*ap, TCHAR);
            result = write_primitive_character(string_writer, receiver, memory_arena_resource, value, flags.align, format_waidth.width);
        }
        break;

        case FORMAT_KIND_POINTER:
        {
            void* value = va_arg(*ap, void*);
            result = write_primitive_pointer(string_writer, receiver, value);
        }
        break;

        case FORMAT_KIND_STRING:
        {
            TCHAR* value = va_arg(*ap, TCHAR*);
            result = write_address_string(string_writer, receiver, value, flags.align, format_waidth.width);
        }
        break;

        case FORMAT_KIND_BOOLEAN:
        {
            bool is_upper = format_type == _T('B');
            bool value = va_arg(*ap, bool);
            result = write_primitive_boolean(string_writer, receiver, value, is_upper);
        }
        break;

        case FORMAT_KIND_TEXT:
        {
            TEXT* value = va_arg(*ap, TEXT*);
            result = write_address_text(string_writer, receiver, value, flags.align, format_waidth.width);
        }
        break;

        default:
            assert(false);
    }

    return result;
}

bool write_vformat(func_string_writer string_writer, func_character_writer character_writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, const TEXT* format, va_list ap)
{
    struct
    {
        /// <summary>
        /// 処理中の位置。
        /// </summary>
        size_t current_index;
        /// <summary>
        /// 書式開始位置。
        /// </summary>
        size_t begin_index;
        /// <summary>
        /// 書き込み数。
        /// </summary>
        size_t write_length;
        /// <summary>
        /// 書式化中。
        /// </summary>
        bool format;
    } status = {
        .format = false,
        .write_length = 0,
    };

    for (status.current_index = 0; status.current_index < format->length; status.current_index++) {
        TCHAR c = format->value[status.current_index];

        bool has_next = status.current_index + 1 < format->length;

        if (status.format) {
            assert(0 <= status.begin_index && status.begin_index < status.current_index);
            FORMAT_KIND format_kind = get_write_format_kind(c);
            if (format_kind == FORMAT_KIND_UNKNOWN) {
                continue;
            }
            size_t current_length = status.current_index - status.begin_index;
            TEXT current_format = wrap_text_with_length(format->value + status.begin_index + 1, current_length, false, NULL);

            WRITE_RESULT write_result = write_format_value(string_writer, character_writer, receiver, memory_arena_resource, format_kind, &current_format, &ap);
            if (write_result.error == WRITE_ERROR_KIND_NONE) {
                status.write_length += write_result.write_length;
            }
            status.format = false;
        } else {
            if (c == _T('%')) {
                if (!has_next) {
                    // 次がないならそのまま%を出力してサヨナラ
                    WRITE_RESULT write_result = write_format_simple_character(character_writer, receiver, c);
                    if (is_success_write(&write_result)) {
                        status.write_length += write_result.write_length;
                    }
                    break;
                }
                if (format->value[status.current_index + 1] == _T('%')) {
                    // %% は % として解釈し、次の次へ回す
                    status.current_index += 1;

                    WRITE_RESULT write_result = write_format_simple_character(character_writer, receiver, c);
                    if (is_success_write(&write_result)) {
                        status.write_length += write_result.write_length;
                    }

                    continue;
                }

                status.format = true;
                status.begin_index = status.current_index;
            } else {
                write_format_simple_character(character_writer, receiver, c);
                status.write_length += 1;
            }
        }
    }

    return true;
}

bool write_format(func_string_writer string_writer, func_character_writer character_writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, const TEXT* format, ...)
{
    va_list ap;
    va_start(ap, format);

    bool result = write_vformat(string_writer, character_writer, receiver, memory_arena_resource, format, ap);

    va_end(ap);

    return result;
}
