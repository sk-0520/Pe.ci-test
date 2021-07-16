#include <assert.h>

#include "commandline.h"
#include "memory.h"
#include "tstring.h"

static bool equalsCommandLineItemKey(const TEXT* a, const TEXT* b)
{
    return equalsMapKeyDefault(a, b);
}

static void freeCommandLineItemValue(MAP_PAIR* pair)
{
    COMMAND_LINE_ITEM* item = (COMMAND_LINE_ITEM*)pair->value;

    freeMemory(item->values);
    freeMemory(item);
}

static void setCommandLineMapSetting(MAP* map)
{
    map->library.equalsMapKey = equalsCommandLineItemKey;
    map->library.freeValue = freeCommandLineItemValue;
}

static ssize_t getKeyMarkIndex(const TEXT* argument, TEXT markTexts[], size_t count)
{
    for (size_t i = 0; i < count; i++) {
        const TEXT* markText = markTexts + i;
        if (startsWithText(argument, markText)) {
            return i;
        }
    }

    return -1;
}

static void convertMapFromArguments(MAP* result, const TEXT arguments[], size_t count)
{
    TEXT markTexts[] = {
        wrapText(_T("--")),
        wrapText(_T("-")),
        wrapText(_T("/")),
    };
    //COMMAND_LINE_MARK markValues[] = {
    //    COMMAND_LINE_MARK_LONG,
    //    COMMAND_LINE_MARK_SHORT,
    //    COMMAND_LINE_MARK_DOS,
    //};

    for (size_t i = 0; i < count; i++) {
        //bool canNext = i + 1 < count;

        const TEXT* current = &arguments[i];

        ssize_t markIndex = getKeyMarkIndex(current, markTexts, SIZEOF_ARRAY(markTexts));
        if (markIndex == -1) {
            // キー不明の値は無視する
            continue;
        }

        const TEXT* markText = markTexts + markIndex;
        TEXT arg = wrapTextWithLength(current->value + markText->length, current->length - markText->length, false);
        assert(&arg);
    }
}

COMMAND_LINE_OPTION parseCommandLine(const TEXT* commandLine, bool withCommand)
{
    if (!commandLine || !commandLine->length) {
        COMMAND_LINE_OPTION empty;
        setMemory(&empty, 0, sizeof(empty));
        setCommandLineMapSetting(&empty.map);
        empty.map.library.capacity = 2;
        return empty;
    }

    int tempArgc;
    TCHAR** argv = CommandLineToArgvW(commandLine->value, &tempArgc);
    size_t argc = (size_t)tempArgc;

    TEXT* arguments = allocateMemory(argc * sizeof(TEXT), false);
    for (size_t i = 0; i < argc; i++) {
        TCHAR* arg = argv[i];
        arguments[i] = wrapText(arg);
    }

    size_t count;
    TEXT* argumentsWithoutCommand;
    TEXT* command;
    if (withCommand) {
        if (1 < argc) {
            argumentsWithoutCommand = arguments + 1;
        } else {
            argumentsWithoutCommand = NULL;
        }
        count = argc - 1;
        command = arguments;
    } else {
        argumentsWithoutCommand = arguments;
        count = argc;
        command = NULL;
    }

    COMMAND_LINE_OPTION result = {
        .arguments = argumentsWithoutCommand,
        .count = count,
        .library = {
            .argv = argv,
            .rawArguments = arguments,
            .rawCount = argc,
            .command = command,
        },
    };
    convertMapFromArguments(&result.map, result.arguments, result.count);

    return result;
}

void freeCommandLine(const COMMAND_LINE_OPTION* commandLineOption)
{
    freeMemory((void*)commandLineOption->library.rawArguments);
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
