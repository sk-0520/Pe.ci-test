#include "../Pe.Library/debug.h"
#include "app_main.h"
#include "../Pe.Library/debug.h"
#include "../Pe.Library/memory.h"
#include "../Pe.Library/tstring.h"
#include "../Pe.Library/path.h"
#include "../Pe.Library/logging.h"
#include "app_boot.h"
#include "app_command_line.h"
#include "app_dry_run.h"
#include "app_console.h"

//#define OREORE_TEST
#ifdef OREORE_TEST
#   include "oreore_test.h"
#endif

EXIT_CODE app_main(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
#ifdef OREORE_TEST
    oreore_test(hInstance, command_line_option);
#endif

    if (command_line_option->count < 1) {
        // そのまま実行
        logger_put_trace(_T("引数なしのため通常起動"));
        return boot_normal(hInstance);
    }

    EXECUTE_MODE execute_mode = get_execute_mode(command_line_option);
    logger_format_information(_T("起動方法: %d"), execute_mode);

    switch (execute_mode) {
        case EXECUTE_MODE_BOOT:
            return boot_with_option(hInstance, command_line_option);

        case EXECUTE_MODE_DRY_RUN:
            return dry_run(hInstance, command_line_option);

        //case EXECUTE_MODE_CONSOLE:
        //    return console_execute(hInstance, command_line_option);

        default:
            assert(false);
    }

    return EXIT_CODE_UNKNOWN_EXECUTE_MODE;
}

