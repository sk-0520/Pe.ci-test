#pragma once
#include <Windows.h>

#include "app_common.h"
#include "../Pe.Library/command_line.h"

/// <summary>
/// アプリケーションエントリポイント。
/// </summary>
/// <param name="hInstance">アプリケーションインスタンス。</param>
/// <param name="command_line_option">コマンドラインオプション。</param>
/// <returns>終了コード。</returns>
EXIT_CODE app_main(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option);
