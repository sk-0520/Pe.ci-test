#include "app_console_prompt.h"

EXIT_CODE console_execute_prompt(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option, const CONSOLE_RESOURCE* console_resource)
{
    bool running_prompt = true;
    while (running_prompt) {
        TEXT t = wrap_text(_T("abc"));
        output_console_text(console_resource, &t, true);
    }

    return EXIT_CODE_SUCCESS;
}
