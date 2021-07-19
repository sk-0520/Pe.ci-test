#include <shlwapi.h>

#include "tstring.h"
#include "memory.h"

size_t get_string_length(const TCHAR* s)
{
    return lstrlen(s);
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


