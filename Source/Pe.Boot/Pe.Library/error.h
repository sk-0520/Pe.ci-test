#pragma once
#include <windows.h>

#include "res_check.h"
#include "text.h"

typedef struct tag_ERROR_CODE
{
    DWORD code;
} ERROR_CODE;

/// <summary>
/// 呼び出し側スレッドの持つ最新のエラーコードをリセット。
/// </summary>
#define clear_last_error_code set_last_error_code((ERROR_CODE) { .code = ERROR_SUCCESS, })

/// <summary>
/// 呼び出し側スレッドの持つ最新のエラーコードを設定。
/// </summary>
/// <param name="error_code"></param>
void set_last_error_code(ERROR_CODE error_code);

/// <summary>
/// 呼び出し側スレッドの持つ最新のエラーコードを取得。
/// </summary>
/// <param name=""></param>
/// <returns></returns>
ERROR_CODE get_last_error_code(void);

/// <summary>
/// エラーコードからエラーメッセージを取得。
/// <para>Windows API が主軸になっている。</para>
/// </summary>
/// <param name="error_code">エラーコード</param>
/// <param name="memory_resource"></param>
/// <returns>エラーメッセージ。解放が必要。失敗時は無効テキスト。</returns>
TEXT RC_HEAP_FUNC(get_error_message, ERROR_CODE error_code, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define get_error_message(error_code, memory_resource) RC_HEAP_WRAP(get_error_message, error_code, memory_resource)
#endif
