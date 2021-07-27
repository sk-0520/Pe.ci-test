#pragma once
#include "text.h"


/// <summary>
/// 数値(64bit幅)変換結果。
/// </summary>
typedef struct tag_TEXT_PARSED_INT32_RESULT
{
    /// <summary>
    /// 変換値。
    /// <para>successが真の場合に有効値が設定される。</para>
    /// </summary>
    __int32 value;
    /// <summary>
    /// 変換成功状態。
    /// </summary>
    bool success;
} TEXT_PARSED_INT32_RESULT;

/// <summary>
/// 数値(64bit幅)変換結果。
/// </summary>
typedef struct tag_TEXT_PARSED_INT64_RESULT
{
    /// <summary>
    /// 変換値。
    /// <para>successが真の場合に有効値が設定される。</para>
    /// </summary>
    __int64 value;
    /// <summary>
    /// 変換成功状態。
    /// </summary>
    bool success;
} TEXT_PARSED_INT64_RESULT;

/// <summary>
/// テキストを数値(32bit幅)に変換。
/// </summary>
/// <param name="input">入力テキスト。</param>
/// <param name="support_hex">16進数(0x)を考慮するか</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_INT32_RESULT parse_integer_from_text(const TEXT* input, bool support_hex);
/// <summary>
/// テキストを数値(64bit幅)に変換。
/// </summary>
/// <param name="input">入力テキスト。</param>
/// <param name="support_hex">16進数(0x)を考慮するか</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_INT64_RESULT parse_long_from_text(const TEXT* input, bool support_hex);

#ifdef _UNICODE

typedef enum tag_MULTIBYTE_CHARACTER_TYPE
{
    MULTI_BYTE_CHARACTER_TYPE_SYSTEM = CP_ACP,
    MULTI_BYTE_CHARACTER_TYPE_UTF7 = CP_UTF7,
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
/// <returns></returns>
MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE convert_type);
#ifdef RES_CHECK
#   define convert_to_multibyte_character(input, convert_type) RC_HEAP_WRAP(convert_to_multibyte_character, (input), (convert_type))
#endif

bool RC_HEAP_FUNC(free_multibyte_character_result, MULTIBYTE_CHARACTER_RESULT* mbcr);
#ifdef RES_CHECK
#   define free_multibyte_character_result(mbcr) RC_HEAP_WRAP(free_multibyte_character_result, (mbcr))
#endif



#endif

