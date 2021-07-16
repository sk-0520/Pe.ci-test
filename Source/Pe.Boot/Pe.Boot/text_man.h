#pragma once
#include "text.h"


/// <summary>
/// テキスト長の取得。
/// <para>TEXT.length でもいいけどこちらの方が安全。</para>
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>長さ。文字列格納領域の長さであるため文字数とは違う。</returns>
size_t getTextLength(const TEXT* text);

/// <summary>
/// テキスト追加。
/// </summary>
/// <param name="source">追加元テキスト。</param>
/// <param name="text">追加対象テキスト。</param>
/// <returns>追加済みテキスト。解放が必要。</returns>
TEXT addText(const TEXT* source, const TEXT* text);

/// <summary>
/// テキスト結合。
/// </summary>
/// <param name="separator">セパレータ。</param>
/// <param name="texts">結合するテキスト。</param>
/// <param name="count">textsの個数。</param>
/// <param name="ignoreEmpty">空要素を無視するか。</param>
/// <returns>結合済みテキスト。解放が必要。</returns>
TEXT joinText(const TEXT* separator, const TEXT texts[], size_t count, IGNORE_EMPTY ignoreEmpty);

/// <summary>
/// 空テキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空の場合に真。</returns>
bool isEmptyText(const TEXT* text);

/// <summary>
/// 空か空白文字で構成されたテキストか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <returns>空か空白文字で構成されている場合に真。</returns>
bool isWhiteSpaceText(const TEXT* text);

