#include "app_boot.h"
#include "app_path.h"
#include "path.h"
#include "logging.h"
#include "debug.h"
#include "tstring.h"
#include "platform.h"
#include "command_line.h"
#include "app_command_line.h"

/// <summary>
/// ラインタイムパスを環境変数に設定。
/// </summary>
/// <param name="root_directory_path"></param>
static void add_visual_cpp_runtime_redist(const TEXT* root_directory_path)
{
    TEXT dirs[] = {
        wrap_text(_T("bin")),
        wrap_text(_T("lib")),
        wrap_text(_T("Redist.MSVC.CRT")),
#ifdef _WIN64
        wrap_text(_T("x64")),
#else
        wrap_text(_T("x86")),
#endif
    };

    TEXT crt_path = join_path(root_directory_path, dirs, SIZEOF_ARRAY(dirs));
    output_debug(crt_path.value);

    TEXT env_path_key = wrap_text(_T("PATH"));
    TEXT path_src_value = get_environment_variable(&env_path_key);

    TEXT values[] = {
        path_src_value,
        crt_path
    };
    TEXT env_sep = wrap_text(_T(";"));
    TEXT path_new_value = join_text(&env_sep, values, SIZEOF_ARRAY(values), IGNORE_EMPTY_ONLY);
    set_environment_variable(&env_path_key, &path_new_value);

    free_text(&path_new_value);
    free_text(&path_src_value);
    free_text(&crt_path);
}

static EXIT_CODE boot_core(HINSTANCE hInstance, const TEXT* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist(&app_path_items.rootDirectory);

    ShellExecute(NULL, _T("open"), app_path_items.mainModule.value, is_enabled_text(command_line) ? command_line->value : NULL, NULL, SW_SHOWNORMAL);

    uninitialize_app_path_items(&app_path_items);

    return 0;
}

EXIT_CODE boot_normal(HINSTANCE hInstance)
{
    return boot_core(hInstance, NULL);
}

EXIT_CODE boot_with_option(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT_LIST args = allocate_clear_memory(command_line_option->count, sizeof(TEXT));
    size_t arg_count = 0;


    const WAIT_TIME_ARG  wait_time_arg = get_wait_time(command_line_option);

    if (wait_time_arg.enabled) {
        TCHAR s[1000];
        format_string(s, _T("起動前停止: %d ms"), wait_time_arg.time);
        output_debug(s);
        Sleep(wait_time_arg.time);
        output_debug(_T("待機終了"));
    }

    for (size_t i = 0; i < command_line_option->count; i++) {
        if (wait_time_arg.item) {
            if (wait_time_arg.item->key_index == i) {
                continue;
            }
            if (wait_time_arg.item->value_index == i) {
                continue;
            }
        }

        args[arg_count++] = command_line_option->arguments[i];
    }

    TEXT argument = to_command_line_argument(args, arg_count);
    output_debug(argument.value);
    EXIT_CODE result = boot_core(hInstance, &argument);
    free_text(&argument);
    free_memory(args);

    return result;
}
