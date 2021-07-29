#include "debug.h"
#include "res_check.h"
#include "logging.h"
#include "app_main.h"

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
        _T("TRACE      "),
        _T("DEBUG      "),
        _T("INFORMATION"),
        _T("WARNING    "),
        _T("ERROR      "),
    };
    STRING_BUILDER sb = create_string_builder(256);
    TEXT format = wrap_text(
        _T("%02d:%02d:%02d.%03d")
        _T(" ")
        _T("%s")
        _T(" -> ")
        _T("%t")
        _T(" (%s:%zd)")
        NEWLINET
    );
    append_builder_format(&sb, &format,
        log_item->timestamp->hour, log_item->timestamp->minute, log_item->timestamp->second, log_item->timestamp->milli_sec,
        log_levels[log_item->log_level],
        log_item->message,
        log_item->caller_file, log_item->caller_line
    );
    TEXT text = build_text_string_builder(&sb);
    OutputDebugString(text.value);

    free_text(&text);
    free_string_builder(&sb);
}

static void start_logging(const COMMAND_LINE_OPTION* command_line_option)
{
#ifdef _DEBUG
    TEXT path = wrap_text(_T("x:\\logging.log"));
    FILE_WRITER fw = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_OPEN_OR_CREATE, FILE_WRITER_OPTIONS_BOM);
    seek_end_file_resource(&fw.resource);
    setup_default_log(&fw, LOG_LEVEL_TRACE);

    LOGGER logger = {
        .function = logging,
        .data = NULL,
    };
    log_id = attach_logger(&logger);
#endif
}

static void end_logging()
{
    detach_logger(log_id);
    cleanup_default_log();
}


static int application_main(HINSTANCE hInstance)
{
#ifdef RES_CHECK
    rc__initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
#endif

    TEXT command_line = wrap_text(GetCommandLine());
    COMMAND_LINE_OPTION command_line_option = parse_command_line(&command_line, true);

    start_logging(&command_line_option);

    logger_put_information(_T("おうまさんぱっぱか🏇"));

    int return_code = app_main(hInstance, &command_line_option);

    free_command_line(&command_line_option);

    end_logging();

#ifdef RES_CHECK
    rc__print(true);
    rc__uninitialize();
#endif

    return return_code;
}

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

/// <summary>
/// 非CRT版スタートアップ。
/// </summary>
/// <returns></returns>
void WINAPI entry_main()
{
    HINSTANCE hInstance = GetModuleHandle(NULL);
    int return_code = application_main(hInstance);
    ExitProcess(return_code);
}

