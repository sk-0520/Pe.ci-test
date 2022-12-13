#include "linked_list.h"
#include "debug.h"

typedef struct tag_LINK_NODE
{
    /// <summary>
    /// 前ノード。
    /// <para>先頭の場合は<see cref="NULL" /></para>
    /// </summary>
    struct tag_LINK_NODE* prev;
    /// <summary>
    /// 次ノード。
    /// <para>終端の場合は<see cref="NULL" /></para>
    /// </summary>
    struct tag_LINK_NODE* next;
    /// <summary>
    /// 格納値。
    /// <para>値の領域までは<see cref="LINKED_LIST" />で面倒見るが、値の中身は呼び出し側で責任を持つこと。</para>
    /// </summary>
    void* value;
} LINK_NODE;

const void* get_link_node_value(const LINK_NODE* node)
{
    assert(node);

    return node->value;
}

static LINK_NODE* search_linked_list_core(const LINKED_LIST* linked_list, const void* needle, void* arg, func_search_linked_list func)
{
    if (!linked_list) {
        return NULL;
    }

    if (!linked_list->length) {
        return NULL;
    }

    LINK_NODE* node = linked_list->library.head;
    while (node) {
        if (func(needle, node->value, linked_list->data, arg)) {
            return node;
        }
        node = node->next;
    }

    return NULL;
}

int compare_linked_list_value_null(const void* a, const void* b, void* data)
{
    return 0;
}

void release_linked_list_value_null(void* value, void* data, const MEMORY_RESOURCE* memory_resource)
{
}

LINKED_LIST RC_HEAP_FUNC(new_linked_list, byte_t item_size, void* data, func_compare_linked_list_value compare_linked_list_value, func_release_linked_list_value release_linked_list_value, const MEMORY_RESOURCE* value_memory_resource, const MEMORY_RESOURCE* linked_list_memory_resource)
{
    LINKED_LIST result = {
        .length = 0,
        .data = data,
        .library = {
            .value_bytes = item_size,
            .value_memory_resource = value_memory_resource,
            .linked_list_memory_resource = linked_list_memory_resource,
            .compare_linked_list_value = compare_linked_list_value,
            .release_linked_list_value = release_linked_list_value,
            .head = NULL,
            .tail = NULL,
    }
    };

    return result;
}

static void release_linked_list_item(LINKED_LIST* linked_list, void* item)
{
    if (item) {
        linked_list->library.release_linked_list_value(item, linked_list->data, linked_list->library.value_memory_resource);
    }
}

static void RC_HEAP_FUNC(release_linked_list_node, LINKED_LIST* linked_list, LINK_NODE* node, bool value_release)
{
    if (value_release) {
        // 値自体の解放は確保側処理に任せる
        release_linked_list_item(linked_list, node->value);
    }

    // 値の領域自体はLINKED_LIST側で確保しているので解放
    RC_HEAP_CALL(release_memory, node->value, linked_list->library.linked_list_memory_resource);
    RC_HEAP_CALL(release_memory, node, linked_list->library.linked_list_memory_resource);
}

static void RC_HEAP_FUNC(release_linked_list_core, LINKED_LIST* linked_list, bool value_release)
{
    if (linked_list->length) {
        LINK_NODE* node = linked_list->library.head;
        while (node) {
            LINK_NODE* current_node = node;
            node = current_node->next;

            RC_HEAP_CALL(release_linked_list_node, linked_list, current_node, value_release);
        }
    }

    linked_list->length = 0;
    linked_list->library.head = NULL;
    linked_list->library.tail = NULL;
}

bool RC_HEAP_FUNC(release_linked_list, LINKED_LIST* linked_list, bool value_release)
{
    if (!linked_list) {
        return false;
    }

    RC_HEAP_CALL(release_linked_list_core, linked_list, value_release);

    return true;
}

static LINK_NODE* RC_HEAP_FUNC(new_link_node, LINKED_LIST* linked_list, void* value)
{
    LINK_NODE* node = RC_HEAP_CALL(new_memory, 1, sizeof(LINK_NODE), linked_list->library.linked_list_memory_resource);

    node->next = NULL;
    node->prev = NULL;
    // 値の領域自体はLINKED_LISTで確保
    node->value = RC_HEAP_CALL(new_memory, 1, linked_list->library.value_bytes, linked_list->library.linked_list_memory_resource);

    copy_memory(node->value, value, linked_list->library.value_bytes);

    return node;
}

void* RC_HEAP_FUNC(add_linked_list, LINKED_LIST* linked_list, void* value)
{
    LINK_NODE* node = RC_HEAP_CALL(new_link_node, linked_list, value);

    if (!linked_list->length) {
        // 初回は先頭・終端として生成
        linked_list->library.head = node;
        linked_list->library.tail = node;
    } else {
        // 通常追加は終端ノードを付け替える
        LINK_NODE* current_tail_node = linked_list->library.tail;
        current_tail_node->next = node;
        node->prev = current_tail_node;
        linked_list->library.tail = node;
    }

    linked_list->length += 1;

    return linked_list->library.tail->value;
}

static LINK_NODE* find_node_by_index(const LINKED_LIST* linked_list, size_t index)
{
    assert(index < linked_list->length);

    // 先頭を使用
    if (!index) {
        return linked_list->library.head;
    }

    // 終端を使用
    if (index == linked_list->length - 1) {
        return linked_list->library.tail;
    }

    // 次の奴はとりま返しとく
    if (index == 1) {
        return linked_list->library.head->next;
    }
    // 最終から1つ前も返しとく
    if (index - 1 == linked_list->length - 2) {
        return linked_list->library.tail->prev;
    }

    // 3番目から頑張る
    LINK_NODE* node = linked_list->library.head->next;
    for (size_t i = 2; i <= index; i++) {
        node = node->next;
    }
    return node;
}

LINK_RESULT_VALUE get_linked_list(const LINKED_LIST* linked_list, size_t index)
{
    if (index < linked_list->length) {
        LINK_NODE* node = find_node_by_index(linked_list, index);
        LINK_RESULT_VALUE result = {
            .value = node->value,
            .exists = true,
            .node = node,
        };

        return result;
    }

    LINK_RESULT_VALUE none = {
        .value = NULL,
        .exists = false,
        .node = NULL,
    };
    return none;

}

static void RC_HEAP_FUNC(insert_linked_list_core, LINKED_LIST* linked_list, LINK_NODE* node, void* value)
{
    assert(node);

    LINK_NODE* new_node = RC_HEAP_CALL(new_link_node, linked_list, value);

    if (node->prev) {
        node->prev->next = new_node;
    }

    new_node->prev = node->prev;
    new_node->next = node;

    node->prev = new_node;

    if (linked_list->library.head == node) {
        linked_list->library.head = new_node;
    }

    linked_list->length += 1;
}

bool RC_HEAP_FUNC(insert_linked_list, LINKED_LIST* linked_list, size_t index, void* value)
{
    // 保持する長さ以下の場合は失敗する
    if (linked_list->length <= index) {
        return false;
    }

    LINK_NODE* current_node = find_node_by_index(linked_list, index);

    RC_HEAP_CALL(insert_linked_list_core, linked_list, current_node, value);

    return true;
}

static void RC_HEAP_FUNC(remove_linked_list_core, LINKED_LIST* linked_list, LINK_NODE* node)
{
    assert(node);

    if (linked_list->length == 1) {
        RC_HEAP_CALL(release_linked_list_node, linked_list, node, true);
        linked_list->library.head = linked_list->library.tail = NULL;
        linked_list->length = 0;
        return;
    }

    assert(linked_list->length != 1);

    if (linked_list->library.head == node) {
        linked_list->library.head = node->next;
        linked_list->library.head->prev = NULL;
    } else if (linked_list->library.tail == node) {
        linked_list->library.tail = node->prev;
        linked_list->library.tail->next = NULL;
    } else {
        LINK_NODE* prev_node = node->prev;
        LINK_NODE* next_node = node->next;

        prev_node->next = next_node;
        next_node->prev = prev_node;

    }

    RC_HEAP_CALL(release_linked_list_node, linked_list, node, true);

    linked_list->length -= 1;
}

bool RC_HEAP_FUNC(remove_linked_list, LINKED_LIST* linked_list, size_t index)
{
    // 保持する長さ未満の場合は失敗する
    if (linked_list->length <= index) {
        return false;
    }

    LINK_NODE* current_node = find_node_by_index(linked_list, index);

    RC_HEAP_CALL(remove_linked_list_core, linked_list, current_node);

    return true;
}

static void RC_HEAP_FUNC(set_linked_list_core, LINKED_LIST* linked_list, LINK_NODE* node, void* value, bool need_release)
{
    assert(node);

    if (need_release) {
        release_linked_list_item(linked_list, node->value);
    }

    copy_memory(node->value, value, linked_list->library.value_bytes);
}

bool RC_HEAP_FUNC(set_linked_list, LINKED_LIST* linked_list, size_t index, void* value, bool need_release)
{
    // 保持する長さ未満の場合は失敗する
    if (linked_list->length <= index) {
        return false;
    }

    LINK_NODE* current_node = find_node_by_index(linked_list, index);

    RC_HEAP_CALL(set_linked_list_core, linked_list, current_node, value, need_release);

    return true;
}

size_t foreach_linked_list(const LINKED_LIST* linked_list, func_foreach_linked_list func, void* arg)
{
    assert(linked_list);
    assert(func);

    size_t result = 0;

    LINK_NODE* node = linked_list->library.head;
    while (node) {
        if (!func(node->value, result, linked_list->length, linked_list->data, arg)) {
            break;
        }
        node = node->next;
        result += 1;
    }

    return result;
}

const LINK_NODE* search_linked_list(const LINKED_LIST* linked_list, const void* needle, void* arg, func_search_linked_list func)
{
    return search_linked_list_core(linked_list, needle, arg, func);
}

bool RC_HEAP_FUNC(remove_linked_list_by_node, LINKED_LIST* linked_list, LINK_NODE* node)
{
    if (!linked_list) {
        return false;
    }

    if (!node) {
        return false;
    }

    RC_HEAP_CALL(remove_linked_list_core, linked_list, node);

    return true;
}

static bool to_object_list_from_linked_list_core(const void* value, size_t index, size_t length, void* data, void* arg)
{
    OBJECT_LIST* list = (OBJECT_LIST*)arg;

    push_object_list(list, value);

    return true;
}

OBJECT_LIST RC_HEAP_FUNC(to_object_list_from_linked_list, const LINKED_LIST* linked_list)
{
    OBJECT_LIST result = RC_HEAP_CALL(new_object_list, linked_list->library.value_bytes, linked_list->length, NULL, compare_object_list_value_null, release_object_list_value_null, linked_list->library.linked_list_memory_resource);

    foreach_linked_list(linked_list, to_object_list_from_linked_list_core, &result);

    return result;
}
