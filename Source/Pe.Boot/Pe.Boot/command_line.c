#include <assert.h>

#include "command_line.h"
#include "memory.h"
#include "tstring.h"

static bool equals_command_line_item_key(const TEXT* a, const TEXT* b)
{
    return equals_map_key_default(a, b);
}

static void free_command_line_item_value(MAP_PAIR* pair)
{
    COMMAND_LINE_ITEM* item = (COMMAND_LINE_ITEM*)pair->value;

    free_text(&item->value);
    free_memory(item);
}

static void set_command_line_map_setting(MAP* map, size_t capacity)
{
    *map = create_map(capacity, equals_command_line_item_key, free_command_line_item_value);
}

static ssize_t get_key_mark_index(const TEXT* argument, TEXT mark_texts[], size_t count)
{
    for (size_t i = 0; i < count; i++) {
        const TEXT* mark_text = mark_texts + i;
        if (starts_with_text(argument, mark_text)) {
            return i;
        }
    }

    return -1;
}

static void convert_map_from_arguments(MAP* result, const TEXT arguments[], size_t count)
{
    TEXT mark_texts[] = {
        wrap_text(_T("--")),
        wrap_text(_T("-")),
        wrap_text(_T("/")),
    };

    for (size_t i = 0; i < count; i++) {
        const TEXT* current = &arguments[i];

        ssize_t mark_index = get_key_mark_index(current, mark_texts, SIZEOF_ARRAY(mark_texts));
        if (mark_index == -1) {
            // キー不明の値は無視する
            continue;
        }

        const TEXT* mark_text = mark_texts + mark_index;
        // 先頭のマークを外した引数取得
        TEXT arg = wrap_text_with_length(current->value + mark_text->length, current->length - mark_text->length, false);

        COMMAND_LINE_ITEM* item = allocate_clear_memory(1, sizeof(COMMAND_LINE_ITEM));
        item->key_index = i;

        TEXT key;
        TEXT value_with_separator = find_character(&arg, _T('='));
        if (is_enabled_text(&value_with_separator)) {
            // 引数が値とキーを持つ
            key = new_text_with_length(arg.value, (value_with_separator.value - arg.value));
            if (1 < value_with_separator.length) {
                // 値は存在する
                TEXT rawValue = wrap_text_with_length(value_with_separator.value + 1, value_with_separator.length - 1, false);
                if (rawValue.length && ((rawValue.value[0] == '"' || rawValue.value[0] == '\'') && rawValue.value[rawValue.length - 1] == rawValue.value[0]) ) {
                    // 囲まれている
                    item->value = new_text_with_length(rawValue.value + 1, rawValue.length - 2);
                } else {
                    item->value = rawValue;
                }
            } else {
                // 値は存在しない、が=指定されてるなら空文字列
                item->value = new_empty_text();
            }
            item->value_index = i;
        } else if(i + 1 < count) {
            key = arg;
            // 値は次要素を用いる
            ssize_t next_mark_index = get_key_mark_index(current + 1, mark_texts, SIZEOF_ARRAY(mark_texts));
            if (next_mark_index != -1) {
                // キーっぽいので値なし引数扱いにする
                item->value_index = 0;
                item->value = create_invalid_text();
            } else {
                // 次要素を値として取り込む
                item->value_index = i + 1;
                item->value = clone_text(current + 1);
                // 次要素をスキップ
                i += 1;
            }
        } else {
            key = arg;
            // 次要素はないのでキーのみを用いる
            item->value_index = 0;
            item->value = create_invalid_text();
        }

        add_map(result, &key, item, true);
        free_text(&key);
    }
}

COMMAND_LINE_OPTION parse_command_line(const TEXT* command_line, bool with_command)
{
    if (!command_line || !command_line->length) {
        COMMAND_LINE_OPTION empty;
        set_memory(&empty, 0, sizeof(empty));
        set_command_line_map_setting(&empty.library.map, 2);
        return empty;
    }

    int temp_argc;
    TCHAR** argv = CommandLineToArgvW(command_line->value, &temp_argc);
    size_t argc = (size_t)temp_argc;

    TEXT* arguments = allocate_memory(argc * sizeof(TEXT), false);
    for (size_t i = 0; i < argc; i++) {
        TCHAR* arg = argv[i];
        arguments[i] = wrap_text(arg);
    }

    size_t count;
    TEXT* arguments_without_command;
    TEXT* command;
    if (with_command) {
        if (1 < argc) {
            arguments_without_command = arguments + 1;
        } else {
            arguments_without_command = NULL;
        }
        count = argc - 1;
        command = arguments;
    } else {
        arguments_without_command = arguments;
        count = argc;
        command = NULL;
    }

    COMMAND_LINE_OPTION result = {
        .arguments = arguments_without_command,
        .count = count,
        .library = {
            .argv = argv,
            .raw_arguments = arguments,
            .raw_count = argc,
            .command = command,
        },
    };
    set_command_line_map_setting(&result.library.map, result.count);
    convert_map_from_arguments(&result.library.map, result.arguments, result.count);

    return result;
}

void free_command_line(COMMAND_LINE_OPTION* command_line_option)
{
    free_map(&command_line_option->library.map);

    free_memory(command_line_option->library.raw_arguments);
    command_line_option->library.raw_arguments = NULL;

    LocalFree((HLOCAL)command_line_option->library.argv);
    command_line_option->library.argv = NULL;
}

const COMMAND_LINE_ITEM* get_command_line_item(const COMMAND_LINE_OPTION* command_line_option, const TEXT* key)
{
    MAP_RESULT_VALUE result_value = get_map(&command_line_option->library.map, key);
    if (result_value.exists) {
        return (COMMAND_LINE_ITEM*)result_value.value;
    }

    return NULL;
}

bool has_value_command_line_item(const COMMAND_LINE_ITEM* item)
{
    if (!item) {
        return false;
    }

    return is_enabled_text(&item->value);
}


TCHAR* tuneArg(const TCHAR* arg)
{
    int hasSpace = findCharacter(arg, ' ') != NULL;
    size_t len = (size_t)get_string_length(arg) + (hasSpace ? 2 : 0);
    TCHAR* s = allocate_clear_memory(len + 1, sizeof(TCHAR*));
    assert(s);
    if (hasSpace) {
        copy_string(s + 1, arg);
        s[0] = '"';
        s[len - 1] = '"';
        s[len - 0] = 0; // ↑で +1 してるから安全安全
    } else {
        copy_string(s, arg);
    }
    return s;
}
