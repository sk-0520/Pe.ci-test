#pragma once

typedef enum tag_EXIT_CODE
{
    EXIT_CODE_SUCCESS = 0,
    EXIT_CODE_UNKNOWN_EXECUTE_MODE = 1,
} EXIT_CODE;

/// <summary>
/// 実行モード。
/// </summary>
typedef enum tag_EXECUTE_MODE
{
    /// <summary>
    /// 通常起動。
    /// </summary>
    EXECUTE_MODE_BOOT,
    /// <summary>
    /// コンソール起動。
    /// </summary>
    EXECUTE_MODE_CONSOLE,
} EXECUTE_MODE;
