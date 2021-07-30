#pragma once
#include "command_line.h"
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
/// コンソールリソース。
/// </summary>
typedef struct tag_CONSOLE_RESOURCE
{
    /// <summary>
    /// 標準入力。
    /// </summary>
    HANDLE input;
    /// <summary>
    /// 標準出力。
    /// </summary>
    HANDLE output;
    /// <summary>
    /// 標準エラー。
    /// </summary>
    HANDLE error;
} CONSOLE_RESOURCE;

/// <summary>
/// コンソール処理を開始。
/// </summary>
/// <returns></returns>
CONSOLE_RESOURCE begin_console();
/// <summary>
/// コンソール処理を終了。
/// </summary>
/// <param name="console_resource"></param>
void end_console(CONSOLE_RESOURCE* console_resource);

/// <summary>
/// コマンドラインオプションからコンソール種別を取得。
/// </summary>
/// <param name="command_line_option"></param>
/// <returns></returns>
CONSOLE_KIND get_console_kind(const COMMAND_LINE_OPTION* command_line_option);

/// <summary>
/// 標準出力を使用してコンソールに対してテキスト出力。
/// </summary>
/// <param name="console_resource"></param>
/// <param name="text"></param>
/// <param name="newline"></param>
/// <returns></returns>
size_t output_console_text(const CONSOLE_RESOURCE* console_resource, const TEXT* text, bool newline);

/// <summary>
/// コンソール処理実行。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="command_line_option"></param>
/// <returns></returns>
EXIT_CODE console_execute(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option);
