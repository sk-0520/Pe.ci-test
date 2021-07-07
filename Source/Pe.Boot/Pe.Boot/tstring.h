#pragma once
#include <tchar.h>

/// <summary>
/// 文字列 s1 の先頭から文字列 s2 を探し、見つかったときにはその位置をポインタで返却し、見つからなかったときにはNULLを返却します。
/// </summary>
/// <param name="haystack ">検索対象文字列</param>
/// <param name="s2">検索文字列</param>
/// <returns>一致文字のアドレス, 見つからない場合は <c>NULL</c></returns>
const TCHAR* tstrstr(const TCHAR* haystack, const TCHAR* needle);
const TCHAR* tstrstri(const TCHAR* haystack, const TCHAR* needle);
