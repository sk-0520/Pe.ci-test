#pragma once
#include <Windows.h>

#include "res_check.h"
#include "text.h"


/// <summary>
/// 環境変数の取得。
/// </summary>
/// <param name="key">環境変数名。</param>
/// <returns>取得した環境変数。解放が必要。存在しない場合は無効テキスト。</returns>
TEXT RC_HEAP_FUNC(get_environment_variable, const TEXT* key, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define get_environment_variable(key, memory_arena_resource) RC_HEAP_WRAP(get_environment_variable, key, memory_arena_resource)
#endif

/// <summary>
/// 環境変数の設定。
/// </summary>
/// <param name="key">環境変数名。</param>
/// <param name="value">設定する値。</param>
/// <returns>成功状態。</returns>
bool set_environment_variable(const TEXT* key, const TEXT* value);

/// <summary>
/// 環境変数展開。
/// </summary>
/// <param name="text"></param>
/// <returns>展開後テキスト。解放が必要。</returns>
TEXT RC_HEAP_FUNC(expand_environment_variable, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define expand_environment_variable(text, memory_arena_resource) RC_HEAP_WRAP(expand_environment_variable, text, memory_arena_resource)
#endif
