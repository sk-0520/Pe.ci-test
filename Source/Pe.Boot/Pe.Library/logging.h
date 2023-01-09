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
    /// <summary>
    /// 細かいログ情報。
    /// <para>開発時のみの出力を想定。</para>
    /// </summary>
    LOG_LEVEL_TRACE,
    /// <summary>
    /// デバッグログ情報。
    /// <para>開発時かデバッグ時の出力を想定。</para>
    /// </summary>
    LOG_LEVEL_DEBUG,
    /// <summary>
    /// 通知ログ情報。
    /// </summary>
    LOG_LEVEL_INFO,
    /// <summary>
    /// 警告ログ情報。
    /// </summary>
    LOG_LEVEL_WARNING,
    /// <summary>
    /// 異常ログ情報。
    /// </summary>
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
    /// <summary>
    /// ログ日時(ローカル)。
    /// <para><c>timestamp</c>の素になったデータ。</para>
    /// </summary>
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

/// <summary>
/// ロガー。
/// </summary>
typedef struct tag_LOGGER
{
    /// <summary>
    /// ロガー実装。
    /// </summary>
    func_custom_logger function;
    /// <summary>
    /// <see cref="function" />に渡されるデータ。
    /// </summary>
    void* data;
} LOGGER;

/// <summary>
/// ログ操作時に使用するメモリリソース。
/// </summary>
/// <param name="memory_arena_resource"></param>
void initialize_logger(const MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// 標準のログファイル設定。
/// </summary>
/// <param name="file_writer">書き込み処理。<c>NULL</c>の場合は無効化。</param>
void set_default_log_file(FILE_WRITER* file_writer);
/// <summary>
/// 標準のログレベルを設定。
/// </summary>
/// <param name="log_level"></param>
void set_default_log_level(LOG_LEVEL log_level);
void cleanup_default_log(void);

/// <summary>
/// ロガーを追加。
/// </summary>
/// <param name="logger"></param>
/// <returns>成功時に0以上のログID。失敗時は負数。</returns>
ssize_t attach_logger(const LOGGER* logger);
/// <summary>
/// ロガーを破棄。
/// </summary>
/// <param name="log_id"><see cref="attach_logger" />で取得したログID。</param>
bool detach_logger(ssize_t log_id);

/// <summary>
/// 書式付きログ出力。
/// <para>アプリケーション側では明示的に使用しない。<c>logger_format_*</c>を使用すること。</para>
/// </summary>
/// <param name="log_level">ログレベル。</param>
/// <param name="format">書式。</param>
void library_format_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* format, ...);
/// <summary>
/// 書式なしログ出力。
/// <para>アプリケーション側では明示的に使用しない。<c>logger_put_*</c>を使用すること。</para>
/// </summary>
/// <param name="log_level">ログレベル。</param>
/// <param name="message">メッセージ。</param>
void library_put_log(LOG_LEVEL log_level, const TCHAR* caller_file, size_t caller_line, const TCHAR* message);

#define logger_format_level(level, format, ...) library_format_log((level), RELATIVE_FILET, __LINE__, format, __VA_ARGS__)
#define logger_format_trace(format, ...) logger_format_level(LOG_LEVEL_TRACE, format, __VA_ARGS__)
#define logger_format_debug(format, ...) logger_format_level(LOG_LEVEL_DEBUG, format, __VA_ARGS__)
#define logger_format_info(format, ...) logger_format_level(LOG_LEVEL_INFO, format, __VA_ARGS__)
#define logger_format_warn(format, ...) logger_format_level(LOG_LEVEL_WARNING, format, __VA_ARGS__)
#define logger_format_error(format, ...) logger_format_level(LOG_LEVEL_ERROR, format, __VA_ARGS__)

#define logger_put_level(level, message) library_put_log((level), RELATIVE_FILET, __LINE__, message)
#define logger_put_trace(message) logger_put_level(LOG_LEVEL_TRACE, message)
#define logger_put_debug(message) logger_put_level(LOG_LEVEL_DEBUG, message)
#define logger_put_info(message) logger_put_level(LOG_LEVEL_INFO, message)
#define logger_put_warn(message) logger_put_level(LOG_LEVEL_WARNING, message)
#define logger_put_error(message) logger_put_level(LOG_LEVEL_ERROR, message)
