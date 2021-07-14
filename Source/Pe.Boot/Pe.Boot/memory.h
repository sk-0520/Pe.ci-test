#pragma once
#include <stdbool.h>
#include <stddef.h>

#if MEM_CHECK
void _mem_check_printAllocateMemory();
#endif

/// <summary>
/// 指定したサイズ以上のヒープ領域を確保。
/// </summary>
/// <param name="bytes">確保サイズ</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
#if MEM_CHECK
void* _mem_check_allocateMemory(size_t bytes, bool zeroFill, const TCHAR* _file, size_t _line);
#   define allocateMemory(bytes, zeroFill) _mem_check_allocateMemory((bytes), (zeroFill), __FILEW__, __LINE__)
#else
void* allocateMemory(size_t bytes, bool zeroFill);
#endif

/// <summary>
/// 指定したサイズ以上のヒープ領域を0クリアで確保。
/// </summary>
/// <param name="count">確保する個数。</param>
/// <param name="typeSize">型サイズ。</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
#if MEM_CHECK
void* _mem_check_allocateClearMemory(size_t count, size_t typeSize, const TCHAR* _file, size_t _line);
#   define allocateClearMemory(count, typeSize) _mem_check_allocateClearMemory((count), (typeSize), __FILEW__, __LINE__)
#else
void* allocateClearMemory(size_t count, size_t typeSize);
#endif

/// <summary>
/// allocateMemory で確保した領域を解放。
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
#if MEM_CHECK
void _mem_check_freeMemory(void* p, const TCHAR* _file, size_t _line);
#   define freeMemory(p) _mem_check_freeMemory((p), __FILEW__, __LINE__)
#else
void freeMemory(void* p);
#endif

/// <summary>
/// <c>memset</c> する。
/// </summary>
/// <param name="target">対象領域。</param>
/// <param name="value">値。</param>
/// <param name="bytes">範囲。</param>
/// <returns>target</returns>
void* setMemory(void* target, int value, size_t bytes);

/// <summary>
/// <c>memcpy</c>する。
/// </summary>
/// <param name="destination">コピー先。</param>
/// <param name="source">コピー元。</param>
/// <param name="bytes">コピーサイズ。</param>
/// <returns></returns>
void* copyMemory(void* destination, void* source, size_t bytes);

/// <summary>
/// <c>memmove</c>する。
/// </summary>
/// <param name="destination">移動先。</param>
/// <param name="source">移動元。</param>
/// <param name="bytes">移動サイズ。</param>
/// <returns></returns>
void* moveMemory(void* destination, void* source, size_t bytes);
