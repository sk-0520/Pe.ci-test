#include <windows.h>

#include "logging.h"

void setup_default_log(FILE_RESOURCE* fileResource, LOG_LEVEL logLevel)
{
    library__default_log_level = logLevel;
    library__default_log_file_pointer = *fileResource;
}
void cleanup_default_log()
{
    close_file_resource(&library__default_log_file_pointer);
}

void logging(LOG_LEVEL logLevel, const TCHAR* format, const TCHAR* file, size_t line, ...)
{
    //TODO: OutputDebugString に渡す感じでいいと思う
}
