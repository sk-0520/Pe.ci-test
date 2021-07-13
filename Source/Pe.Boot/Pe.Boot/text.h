#pragma once
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
    const size_t length;
    /// <summary>
    /// スタックに乗っているか。
    /// <para>乗っていない場合は解放が必要。</para>
    /// </summary>
    bool _needRelease;
    bool _released;
} TEXT;

/// <summary>
/// 不変文字列を生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>不変文字列。</returns>
TEXT createText(TCHAR* source);

/// <summary>
/// 文字列から不変文字列にラップ。
/// <para>スタック内で元文字列を変更せずに使用することが前提条件。</para>
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>不変文字列。</returns>
TEXT wrapText(TCHAR* source);
