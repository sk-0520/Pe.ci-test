#include "pe_boot.h"
#include "path.h"
#include "logging.h"
#include "tstring.h"

#define PATH_LENGTH (1024 * 4)

/// <summary>
/// ラインタイムパスを環境変数に設定。
/// </summary>
/// <param name="rootDirPath"></param>
void addVisualCppRuntimeRedist(const TCHAR* rootDirPath)
{
    TCHAR crtPath[MAX_PATH];
    copyString(crtPath, rootDirPath);

    TCHAR dirs[][32] = {
        _T("bin"),
        _T("lib"),
        _T("Redist.MSVC.CRT"),
#ifdef _WIN64
        _T("x64"),
#else
        _T("x86"),
#endif
    };
    for (size_t i = 0; i < (sizeof(dirs) / sizeof(dirs[0])); i++) {
        TCHAR buffer[MAX_PATH];
        TCHAR* name = dirs[i];
        combinePath(buffer, crtPath, name);
        copyString(crtPath, buffer);
    }
    outputDebug(crtPath);

    TCHAR pathValue[PATH_LENGTH];
    GetEnvironmentVariable(_T("PATH"), pathValue, PATH_LENGTH - 1);
    concatString(pathValue, _T(";"));
    concatString(pathValue, crtPath);
    SetEnvironmentVariable(_T("PATH"), pathValue);
}

static void bootCore(HINSTANCE hInstance, const TCHAR* options)
{
    APP_PATH_ITEMS appPathItems;
    getAppPathItems(hInstance, &appPathItems);

    addVisualCppRuntimeRedist(appPathItems.rootDirectory);

    ShellExecute(NULL, _T("open"), appPathItems.mainModule, options, NULL, SW_SHOWNORMAL);
}

void bootNormal(HINSTANCE hInstance)
{
    bootCore(hInstance, NULL);
}

void bootWithOption(HINSTANCE hInstance, const TCHAR* options)
{
    bootCore(hInstance, options);
}
