#include <shlwapi.h>

#include "tstring.h"
#include "memory.h"

TCHAR* findString(const TCHAR* haystack, const TCHAR* needle, bool ignoreCase)
{
    return ignoreCase
        ? StrStrI(haystack, needle)
        : StrStr(haystack, needle)
        ;
}

size_t get_string_length(const TCHAR* s)
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

SSIZE_T indexCharacter(const TCHAR* haystack, TCHAR needle)
{
    TCHAR* p = findCharacter(haystack, needle);
    if (!p) {
        return -1;
    }

    return p - haystack;
}

int compare_string(const TCHAR* a, const TCHAR* b, bool ignore_case)
{
    return ignore_case
        ? lstrcmpi(a, b)
        : lstrcmp(a, b)
        ;
}

static bool tryParseIntegerCore(int* result, const TCHAR* input, bool hex)
{
    return StrToIntEx(input, hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, result);
}

bool tryParseInteger(int* result, const TCHAR* input)
{
    return tryParseIntegerCore(result, input, false);
}

bool tryParseHexOrInteger(int* result, const TCHAR* input)
{
    return tryParseIntegerCore(result, input, true);
}

static bool tryParseLongCore(long long* result, const TCHAR* input, bool hex)
{
    return StrToInt64Ex(input, hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, result);
}

bool tryParseLong(long long* result, const TCHAR* input)
{
    return tryParseLongCore(result, input, false);
}

bool tryParseHexOrLong(long long* result, const TCHAR* input)
{
    return tryParseLongCore(result, input, true);
}

TCHAR* concat_string(TCHAR* target, const TCHAR* value)
{
    return lstrcat(target, value);
}

TCHAR* copy_string(TCHAR* result, const TCHAR* value)
{
    return lstrcpy(result, value);
}

TCHAR* clone_string(const TCHAR* source)
{
    if (!source) {
        return NULL;
    }

    size_t length = get_string_length(source);
    TCHAR* result = allocate_memory((length * sizeof(TCHAR)) + sizeof(TCHAR), false);
    copy_memory(result, (void*)source, length * sizeof(TCHAR));
    result[length] = 0;

    return result;
}

#ifdef MEM_CHECK
TCHAR* mem_check__allocate_string(size_t length, const TCHAR* callerFile, size_t callerLine)
#else
TCHAR* allocate_string(size_t length)
#endif
{
#ifdef MEM_CHECK
    TCHAR* result = mem_check__allocate_memory(sizeof(TCHAR) * length + sizeof(TCHAR), false, callerFile, callerLine);
#else
    TCHAR* result = allocate_memory(sizeof(TCHAR) * length + sizeof(TCHAR), false);
#endif
    result[0] = 0;
    return result;
}

#ifdef MEM_CHECK
void mem_check__free_string(const TCHAR* s, const TCHAR* callerFile, size_t callerLine)
#else
void free_string(const TCHAR* s)
#endif
{
#ifdef MEM_CHECK
    mem_check__free_memory((void*)s, callerFile, callerLine);
#else
    free_memory((void*)s);
#endif
}


