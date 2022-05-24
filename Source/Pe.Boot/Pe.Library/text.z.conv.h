#pragma once
#include "text.h"

#ifdef _UNICODE

typedef enum tag_MULTIBYTE_CHARACTER_TYPE
{
    MULTI_BYTE_CHARACTER_TYPE_SYSTEM = CP_ACP,
    //MULTI_BYTE_CHARACTER_TYPE_UTF7 = CP_UTF7,
    MULTI_BYTE_CHARACTER_TYPE_UTF8 = CP_UTF8,
    MULTI_BYTE_CHARACTER_TYPE_SJIS = 932,
} MULTIBYTE_CHARACTER_TYPE;

typedef struct tag_MULTIBYTE_CHARACTER_RESULT
{
    uint8_t* buffer;
    size_t length;
} MULTIBYTE_CHARACTER_RESULT;

bool is_enabled_multibyte_character_result(const MULTIBYTE_CHARACTER_RESULT* mbcr);

/// <summary>
///
/// </summary>
/// <param name="input"></param>
/// <param name="convert_type"></param>
/// <returns>変換データ。解放が必要。</returns>
MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define convert_to_multibyte_character(input, convert_type, memory_resource) RC_HEAP_WRAP(convert_to_multibyte_character, (input), (convert_type), memory_resource)
#endif

bool RC_HEAP_FUNC(release_multibyte_character_result, MULTIBYTE_CHARACTER_RESULT* mbcr, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define release_multibyte_character_result(mbcr, memory_resource) RC_HEAP_WRAP(release_multibyte_character_result, (mbcr), memory_resource)
#endif

/// <summary>
///
/// </summary>
/// <param name="input"></param>
/// <param name="length"></param>
/// <param name="mbc_type"></param>
/// <returns>解放が必要。</returns>
TEXT RC_HEAP_FUNC(make_text_from_multibyte, const uint8_t* input, size_t length, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define make_text_from_multibyte(input, length, mbc_type, memory_resource) RC_HEAP_WRAP(make_text_from_multibyte, (input), (length), (mbc_type), memory_resource)
#endif

#endif

