#pragma once
#include <stdbool.h>
#include <tchar.h>

#include "tstring.h"
#include "text.h"

#define MAP_DEFAULT_CAPACITY 16

//#define MAP_MAX_KEY_LENGTH 64

/// <summary>
/// 文字列キーと値のペア。
/// </summary>
typedef struct _TAG_MAP_PAIR
{
    /// <summary>
    /// キー項目。
    /// </summary>
    TEXT key;

    /// <summary>
    /// 値。
    /// </summary>
    void* value;
    /// <summary>
    /// 値は管理下にあるか。
    /// </summary>
    bool managedValue;
} MAP_PAIR;

typedef int (*funcCompareMapKey)(const TEXT* a, const TEXT* b);
typedef void (*funcFreeMapValue)(MAP_PAIR* pair);

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
    size_t _capacity;

    funcCompareMapKey _compareMapKey;
    funcFreeMapValue _freeValue;
} MAP;

int compareMapKeyDefault(const TEXT* a, const TEXT* b);

/// <summary>
/// マップの値解放不要処理。
/// </summary>
/// <param name="pair"></param>
void freeMapEmpty(MAP_PAIR* pair);

/// <summary>
/// マップの生成。
/// </summary>
/// <param name="capacity">初期予約領域。特に指定しない場合は<c>MAP_DEFAULT_CAPACITY</c>を使用する。</param>
/// <param name="compareMapKey">キー比較処理。</param>
/// <param name="freeMapValue">値解放処理。</param>
/// <returns></returns>
MAP createMap(size_t capacity, funcCompareMapKey compareMapKey, funcFreeMapValue freeMapValue);
#define createMapDefault(capacity) createMap((capacity), compareMapKeyDefault, freeMapEmpty);

/// <summary>
/// マップの開放。
/// </summary>
/// <param name="map">対象マップ。</param>
void freeMap(MAP* map);

/// <summary>
/// 指定したキーが存在するか。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <returns>存在するペア情報。存在しない場合は<c>NULL</c>。</returns>
MAP_PAIR* existsMap(MAP* map, const TCHAR* key);
/// <summary>
/// 値の追加。
/// 既に存在する場合は失敗する。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <param name="value">値。</param>
/// <param name="needRelease">開放が必要か</param>
/// <returns>追加されたペア情報。追加できない場合は<c>NULL</c>。</returns>
MAP_PAIR* addMap(MAP* map, const TCHAR* key, void* value, bool needRelease);
/// <summary>
/// 値の設定。
/// 既に存在する場合は(解放処理とともに)上書き、存在しない場合は追加される。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <param name="value">値。</param>
/// <param name="needRelease">開放が必要か</param>
/// <returns>設定されたペア情報。</returns>
MAP_PAIR* setMap(MAP* map, const TCHAR* key, void* value, bool needRelease);
/// <summary>
/// 削除。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <returns>削除の成功状態。</returns>
bool removeMap(MAP* map, const TCHAR* key);


