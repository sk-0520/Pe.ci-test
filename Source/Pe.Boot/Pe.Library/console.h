#pragma once
#include <windows.h>

#include "text.h"
#include "fsio.h"

/// <summary>
/// コンソールリソース。
/// </summary>
typedef struct tag_CONSOLE_RESOURCE
{
    struct
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
    } handle;
    struct
    {
        /// <summary>
        /// 標準入力。
        /// </summary>
        FILE_RESOURCE input;
        /// <summary>
        /// 標準出力。
        /// </summary>
        FILE_RESOURCE output;
        /// <summary>
        /// 標準エラー。
        /// </summary>
        FILE_RESOURCE error;
    } stdio;
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
/// <returns>コンソールリソース。<c>end_console</c>で解放が必要。</returns>
CONSOLE_RESOURCE begin_console(const MEMORY_ARENA_RESOURCE* memory_arena_resource);

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

/// <summary>
/// なんだこれ、なんだ。
/// </summary>
size_t write_console_text(const CONSOLE_RESOURCE* console_resource, const TEXT* text, bool newline);
