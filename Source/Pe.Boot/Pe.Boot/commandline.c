#include <assert.h>

#include "commandline.h"
#include "memory.h"
#include "tstring.h"


COMMAND_LINE_OPTION parseCommandLine(const TEXT* commandLine)
{
    int tempArgc;
    TCHAR** argv = CommandLineToArgvW(commandLine->value, &tempArgc);
    size_t argc = (size_t)tempArgc;

    TEXT* arguments = allocateMemory(argc * sizeof(TEXT), false);
    for (size_t i = 0; i < argc; i++) {
        TCHAR* arg = argv[i];
        arguments[i] = wrapText(arg);
    }

    //size_t maxItemLength = (argc - 1) / 2;
    //COMMAND_LINE_ITEM* items = allocateClearMemory(maxItemLength, sizeof(COMMAND_LINE_ITEM));
    //for (size_t i = 0; i < maxItemLength; i++) {

    //}

    COMMAND_LINE_OPTION result = {
        .arguments = arguments,
        .count = argc,
        ._mng = {
            .argv = argv,
        }
    };

    return result;
}

void freeCommandLine(const COMMAND_LINE_OPTION* commandLineOption)
{
    freeMemory((void*)commandLineOption->arguments);
    LocalFree((HLOCAL)commandLineOption->_mng.argv);
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
