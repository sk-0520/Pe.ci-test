#pragma once
#include <stdbool.h>
#include <stddef.h>

#include "common.h"
#include "debug.h"
#include "res_check.h"

/* ヒープ上に連続した値を保持する */

/// <summary>
/// デフォルト予約数。
/// </summary>
#define OBJECT_LIST_DEFAULT_CAPACITY_COUNT (64)

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
typedef int (*func_compare_object_list_value)(const void* a, const void* b);

/// <summary>
/// 格納値解放処理。
/// </summary>
typedef void (*func_free_object_list_value)(void* value);

/// <summary>
/// 値連続処理。
/// </summary>
/// <param name="value">現在値。</param>
/// <param name="index">現在処理数。</param>
/// <param name="length">最大件数。</param>
/// <param name="data">ご自由にどうぞ。</param>
/// <returns>継続状態。</returns>
typedef bool (*func_foreach_object_list)(const void* value, size_t index, size_t length, void* data);

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
    struct
    {
        /// <summary>
        /// 格納アイテムのサイズ。
        /// </summary>
        byte_t item_size;
        /// <summary>
        /// 容量。
        /// </summary>
        byte_t capacity;
        func_compare_object_list_value compare_object_list_value;
        func_free_object_list_value free_object_list_value;
    } library;
} OBJECT_LIST;

/// <summary>
/// リストの値比較なし処理。
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
int compare_object_list_value_null(const void* a, const void* b);
/// <summary>
/// リストの値解放不要処理。
/// </summary>
/// <param name="value"></param>
void free_object_list_value_null(void* value);

/// <summary>
/// オブジェクトリストの生成。
/// <para>各アイテムはヒープ上で管理され、ヒープの制御はライブラリ側で行う。</para>
/// </summary>
/// <param name="item_size">アイテムのサイズ。</param>
/// <param name="capacity_count">容量をアイテム数で指定。</param>
/// <param name="compare_object_list_value">比較処理。</param>
/// <param name="free_object_list_value">実データの解放処理。オブジェクトリストの解放や、アイテム変更時に使用される。</param>
/// <returns>生成したオブジェクトリスト。</returns>
OBJECT_LIST RC_HEAP_FUNC(create_object_list, byte_t item_size, size_t capacity_count, func_compare_object_list_value compare_object_list_value, func_free_object_list_value free_object_list_value);
#ifdef RES_CHECK
#   define create_object_list(item_size, capacity_count, compare_object_list_value, free_object_list_value) RC_HEAP_WRAP(create_object_list, (item_size), (capacity_count), (compare_object_list_value), (free_object_list_value))
#endif

/// <summary>
/// オブジェクトリストの解放。
/// </summary>
/// <param name=""></param>
/// <param name="object_list"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(free_object_list, OBJECT_LIST* object_list);
#ifdef RES_CHECK
#   define free_object_list(object_list) RC_HEAP_WRAP(free_object_list, (object_list))
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
/// <param name="need_release">設定前のデータを解放するか。</param>
/// <returns>成功状態。</returns>
bool set_object_list(OBJECT_LIST* object_list, size_t index, void* value, bool need_release);

/// <summary>
/// リストを空にする。
/// <para>領域自体はそのまま残る点に注意。</para>
/// </summary>
/// <param name="list">対象リスト。</param>
void clear_object_list(OBJECT_LIST* object_list);

/// <summary>
/// 順々に処理する。
/// </summary>
/// <param name="object_list"></param>
/// <param name="func"></param>
/// <returns>処理件数。</returns>
size_t foreach_object_list(const OBJECT_LIST* object_list, func_foreach_object_list func, void* data);

