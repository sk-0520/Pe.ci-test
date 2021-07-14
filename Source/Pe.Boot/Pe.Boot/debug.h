#pragma once
#include <tchar.h>

#include <windows.h>

/// <summary>
/// デバッグ時のみアウトプット処理。
/// </summary>
/// <param name="s"></param>
void outputDebug(const TCHAR * s);
#define debug(s) outputDebug(_T(s))
