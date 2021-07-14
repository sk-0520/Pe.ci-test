#pragma once
#include "commandline.h"

/// <summary>
/// コマンドラインキーの識別子。
/// </summary>
typedef enum _TAG_COMMAND_LINE_MARK
{
    /// <summary>
    /// -
    /// </summary>
    COMMAND_LINE_MARK_SHORT,
    /// <summary>
    /// --
    /// </summary>
    COMMAND_LINE_MARK_LONG,
    /// <summary>
    /// /
    /// </summary>
    COMMAND_LINE_MARK_DOS,
} COMMAND_LINE_MARK;

/// <summary>
/// コマンドラインキー。
/// </summary>
typedef struct _TAG_COMMAND_LINE_KEY
{
    /// <summary>
    /// コマンドラインキーの識別子。
    /// </summary>
    COMMAND_LINE_MARK mark;
    /// <summary>
    /// キー。
    /// </summary>
    TEXT key;
} COMMAND_LINE_KEY;

/// <summary>
/// 実行モードを取得。
/// </summary>
/// <param name="commandLineOption"></param>
/// <returns></returns>
EXECUTE_MODE getExecuteMode(const COMMAND_LINE_OPTION* commandLineOption);

