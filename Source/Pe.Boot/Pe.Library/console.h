#pragma once
#include <windows.h>

#include "../Pe.Library/text.h"

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
    struct
    {
        /// <summary>
        ///
        /// </summary>
        bool attached;
    } library;
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
/// 標準出力を使用してコンソールに対してテキスト出力。
/// </summary>
/// <param name="console_resource"></param>
/// <param name="text"></param>
/// <param name="newline"></param>
/// <returns></returns>
size_t output_console_text(const CONSOLE_RESOURCE* console_resource, const TEXT* text, bool newline);
