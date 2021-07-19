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

#ifdef MEM_CHECK
TCHAR* mem_check__clone_string(const TCHAR* source, MEM_CHECK_HEAD_ARGS)
#else
TCHAR* clone_string(const TCHAR* source)
#endif
{
    if (!source) {
        return NULL;
    }

    size_t length = get_string_length(source);
#ifdef MEM_CHECK
    TCHAR* result = mem_check__allocate_memory((length * sizeof(TCHAR)) + sizeof(TCHAR), false, MEM_CHECK_CALL_ARGS);
#else
    TCHAR* result = allocate_memory((length * sizeof(TCHAR)) + sizeof(TCHAR), false);
#endif
    copy_memory(result, (void*)source, length * sizeof(TCHAR));
    result[length] = 0;

    return result;
}

#ifdef MEM_CHECK
TCHAR* mem_check__allocate_string(size_t length, MEM_CHECK_HEAD_ARGS)
#else
TCHAR* allocate_string(size_t length)
#endif
{
#ifdef MEM_CHECK
    TCHAR* result = mem_check__allocate_memory(sizeof(TCHAR) * length + sizeof(TCHAR), false, MEM_CHECK_CALL_ARGS);
#else
    TCHAR* result = allocate_memory(sizeof(TCHAR) * length + sizeof(TCHAR), false);
#endif
    result[0] = 0;
    return result;
}

#ifdef MEM_CHECK
void mem_check__free_string(const TCHAR* s, MEM_CHECK_HEAD_ARGS)
#else
void free_string(const TCHAR* s)
#endif
{
#ifdef MEM_CHECK
    mem_check__free_memory((void*)s, MEM_CHECK_CALL_ARGS);
#else
    free_memory((void*)s);
#endif
}


