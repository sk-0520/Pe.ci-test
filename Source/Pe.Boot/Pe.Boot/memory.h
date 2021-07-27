#pragma once
#include <stdbool.h>
#include <stdint.h>

#include <tchar.h>

#include "res_check.h"

/// <summary>
/// 指定したサイズ以上のヒープ領域を確保。
/// </summary>
/// <param name="bytes">確保サイズ</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
void* RC_HEAP_FUNC(allocate_memory, size_t bytes, bool zero_fill);
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
/// <c>memset</c> する。
/// </summary>
/// <param name="target">対象領域。</param>
/// <param name="value">値。</param>
/// <param name="bytes">範囲。</param>
/// <returns>target</returns>
void* set_memory(void* target, uint8_t value, size_t bytes);

/// <summary>
/// <c>memcpy</c>する。
/// </summary>
/// <param name="destination">コピー先。</param>
/// <param name="source">コピー元。</param>
/// <param name="bytes">コピーサイズ。</param>
/// <returns></returns>
void* copy_memory(void* destination, const void* source, size_t bytes);

/// <summary>
/// <c>memmove</c>する。
/// </summary>
/// <param name="destination">移動先。</param>
/// <param name="source">移動元。</param>
/// <param name="bytes">移動サイズ。</param>
/// <returns></returns>
void* move_memory(void* destination, const void* source, size_t bytes);

/// <summary>
/// <c>memcmp</c> する。
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <param name="bytes"></param>
/// <returns>a &lt; b: 負, a = b: 0, a &gt; b: 正。</returns>
int compare_memory(const void* a, const void* b, size_t bytes);
