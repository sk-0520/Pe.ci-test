#pragma once
#include <stdbool.h>
#include <stdint.h>

#include <tchar.h>

#include "res_check.h"

typedef byte_t (*library__func_calc_extend_capacity)(byte_t input_bytes);

/// <summary>
/// 指定したサイズ以上のヒープ領域を確保。
/// </summary>
/// <param name="bytes">確保サイズ</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
void* RC_HEAP_FUNC(allocate_memory, byte_t bytes, bool zero_fill);
#if RES_CHECK
#   define allocate_memory(bytes, zero_fill) RC_HEAP_WRAP(allocate_memory, bytes, zero_fill)
#endif

/// <summary>
/// 指定したサイズ以上のヒープ領域を0クリアで確保。
/// </summary>
/// <param name="count">確保する個数。</param>
/// <param name="type_size">型サイズ。</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
void* RC_HEAP_FUNC(allocate_clear_memory, size_t count, size_t type_size);
#if RES_CHECK
#   define allocate_clear_memory(count, type_size) RC_HEAP_WRAP(allocate_clear_memory, count, type_size)
#endif

/// <summary>
/// allocateMemory で確保した領域を解放。
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(free_memory, void* p);
#if RES_CHECK
#   define free_memory(p) RC_HEAP_WRAP(free_memory, p)
#endif



/// <summary>
/// <see cref="memset" />する。
/// </summary>
/// <param name="target">対象領域。</param>
/// <param name="value">値。</param>
/// <param name="bytes">範囲。</param>
/// <returns>target</returns>
void* set_memory(void* target, uint8_t value, byte_t bytes);

/// <summary>
/// <see cref="memcpy" />する。
/// </summary>
/// <param name="destination">コピー先。</param>
/// <param name="source">コピー元。</param>
/// <param name="bytes">コピーサイズ。</param>
/// <returns></returns>
void* copy_memory(void* destination, const void* source, byte_t bytes);

/// <summary>
/// <see cref="memmove" />する。
/// </summary>
/// <param name="destination">移動先。</param>
/// <param name="source">移動元。</param>
/// <param name="bytes">移動サイズ。</param>
/// <returns></returns>
void* move_memory(void* destination, const void* source, byte_t bytes);

/// <summary>
/// <see cref="memcmp" />する。
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <param name="bytes"></param>
/// <returns>a &lt; b: 負, a = b: 0, a &gt; b: 正。</returns>
int compare_memory(const void* a, const void* b, byte_t bytes);

/// <summary>
/// 予約領域を持つバッファを拡張する基底処理。
/// <para>ライブラリ側で使用する前提処理。アプリケーション側からは使用しない。</para>
/// </summary>
/// <param name="buffer">対象領域のポインタ。</param>
/// <param name="current_bytes">現在のバイト数。</param>
/// <param name="current_capacity_bytes">現在の予約バイト数。</param>
/// <param name="need_bytes">必要なバイト数。</param>
/// <param name="default_capacity_bytes">予約領域の標準値。</param>
/// <param name="calc_extend_capacity">予約領域拡張方法。</param>
/// <returns>拡張後の総バイト数。未実施の場合は0を返す。</returns>
byte_t library__extend_capacity_if_not_enough_bytes(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, library__func_calc_extend_capacity calc_extend_capacity);

byte_t library__extend_capacity_if_not_enough_bytes_x2(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes);
