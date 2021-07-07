#include <shlwapi.h>

#include "tstring.h"

TCHAR* tstrstr(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStr(haystack, needle);
}

TCHAR* tstrstri(const TCHAR* haystack, const TCHAR* needle)
{
    return StrStrI(haystack, needle);
}

size_t tstrlen(const TCHAR* s)
{
    return lstrlen(s);
}

TCHAR* tstrchr(const TCHAR* haystack, TCHAR needle)
{
    while (*haystack != needle) {
        if (!*haystack) {
            return NULL;
        }
        haystack++;
    }

    return (TCHAR*)haystack;
}
