#include <assert.h>

#include <windows.h>
#include <shlwapi.h>

#include "text.h"


static TEXT_PARSED_INT32_RESULT createFailedIntegerParseResult()
{
    TEXT_PARSED_INT32_RESULT result = {
        .success = false,
    };

    return result;
}

static TEXT_PARSED_INT64_RESULT createFailedLongParseResult()
{
    TEXT_PARSED_INT64_RESULT result = {
        .success = false,
    };

    return result;
}

TEXT_PARSED_INT32_RESULT parseInteger(const TEXT* input, bool supportHex)
{
    if (!isEnabledText(input)) {
        return createFailedIntegerParseResult();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT32_RESULT result;
#pragma warning(pop)
    result.success = StrToIntEx(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;
}

TEXT_PARSED_INT64_RESULT parseLong(const TEXT* input, bool supportHex)
{
    if (!isEnabledText(input)) {
        return createFailedLongParseResult();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT64_RESULT result;
#pragma warning(pop)
    result.success = StrToInt64Ex(input->value, supportHex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;

}
