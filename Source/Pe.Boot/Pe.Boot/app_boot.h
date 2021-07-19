#pragma once
#include <windows.h>

#include "command_line.h"


/// <summary>
/// 通常起動。
/// </summary>
/// <param name="hInstance"></param>
int boot_normal(HINSTANCE hInstance);

/// <summary>
/// 引数付き通常起動。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="options"></param>
int boot_with_option(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option);
