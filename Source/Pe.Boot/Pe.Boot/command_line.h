﻿#pragma once
#include <tchar.h>

#include "common.h"
#include "text.h"
#include "map.h"


/// <summary>
/// コマンドラインの値。
/// </summary>
typedef struct tag_COMMAND_LINE_ITEM
{
    /// <summary>
    /// COMMAND_LINE_OPTION.arguments から見たキーの位置。
    /// </summary>
    size_t key_index;
    /// <summary>
    /// COMMAND_LINE_OPTION.arguments から見た値の位置。
    /// <para>keyIndex以上になる(=区切りだと同じ)。</para>
    /// </summary>
    size_t value_index;
    /// <summary>
    /// 値データ。
    /// <para>値がない場合は無効テキスト。</para>
    /// </summary>
    TEXT value;
} COMMAND_LINE_ITEM;

/// <summary>
/// コマンドラインオプション。
/// </summary>
typedef struct tag_COMMAND_LINE_OPTION
{
    /// <summary>
    /// 引数一覧。
    /// <para>起動コマンドは含まない。</para>
    /// </summary>
    const TEXT* arguments;
    /// <summary>
    /// argumentsの個数。
    /// </summary>
    size_t count;

    /// <summary>
   /// 管理データ。
   /// </summary>
    struct
    {
        /// <summary>
        /// キーと値のマッピング。
        /// <para>キー項目のみは値がない。</para>
        /// <para>同一キーは前が優先される。</para>
        /// </summary>
        MAP map;
        /// <summary>
        ///解放用データ。
        /// </summary>
        TCHAR** argv;
        /// <summary>
        /// 解放用テキストデータ一覧。
        /// </summary>
        TEXT* raw_arguments;
        size_t raw_count;
        /// <summary>
        /// 起動コマンド。
        /// </summary>
        TEXT* command;
    } library;
} COMMAND_LINE_OPTION;

/// <summary>
/// コマンドライン文字列を分解。
/// </summary>
/// <param name="command_line"></param>
/// <param name="with_command">commandLineに起動コマンド(プログラム)が含まれているか</param>
/// <returns>分解結果。freeCommandLine による開放が必要。</returns>
COMMAND_LINE_OPTION RC_HEAP_FUNC(parse_command_line, const TEXT* command_line, bool with_command);
#ifdef RES_CHECK
#   define parse_command_line(command_line, with_command) RC_HEAP_WRAP(parse_command_line, (command_line), (with_command))
#endif

/// <summary>
/// コマンドラインオプションを解放。
/// </summary>
/// <param name="commandLineOption"></param>
bool RC_HEAP_FUNC(free_command_line, COMMAND_LINE_OPTION* command_line_option);
#ifdef RES_CHECK
#   define free_command_line(command_line_option) RC_HEAP_WRAP(free_command_line, (command_line_option))
#endif

/// <summary>
/// コマンドラインアイテムを取得する。
/// </summary>
/// <param name="commandLineOption"></param>
/// <param name="key"></param>
/// <returns>取得したコマンドラインアイテム。アイテムが存在しない場合はNULL。</returns>
const COMMAND_LINE_ITEM* get_command_line_item(const COMMAND_LINE_OPTION* command_line_option, const TEXT* key);

/// <summary>
/// コマンドラインアイテムは値を持つか。
/// <para>空文字列も許容しているので文字列長も含めてチェックする場合は<c>is_inputed_command_line_item</c>を使用すること。</para>
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
bool has_value_command_line_item(const COMMAND_LINE_ITEM* item);

/// <summary>
/// コマンドラインアイテムは非空白の値を持つか。
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
bool is_inputed_command_line_item(const COMMAND_LINE_ITEM* item);

/// <summary>
/// コマンドライン引数一覧から単体テキストとしてのコマンドライン引数を生成する。
/// <para>TODO: "が閉じられていないパターンとか " 自体のエスケープが未対応</para>
/// </summary>
/// <param name="arguments">コマンドライン引数一覧</param>
/// <param name="count">argumentの個数</param>
/// <returns>生成テキスト。解放が必要。</returns>
TEXT to_command_line_argument(const TEXT_LIST arguments, size_t count);

