#include <shlwapi.h>

#include "tstring.h"

TCHAR* tstrstr(const TCHAR* s1, const TCHAR* s2)
{
    return StrStr(s1, s2);
}

TCHAR* tstrstri(const TCHAR* s1, const TCHAR* s2)
{
    return StrStrI(s1, s2);
}
