#pragma once
#include <stdint.h>

#include "res_check.h"

typedef enum tag_PRIMITIVE_LIST_TYPE
{
    PRIMITIVE_LIST_TYPE_UINT8,
    PRIMITIVE_LIST_TYPE_UINT16,
    PRIMITIVE_LIST_TYPE_UINT32,
} PRIMITIVE_LIST_TYPE;

typedef struct tag_PRIMITIVE_LIST
{
    /// <summary>
    /// 型に合わせた長さ。
    /// </summary>
    size_t length;
    /// <summary>
    /// 実体。
    /// </summary>
    void* buffer;
    struct
    {
        /// <summary>
        /// 型。
        /// </summary>
        PRIMITIVE_LIST_TYPE type;
        /// <summary>
        /// 確保済みサイズ(バイト幅)。
        /// </summary>
        size_t capacity_bytes;
    } library;

} PRIMITIVE_LIST;

typedef PRIMITIVE_LIST UINT8_LIST;
typedef PRIMITIVE_LIST UINT16_LIST;
typedef PRIMITIVE_LIST UINT32_LIST;

/// <summary>
/// リストの生成。
/// </summary>
/// <param name="list_type"></param>
/// <param name="capacity"></param>
/// <returns></returns>
PRIMITIVE_LIST RC_HEAP_FUNC(new_primitive_list, PRIMITIVE_LIST_TYPE list_type, size_t capacity);
#ifdef RES_CHECK
#   define new_primitive_list(list_type, capacity) RC_HEAP_WRAP(new_primitive_list, (list_type), (capacity))
#endif


bool RC_HEAP_FUNC(free_primitive_list, PRIMITIVE_LIST* list);
#ifdef RES_CHECK
#   define free_primitive_list(list) RC_HEAP_WRAP(free_primitive_list, (list))
#endif

#define PUSH_PRIMITIVE_LIST_FUNC(list_type, function, value_type) bool push_ ##function ##_list(list_type* list, value_type value)

PUSH_PRIMITIVE_LIST_FUNC(UINT8_LIST, uint8, uint8_t);
PUSH_PRIMITIVE_LIST_FUNC(UINT16_LIST, uint16, uint16_t);
PUSH_PRIMITIVE_LIST_FUNC(UINT32_LIST, uint32, uint32_t);

#define GET_PRIMITIVE_LIST_FUNC(list_type, function, value_type) bool get_ ##function ##_list(value_type* result, list_type* list, size_t index)
GET_PRIMITIVE_LIST_FUNC(UINT8_LIST, uint8, uint8_t);
GET_PRIMITIVE_LIST_FUNC(UINT16_LIST, uint16, uint16_t);
GET_PRIMITIVE_LIST_FUNC(UINT32_LIST, uint32, uint32_t);

#define REFERENCE_PRIMITIVE_LIST_FUNC(list_type, function, value_type) value_type* reference_ ##function ##_list(list_type* list)
REFERENCE_PRIMITIVE_LIST_FUNC(UINT8_LIST, uint8, uint8_t);
REFERENCE_PRIMITIVE_LIST_FUNC(UINT16_LIST, uint16, uint16_t);
REFERENCE_PRIMITIVE_LIST_FUNC(UINT32_LIST, uint32, uint32_t);
