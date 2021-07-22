#include <windows.h>

#include "common.h"
#include "memory.h"


void* RC_HEAP_FUNC(allocate_memory, size_t bytes, bool zero_fill)
{
    HANDLE hHeap = GetProcessHeap();
    if (!hHeap) {
        return NULL;
    }

    void* heap = HeapAlloc(hHeap, zero_fill ? HEAP_ZERO_MEMORY : 0, bytes);
    if (!heap) {
        return NULL;
    }

#ifdef RES_CHECK
    rc__heap_check(heap, true, RES_CHECK_CALL_ARGS);
#endif

    return heap;
}

void* RC_HEAP_FUNC(allocate_clear_memory, size_t count, size_t type_size)
{
    return RC_HEAP_CALL(allocate_memory, count * type_size, true);
}

void RC_HEAP_FUNC(free_memory, void* p)
{
#ifdef RES_CHECK
    rc__heap_check(p, false, RES_CHECK_CALL_ARGS);
#endif

    HeapFree(GetProcessHeap(), 0, p);
}

void* set_memory(void* target, uint8_t value, size_t bytes)
{
    //NOTE: CRT!
    return FillMemory(target, bytes, value);
}

void* copy_memory(void* destination, const void* source, size_t bytes)
{
    //NOTE: CRT!
    return CopyMemory(destination, source, bytes);
}

void* move_memory(void* destination, const void* source, size_t bytes)
{
    //NOTE: CRT!
    return MoveMemory(destination, source, bytes);
}

int compare_memory(const void* a, const void* b, size_t bytes)
{
    //NOTE: CRT!
    return memcmp(a, b, bytes);
}
