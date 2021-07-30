#include "object_list.h"
#include "memory.h"

void free_object_list_value_null(void* value)
{
}

OBJECT_LIST RC_HEAP_FUNC(create_object_list, size_t capacity, func_free_object_list_value free_object_list_value)
{
    OBJECT_LIST result;

    result.length = 0;
    result.library.capacity = capacity;
    result.library.free_object_list_value = free_object_list_value;

    result.buffer = RC_HEAP_CALL(allocate_memory, sizeof(OBJECT_LIST_ITEM) * capacity, false);

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
        free_object_list_item(object_list, object_list->buffer + i);
    }

    RC_HEAP_CALL(free_memory, object_list->buffer);

    object_list->buffer = NULL;
    object_list->length = 0;
    object_list->library.capacity = 0;
    object_list->library.free_object_list_value = NULL;

    return true;
}


