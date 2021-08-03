#include "app_dry_run.h"
#include "../Pe.Library/platform.h"
#include "../Pe.Library/logging.h"
#include "app_command_line.h"
#include "app_console.h"
#include "execute.h"
#include "app_path.h"

static EXIT_CODE dry_run_core(HINSTANCE hInstance, const CONSOLE_RESOURCE* console_resource, const TEXT* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist_env_path(&app_path_items.root_directory);

    STARTUPINFO startupinfo;
    set_memory(&startupinfo, 0, sizeof(startupinfo));
    startupinfo.cb = sizeof(startupinfo);
    /*
    startupinfo.hStdInput = console_resource->input;
    startupinfo.hStdOutput = console_resource->output;
    startupinfo.hStdError = console_resource->error;
    */

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
    CONSOLE_RESOURCE console_resource = begin_console();

    TEXT env_special_key = wrap_text(_T("PE_SPECIAL_MODE"));
    TEXT value = wrap_text(_T("DRY-RUN"));
    set_environment_variable(&env_special_key, &value);

    logger_format_debug(_T("[ENV] %t = %t"), &env_special_key, &value);
    output_console_text(&console_resource, &value, true);

    TEXT_LIST args = allocate_clear_memory(command_line_option->count, sizeof(TEXT));
    size_t arg_count = filter_enable_command_line_items(args, command_line_option);

    TEXT argument = to_command_line_argument(args, arg_count);
    logger_format_debug(_T("argument = %t"), &argument);
    free_memory(args);
    EXIT_CODE exit_code = dry_run_core(hInstance, &console_resource, &argument);
    free_text(&argument);

    end_console(&console_resource);

    return exit_code;
}
