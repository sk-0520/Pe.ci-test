#pragma once
#include "text.h"


/// <summary>
/// テキスト検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索テキスト。</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>見つかった参照テキスト、見つからない場合は空テキスト。解放不要。</returns>
TEXT findText(const TEXT* haystack, const TEXT* needle, bool ignoreCase);

