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
/// テキスト検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索テキスト。</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>見つかったテキストを開始とする参照テキスト、見つからない場合は無効テキスト。解放不要。</returns>
TEXT findText(const TEXT* haystack, const TEXT* needle, bool ignoreCase);

/// <summary>
/// テキスト検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索文字。</param>
/// <returns>見つかったテキストを開始とする参照テキスト、見つからない場合は無効テキスト。解放不要。</returns>
TEXT findCharacter2(const TEXT* haystack, TCHAR needle);

/// <summary>
/// テキスト内の文字位置を検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索文字。</param>
/// <returns>一致文字のインデックス。見つからない場合は0未満。</returns>
ssize_t indexOfCharacter(const TEXT* haystack, TCHAR needle);
