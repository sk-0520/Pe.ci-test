#pragma once
#include <stdbool.h>

#include <tchar.h>

#include "tstring.h"
#include "text.h"

// 呼び出し側でヒープを確保して使用する前提。のはず。

#define MAP_DEFAULT_CAPACITY (16)

/// <summary>
/// 文字列キーと値のペア。
/// </summary>
typedef struct tag_MAP_PAIR
{
    /// <summary>
    /// キー項目。
    /// <para>キーそのものは<see cref="MAP" />にて管理される。</para>
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
        bool need_release;
    } library;
} MAP_PAIR;

/// <summary>
/// マップ初期化データ。
/// </summary>
typedef struct tag_MAP_INIT
{
    /// <summary>
    /// キー。
    /// </summary>
    TEXT key;
    /// <summary>
    /// データ。
    /// </summary>
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

/// <summary>
/// マップキー比較処理。
/// </summary>
typedef bool (*func_equals_map_key)(const TEXT* a, const TEXT* b);
/// <summary>
/// マップ値解放処理。
/// </summary>
typedef void (*func_free_map_value)(MAP_PAIR* pair);

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
        /// <summary>
        /// キー比較処理。
        /// </summary>
        func_equals_map_key equals_map_key;
        /// <summary>
        /// 値解放処理。
        /// </summary>
        func_free_map_value free_value;
    } library;
} MAP;

/// <summary>
/// キー項目比較の標準処理。
/// <para>大文字小文字を区別する通常の文字列比較。</para>
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
bool equals_map_key_default(const TEXT* a, const TEXT* b);

/// <summary>
/// マップの値解放不要処理。
/// </summary>
/// <param name="pair"></param>
void free_map_value_null(MAP_PAIR* pair);

/// <summary>
/// マップの生成。
/// </summary>
/// <param name="capacity">初期予約領域。特に指定しない場合は<c>MAP_DEFAULT_CAPACITY</c>を使用する。</param>
/// <param name="equals_map_key">キー比較処理。</param>
/// <param name="free_map_value">値解放処理。</param>
/// <returns></returns>
MAP RC_HEAP_FUNC(create_map, size_t capacity, func_equals_map_key equals_map_key, func_free_map_value free_map_value);
#ifdef RES_CHECK
#   define create_map(capacity, equals_map_key, free_map_value) RC_HEAP_WRAP(create_map, (capacity), (equals_map_key), (free_map_value))
#endif

#define create_map_default(freeMapValue) create_map(MAP_DEFAULT_CAPACITY, equals_map_key_default, free_map_value_null)

/// <summary>
/// 初期化処理。
/// </summary>
/// <param name="map"></param>
/// <param name="init"></param>
/// <param name="length"></param>
/// <param name="need_release"></param>
/// <returns></returns>
bool initialize_map(MAP* map, MAP_INIT init[], size_t length, bool need_release);

/// <summary>
/// マップの開放。
/// </summary>
/// <param name="map">対象マップ。</param>
bool RC_HEAP_FUNC(free_map, MAP* map);
#ifdef RES_CHECK
#   define free_map(map) RC_HEAP_WRAP(free_map, (map))
#endif

/// <summary>
/// 指定したキーが存在するか。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <returns>有無。</returns>
bool exists_map(const MAP* map, const TEXT* key);

/// <summary>
/// 値の追加。
/// 既に存在する場合は失敗する。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <param name="value">値。</param>
/// <param name="need_release">解放が必要か</param>
/// <returns>追加されたペア情報。追加できない場合は<c>NULL</c>。</returns>
MAP_PAIR* add_map(MAP* map, const TEXT* key, void* value, bool need_release);
/// <summary>
/// 値の設定。
/// 既に存在する場合は(解放処理とともに)上書き、存在しない場合は追加される。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <param name="value">値。</param>
/// <param name="need_release">解放が必要か</param>
/// <returns>設定されたペア情報。</returns>
MAP_PAIR* set_map(MAP* map, const TEXT* key, void* value, bool need_release);
/// <summary>
/// 削除。
/// 詰め処理が行われているため呼び出し前の<c>MAP_PAIR*</c>は使用不可になる。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <returns>削除の成功状態。</returns>
bool remove_map(MAP* map, const TEXT* key);

/// <summary>
/// 取得。
/// </summary>
/// <param name="map">対象マップ。</param>
/// <param name="key">キー。</param>
/// <returns>取得データ。</returns>
MAP_RESULT_VALUE get_map(const MAP* map, const TEXT* key);

