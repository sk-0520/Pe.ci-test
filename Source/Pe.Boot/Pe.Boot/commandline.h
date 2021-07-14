#pragma once
#include <tchar.h>

#include "common.h"
#include "text.h"


typedef struct _TAG_COMMAND_LINE_ITEM
{
    const TCHAR* key;
    const TCHAR* value;
} COMMAND_LINE_ITEM;

/// <summary>
/// コマンドラインオプション。
/// </summary>
typedef struct _TAG_COMMAND_LINE_OPTION
{
    /// <summary>
    /// 引数一覧。
    /// </summary>
    const TEXT* arguments;
    /// <summary>
    /// 引数の個数。
    /// </summary>
    size_t count;

    struct
    {
        TCHAR** argv;
    } _mng;
} COMMAND_LINE_OPTION;

/// <summary>
/// コマンドライン文字列を分解。
/// </summary>
/// <param name="commandLine"></param>
/// <returns>分解結果。freeCommandLine による開放が必要。</returns>
COMMAND_LINE_OPTION parseCommandLine(const TEXT* commandLine);

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
