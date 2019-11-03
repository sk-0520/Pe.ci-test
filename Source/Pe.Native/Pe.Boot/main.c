#include <windows.h>
#include <tchar.h>
#include <shlwapi.h>

#pragma comment(lib, "shlwapi.lib")

void outputDebug(TCHAR* s);
size_t getAppPath(HINSTANCE hInstance, TCHAR* buffer);
size_t getParentDirPath(TCHAR* buffer, const TCHAR* filePath);
size_t getMainModulePath(TCHAR* buffer, const TCHAR* rootDirPath);

int CALLBACK WinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPSTR lpCmdLine, _In_ int nCmdShow)
{
    TCHAR appFilePath[MAX_PATH];
    getAppPath(hInstance, appFilePath);

    TCHAR appDirPath[MAX_PATH];
    getParentDirPath(appDirPath, appFilePath);

    TCHAR appExePath[MAX_PATH];
    getMainModulePath(appExePath, appDirPath);

    int argCount = 0;
    LPTSTR* args = CommandLineToArgvW(GetCommandLine(), &argCount);

    if (argCount <= 1) {
        // そのまま実行
        ShellExecute(NULL, _T("open"), appExePath, NULL, NULL, SW_SHOWNORMAL);
    }
    else {
        // コマンドライン渡して実行
        //todo: ちょっと疲れた
    }

    return 0;
}

void outputDebug(TCHAR* s)
{
    OutputDebugString(s);
    OutputDebugString(_T("\r\n"));
}

size_t getAppPath(HINSTANCE hInstance, TCHAR* buffer)
{
    TCHAR appRawPath[MAX_PATH];
    GetModuleFileName(hInstance, appRawPath, MAX_PATH);
    // 正規化しておく
    PathCanonicalize(buffer, appRawPath);
    outputDebug(buffer);
    return lstrlen(buffer);
}

size_t getParentDirPath(TCHAR* buffer, const TCHAR* filePath)
{
    lstrcpy(buffer, filePath);
    PathRemoveFileSpec(buffer);
    outputDebug(buffer);
    return lstrlen(buffer);
}

size_t getMainModulePath(TCHAR* buffer, const TCHAR* rootDirPath)
{
    PathCombine(buffer, rootDirPath, _T("Pe.Main.exe"));
    outputDebug(buffer);
    return lstrlen(buffer);
}
