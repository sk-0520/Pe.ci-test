#include "app_boot.h"
#include "app_path.h"
#include "../Pe.Library/path.h"
#include "../Pe.Library/logging.h"
#include "../Pe.Library/debug.h"
#include "../Pe.Library/tstring.h"
#include "../Pe.Library/platform.h"
#include "../Pe.Library/command_line.h"
#include "app_command_line.h"

/// <summary>
/// ラインタイムパスを環境変数に設定。
/// </summary>
/// <param name="root_directory_path"></param>
static void add_visual_cpp_runtime_redist_env_path(const TEXT* root_directory_path)
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
    logger_put_debug(crt_path.value);

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

EXIT_CODE boot(HINSTANCE hInstance, const TEXT* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist_env_path(&app_path_items.root_directory);

    ShellExecute(NULL, _T("open"), app_path_items.main_module.value, is_enabled_text(command_line) ? command_line->value : NULL, NULL, SW_SHOWNORMAL);

    uninitialize_app_path_items(&app_path_items);

    return 0;
}

EXIT_CODE boot_normal(HINSTANCE hInstance)
{
    logger_put_information(_T("通常起動処理"));

    return boot(hInstance, NULL);
}

EXIT_CODE boot_with_option(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    logger_put_information(_T("オプションあり処理"));

    const WAIT_TIME_ARG wait_time_arg = get_wait_time(command_line_option);

    if (wait_time_arg.enabled) {
        logger_format_information(_T("起動前停止 %d ms"), wait_time_arg.time);
        Sleep(wait_time_arg.time);
        logger_put_information(_T("待機終了"));
    }

    TEXT_LIST args = allocate_clear_memory(command_line_option->count, sizeof(TEXT));
    size_t arg_count = filter_enable_command_line_items(args, command_line_option);

    TEXT argument = to_command_line_argument(args, arg_count);
    logger_put_information(argument.value);

    EXIT_CODE result = boot(hInstance, &argument);
    free_text(&argument);
    free_memory(args);

    return result;
}
