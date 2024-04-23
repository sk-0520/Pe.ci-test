#include "datetime.h"

TIME_ZONE get_current_time_zone()
{
    TIME_ZONE time_zone = {
        .library = {
            .enabled = GetTimeZoneInformation(&time_zone.tzi) != TIME_ZONE_ID_UNKNOWN,
    },
    };

    return time_zone;
}

DATETIME get_current_datetime()
{
    DATETIME datetime = { 0 };
    GetSystemTimeAsFileTime(&datetime.filetime);
    return datetime;
}

DATETIME create_datetime(bool is_utc, unsigned int year, unsigned int month, unsigned int day, unsigned int hour, unsigned int minute, unsigned int second, unsigned int milli_sec)
{
    SYSTEMTIME systemTime = {
        .wYear = (WORD)year,
        .wMonth = (WORD)month,
        .wDay = (WORD)day,
        .wHour = (WORD)hour,
        .wMinute = (WORD)minute,
        .wSecond = (WORD)second,
        .wMilliseconds = (WORD)milli_sec,
    };

    FILETIME fileTime;
    SystemTimeToFileTime(&systemTime, &fileTime);

    if (!is_utc) {
        FILETIME utc;
        LocalFileTimeToFileTime(&fileTime, &utc);
        fileTime = utc;
    }

    DATETIME datetime = {
        .filetime = fileTime,
    };

    return datetime;
}

static void systemtime_to_timestamp(TIMESTAMP* result, const SYSTEMTIME* system_time, bool is_utc)
{
    result->year = system_time->wYear;
    result->month = system_time->wMonth;
    result->day = (uint8_t)system_time->wDay;
    result->hour = (uint8_t)system_time->wHour;
    result->minute = (uint8_t)system_time->wMinute;
    result->second = (uint8_t)system_time->wSecond;
    result->millisecond = system_time->wMilliseconds;
    result->day_of_week = system_time->wDayOfWeek;
    result->library.is_utc = is_utc;
}

TIMESTAMP datetime_to_timestamp(const DATETIME* datetime, bool to_utc)
{
    SYSTEMTIME systemtime;

    if (to_utc) {
        FileTimeToSystemTime(&datetime->filetime, &systemtime);
    } else {
        FILETIME local_filetime;
        FileTimeToLocalFileTime(&datetime->filetime, &local_filetime);
        FileTimeToSystemTime(&local_filetime, &systemtime);
    }

    //FileTimeToSystemTime(&datetime->filetime, &systemtime);

    TIMESTAMP timestamp;
    systemtime_to_timestamp(&timestamp, &systemtime, to_utc);

    return timestamp;
}

