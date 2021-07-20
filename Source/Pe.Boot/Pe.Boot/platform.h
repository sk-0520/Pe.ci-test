#pragma once
#include <windows.h>

#include "text.h"


/// <summary>
/// 環境変数の取得。
/// </summary>
/// <param name="key">環境変数名。</param>
/// <returns>取得した環境変数。解放が必要。存在しない場合は無効テキスト。</returns>
#ifdef MEM_CHECK
TEXT mem_check__get_environment_variable(const TEXT* key, MEM_CHECK_PORT_ARGS);
#   define get_environment_variable(key) mem_check__get_environment_variable((key), MEM_CHECK_WRAP_ARGS)
#else
TEXT get_environment_variable(const TEXT* key);
#endif

/// <summary>
/// 環境変数の設定。
/// </summary>
/// <param name="key">環境変数名。</param>
/// <param name="value">設定する値。</param>
/// <returns>成功状態。</returns>
bool set_environment_variable(const TEXT* key, const TEXT* value);

