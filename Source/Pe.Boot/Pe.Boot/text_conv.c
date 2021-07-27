#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"


static TEXT_PARSED_INT32_RESULT create_failed_integer_parse_result()
{
    TEXT_PARSED_INT32_RESULT result = {
        .success = false,
    };

    return result;
}

static TEXT_PARSED_INT64_RESULT create_failed_long_parse_result()
{
    TEXT_PARSED_INT64_RESULT result = {
        .success = false,
    };

    return result;
}

TEXT_PARSED_INT32_RESULT parse_integer_from_text(const TEXT* input, bool support_hex)
{
    if (!is_enabled_text(input)) {
        return create_failed_integer_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT32_RESULT result;
#pragma warning(pop)
    result.success = StrToIntEx(input->value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;
}

TEXT_PARSED_INT64_RESULT parse_long_from_text(const TEXT* input, bool support_hex)
{
    if (!is_enabled_text(input)) {
        return create_failed_long_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT64_RESULT result;
#pragma warning(pop)
    result.success = StrToInt64Ex(input->value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;

}

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

MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE convert_type)
{
    MULTIBYTE_CHARACTER_RESULT result = { 0 };
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



#endif // UNICODE
