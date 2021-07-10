#include <assert.h>

#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "memory.h"
#include "tstring.h"
#include "path.h"
#include "logging.h"
#include "commandline.h"
#include "pe_boot.h"


TCHAR* tuneArg(const TCHAR* arg);
int getWaitTime(const TCHAR* s);

//int CALLBACK WinMainEx(HINSTANCE hInstance, HINSTANCE hPrevInstance, const LPTSTR* argv, size_t argc, int nCmdShow)
int APIENTRY _tWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
{
    COMMAND_LINE_OPTION commandLineOption = parseCommandLine(lpCmdLine);
    
    if (commandLineOption.count <= 1) {
        // そのまま実行
        bootNormal(hInstance);
        return 0;
    }

    // コマンドライン渡して実行
    size_t tunedArgsCount = commandLineOption.count - 1;
    TCHAR** tunedArgs = allocateClearMemory(tunedArgsCount, sizeof(TCHAR*));
    if (!tunedArgs) {
        // これもう立ち上げ不能だと思う
        outputDebug(_T("メモリ確保できんかったね！"));
        bootNormal(hInstance);
        return 0;
    }

    // 実行待機用
    int waitTime = 0;
    size_t totalLength = 0;
    size_t skipIndex1 = SIZE_MAX;
    size_t skipIndex2 = SIZE_MAX;

    for (size_t i = 1, j = 0; i < commandLineOption.count; i++, j++) {
        const TCHAR* workArg = commandLineOption.arguments[i];
        outputDebug(workArg);
        TCHAR* tunedArg = tuneArg(workArg);
        assert(tunedArg);
        tunedArgs[j] = tunedArg;
        totalLength += getStringLength(tunedArgs[j]);
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
                    else if (i + 1 < commandLineOption.count) {
                        waitTime = getWaitTime(commandLineOption.arguments[i + 1]);

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
        formatString(s, _T("起動前停止: %d ms"), waitTime);
        outputDebug(s);
        Sleep(waitTime);
        outputDebug(_T("待機終了"));
    }

    // commandArg の確保に失敗してても引数無し扱いで起動となる
    outputDebug(commandArg);
    bootWithOption(hInstance, commandArg);

    // もはや死ぬだけなので後処理不要

    return 0;
}

//void WINAPI RawWinMain()
//{
//    HINSTANCE hInstance = GetModuleHandle(NULL);
//
//    int argc;
//    TCHAR** argv = CommandLineToArgvW(GetCommandLine(), &argc);
//
//    DWORD result = WinMainEx(hInstance, NULL, argv, (size_t)argc, SW_SHOWDEFAULT);
//
//    LocalFree(argv);
//
//    ExitProcess(result);
//}



int getWaitTime(const TCHAR* s)
{
    if (!s) {
        return 0;
    }

    int result;
    if (tryParseInteger(&result, s)) {
        return result;
    }

    return 0;
}

