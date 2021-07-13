#include "assert.h"

#include "map.h"
#include "memory.h"
#include "tstring.h"

void doNotFreeMap(MAP_PAIR* pair)
{ /* 何もしない */ }

MAP createMap(size_t capacity, funcFreeMapValue freeMapValue)
{
    MAP map = {
        freeMapValue,
        allocateMemory(capacity * sizeof(MAP_PAIR), false),
        0,
        capacity,
    };

    return map;
}

void freeMap(MAP* map)
{
    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &(map->pairs[i]);

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

MAP_PAIR* addMap(MAP* map, const TCHAR* key, void* value, bool managed)
{
    return NULL;
}

MAP_PAIR* setMap(MAP* map, const TCHAR* key, void* value, bool managed)
{
    return NULL;
}

bool removeMap(MAP* map, const TCHAR* key)
{
    return false;
}
