#pragma once
#include <stdbool.h>

#include <windows.h>

#include "memory.h"

/*
* 文字列処理。
* C言語的な処理なので原則アプリ内では使用しない想定。
*/

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
TCHAR* concatenate_string(TCHAR* target, const TCHAR* value);
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
/// <returns>複製された文字列。解放が必要。</returns>
TCHAR* RC_HEAP_FUNC(clone_string, const TCHAR* source, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define clone_string(source, memory_arena_resource) RC_HEAP_WRAP(clone_string, source, memory_arena_resource)
#endif

/// <summary>
/// 文字列を指定した長さで複製。
/// </summary>
/// <param name="source"></param>
/// <param name="length"></param>
/// <returns>複製された文字列。解放が必要。</returns>
TCHAR* RC_HEAP_FUNC(clone_string_with_length, const TCHAR* source, size_t length, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define clone_string_with_length(source, length, memory_arena_resource) RC_HEAP_WRAP(clone_string_with_length, (source), (length), memory_arena_resource)
#endif

/// <summary>
/// 文字列を確保。
/// </summary>
/// <param name="length">文字列の長さ。</param>
/// <returns>先頭 0 の番兵を考慮した領域(length + 1)。<see cref="release_string"/>による解放が必要。</returns>
TCHAR* RC_HEAP_FUNC(allocate_string, size_t length, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define allocate_string(length, memory_arena_resource) RC_HEAP_WRAP(allocate_string, (length), memory_arena_resource)
#endif

/// <summary>
/// 確保した文字列を解放。
/// ドメインとしての関数で<see cref="release_memory"/>のラッパー。
/// </summary>
/// <param name="s"></param>
void RC_HEAP_FUNC(release_string, const TCHAR* s, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define release_string(s, memory_arena_resource) RC_HEAP_WRAP(release_string, (s), memory_arena_resource)
#endif

