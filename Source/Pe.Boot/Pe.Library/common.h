#pragma once
#include <stdint.h>

#include <windows.h>
#include <tchar.h>

/// <summary>
/// バイト長。
/// </summary>
typedef size_t byte_t;

/// <summary>
/// テキスト長。
/// <para>通常のC文字列としてもこの長さを最大長とする。</para>
/// </summary>
typedef uint32_t text_t;

#define TEXT_MAX (UINT32_MAX)

#define NEWLINE_CR "\r"
#define NEWLINE_LF "\n"
#define NEWLINE_CRLF "\r\n"

#define NEWLINE_CRT _T(NEWLINE_CR)
#define NEWLINE_LFT _T(NEWLINE_LF)
#define NEWLINE_CRLFT _T(NEWLINE_CRLF)

#define NEWLINE NEWLINE_CRLF
#define NEWLINET _T(NEWLINE)

#define TO_STRING_CORE(x) #x
#define TO_STRING(literal) TO_STRING_CORE(literal)

/// 配列サイズの取得
#define SIZEOF_ARRAY(arr) (size_t)(sizeof(arr) / sizeof(arr[0]))

#define FILE_BASE_DIR TO_STRING(SOLUTION_DIR)
#define RELATIVE_FILE (__FILE__ + (sizeof(FILE_BASE_DIR) - 4 /* "\."\0 */))
#define RELATIVE_FILET (_T(__FILE__) + ((sizeof(_T(FILE_BASE_DIR)) / sizeof(TCHAR)) - 4 /* "\."\0 */))
#ifdef UNICODE
#   define FUNCTIONT __FUNCTIONW__
#else
#   define FUNCTIONT __func__
#endif // UNICODE



#ifdef _WIN64
typedef int64_t ssize_t;
typedef double real_t;
#else
typedef int32_t ssize_t;
typedef float real_t;
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


