#pragma once
#include <tchar.h>

/// <summary>
/// ログレベル。
/// </summary>
typedef enum
{
    LOG_LEVEL_TRACE,
    LOG_LEVEL_DEBUG,
    LOG_LEVEL_INFO,
    LOG_LEVEL_WARNING,
    LOG_LEVEL_ERROR,
} LOG_LEVEL;

/// <summary>
/// デバッグ時のみ使用可能なログ出力。
///
/// 内部的には<c>OutputDebugString</c>だが<c>DEBUG</c>定義がある場合にのみ使用される。
/// </summary>
/// <param name="s"></param>
void outputDebug(const TCHAR* s);

/// <summary>
/// ログ出力。
/// </summary>
/// <param name="logLevel"></param>
/// <param name="format"></param>
/// <param name=""></param>
void logging(LOG_LEVEL logLevel, const TCHAR* format, ...);

#define logTrace(format, ...) do { logging(LOG_LEVEL_TRACE, format, __VA_ARGS__) } while(0)
#define logDebug(format, ...) do { logging(LOG_LEVEL_DEBUG, format, __VA_ARGS__) } while(0)
#define logInformation(format, ...) do { logging(LOG_LEVEL_INFO, format, __VA_ARGS__) } while(0)
#define logWarning(format, ...) do { logging(LOG_LEVEL_WARNING, format, __VA_ARGS__) } while(0)
#define logError(format, ...) do { logging(LOG_LEVEL_ERROR, format, __VA_ARGS__) } while(0)
