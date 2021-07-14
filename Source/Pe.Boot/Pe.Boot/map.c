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
        ._mng = {
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
            map->_mng.freeValue(pair->value);
        }
    }

    freeMemory(map->pairs);
    map->length = 0;
    map->_mng.capacity = 0;
}

static MAP_PAIR* _findMap(const MAP* map, const TEXT* key)
{
    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &map->pairs[i];
        if (!map->_mng.compareMapKey(&pair->key, key)) {
            return pair;
        }
    }
    return NULL;
}

bool existsMap(const MAP* map, const TEXT* key)
{
    return _findMap(map, key);
}

MAP_PAIR* addMap(MAP* map, const TEXT* key, void* value, bool needRelease)
{
    return NULL;
}

MAP_PAIR* setMap(MAP* map, const TEXT* key, void* value, bool needRelease)
{
    return NULL;
}

bool removeMap(MAP* map, const TEXT* key)
{
    return false;
}
