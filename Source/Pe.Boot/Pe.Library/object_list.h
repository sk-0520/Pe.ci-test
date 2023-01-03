#pragma once
#include <stdbool.h>
#include <stddef.h>

#include "common.h"
#include "memory.h"
#include "debug.h"
#include "res_check.h"

/* ヒープ上に連続した値を保持する */

/// <summary>
/// デフォルト予約数。
/// </summary>
#define OBJECT_LIST_DEFAULT_CAPACITY_COUNT (64)

/// <summary>
/// (値)リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="TYPE">型</param>
/// <param name="LIST">対象リスト</param>
#define reference_value_object_list(TYPE, LIST) (const TYPE*)(LIST.items)
/// <summary>
/// (ポインタ)リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// <para>reference_value_object_listとやってることは同じだけど * 付きで呼び出すのもなぁという思いで作っただけ。</para>
/// </summary>
/// <param name="TYPE">型</param>
/// <param name="LIST">対象リスト</param>
#define reference_ref_object_list(TYPE, LIST) (const TYPE*)(LIST->items)

/// <summary>
/// 値ラッパー。
/// </summary>
typedef struct tag_OBJECT_RESULT_VALUE
{
    /// <summary>
    /// 格納されている値。
    /// </summary>
    void* value;
    /// <summary>
    /// 値は取得できたか。
    /// </summary>
    bool exists;
} OBJECT_RESULT_VALUE;

/// <summary>
/// 格納値比較処理。
/// </summary>
typedef int (*func_compare_object_list_value)(const void* a, const void* b, void* data);

/// <summary>
/// 格納値解放処理。
/// </summary>
typedef void (*func_release_object_list_value)(void* value, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource);

typedef struct tag_OBJECT_LIST
{
    /// <summary>
    /// 型に合わせた長さ。
    /// </summary>
    size_t length;
    /// <summary>
    /// 実体。
    /// </summary>
    uint8_t* items;
    /// <summary>
    /// 各種関数ポインタに渡される追加情報。
    /// </summary>
    void* data;
    struct
    {
        /// <summary>
        /// メモリリソース。
        /// </summary>
        const MEMORY_ARENA_RESOURCE* memory_arena_resource;
        /// <summary>
        /// 格納アイテムのサイズ。
        /// </summary>
        byte_t item_size;
        /// <summary>
        /// 容量。
        /// </summary>
        byte_t capacity;
        func_compare_object_list_value compare_object_list_value;
        func_release_object_list_value release_object_list_value;
    } library;
} OBJECT_LIST;

/// <summary>
/// リストの値比較なし処理。
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
int compare_object_list_value_null(const void* a, const void* b, void* data);
/// <summary>
/// リストの値解放不要処理。
/// </summary>
/// <param name="value"></param>
void release_object_list_value_null(void* value, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource);

int compare_object_list_value_text(const void* a, const void* b, void* data);

void release_object_list_value_text(void* target, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// オブジェクトリストの生成。
/// <para>各アイテムはヒープ上で管理され、ヒープの制御はライブラリ側で行う。</para>
/// </summary>
/// <param name="item_size">アイテムのサイズ。</param>
/// <param name="capacity_count">容量をアイテム数で指定。</param>
/// <param name="compare_object_list_value">比較処理。</param>
/// <param name="release_object_list_value">実データの解放処理。オブジェクトリストの解放や、アイテム変更時に使用される。</param>
/// <returns>生成したオブジェクトリスト。</returns>
OBJECT_LIST RC_HEAP_FUNC(new_object_list, byte_t item_size, size_t capacity_count, void* data, func_compare_object_list_value compare_object_list_value, func_release_object_list_value release_object_list_value, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define new_object_list(item_size, capacity_count, data, compare_object_list_value, release_object_list_value, memory_arena_resource) RC_HEAP_WRAP(new_object_list, (item_size), (capacity_count), (data), (compare_object_list_value), (release_object_list_value), memory_arena_resource)
#endif

/// <summary>
/// オブジェクトリストの解放。
/// </summary>
/// <param name=""></param>
/// <param name="object_list"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(release_object_list, OBJECT_LIST* object_list, bool value_release);
#ifdef RES_CHECK
#   define release_object_list(object_list, value_release) RC_HEAP_WRAP(release_object_list, (object_list), (value_release))
#endif

/// <summary>
/// 値の追加。
/// </summary>
/// <param name="object_list">追加対象のリスト。</param>
/// <param name="value">値のポインタ。この領域からアイテムバイト数がリストに複製される。</param>
/// <returns>複製後の領域。</returns>
void* push_object_list(OBJECT_LIST* object_list, const void* value);

/// <summary>
/// 値の連続追加。
/// </summary>
/// <param name="object_list">追加対象のリスト。</param>
/// <param name="values">値の配列。この領域からアイテムバイト数がリストに複製される。</param>
/// <param name="length">valuesの長さ。</param>
/// <returns>成功状態。</returns>
bool add_range_object_list(OBJECT_LIST* object_list, const void* values, size_t length);

/// <summary>
/// 最後尾の値を取り出し。
/// </summary>
/// <param name="result">最後尾の値を格納するポインタ。解放処理は行われないことに注意。</param>
/// <param name="object_list">取得対象のリスト。成功時に格納数は-1される。</param>
/// <returns>成功状態。</returns>
bool pop_object_list(void* result, OBJECT_LIST* object_list);

/// <summary>
/// 最後尾の値を参照。
/// </summary>
/// <param name="object_list">取得対象のリスト。</param>
/// <returns>値が参照できる場合にその領域。参照できない場合は<c>NULL</c>。</returns>
void* peek_object_list(OBJECT_LIST* object_list);

/// <summary>
/// 値の取得。
/// </summary>
/// <param name="object_list">取得対象のリスト。</param>
/// <param name="index">取得対象インデックス。</param>
/// <returns>値ラッパー。</returns>
OBJECT_RESULT_VALUE get_object_list(const OBJECT_LIST* object_list, size_t index);

/// <summary>
/// 値の設定。
/// </summary>
/// <param name="object_list">設定対象のリスト。</param>
/// <param name="index">設定対象インデックス。</param>
/// <param name="value">値のポインタ。この領域からアイテムバイト数がリストに複製される。</param>
/// <param name="value_release">設定前の値を解放するか。</param>
/// <returns>成功状態。</returns>
bool set_object_list(OBJECT_LIST* object_list, size_t index, void* value, bool value_release);

/// <summary>
/// リストを空にする。
/// <para>領域自体はそのまま残る点に注意。</para>
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value_release">値を解放するか。</param>
void clear_object_list(OBJECT_LIST* object_list, bool value_release);
