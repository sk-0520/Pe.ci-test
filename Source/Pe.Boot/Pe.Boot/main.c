#include <assert.h>

#include "debug.h"
#include "res_check.h"
#include "app_main.h"

#ifdef RES_CHECK
static void output(const TCHAR* s)
{
    OutputDebugString(s);
    OutputDebugString(NEWLINET);
}
#endif

int WINAPI _tWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPTSTR lpCmdLine, _In_ int nCmdShow)
{
    debug("!START!");
#ifdef RES_CHECK
    rc__initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
#endif

    TEXT command_line = wrap_text(GetCommandLine());
    COMMAND_LINE_OPTION command_line_option = parse_command_line(&command_line, true);

    int resutn_code = app_main(hInstance, &command_line_option);
    //int resutn_code = 0;

    free_command_line(&command_line_option);

#ifdef RES_CHECK
    rc__print(true);
    rc__uninitialize();
#endif

    return resutn_code;
}
