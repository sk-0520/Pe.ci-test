﻿#include <assert.h>

#include "app_common.h"
#include "app_commandline.h"

EXECUTE_MODE get_execute_mode(const COMMAND_LINE_OPTION* command_line_option)
{
    assert(command_line_option);
    //TODO: 判定処理
    return EXECUTE_MODE_BOOT;
}

