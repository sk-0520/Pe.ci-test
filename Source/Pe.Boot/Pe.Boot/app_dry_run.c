#include "app_dry_run.h"
#include "../Pe.Library/platform.h"
#include "app_command_line.h"
#include "execute.h"
#include "app_path.h"

static EXIT_CODE dry_run_core(HINSTANCE hInstance, const TEXT* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist_env_path(&app_path_items.root_directory);

    STARTUPINFO startupinfo = {
        0
    };
    PROCESS_INFORMATION process_information;

    TCHAR* argument = NULL;
    if (is_enabled_text(command_line)) {
        argument = clone_string_with_length(command_line->value, command_line->length);
    }

    bool result = CreateProcess(
        app_path_items.main_module.value,
        argument,
        NULL,
        NULL,
        false,
        0,
        NULL,
        NULL,
        &startupinfo,
        &process_information
    );
    free_string(argument);
    if (result) {
        WaitForSingleObject(process_information.hProcess, INFINITE);
        CloseHandle(process_information.hThread);
    }

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
