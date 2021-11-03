#pragma once
#include <stdbool.h>
#include <tchar.h>

/// <summary>
/// 改行か。
/// <para>一文字だけを見るのでCRLFの連続は関知しない。</para>
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_newline_character(TCHAR c);

/// 数字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_digit_character(TCHAR c);

/// <summary>
/// 小文字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_lower_character(TCHAR c);

/// <summary>
/// 大文字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_upper_character(TCHAR c);

/// <summary>
/// <summary>
/// 小文字に変換。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
TCHAR to_lower_character(TCHAR c);

/// <summary>
/// 大文字に変換。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
TCHAR to_upper_character(TCHAR c);
