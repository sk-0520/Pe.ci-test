#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"


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

TEXT_PARSED_I32_RESULT parse_i32_from_text(const TEXT* input, bool support_hex)
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
        TEXT sentinel = clone_text(input);
        result.success = StrToIntEx(sentinel.value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);
        free_text(&sentinel);
    }

    return result;
}

#ifdef _WIN64
TEXT_PARSED_I64_RESULT parse_i64_from_text(const TEXT* input, bool support_hex)
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
        TEXT sentinel = clone_text(input);
        result.success = StrToInt64Ex(sentinel.value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);
        free_text(&sentinel);
    }

    return result;
}
#endif

TEXT_PARSED_I32_RESULT parse_i32_from_bin_text(const TEXT* input)
{
    if (!is_enabled_text(input)) {
        return create_failed_i32_parse_result();
    }

    if (!input->length) {
        return create_failed_i32_parse_result();
    }

    TEXT_PARSED_I32_RESULT result = {
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

MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE mbc_type)
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

    uint8_t* buffer = RC_HEAP_CALL(allocate_memory, mc_length1 * sizeof(uint8_t) + sizeof(uint8_t), true);
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

bool RC_HEAP_FUNC(free_multibyte_character_result, MULTIBYTE_CHARACTER_RESULT* mbcr)
{
    if (!mbcr) {
        return false;
    }
    if (!mbcr->buffer) {
        return false;
    }

    bool result = RC_HEAP_CALL(free_memory, mbcr->buffer);

    mbcr->buffer = NULL;
    mbcr->length = 0;

    return result;
}

TEXT RC_HEAP_FUNC(make_text_from_multibyte, const uint8_t* input, size_t length, MULTIBYTE_CHARACTER_TYPE mbc_type)
{
    DWORD flags = 0;
    int wc_length1 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, NULL, 0);
    if (!wc_length1) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, wc_length1);
    int wc_length2 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, buffer, wc_length1);
    if (!wc_length2) {
        RC_HEAP_CALL(free_string, buffer);
        return create_invalid_text();
    }
    if (wc_length1 != wc_length2) {
        RC_HEAP_CALL(free_string, buffer);
        return create_invalid_text();
    }

    return wrap_text_with_length(buffer, wc_length2, true);
}


#endif // UNICODE
