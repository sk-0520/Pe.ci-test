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

TCHAR* RC_HEAP_FUNC(clone_string, const TCHAR* source, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!source) {
        return NULL;
    }

    size_t length = get_string_length(source);
    size_t bytes = (length * sizeof(TCHAR)) + sizeof(TCHAR);
    TCHAR* result = (TCHAR*)RC_HEAP_CALL(allocate_raw_memory, bytes, false, memory_arena_resource);
    copy_memory(result, (void*)source, length * sizeof(TCHAR));
    result[length] = 0;

    return result;
}

TCHAR* RC_HEAP_FUNC(clone_string_with_length, const TCHAR* source, size_t length, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    TCHAR* buffer = RC_HEAP_CALL(allocate_string, length, memory_arena_resource);
    copy_memory(buffer, source, length * sizeof(TCHAR));
    buffer[length] = 0;

    return buffer;
}

TCHAR* RC_HEAP_FUNC(allocate_string, size_t length, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    size_t bytes = sizeof(TCHAR) * length + sizeof(TCHAR);
    TCHAR* result = RC_HEAP_CALL(allocate_raw_memory, bytes, true, memory_arena_resource);
    return result;
}

void RC_HEAP_FUNC(release_string, const TCHAR* s, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    RC_HEAP_CALL(release_memory, (void*)s, memory_arena_resource);
}

