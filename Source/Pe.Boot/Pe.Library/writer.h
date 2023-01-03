#pragma once
#include <stdint.h>
#include <stdbool.h>

#include <tchar.h>

#include "common.h"
#include "text.h"

/*
* データ書き込み処理の抽象化を担当
* TSTRING_BUILDER へのあれこれが基本の呼び出し口
*/

typedef enum tag_WRITE_ERROR_KIND
{
    WRITE_ERROR_KIND_NONE,
    WRITE_ERROR_KIND_UNKNOWN,
} WRITE_ERROR_KIND;

typedef struct tag_WRITE_RESULT
{
    size_t write_length;
    WRITE_ERROR_KIND error;
} WRITE_RESULT;

/// <summary>
/// 書き込み文字データ。
/// </summary>
typedef struct tag_WRITE_CHARACTER_DATA
{
    /// <summary>
    /// 書き込み対象データ受信処理。
    /// </summary>
    void* receiver;
    /// <summary>
    /// 書き込み文字。
    /// </summary>
    TCHAR value;
} WRITE_CHARACTER_DATA;

/// <summary>
/// 書き込み文字列データ。
/// </summary>
typedef struct tag_WRITE_STRING_DATA
{
    /// <summary>
    /// 書き込み対象データ受信処理。
    /// </summary>
    void* receiver;
    /// <summary>
    /// 書き込み文字列(番兵なし)。
    /// </summary>
    const TCHAR* value;
    /// <summary>
    /// <c>value</c>の長さ。
    /// </summary>
    size_t length;
} WRITE_STRING_DATA;

/// <summary>
/// 埋め方。
/// </summary>
typedef enum tag_WRITE_PADDING
{
    /// <summary>
    /// 0埋め。
    /// </summary>
    WRITE_PADDING_ZERO,
    /// <summary>
    /// スペース埋め。
    /// </summary>
    WRITE_PADDING_SPACE,
} WRITE_PADDING;

/// <summary>
/// 揃え方。
/// </summary>
typedef enum tag_WRITE_ALIGN
{
    /// <summary>
    /// 左揃え。
    /// </summary>
    WRITE_ALIGN_LEFT,
    /// <summary>
    /// 右揃え。
    /// </summary>
    WRITE_ALIGN_RIGHT,
} WRITE_ALIGN;

typedef struct tag_WRITE_FORMAT_FLAGS
{
    WRITE_ALIGN align;
    WRITE_PADDING padding;
    bool alternate_form;
    bool show_sign;
} WRITE_FORMAT_FLAGS;

/// <summary>
/// 単一文字列書き出し。
/// <para>書き込み成功状態を返す。</para>
/// </summary>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="data">書き込みデータ。</param>
/// <returns>書き込んだデータ数。負数は<c>WRITE_ERROR_KIND</c>を使用する。</returns>
typedef WRITE_RESULT (*func_character_writer)(const WRITE_CHARACTER_DATA* data);

/// <summary>
/// 文字列書き出し。
/// <para>書き込み成功状態を返す。</para>
/// </summary>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="data">書き込みデータ。</param>
/// <returns>処理成功状態。</returns>
typedef WRITE_RESULT(*func_string_writer)(const WRITE_STRING_DATA* data);

bool is_success_write(const WRITE_RESULT* write_result);

WRITE_RESULT write_success(size_t length);

WRITE_RESULT write_failed(WRITE_ERROR_KIND kind);

/// <summary>
/// 真偽データを書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="value">真偽値。</param>
/// <param name="is_uppper">大文字にするか。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_primitive_boolean(func_string_writer writer, void* receiver, bool value, bool is_uppper);

/// <summary>
/// <c>ssize_t</c>の数値を書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="value">数値。</param>
/// <param name="write_padding">埋め処理を方法。</param>
/// <param name="write_align">埋め処理時の揃え方向。</param>
/// <param name="show_sign">符号を常に表示するか。偽の場合でも負数は表示される。</param>
/// <param name="width">表示幅。</param>
/// <param name="separator">区切り文字。NUL文字の場合区切りなしとする。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_primitive_integer(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, ssize_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator);

/// <summary>
/// <c>size_t</c>の数値を書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="value">数値。</param>
/// <param name="write_padding">埋め処理を方法。</param>
/// <param name="write_align">埋め処理時の揃え方向。</param>
/// <param name="show_sign">符号を常に表示するか。偽の場合でも負数は表示される。</param>
/// <param name="width">表示幅。</param>
/// <param name="separator">区切り文字。NUL文字の場合区切りなしとする。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_primitive_uinteger(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, size_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator);

/// <summary>
/// <c>ssize_t</c>の16進数を書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="value">数値。</param>
/// <param name="write_padding">埋め処理を方法。</param>
/// <param name="write_align">埋め処理時の揃え方向。</param>
/// <param name="is_upper">大文字で表示するか。</param>
/// <param name="alternate_form">0x(X)を付与するか。</param>
/// <param name="width">表示幅。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_primitive_hex(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, ssize_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool is_upper, bool alternate_form, size_t width);
/// <summary>
/// <c>size_t</c>の16進数を書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="value">数値。</param>
/// <param name="write_padding">埋め処理を方法。</param>
/// <param name="write_align">埋め処理時の揃え方向。</param>
/// <param name="is_upper">大文字で表示するか。</param>
/// <param name="alternate_form">0x(X)を付与するか。</param>
/// <param name="width">表示幅。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_primitive_uhex(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, size_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool is_upper, bool alternate_form, size_t width);

/// <summary>
/// 文字を書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="character">文字。</param>
/// <param name="write_align">埋め処理時の揃え方向。</param>
/// <param name="width">表示幅。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_primitive_character(func_string_writer writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, TCHAR character, WRITE_ALIGN write_align, size_t width);

/// <summary>
/// ポインタの表示。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="pointer"></param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_primitive_pointer(func_string_writer writer, void* receiver, const void* pointer);

/// <summary>
/// 文字列の表示。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="string">文字列。</param>
/// <param name="write_align">(未実装)埋め処理時の揃え方向。</param>
/// <param name="width">(未実装)表示幅。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_address_string(func_string_writer writer, void* receiver, const TCHAR* string, WRITE_ALIGN write_align, size_t width);

/// <summary>
/// テキストの表示。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="string">テキスト。</param>
/// <param name="write_align">(未実装)埋め処理時の揃え方向。</param>
/// <param name="width">(未実装)表示幅。</param>
/// <returns>成功状態。</returns>
WRITE_RESULT write_address_text(func_string_writer writer, void* receiver, const TEXT* text, WRITE_ALIGN write_align, size_t width);

/// <summary>
/// 書式フラグの取得。
/// </summary>
/// <param name="result">書式フラグ格納先。</param>
/// <param name="format">書式テキスト。</param>
/// <returns>書式解析時に使用した長さ。</returns>
size_t get_write_format_flags(WRITE_FORMAT_FLAGS* result, const TEXT* format);

/// <summary>
/// 書式化。
/// * FLAG
///   * -+ #0
/// * 長さ
///   * z: ビット幅
/// * 型
///   * d, i: ssize_t
///   * u: size_t
///   * x, X: size_t
///   * c: TCHAR
///   * p: void*
///   * s: TCHAR*
///   * b, B: bool
///   * t: TEXT*
/// </summary>
/// <param name="string_writer">文字列書き込みデータ送信処理。</param>
/// <param name="character_writer">文字書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="format">書式。</param>
/// <param name="ap">パラメータ。</param>
/// <returns>成功状態。</returns>
bool write_vformat(func_string_writer string_writer, func_character_writer character_writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, const TEXT* format, va_list ap);

/// <summary>
/// 書式化。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="format">書式。</param>
/// <returns>成功状態。</returns>
bool write_format(func_string_writer string_writer, func_character_writer character_writer, void* receiver, const MEMORY_ARENA_RESOURCE* memory_arena_resource, const TEXT* format, ...);

