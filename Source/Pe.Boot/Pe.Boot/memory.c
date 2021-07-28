#include <windows.h>

#include "common.h"
#include "memory.h"


void* RC_HEAP_FUNC(allocate_memory, byte_t bytes, bool zero_fill)
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

bool RC_HEAP_FUNC(free_memory, void* p)
{
    if (!p) {
        return false;
    }
#ifdef RES_CHECK
    rc__heap_check(p, false, RES_CHECK_CALL_ARGS);
#endif

    return HeapFree(GetProcessHeap(), 0, p);
}

void* set_memory(void* target, uint8_t value, byte_t bytes)
{
    return FillMemory(target, bytes, value);
}

void* copy_memory(void* destination, const void* source, byte_t bytes)
{
    return CopyMemory(destination, source, bytes);
}

void* move_memory(void* destination, const void* source, byte_t bytes)
{
    return MoveMemory(destination, source, bytes);
}

int compare_memory(const void* a, const void* b, byte_t bytes)
{
    return memcmp(a, b, bytes);
}
