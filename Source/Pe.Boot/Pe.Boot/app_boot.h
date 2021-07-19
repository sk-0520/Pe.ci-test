#pragma once
#include <windows.h>

/// <summary>
/// 通常起動。
/// </summary>
/// <param name="hInstance"></param>
void boot_normal(HINSTANCE hInstance);

/// <summary>
/// 引数付き通常起動。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="options"></param>
void boot_with_option(HINSTANCE hInstance, const TCHAR* command_line);
