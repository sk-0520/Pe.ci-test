#include "app_dry_run.h"
#include "../Pe.Library/platform.h"
#include "../Pe.Library/logging.h"
#include "app_command_line.h"
#include "app_console.h"
#include "execute.h"
#include "app_path.h"

static EXIT_CODE dry_run_core(HINSTANCE hInstance, const CONSOLE_RESOURCE* console_resource, const TEXT* command_line)
{
    EXIT_CODE exit_code = EXIT_CODE_UNKNOWN_EXECUTE_MODE;

    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist_env_path(&app_path_items.root_directory);

    STARTUPINFO startupinfo;
    set_memory(&startupinfo, 0, sizeof(startupinfo));
    startupinfo.cb = sizeof(startupinfo);

    //startupinfo.hStdInput = console_resource->handle.input;
    //startupinfo.hStdOutput = console_resource->handle.output;
    //startupinfo.hStdError = console_resource->handle.error;


    PROCESS_INFORMATION process_information;

    TCHAR* argument = NULL;
    if (is_enabled_text(command_line)) {
        argument = clone_string_with_length(command_line->value, command_line->length, DEFAULT_MEMORY_ARENA);
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
    release_string(argument, DEFAULT_MEMORY_ARENA);

    if (result) {
        WaitForSingleObject(process_information.hProcess, INFINITE);
        CloseHandle(process_information.hThread);

        DWORD process_exit_code;
        if (GetExitCodeProcess(process_information.hProcess, &process_exit_code)) {
            if (process_exit_code) {
                exit_code = process_exit_code;
            } else {
                exit_code = EXIT_CODE_SUCCESS;
            }
        } else {
            exit_code = EXIT_CODE_DRY_RUN_EXIT_ERROR;
        }
    } else {
        exit_code = EXIT_CODE_DRY_RUN_FAILED;
    }

    CloseHandle(process_information.hProcess);
    uninitialize_app_path_items(&app_path_items);

    return exit_code;
}

EXIT_CODE dry_run(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    CONSOLE_RESOURCE console_resource = begin_console(DEFAULT_MEMORY_ARENA);

    TEXT env_special_key = wrap_text(_T("PE_SPECIAL_MODE"));
    TEXT value = wrap_text(_T("DRY-RUN"));
    set_environment_variable(&env_special_key, &value);
    //output_console_text(&console_resource, &value, true);

    logger_format_debug(_T("[ENV] %t = %t"), &env_special_key, &value);

    TEXT_LIST args = new_memory(command_line_option->count, sizeof(TEXT), DEFAULT_MEMORY_ARENA);
    size_t arg_count = filter_enable_command_line_items(args, command_line_option);

    TEXT argument = to_command_line_argument(args, arg_count, DEFAULT_MEMORY_ARENA);
    logger_format_debug(_T("argument = %t"), &argument);
    release_memory(args, DEFAULT_MEMORY_ARENA);
    EXIT_CODE exit_code = dry_run_core(hInstance, &console_resource, &argument);
    release_text(&argument);

    end_console(&console_resource);

    return exit_code;
}
