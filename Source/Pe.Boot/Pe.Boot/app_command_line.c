#include "../Pe.Library/debug.h"
#include "../Pe.Library/logging.h"
#include "app_command_line.h"

EXECUTE_MODE get_execute_mode(const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT key = wrap_text(OPTION_APP_MODE_KEY);
    const COMMAND_LINE_ITEM* item = get_command_line_item(command_line_option, &key);

    if (!has_value_command_line_item(item)) {
        return EXECUTE_MODE_BOOT;
    }
    if (is_whitespace_text(&item->value)) {
        return EXECUTE_MODE_BOOT;
    }

    struct
    {
        EXECUTE_MODE mode;
        TEXT value;
    } mode_mappings[] = {
        { EXECUTE_MODE_BOOT, wrap_text(_T("boot")), },
        { EXECUTE_MODE_DRY_RUN, wrap_text(_T("dry-run")), },
        { EXECUTE_MODE_CONSOLE, wrap_text(_T("console")), },
    };

    for (size_t i = 0; i < SIZEOF_ARRAY(mode_mappings); i++) {
        if (!compare_text(&mode_mappings[i].value, &item->value, true)) {
            return mode_mappings[i].mode;
        }
    }

    return EXECUTE_MODE_BOOT;
}

WAIT_TIME_ARG get_wait_time(const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT wait_keys[] = {
        wrap_text(OPTION_APP_BOOT_WAIT_KEY),
    };

    for (size_t i = 0; i < SIZEOF_ARRAY(wait_keys); i++) {
        const TEXT* wait_key = wait_keys + i;
        const COMMAND_LINE_ITEM* item = get_command_line_item(command_line_option, wait_key);
        if (item) {
            WAIT_TIME_ARG result = {
                .item = item,
            };
            if (is_inputted_command_line_item(item)) {
                TEXT_PARSED_I32_RESULT time_result = parse_i32_from_text(&item->value, PARSE_BASE_NUMBER_D);
                result.enabled = time_result.success && 0 < time_result.value;
                if (result.enabled) {
                    result.time = time_result.value;
                }
            } else {
                result.enabled = false;
            }

            return result;
        }
    }

    WAIT_TIME_ARG none = {
        .item = NULL,
        .enabled = false,
    };

    return none;
}

size_t filter_enable_command_line_items(TEXT_LIST result, const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT ignore_command_line_item_keys[] = {
        wrap_text(OPTION_LOG_FILE_KEY),
        wrap_text(OPTION_LOG_LEVEL_KEY),
        wrap_text(OPTION_APP_MODE_KEY),
        wrap_text(OPTION_APP_BOOT_WAIT_KEY),
    };

    size_t ignore_length = 0;
    const COMMAND_LINE_ITEM* ignore_command_line_items[SIZEOF_ARRAY(ignore_command_line_item_keys)] = { NULL, };
    for (size_t i = 0; i < SIZEOF_ARRAY(ignore_command_line_item_keys); i++) {
        const COMMAND_LINE_ITEM* item = get_command_line_item(command_line_option, ignore_command_line_item_keys + i);
        if (item) {
            ignore_command_line_items[ignore_length++] = item;
        }
    }

    size_t arg_count = 0;
    for (size_t option_index = 0; option_index < command_line_option->count; option_index++) {
        bool ignore = false;
        for (size_t ignore_index = 0; ignore_index < ignore_length; ignore_index++) {
            const COMMAND_LINE_ITEM* ignore_command_line_item = ignore_command_line_items[ignore_index];
            if (ignore_command_line_item->key_index == option_index || ignore_command_line_item->value_index == option_index) {
                ignore = true;
                break;
            }
        }
        if (ignore) {
            continue;
        }

        result[arg_count++] = command_line_option->arguments[option_index];
    }

    return arg_count;
}
