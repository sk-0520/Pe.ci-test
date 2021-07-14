#pragma once
#include "common.h"
#include "tstring.h"

/// <summary>
/// 不変文字列ラッパー。
/// </summary>
typedef struct _TAG_TEXT
{
    /// <summary>
    /// 値。
    /// </summary>
    const TCHAR* value;
    /// <summary>
    /// 長さ。
    /// </summary>
    size_t length;

    /// <summary>
    /// 管理データ。
    /// </summary>
    struct
    {
        /// <summary>
        /// 解放が必要か。
        /// <para>アプリケーション内では使用しない。</para>
        /// </summary>
        bool needRelease : 1;
        /// <summary>
        /// 解放済みか。
        /// <para>アプリケーション内では使用しない。</para>
        /// </summary>
        bool released : 1;
    } library;
} TEXT;

/// <summary>
/// 空の不変文字列を生成。
/// </summary>
/// <returns>領域自体がNULLの不変文字列(通常使用は出来ない)。</returns>
TEXT createEmptyText();

/// <summary>
/// 不変文字列が使用可能か。
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
bool isEnabledText(const TEXT* text);

/// <summary>
/// 不変文字列を生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <param name="length">対象文字列の長さ。</param>
/// <returns>不変文字列。</returns>
TEXT newTextWithLength(const TCHAR* source, size_t length);

/// <summary>
/// 不変文字列を生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>不変文字列。</returns>
TEXT newText(const TCHAR* source);

/// <summary>
/// 文字列から不変文字列にラップ。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <param name="length">対象文字列の長さ。</param>
/// <param name="needRelease">解放が必要か。真の場合、<c>wrapText</c>と異なり呼び出し側で確保した領域を信頼して持ち運ぶ。</param>
/// <returns></returns>
TEXT wrapTextWithLength(const TCHAR* source, size_t length, bool needRelease);

/// <summary>
/// 文字列から不変文字列にラップ。
/// <para>スタック内で元文字列を変更せずに使用することが前提条件。</para>
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>不変文字列。</returns>
TEXT wrapText(const TCHAR* source);


/// <summary>
/// 不変文字列の複製。
/// </summary>
/// <param name="source">入力不変文字列。</param>
/// <returns>複製された不変文字列。</returns>
TEXT cloneText(const TEXT* source);

/// <summary>
/// 不変文字列を参照として複製。
/// </summary>
/// <param name="source">入力不変文字列。</param>
/// <returns>参照として複製された不変文字列。参照元が生きている限り生きている。解放不要。</returns>
TEXT referenceText(const TEXT* source);

/// <summary>
/// 不変文字列の解放。
/// <para>不要な場合は処理しない。</para>
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
bool freeText(TEXT* text);

