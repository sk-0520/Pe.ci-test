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
    /*
    unsigned char* p = (unsigned char*)target;
    const unsigned char v = (unsigned char)value;

    while (bytes--) {
        *p++ = v;
    }

    return target;
    */
    //NOTE: CRT!
    return FillMemory(target, bytes, value);
}

void* copyMemory(void* destination, void* source, size_t bytes)
{
    //NOTE: CRT!
    return CopyMemory(destination, source, bytes);
}

void* moveMemory(void* destination, void* source, size_t bytes)
{
    //NOTE: CRT!
    return MoveMemory(destination, source, bytes);
}
