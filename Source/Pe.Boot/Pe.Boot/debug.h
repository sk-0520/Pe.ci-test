﻿#pragma once
#include <windows.h>

/// <summary>
/// デバッグ時のみ使用可能なログ出力。
///
/// 内部的には<c>OutputDebugString</c>だが<c>DEBUG</c>定義がある場合にのみ使用される。
/// </summary>
/// <param name="s"></param>
void outputDebug(const TCHAR * s);
#define debug(s) outputDebug(_T(s))
