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
