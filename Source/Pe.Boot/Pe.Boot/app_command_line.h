#pragma once
#include "command_line.h"
#include "app_common.h"

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
const COMMAND_LINE_ITEM* get_wait_time_item(const COMMAND_LINE_OPTION* command_line_option);
