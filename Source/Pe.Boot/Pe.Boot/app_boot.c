#include "app_boot.h"
#include "app_path.h"
#include "path.h"
#include "logging.h"
#include "debug.h"
#include "tstring.h"

/// <summary>
/// ラインタイムパスを環境変数に設定。
/// </summary>
/// <param name="rootDirPath"></param>
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

    const TCHAR* env_path_key = _T("PATH");
    DWORD path_src_length = GetEnvironmentVariable(env_path_key, NULL, 0);
    DWORD path_dst_length = path_src_length + 1/* ; */ + (DWORD)crt_path.length - 1/*GetEnvironmentVariable が \0 を含めたサイズを返すのでここで補正しておく(allocate_stringが+1する) */;
    TCHAR* path_value = allocate_string(path_dst_length);

    GetEnvironmentVariable(env_path_key, path_value, path_dst_length);
    concat_string(path_value, _T(";"));
    concat_string(path_value, crt_path.value);
    SetEnvironmentVariable(env_path_key, path_value);

    free_string(path_value);
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
