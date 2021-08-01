﻿#include "app_dry_run.h"
#include "../Pe.Library/platform.h"
#include "app_command_line.h"
#include "execute.h"
#include "app_path.h"

static EXIT_CODE dry_run_core(HINSTANCE hInstance, const TEXT* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist_env_path(&app_path_items.root_directory);

    ShellExecute(NULL, _T("open"), app_path_items.main_module.value, is_enabled_text(command_line) ? command_line->value : NULL, NULL, SW_SHOWNORMAL);

    uninitialize_app_path_items(&app_path_items);

    return 0;
}

EXIT_CODE dry_run(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT env_console_key = wrap_text(_T("PE_CONSOLE_MODE"));
    TEXT value = wrap_text(_T("DRY-RUN"));
    set_environment_variable(&env_console_key, &value);

    TEXT_LIST args = allocate_clear_memory(command_line_option->count, sizeof(TEXT));
    size_t arg_count = filter_enable_command_line_items(args, command_line_option);

    TEXT argument = to_command_line_argument(args, arg_count);
    free_memory(args);
    EXIT_CODE exit_code = dry_run_core(hInstance, &argument);
    free_text(&argument);

    return exit_code;
}
