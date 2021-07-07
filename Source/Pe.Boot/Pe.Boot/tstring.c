#include <shlwapi.h>

#include "tstring.h"

TCHAR* findString(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStr(haystack, needle);
}

TCHAR* findStringCase(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStrI(haystack, needle);
}

size_t getStringLength(const TCHAR* s)
{
    return lstrlen(s);
}

TCHAR* findCharacter(const TCHAR* haystack, TCHAR needle)
{
    while (*haystack != needle) {
        if (!*haystack) {
            return NULL;
        }
        haystack++;
    }

    return (TCHAR*)haystack;
}

TCHAR* concatString(TCHAR* target, const TCHAR* value)
{
    return lstrcat(target, value);
}

static bool tryParseIntegerCore(const TCHAR* input, bool hex, int* result)
{
    return StrToIntEx(input, hex ? STIF_SUPPORT_HEX: STIF_DEFAULT, result);
}

bool tryParseInteger(const TCHAR* input, int* result)
{
    return tryParseIntegerCore(input, false, result);
}

bool tryParseHexOrInteger(const TCHAR* input, int* result)
{
    return tryParseIntegerCore(input, true, result);
}

static bool tryParseLongCore(const TCHAR* input, bool hex, long long* result)
{
    return StrToInt64Ex(input, hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, result);
}

bool tryParseLong(const TCHAR* input, long long* result)
{
    return tryParseLongCore(input, false, result);
}

bool tryParseHexOrLong(const TCHAR* input, long long* result)
{
    return tryParseLongCore(input, true, result);
}

