#include <windows.h>

#include "common.h"
#include "debug.h"
#include "memory.h"

static MEMORY_ARENA_RESOURCE library_default_memory_arena_resource = {
    .handle = NULL,
    .maximum_size = 0,
};

static MEMORY_ARENA_RESOURCE create_invalid_memory_arena_resource()
{
    MEMORY_ARENA_RESOURCE result = {
        .handle = NULL,
        .maximum_size = 0,
    };

    return result;
}

/// <summary>
/// メモリリソースがライブラリ管理の通常使用かどうかを判断。
/// </summary>
/// <param name="memory_arena_resource"></param>
/// <returns></returns>
static bool is_default_memory_arena_resource(const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    assert(memory_arena_resource);

    if (memory_arena_resource == &library_default_memory_arena_resource) {
        return true;
    }
    if (memory_arena_resource->handle == library_default_memory_arena_resource.handle) {
        return true;
    }

    return false;
}

MEMORY_ARENA_RESOURCE* get_default_memory_arena_resource()
{
    if (!library_default_memory_arena_resource.handle) {
        library_default_memory_arena_resource.handle = GetProcessHeap();
    }

    return &library_default_memory_arena_resource;
}

MEMORY_ARENA_RESOURCE new_memory_arena_resource(byte_t initial_size, byte_t maximum_size)
{
    if (initial_size > maximum_size) {
        return create_invalid_memory_arena_resource();
    }

    if (maximum_size) {
#if _WIN64
        size_t max_size_limit = 1024 * 1024;
#else
        size_t max_size_limit = 512 * 1024;
#endif
        if (max_size_limit < maximum_size) {
            return create_invalid_memory_arena_resource();
        }
    }

    HANDLE handle = HeapCreate(0, initial_size, maximum_size);

    MEMORY_ARENA_RESOURCE result = {
        .handle = handle,
        .maximum_size = 0,
    };

    return result;
}

bool release_memory_arena_resource(MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!is_enabled_memory_arena_resource(memory_arena_resource)) {
        return false;
    }

    bool success = HeapDestroy(memory_arena_resource->handle);
    if (!success) {
        return false;
    }

    memory_arena_resource->handle = NULL;

    return true;
}

bool is_enabled_memory_arena_resource(const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!memory_arena_resource) {
        return false;
    }

    return memory_arena_resource->handle;
}

void* RC_HEAP_FUNC(allocate_raw_memory, byte_t bytes, bool zero_fill, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!is_enabled_memory_arena_resource(memory_arena_resource)) {
        return NULL;
    }

    void* heap = HeapAlloc(memory_arena_resource->handle, zero_fill ? HEAP_ZERO_MEMORY : 0, bytes);
    if (!heap) {
        return NULL;
    }

#ifdef RES_CHECK
    if (is_default_memory_arena_resource(memory_arena_resource)) {
        library_rc_heap_check(heap, true, RES_CHECK_CALL_ARGS);
    }
#endif

    return heap;
}

void* RC_HEAP_FUNC(new_memory, size_t count, byte_t type_size, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    byte_t allocate_size = count * type_size;
    if (allocate_size / type_size != count) {
        return NULL;
    }

    return RC_HEAP_CALL(allocate_raw_memory, allocate_size, true, memory_arena_resource);
}

bool RC_HEAP_FUNC(release_memory, void* p, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!p) {
        return false;
    }

    bool result = HeapFree(memory_arena_resource->handle, 0, p);
    if (!result) {
        return false;
    }

#ifdef RES_CHECK
    if (is_default_memory_arena_resource(memory_arena_resource)) {
#pragma warning(push)
#pragma warning(disable:6001)
        library_rc_heap_check(p, false, RES_CHECK_CALL_ARGS);
#pragma warning(pop)
    }
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

byte_t library_extend_capacity_if_not_enough_bytes(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, func_calc_extend_capacity calc_extend_capacity, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    assert(memory_arena_resource);

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

    void* new_buffer = allocate_raw_memory(new_capacity_bytes, false, memory_arena_resource);
    void* old_buffer = *target;

    copy_memory(new_buffer, old_buffer, new_capacity_bytes);
    release_memory(old_buffer, memory_arena_resource);

    *target = new_buffer;

    //return new_capacity_bytes - old_capacity_bytes;
    return new_capacity_bytes;
}

static byte_t extend_x2(byte_t input_bytes)
{
    return input_bytes * 2;
}

byte_t library_extend_capacity_if_not_enough_bytes_x2(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    assert(memory_arena_resource);

    return library_extend_capacity_if_not_enough_bytes(target, current_bytes, current_capacity_bytes, need_bytes, default_capacity_bytes, extend_x2, memory_arena_resource);
}
