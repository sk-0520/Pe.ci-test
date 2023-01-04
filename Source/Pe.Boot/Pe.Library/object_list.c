#include "object_list.h"
#include "memory.h"
#include "text.h"
#include "debug.h"


int compare_object_list_value_null(const void* a, const void* b, void* data)
{
    return -1;
}

void release_object_list_value_null(void* value, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
}

int compare_object_list_value_text(const void* a, const void* b, void* data)
{
    const TEXT* aa = (TEXT*)a;
    const TEXT* bb = (TEXT*)b;
    return compare_text(aa, bb, false);
}

void release_object_list_value_text(void* target, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!target) {
        return;
    }
    TEXT* text = (TEXT*)target;
    release_text(text);
}

OBJECT_LIST RC_HEAP_FUNC(new_object_list, byte_t item_size, size_t capacity_count, void* data, func_compare_object_list_value compare_object_list_value, func_release_object_list_value release_object_list_value, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    assert(item_size);

    OBJECT_LIST result = {
        .length = 0,
        .items = RC_HEAP_CALL(allocate_raw_memory, capacity_count * item_size, false, memory_arena_resource),
        .data = data,
        .library = {
            .memory_arena_resource = memory_arena_resource,
            .item_size = item_size,
            .capacity = capacity_count + item_size,
            .compare_object_list_value = compare_object_list_value,
            .release_object_list_value = release_object_list_value
    },
    };

    return result;
}

bool RC_HEAP_FUNC(release_object_list, OBJECT_LIST* object_list, bool value_release)
{
    if (!object_list) {
        return false;
    }

    if (value_release) {
        for (size_t i = 0; i < object_list->length; i++) {
            void* item = object_list->items + (i * object_list->library.item_size);
            object_list->library.release_object_list_value(item, object_list->data, object_list->library.memory_arena_resource);
        }
    }

    RC_HEAP_CALL(release_memory, object_list->items, object_list->library.memory_arena_resource);

    object_list->items = NULL;
    object_list->length = 0;
    object_list->library.capacity = 0;
    object_list->library.release_object_list_value = NULL;

    return true;
}

static void extend_capacity_if_not_enough_object_list(OBJECT_LIST* object_list, size_t need_length)
{
    byte_t need_bytes = need_length * object_list->library.item_size;
    byte_t current_bytes = object_list->length * object_list->library.item_size;
    byte_t default_capacity_bytes = OBJECT_LIST_DEFAULT_CAPACITY_COUNT * object_list->library.item_size;

    byte_t extend_total_byte = library_extend_capacity_if_not_enough_bytes_x2(&object_list->items, current_bytes, object_list->library.capacity * object_list->library.item_size, need_bytes, default_capacity_bytes, object_list->library.memory_arena_resource);
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

bool set_object_list(OBJECT_LIST* object_list, size_t index, void* value, bool value_release)
{
    if (!object_list) {
        return false;
    }

    if (index < object_list->length) {
        void* current = object_list->items + index * object_list->library.item_size;
        if (value_release) {
            object_list->library.release_object_list_value(current, object_list->data, object_list->library.memory_arena_resource);
        }
        copy_memory(current, value, object_list->library.item_size);
        return true;
    }

    return false;
}

void clear_object_list(OBJECT_LIST* object_list, bool value_release)
{
    if (value_release) {
        for (size_t i = 0; i < object_list->length; i++) {
            object_list->library.release_object_list_value(object_list->items + (i * object_list->library.item_size), object_list->data, object_list->library.memory_arena_resource);
        }
    }

    object_list->length = 0;
}

