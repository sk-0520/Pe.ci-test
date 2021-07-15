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


static LOG_LEVEL s_defaultLogLevel;
static FILE_POINTER s_defaultLogFilePointer;

void setupDefaultLog(FILE_POINTER* filePointer, LOG_LEVEL logLevel);
void cleanupDefaultLog();

/// <summary>
/// ログ出力。
/// </summary>
/// <param name="logLevel"></param>
/// <param name="format"></param>
/// <param name=""></param>
void logging(LOG_LEVEL logLevel, const TCHAR* format, const TCHAR* file, size_t line, ...);

#define logLevel(level, format, ...) logging((level), __FILEW__, __LINE__, format, __VA_ARGS__)
#define logTrace(format, ...) logging(LOG_LEVEL_TRACE, __FILEW__, __LINE__, format, __VA_ARGS__)
#define logDebug(format, ...) logging(LOG_LEVEL_DEBUG, __FILEW__, __LINE__, format, __VA_ARGS__)
#define logInformation(format, ...) logging(LOG_LEVEL_INFO, __FILEW__, __LINE__, format, __VA_ARGS__)
#define logWarning(format, ...) logging(LOG_LEVEL_WARNING, __FILEW__, __LINE__, format, __VA_ARGS__)
#define logError(format, ...) logging(LOG_LEVEL_ERROR, __FILEW__, __LINE__, format, __VA_ARGS__)
