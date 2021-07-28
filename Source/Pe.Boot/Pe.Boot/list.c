#include "list.h"
#include "memory.h"
#include "debug.h"

/// <summary>
/// 指定型のバイト幅を取得。
/// </summary>
/// <param name="list_type"></param>
/// <returns></returns>
static size_t get_type_byte(PRIMITIVE_LIST_TYPE list_type)
{
    switch (list_type) {
        case PRIMITIVE_LIST_TYPE_UINT8:
            return 1;
        case PRIMITIVE_LIST_TYPE_UINT16:
            return 2;
        case PRIMITIVE_LIST_TYPE_UINT32:
            return 4;
        default:
            assert_debug(false);
    }

    return 0;
}

/// <summary>
/// 指定型の長さをバイト単位で取得。
/// </summary>
/// <param name="list_type"></param>
/// <param name="length"></param>
/// <returns></returns>
static size_t get_type_bytes(PRIMITIVE_LIST_TYPE list_type, size_t length)
{
    if (!length) {
        return 0;
    }
    return get_type_byte(list_type) * length;
}

PRIMITIVE_LIST RC_HEAP_FUNC(new_primitive_list, PRIMITIVE_LIST_TYPE list_type, size_t capacity)
{
    size_t capacity_bytes = get_type_bytes(list_type, capacity);
    void* buffer = RC_HEAP_CALL(allocate_memory, capacity_bytes, false);

    PRIMITIVE_LIST result = {
        .buffer = buffer,
        .length = 0,
        .library = {
            .capacity_bytes = capacity_bytes,
            .type = list_type,
        },
    };

    return result;
}

bool RC_HEAP_FUNC(free_primitive_list, PRIMITIVE_LIST* list)
{
    if (!list) {
        return false;
    }
    if (!list->buffer) {
        return false;
    }

    return RC_HEAP_CALL(free_memory, list->buffer);
}

// 同じような処理ばっか書いてんね
static size_t extend_capacity_if_not_enough_list(PRIMITIVE_LIST* list, size_t need_length)
{
    size_t need_bytes = get_type_bytes(list->library.type, need_length);
    size_t current_bytes = get_type_bytes(list->library.type, list->length);
    // まだ大丈夫なら何もしない
    size_t need_total_bytes = current_bytes + need_bytes;
    if (need_total_bytes <= list->library.capacity_bytes) {
        return 0;
    }

    size_t old_capacity_bytes = list->library.capacity_bytes;
    size_t new_capacity_bytes = list->library.capacity_bytes;
    do {
        new_capacity_bytes *= 2;
    } while (new_capacity_bytes < need_total_bytes);

    void* new_buffer = allocate_memory(new_capacity_bytes, false);
    void* old_buffer = list->buffer;

    copy_memory(new_buffer, old_buffer, new_capacity_bytes);
    free_memory(old_buffer);

    list->buffer = new_buffer;
    list->library.capacity_bytes = new_capacity_bytes;

    return new_capacity_bytes - old_capacity_bytes;
}

bool push_list_uint8(PRIMITIVE_LIST_UINT8* list, uint8_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    uint8_t* buffer = (uint8_t*)list->buffer;
    buffer[list->length++] = value;

    return true;
}
bool push_list_uint16(PRIMITIVE_LIST_UINT16* list, uint16_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    uint16_t* buffer = (uint16_t*)list->buffer;
    buffer[list->length++] = value;

    return true;
}
bool push_list_uint32(PRIMITIVE_LIST_UINT32* list, uint32_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    uint32_t* buffer = (uint32_t*)list->buffer;
    buffer[list->length++] = value;

    return true;
}

bool pop_list_uint8(uint8_t* result, PRIMITIVE_LIST_UINT8* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        uint8_t* buffer = (uint8_t*)list->buffer;
        *result = buffer[list->length--];
    } else {
        list->length -= 1;
    }

    return true;
}
bool pop_list_uint16(uint16_t* result, PRIMITIVE_LIST_UINT16* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        uint16_t* buffer = (uint16_t*)list->buffer;
        *result = buffer[list->length--];
    } else {
        list->length -= 1;
    }

    return true;
}
bool pop_list_uint32(uint32_t* result, PRIMITIVE_LIST_UINT32* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        uint32_t* buffer = (uint32_t*)list->buffer;
        *result = buffer[--list->length];
    } else {
        list->length -= 1;
    }

    return true;
}

bool get_list_uint8(uint8_t* result, PRIMITIVE_LIST_UINT8* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return false;
    }

    if(index < list->length) {
        uint8_t* buffer = (uint8_t*)list->buffer;
        *result = buffer[index];
        return true;
    }

    return false;
}
bool get_list_uint16(uint16_t* result, PRIMITIVE_LIST_UINT16* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return false;
    }

    if(index < list->length) {
        uint16_t* buffer = (uint16_t*)list->buffer;
        *result = buffer[index];
        return true;
    }

    return false;
}
bool get_list_uint32(uint32_t* result, PRIMITIVE_LIST_UINT32* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return false;
    }

    if(index < list->length) {
        uint32_t* buffer = (uint32_t*)list->buffer;
        *result = buffer[index];
        return true;
    }

    return false;
}

uint8_t* reference_list_uint8(PRIMITIVE_LIST_UINT8* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return NULL;
    }

    return (uint8_t*)list->buffer;
}
uint16_t* reference_list_uint16(PRIMITIVE_LIST_UINT16* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return NULL;
    }

    return (uint16_t*)list->buffer;
}
uint32_t* reference_list_uint32(PRIMITIVE_LIST_UINT32* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return NULL;
    }

    return (uint32_t*)list->buffer;
}

