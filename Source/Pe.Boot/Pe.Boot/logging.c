#include <stdarg.h>

#include <windows.h>

#include "logging.h"

#define LOGGER_LENGTH (4)

#define MESSAGE_CAPACITY (1024)
#define OUTPUT_LINE_CAPACITY (1024)

static FILE_WRITER library__default_log_file_writer;
static LOG_LEVEL library__default_log_level;
static LOGGER library__log_loggers[LOGGER_LENGTH];

void setup_default_log(FILE_WRITER* file_writer, LOG_LEVEL log_level)
{
    library__default_log_file_writer = *file_writer;
    library__default_log_level = log_level;
}

ssize_t attach_logger(const LOGGER* logger)
{
    if (!logger) {
        return -1;
    }
    if (!logger->function) {
        return -10;
    }

    for (size_t i = 0; i < SIZEOF_ARRAY(library__log_loggers); i++) {
        if (!library__log_loggers[i].function) {
            library__log_loggers[i] = *logger;
            return (ssize_t)i;
        }
    }

    return -100;
}

bool detach_logger(ssize_t log_id)
{
    if (log_id < 0) {
        return false;
    }

    if (SIZEOF_ARRAY(library__log_loggers) <= log_id) {
        return false;
    }

    LOGGER* logger = &library__log_loggers[log_id];
    if (!logger->function) {
        return false;
    }
    logger->function = NULL;
    logger->data = NULL;

    return true;
}

void cleanup_default_log()
{
    free_file_writer(&library__default_log_file_writer);
}

static void logging_core(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TEXT* message, const DATETIME* datetime)
{
    TIMESTAMP timestamp = datetime_to_timestamp(datetime, false);
    LOG_ITEM log_item = {
        .caller_file = caller_file,
        .caller_line = caller_line,
        .log_level = log_level,
        .message = message,
        .timestamp = &timestamp,
    };

    if (is_enabled_file_writer(&library__default_log_file_writer)) {
        static const TCHAR* log_levels[] = {
            _T("TRACE"),
            _T("DEBUG"),
            _T("INFO "),
            _T("WARN "),
            _T("ERROR"),
        };

        STRING_BUILDER sb = create_string_builder(OUTPUT_LINE_CAPACITY);
        TEXT format = wrap_text(
            _T("%04d-%02d-%02dT%02d:%02d:%02d.%03d")
            _T("|")
            _T("%s")
            _T("|")
            _T("%t")
            _T("|")
            _T("%s:%zd")
        );
        append_builder_format(&sb, &format,
            log_item.timestamp->year, log_item.timestamp->month, log_item.timestamp->day, log_item.timestamp->hour, log_item.timestamp->minute, log_item.timestamp->second, log_item.timestamp->milli_sec,
            log_levels[log_item.log_level],
            log_item.message,
            log_item.caller_file, log_item.caller_line
        );
        TEXT log_message = build_text_string_builder(&sb);

        write_text_file_writer(&library__default_log_file_writer, &log_message, true);

        free_string_builder(&sb);
        free_text(&log_message);
    }
}

static bool can_logging(LOG_LEVEL log_level)
{
    if (log_level < library__default_log_level) {
        return false;
    }

    bool enable_default = is_enabled_file_writer(&library__default_log_file_writer);
    bool enable_logger = false;
    if (!enable_default) {
        for (size_t i = 0; i < SIZEOF_ARRAY(library__log_loggers); i++) {
            if (library__log_loggers[i].function) {
                enable_logger = true;
                break;
            }
        }
    }

    return enable_default || enable_logger;
}

void library__format_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* format, ...)
{
    if (!can_logging(log_level)) {
        return;
    }

    DATETIME datetime = get_current_datetime();

    va_list ap;
    va_start(ap, format);

    TEXT text_format = wrap_text(format);
    STRING_BUILDER sb = create_string_builder(MESSAGE_CAPACITY);
    append_builder_vformat(&sb, &text_format, ap);
    TEXT message = build_text_string_builder(&sb);

    free_string_builder(&sb);

    va_end(ap);

    logging_core(log_level, caller_file, caller_line, &message, &datetime);

    free_text(&message);

}

void library__put_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* message)
{
    if (!can_logging(log_level)) {
        return;
    }

    DATETIME datetime = get_current_datetime();

    TEXT text_message = wrap_text(message);

    logging_core(log_level, caller_file, caller_line, &text_message, &datetime);
}
