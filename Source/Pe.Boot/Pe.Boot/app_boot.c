#include "app_boot.h"
#include "app_path.h"
#include "path.h"
#include "logging.h"
#include "debug.h"
#include "tstring.h"

#define PATH_LENGTH (1024 * 4)

/// <summary>
/// ラインタイムパスを環境変数に設定。
/// </summary>
/// <param name="rootDirPath"></param>
void add_visual_cpp_runtime_redist(const TEXT* rootDirPath)
{
    TEXT dirs[] = {
        wrap_text(_T("bin")),
        wrap_text(_T("lib")),
        wrap_text(_T("Redist.MSVC.CRT")),
#ifdef _WIN64
        wrap_text(_T("x64")),
#else
        wrapText(_T("x86")),
#endif
    };

    TEXT crtPath = join_path(rootDirPath, dirs, sizeof(dirs) / sizeof(dirs[0]));
    output_debug(crtPath.value);

    TCHAR pathValue[PATH_LENGTH];
    GetEnvironmentVariable(_T("PATH"), pathValue, PATH_LENGTH - 1);
    concat_string(pathValue, _T(";"));
    concat_string(pathValue, crtPath.value);
    SetEnvironmentVariable(_T("PATH"), pathValue);

    free_text(&crtPath);
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
