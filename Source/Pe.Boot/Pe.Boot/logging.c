#include <stdarg.h>

#include <windows.h>

#include "logging.h"

static FILE_WRITER library__default_log_file_writer;
static LOG_LEVEL library__default_log_level;

void setup_default_log(FILE_WRITER* file_writer, LOG_LEVEL log_level)
{
    library__default_log_file_writer = *file_writer;
    library__default_log_level = log_level;
}
void cleanup_default_log()
{
    free_file_writer(&library__default_log_file_writer);
}

static void logging_core(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* format, va_list ap)
{
    // もう寝ていい？
    write_string_file_writer(&library__default_log_file_writer, format, true);
}

void logging(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* format, ...)
{
    va_list ap;
    va_start(ap, format);

    logging_core(log_level, caller_file, caller_line, format, ap);

    va_end(ap);
}
