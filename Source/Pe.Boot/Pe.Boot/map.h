#pragma once
#include <stdbool.h>
#include <tchar.h>

#include "tstring.h"
#include "text.h"

#define MAP_DEFAULT_CAPACITY 16

/// <summary>
/// 文字列キーと値のペア。
/// </summary>
typedef struct tag_MAP_PAIR
{
    /// <summary>
    /// キー項目。
    /// <para>キーそのものはMAPにて管理される。</para>
    /// </summary>
    TEXT key;

    /// <summary>
    /// 値。
    /// </summary>
    void* value;

    /// <summary>
   /// 管理データ。
   /// </summary>
    struct
    {
        /// <summary>
        /// 値の開放は必要か。
        /// </summary>
        bool needRelease;
    } library;
} MAP_PAIR;

typedef struct tag_MAP_INIT
{
    TEXT key;
    void* value;
} MAP_INIT;

/// <summary>
/// 値ラッパー。
/// </summary>
typedef struct tag_MAP_RESULT_VALUE
{
    /// <summary>
    /// 格納されている値。
    /// </summary>
    void* value;
    /// <summary>
    /// 値は取得できたか。
    /// </summary>
    bool exists;
} MAP_RESULT_VALUE;

typedef bool (*funcEqualsMapKey)(const TEXT* a, const TEXT* b);
typedef void (*funcFreeMapValue)(MAP_PAIR* pair);

/// <summary>
/// 連想配列データ。
///
/// 注意: 連想配列とは名ばかりの線形検索。
/// </summary>
typedef struct tag_MAP
{
    /// <summary>
    /// キー・値の配列構造。
    /// </summary>
    MAP_PAIR* pairs;
    /// <summary>
    /// 現在の項目数。
    /// </summary>
    size_t length;

    /// <summary>
    /// 管理データ。
    /// </summary>
    struct
    {
        /// <summary>
        /// 容量。
        /// </summary>
        size_t capacity;

        funcEqualsMapKey equalsMapKey;
        funcFreeMapValue freeValue;
    } library;
} MAP;

/// <summary>
/// キー項目比較の標準処理。
/// <para>大文字小文字を区別する通常の文字列比較。</para>
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
bool equalsMapKeyDefault(const TEXT* a, const TEXT* b);

/// <summary>
/// マップの値解放不要処理。
/// </summary>
/// <param name="pair"></param>
void freeMapValueNull(MAP_PAIR* pair);

/// <summary>
/// マップの生成。
/// </summary>
/// <param name="capacity">初期予約領域。特に指定しない場合は<c>MAP_DEFAULT_CAPACITY</c>を使用する。</param>
/// <param name="equalsMapKey">キー比較処理。</param>
/// <param name="freeMapValue">値解放処理。</param>
/// <returns></returns>
MAP createMap(size_t capacity, funcEqualsMapKey equalsMapKey, funcFreeMapValue freeMapValue);
#define createMapDefault(freeMapValue) createMap(MAP_DEFAULT_CAPACITY, compareMapKeyDefault, freeMapPairValueOnly)

bool initializeMap(MAP* map, MAP_INIT init[], size_t length, bool needRelease);

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
/// <returns>有無。</returns>
bool existsMap(const MAP* map, const TEXT* key);

/// <summary>
/// 値の追加。
/// 既に存在する場合は失敗する。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <param name="value">値。</param>
/// <param name="needRelease">開放が必要か</param>
/// <returns>追加されたペア情報。追加できない場合は<c>NULL</c>。</returns>
MAP_PAIR* addMap(MAP* map, const TEXT* key, void* value, bool needRelease);
/// <summary>
/// 値の設定。
/// 既に存在する場合は(解放処理とともに)上書き、存在しない場合は追加される。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <param name="value">値。</param>
/// <param name="needRelease">開放が必要か</param>
/// <returns>設定されたペア情報。</returns>
MAP_PAIR* setMap(MAP* map, const TEXT* key, void* value, bool needRelease);
/// <summary>
/// 削除。
/// 詰め処理が行われているため呼び出し前の<c>MAP_PAIR*</c>は使用不可になる。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <returns>削除の成功状態。</returns>
bool removeMap(MAP* map, const TEXT* key);

/// <summary>
/// 取得。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <returns>取得データ。</returns>
MAP_RESULT_VALUE getMap(MAP* map, const TEXT* key);

