#include "app_boot.h"
#include "app_path.h"
#include "../Pe.Library/path.h"
#include "../Pe.Library/logging.h"
#include "../Pe.Library/debug.h"
#include "../Pe.Library/tstring.h"
#include "../Pe.Library/platform.h"
#include "../Pe.Library/command_line.h"
#include "app_command_line.h"

static EXIT_CODE boot_core(HINSTANCE hInstance, const TEXT* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    ShellExecute(NULL, _T("open"), app_path_items.main_module.value, is_enabled_text(command_line) ? command_line->value : NULL, NULL, SW_SHOWNORMAL);

    finalize_app_path_items(&app_path_items);

    return 0;
}

EXIT_CODE boot_normal(HINSTANCE hInstance)
{
    logger_put_info(_T("通常起動処理"));

    return boot_core(hInstance, NULL);
}

EXIT_CODE boot_with_option(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    logger_put_info(_T("オプションあり処理"));

    const WAIT_TIME_ARG wait_time_arg = get_wait_time(command_line_option);

    if (wait_time_arg.enabled) {
        logger_format_info(_T("起動前停止 %d ms"), wait_time_arg.time);
        Sleep(wait_time_arg.time);
        logger_put_info(_T("待機終了"));
    }

    TEXT_LIST args = new_memory(command_line_option->count, sizeof(TEXT), DEFAULT_MEMORY_ARENA);
    size_t arg_count = filter_enable_command_line_items(args, command_line_option);

    TEXT argument = to_command_line_argument(args, arg_count, DEFAULT_MEMORY_ARENA);
    logger_put_info(argument.value);

    EXIT_CODE result = boot_core(hInstance, &argument);
    release_text(&argument);
    release_memory(args, DEFAULT_MEMORY_ARENA);

    return result;
}
