#pragma once
#include "command_line.h"
#include "app_common.h"


/// <summary>
/// 起動待機時間設定格納領域。
/// </summary>
typedef struct tag_WAIT_TIME_ARG
{
    /// <summary>
    /// コマンドラインアイテム。
    /// <para><c>enabled</c>に関わらず発見した場合は設定される。</para>
    /// </summary>
    const COMMAND_LINE_ITEM* item;
    /// <summary>
    /// ミリ秒時間。
    /// </summary>
    __int32 time;
    /// <summary>
    /// <c>time</c>は有効か。
    /// </summary>
    bool enabled;
} WAIT_TIME_ARG;


/// <summary>
/// 実行モードを取得。
/// </summary>
/// <param name="command_line_option"></param>
/// <returns></returns>
EXECUTE_MODE get_execute_mode(const COMMAND_LINE_OPTION* command_line_option);

/// <summary>
/// 待機時間の取得。
/// </summary>
/// <param name="command_line_option"></param>
/// <returns></returns>
WAIT_TIME_ARG get_wait_time(const COMMAND_LINE_OPTION* command_line_option);
