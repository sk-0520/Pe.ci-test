#include <assert.h>

#include "app_command_line.h"

EXECUTE_MODE get_execute_mode(const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT key = wrap_text(_T("_mode"));
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
        { EXECUTE_MODE_BOOT, wrap_text(_T("boot")), }
    };

    for (size_t i = 0; i < SIZEOF_ARRAY(mode_mappings); i++) {
        if (!compare_text(&mode_mappings[i].value, &item->value, true)) {
            return mode_mappings[i].mode;
        }
    }

    return EXECUTE_MODE_BOOT;
}

const COMMAND_LINE_ITEM* get_wait_time_item(const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT wait_keys[] = {
        wrap_text(_T("_boot-wait")),
        wrap_text(_T("wait")), //TODO: #737 互換用処理
    };
    for (size_t i = 0; i < SIZEOF_ARRAY(wait_keys); i++) {
        const TEXT* wait_key = wait_keys + i;
        const COMMAND_LINE_ITEM* item = get_command_line_item(command_line_option, wait_key);
        if (item) {
            return item;
        }
    }

    return NULL;
}

