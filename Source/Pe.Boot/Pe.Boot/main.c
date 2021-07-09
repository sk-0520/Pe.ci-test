#include <windows.h>
#include <tchar.h>
#include <shlwapi.h>
#include <assert.h>

#include "memory.h"
#include "tstring.h"
#include "path.h"
#include "logging.h"

#define PATH_LENGTH (1024 * 4)

void addVisualCppRuntimeRedist(const TCHAR* rootDirPath);
TCHAR* tuneArg(const TCHAR* arg);
int getWaitTime(const TCHAR* s);

int CALLBACK WinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPSTR lpCmdLine, _In_ int nCmdShow)
{
    APP_PATH_ITEMS appPathItems;
    getAppPathItems(hInstance, &appPathItems);

    addVisualCppRuntimeRedist(appPathItems.rootDirectory);

    int argCount = 0;
    LPTSTR* args = CommandLineToArgvW(GetCommandLine(), &argCount);

    if (argCount <= 1) {
        // そのまま実行
        ShellExecute(NULL, _T("open"), appPathItems.mainModule, NULL, NULL, SW_SHOWNORMAL);
    }
    else {
        // コマンドライン渡して実行
        size_t tunedArgsCount = (size_t)argCount - 1;
        TCHAR** tunedArgs = allocateClearMemory(tunedArgsCount, sizeof(TCHAR*));
        if (!tunedArgs) {
            // これもう立ち上げ不能だと思う
            outputDebug(_T("メモリ確保できんかったね！"));
            ShellExecute(NULL, _T("open"), appPathItems.mainModule, NULL, NULL, SW_SHOWNORMAL);
            return 0;
        }

        // 実行待機用
        int waitTime = 0;
        size_t totalLength = 0;
        size_t skipIndex1 = SIZE_MAX;
        size_t skipIndex2 = SIZE_MAX;

        for (int i = 1, j = 0; i < argCount; i++, j++) {
            TCHAR* workArg = args[i];
            outputDebug(workArg);
            TCHAR* tunedArg = tuneArg(workArg);
            assert(tunedArg);
#pragma warning(push)
#pragma warning(disable:6385 6386)
            tunedArgs[j] = tunedArg;
            totalLength += getStringLength(tunedArgs[j]);
#pragma warning(pop)
            if (!waitTime) {
                TCHAR waits[][16] = {
                    _T("--_boot-wait"), _T("-_boot-wait"), _T("/_boot-wait"),
                    _T("--wait"), _T("-wait"), _T("/wait"), //TODO: #737 互換用処理
                };
                for (size_t waitIndex = 0; waitIndex < sizeof(waits) / sizeof(waits[0]); waitIndex++) {
                    const TCHAR* wait = findString(tunedArg, waits[waitIndex]);
                    if (wait == tunedArg) {
                        skipIndex1 = j;

                        TCHAR* eq = findCharacter(wait, '=');
                        if (eq && eq + 1) {
                            TCHAR* value = eq + 1;
                            waitTime = getWaitTime(value);
                        }
                        else if(i + 1 < argCount) {
                            waitTime = getWaitTime(args[i + 1]);

                            skipIndex2 = (size_t)(j + 1);
                        }
                        break;
                    }
                }
            }
        }

        TCHAR* commandArg = allocateClearMemory(totalLength + 1, sizeof(TCHAR*));
        if (commandArg) {
            commandArg[0] = 0;
            for (size_t i = 0; i < tunedArgsCount; i++) {
                // 大丈夫、はやいよ！
                if ((skipIndex1 == i) || (skipIndex2 == i)) {
                    continue;
                }
                concatString(commandArg, tunedArgs[i]);
                concatString(commandArg, _T(" "));
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
        ShellExecute(NULL, _T("open"), appPathItems.mainModule, commandArg, NULL, SW_SHOWNORMAL);

        // もはや死ぬだけなので後処理不要
    }

    return 0;
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
        combinePath(buffer, crtPath, name);
        lstrcpy(crtPath, buffer);
    }
    outputDebug(crtPath);

    TCHAR pathValue[PATH_LENGTH];
    GetEnvironmentVariable(_T("PATH"), pathValue, PATH_LENGTH - 1);
    concatString(pathValue, _T(";"));
    concatString(pathValue, crtPath);
    SetEnvironmentVariable(_T("PATH"), pathValue);

}

/**
書式調整後の動的確保された文字列を返す。
呼び出し側で世話すること。
*/
TCHAR* tuneArg(const TCHAR* arg)
{
    int hasSpace = _tcschr(arg, ' ') != NULL;
    size_t len = (size_t)getStringLength(arg) + (hasSpace ? 2 : 0);
    TCHAR* s = allocateClearMemory(len + 1, sizeof(TCHAR*));
    assert(s);
    if (hasSpace) {
        lstrcpy(s + 1, arg);
        s[0] = '"';
        s[len - 1] = '"';
        s[len - 0] = 0; // ↑で +1 してるから安全安全
    }
    else {
        lstrcpy(s, arg);
    }
    return s;
}

int getWaitTime(const TCHAR* s)
{
    if (!s) {
        return 0;
    }

    int result;
    if (tryParseInteger(s, &result)) {
        return result;
    }

    return 0;
}

