#pragma once
#include <stdbool.h>
#include <stddef.h>

#include "common.h"
#include "res_check.h"

#define OBJECT_LIST_DEFAULT_CAPACITY (64)

typedef struct tag_OBJECT_LIST_ITEM
{
    void* value;
    struct
    {
        /// <summary>
        /// 値の開放は必要か。
        /// </summary>
        bool need_release;
    } library;
} OBJECT_LIST_ITEM;

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

typedef struct tag_OBJECT_LIST_RANGE_ITEM
{
    void* value;
} OBJECT_LIST_RANGE_ITEM;

/// <summary>
/// 格納値比較処理。
/// </summary>
typedef int (*func_compare_object_list_value)(const void* a, const void* b);

/// <summary>
/// 格納値解放処理。
/// </summary>
typedef void (*func_free_object_list_value)(void* value);

typedef struct tag_OBJECT_LIST
{
    /// <summary>
    /// 型に合わせた長さ。
    /// </summary>
    size_t length;
    /// <summary>
    /// 実体。
    /// </summary>
    OBJECT_LIST_ITEM* items;
    struct
    {
        byte_t capacity;
        func_compare_object_list_value compare_object_list_value;
        func_free_object_list_value free_object_list_value;
    } library;
} OBJECT_LIST;

int compare_object_list_value_null(const void* a, const void* b);
/// <summary>
/// リストの値解放不要処理。
/// </summary>
/// <param name="value"></param>
void free_object_list_value_null(void* value);

OBJECT_LIST RC_HEAP_FUNC(create_object_list, size_t capacity, func_compare_object_list_value compare_object_list_value, func_free_object_list_value free_object_list_value);
#ifdef RES_CHECK
#   define create_object_list(capacity, compare_object_list_value, free_object_list_value) RC_HEAP_WRAP(create_object_list, (capacity), (compare_object_list_value), (free_object_list_value))
#endif

bool RC_HEAP_FUNC(free_object_list, OBJECT_LIST* object_list);
#ifdef RES_CHECK
#   define free_object_list(object_list) RC_HEAP_WRAP(free_object_list, (object_list))
#endif

/// <summary>
/// リストに対して値追加。
/// </summary>
/// <param name="value">値。</param>
/// <param name="need_release">解放が必要か。</param>
/// <returns>追加したアイテム。失敗時はNULL。</returns>
OBJECT_LIST_ITEM* push_object_list(OBJECT_LIST* object_list, void* value, bool need_release);

bool add_range_object_list(OBJECT_LIST* object_list, OBJECT_LIST_RANGE_ITEM items[], size_t length, bool need_release);

bool pop_object_list(void** result, OBJECT_LIST* object_list);

OBJECT_RESULT_VALUE get_object_list(OBJECT_LIST* object_list, size_t index);

bool set_object_list(OBJECT_LIST* object_list, size_t index, void* value, bool need_release);

/// <summary>
/// リストを空にする。
/// <para>領域自体はそのまま残る点に注意。</para>
/// </summary>
/// <param name="list">対象リスト。</param>
void clear_object_list(OBJECT_LIST* list);
