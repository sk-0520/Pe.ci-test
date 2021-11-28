#pragma once
#include <stdint.h>

#include "text.h"

/// <summary>
/// FNV-1 を用いたハッシュ処理。
/// </summary>
/// <param name="value"></param>
/// <param name="length"></param>
/// <returns></returns>
size_t calc_hash_fnv1(const uint8_t* value, size_t length);

/// <summary>
/// FNV-1 を用いたハッシュ処理。
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
size_t calc_hash_fnv1_from_text(const TEXT* text);


