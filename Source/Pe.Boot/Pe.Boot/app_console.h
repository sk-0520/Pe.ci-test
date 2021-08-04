#pragma once
#include "../Pe.Library/command_line.h"
#include "../Pe.Library/console.h"
#include "app_common.h"


/// <summary>
/// コンソール実行種別
/// </summary>
typedef enum tag_CONSOLE_KIND
{
    /// <summary>
    /// 不明。
    /// </summary>
    CONSOLE_KIND_UNKNOWN,
    /// <summary>
    /// プロンプト。
    /// </summary>
    CONSOLE_KIND_PROMPT,
} CONSOLE_KIND;


/// <summary>
/// コマンドラインオプションからコンソール種別を取得。
/// </summary>
/// <param name="command_line_option"></param>
/// <returns></returns>
CONSOLE_KIND get_console_kind(const COMMAND_LINE_OPTION* command_line_option);

/// <summary>
/// コンソール処理実行。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="command_line_option"></param>
/// <returns></returns>
EXIT_CODE console_execute(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option);
