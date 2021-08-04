#pragma once
#include "app_console.h"

/// <summary>
/// コンソールプロンプト実行。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="command_line_option"></param>
/// <param name="console_resource"></param>
/// <returns></returns>
EXIT_CODE console_execute_prompt(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option, const CONSOLE_RESOURCE* console_resource);
