#include <shlwapi.h>

#include "tstring.h"

const TCHAR* tstrstr(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStr(haystack, needle);
}

const TCHAR* tstrstri(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStrI(haystack, needle);
}
