#pragma once
#include <tchar.h>

#include "common.h"
#include "text.h"
#include "map.h"


/// <summary>
/// コマンドラインキーの識別子。
/// </summary>
typedef enum tag_COMMAND_LINE_MARK
{
    /// <summary>
    /// -
    /// </summary>
    COMMAND_LINE_MARK_SHORT,
    /// <summary>
    /// --
    /// </summary>
    COMMAND_LINE_MARK_LONG,
    /// <summary>
    /// /
    /// </summary>
    COMMAND_LINE_MARK_DOS,
} COMMAND_LINE_MARK;

/// <summary>
/// コマンドラインの値。
/// </summary>
typedef struct tag_COMMAND_LINE_ITEM
{
    /// <summary>
    /// コマンドラインキーの識別子。
    /// </summary>
    COMMAND_LINE_MARK mark;
    TEXT* values;
    size_t length;
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
    /// キーと値のマッピング。
    /// <para>キー項目のみは値がない。</para>
    /// <para>Pe としては値だけを考慮する必要なし。</para>
    /// </summary>
    MAP map;

    /// <summary>
   /// 管理データ。
   /// </summary>
    struct
    {
        /// <summary>
        ///解放用データ。
        /// </summary>
        TCHAR** argv;
        /// <summary>
        /// 解放用テキストデータ一覧。
        /// </summary>
        TEXT* rawArguments;
        size_t rawCount;
        /// <summary>
        /// 起動コマンド。
        /// </summary>
        TEXT* command;
    } library;
} COMMAND_LINE_OPTION;

/// <summary>
/// コマンドライン文字列を分解。
/// </summary>
/// <param name="commandLine"></param>
/// <param name="withCommand">commandLineに起動コマンド(プログラム)が含まれているか</param>
/// <returns>分解結果。freeCommandLine による開放が必要。</returns>
COMMAND_LINE_OPTION parseCommandLine(const TEXT* commandLine, bool withCommand);

/// <summary>
/// コマンドラインオプションを解放。
/// </summary>
/// <param name="commandLineOption"></param>
void freeCommandLine(const COMMAND_LINE_OPTION* commandLineOption);

/// <summary>
/// 書式調整後の動的確保された文字列を返す。
/// </summary>
/// <param name="arg"></param>
/// <returns>呼び出し側で世話すること。</returns>
TCHAR* tuneArg(const TCHAR* arg);
