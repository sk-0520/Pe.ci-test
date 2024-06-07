#include <Windows.h>
#include "app_console.h"
#include "app_console_prompt.h"

CONSOLE_KIND get_console_kind(const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT key = wrap_text(_T("_console"));
    const COMMAND_LINE_ITEM* item = get_command_line_item(command_line_option, &key);

    if (!has_value_command_line_item(item)) {
        return CONSOLE_KIND_UNKNOWN;
    }
    if (is_whitespace_text(&item->value)) {
        return CONSOLE_KIND_UNKNOWN;
    }

    struct
    {
        CONSOLE_KIND kind;
        TEXT value;
    } kind_mappings[] = {
        { CONSOLE_KIND_PROMPT, wrap_text(_T("prompt")), },
    };

    for (size_t i = 0; i < SIZEOF_ARRAY(kind_mappings); i++) {
        if (!compare_text(&kind_mappings[i].value, &item->value, true)) {
            return kind_mappings[i].kind;
        }
    }

    return CONSOLE_KIND_UNKNOWN;
}

EXIT_CODE console_execute(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    CONSOLE_RESOURCE console_resource = begin_console(DEFAULT_MEMORY_ARENA);

    EXIT_CODE exit_code = EXIT_CODE_UNKNOWN_EXECUTE_MODE;
    CONSOLE_KIND console_kind = get_console_kind(command_line_option);
    switch (console_kind) {
        case CONSOLE_KIND_PROMPT:
            exit_code = console_execute_prompt(hInstance, command_line_option, &console_resource);
            break;

        default:
            break;
    }

    end_console(&console_resource);

    return exit_code;
}
