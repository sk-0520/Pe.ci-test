#pragma once
#include <windows.h>

#include "res_check.h"
#include "text.h"


/// <summary>
/// 環境変数の取得。
/// </summary>
/// <param name="key">環境変数名。</param>
/// <returns>取得した環境変数。解放が必要。存在しない場合は無効テキスト。</returns>
TEXT RC_HEAP_FUNC(get_environment_variable, const TEXT* key);
#ifdef RES_CHECK
#   define get_environment_variable(key) RC_HEAP_WRAP(get_environment_variable, key)
#endif

/// <summary>
/// 環境変数の設定。
/// </summary>
/// <param name="key">環境変数名。</param>
/// <param name="value">設定する値。</param>
/// <returns>成功状態。</returns>
bool set_environment_variable(const TEXT* key, const TEXT* value);

