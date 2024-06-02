#pragma once
#include <stdint.h>
#include <stdbool.h>

#include <Windows.h>

/// <summary>
/// タイムゾーン。
/// <para>使ってない。</para>
/// </summary>
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
typedef union tag_DATETIME
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
    /// <summary>
    /// 年。
    /// </summary>
    int16_t year;
    /// <summary>
    /// 月。
    /// </summary>
    int16_t month;
    /// <summary>
    /// 日。
    /// </summary>
    uint8_t day;
    /// <summary>
    /// 時。
    /// </summary>
    uint8_t hour;
    /// <summary>
    /// 分。
    /// </summary>
    uint8_t minute;
    /// <summary>
    /// 秒。
    /// </summary>
    uint8_t second;
    /// <summary>
    /// ミリ秒。
    /// </summary>
    uint16_t millisecond;
    /// <summary>
    /// 曜日。
    /// </summary>
    DAY_OF_WEEK day_of_week : 16;
    struct
    {
        /// <summary>
        /// UTCか。
        /// </summary>
        bool is_utc;
    } library;
} TIMESTAMP;

TIME_ZONE get_current_time_zone(void);

/// <summary>
/// 現在時間(UTC)の取得。
/// </summary>
/// <returns></returns>
DATETIME get_current_datetime(void);

/// <summary>
/// 指定の日時で時間を生成。
/// </summary>
/// <param name="is_utc">入力値はUTCか。</param>
/// <param name="year">年。</param>
/// <param name="month">月。</param>
/// <param name="day">日。</param>
/// <param name="hour">時。</param>
/// <param name="minute">分。</param>
/// <param name="second">秒。</param>
/// <param name="milli_sec">ミリ秒。</param>
/// <returns>生成された日時。</returns>
DATETIME create_datetime(bool is_utc, unsigned int year, unsigned int month, unsigned int day, unsigned int hour, unsigned int minute, unsigned int second, unsigned int milli_sec);

/// <summary>
/// <see cref="DATETIME" />を<see cref="TIMESTAMP" />に変換。
/// </summary>
/// <param name="datetime"></param>
/// <param name="to_utc">UTCにするか。</param>
/// <returns></returns>
TIMESTAMP datetime_to_timestamp(const DATETIME* datetime, bool to_utc);

