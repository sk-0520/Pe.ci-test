#include <windows.h>

#include "common.h"
#include "memory.h"

MEMORY_RESOURCE create_invalid_memory_manager()
{
    MEMORY_RESOURCE result = {
        .hHeap = NULL,
        .maximum_size = 0,
    };

    return result;
}

MEMORY_RESOURCE create_memory_resource(byte_t initial_size, byte_t maximum_size)
{
    if (initial_size > maximum_size) {
        return create_invalid_memory_manager();
    }

    if (maximum_size) {
#if _WIN64
        size_t max_size_limit = 1024 * 1024;
#else
        size_t max_size_limit = 512 * 1024;
#endif
        if (max_size_limit < maximum_size) {
            return create_invalid_memory_manager();
        }
    }

    HANDLE hHeap = HeapCreate(0, initial_size, maximum_size);

    MEMORY_RESOURCE result = {
        .hHeap = hHeap,
        .maximum_size = 0,
    };

    return result;
}

bool release_memory_resource(MEMORY_RESOURCE* memory_resource)
{
    if (!is_enabled_memory_resource(memory_resource)) {
        return false;
    }

    bool success = HeapDestroy(memory_resource->hHeap);
    if (!success) {
        return false;
    }

    memory_resource->hHeap = NULL;

    return true;
}

bool is_enabled_memory_resource(const MEMORY_RESOURCE* memory_resource)
{
    if (!memory_resource) {
        return false;
    }

    return memory_resource->hHeap;
}


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

byte_t library__extend_capacity_if_not_enough_bytes(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, library__func_calc_extend_capacity calc_extend_capacity)
{
    // まだ大丈夫なら何もしない
    byte_t need_total_bytes = current_bytes + need_bytes;
    if (need_total_bytes <= current_capacity_bytes) {
        return 0;
    }

    //byte_t old_capacity_bytes = current_capacity_bytes;
    byte_t new_capacity_bytes = current_capacity_bytes ? current_capacity_bytes : default_capacity_bytes;
    do {
        new_capacity_bytes = calc_extend_capacity(new_capacity_bytes);
    } while (new_capacity_bytes < need_total_bytes);

    void* new_buffer = allocate_memory(new_capacity_bytes, false);
    void* old_buffer = *target;

    copy_memory(new_buffer, old_buffer, new_capacity_bytes);
    free_memory(old_buffer);

    *target = new_buffer;

    //return new_capacity_bytes - old_capacity_bytes;
    return new_capacity_bytes;
}

static byte_t extend_x2(byte_t input_bytes)
{
    return input_bytes * 2;
}

byte_t library__extend_capacity_if_not_enough_bytes_x2(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes)
{
    return library__extend_capacity_if_not_enough_bytes(target, current_bytes, current_capacity_bytes, need_bytes, default_capacity_bytes, extend_x2);
}
