#pragma once
#include <stdbool.h>

#include <windows.h>

#include "memory.h"

#define STRING_BUILDER_DEFAULT_CAPACITY (64)

/// <summary>
/// 可変長文字列生成処理。
/// </summary>
typedef struct tag_STRING_BUILDER
{
    /// <summary>
    /// バッファ。
    /// <para>番兵を前提として扱わないこと。</para>
    /// <para>構築中に使用する場合、追加・削除処理を行ってはいけない(再確保でヒープが無効の可能性あり)。</para>
    /// </summary>
    TCHAR* buffer;
    /// <summary>
    /// 現在の長さ。
    /// </summary>
    size_t length;
    struct
    {
        size_t capacity;
    } library;
} STRING_BUILDER;

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
/// <returns>複製された文字列。解放が必要。</returns>
TCHAR* RC_HEAP_FUNC(clone_string, const TCHAR* source);
#ifdef RES_CHECK
#   define clone_string(source) RC_HEAP_WRAP(clone_string, source)
#endif

/// <summary>
/// 文字列を確保。
/// </summary>
/// <param name="length">文字列の長さ。</param>
/// <returns>先頭 0 の番兵を考慮した領域(length + 1)。freeStringによる解放が必要。</returns>
TCHAR* RC_HEAP_FUNC(allocate_string, size_t length);
#ifdef RES_CHECK
#   define allocate_string(length) RC_HEAP_WRAP(allocate_string, (length))
#endif

/// <summary>
/// 確保した文字列を解放。
/// ドメインとしての関数で<c>freeMemory</c>のラッパー。
/// </summary>
/// <param name="s"></param>
void RC_HEAP_FUNC(free_string, const TCHAR* s);
#ifdef RES_CHECK
#   define free_string(s) RC_HEAP_WRAP(free_string, (s))
#endif

/// <summary>
/// <c>STRING_BUILDER</c>を初期化文字列から生成。
/// </summary>
/// <param name="s">初期化に使用する文字列。</param>
/// <param name="capacity">初期予約領域。特に指定しない場合は<c>STRING_BUILDER_DEFAULT_CAPACITY</c>を使用する。<c>s</c>より小さい場合、<c>s</c>の長さまで拡張される。</param>
/// <returns>生成した<c>STRING_BUILDER</c>。解放が必要。</returns>
STRING_BUILDER RC_HEAP_FUNC(initialize_string_builder, const TCHAR* s, size_t capacity);
#ifdef RES_CHECK
#   define initialize_string_builder(s, capacity) RC_HEAP_WRAP(initialize_string_builder, (s), (capacity))
#endif

/// <summary>
/// <c>STRING_BUILDER</c>を生成。
/// </summary>
/// <param name=""></param>
/// <param name="capacity">初期予約領域。特に指定しない場合は<c>STRING_BUILDER_DEFAULT_CAPACITY</c>を使用する。</param>
/// <returns>生成した<c>STRING_BUILDER</c>。解放が必要。</returns>
STRING_BUILDER RC_HEAP_FUNC(create_string_builder, size_t capacity);
#ifdef RES_CHECK
#   define create_string_builder(capacity) RC_HEAP_WRAP(create_string_builder, (capacity))
#endif

/// <summary>
/// <c>STRING_BUILDER</c>の解放。
/// </summary>
/// <param name=""></param>
/// <param name="string_builder"></param>
bool RC_HEAP_FUNC(free_string_builder, STRING_BUILDER* string_builder);
#ifdef RES_CHECK
#   define free_string_builder(string_builder) RC_HEAP_WRAP(free_string_builder, (string_builder))
#endif

