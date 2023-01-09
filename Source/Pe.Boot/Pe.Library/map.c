#include "map.h"
#include "debug.h"
#include "hash.h"
#include "lib_math.h"

size_t calc_map_hash_default(const TEXT* key)
{
    return calc_hash_fnv1_from_text(key);
}

bool equals_map_key_default(const TEXT* a, const TEXT* b)
{
    return is_equals_text(a, b, false);
}

static size_t get_capacity(const MAP* map)
{
    return map->library.mask + 1;
}

/// <summary>
/// 容量と負荷係数から次回タイミングの数を計算。
/// </summary>
/// <param name="capacity"></param>
/// <param name="load_factor"></param>
/// <returns></returns>
static size_t get_next_limit(size_t capacity, real_t load_factor)
{
    assert(capacity);

    size_t next_limit = (size_t)(capacity * load_factor);
    return next_limit;
}

static size_t get_hash_index(const MAP* map, const TEXT* key)
{
    size_t row_index = map->library.calc_map_hash(key);
    size_t hash_index = row_index & map->library.mask;

    assert(hash_index < get_capacity(map));

    return hash_index;
}

static KEY_VALUE_PAIR RC_HEAP_FUNC(new_map_pair, const MAP* map, const TEXT* key, const void* value)
{
    KEY_VALUE_PAIR pair = {
        .key = clone_text(key, map->library.map_memory_resource),
        .value = NULL,
    };
    pair.value = RC_HEAP_CALL(new_memory, 1, map->library.value_bytes, map->library.map_memory_resource);
    copy_memory(pair.value, value, map->library.value_bytes);

    return pair;
}

static bool search_map_from_key_core(const void* needle, const void* value, void* data, void* arg)
{
    const TEXT* needle_key = (const TEXT*)needle;
    const KEY_VALUE_PAIR* pair = (const KEY_VALUE_PAIR*)value;
    const MAP* map = (const MAP*)arg;

    return map->library.equals_map_key(needle_key, &pair->key);
}

static LINK_NODE* search_map_from_key(const LINKED_LIST* linked_list, const TEXT* needle, const MAP* map)
{
    return (LINK_NODE*)search_linked_list(linked_list, needle, (void*)map, search_map_from_key_core);
}

static void release_link_item_value(void* value, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    KEY_VALUE_PAIR* pair = (KEY_VALUE_PAIR*)value;
    // 再構築時はdataが空のため値自体の解放はスキップさせる(NULL設定は再構築処理側)
    if (data) {
        func_release_map_value release_value = (func_release_map_value)data;
        release_value(&pair->key, pair->value, memory_arena_resource);
    }

    release_memory(pair->value, pair->key.library.memory_arena_resource); // まぁ、うん
    release_text(&pair->key);
}

static LINKED_LIST* RC_HEAP_FUNC(new_items, size_t capacity, func_release_linked_list_value release_map_value, const MEMORY_ARENA_RESOURCE* value_memory_resource, const MEMORY_ARENA_RESOURCE* map_memory_resource)
{
    LINKED_LIST* item_lists = RC_HEAP_CALL(new_memory, capacity, sizeof(LINKED_LIST), map_memory_resource);
    for (size_t i = 0; i < capacity; i++) {
        LINKED_LIST item_list = RC_HEAP_CALL(new_linked_list, sizeof(KEY_VALUE_PAIR), (void*)release_map_value, compare_linked_list_value_null, release_link_item_value, value_memory_resource, map_memory_resource);
        item_lists[i] = item_list;
    }

    return item_lists;
}

MAP RC_HEAP_FUNC(new_map, byte_t item_size, size_t capacity, real_t load_factor, func_release_linked_list_value release_map_value, func_calc_map_hash calc_map_hash, func_equals_map_key equals_map_key, const MEMORY_ARENA_RESOURCE* value_memory_resource, const MEMORY_ARENA_RESOURCE* map_memory_resource)
{
    size_t adjusted_capacity = power_of_2(capacity ? capacity : MAP_DEFAULT_CAPACITY);

    MAP result = {
        .length = 0,
        .library = {
            .value_bytes = item_size,
            .next_limit = get_next_limit(adjusted_capacity, load_factor),
            .mask = adjusted_capacity - 1,
            .load_factor = load_factor,
            .items = RC_HEAP_CALL(new_items, adjusted_capacity, release_map_value, value_memory_resource, map_memory_resource),
            .map_memory_resource = map_memory_resource,
            .value_memory_resource = value_memory_resource,
            .release_map_value = release_map_value,
            .calc_map_hash = calc_map_hash,
            .equals_map_key = equals_map_key,
#ifdef DEBUG
            .now_rehash = false,
#endif
    }
    };

    return result;
}

void RC_HEAP_FUNC(release_map_core, MAP* map, bool value_release)
{
    size_t length = get_capacity(map);
    for (size_t i = 0; i < length; i++) {
        LINKED_LIST* linked_list = map->library.items + i;
        RC_HEAP_CALL(release_linked_list, linked_list, value_release);
    }
    RC_HEAP_CALL(release_memory, map->library.items, map->library.map_memory_resource);

    map->length = 0;
    map->library.mask = 0;
}

bool RC_HEAP_FUNC(release_map, MAP* map)
{
    if (!map) {
        return false;
    }

    RC_HEAP_CALL(release_map_core, map, true);

    return true;
}

static void RC_HEAP_FUNC(extend_map_if_over_load_factor, MAP* map)
{
    if (map->length <= map->library.next_limit) {
        // 拡張不要
        return;
    }
#ifdef RES_CHECK
    assert(!map->library.now_rehash);
    map->library.now_rehash = true;
#endif

    size_t current_capacity = get_capacity(map);
    LINKED_LIST* current_items = map->library.items;

    // 各値を平坦に保存
    KEY_VALUE_PAIR* flat_list = RC_HEAP_CALL(new_memory, map->length, sizeof(KEY_VALUE_PAIR), map->library.map_memory_resource);
    size_t flat_list_length = 0;

    for (size_t i = 0; i < current_capacity; i++) {
        LINKED_LIST* current_chain = current_items + i;
        if (current_chain->length) {
            OBJECT_LIST current_chain_list = RC_HEAP_CALL(to_object_list_from_linked_list, current_chain);
            const KEY_VALUE_PAIR* current_chain_items = reference_value_object_list(KEY_VALUE_PAIR, current_chain_list);
            for (size_t j = 0; j < current_chain_list.length; j++) {
                flat_list[flat_list_length++] = *(current_chain_items + j);
            }
            RC_HEAP_CALL(release_object_list, &current_chain_list, true);
        }
    }

    //拡張後のサイズを算出(拡張中に拡張されることは回避するため負荷係数と合わせて広げられるとこまで広げる)
    size_t new_capacity = current_capacity;
    size_t new_next_limit = map->library.next_limit;
    do {
        new_capacity <<= 1;
        new_next_limit = get_next_limit(new_capacity, map->library.load_factor);
    } while (new_next_limit < flat_list_length);

    LINKED_LIST* next_items = RC_HEAP_CALL(new_items, new_capacity, map->library.release_map_value, map->library.value_memory_resource, map->library.map_memory_resource);

    // 再構築
    map->library.mask = new_capacity - 1;
    map->library.next_limit = new_next_limit;
    map->library.items = next_items;
    map->length = 0;
    for (size_t i = 0; i < flat_list_length; i++) {
        KEY_VALUE_PAIR* pair = flat_list + i;
        RC_HEAP_CALL(add_map, map, &pair->key, pair->value);
    }

    // 旧配列の値以外解放
    RC_HEAP_CALL(release_memory, flat_list, map->library.map_memory_resource);
    for (size_t i = 0; i < current_capacity; i++) {
        LINKED_LIST* linked_list = current_items + i;
        linked_list->data = NULL;//release_link_item_value呼び出しの曲芸処理
        RC_HEAP_CALL(release_linked_list, linked_list, true);
    }
    RC_HEAP_CALL(release_memory, current_items, map->library.map_memory_resource);

#ifdef RES_CHECK
    map->library.now_rehash = false;
#endif
}

MAP_RESULT_VALUE get_map(const MAP* map, const TEXT* key)
{
    if (!map->length) {
        MAP_RESULT_VALUE none_item = {
            .value = NULL,
            .exists = false,
        };
        return none_item;
    }

    size_t index = get_hash_index(map, key);

    LINKED_LIST* linked_list = map->library.items + index;
    const LINK_NODE* node = search_map_from_key(linked_list, key, map);
    if (!node) {
        MAP_RESULT_VALUE not_found_item = {
            .value = NULL,
            .exists = false,
        };
        return not_found_item;
    }

    const KEY_VALUE_PAIR* pair = (const KEY_VALUE_PAIR*)get_link_node_value(node);

    MAP_RESULT_VALUE result = {
        .value = pair->value,
        .exists = true,
    };

    return result;
}

bool RC_HEAP_FUNC(add_map, MAP* map, const TEXT* key, void* value)
{
    size_t index = get_hash_index(map, key);

    LINKED_LIST* linked_list = map->library.items + index;
    const LINK_NODE* node = search_map_from_key(linked_list, key, map);
    if (node) {
        return false;
    }

    KEY_VALUE_PAIR pair = RC_HEAP_CALL(new_map_pair, map, key, value);

    add_linked_list(linked_list, &pair);

    map->length += 1;

    RC_HEAP_CALL(extend_map_if_over_load_factor, map);

    return true;
}

void RC_HEAP_FUNC(set_map, MAP* map, const TEXT* key, void* value)
{
    size_t index = get_hash_index(map, key);

    LINKED_LIST* linked_list = map->library.items + index;
    LINK_NODE* node = search_map_from_key(linked_list, key, map);
    if (node && linked_list->length) {
        //TODO: もう色々諦め感がすごい
        RC_HEAP_CALL(remove_linked_list_by_node, linked_list, node);
        map->length -= 1;
    }

    KEY_VALUE_PAIR pair = RC_HEAP_CALL(new_map_pair, map, key, value);

    add_linked_list(linked_list, &pair);

    map->length += 1;

    RC_HEAP_CALL(extend_map_if_over_load_factor, map);
}

bool RC_HEAP_FUNC(remove_map, MAP* map, const TEXT* key)
{
    size_t index = get_hash_index(map, key);

    LINKED_LIST* linked_list = map->library.items + index;
    LINK_NODE* node = search_map_from_key(linked_list, key, map);
    if (node && linked_list->length) {
        RC_HEAP_CALL(remove_linked_list_by_node, linked_list, node);
        map->length -= 1;

        return true;
    }

    return false;
}

typedef struct
{
    size_t length;
    size_t* index;
    void* arg;
    func_foreach_map func;
} FOREACH_MAP_DATA;

static bool foreach_map_core(const void* value, size_t index, size_t length, void* data, void* arg)
{
    FOREACH_MAP_DATA* foreach_arg = arg;
    KEY_VALUE_PAIR* pair = (KEY_VALUE_PAIR*)value;

    foreach_arg->func(pair, *foreach_arg->index, foreach_arg->length, foreach_arg->arg);

    *foreach_arg->index += 1;

    return true;
}

void foreach_map(const MAP* map, func_foreach_map func, void* arg)
{
    size_t length = map->length;
    size_t index = 0;

    FOREACH_MAP_DATA data = {
        .arg = arg,
        .index = &index,
        .length = length,
        .func = func,
    };

    for (size_t i = 0; i < length; i++) {
        LINKED_LIST* linked_list = map->library.items + i;
        foreach_linked_list(linked_list, foreach_map_core, &data);
    }
}
