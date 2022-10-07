/* 自動生成: primitive_list.gen.c.tt */
#include "primitive_list.gen.h"
#include "memory.h"
#include "debug.h"

/// <summary>
/// 指定型のバイト幅を取得。
/// </summary>
/// <param name="list_type"></param>
/// <returns></returns>
static byte_t get_type_byte(PRIMITIVE_LIST_TYPE list_type)
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
        case PRIMITIVE_LIST_TYPE_SIZE:
            return sizeof(size_t);
        case PRIMITIVE_LIST_TYPE_SSIZE:
            return sizeof(ssize_t);
        case PRIMITIVE_LIST_TYPE_TCHAR:
            return sizeof(TCHAR);
        default:
            assert(false);
    }

    return 0;
}

/// <summary>
/// 指定型の長さをバイト単位で取得。
/// </summary>
/// <param name="list_type"></param>
/// <param name="length"></param>
/// <returns></returns>
static byte_t get_type_bytes(PRIMITIVE_LIST_TYPE list_type, size_t length)
{
    if (!length) {
        return 0;
    }
    return get_type_byte(list_type) * length;
}

PRIMITIVE_LIST RC_HEAP_FUNC(new_primitive_list, PRIMITIVE_LIST_TYPE list_type, size_t capacity, const MEMORY_RESOURCE* memory_resource)
{
    assert(memory_resource);

    size_t capacity_bytes = get_type_bytes(list_type, capacity);
    void* items = RC_HEAP_CALL(allocate_raw_memory, capacity_bytes, false, memory_resource);

    PRIMITIVE_LIST result = {
        .items = items,
        .length = 0,
        .library = {
            .memory_resource = memory_resource,
            .capacity_bytes = capacity_bytes,
            .type = list_type,
        },
    };

    return result;
}

bool RC_HEAP_FUNC(release_primitive_list, PRIMITIVE_LIST* list)
{
    if (!list) {
        return false;
    }
    if (!list->items) {
        return false;
    }

    return RC_HEAP_CALL(release_memory, list->items, list->library.memory_resource);
}

static void extend_capacity_if_not_enough_list(PRIMITIVE_LIST* list, size_t need_length)
{
    byte_t need_bytes = get_type_bytes(list->library.type, need_length);
    byte_t current_bytes = get_type_bytes(list->library.type, list->length);
    byte_t default_capacity_bytes = get_type_bytes(list->library.type, PRIMITIVE_LIST_DEFAULT_CAPACITY);

    byte_t extend_total_byte = library__extend_capacity_if_not_enough_bytes_x2(&list->items, current_bytes, list->library.capacity_bytes, need_bytes, default_capacity_bytes, list->library.memory_resource);
    if (extend_total_byte) {
        list->library.capacity_bytes = extend_total_byte;
    }
}


bool push_list_int8(PRIMITIVE_LIST_INT8* list, int8_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    int8_t* items = (int8_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_uint8(PRIMITIVE_LIST_UINT8* list, uint8_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    uint8_t* items = (uint8_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_int16(PRIMITIVE_LIST_INT16* list, int16_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    int16_t* items = (int16_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_uint16(PRIMITIVE_LIST_UINT16* list, uint16_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    uint16_t* items = (uint16_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_int32(PRIMITIVE_LIST_INT32* list, int32_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    int32_t* items = (int32_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_uint32(PRIMITIVE_LIST_UINT32* list, uint32_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    uint32_t* items = (uint32_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_size(PRIMITIVE_LIST_SIZE* list, size_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SIZE) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    size_t* items = (size_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_ssize(PRIMITIVE_LIST_SSIZE* list, ssize_t value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SSIZE) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    ssize_t* items = (ssize_t*)list->items;
    items[list->length++] = value;

    return true;
}
bool push_list_tchar(PRIMITIVE_LIST_TCHAR* list, TCHAR value)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return false;
    }

    extend_capacity_if_not_enough_list(list, 1);

    TCHAR* items = (TCHAR*)list->items;
    items[list->length++] = value;

    return true;
}

bool add_range_list_int8(PRIMITIVE_LIST_INT8* list, const int8_t values[], size_t count)
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
    copy_memory((int8_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_uint8(PRIMITIVE_LIST_UINT8* list, const uint8_t values[], size_t count)
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
    copy_memory((uint8_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_int16(PRIMITIVE_LIST_INT16* list, const int16_t values[], size_t count)
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
    copy_memory((int16_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_uint16(PRIMITIVE_LIST_UINT16* list, const uint16_t values[], size_t count)
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
    copy_memory((uint16_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_int32(PRIMITIVE_LIST_INT32* list, const int32_t values[], size_t count)
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
    copy_memory((int32_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_uint32(PRIMITIVE_LIST_UINT32* list, const uint32_t values[], size_t count)
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
    copy_memory((uint32_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_size(PRIMITIVE_LIST_SIZE* list, const size_t values[], size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SIZE) {
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
    copy_memory((size_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_ssize(PRIMITIVE_LIST_SSIZE* list, const ssize_t values[], size_t count)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SSIZE) {
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
    copy_memory((ssize_t*)list->items + list->length, values, data_bytes);

    list->length += count;

    return true;
}
bool add_range_list_tchar(PRIMITIVE_LIST_TCHAR* list, const TCHAR values[], size_t count)
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
    copy_memory((TCHAR*)list->items + list->length, values, data_bytes);

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
        int8_t* items = (int8_t*)list->items;
        *result = items[--list->length];
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
        uint8_t* items = (uint8_t*)list->items;
        *result = items[--list->length];
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
        int16_t* items = (int16_t*)list->items;
        *result = items[--list->length];
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
        uint16_t* items = (uint16_t*)list->items;
        *result = items[--list->length];
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
        int32_t* items = (int32_t*)list->items;
        *result = items[--list->length];
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
        uint32_t* items = (uint32_t*)list->items;
        *result = items[--list->length];
    } else {
        list->length -= 1;
    }

    return true;
}
bool pop_list_size(size_t* result, PRIMITIVE_LIST_SIZE* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SIZE) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        size_t* items = (size_t*)list->items;
        *result = items[--list->length];
    } else {
        list->length -= 1;
    }

    return true;
}
bool pop_list_ssize(ssize_t* result, PRIMITIVE_LIST_SSIZE* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SSIZE) {
        return false;
    }

    if(!list->length) {
        return false;
    }

    if(result) {
        ssize_t* items = (ssize_t*)list->items;
        *result = items[--list->length];
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
        TCHAR* items = (TCHAR*)list->items;
        *result = items[--list->length];
    } else {
        list->length -= 1;
    }

    return true;
}

bool get_list_int8(int8_t* result, const PRIMITIVE_LIST_INT8* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return false;
    }

    if(index < list->length) {
        int8_t* items = (int8_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_uint8(uint8_t* result, const PRIMITIVE_LIST_UINT8* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return false;
    }

    if(index < list->length) {
        uint8_t* items = (uint8_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_int16(int16_t* result, const PRIMITIVE_LIST_INT16* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return false;
    }

    if(index < list->length) {
        int16_t* items = (int16_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_uint16(uint16_t* result, const PRIMITIVE_LIST_UINT16* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return false;
    }

    if(index < list->length) {
        uint16_t* items = (uint16_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_int32(int32_t* result, const PRIMITIVE_LIST_INT32* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return false;
    }

    if(index < list->length) {
        int32_t* items = (int32_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_uint32(uint32_t* result, const PRIMITIVE_LIST_UINT32* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return false;
    }

    if(index < list->length) {
        uint32_t* items = (uint32_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_size(size_t* result, const PRIMITIVE_LIST_SIZE* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SIZE) {
        return false;
    }

    if(index < list->length) {
        size_t* items = (size_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_ssize(ssize_t* result, const PRIMITIVE_LIST_SSIZE* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SSIZE) {
        return false;
    }

    if(index < list->length) {
        ssize_t* items = (ssize_t*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}
bool get_list_tchar(TCHAR* result, const PRIMITIVE_LIST_TCHAR* list, size_t index)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return false;
    }

    if(index < list->length) {
        TCHAR* items = (TCHAR*)list->items;
        *result = items[index];
        return true;
    }

    return false;
}

int8_t* reference_list_int8(const PRIMITIVE_LIST_INT8* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT8) {
        return NULL;
    }

    return (int8_t*)list->items;
}
uint8_t* reference_list_uint8(const PRIMITIVE_LIST_UINT8* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT8) {
        return NULL;
    }

    return (uint8_t*)list->items;
}
int16_t* reference_list_int16(const PRIMITIVE_LIST_INT16* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT16) {
        return NULL;
    }

    return (int16_t*)list->items;
}
uint16_t* reference_list_uint16(const PRIMITIVE_LIST_UINT16* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT16) {
        return NULL;
    }

    return (uint16_t*)list->items;
}
int32_t* reference_list_int32(const PRIMITIVE_LIST_INT32* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_INT32) {
        return NULL;
    }

    return (int32_t*)list->items;
}
uint32_t* reference_list_uint32(const PRIMITIVE_LIST_UINT32* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_UINT32) {
        return NULL;
    }

    return (uint32_t*)list->items;
}
size_t* reference_list_size(const PRIMITIVE_LIST_SIZE* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SIZE) {
        return NULL;
    }

    return (size_t*)list->items;
}
ssize_t* reference_list_ssize(const PRIMITIVE_LIST_SSIZE* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_SSIZE) {
        return NULL;
    }

    return (ssize_t*)list->items;
}
TCHAR* reference_list_tchar(const PRIMITIVE_LIST_TCHAR* list)
{
    if(list->library.type != PRIMITIVE_LIST_TYPE_TCHAR) {
        return NULL;
    }

    return (TCHAR*)list->items;
}

void clear_primitive_list(PRIMITIVE_LIST* list)
{
    list->length = 0;
}
