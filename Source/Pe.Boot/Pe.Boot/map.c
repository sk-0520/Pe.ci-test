#include <assert.h>

#include "map.h"
#include "memory.h"
#include "tstring.h"

int compareMapKeyDefault(const TEXT* a, const TEXT* b)
{
    return compareString(a->value, b->value, false);
}

void freeMapValueNull(MAP_PAIR* pair)
{
    /* 何もしない */
}

MAP createMap(size_t capacity, funcCompareMapKey compareMapKey, funcFreeMapValue freeMapValue)
{
    MAP map = {
        .pairs = allocateMemory(capacity * sizeof(MAP_PAIR), false),
        .length = 0,
        .library = {
            .capacity = capacity,
            .compareMapKey = compareMapKey,
            .freeValue = freeMapValue,
        },
    };

    return map;
}

void freeMap(MAP* map)
{
    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &(map->pairs[i]);

        freeText(&pair->key);

        if (pair->library.needRelease) {
            map->library.freeValue(pair->value);
        }
    }

    freeMemory(map->pairs);
    map->pairs = NULL;
    map->length = 0;
    map->library.capacity = 0;
}

static MAP_PAIR* findMap(const MAP* map, const TEXT* key)
{
    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &map->pairs[i];
        if (!map->library.compareMapKey(&pair->key, key)) {
            return pair;
        }
    }
    return NULL;
}

bool existsMap(const MAP* map, const TEXT* key)
{
    return findMap(map, key);
}

MAP_PAIR* addKeyCore(MAP* map, const MAP_PAIR* pair)
{
    if (map->length == map->library.capacity) {
        // 拡張が必要
        map->library.capacity *= 2;
        MAP_PAIR* pairs = allocateMemory(map->library.capacity * sizeof(MAP_PAIR), false);
        copyMemory(pairs, map->pairs, map->length * sizeof(MAP_PAIR));
        freeMemory(map->pairs);
        map->pairs = pairs;
    }

    size_t newIndex = map->length++;
    map->pairs[newIndex] = *pair;

    return &map->pairs[newIndex];
}

MAP_PAIR* addMap(MAP* map, const TEXT* key, void* value, bool needRelease)
{
    if (existsMap(map, key)) {
        return NULL;
    }

    MAP_PAIR pair = {
        .key = cloneText(key),
        .value = value,
        .library = {
            .needRelease = needRelease,
        },
    };


    return addKeyCore(map, &pair);
}

MAP_PAIR* setMap(MAP* map, const TEXT* key, void* value, bool needRelease)
{
    MAP_PAIR* currentPair = findMap(map, key);
    if (currentPair) {
        if (currentPair->library.needRelease) {
            map->library.freeValue(currentPair->value);
        }
        currentPair->value = value;
        currentPair->library.needRelease = needRelease;
    } else {
        currentPair = addMap(map, key, value, needRelease);
    }

    return currentPair;
}

bool removeMap(MAP* map, const TEXT* key)
{
    return false;
}
