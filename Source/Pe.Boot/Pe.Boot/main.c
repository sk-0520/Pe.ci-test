#include <windows.h>
#include <tchar.h>
#include <shlwapi.h>
#include <assert.h>

#pragma comment(lib, "shlwapi.lib")

#define PATH_LENGTH (1024 * 4)

void outputDebug(TCHAR* s);
size_t getAppPath(HINSTANCE hInstance, TCHAR* buffer);
size_t getParentDirPath(TCHAR* buffer, const TCHAR* filePath);
void addVisualCppRuntimeRedist(const TCHAR* rootDirPath);
size_t getMainModulePath(TCHAR* buffer, const TCHAR* rootDirPath);
TCHAR* tuneArg(const TCHAR* arg);
long getWaitTime(const TCHAR* s);

int CALLBACK WinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPSTR lpCmdLine, _In_ int nCmdShow)
{
    TCHAR appFilePath[MAX_PATH];
    getAppPath(hInstance, appFilePath);

    TCHAR appDirPath[MAX_PATH];
    getParentDirPath(appDirPath, appFilePath);

    addVisualCppRuntimeRedist(appDirPath);

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
        size_t tunedArgsCount = (size_t)argCount - 1;
        TCHAR** tunedArgs = malloc(tunedArgsCount * sizeof(TCHAR*));
        if (!tunedArgs) {
            // これもう立ち上げ不能だと思う
            outputDebug(_T("メモリ確保できんかったね！"));
            ShellExecute(NULL, _T("open"), appExePath, NULL, NULL, SW_SHOWNORMAL);
            return 0;
        }

        // 実行待機用
        long waitTime = 0;
        size_t totalLength = 0;
        int skipIndex1 = -1;
        int skipIndex2 = -1;

        for (int i = 1, j = 0; i < argCount; i++, j++) {
            TCHAR* workArg = args[i];
            outputDebug(workArg);
            TCHAR* tunedArg = tuneArg(workArg);
            assert(tunedArg);
#pragma warning(push)
#pragma warning(disable:6385 6386)
            tunedArgs[j] = tunedArg;
            totalLength += lstrlen(tunedArgs[j]);
#pragma warning(pop)
            if (!waitTime) {
                TCHAR waits[][16] = { _T("--wait"), _T("-wait"), _T("/wait") };
                for (size_t waitIndex = 0; waitIndex < sizeof(waits) / sizeof(waits[0]); waitIndex++) {
                    TCHAR* wait = _tcsstr(tunedArg, waits[waitIndex]);
                    if (wait == tunedArg) {
                        skipIndex1 = j;

                        TCHAR* eq = _tcschr(wait, '=');
                        if (eq && eq + 1) {
                            TCHAR* value = eq + 1;
                            waitTime = getWaitTime(value);
                        }
                        else if(i + 1 < argCount) {
                            waitTime = getWaitTime(args[i + 1]);

                            skipIndex2 = j + 1;
                        }
                        break;
                    }
                }
            }
        }

        TCHAR* commandArg = malloc((totalLength + 1) * sizeof(TCHAR*));
        if (commandArg) {
            commandArg[0] = 0;
            for (size_t i = 0; i < tunedArgsCount; i++) {
                // 大丈夫、はやいよ！
                if ((skipIndex1 == i) || (skipIndex2 == i)) {
                    continue;
                }
                lstrcat(commandArg, tunedArgs[i]);
                lstrcat(commandArg, _T(" "));
            }
        }

        // 起動前停止
        if (0 < waitTime) {
            TCHAR s[1000];
            swprintf(s, 1000 - 1, _T("起動前停止: %d ms"), waitTime);
            outputDebug(s);
            Sleep(waitTime);
            outputDebug(_T("待機終了"));
        }

        // commandArg の確保に失敗してても引数無し扱いで起動となる
        outputDebug(commandArg);
        ShellExecute(NULL, _T("open"), appExePath, commandArg, NULL, SW_SHOWNORMAL);

        // もはや死ぬだけなので後処理不要
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

void addVisualCppRuntimeRedist(const TCHAR* rootDirPath) {
    TCHAR crtPath[MAX_PATH];
    lstrcpy(crtPath, rootDirPath);


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
        PathCombine(buffer, crtPath, name);
        lstrcpy(crtPath, buffer);
    }
    outputDebug(crtPath);

    TCHAR pathValue[PATH_LENGTH];
    GetEnvironmentVariable(_T("PATH"), pathValue, PATH_LENGTH - 1);
    lstrcat(pathValue, _T(";"));
    lstrcat(pathValue, crtPath);
    SetEnvironmentVariable(_T("PATH"), pathValue);

}

size_t getMainModulePath(TCHAR* buffer, const TCHAR* rootDirPath)
{
    TCHAR binPath[MAX_PATH];
    binPath[0] = 0;
    PathCombine(binPath, rootDirPath, _T("bin"));
    PathCombine(buffer, binPath, _T("Pe.Main.exe"));
    outputDebug(buffer);
    return lstrlen(buffer);
}

/**
書式調整後の動的確保された文字列を返す。
呼び出し側で世話すること。
*/
TCHAR* tuneArg(const TCHAR* arg)
{
    int hasSpace = _tcschr(arg, ' ') != NULL;
    size_t len = (size_t)lstrlen(arg) + (hasSpace ? 2 : 0);
    TCHAR* s = malloc((len + 1) * sizeof(TCHAR*));
    assert(s);
    if (hasSpace) {
        lstrcpy(s + 1, arg);
        s[0] = '"';
        s[len - 2] = '"';
        s[len - 1] = 0;
    }
    else {
        lstrcpy(s, arg);
    }
    return s;
}

long getWaitTime(const TCHAR* s)
{
    if (!s) {
        return 0;
    }
    //size_t len = lstrlen(s);
    TCHAR* end = NULL;
    long result = _tcstol(s, &end, 10);
    return result;
}

