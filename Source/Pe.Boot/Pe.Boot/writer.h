#pragma once
#include <stdint.h>
#include <stdbool.h>

#include <tchar.h>


/*
* データ書き込み処理の抽象化を担当
* TSTRING_BUILDER へのあれこれが基本の呼び出し口
*/

typedef struct tag_WRITE_CHARACTER_DATA
{
    TCHAR value;
} WRITE_CHARACTER_DATA;

typedef struct tag_WRITE_STRING_DATA
{
    const TCHAR* value;
    size_t length;
} WRITE_STRING_DATA;


/// <summary>
/// 単一文字列書き出し。
/// <para>書き込み成功状態を返す。</para>
/// </summary>
typedef bool (*func_character_writer)(const WRITE_CHARACTER_DATA* data);

/// <summary>
/// 文字列書き出し。
/// <para>書き込み成功状態を返す。</para>
/// </summary>
/// <param name="receiver">書き込み対象データ受信処理</param>
/// <param name="s">書き込み文字列（番兵なし）。</param>
/// <param name="length">書き込み文字列長。</param>
/// <returns></returns>
typedef bool (*func_string_writer)(void* receiver, const WRITE_STRING_DATA* data);

/// <summary>
/// 真偽データを書き込み。
/// </summary>
/// <param name="writer"></param>
/// <param name="value"></param>
/// <param name="is_uppper"></param>
/// <returns></returns>
bool write_to_primitive_boolean(func_string_writer writer, void* receiver, bool value, bool is_uppper);

