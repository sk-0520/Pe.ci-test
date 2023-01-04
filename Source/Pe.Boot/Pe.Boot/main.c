#include "../Pe.Library/debug.h"
#include "../Pe.Library/res_check.h"
#include "../Pe.Library/logging.h"
#include "app_main.h"

#define WITHOUT_CRT

#ifdef RES_CHECK
static void output(const TCHAR* s)
{
    OutputDebugString(s);
    OutputDebugString(NEWLINET);
}
#endif

static ssize_t log_id;
static void logging(const LOG_ITEM* log_item, void* data)
{
    static TCHAR* log_levels[] = {
        _T("TRACE"),
        _T("DEBUG"),
        _T("INFORMATION"),
        _T("WARNING"),
        _T("ERROR"),
    };
    STRING_BUILDER sb = new_string_builder(256, DEFAULT_MEMORY_ARENA);
    TEXT format = wrap_text(
        _T("[LOG:%s]")
        _T(" ")
        _T("%t")
        _T(" -> ")
        _T("%t")
        _T(" (%t)")
        NEWLINET
    );
    append_builder_format(&sb, &format,
        log_levels[log_item->log_level],
        log_item->format.time,
        log_item->message,
        log_item->format.caller
    );
    TEXT text = build_text_string_builder(&sb);
    OutputDebugString(text.value);

    release_text(&text);
    release_string_builder(&sb);
}

static void setup_logging_file(const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT log_file_key = wrap_text(OPTION_LOG_FILE_KEY);
    const COMMAND_LINE_ITEM* log_file_item = get_command_line_item(command_line_option, &log_file_key);
    
    if (is_inputted_command_line_item(log_file_item)) {
        TEXT default_log_path = log_file_item->value;

        FILE_WRITER log_file_writer = new_file_writer(&default_log_path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_OPEN_OR_CREATE, FILE_WRITER_OPTIONS_BOM, DEFAULT_MEMORY_ARENA);
        seek_end_file_resource(&log_file_writer.resource);
        set_default_log_file(&log_file_writer);
    } else {
        set_default_log_file(NULL);
    }
}
static void setup_logging_level(const COMMAND_LINE_OPTION* command_line_option)
{
#ifdef _DEBUG
    LOG_LEVEL default_log_level = LOG_LEVEL_TRACE;
#else
    LOG_LEVEL default_log_level = LOG_LEVEL_INFO;
#endif

    TEXT log_level_key = wrap_text(OPTION_LOG_LEVEL_KEY);
    const COMMAND_LINE_ITEM* log_level_item = get_command_line_item(command_line_option, &log_level_key);
    if (is_inputted_command_line_item(log_level_item)) {
        TEXT_PARSED_I32_RESULT num_result = parse_i32_from_text(&log_level_item->value, PARSE_BASE_NUMBER_D);
        int log_level = default_log_level;
        if (num_result.success) {
            log_level = num_result.value;
        } else {
            TEXT levels[] = {
                wrap_text(_T("trace")),
                wrap_text(_T("debug")),
                wrap_text(_T("information")),
                wrap_text(_T("warning")),
                wrap_text(_T("error")),
            };
            for (size_t i = 0; i < SIZEOF_ARRAY(levels); i++) {
                if (!compare_text(levels + i, &log_level_item->value, true)) {
                    log_level = (int)i;
                    break;
                }
            }
        }

        if (LOG_LEVEL_TRACE <= log_level && log_level <= LOG_LEVEL_ERROR) {
            default_log_level = log_level;
        }
    }

    set_default_log_level(default_log_level);
}

static void start_logging(const COMMAND_LINE_OPTION* command_line_option)
{
    initialize_logger(DEFAULT_MEMORY_ARENA);
    setup_logging_file(command_line_option);
    setup_logging_level(command_line_option);

#ifdef _DEBUG
    LOGGER logger = {
        .function = logging,
        .data = NULL,
    };
    log_id = attach_logger(&logger);
#endif

    logger_put_trace(_T("お馬さんパッカパッカ🏇"));
}

static void end_logging(void)
{
    logger_put_trace(_T("お魚さんブックブック🐟"));

    detach_logger(log_id);
    cleanup_default_log();
}


static int application_main(HINSTANCE hInstance)
{
#ifdef RES_CHECK
    library_rc_initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
#endif

    TEXT command_line = wrap_text(GetCommandLine());
    COMMAND_LINE_OPTION command_line_option = parse_command_line(&command_line, true, DEFAULT_MEMORY_ARENA);

    start_logging(&command_line_option);

    logger_put_info(_T("Pe アプリケーション処理開始"));

    int return_code = app_main(hInstance, &command_line_option);

    logger_put_info(_T("Pe アプリケーション処理終了"));

    release_command_line(&command_line_option);

    end_logging();

#ifdef RES_CHECK
    library_rc_print(true);
    library_rc_uninitialize();
#endif

    return return_code;
}

#ifdef WITHOUT_CRT
/// <summary>
/// 非CRT版スタートアップ。
/// </summary>
/// <returns></returns>
void WINAPI entry_main(void)
{
    HINSTANCE hInstance = GetModuleHandle(NULL);
    int return_code = application_main(hInstance);
    ExitProcess(return_code);
}
#else
/// <summary>
/// CRT版スタートアップ。
/// </summary>
/// <param name="hInstance"></param>
/// <param name="hPrevInstance"></param>
/// <param name="lpCmdLine"></param>
/// <param name="nCmdShow"></param>
/// <returns></returns>
int WINAPI _tWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPTSTR lpCmdLine, _In_ int nCmdShow)
{
    return application_main(hInstance);
}
#endif
