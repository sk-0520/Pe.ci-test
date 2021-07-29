#pragma once
#include <stdint.h>

#include <windows.h>
#include <tchar.h>

/// <summary>
/// バイト長。
/// </summary>
typedef size_t byte_t;

#define NEWLINE_CR "\r"
#define NEWLINE_LF "\n"
#define NEWLINE_CRLF "\r\n"

#define NEWLINE_CRT _(NEWLINE_CR)
#define NEWLINE_LFT _(NEWLINE_LF)
#define NEWLINE_CRLFT _(NEWLINE_CRLF)

#define NEWLINE NEWLINE_CRLF
#define NEWLINET _T(NEWLINE)

#define TO_STRING_CORE(x) #x
#define TO_STRING(literal) TO_STRING_CORE(literal)

#define SIZEOF_ARRAY(arr) (sizeof(arr) / sizeof(arr[0]))

#define FILE_BASE_DIR TO_STRING(SOLUTION_DIR)
#define RELATIVE_FILE (__FILE__ + (sizeof(FILE_BASE_DIR) - 4 /* "\."\0 */))
#define RELATIVE_FILET (_T(__FILE__) + ((sizeof(_T(FILE_BASE_DIR)) / sizeof(TCHAR)) - 4 /* "\."\0 */))

#ifdef _WIN64
typedef int64_t ssize_t;
#else
typedef int32_t ssize_t;
#endif

#define MIN(a, b) (((a) < (b)) ? (a): (b))
#define MAX(a, b) (((a) > (b)) ? (a): (b))

typedef union tag_DATA_INT64
{
    LARGE_INTEGER large;
    int64_t plain;
} DATA_INT64;

typedef union tag_DATA_UINT64
{
    ULARGE_INTEGER large;
    uint64_t plain;
} DATA_UINT64;

// ライブラリ側使用オプションは _: で始まる(アプリケーション側は _ のみ)
#define OPTION_LOG_FILE_KEY _T("_:log-file")
#define OPTION_LOG_LEVEL_KEY _T("_:log-level")


