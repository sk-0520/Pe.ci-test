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

typedef PRIMITIVE_LIST PRIMITIVE_LIST_UINT8;
typedef PRIMITIVE_LIST PRIMITIVE_LIST_UINT16;
typedef PRIMITIVE_LIST PRIMITIVE_LIST_UINT32;

/// <summary>
/// リストの生成。
/// </summary>
/// <param name="list_type"></param>
/// <param name="capacity"></param>
/// <returns>解放が必要。</returns>
PRIMITIVE_LIST RC_HEAP_FUNC(new_primitive_list, PRIMITIVE_LIST_TYPE list_type, size_t capacity);
#ifdef RES_CHECK
#   define new_primitive_list(list_type, capacity) RC_HEAP_WRAP(new_primitive_list, (list_type), (capacity))
#endif

/// <summary>
/// リストの解放。
/// </summary>
/// <param name="list"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(free_primitive_list, PRIMITIVE_LIST* list);
#ifdef RES_CHECK
#   define free_primitive_list(list) RC_HEAP_WRAP(free_primitive_list, (list))
#endif

bool push_list_uint8(PRIMITIVE_LIST_UINT8* list, uint8_t value);
bool push_list_uint16(PRIMITIVE_LIST_UINT16* list, uint16_t value);
bool push_list_uint32(PRIMITIVE_LIST_UINT32* list, uint32_t value);

bool pop_list_uint8(uint8_t* result, PRIMITIVE_LIST_UINT8* list);
bool pop_list_uint16(uint16_t* result, PRIMITIVE_LIST_UINT16* list);
bool pop_list_uint32(uint32_t* result, PRIMITIVE_LIST_UINT32* list);

bool get_list_uint8(uint8_t* result, PRIMITIVE_LIST_UINT8* list, size_t index);
bool get_list_uint16(uint16_t* result, PRIMITIVE_LIST_UINT16* list, size_t index);
bool get_list_uint32(uint32_t* result, PRIMITIVE_LIST_UINT32* list, size_t index);

uint8_t* reference_list_uint8(PRIMITIVE_LIST_UINT8* list);
uint16_t* reference_list_uint16(PRIMITIVE_LIST_UINT16* list);
uint32_t* reference_list_uint32(PRIMITIVE_LIST_UINT32* list);
