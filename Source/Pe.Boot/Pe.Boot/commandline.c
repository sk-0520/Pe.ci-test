#include <assert.h>

#include "commandline.h"
#include "memory.h"
#include "tstring.h"


COMMAND_LINE_OPTION parseCommandLine(const TCHAR* commandLine)
{
    int tempArgc;
    TCHAR** argv = CommandLineToArgvW(commandLine, &tempArgc);
    size_t argc = (size_t)tempArgc;

    size_t maxItemLength = (argc - 1) / 2;
    COMMAND_LINE_ITEM* items = allocateClearMemory(maxItemLength, sizeof(COMMAND_LINE_ITEM));
    for (size_t i = 0; i < maxItemLength; i++) {

    }

    COMMAND_LINE_OPTION result = {
        argv,
        argc
    };

    return result;
}

void freeCommandLine(const COMMAND_LINE_OPTION* commandLineOption)
{
    LocalFree((HLOCAL)commandLineOption->arguments);
}

EXECUTE_MODE getExecuteMode(const COMMAND_LINE_OPTION* commandLineOption)
{
    assert(commandLineOption);
    //TODO: 判定処理
    return EXECUTE_MODE_BOOT;
}


TCHAR* tuneArg(const TCHAR* arg)
{
    int hasSpace = findCharacter(arg, ' ') != NULL;
    size_t len = (size_t)getStringLength(arg) + (hasSpace ? 2 : 0);
    TCHAR* s = allocateClearMemory(len + 1, sizeof(TCHAR*));
    assert(s);
    if (hasSpace) {
        copyString(s + 1, arg);
        s[0] = '"';
        s[len - 1] = '"';
        s[len - 0] = 0; // ↑で +1 してるから安全安全
    } else {
        copyString(s, arg);
    }
    return s;
}
