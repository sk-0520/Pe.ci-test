#include <assert.h>

#include "app_common.h"
#include "app_commandline.h"

EXECUTE_MODE getExecuteMode(const COMMAND_LINE_OPTION* commandLineOption)
{
    assert(commandLineOption);
    //TODO: 判定処理
    return EXECUTE_MODE_BOOT;
}

