#pragma once

#include "common.h"
#include "tstring.h"
#include "primitive_list.gen.h"
#include "text.h"


#define STRING_BUILDER_DEFAULT_CAPACITY (64)

/// <summary>
/// 可変長文字列生成処理。
/// </summary>
typedef struct tag_STRING_BUILDER
{
    /// <summary>
    /// 改行に使用するテキスト。
    /// <para>解放処理は問答無用で行われるので管理をきちんと行うこと。デフォルト値は<c>NEWLINE_TEXT</c></para>
    /// </summary>
    TEXT newline;
    struct
    {
        /// <summary>
        /// 内部文字列。
        /// <para>各種確保処理は<c>PRIMITIVE_LIST_TCHAR.library.memory_arena_resource</c>が使用される。</para>
        /// </summary>
        PRIMITIVE_LIST_TCHAR list;
    } library;
} STRING_BUILDER;

/// <summary>
/// <c>STRING_BUILDER</c>を生成。
/// </summary>
/// <param name=""></param>
/// <param name="capacity">初期予約領域。特に指定しない場合は<c>STRING_BUILDER_DEFAULT_CAPACITY</c>を使用する。</param>
/// <returns>生成した<c>STRING_BUILDER</c>。解放が必要。</returns>
STRING_BUILDER RC_HEAP_FUNC(new_string_builder, size_t capacity, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#ifdef RES_CHECK
#   define new_string_builder(capacity, memory_arena_resource) RC_HEAP_WRAP(new_string_builder, (capacity), memory_arena_resource)
#endif

/// <summary>
/// <c>STRING_BUILDER</c>の解放。
/// </summary>
/// <param name=""></param>
/// <param name="string_builder"></param>
bool RC_HEAP_FUNC(release_string_builder, STRING_BUILDER* string_builder);
#ifdef RES_CHECK
#   define release_string_builder(string_builder) RC_HEAP_WRAP(release_string_builder, (string_builder))
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

STRING_BUILDER* clear_builder(STRING_BUILDER* string_builder);

STRING_BUILDER* append_builder_newline(STRING_BUILDER* string_builder);
STRING_BUILDER* append_builder_string(STRING_BUILDER* string_builder, const TCHAR* s, bool newline);
STRING_BUILDER* append_builder_text(STRING_BUILDER* string_builder, const TEXT* text, bool newline);
STRING_BUILDER* append_builder_character(STRING_BUILDER* string_builder, TCHAR c, bool newline);
STRING_BUILDER* append_builder_int(STRING_BUILDER* string_builder, ssize_t value, bool newline);
STRING_BUILDER* append_builder_uint(STRING_BUILDER* string_builder, size_t value, bool newline);
STRING_BUILDER* append_builder_bool(STRING_BUILDER* string_builder, bool value, bool newline);
STRING_BUILDER* append_builder_pointer(STRING_BUILDER* string_builder, const void* pointer, bool newline);

STRING_BUILDER* append_builder_vformat(STRING_BUILDER* string_builder, const TEXT* format, va_list ap);
STRING_BUILDER* append_builder_format(STRING_BUILDER* string_builder, const TEXT* format, ...);

#define append_builder_string_word(string_builder, s)       append_builder_string((string_builder), (s), false)
#define append_builder_text_word(string_builder, text)      append_builder_text((string_builder), (text), false)
#define append_builder_character_word(string_builder, c)     append_builder_character((string_builder), (c), false)
#define append_builder_int_word(string_builder, value)       append_builder_int((string_builder), (value), false)
#define append_builder_uint_word(string_builder, value)      append_builder_uint((string_builder), (value), false)
#define append_builder_bool_word(string_builder, value)      append_builder_bool((string_builder), (value), false)
#define append_builder_pointer_word(string_builder, pointer) append_builder_pointer((string_builder), pointer, false)

#define append_builder_string_newline(string_builder, s)       append_builder_string((string_builder), (s), true)
#define append_builder_text_newline(string_builder, text)      append_builder_text((string_builder), (text), true)
#define append_builder_character_newline(string_builder, c)     append_builder_character((string_builder), (c), true)
#define append_builder_int_newline(string_builder, value)       append_builder_int((string_builder), (value), true)
#define append_builder_uint_newline(string_builder, value)      append_builder_uint((string_builder), (value), true)
#define append_builder_bool_newline(string_builder, value)      append_builder_bool((string_builder), (value), true)
#define append_builder_pointer_newline(string_builder, pointer) append_builder_pointer((string_builder), pointer, true)
