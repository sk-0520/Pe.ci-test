
#pragma once
/* 自動生成: text.z.parse.h.tt */

/// <summary>
/// 変換値の今のとこ受け入れ可能基数。
/// <para>多分普通の数値もいけると思うけど知らん。</para>
/// </summary>
typedef enum tag_PARSE_BASE_NUMBER
{
    /// <summary>
    /// 2進数。
    /// </summary>
    PARSE_BASE_NUMBER_B = 2,
    /// <summary>
    /// 8進数。
    /// </summary>
    PARSE_BASE_NUMBER_O = 8,
    /// <summary>
    /// 10進数。
    /// </summary>
    PARSE_BASE_NUMBER_D = 10,
    /// <summary>
    /// 16進数。
    /// </summary>
    PARSE_BASE_NUMBER_X = 16,
} PARSE_BASE_NUMBER;

/// <summary>
/// i32変換結果。
/// </summary>
typedef struct tag_TEXT_PARSED_I32_RESULT
{
    /// <summary>
    /// 変換値。
    /// <para>successが真の場合に有効値が設定される。</para>
    /// </summary>
    int32_t value;
    /// <summary>
    /// 変換成功状態。
    /// </summary>
    bool success;
} TEXT_PARSED_I32_RESULT;

#ifdef _WIN64
/// <summary>
/// i64変換結果。
/// </summary>
typedef struct tag_TEXT_PARSED_I64_RESULT
{
    /// <summary>
    /// 変換値。
    /// <para>successが真の場合に有効値が設定される。</para>
    /// </summary>
    int64_t value;
    /// <summary>
    /// 変換成功状態。
    /// </summary>
    bool success;
} TEXT_PARSED_I64_RESULT;
#endif



/// <summary>
/// テキストをi32に変換。
/// </summary>
/// <param name="input">入力テキスト。</param>
/// <param name="base">入力テキストをN進数として扱う(N=10=10進数)</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_I32_RESULT parse_i32_from_text(const TEXT* input, size_t base);

#ifdef _WIN64
/// <summary>
/// テキストをi64に変換。
/// </summary>
/// <param name="input">入力テキスト。</param>
/// <param name="base">入力テキストをN進数として扱う(N=10=10進数)</param>
/// <returns>結果データ。</returns>
TEXT_PARSED_I64_RESULT parse_i64_from_text(const TEXT* input, size_t base);
#endif

