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
/// テキスト長の取得。
/// <para>TEXT.length でもいいけどこちらの方が安全。</para>
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>長さ。文字列格納領域の長さであるため文字数とは違う。</returns>
size_t get_text_length(const TEXT* text);

/// <summary>
/// テキスト追加。
/// </summary>
/// <param name="source">追加元テキスト。</param>
/// <param name="text">追加対象テキスト。</param>
/// <returns>追加済みテキスト。解放が必要。</returns>
TEXT add_text(const TEXT* source, const TEXT* text);

/// <summary>
/// テキスト結合。
/// </summary>
/// <param name="separator">セパレータ。</param>
/// <param name="texts">結合するテキスト。</param>
/// <param name="count">textsの個数。</param>
/// <param name="ignore_empty">空要素を無視するか。</param>
/// <returns>結合済みテキスト。解放が必要。</returns>
TEXT join_text(const TEXT* separator, const TEXT texts[], size_t count, IGNORE_EMPTY ignore_empty);

/// <summary>
/// 空テキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空の場合に真。</returns>
bool is_empty_text(const TEXT* text);

/// <summary>
/// 空か空白文字で構成されたテキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空か空白文字で構成されている場合に真。</returns>
bool is_whitespace_text(const TEXT* text);

/// <summary>
/// テキストのトリム処理。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <param name="start">先頭をトリム対象とするか。</param>
/// <param name="end">終端をトリム対象とするか。</param>
/// <param name="characters">トリム対象文字一覧。</param>
/// <param name="count">charactersの個数。</param>
/// <returns>トリムされたテキスト。解放が必要。</returns>
TEXT trim_text(const TEXT* text, bool start, bool end, const TCHAR* characters, size_t count);

/// <summary>
/// 両端のホワイトスペースをトリム。
/// <para><c>trimText</c>のラッパー。</para>
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>トリムされたテキスト。解放が必要。</returns>
TEXT trim_whitespace_text(const TEXT* text);
