#include <stdarg.h>

#include <Windows.h>

#include "debug.h"
#include "logging.h"

#define LOGGER_LENGTH (4)

#define MESSAGE_CAPACITY (1024)
#define LOG_FORMAT_CAPACITY (256)
#define OUTPUT_LINE_CAPACITY (1024)

static const MEMORY_ARENA_RESOURCE* library_log_memory_arena_resource;
static FILE_WRITER library_default_log_file_writer;
static LOG_LEVEL library_default_log_level;
static LOGGER library_log_loggers[LOGGER_LENGTH];

void initialize_logger(const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    assert(memory_arena_resource);
    library_log_memory_arena_resource = memory_arena_resource;
}

void set_default_log_file(FILE_WRITER* file_writer)
{
    if (is_enabled_file_writer(&library_default_log_file_writer)) {
        release_file_writer(&library_default_log_file_writer);
    }

    if (file_writer) {
        library_default_log_file_writer = *file_writer;
    } else {
        library_default_log_file_writer = create_invalid_file_writer();
    }
}

void set_default_log_level(LOG_LEVEL log_level)
{
    library_default_log_level = log_level;
}

ssize_t attach_logger(const LOGGER* logger)
{
    if (!logger) {
        return -1;
    }
    if (!logger->function) {
        return -10;
    }

    for (size_t i = 0; i < SIZEOF_ARRAY(library_log_loggers); i++) {
        if (!library_log_loggers[i].function) {
            library_log_loggers[i] = *logger;
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

    if (SIZEOF_ARRAY(library_log_loggers) <= log_id) {
        return false;
    }

    LOGGER* logger = &library_log_loggers[log_id];
    if (!logger->function) {
        return false;
    }
    logger->function = NULL;
    logger->data = NULL;

    return true;
}

void cleanup_default_log()
{
    release_file_writer(&library_default_log_file_writer);
}

static void logging_default(const LOG_ITEM* log_item)
{
    if (is_enabled_file_writer(&library_default_log_file_writer)) {
        static const TCHAR* log_levels[] = {
            _T("TRACE"),
            _T("DEBUG"),
            _T("INFO "),
            _T("WARN "),
            _T("ERROR"),
        };

        STRING_BUILDER sb = new_string_builder(OUTPUT_LINE_CAPACITY, library_log_memory_arena_resource);
        TEXT format = wrap_text(
            _T("%tT%t")
            _T(" | ")
            _T("%s")
            _T(" | ")
            _T("%t")
            _T(" | ")
            _T("%t")
        );
        append_builder_format(&sb, &format,
            log_item->format.date, log_item->format.time,
            log_levels[log_item->log_level],
            log_item->message,
            log_item->format.caller
        );
        TEXT log_message = build_text_string_builder(&sb);

        write_text_file_writer(&library_default_log_file_writer, &log_message, true);
        flush_file_writer(&library_default_log_file_writer, true);

        release_string_builder(&sb);
        release_text(&log_message);
    }
}

static void logging_logger(const LOG_ITEM* log_item)
{
    for (size_t i = 0; i < SIZEOF_ARRAY(library_log_loggers); i++) {
        LOGGER* logger = library_log_loggers + i;
        if (logger->function) {
            logger->function(log_item, logger->data);
        }
    }
}

static void logging(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TEXT* message, const DATETIME* datetime)
{
    TIMESTAMP timestamp = datetime_to_timestamp(datetime, false);

    TEXT caller_file_text = wrap_text(caller_file);

    STRING_BUILDER sb = new_string_builder(LOG_FORMAT_CAPACITY, library_log_memory_arena_resource);

    TEXT date_format = wrap_text(_T("%04d-%02d-%02d"));
    append_builder_format(&sb, &date_format, timestamp.year, timestamp.month, timestamp.day);
    TEXT ref_date_text = reference_text_string_builder(&sb);
    new_stack_or_heap_array(date_buffer, date_array, TCHAR, ref_date_text.length + 1, 16, library_log_memory_arena_resource);
    copy_memory(date_buffer, ref_date_text.value, ref_date_text.length * sizeof(TCHAR));
    date_buffer[ref_date_text.length] = 0;
    TEXT date_text = wrap_text_with_length(date_buffer, ref_date_text.length, false, library_log_memory_arena_resource);
    clear_builder(&sb);

    TEXT time_format = wrap_text(_T("%02d:%02d:%02d.%03d"));
    append_builder_format(&sb, &time_format, timestamp.hour, timestamp.minute, timestamp.second, timestamp.millisecond);
    TEXT ref_time_text = reference_text_string_builder(&sb);
    new_stack_or_heap_array(time_buffer, time_array, TCHAR, ref_time_text.length + 1, 16, library_log_memory_arena_resource);
    copy_memory(time_buffer, ref_time_text.value, ref_time_text.length * sizeof(TCHAR));
    time_buffer[ref_time_text.length] = 0;
    TEXT time_text = wrap_text_with_length(time_buffer, ref_time_text.length, false, library_log_memory_arena_resource);
    clear_builder(&sb);

    TEXT caller_format = wrap_text(_T("%t:%zd"));
    append_builder_format(&sb, &caller_format, &caller_file_text, caller_line);
    TEXT ref_caller_text = reference_text_string_builder(&sb);
    new_stack_or_heap_array(caller_buffer, caller_array, TCHAR, ref_caller_text.length + 1, 1024, library_log_memory_arena_resource);
    copy_memory(caller_buffer, ref_caller_text.value, ref_caller_text.length * sizeof(TCHAR));
    TEXT caller_text = wrap_text_with_length(caller_buffer, ref_caller_text.length, false, library_log_memory_arena_resource);

    release_string_builder(&sb);

    LOG_ITEM log_item = {
        .caller_file = &caller_file_text,
        .caller_line = caller_line,
        .log_level = log_level,
        .message = message,
        .timestamp = &timestamp,
        .datetime = datetime,
        .format = {
            .date = &date_text,
            .time = &time_text,
            .caller = &caller_text,
        },
    };

    logging_default(&log_item);
    logging_logger(&log_item);

    release_stack_or_heap_array(date_array);
    release_stack_or_heap_array(time_array);
    release_stack_or_heap_array(caller_array);
}

static bool can_logging(LOG_LEVEL log_level)
{
    if (log_level < library_default_log_level) {
        return false;
    }

    bool enable_default = is_enabled_file_writer(&library_default_log_file_writer);
    bool enable_logger = false;
    if (!enable_default) {
        for (size_t i = 0; i < SIZEOF_ARRAY(library_log_loggers); i++) {
            if (library_log_loggers[i].function) {
                enable_logger = true;
                break;
            }
        }
    }

    return enable_default || enable_logger;
}

void library_format_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* format, ...)
{
    if (!can_logging(log_level)) {
        return;
    }

    DATETIME datetime = get_current_datetime();

    va_list ap;
    va_start(ap, format);

    TEXT text_format = wrap_text(format);
    STRING_BUILDER sb = new_string_builder(MESSAGE_CAPACITY, library_log_memory_arena_resource);
    append_builder_vformat(&sb, &text_format, ap);
    TEXT message = build_text_string_builder(&sb);

    release_string_builder(&sb);

    va_end(ap);

    logging(log_level, caller_file, caller_line, &message, &datetime);

    release_text(&message);

}

void library_put_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* message)
{
    if (!can_logging(log_level)) {
        return;
    }

    DATETIME datetime = get_current_datetime();

    TEXT text_message = wrap_text(message);

    logging(log_level, caller_file, caller_line, &text_message, &datetime);
}
