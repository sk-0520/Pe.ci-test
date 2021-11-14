#include "debug.h"
#include "map.h"
#include "memory.h"
#include "tstring.h"

bool equals_map_key_default(const TEXT* a, const TEXT* b)
{
    return !compare_text(a, b, false);
}

void free_map_value_null(MAP_PAIR* pair)
{
    /* 何もしない */
}

MAP RC_HEAP_FUNC(create_map, size_t capacity, func_equals_map_key equals_map_key, func_free_map_value free_map_value)
{
    MAP map = {
        .pairs = allocate_raw_memory(capacity * sizeof(MAP_PAIR), false, DEFAULT_MEMORY),
        .length = 0,
        .library = {
            .capacity = capacity,
            .equals_map_key = equals_map_key,
            .free_value = free_map_value,
        },
    };

    return map;
}

static void free_map_pair_value_only(MAP* map, MAP_PAIR* pair)
{
    if (pair->library.need_release) {
        map->library.free_value(pair);
        pair->value = NULL;
    }
}

static void free_map_pair(MAP* map, MAP_PAIR* pair)
{
    free_text(&pair->key);
    free_map_pair_value_only(map, pair);
}

bool RC_HEAP_FUNC(free_map, MAP* map)
{
    if (!map) {
        return false;
    }

    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &(map->pairs[i]);

        free_map_pair(map, pair);
    }

    RC_HEAP_CALL(free_memory, map->pairs);
    map->pairs = NULL;
    map->length = 0;
    map->library.capacity = 0;

    return true;
}

static void extend_capacity_if_not_enough_map(MAP* map, size_t need_length)
{
    byte_t need_bytes = need_length * sizeof(MAP_PAIR);
    byte_t current_bytes = map->length * sizeof(MAP_PAIR);
    byte_t default_capacity_bytes = MAP_DEFAULT_CAPACITY * sizeof(MAP_PAIR);

    byte_t extend_total_byte = library__extend_capacity_if_not_enough_bytes_x2(&map->pairs, current_bytes, map->library.capacity * sizeof(MAP_PAIR), need_bytes, default_capacity_bytes, DEFAULT_MEMORY);
    if (extend_total_byte) {
        map->library.capacity = extend_total_byte / sizeof(MAP_PAIR);
    }
}

bool initialize_map(MAP* map, MAP_INIT init[], size_t length, bool need_release)
{
    for (size_t i = 0; i < length; i++) {
        MAP_INIT* item = &init[i];
        bool result = add_map(map, &item->key, item->value, need_release);
        if (!result) {
            return false;
        }
    }

    return true;
}

static MAP_PAIR* find_map(const MAP* map, const TEXT* key)
{
    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &map->pairs[i];
        if (map->library.equals_map_key(&pair->key, key)) {
            return pair;
        }
    }
    return NULL;
}

bool exists_map(const MAP* map, const TEXT* key)
{
    return find_map(map, key);
}

static MAP_PAIR* add_key_core(MAP* map, const MAP_PAIR* pair)
{
    extend_capacity_if_not_enough_map(map, 1);

    size_t new_index = map->length++;
    map->pairs[new_index] = *pair;

    return &map->pairs[new_index];
}

MAP_PAIR* add_map(MAP* map, const TEXT* key, void* value, bool need_release)
{
    if (exists_map(map, key)) {
        return NULL;
    }

    MAP_PAIR pair = {
        .key = clone_text(key),
        .value = value,
        .library = {
            .need_release = need_release,
        },
    };

    return add_key_core(map, &pair);
}

MAP_PAIR* set_map(MAP* map, const TEXT* key, void* value, bool need_release)
{
    MAP_PAIR* current_pair = find_map(map, key);
    if (current_pair) {
        free_map_pair_value_only(map, current_pair);

        current_pair->value = value;
        current_pair->library.need_release = need_release;
    } else {
        current_pair = add_map(map, key, value, need_release);
    }

    return current_pair;
}

bool remove_map(MAP* map, const TEXT* key)
{
    MAP_PAIR* current_pair = find_map(map, key);
    if (!current_pair) {
        return false;
    }

    if (current_pair == &map->pairs[map->length - 1]) {
        // 最後尾の場合はずらさず要素の破棄のみ行う
        free_map_pair(map, current_pair);
        map->length -= 1;
        return true;
    }

    // 最後尾以外は前へずらす
    free_map_pair(map, current_pair); // 解放するけどアドレスだけもうちと使う
    size_t index = (size_t)(current_pair - map->pairs);
    move_memory(current_pair, current_pair + 1, sizeof(MAP_PAIR) * (map->length - index - 1));
    map->length -= 1;
    return true;
}

MAP_RESULT_VALUE get_map(const MAP* map, const TEXT* key)
{
    MAP_PAIR* pair = find_map(map, key);

    if (!pair) {
        MAP_RESULT_VALUE not_found = {
            .value = NULL,
            .exists = false,
        };

        return not_found;
    }

    MAP_RESULT_VALUE result = {
        .value = pair->value,
        .exists = true,
    };

    return result;
}
