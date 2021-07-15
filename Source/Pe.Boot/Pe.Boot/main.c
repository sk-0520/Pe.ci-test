#include <assert.h>

#include "debug.h"
#include "memory.h"
#include "app_main.h"

int WINAPI _tWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPTSTR lpCmdLine, _In_ int nCmdShow)
{
    debug("!START!");
    TEXT commandLine = wrapText(GetCommandLine());
    COMMAND_LINE_OPTION commandLineOption = parseCommandLine(&commandLine, true);

    int resutnCode = appMain(hInstance, &commandLineOption);
    //int resutnCode = 0;

    freeCommandLine(&commandLineOption);

#ifdef MEM_CHECK
    mem_check__printAllocateMemory(true, OutputDebugString, true);
#endif

    return resutnCode;
}
