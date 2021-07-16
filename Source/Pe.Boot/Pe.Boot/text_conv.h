#pragma once
#include "text.h"


/// <summary>
/// 空テキストの無視設定
/// </summary>
typedef enum tag_IGNORE_EMPTY
{
    /// <summary>
    /// 無視しない。
    /// </summary>
    IGNORE_EMPTY_NONE,
    /// <summary>
    /// 空を無視する。
    /// </summary>
    IGNORE_EMPTY_ONLY,
    /// <summary>
    /// 空白文字を無視する。
    /// </summary>
    IGNORE_EMPTY_WHITESPACE,
} IGNORE_EMPTY;

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
/// <param name="supportHex">16進数(0x)を考慮するか</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_INT32_RESULT parseInteger(const TEXT* input, bool supportHex);
/// <summary>
/// テキストを数値(64bit幅)に変換。
/// </summary>
/// <param name="input">入力テキスト。</param>
/// <param name="supportHex">16進数(0x)を考慮するか</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_INT64_RESULT parseLong(const TEXT* input, bool supportHex);
