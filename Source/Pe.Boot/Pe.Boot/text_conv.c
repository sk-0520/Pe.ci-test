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

TEXT_PARSED_INT32_RESULT parse_integer_from_text(const TEXT* input, bool supportHex)
{
    if (!is_enabled_text(input)) {
        return create_failed_integer_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT32_RESULT result;
#pragma warning(pop)
    result.success = StrToIntEx(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;
}

TEXT_PARSED_INT64_RESULT parse_long_from_text(const TEXT* input, bool supportHex)
{
    if (!is_enabled_text(input)) {
        return create_failed_long_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT64_RESULT result;
#pragma warning(pop)
    result.success = StrToInt64Ex(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;

}
