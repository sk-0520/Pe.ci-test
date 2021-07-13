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
    /// 解放が必要か。
    /// <para>アプリケーション内では使用しない。</para>
    /// </summary>
    bool _needRelease;
    /// <summary>
    /// 解放済みか。
    /// <para>アプリケーション内では使用しない。</para>
    /// </summary>
    bool _released;
} TEXT;

/// <summary>
/// 不変文字列を生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>不変文字列。</returns>
TEXT createText(const TCHAR* source);

/// <summary>
/// 文字列から不変文字列にラップ。
/// <para>スタック内で元文字列を変更せずに使用することが前提条件。</para>
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>不変文字列。</returns>
TEXT wrapText(const TCHAR* source);

/// <summary>
/// 不変文字列の解放。
/// <para>不要な場合は処理しない。</para>
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
bool freeText(TEXT* text);
