
#pragma once
/* 自動生成: text.z.parse.h.tt */
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

