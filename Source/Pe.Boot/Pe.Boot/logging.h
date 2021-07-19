#pragma once
#include <tchar.h>

#include "fsio.h"

/// <summary>
/// ログレベル。
/// </summary>
typedef enum tag_LOG_LEVEL
{
    LOG_LEVEL_TRACE,
    LOG_LEVEL_DEBUG,
    LOG_LEVEL_INFO,
    LOG_LEVEL_WARNING,
    LOG_LEVEL_ERROR,
} LOG_LEVEL;


static LOG_LEVEL library__default_log_level;
static FILE_POINTER library__default_log_file_pointer;

void setup_default_log(FILE_POINTER* filePointer, LOG_LEVEL logLevel);
void cleanup_default_log();

/// <summary>
/// ログ出力。
/// </summary>
/// <param name="logLevel"></param>
/// <param name="format"></param>
/// <param name=""></param>
void logging(LOG_LEVEL logLevel, const TCHAR* format, const TCHAR* file, size_t line, ...);

#define log_level(level, format, ...) logging((level), _T(__FILE__), __LINE__, format, __VA_ARGS__)
#define log_trace(format, ...) logging(LOG_LEVEL_TRACE, _T(__FILE__), __LINE__, format, __VA_ARGS__)
#define log_debug(format, ...) logging(LOG_LEVEL_DEBUG, _T(__FILE__), __LINE__, format, __VA_ARGS__)
#define log_information(format, ...) logging(LOG_LEVEL_INFO, _T(__FILE__), __LINE__, format, __VA_ARGS__)
#define log_warning(format, ...) logging(LOG_LEVEL_WARNING, _T(__FILE__), __LINE__, format, __VA_ARGS__)
#define log_error(format, ...) logging(LOG_LEVEL_ERROR, _T(__FILE__), __LINE__, format, __VA_ARGS__)
