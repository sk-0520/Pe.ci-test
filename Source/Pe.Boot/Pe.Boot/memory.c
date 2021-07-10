#include <windows.h>

#include "memory.h"

void* allocateMemory(size_t bytes, bool zeroFill)
{
    HANDLE hHeap = GetProcessHeap();
    if (!hHeap) {
        return NULL;
    }

    void* heap = HeapAlloc(hHeap, zeroFill ? HEAP_ZERO_MEMORY : 0, bytes);
    return heap;
}

void* allocateClearMemory(size_t count, size_t typeSize)
{
    return allocateMemory(count * typeSize, true);
}

void freeMemory(void* p)
{
    HeapFree(GetProcessHeap(), 0, p);
}

void* setMemory(void* target, int value, size_t bytes)
{
    unsigned char* p = (unsigned char*)target;
#pragma warning(push)
#   pragma warning(disable : 4244)
    const unsigned char v = value;
#pragma warning(pop)

    while (bytes--) {
        *p++ = v;
    }

    return target;
}
