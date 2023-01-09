#pragma once
#include "text.h"
#include "object_list.h"
#include "linked_list.h"

/*
* ハッシュマップ。
* 値は連結リスト(linked_list.h)で管理する。
*/

/// <summary>
/// 標準の予約数。
/// </summary>
#define MAP_DEFAULT_CAPACITY (32)
/// <summary>
/// 標準の負荷係数。
/// </summary>
#define MAP_DEFAULT_LOAD_FACTOR ((real_t)0.75)

/// <summary>
/// マップキーのハッシュ値作成処理。
/// </summary>
typedef size_t (*func_calc_map_hash)(const TEXT* key);
/// <summary>
/// マップキー比較処理。
/// </summary>
typedef bool (*func_equals_map_key)(const TEXT* a, const TEXT* b);
/// <summary>
/// マップの値解放処理。
/// </summary>
typedef void (*func_release_map_value)(const TEXT* key, void* value, const MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// キーと値のペア。
/// <para><see cref="MAP"/>で管理される。</para>
/// <para>NOTE: ハッシュテーブル再構築時にハッシュ値生成の負荷が無視できなくなったらここに計算済みの値を持たす予定(多分大丈夫でしょ)。</para>
/// </summary>
typedef struct tag_KEY_VALUE_PAIR
{
    /// <summary>
    /// キー。
    /// </summary>
    TEXT key;
    /// <summary>
    /// 値。
    /// </summary>
    void* value;
} KEY_VALUE_PAIR;

/// <summary>
/// 値ラッパー。
/// </summary>
typedef struct tag_MAP_RESULT_VALUE
{
    /// <summary>
    /// 格納されている値。
    /// </summary>
    const void* value;
    /// <summary>
    /// 値は取得できたか。
    /// </summary>
    bool exists;
} MAP_RESULT_VALUE;


typedef struct tag_MAP
{
    /// <summary>
    /// 格納数。
    /// </summary>
    size_t length;
    struct
    {
        byte_t value_bytes;
        /// <summary>
        /// 倍々ゲームのマスク。
        /// </summary>
        size_t mask;
        /// <summary>
        /// この数を超過した際に拡張
        /// </summary>
        size_t next_limit;
        /// <summary>
        /// 負荷係数。
        /// </summary>
        real_t load_factor;
        /// <summary>
        /// キーをハッシュ関数で割り当てる領域を保持する<see cref="LINKED_LIST" />のかたまり。
        /// <para>(なに書いてんのか分からん)</para>
        /// </summary>
        LINKED_LIST*/*KEY_VALUE_PAIR*/ items;
        /// <summary>
        /// <see cref="MAP" />で使用する<see cref="MEMORY_ARENA_RESOURCE" />
        /// </summary>
        const MEMORY_ARENA_RESOURCE* map_memory_resource;
        const MEMORY_ARENA_RESOURCE* value_memory_resource;
        func_release_linked_list_value release_map_value;
        func_calc_map_hash calc_map_hash;
        func_equals_map_key equals_map_key;
#ifdef RES_CHECK
        /// <summary>
        /// 再構築中か。
        /// <para></para>
        /// </summary>
        bool now_rehash;
#endif
    } library;
} MAP;

/// <summary>
/// 値連続処理。
/// </summary>
/// <param name="pair">ペア。</param>
/// <param name="index">現在処理数。</param>
/// <param name="length">最大件数。</param>
/// <param name="arg">ご自由にどうぞ。</param>
/// <returns>継続状態。</returns>
typedef bool (*func_foreach_map)(const KEY_VALUE_PAIR* pair, size_t index, size_t length, void* arg);

size_t calc_map_hash_default(const TEXT* key);
/// <summary>
/// キー項目比較の標準処理。
/// <para>大文字小文字を区別する通常の文字列比較。</para>
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
bool equals_map_key_default(const TEXT* a, const TEXT* b);

/// <summary>
/// マップの生成。
/// </summary>
/// <param name="item_size"></param>
/// <param name="capacity"></param>
/// <param name="load_factor"></param>
/// <param name="release_linked_list_value"></param>
/// <param name="calc_map_hash"></param>
/// <param name="equals_map_key"></param>
/// <param name="value_memory_resource"></param>
/// <param name="map_memory_resource"></param>
/// <returns>解放が必要。</returns>
MAP RC_HEAP_FUNC(new_map, byte_t item_size, size_t capacity, real_t load_factor, func_release_linked_list_value release_linked_list_value, func_calc_map_hash calc_map_hash, func_equals_map_key equals_map_key, const MEMORY_ARENA_RESOURCE* value_memory_resource, const MEMORY_ARENA_RESOURCE* map_memory_resource);
#ifdef RES_CHECK
#   define new_map(item_size, capacity, load_factor, release_linked_list_value, calc_map_hash, equals_map_key, value_memory_resource, map_memory_resource) RC_HEAP_WRAP(new_map, item_size, capacity, load_factor, release_linked_list_value, calc_map_hash, equals_map_key, value_memory_resource, map_memory_resource)
#endif

/// <summary>
/// マップの解放。
/// </summary>
/// <param name=""></param>
/// <param name="map"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(release_map, MAP* map);
#ifdef RES_CHECK
#   define release_map(map) RC_HEAP_WRAP(release_map, map)
#endif

/// <summary>
/// マップから値の取得。
/// </summary>
/// <param name="map"></param>
/// <param name="key"></param>
/// <returns></returns>
MAP_RESULT_VALUE get_map(const MAP* map, const TEXT* key);

/// <summary>
/// 値を追加。
/// <para>既に存在する場合は失敗する。</para>
/// </summary>
/// <param name="map"></param>
/// <param name="key"></param>
/// <param name="value"></param>
/// <returns>追加成功状態。</returns>
bool RC_HEAP_FUNC(add_map, MAP* map, const TEXT* key, void* value);
#ifdef RES_CHECK
#   define add_map(map, key, value) RC_HEAP_WRAP(add_map, map, key, value)
#endif

/// <summary>
/// 値の設定。
/// <para>既に存在していても存在していなくても設定処理が行われる。</para>
/// </summary>
/// <param name="map"></param>
/// <param name="key"></param>
/// <param name="value"></param>
void RC_HEAP_FUNC(set_map, MAP* map, const TEXT* key, void* value);
#ifdef RES_CHECK
#   define set_map(map, key, value) RC_HEAP_WRAP(set_map, map, key, value)
#endif

/// <summary>
/// 指定したキー項目を破棄。
/// </summary>
/// <param name="map"></param>
/// <param name="key"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(remove_map, MAP* map, const TEXT* key);
#ifdef RES_CHECK
#   define remove_map(map, key) RC_HEAP_WRAP(remove_map, map, key)
#endif

/// <summary>
/// ぐるんぐるん回す
/// </summary>
/// <param name="map"></param>
/// <param name="func"></param>
/// <param name="arg"></param>
void foreach_map(const MAP* map, func_foreach_map func, void* arg);

