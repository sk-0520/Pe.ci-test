#pragma once
#include <windows.h>

/// <summary>
/// 通常起動。
/// </summary>
/// <param name="hInstance"></param>
void bootNormal(HINSTANCE hInstance);

/// <summary>
/// 引数付き通常起動。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="options"></param>
void bootWithOption(HINSTANCE hInstance, const TCHAR* commandLine);
