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

    freeText(&item->value);
    freeMemory(item);
}

static void setCommandLineMapSetting(MAP* map, size_t capacity)
{
    *map = createMap(capacity, equalsCommandLineItemKey, freeCommandLineItemValue);
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

    for (size_t i = 0; i < count; i++) {
        const TEXT* current = &arguments[i];

        ssize_t markIndex = getKeyMarkIndex(current, markTexts, SIZEOF_ARRAY(markTexts));
        if (markIndex == -1) {
            // キー不明の値は無視する
            continue;
        }

        const TEXT* markText = markTexts + markIndex;
        // 先頭のマークを外した引数取得
        TEXT arg = wrapTextWithLength(current->value + markText->length, current->length - markText->length, false);

        COMMAND_LINE_ITEM* item = allocateClearMemory(1, sizeof(COMMAND_LINE_ITEM));
        item->keyIndex = i;

        TEXT key;
        TEXT valueWithSeparator = findCharacter2(&arg, _T('='));
        if (isEnabledText(&valueWithSeparator)) {
            // 引数が値とキーを持つ
            key = newTextWithLength(arg.value, (valueWithSeparator.value - arg.value));
            if (1 < valueWithSeparator.length) {
                // 値は存在する
                TEXT rawValue = wrapTextWithLength(valueWithSeparator.value + 1, valueWithSeparator.length - 1, false);
                if (rawValue.length && ((rawValue.value[0] == '"' || rawValue.value[0] == '\'') && rawValue.value[rawValue.length - 1] == rawValue.value[0]) ) {
                    // 囲まれている
                    item->value = newTextWithLength(rawValue.value + 1, rawValue.length - 2);
                } else {
                    item->value = rawValue;
                }
            } else {
                // 値は存在しない、が=指定されてるなら空文字列
                item->value = newEmptyText();
            }
            item->valueIndex = i;
        } else if(i + 1 < count) {
            key = arg;
            // 値は次要素を用いる
            ssize_t nextMarkIndex = getKeyMarkIndex(current + 1, markTexts, SIZEOF_ARRAY(markTexts));
            if (nextMarkIndex != -1) {
                // キーっぽいので値なし引数扱いにする
                item->valueIndex = 0;
                item->value = createInvalidText();
            } else {
                // 次要素を値として取り込む
                item->valueIndex = i + 1;
                item->value = cloneText(current + 1);
                // 次要素をスキップ
                i += 1;
            }
        } else {
            key = arg;
            // 次要素はないのでキーのみを用いる
            item->valueIndex = 0;
            item->value = createInvalidText();
        }

        addMap(result, &key, item, true);
        freeText(&key);
    }
}

COMMAND_LINE_OPTION parseCommandLine(const TEXT* commandLine, bool withCommand)
{
    if (!commandLine || !commandLine->length) {
        COMMAND_LINE_OPTION empty;
        setMemory(&empty, 0, sizeof(empty));
        setCommandLineMapSetting(&empty.library.map, 2);
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
    setCommandLineMapSetting(&result.library.map, result.count);
    convertMapFromArguments(&result.library.map, result.arguments, result.count);

    return result;
}

void freeCommandLine(COMMAND_LINE_OPTION* commandLineOption)
{
    freeMap(&commandLineOption->library.map);

    freeMemory(commandLineOption->library.rawArguments);
    commandLineOption->library.rawArguments = NULL;

    LocalFree((HLOCAL)commandLineOption->library.argv);
    commandLineOption->library.argv = NULL;
}

const COMMAND_LINE_ITEM* getCommandLineItem(const COMMAND_LINE_OPTION* commandLineOption, const TEXT* key)
{
    MAP_RESULT_VALUE resultValue = getMap(&commandLineOption->library.map, key);
    if (resultValue.exists) {
        return (COMMAND_LINE_ITEM*)resultValue.value;
    }

    return NULL;
}

bool hasValueCommandLineItem(const COMMAND_LINE_ITEM* item)
{
    if (!item) {
        return false;
    }

    return isEnabledText(&item->value);
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
