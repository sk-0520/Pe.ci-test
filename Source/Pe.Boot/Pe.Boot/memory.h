#pragma once
#include <stdbool.h>
#include <stddef.h>

#if MEM_CHECK

#define MEM_CHECK_CALLER_FILE_PATH MAX_PATH

#define MEM_CHECK_ALLOC_STOCK_LENGTH (1024 * 4)
#define MEM_CHECK_PRINT_BUFFER_LENGTH (100 + (MEM_CHECK_CALLER_FILE_PATH * 2))


typedef struct
{
    void* p;
    TCHAR file[MEM_CHECK_CALLER_FILE_PATH];
    size_t line;
} mem_check__ALLOC_STOCK_ITEM;

void mem_check__printAllocateMemory(bool leak);
#endif

/// <summary>
/// 指定したサイズ以上のヒープ領域を確保。
/// </summary>
/// <param name="bytes">確保サイズ</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
#if MEM_CHECK
void* mem_check__allocateMemory(size_t bytes, bool zeroFill, const TCHAR* callerFile, size_t callerLine);
#   define allocateMemory(bytes, zeroFill) mem_check__allocateMemory((bytes), (zeroFill), __FILEW__, __LINE__)
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
void* mem_check__allocateClearMemory(size_t count, size_t typeSize, const TCHAR* callerFile, size_t callerLine);
#   define allocateClearMemory(count, typeSize) mem_check__allocateClearMemory((count), (typeSize), __FILEW__, __LINE__)
#else
void* allocateClearMemory(size_t count, size_t typeSize);
#endif

/// <summary>
/// allocateMemory で確保した領域を解放。
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
#if MEM_CHECK
void mem_check__freeMemory(void* p, const TCHAR* callerFile, size_t callerLine);
#   define freeMemory(p) mem_check__freeMemory((p), __FILEW__, __LINE__)
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
