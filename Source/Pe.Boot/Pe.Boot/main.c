#include <assert.h>

#include "debug.h"
#include "res_check.h"
#include "app_main.h"

#ifdef RES_CHECK
static void output(const TCHAR* s)
{
    OutputDebugString(s);
}
#endif

int WINAPI _tWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPTSTR lpCmdLine, _In_ int nCmdShow)
{
    debug("!START!");
    TEXT command_line = wrap_text(GetCommandLine());
    COMMAND_LINE_OPTION command_line_option = parse_command_line(&command_line, true);

    int resutn_code = app_main(hInstance, &command_line_option);
    //int resutn_code = 0;

    free_command_line(&command_line_option);

#ifdef RES_CHECK
    rc_heap__print_allocate_memory(true, output, true);
#endif

    return resutn_code;
}
