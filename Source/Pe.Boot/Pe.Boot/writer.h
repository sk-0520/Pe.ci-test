#pragma once
#include <stdint.h>
#include <stdbool.h>

#include <tchar.h>

#include "common.h"

/*
* データ書き込み処理の抽象化を担当
* TSTRING_BUILDER へのあれこれが基本の呼び出し口
*/

/// <summary>
/// 書き込み文字データ。
/// </summary>
typedef struct tag_WRITE_CHARACTER_DATA
{
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

/// <summary>
/// 単一文字列書き出し。
/// <para>書き込み成功状態を返す。</para>
/// </summary>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="data">書き込みデータ。</param>
/// <returns>処理成功状態。</returns>
typedef bool (*func_character_writer)(void* receiver, const WRITE_CHARACTER_DATA* data);

/// <summary>
/// 文字列書き出し。
/// <para>書き込み成功状態を返す。</para>
/// </summary>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="data">書き込みデータ。</param>
/// <returns>処理成功状態。</returns>
typedef bool (*func_string_writer)(void* receiver, const WRITE_STRING_DATA* data);

/// <summary>
/// 真偽データを書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="value">真偽値。</param>
/// <param name="is_uppper">大文字にするか。</param>
/// <returns>成功状態。</returns>
bool write_to_primitive_boolean(func_string_writer writer, void* receiver, bool value, bool is_uppper);

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
bool write_primitive_integer(func_string_writer writer, void* receiver, ssize_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator);

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
bool write_primitive_uinteger(func_string_writer writer, void* receiver, size_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator);

/// <summary>
/// 文字を書き込み。
/// </summary>
/// <param name="writer">書き込みデータ送信処理。</param>
/// <param name="receiver">書き込み対象データ受信処理。</param>
/// <param name="character">文字。</param>
/// <param name="write_align">埋め処理時の揃え方向。</param>
/// <param name="width">表示幅。</param>
/// <returns>成功状態。</returns>
bool write_primitive_character(func_string_writer writer, void* receiver, TCHAR character, WRITE_ALIGN write_align, size_t width);

