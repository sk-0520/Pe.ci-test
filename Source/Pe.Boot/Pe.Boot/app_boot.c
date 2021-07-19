#include "app_boot.h"
#include "app_path.h"
#include "path.h"
#include "logging.h"
#include "debug.h"
#include "tstring.h"
#include "platform.h"

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

static void boot_core(HINSTANCE hInstance, const TCHAR* command_line)
{
    APP_PATH_ITEMS app_path_items;
    initialize_app_path_items(&app_path_items, hInstance);

    add_visual_cpp_runtime_redist(&app_path_items.rootDirectory);

    ShellExecute(NULL, _T("open"), app_path_items.mainModule.value, command_line, NULL, SW_SHOWNORMAL);

    uninitialize_app_path_items(&app_path_items);
}

void boot_normal(HINSTANCE hInstance)
{
    boot_core(hInstance, NULL);
}

void boot_with_option(HINSTANCE hInstance, const TCHAR* command_line)
{
    boot_core(hInstance, command_line);
}
