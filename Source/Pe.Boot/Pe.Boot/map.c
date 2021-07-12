#include "map.h"
#include "memory.h"
#include "tstring.h"

MAP createMap(size_t capacity)
{
    MAP map = {
        allocateMemory(capacity * sizeof(MAP_PAIR), false),
        0,
        capacity,
    };

    return map;
}

void freeMap(MAP* map, funcFreeMapPair freeMapPair)
{
    for (size_t i = 0; i < map->length; i++) {
        MAP_PAIR* pair = &(map->pairs[i]);

        freeString(pair->key);

        if (pair->managedValue) {
            assert(freeMapPair);
            freeMapPair(pair->value);
        }
    }

    freeMemory(map->pairs);
    map->length = 0;
    map->capacity = 0;
}
