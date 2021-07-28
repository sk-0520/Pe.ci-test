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
        case PRIMITIVE_LIST_TYPE_INT8:
            return sizeof(int8_t);
        case PRIMITIVE_LIST_TYPE_UINT8:
            return sizeof(uint8_t);
        case PRIMITIVE_LIST_TYPE_INT16:
            return sizeof(int16_t);
        case PRIMITIVE_LIST_TYPE_UINT16:
            return sizeof(uint16_t);
        case PRIMITIVE_LIST_TYPE_INT32:
            return sizeof(int32_t);
        case PRIMITIVE_LIST_TYPE_UINT32:
            return sizeof(uint32_t);
        case PRIMITIVE_LIST_TYPE_TCHAR:
            return sizeof(TCHAR);
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

bool push_list_int8(PRIMITIVE_LIST_INT8* list, int8_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    int8_t* buffer = (int8_t*)list->buffer;
    buffer[list->length++] = value;

    return true;
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
bool push_list_int16(PRIMITIVE_LIST_INT16* list, int16_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    int16_t* buffer = (int16_t*)list->buffer;
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
bool push_list_int32(PRIMITIVE_LIST_INT32* list, int32_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    int32_t* buffer = (int32_t*)list->buffer;
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
bool push_list_tchar(PRIMITIVE_LIST_TCHAR* list, TCHAR value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    TCHAR* buffer = (TCHAR*)list->buffer;
    buffer[list->length++] = value;

    return true;
}

bool add_range_list_int8(PRIMITIVE_LIST_INT8* list, const int8_t* values, size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return false;
    }

    if(!values) {
        return false;
    }
    if(!count) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, count);

    size_t data_bytes = get_type_bytes(list->library.type, count);
    copy_memory((int8_t*)list->buffer + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_uint8(PRIMITIVE_LIST_UINT8* list, const uint8_t* values, size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return false;
    }

    if(!values) {
        return false;
    }
    if(!count) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, count);

    size_t data_bytes = get_type_bytes(list->library.type, count);
    copy_memory((uint8_t*)list->buffer + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_int16(PRIMITIVE_LIST_INT16* list, const int16_t* values, size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return false;
    }

    if(!values) {
        return false;
    }
    if(!count) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, count);

    size_t data_bytes = get_type_bytes(list->library.type, count);
    copy_memory((int16_t*)list->buffer + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_uint16(PRIMITIVE_LIST_UINT16* list, const uint16_t* values, size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return false;
    }

    if(!values) {
        return false;
    }
    if(!count) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, count);

    size_t data_bytes = get_type_bytes(list->library.type, count);
    copy_memory((uint16_t*)list->buffer + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_int32(PRIMITIVE_LIST_INT32* list, const int32_t* values, size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return false;
    }

    if(!values) {
        return false;
    }
    if(!count) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, count);

    size_t data_bytes = get_type_bytes(list->library.type, count);
    copy_memory((int32_t*)list->buffer + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_uint32(PRIMITIVE_LIST_UINT32* list, const uint32_t* values, size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return false;
    }

    if(!values) {
        return false;
    }
    if(!count) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, count);

    size_t data_bytes = get_type_bytes(list->library.type, count);
    copy_memory((uint32_t*)list->buffer + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_tchar(PRIMITIVE_LIST_TCHAR* list, const TCHAR* values, size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return false;
    }

    if(!values) {
        return false;
    }
    if(!count) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, count);

    size_t data_bytes = get_type_bytes(list->library.type, count);
    copy_memory((TCHAR*)list->buffer + list->length, values, data_bytes);

    list->length += count;

    return true;
}

bool pop_list_int8(int8_t* result, PRIMITIVE_LIST_INT8* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        int8_t* buffer = (int8_t*)list->buffer;
        *result = buffer[--list->length];
    } else {
        list->length -= 1;
    }

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
        *result = buffer[--list->length];
    } else {
        list->length -= 1;
    }

    return true;
}
bool pop_list_int16(int16_t* result, PRIMITIVE_LIST_INT16* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        int16_t* buffer = (int16_t*)list->buffer;
        *result = buffer[--list->length];
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
        *result = buffer[--list->length];
    } else {
        list->length -= 1;
    }

    return true;
}
bool pop_list_int32(int32_t* result, PRIMITIVE_LIST_INT32* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        int32_t* buffer = (int32_t*)list->buffer;
        *result = buffer[--list->length];
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
bool pop_list_tchar(TCHAR* result, PRIMITIVE_LIST_TCHAR* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        TCHAR* buffer = (TCHAR*)list->buffer;
        *result = buffer[--list->length];
    } else {
        list->length -= 1;
    }

    return true;
}

bool get_list_int8(int8_t* result, PRIMITIVE_LIST_INT8* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return false;
    }

    if(index < list->length) {
        int8_t* buffer = (int8_t*)list->buffer;
        *result = buffer[index];
        return true;
    }

    return false;
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
bool get_list_int16(int16_t* result, PRIMITIVE_LIST_INT16* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return false;
    }

    if(index < list->length) {
        int16_t* buffer = (int16_t*)list->buffer;
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
bool get_list_int32(int32_t* result, PRIMITIVE_LIST_INT32* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return false;
    }

    if(index < list->length) {
        int32_t* buffer = (int32_t*)list->buffer;
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
bool get_list_tchar(TCHAR* result, PRIMITIVE_LIST_TCHAR* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return false;
    }

    if(index < list->length) {
        TCHAR* buffer = (TCHAR*)list->buffer;
        *result = buffer[index];
        return true;
    }

    return false;
}

const int8_t* reference_list_int8(PRIMITIVE_LIST_INT8* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return NULL;
    }

    return (int8_t*)list->buffer;
}
const uint8_t* reference_list_uint8(PRIMITIVE_LIST_UINT8* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return NULL;
    }

    return (uint8_t*)list->buffer;
}
const int16_t* reference_list_int16(PRIMITIVE_LIST_INT16* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return NULL;
    }

    return (int16_t*)list->buffer;
}
const uint16_t* reference_list_uint16(PRIMITIVE_LIST_UINT16* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return NULL;
    }

    return (uint16_t*)list->buffer;
}
const int32_t* reference_list_int32(PRIMITIVE_LIST_INT32* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return NULL;
    }

    return (int32_t*)list->buffer;
}
const uint32_t* reference_list_uint32(PRIMITIVE_LIST_UINT32* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return NULL;
    }

    return (uint32_t*)list->buffer;
}
const TCHAR* reference_list_tchar(PRIMITIVE_LIST_TCHAR* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return NULL;
    }

    return (TCHAR*)list->buffer;
}

