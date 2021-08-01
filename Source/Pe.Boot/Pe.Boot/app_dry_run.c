#include "app_dry_run.h"
#include "../Pe.Library/platform.h"
#include "app_command_line.h"
#include "app_boot.h"

EXIT_CODE dry_run(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT env_console_key = wrap_text(_T("PE_CONSOLE_MODE"));
    TEXT value = wrap_text(_T("DRY-RUN"));
    set_environment_variable(&env_console_key, &value);

    TEXT_LIST args = allocate_clear_memory(command_line_option->count, sizeof(TEXT));
    size_t arg_count = filter_enable_command_line_items(args, command_line_option);

    TEXT argument = to_command_line_argument(args, arg_count);
    return boot(hInstance, &argument);
}
