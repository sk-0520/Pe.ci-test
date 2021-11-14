#include "object_list.h"
#include "memory.h"
#include "debug.h"


int compare_object_list_value_null(const void* a, const void* b)
{
    return -1;
}

void free_object_list_value_null(void* value)
{
}

OBJECT_LIST RC_HEAP_FUNC(new_object_list, byte_t item_size, size_t capacity_count, func_compare_object_list_value compare_object_list_value, func_free_object_list_value free_object_list_value, const MEMORY_RESOURCE* memory_resource)
{
    assert(item_size);

    OBJECT_LIST result = {
        .length = 0,
        .items = RC_HEAP_CALL(allocate_raw_memory, capacity_count * item_size, false, DEFAULT_MEMORY),
        .library = {
            .memory_resource = memory_resource,
            .item_size = item_size,
            .capacity = capacity_count + item_size,
            .compare_object_list_value = compare_object_list_value,
            .free_object_list_value = free_object_list_value
    },
    };

    return result;
}

static void free_object_list_item(OBJECT_LIST* object_list, void* item)
{
    if (item) {
        object_list->library.free_object_list_value(item);
    }
}

bool RC_HEAP_FUNC(free_object_list, OBJECT_LIST* object_list)
{
    if (!object_list) {
        return false;
    }

    for (size_t i = 0; i < object_list->length; i++) {
        free_object_list_item(object_list, object_list->items + (i * object_list->library.item_size));
    }

    RC_HEAP_CALL(free_memory, object_list->items, DEFAULT_MEMORY);

    object_list->items = NULL;
    object_list->length = 0;
    object_list->library.capacity = 0;
    object_list->library.free_object_list_value = NULL;

    return true;
}

static void extend_capacity_if_not_enough_object_list(OBJECT_LIST* object_list, size_t need_length)
{
    byte_t need_bytes = need_length * object_list->library.item_size;
    byte_t current_bytes = object_list->length * object_list->library.item_size;
    byte_t default_capacity_bytes = OBJECT_LIST_DEFAULT_CAPACITY_COUNT * object_list->library.item_size;

    byte_t extend_total_byte = library__extend_capacity_if_not_enough_bytes_x2(&object_list->items, current_bytes, object_list->library.capacity * object_list->library.item_size, need_bytes, default_capacity_bytes, DEFAULT_MEMORY);
    if (extend_total_byte) {
        object_list->library.capacity = extend_total_byte / object_list->library.item_size;
    }
}

void* push_object_list(OBJECT_LIST* object_list, const void* value)
{
    if (!object_list) {
        return NULL;
    }

    extend_capacity_if_not_enough_object_list(object_list, 1);

    copy_memory(object_list->items + (object_list->length * object_list->library.item_size), value, object_list->library.item_size);
    object_list->length += 1;

    return object_list->items + ((object_list->length - 1) * object_list->library.item_size);
}

bool add_range_object_list(OBJECT_LIST* object_list, const void* values, size_t length)
{
    if (!object_list) {
        return false;
    }
    if (!values) {
        return false;
    }
    if (!length) {
        return false;
    }

    extend_capacity_if_not_enough_object_list(object_list, length);

    copy_memory(object_list->items + (object_list->length * object_list->library.item_size), values, object_list->library.item_size * length);
    object_list->length += length;

    return true;
}

bool pop_object_list(void* result, OBJECT_LIST* object_list)
{
    if (!object_list) {
        return false;
    }

    if (!object_list->length) {
        return false;
    }

    copy_memory(result, object_list->items + (--object_list->length * object_list->library.item_size), object_list->library.item_size);

    return true;
}

void* peek_object_list(OBJECT_LIST* object_list)
{
    if (!object_list) {
        return NULL;
    }

    if (!object_list->length) {
        return NULL;
    }

    return object_list->items + ((object_list->length - 1) * object_list->library.item_size);
}


OBJECT_RESULT_VALUE get_object_list(const OBJECT_LIST* object_list, size_t index)
{
    if (index < object_list->length) {
        OBJECT_RESULT_VALUE result = {
            .value = object_list->items + (index * object_list->library.item_size),
            .exists = true,
        };

        return result;
    }

    OBJECT_RESULT_VALUE none = {
        .value = NULL,
        .exists = false,
    };
    return none;
}

bool set_object_list(OBJECT_LIST* object_list, size_t index, void* value, bool need_release)
{
    if (!object_list) {
        return false;
    }

    if (index < object_list->length) {
        void* current = object_list->items + index * object_list->library.item_size;
        if (need_release) {
            object_list->library.free_object_list_value(current);
        }
        copy_memory(current, value, object_list->library.item_size);
        return true;
    }

    return false;
}

void clear_object_list(OBJECT_LIST* object_list)
{
    for (size_t i = 0; i < object_list->length; i++) {
        object_list->library.free_object_list_value(object_list->items + (i * object_list->library.item_size));
    }
    object_list->length = 0;
}

size_t foreach_object_list(const OBJECT_LIST* object_list, func_foreach_object_list func, void* data)
{
    assert(object_list);
    assert(func);

    size_t result = 0;
    for (size_t i = 0; i < object_list->length; i++) {
        void* item = object_list->items + (i * object_list->library.item_size);
        if (!func(item, i, object_list->length, data)) {
            break;
        }
        result += 1;
    }

    return result;
}
