#pragma once
#include <tchar.h>

#include "common.h"

/// <summary>
/// コマンドラインオプション。
/// </summary>
typedef struct _TAG_COMMAND_LINE_OPTION
{
    /// <summary>
    /// 引数一覧。
    /// </summary>
    const TCHAR** arguments;
    /// <summary>
    /// 引数の個数。
    /// </summary>
    const size_t count;
} COMMAND_LINE_OPTION;

/// <summary>
/// コマンドライン文字列を分解。
/// </summary>
/// <param name="commandLine"></param>
/// <returns>分解結果。freeCommandLine による開放が必要。</returns>
COMMAND_LINE_OPTION parseCommandLine(const TCHAR* commandLine);

/// <summary>
/// コマンドラインオプションを解放。
/// </summary>
/// <param name="commandLineOption"></param>
void freeCommandLine(const COMMAND_LINE_OPTION* commandLineOption);

/// <summary>
/// 実行モードを取得。
/// </summary>
/// <param name="commandLineOption"></param>
/// <returns></returns>
EXECUTE_MODE getExecuteMode(const COMMAND_LINE_OPTION* commandLineOption);

/// <summary>
/// 書式調整後の動的確保された文字列を返す。
/// </summary>
/// <param name="arg"></param>
/// <returns>呼び出し側で世話すること。</returns>
TCHAR* tuneArg(const TCHAR* arg);
