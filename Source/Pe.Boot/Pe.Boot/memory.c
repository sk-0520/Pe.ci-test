#include <windows.h>

#include "common.h"
#include "memory.h"


#ifdef MEM_CHECK
void* mem_check__allocate_memory(size_t bytes, bool zero_fill, MEM_CHECK_FUNC_ARGS)
#else
void* allocate_memory(size_t bytes, bool zero_fill)
#endif
{
    HANDLE hHeap = GetProcessHeap();
    if (!hHeap) {
        return NULL;
    }

    void* heap = HeapAlloc(hHeap, zero_fill ? HEAP_ZERO_MEMORY : 0, bytes);
#ifdef MEM_CHECK
    mem_check__debugHeap(heap, true, caller_file, caller_line);
#endif
    return heap;
}

#ifdef MEM_CHECK
void* mem_check__allocate_clear_memory(size_t count, size_t type_size, MEM_CHECK_FUNC_ARGS)
{
    return mem_check__allocate_memory(count * type_size, true, MEM_CHECK_CALL_ARGS);
}
#else
void* allocate_clear_memory(size_t count, size_t type_size)
{
    return allocate_memory(count * type_size, true);
}
#endif

#ifdef MEM_CHECK
void mem_check__free_memory(void* p, MEM_CHECK_FUNC_ARGS)
#else
void free_memory(void* p)
#endif
{
#ifdef MEM_CHECK
    mem_check__debugHeap(p, false, MEM_CHECK_CALL_ARGS);
#endif

    HeapFree(GetProcessHeap(), 0, p);
}

void* set_memory(void* target, unsigned char value, size_t bytes)
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
