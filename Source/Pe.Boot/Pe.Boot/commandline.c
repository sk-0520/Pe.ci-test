#include <assert.h>

#include "commandline.h"
#include "memory.h"
#include "tstring.h"

static void convertMapFromArguments(MAP* result, const TEXT arguments[], size_t count)
{
    for (size_t i = 0; i < count; i++) {
        //bool canNext = i + 1 < count;
    }
}

COMMAND_LINE_OPTION parseCommandLine(const TEXT* commandLine, bool withCommand)
{
    int tempArgc;
    TCHAR** argv = CommandLineToArgvW(commandLine->value, &tempArgc);
    size_t argc = (size_t)tempArgc;

    TEXT* arguments = allocateMemory(argc * sizeof(TEXT), false);
    for (size_t i = 0; i < argc; i++) {
        TCHAR* arg = argv[i];
        arguments[i] = wrapText(arg);
    }

    TEXT* argumentsWithoutCommand;
    TEXT* command;
    if (withCommand) {
        if (1 < argc) {
            argumentsWithoutCommand = arguments + 1;
        } else {
            argumentsWithoutCommand = NULL;
        }
        command = arguments;
    } else {
        argumentsWithoutCommand = arguments;
        command = NULL;
    }

    //size_t maxItemLength = (argc - 1) / 2;
    //COMMAND_LINE_ITEM* items = allocateClearMemory(maxItemLength, sizeof(COMMAND_LINE_ITEM));
    //for (size_t i = 0; i < maxItemLength; i++) {

    //}

    COMMAND_LINE_OPTION result = {
        .arguments = argumentsWithoutCommand,
        .count = argc,
        .library = {
            .argv = argv,
            .rawTextArguments = arguments,
            .command = command,
        },
    };
    convertMapFromArguments(&result.pairs, result.arguments, result.count);

    return result;
}

void freeCommandLine(const COMMAND_LINE_OPTION* commandLineOption)
{
    freeMemory((void*)commandLineOption->library.rawTextArguments);
    LocalFree((HLOCAL)commandLineOption->library.argv);
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
