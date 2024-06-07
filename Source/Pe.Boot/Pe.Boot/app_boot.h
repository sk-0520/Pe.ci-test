#pragma once
#include <Windows.h>

#include "app_common.h"
#include "../Pe.Library/command_line.h"


/// <summary>
/// 通常起動。
/// </summary>
/// <param name="hInstance"></param>
EXIT_CODE boot_normal(HINSTANCE hInstance);

/// <summary>
/// 引数付き通常起動。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="options"></param>
EXIT_CODE boot_with_option(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option);
