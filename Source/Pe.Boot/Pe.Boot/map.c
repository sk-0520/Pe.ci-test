#include <assert.h>

#include "map.h"
#include "memory.h"
#include "tstring.h"

int compareMapKeyDefault(const TEXT* a, const TEXT* b)
{
    return compareString(a->value, b->value, false);
}

void freeMapEmpty(MAP_PAIR* pair)
{
    /* 何もしない */
}

MAP createMap(size_t capacity, funcCompareMapKey compareMapKey, funcFreeMapValue freeMapValue)
{
    MAP map = {
        .pairs = allocateMemory(capacity * sizeof(MAP_PAIR), false),
        .length = 0,
        ._capacity = capacity,
        ._compareMapKey = compareMapKey,
        ._freeValue = freeMapValue,
    };

    return map;
}

void freeMap(MAP* map)
{
    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &(map->pairs[i]);

        freeString(pair->key.value);

        if (pair->managedValue) {
            map->_freeValue(pair->value);
        }
    }

    freeMemory(map->pairs);
    map->length = 0;
    map->_capacity = 0;
}

MAP_PAIR* existsMap(MAP* map, const TCHAR* key)
{
    return NULL;
}

MAP_PAIR* addMap(MAP* map, const TCHAR* key, void* value, bool needRelease)
{
    return NULL;
}

MAP_PAIR* setMap(MAP* map, const TCHAR* key, void* value, bool needRelease)
{
    return NULL;
}

bool removeMap(MAP* map, const TCHAR* key)
{
    return false;
}
