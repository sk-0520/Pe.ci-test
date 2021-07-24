#pragma once

#include "common.h"
#include "tstring.h"
#include "text.h"


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
        TCHAR* newline;
    } library;
} STRING_BUILDER;



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

/// <summary>
/// テキストを生成する。
/// </summary>
/// <param name="string_builder"></param>
/// <param name="text"></param>
/// <param name="newline"></param>
/// <returns>生成したテキスト。解放が必要。</returns>
TEXT RC_HEAP_FUNC(build_text_string_builder, const STRING_BUILDER* string_builder);
#ifdef RES_CHECK
#   define build_text_string_builder(string_builder) RC_HEAP_WRAP(build_text_string_builder, (string_builder))
#endif

/// <summary>
/// テキストを参照する。
/// <para>参照テキスト使用中に元<c>STRING_BUILDER</c>を操作しない前提。</para>
/// </summary>
/// <param name="string_builder"></param>
/// <returns>参照テキスト。</returns>
TEXT reference_text_string_builder(STRING_BUILDER* string_builder);


STRING_BUILDER* append_builder_newline(STRING_BUILDER* string_builder);
STRING_BUILDER* append_builder_string(STRING_BUILDER* string_builder, const TCHAR* s, bool newline);
STRING_BUILDER* append_builder_text(STRING_BUILDER* string_builder, const TEXT* text, bool newline);
STRING_BUILDER* append_builder_character(STRING_BUILDER* string_builder, TCHAR c, bool newline);
STRING_BUILDER* append_builder_int(STRING_BUILDER* string_builder, ssize_t value, bool newline);
STRING_BUILDER* append_builder_uint(STRING_BUILDER* string_builder, size_t value, bool newline);
STRING_BUILDER* append_builder_bool(STRING_BUILDER* string_builder, bool value, bool newline);
STRING_BUILDER* append_builder_pointer(STRING_BUILDER* string_builder, void* pointer, bool newline);

