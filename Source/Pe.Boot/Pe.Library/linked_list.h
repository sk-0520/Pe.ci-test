#pragma once
#include <stddef.h>

#include "common.h"
#include "memory.h"
#include "res_check.h"
#include "object_list.h"

/*
* 双方向リスト
* 値の保持まで担当する。
*/

/// <summary>
/// 連結リストの格納値比較処理。
/// </summary>
typedef int (*func_compare_linked_list_value)(const void* a, const void* b, void* data);

/// <summary>
/// 連結リストの格納値解放処理。
/// </summary>
typedef void (*func_release_linked_list_value)(void* value, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// 値連続処理。
/// </summary>
/// <param name="value">現在値。</param>
/// <param name="index">現在処理数。</param>
/// <param name="length">最大件数。</param>
/// <param name="arg">ご自由にどうぞ。</param>
/// <returns>継続状態。</returns>
typedef bool (*func_foreach_linked_list)(const void* value, size_t index, size_t length, void* data, void* arg);

typedef bool (*func_search_linked_list)(const void* needle, const void* value, void* data, void* arg);

/// <summary>
/// 連結リストノード。
/// <para>アプリケーション側からさわることはない。</para>
/// </summary>
typedef struct tag_LINK_NODE LINK_NODE;

/// <summary>
/// 連結リスト。
/// </summary>
typedef struct tag_LINKED_LIST
{
    /// <summary>
    /// 長さ。
    /// </summary>
    size_t length;
    /// <summary>
    /// 各種関数ポインタに渡される追加情報。
    /// </summary>
    void* data;
    struct
    {
        const MEMORY_ARENA_RESOURCE* value_memory_resource;
        const MEMORY_ARENA_RESOURCE* linked_list_memory_resource;
        /// <summary>
        /// 先頭ノード。
        /// <para>未格納時は<see cref="NULL" /></para>
        /// </summary>
        LINK_NODE* head;
        /// <summary>
        /// 終端ノード。
        /// <para>未格納時は<see cref="NULL" /></para>
        /// </summary>
        LINK_NODE* tail;
        func_compare_linked_list_value compare_linked_list_value;
        func_release_linked_list_value release_linked_list_value;
        /// <summary>
        /// 格納値のサイズ。
        /// </summary>
        byte_t value_bytes;
    } library;
} LINKED_LIST;

/// <summary>
/// 値ラッパー。
/// </summary>
typedef struct tag_LINK_RESULT_VALUE
{
    /// <summary>
    /// 格納されている値。
    /// </summary>
    void* value;
    /// <summary>
    /// 値は取得できたか。
    /// </summary>
    bool exists;
    /// <summary>
    /// 対象ノード。
    /// </summary>
    LINK_NODE* node;
} LINK_RESULT_VALUE;

/// <summary>
/// 連結リストのノードから値を取得。
/// <para><c>LINK_NODE</c>自体が公開されていないので取得する場合はこれを使うこと。</para>
/// </summary>
/// <param name="node"></param>
/// <returns></returns>
const void* get_link_node_value(const LINK_NODE* node);

/// <summary>
/// 連結リストの値比較なし処理。
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
int compare_linked_list_value_null(const void* a, const void* b, void* data);
/// <summary>
/// 連結リストの値解放不要処理。
/// </summary>
/// <param name="value"></param>
void release_linked_list_value_null(void* value, void* data, const MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// 連結リストの生成。
/// </summary>
/// <param name="item_size"></param>
/// <param name="compare_linked_list_value"></param>
/// <param name="release_linked_list_value"></param>
/// <param name="value_memory_resource"></param>
/// <param name="linked_list_memory_resource"></param>
/// <returns>解放が必要。</returns>
LINKED_LIST RC_HEAP_FUNC(new_linked_list, byte_t item_size, void* data, func_compare_linked_list_value compare_linked_list_value, func_release_linked_list_value release_linked_list_value, const MEMORY_ARENA_RESOURCE* value_memory_resource, const MEMORY_ARENA_RESOURCE* linked_list_memory_resource);
#ifdef RES_CHECK
#   define new_linked_list(item_size, data, compare_linked_list_value, release_linked_list_value, value_memory_resource, linked_list_memory_resource) RC_HEAP_WRAP(new_linked_list, (item_size), (data), (compare_linked_list_value), (release_linked_list_value), value_memory_resource, linked_list_memory_resource)
#endif

/// <summary>
/// 連結リストの解放。
/// </summary>
/// <param name="linked_list"></param>
/// <param name="value_release">値の解放処理を行うか。</param>
/// <returns></returns>
bool RC_HEAP_FUNC(release_linked_list, LINKED_LIST* linked_list, bool value_release);
#ifdef RES_CHECK
#   define release_linked_list(linked_list, value_release) RC_HEAP_WRAP(release_linked_list, (linked_list), (value_release))
#endif

/// <summary>
/// 連結リストに対して値追加。
/// </summary>
/// <param name=""></param>
/// <param name="linked_list"></param>
/// <param name="value"></param>
/// <returns>値。</returns>
void* RC_HEAP_FUNC(add_linked_list, LINKED_LIST* linked_list, void* value);
#ifdef RES_CHECK
#   define add_linked_list(linked_list, value) RC_HEAP_WRAP(add_linked_list, (linked_list), (value))
#endif

/// <summary>
/// 連結リストの値を取得。
/// </summary>
/// <param name="linked_list"></param>
/// <param name="index"></param>
/// <returns></returns>
LINK_RESULT_VALUE get_linked_list(const LINKED_LIST* linked_list, size_t index);

/// <summary>
/// 連結リストに対して値を挿入。
/// </summary>
/// <param name="linked_list"></param>
/// <param name="index">挿入する位置。</param>
/// <param name="value"></param>
/// <returns>挿入成功状態。</returns>
bool RC_HEAP_FUNC(insert_linked_list, LINKED_LIST* linked_list, size_t index, void* value);
#ifdef RES_CHECK
#   define insert_linked_list(linked_list, index, value) RC_HEAP_WRAP(insert_linked_list, (linked_list), (index), (value))
#endif

bool RC_HEAP_FUNC(remove_linked_list, LINKED_LIST* linked_list, size_t index);
#ifdef RES_CHECK
#   define remove_linked_list(linked_list, index) RC_HEAP_WRAP(remove_linked_list, (linked_list), (index))
#endif

bool RC_HEAP_FUNC(set_linked_list, LINKED_LIST* linked_list, size_t index, void* value, bool need_release);
#ifdef RES_CHECK
#   define set_linked_list(linked_list, index, value, need_release) RC_HEAP_WRAP(set_linked_list, (linked_list), (index), (value), (need_release))
#endif

/// <summary>
/// 順々に処理する。
/// </summary>
/// <param name="linked_list"></param>
/// <param name="func"></param>
/// <returns>処理件数。</returns>
size_t foreach_linked_list(const LINKED_LIST* linked_list, func_foreach_linked_list func, void* arg);

const LINK_NODE* search_linked_list(const LINKED_LIST* linked_list, const void* needle, void* arg, func_search_linked_list func);

bool RC_HEAP_FUNC(remove_linked_list_by_node, LINKED_LIST* linked_list, LINK_NODE* node);
#ifdef RES_CHECK
#   define remove_linked_list_by_node(linked_list, node) RC_HEAP_WRAP(remove_linked_list_by_node, linked_list, node)
#endif

OBJECT_LIST RC_HEAP_FUNC(to_object_list_from_linked_list, const LINKED_LIST* linked_list);
#ifdef RES_CHECK
#   define to_object_list_from_linked_list(linked_list) RC_HEAP_WRAP(to_object_list_from_linked_list, (linked_list))
#endif

