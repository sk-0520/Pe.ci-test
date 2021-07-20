#pragma once
#include <stdbool.h>

#include <windows.h>

#include "memory.h"


/// <summary>
/// 文字列長を取得。
/// </summary>
/// <param name="s">対象文字列。</param>
/// <returns>長さ。</returns>
size_t get_string_length(const TCHAR* s);

#define format_string(result, format, ...) do { wsprintf(result, format,  __VA_ARGS__); } while(0)

/// <summary>
/// 文字列を結合。
/// </summary>
/// <param name="target">結合対象文字列。</param>
/// <param name="value">追加する文字列。</param>
/// <returns>結合された文字列。</returns>
TCHAR* concat_string(TCHAR* target, const TCHAR* value);
/// <summary>
/// 文字列をコピー。
/// </summary>
/// <param name="result">コピー後の文字列の格納先。</param>
/// <param name="value">コピー対象文字列。</param>
/// <returns>コピーされた文字列。</returns>
TCHAR* copy_string(TCHAR* result, const TCHAR* value);

/// <summary>
/// 文字列を複製。
/// </summary>
/// <param name="source"></param>
/// <returns>複製された文字列。<c>freeString(freeMemory)</c>にて解放する必要あり。</returns>
#ifdef RES_CHECK
TCHAR* rc_heap__clone_string(const TCHAR * source, RES_CHECK_FUNC_ARGS);
#   define clone_string(source) rc_heap__clone_string(source, RES_CHECK_WRAP_ARGS)
#else
TCHAR* clone_string(const TCHAR* source);
#endif

/// <summary>
/// 文字列を確保。
/// </summary>
/// <param name="length">文字列の長さ。</param>
/// <returns>先頭 0 の番兵を考慮した領域(length + 1)。freeStringによる解放が必要。</returns>
#ifdef RES_CHECK
TCHAR* rc_heap__allocate_string(size_t length, RES_CHECK_FUNC_ARGS);
#   define allocate_string(length) rc_heap__allocate_string((length), RES_CHECK_WRAP_ARGS)
#else
TCHAR* allocate_string(size_t length);
#endif

/// <summary>
/// 確保した文字列を解放。
/// ドメインとしての関数で<c>freeMemory</c>のラッパー。
/// </summary>
/// <param name="s"></param>
#ifdef RES_CHECK
void rc_heap__free_string(const TCHAR * s, RES_CHECK_FUNC_ARGS);
#   define free_string(s) rc_heap__free_string((s), RES_CHECK_WRAP_ARGS)
#else
void free_string(const TCHAR * s);
#endif


