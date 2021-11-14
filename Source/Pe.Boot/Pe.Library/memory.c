#include <windows.h>

#include "common.h"
#include "memory.h"

static MEMORY_RESOURCE library__default_memory_resource = {
    .hHeap = NULL,
    .maximum_size = 0,
};

static MEMORY_RESOURCE create_invalid_memory_resource()
{
    MEMORY_RESOURCE result = {
        .hHeap = NULL,
        .maximum_size = 0,
    };

    return result;
}

MEMORY_RESOURCE* get_default_memory_resource()
{
    if (!library__default_memory_resource.hHeap) {
        library__default_memory_resource.hHeap = GetProcessHeap();
    }

    return &library__default_memory_resource;
}

MEMORY_RESOURCE create_memory_resource(byte_t initial_size, byte_t maximum_size)
{
    if (initial_size > maximum_size) {
        return create_invalid_memory_resource();
    }

    if (maximum_size) {
#if _WIN64
        size_t max_size_limit = 1024 * 1024;
#else
        size_t max_size_limit = 512 * 1024;
#endif
        if (max_size_limit < maximum_size) {
            return create_invalid_memory_resource();
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

void* allocate_raw_memory_from_memory_resource(byte_t bytes, bool zero_fill, const MEMORY_RESOURCE* memory_resource)
{
    if (!is_enabled_memory_resource(memory_resource)) {
        return NULL;
    }

    void* heap = HeapAlloc(memory_resource->hHeap, zero_fill ? HEAP_ZERO_MEMORY : 0, bytes);
    if (!heap) {
        return NULL;
    }

    return heap;
}

void* RC_HEAP_FUNC(allocate_raw_memory, byte_t bytes, bool zero_fill)
{
    initialize_default_memory_resource_if_uninitialized();

    void* heap = allocate_raw_memory_from_memory_resource(bytes, zero_fill, &library__default_memory_resource);
    if (!heap) {
        return NULL;
    }

#ifdef RES_CHECK
    rc__heap_check(heap, true, RES_CHECK_CALL_ARGS);
#endif

    return heap;
}

void* allocate_memory_from_memory_resource(size_t count, byte_t type_size, const MEMORY_RESOURCE* memory_resource)
{
    return allocate_raw_memory_from_memory_resource(count * type_size, true, memory_resource);
}

void* RC_HEAP_FUNC(allocate_memory, size_t count, byte_t type_size)
{
    return RC_HEAP_CALL(allocate_raw_memory, count * type_size, true);
}

bool release_memory_from_memory_resource(void* p, const MEMORY_RESOURCE* memory_resource)
{
    if (!p) {
        return false;
    }

    return HeapFree(memory_resource->hHeap, 0, p);
}

bool RC_HEAP_FUNC(free_memory, void* p)
{
    //initialize_default_memory_resource_if_uninitialized();

    bool result = release_memory_from_memory_resource(p, &library__default_memory_resource);
    if (!result) {
        return false;
    }

#ifdef RES_CHECK
    rc__heap_check(p, false, RES_CHECK_CALL_ARGS);
#endif

    return result;
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

byte_t library__extend_capacity_if_not_enough_bytes_from_memory_resource(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, func_calc_extend_capacity calc_extend_capacity, const MEMORY_RESOURCE* memory_resource)
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

    void* new_buffer = allocate_raw_memory(new_capacity_bytes, false);
    void* old_buffer = *target;

    copy_memory(new_buffer, old_buffer, new_capacity_bytes);
    free_memory(old_buffer);

    *target = new_buffer;

    //return new_capacity_bytes - old_capacity_bytes;
    return new_capacity_bytes;
}

byte_t library__extend_capacity_if_not_enough_bytes(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, func_calc_extend_capacity calc_extend_capacity)
{
    initialize_default_memory_resource_if_uninitialized();

    return library__extend_capacity_if_not_enough_bytes_from_memory_resource(target, current_bytes, current_capacity_bytes, need_bytes, default_capacity_bytes, calc_extend_capacity, &library__default_memory_resource);
}


static byte_t extend_x2(byte_t input_bytes)
{
    return input_bytes * 2;
}

byte_t library__extend_capacity_if_not_enough_bytes_x2_from_memory_resource(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, const MEMORY_RESOURCE* memory_resource)
{
    return library__extend_capacity_if_not_enough_bytes_from_memory_resource(target, current_bytes, current_capacity_bytes, need_bytes, default_capacity_bytes, extend_x2, memory_resource);
}

byte_t library__extend_capacity_if_not_enough_bytes_x2(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes)
{
    initialize_default_memory_resource_if_uninitialized();

    return library__extend_capacity_if_not_enough_bytes_from_memory_resource(target, current_bytes, current_capacity_bytes, need_bytes, default_capacity_bytes, extend_x2, &library__default_memory_resource);
}
