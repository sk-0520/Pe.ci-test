#pragma once
#include <stdint.h>
#include <stdbool.h>

#include <windows.h>

typedef struct tag_TIME_ZONE
{
    TIME_ZONE_INFORMATION tzi;
    struct
    {
        bool enabled;
    } library;
} TIME_ZONE;


/// <summary>
/// 日付の持ち運び用データ。
/// <para>アプリケーション側で明示的に生成しない。</para>
/// </summary>
typedef union  tag_DATETIME
{
    /// <summary>
    /// 実データ。
    /// <para>常にUTC。</para>
    /// </summary>
    FILETIME filetime;
    /// <summary>
    /// あれこれ演算用。
    /// </summary>
    uint64_t epoch;
} DATETIME;

/// <summary>
/// 曜日。
/// </summary>
typedef enum tag_DAY_OF_WEEK
{
    /// <summary>
    /// 日曜日。
    /// </summary>
    DAY_OF_WEEK_SUNDAY,
    /// <summary>
    /// 月曜日。
    /// </summary>
    DAY_OF_WEEK_MONDAY,
    /// <summary>
    /// 火曜日。
    /// </summary>
    DAY_OF_WEEK_TUESDAY,
    /// <summary>
    /// 水曜日。
    /// </summary>
    DAY_OF_WEEK_WEDNESDAY,
    /// <summary>
    /// 木曜日。
    /// </summary>
    DAY_OF_WEEK_THURSDAY,
    /// <summary>
    /// 金曜日。
    /// </summary>
    DAY_OF_WEEK_FRIDAY,
    /// <summary>
    /// 土曜日。
    /// </summary>
    DAY_OF_WEEK_SATURDAY,
} DAY_OF_WEEK;

/// <summary>
/// 日付の分解データ。
/// <para>アプリケーション側で明示的に生成しない。</para>
/// </summary>
typedef struct tag_TIMESTAMP
{
    int16_t year;
    int16_t month;
    uint8_t day;
    uint8_t hour;
    uint8_t minute;
    uint8_t second;
    uint16_t milli_sec;
    DAY_OF_WEEK day_of_week : 16;
    struct
    {
        /// <summary>
        /// UTCか。
        /// </summary>
        bool is_utc;
    } library;
} TIMESTAMP;

TIME_ZONE get_current_time_zone();

/// <summary>
/// 現在時間(UTC)の取得。
/// </summary>
/// <returns></returns>
DATETIME get_current_datetime();

DATETIME create_datetime(bool is_utc, unsigned int year, unsigned int month, unsigned int day, unsigned int hour, unsigned int minute, unsigned int second, unsigned int milli_sec);

/// <summary>
/// <c>DATETIME</c>を<c>TIMESTAMP</c>に変換。
/// </summary>
/// <param name="datetime"></param>
/// <param name="to_utc">UTCにするか。</param>
/// <returns></returns>
TIMESTAMP datetime_to_timestamp(const DATETIME* datetime, bool to_utc);

