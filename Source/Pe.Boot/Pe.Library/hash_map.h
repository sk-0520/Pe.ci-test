#pragma once
#include "text.h"
#include "object_list.h"
#include "linked_list.h"

/*
* ハッシュマップ。
* 値は連結リスト(linked_list.h)で管理する。
*
* TODO: 名前 hash_map -> map に変更して既存のMAPと置き換える
*/

#define HASH_MAP_DEFAULT_CAPACITY (32)
#define HASH_MAP_DEFAULT_LOAD_FACTOR (0.75)

/// <summary>
/// マップキーのハッシュ値作成処理。
/// </summary>
typedef size_t (*func_calc_hash_map_hash)(const TEXT* key);
/// <summary>
/// マップキー比較処理。
/// </summary>
typedef bool (*func_equals_hash_map_key)(const TEXT* a, const TEXT* b);
/// <summary>
/// マップの値解放処理。
/// </summary>
typedef void (*func_release_hash_map_value)(const TEXT* key, void* value);



/// <summary>
/// キーと値のペア。
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
typedef struct tag_HASH_MAP_RESULT_VALUE
{
    /// <summary>
    /// 格納されている値。
    /// </summary>
    const void* value;
    /// <summary>
    /// 値は取得できたか。
    /// </summary>
    bool exists;
} HASH_MAP_RESULT_VALUE;


typedef struct tag_HASH_MAP
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
        double load_factor;
        /// <summary>
        /// キーをハッシュ関数で割り当てる領域を保持する<see cref="LINKED_LIST" />のかたまり。
        /// <para>(なに書いてんのか分からん)</para>
        /// </summary>
        LINKED_LIST*/*KEY_VALUE_PAIR*/ items;
        /// <summary>
        /// <see cref="HASH_MAP" />で使用する<see cref="MEMORY_RESOURCE" />
        /// </summary>
        const MEMORY_RESOURCE* map_memory_resource;
        const MEMORY_RESOURCE* value_memory_resource;
        func_release_hash_map_value release_hash_map_value;
        func_calc_hash_map_hash calc_map_hash;
        func_equals_hash_map_key equals_map_key;
#ifdef RES_CHECK
        /// <summary>
        /// 再構築中か。
        /// <para></para>
        /// </summary>
        bool now_rehash;
#endif
    } library;
} HASH_MAP;

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
bool equals_hash_map_key_default(const TEXT* a, const TEXT* b);

HASH_MAP RC_HEAP_FUNC(new_hash_map, byte_t item_size, size_t capacity, double load_factor, func_release_linked_list_value release_linked_list_value, func_calc_hash_map_hash calc_map_hash, func_equals_hash_map_key equals_map_key, const MEMORY_RESOURCE* value_memory_resource, const MEMORY_RESOURCE* map_memory_resource);
#ifdef RES_CHECK
#   define new_hash_map(item_size, capacity, load_factor, release_linked_list_value, calc_map_hash, equals_map_key, value_memory_resource, map_memory_resource) RC_HEAP_WRAP(new_hash_map, item_size, capacity, load_factor, release_linked_list_value, calc_map_hash, equals_map_key, value_memory_resource, map_memory_resource)
#endif

bool RC_HEAP_FUNC(release_hash_map, HASH_MAP* map);
#ifdef RES_CHECK
#   define release_hash_map(map) RC_HEAP_WRAP(release_hash_map, map)
#endif

HASH_MAP_RESULT_VALUE get_hash_map(HASH_MAP* map, const TEXT* key);

/// <summary>
/// 値を追加。
/// <para>既に存在する場合は失敗する。</para>
/// </summary>
/// <param name="map"></param>
/// <param name="key"></param>
/// <param name="value"></param>
/// <returns>追加成功状態。</returns>
bool RC_HEAP_FUNC(add_hash_map, HASH_MAP* map, const TEXT* key, void* value);
#ifdef RES_CHECK
#   define add_hash_map(map, key, value) RC_HEAP_WRAP(add_hash_map, map, key, value)
#endif

void RC_HEAP_FUNC(set_hash_map, HASH_MAP* map, const TEXT* key, void* value);
#ifdef RES_CHECK
#   define set_hash_map(map, key, value) RC_HEAP_WRAP(set_hash_map, map, key, value)
#endif

bool RC_HEAP_FUNC(remove_hash_map, HASH_MAP* map, const TEXT* key);
#ifdef RES_CHECK
#   define remove_hash_map(map, key) RC_HEAP_WRAP(remove_hash_map, map, key)
#endif

void foreach_map(const HASH_MAP* map, func_foreach_map func, void* arg);
