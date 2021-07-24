#include "debug.h"
#include "app_main.h"
#include "debug.h"
#include "memory.h"
#include "tstring.h"
#include "path.h"
#include "logging.h"
#include "app_boot.h"
#include "app_command_line.h"

EXIT_CODE app_main(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    if (command_line_option->count < 1) {
        // そのまま実行
        return boot_normal(hInstance);
    }

    EXECUTE_MODE execute_mode = get_execute_mode(command_line_option);

    switch (execute_mode) {
        case EXECUTE_MODE_BOOT:
            return boot_with_option(hInstance, command_line_option);

        default:
            assert_debug(false);
    }

    return EXIT_CODE_UNKNOWN_EXECUTE_MODE;
}

