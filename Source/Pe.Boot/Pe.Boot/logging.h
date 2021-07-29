#pragma once
#include <stddef.h>

#include <tchar.h>

#include "fsio.h"
#include "datetime.h"

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

typedef struct tag_LOG_ITEM
{
    /// <summary>
    /// ログレベル。
    /// </summary>
    LOG_LEVEL log_level;
    /// <summary>
    /// 呼び出し元ファイルパス。
    /// </summary>
    const TEXT* caller_file;
    /// <summary>
    /// 呼び出し元ファイル行番号。
    /// </summary>
    size_t caller_line;
    /// <summary>
    /// ログ日時(ローカル)
    /// </summary>
    const TIMESTAMP* timestamp;
    const DATETIME* datetime;
    /// <summary>
    /// ログ内容。
    /// <para>番兵なしの可能性あり。</para>
    /// </summary>
    const TEXT* message;
    struct
    {
        const TEXT* date;
        const TEXT* time;
        const TEXT* caller;
    } format;
} LOG_ITEM;

/// <summary>
/// ログ出力拡張。
/// </summary>
/// <param name="log_item">ログ内容。関数内でのみ有効。永続させる場合は複製すること。</param>
/// <param name="data">アタッチ時のデータ。</param>
typedef void (*func_custom_logger)(const LOG_ITEM* log_item, void* data);

typedef struct tag_LOGGER
{
    func_custom_logger function;
    void* data;
} LOGGER;

void setup_default_log(FILE_WRITER* file_writer, LOG_LEVEL log_level);
void cleanup_default_log();

/// <summary>
/// ロガーを追加。
/// </summary>
/// <param name="logger"></param>
/// <returns>成功時に0以上のログID。失敗時は負数。</returns>
ssize_t attach_logger(const LOGGER* logger);
/// <summary>
/// ロガーを破棄。
/// </summary>
/// <param name="log_id">attach_loggerで取得したログID。</param>
bool detach_logger(ssize_t log_id);

/// <summary>
/// 書式付きログ出力。
/// <para>アプリケーション側では明示的に使用しない。<c>logger_format_*</c>を使用すること。</para>
/// </summary>
/// <param name="log_level">ログレベル。</param>
/// <param name="format">書式。</param>
void library__format_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* format, ...);
/// <summary>
/// 書式なしログ出力。
/// <para>アプリケーション側では明示的に使用しない。<c>logger_put_*</c>を使用すること。</para>
/// </summary>
/// <param name="log_level">ログレベル。</param>
/// <param name="message">メッセージ。</param>
void library__put_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* message);

#define logger_format_level(level, format, ...) library__format_log((level), RELATIVE_FILET, __LINE__, format, __VA_ARGS__)
#define logger_format_trace(format, ...) logger_format_level(LOG_LEVEL_TRACE, format, __VA_ARGS__)
#define logger_format_debug(format, ...) logger_format_level(LOG_LEVEL_DEBUG, format, __VA_ARGS__)
#define logger_format_information(format, ...) logger_format_level(LOG_LEVEL_INFO, format, __VA_ARGS__)
#define logger_format_warning(format, ...) logger_format_level(LOG_LEVEL_WARNING, format, __VA_ARGS__)
#define logger_format_error(format, ...) logger_format_level(LOG_LEVEL_ERROR, format, __VA_ARGS__)

#define logger_put_level(level, message) library__put_log((level), RELATIVE_FILET, __LINE__, message)
#define logger_put_trace(message) logger_put_level(LOG_LEVEL_TRACE, message)
#define logger_put_debug(message) logger_put_level(LOG_LEVEL_DEBUG, message)
#define logger_put_information(message) logger_put_level(LOG_LEVEL_INFO, message)
#define logger_put_warning(message) logger_put_level(LOG_LEVEL_WARNING, message)
#define logger_put_error(message) logger_put_level(LOG_LEVEL_ERROR, message)
