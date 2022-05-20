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
/// 英字か。
/// </summary>
/// <param name="c"></param>
/// <returns></returns>
bool is_alphabet_character(TCHAR c);

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

/// <summary>
/// 対象文字が指定文字群に含まれているか
/// </summary>
/// <param name="c">対象文字。</param>
/// <param name="characters">文字群。</param>
/// <param name="length">文字群の数。</param>
/// <returns>含まれている場合に真。</returns>
bool exists_character(TCHAR c, const TCHAR* characters, size_t length);
