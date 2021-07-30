#include "object_list.h"
#include "memory.h"

int compare_object_list_value_null(const void* a, const void* b)
{
    return -1;
}

void free_object_list_value_null(void* value)
{
}

OBJECT_LIST RC_HEAP_FUNC(create_object_list, size_t capacity, func_compare_object_list_value compare_object_list_value, func_free_object_list_value free_object_list_value)
{
    OBJECT_LIST result = {
        .length = 0,
        .items = RC_HEAP_CALL(allocate_memory, sizeof(OBJECT_LIST_ITEM) * capacity, false),
        .library = {
            .capacity = capacity,
            .compare_object_list_value = compare_object_list_value,
            .free_object_list_value = free_object_list_value
        },
    };

    return result;
}

static void free_object_list_item(OBJECT_LIST* object_list, OBJECT_LIST_ITEM* item)
{
    if (item->library.need_release) {
        object_list->library.free_object_list_value(item->value);
    }
}

bool RC_HEAP_FUNC(free_object_list, OBJECT_LIST* object_list)
{
    if (!object_list) {
        return false;
    }

    for (size_t i = 0; i < object_list->length; i++) {
        free_object_list_item(object_list, object_list->items + i);
    }

    RC_HEAP_CALL(free_memory, object_list->items);

    object_list->items = NULL;
    object_list->length = 0;
    object_list->library.capacity = 0;
    object_list->library.free_object_list_value = NULL;

    return true;
}

static void extend_capacity_if_not_enough_object_list(OBJECT_LIST* object_list, size_t need_length)
{
    byte_t need_bytes = need_length * sizeof(OBJECT_LIST_ITEM);
    byte_t current_bytes = object_list->length * sizeof(OBJECT_LIST_ITEM);
    byte_t default_capacity_bytes = OBJECT_LIST_DEFAULT_CAPACITY * sizeof(OBJECT_LIST_ITEM);

    byte_t extend_total_byte = library__extend_capacity_if_not_enough_bytes_x2(&object_list->items, current_bytes, object_list->library.capacity * sizeof(OBJECT_LIST_ITEM), need_bytes, default_capacity_bytes);
    if (extend_total_byte) {
        object_list->library.capacity = extend_total_byte / sizeof(OBJECT_LIST_ITEM);
    }
}

OBJECT_LIST_ITEM* push_object_list(OBJECT_LIST* object_list, void* value, bool need_release)
{
    if (!object_list) {
        return NULL;
    }

    extend_capacity_if_not_enough_object_list(object_list, 1);

    OBJECT_LIST_ITEM item = {
        .value = value,
        .library = {
            .need_release = need_release,
    },
    };

    object_list->items[object_list->length] = item;
    object_list->length += 1;

    return object_list->items + object_list->length - 1;
}

bool add_range_object_list(OBJECT_LIST* object_list, OBJECT_LIST_RANGE_ITEM items[], size_t length, bool need_release)
{
    if (!object_list) {
        return false;
    }
    if (!items) {
        return false;
    }
    if (!length) {
        return false;
    }

    extend_capacity_if_not_enough_object_list(object_list, length);

    for (size_t i = 0; i < length; i++) {
        OBJECT_LIST_ITEM item = {
            .value = items[i].value,
            .library = {
                .need_release = need_release,
            },
        };
        object_list->items[object_list->length + i] = item;
    }

    object_list->length += length;

    return true;
}

bool pop_object_list(void** result, OBJECT_LIST* object_list)
{
    if (!object_list) {
        return false;
    }

    if (!object_list->length) {
        return false;
    }

    OBJECT_LIST_ITEM* item = object_list->items + (--object_list->length);

    *result = item->value;

    return true;
}

OBJECT_RESULT_VALUE get_object_list(OBJECT_LIST* object_list, size_t index)
{
    if (index < object_list->length) {
        OBJECT_LIST_ITEM* item = (OBJECT_LIST_ITEM*)object_list->items + index;
        OBJECT_RESULT_VALUE result = {
            .value = item->value,
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
        OBJECT_LIST_ITEM item = {
            .value = value,
            .library = {
                .need_release = need_release,
            },
        };
        object_list->items[index] = item;
        return true;
    }

    return false;
}

void clear_object_list(OBJECT_LIST* list)
{
    for (size_t i = 0; i < list->length; i++) {
        list->library.free_object_list_value(list->items + i);
    }
    list->length = 0;
}
