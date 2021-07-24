#include <stddef.h>

#include "debug.h"
#include "memory.h"
#include "tstring.h"
#include "writer.h"

#define TRUE_UPPER "TRUE"
#define TRUE_LOWER "true"
#define FALSE_UPPER "FALSE"
#define FALSE_LOWER "false"

static const TCHAR decimals[] = _T("0123456789");


bool write_to_primitive_boolean(func_string_writer writer, void* receiver, bool value, bool is_uppper)
{
    WRITE_STRING_DATA data;
    if (value) {
        if (is_uppper) {
            data.value = _T(TRUE_UPPER);
            data.length = sizeof(TRUE_UPPER) - 1;
        } else {
            data.value = _T(TRUE_LOWER);
            data.length = sizeof(TRUE_LOWER) - 1;
        }
    } else {
        if (is_uppper) {
            data.value = _T(FALSE_UPPER);
            data.length = sizeof(FALSE_UPPER) - 1;
        } else {
            data.value = _T(FALSE_LOWER);
            data.length = sizeof(FALSE_LOWER) - 1;
        }
    }

    return writer(receiver, &data);
}

static TCHAR* allocate_number(bool isHex, size_t width)
{
    return allocate_clear_memory(sizeof(size_t) * 8 + 1 + width + ((sizeof(size_t) * 8) / 3), sizeof(TCHAR));
}

TCHAR get_fill_character(WRITE_PADDING write_padding)
{
    switch (write_padding) {
        case WRITE_PADDING_ZERO:
            return _T('0');

        case WRITE_PADDING_SPACE:
            return _T(' ');

        default:
            assert_debug(false);
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

bool write_primitive_integer(func_string_writer writer, void* receiver, ssize_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator)
{
    TCHAR* buffer = allocate_number(width, sizeof(TCHAR));
    size_t buffer_length = 0;
    bool is_negative = value < 0;
    ssize_t abs_value = is_negative ? -value : value;
    ssize_t number_of_digits = 0;
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
        .value = buffer,
        .length = fill_buffer_length,
    };
    writer(receiver, &data);

    free_string(buffer);

    return true;
}

bool write_primitive_uinteger(func_string_writer writer, void* receiver, size_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator)
{
    TCHAR* buffer = allocate_number(width, sizeof(TCHAR));
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
        .value = buffer,
        .length = fill_buffer_length,
    };
    writer(receiver, &data);

    free_string(buffer);

    return true;
}

bool write_primitive_character(func_string_writer writer, void* receiver, TCHAR character, WRITE_ALIGN write_align, size_t width)
{
    size_t buffer_length = width ? width : 1;
    TCHAR* buffer = allocate_clear_memory(buffer_length, sizeof(TCHAR));
    buffer[0] = character;

    size_t fill_buffer_length = set_sign_and_fill(buffer, 1, buffer_length, 1, false, false, WRITE_PADDING_SPACE, write_align);

    reverse_buffer(buffer, fill_buffer_length);

    fill_buffer_length = fill_last(buffer, fill_buffer_length, width, WRITE_PADDING_SPACE, write_align);

    WRITE_STRING_DATA data = {
        .value = buffer,
        .length = fill_buffer_length,
    };
    writer(receiver, &data);

    free_string(buffer);

    return true;
}
