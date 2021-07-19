#pragma once
#include <stdbool.h>
#include <stddef.h>

#include <tchar.h>

#ifdef MEM_CHECK

#define MEM_CHECK_CALLER_FILE_PATH 260

#define MEM_CHECK_ALLOC_STOCK_LENGTH (1024 * 4)
#define MEM_CHECK_PRINT_BUFFER_LENGTH (100 + (MEM_CHECK_CALLER_FILE_PATH * 2))

#define MEM_CHECK_ARG_FLIE caller_file
#define MEM_CHECK_ARG_LINE caller_line
#define MEM_CHECK_HEAD_DEF _T(__FILE__), __LINE__
#define MEM_CHECK_HEAD_ARGS const TCHAR* MEM_CHECK_ARG_FLIE, size_t MEM_CHECK_ARG_LINE
#define MEM_CHECK_CALL_ARGS MEM_CHECK_ARG_FLIE, MEM_CHECK_ARG_LINE

typedef struct
{
    void* p;
    TCHAR file[MEM_CHECK_CALLER_FILE_PATH];
    size_t line;
} mem_check__ALLOC_STOCK_ITEM;

void mem_check__print_allocate_memory(bool leak, void(*output)(TCHAR*), bool add_new_line);
#endif

/// <summary>
/// 指定したサイズ以上のヒープ領域を確保。
/// </summary>
/// <param name="bytes">確保サイズ</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
#ifdef MEM_CHECK
void* mem_check__allocate_memory(size_t bytes, bool zero_fill, const TCHAR* caller_file, size_t caller_line);
#   define allocate_memory(bytes, zero_fill) mem_check__allocate_memory((bytes), (zero_fill), MEM_CHECK_HEAD_DEF)
#else
void* allocate_memory(size_t bytes, bool zero_fill);
#endif

/// <summary>
/// 指定したサイズ以上のヒープ領域を0クリアで確保。
/// </summary>
/// <param name="count">確保する個数。</param>
/// <param name="type_size">型サイズ。</param>
/// <returns>確保した領域。<c>freeMemory</c>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
#ifdef MEM_CHECK
void* mem_check__allocate_clear_memory(size_t count, size_t type_size, const TCHAR* caller_file, size_t caller_line);
#   define allocate_clear_memory(count, type_size) mem_check__allocate_clear_memory((count), (type_size), _T(__FILE__), __LINE__)
#else
void* allocate_clear_memory(size_t count, size_t type_size);
#endif

/// <summary>
/// allocateMemory で確保した領域を解放。
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
#ifdef MEM_CHECK
void mem_check__free_memory(void* p, const TCHAR* caller_file, size_t caller_line);
#   define free_memory(p) mem_check__free_memory((p), _T(__FILE__), __LINE__)
#else
void free_memory(void* p);
#endif

/// <summary>
/// <c>memset</c> する。
/// </summary>
/// <param name="target">対象領域。</param>
/// <param name="value">値。</param>
/// <param name="bytes">範囲。</param>
/// <returns>target</returns>
void* set_memory(void* target, unsigned char value, size_t bytes);

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
