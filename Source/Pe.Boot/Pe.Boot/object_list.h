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
    OBJECT_LIST_ITEM* buffer;
    struct
    {
        byte_t capacity;
        func_free_object_list_value free_object_list_value;
    } library;
} OBJECT_LIST;

/// <summary>
/// リストの値解放不要処理。
/// </summary>
/// <param name="value"></param>
void free_object_list_value_null(void* value);

OBJECT_LIST RC_HEAP_FUNC(create_object_list, size_t capacity, func_free_object_list_value free_object_list_value);
#ifdef RES_CHECK
#   define create_object_list(capacity, free_object_list_value) RC_HEAP_WRAP(create_object_list, (capacity), (free_object_list_value))
#endif

bool RC_HEAP_FUNC(free_object_list, OBJECT_LIST* object_list);
#ifdef RES_CHECK
#   define free_object_list(object_list) RC_HEAP_WRAP(free_object_list, (object_list))
#endif
