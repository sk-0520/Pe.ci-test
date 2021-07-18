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
void addVisualCppRuntimeRedist(const TEXT* rootDirPath)
{
    TEXT dirs[] = {
        wrapText(_T("bin")),
        wrapText(_T("lib")),
        wrapText(_T("Redist.MSVC.CRT")),
#ifdef _WIN64
        wrapText(_T("x64")),
#else
        wrapText(_T("x86")),
#endif
    };

    TEXT crtPath = joinPath(rootDirPath, dirs, sizeof(dirs) / sizeof(dirs[0]));
    outputDebug(crtPath.value);

    TCHAR pathValue[PATH_LENGTH];
    GetEnvironmentVariable(_T("PATH"), pathValue, PATH_LENGTH - 1);
    concatString(pathValue, _T(";"));
    concatString(pathValue, crtPath.value);
    SetEnvironmentVariable(_T("PATH"), pathValue);

    freeText(&crtPath);
}

static void bootCore(HINSTANCE hInstance, const TCHAR* commandLine)
{
    APP_PATH_ITEMS appPathItems;
    initializeAppPathItems(&appPathItems, hInstance);

    addVisualCppRuntimeRedist(&appPathItems.rootDirectory);

    ShellExecute(NULL, _T("open"), appPathItems.mainModule.value, commandLine, NULL, SW_SHOWNORMAL);

    uninitializeAppPathItems(&appPathItems);
}

void bootNormal(HINSTANCE hInstance)
{
    bootCore(hInstance, NULL);
}

void bootWithOption(HINSTANCE hInstance, const TCHAR* commandLine)
{
    bootCore(hInstance, commandLine);
}
