#include <shlwapi.h>

#include "debug.h"
#include "res_check.h"
#include "tstring.h"

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

TCHAR* RC_HEAP_FUNC(clone_string, const TCHAR* source)
{
    if (!source) {
        return NULL;
    }

    size_t length = get_string_length(source);
    size_t bytes = (length * sizeof(TCHAR)) + sizeof(TCHAR);
    TCHAR* result = (TCHAR*)RC_HEAP_CALL(allocate_memory, bytes, false);
    copy_memory(result, (void*)source, length * sizeof(TCHAR));
    result[length] = 0;

    return result;
}

TCHAR* RC_HEAP_FUNC(allocate_string, size_t length)
{
    size_t bytes = sizeof(TCHAR) * length + sizeof(TCHAR);
    TCHAR* result = RC_HEAP_CALL(allocate_memory, bytes, false);
    result[0] = 0;
    return result;
}

void RC_HEAP_FUNC(free_string, const TCHAR* s)
{
    RC_HEAP_CALL(free_memory, (void*)s);
}



