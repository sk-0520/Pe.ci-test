#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"

#define TEXT_STACK_COUNT (32)

static TEXT_PARSED_I32_RESULT create_failed_i32_parse_result()
{
    TEXT_PARSED_I32_RESULT result = {
        .success = false,
    };

    return result;
}

#ifdef _WIN64
static TEXT_PARSED_I64_RESULT create_failed_i64_parse_result()
{
    TEXT_PARSED_I64_RESULT result = {
        .success = false,
    };

    return result;
}
#endif

static bool check_has_sign(const TEXT* text)
{
    assert(text);
    assert(text->length);

    return text->value[0] == '-' || text->value[0] == '+';
}

static TEXT skip_base_header(const TEXT* text, size_t base)
{
    if (2 <= text->length) {
        switch (base) {
            case 2:
                if (text->value[0] == _T('0') && (text->value[1] == _T('b') || text->value[0] == _T('B'))) {
                    return reference_text_width_length(text, 2, 0);
                }
                break;

            case 8:
                if (text->value[0] == _T('0') && (text->value[1] == _T('o') || text->value[0] == _T('O'))) {
                    return reference_text_width_length(text, 2, 0);
                }
                break;

            case 16:
                if (text->value[0] == _T('0') && (text->value[1] == _T('x') || text->value[0] == _T('X'))) {
                    return reference_text_width_length(text, 2, 0);
                }
                break;

            default:
                break;
        }
    }

    return *text;
}

TEXT_PARSED_I32_RESULT parse_i32_from_text(const TEXT* input, bool support_hex, const MEMORY_RESOURCE* memory_resource)
{
    if (!is_enabled_text(input)) {
        return create_failed_i32_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_I32_RESULT result;
#pragma warning(pop)
    if (input->library.sentinel) {
        result.success = StrToIntEx(input->value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);
    } else {
        //TEXT sentinel = clone_text(input, memory_resource);
        new_array_or_memory(buffer, array, TCHAR, input->length + 1, TEXT_STACK_COUNT, memory_resource);
        copy_memory(buffer, input->value, input->length * sizeof(TCHAR));
        buffer[input->length] = 0;
        result.success = StrToIntEx(buffer, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);
        //release_text(&sentinel);
        release_array_or_memory(array);
    }

    return result;
}

static TEXT_PARSED_I32_RESULT parse_i32_from_text_core(const TEXT* input, int32_t base)
{
    assert(2 <= base && base <= 36);

    if (!is_enabled_text(input)) {
        return create_failed_i32_parse_result();
    }

    TEXT trimmed_input = trim_whitespace_text_stack(input);
    if (!trimmed_input.length) {
        return create_failed_i32_parse_result();
    }

    bool has_sign = check_has_sign(&trimmed_input);

    TEXT sign_skip_text = has_sign ? reference_text_width_length(&trimmed_input, 1, 0) : trimmed_input;
    TEXT parse_target_text = skip_base_header(&sign_skip_text, base);

    int32_t total = 0;

    for (size_t i = 0; i < parse_target_text.length; i++) {
        TCHAR c = parse_target_text.value[i];
        if (i) {
            total *= base;
        }
        if (base <= 10) {
            int32_t n;
            if (_T('0') <= c && c <= (_T('0') + base - 1)) {
                n = c - '0';
            } else {
                return create_failed_i32_parse_result();
            }
            total += n;
        } else {
            int32_t n;
            if ('a' <= c && c <= ((_T('a') + base - 1 - 10))) {
                n = c - 'a' + 10;
            } else if ('A' <= c && c <= ((_T('A') + base - 1 - 10))) {
                n = c - 'A' + 10;
            } else if (_T('0') <= c && c <= (_T('0') + base - 1)) {
                n = c - '0';
            } else {
                return create_failed_i32_parse_result();
            }
            total += n;
        }
    }

    if (has_sign && trimmed_input.value[0] == _T('-')) {
        total *= -1;
    }

    TEXT_PARSED_I32_RESULT result = {
        .success = true,
        .value = total,
    };

    return result;
}

TEXT_PARSED_I32_RESULT parse_i32_from_text_2(const TEXT* input, bool support_hex)
{
    return parse_i32_from_text_core(input, support_hex ? 16 : 10);
}


#ifdef _WIN64
TEXT_PARSED_I64_RESULT parse_i64_from_text(const TEXT* input, bool support_hex, const MEMORY_RESOURCE* memory_resource)
{
    if (!is_enabled_text(input)) {
        return create_failed_i64_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_I64_RESULT result;
#pragma warning(pop)
    if (input->library.sentinel) {
        result.success = StrToInt64Ex(input->value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);
    } else {
        //TEXT sentinel = clone_text(input, memory_resource);
        new_array_or_memory(buffer, array, TCHAR, input->length + 1, TEXT_STACK_COUNT, memory_resource);
        copy_memory(buffer, input->value, input->length * sizeof(TCHAR));
        buffer[input->length] = 0;
        result.success = StrToInt64Ex(buffer, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);
        //release_text(&sentinel);
        release_array_or_memory(array);
    }

    return result;
}
#endif

TEXT_PARSED_I32_RESULT parse_i32_from_bin_text(const TEXT* input)
{
    return parse_i32_from_text_core(input, 2);
}

#ifdef _WIN64
TEXT_PARSED_I64_RESULT parse_i64_from_bin_text(const TEXT* input)
{
    if (!is_enabled_text(input)) {
        return create_failed_i64_parse_result();
    }

    if (!input->length) {
        return create_failed_i64_parse_result();
    }

    TEXT_PARSED_I64_RESULT result = {
        .success = true,
        .value = 0,
    };

    for (size_t i = 0; i < input->length; i++) {
        TCHAR c = input->value[i];
        result.value <<= 1;
        if (c == '1') {
            result.value += 1;
        } else if (c != '0') {
            result.success = false;
            return result;
        }
    }

    return result;
}
#endif

#ifdef _UNICODE

bool is_enabled_multibyte_character_result(const MULTIBYTE_CHARACTER_RESULT* mbcr)
{
    if (!mbcr) {
        return false;
    }

    if (!mbcr->buffer) {
        return false;
    }

    return true;
}

MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_RESOURCE* memory_resource)
{
    DWORD flags = 0;
    CHAR default_char = '?';

    int mc_length1 = WideCharToMultiByte(mbc_type, flags, input->value, (int)input->length, NULL, 0, &default_char, NULL);
    if (!mc_length1) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    uint8_t* buffer = RC_HEAP_CALL(allocate_raw_memory, mc_length1 * sizeof(uint8_t) + sizeof(uint8_t), true, memory_resource);
    int mc_length2 = WideCharToMultiByte(mbc_type, flags, input->value, (int)input->length, (LPSTR)buffer, mc_length1, &default_char, NULL);
    if (!mc_length2) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    if (mc_length1 != mc_length2) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    MULTIBYTE_CHARACTER_RESULT result = {
        .buffer = buffer,
        .length = mc_length2,
    };
    return result;
}

bool RC_HEAP_FUNC(release_multibyte_character_result, MULTIBYTE_CHARACTER_RESULT* mbcr, const MEMORY_RESOURCE* memory_resource)
{
    if (!mbcr) {
        return false;
    }
    if (!mbcr->buffer) {
        return false;
    }

    bool result = RC_HEAP_CALL(release_memory, mbcr->buffer, memory_resource);

    mbcr->buffer = NULL;
    mbcr->length = 0;

    return result;
}

TEXT RC_HEAP_FUNC(make_text_from_multibyte, const uint8_t* input, size_t length, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_RESOURCE* memory_resource)
{
    DWORD flags = 0;
    int wc_length1 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, NULL, 0);
    if (!wc_length1) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, wc_length1, memory_resource);
    int wc_length2 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, buffer, wc_length1);
    if (!wc_length2) {
        RC_HEAP_CALL(release_string, buffer, memory_resource);
        return create_invalid_text();
    }
    if (wc_length1 != wc_length2) {
        RC_HEAP_CALL(release_string, buffer, memory_resource);
        return create_invalid_text();
    }

    return wrap_text_with_length(buffer, wc_length2, true, memory_resource);
}


#endif // UNICODE
