#pragma once
#include <stdbool.h>
#include <tchar.h>

#define MAP_DEFAULT_CAPACITY 16

/// <summary>
/// 文字列キーと値のペア。
/// </summary>
typedef struct _TAG_MAP_PAIR
{
    /// <summary>
    /// キー項目。
    /// </summary>
    const TCHAR* key;
    /// <summary>
    /// キー長。
    /// </summary>
    size_t keyLength;

    /// <summary>
    /// 値。
    /// </summary>
    void* value;
    /// <summary>
    /// 値は管理下にあるか。
    /// </summary>
    bool managedValue;
} MAP_PAIR;

/// <summary>
/// 連想配列データ。
///
/// 注意: 連想配列とは名ばかりの線形検索。
/// </summary>
typedef struct _TAG_MAP
{
    /// <summary>
    /// キー・値。
    /// </summary>
    MAP_PAIR* pairs;
    /// <summary>
    /// 現在の項目数。
    /// </summary>
    size_t length;
    /// <summary>
    /// 容量。
    /// </summary>
    size_t capacity;
} MAP;

typedef void (*funcFreeMapPair)(void* value);

MAP createMap(size_t capacity);
void freeMap(MAP* map, funcFreeMapPair freeMapPair);
