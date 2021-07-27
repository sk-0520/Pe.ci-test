#pragma once
#include <stdint.h>

#include <windows.h>
#include <tchar.h>

#ifdef GLOBAL
#   define GLOBAL extern
#else
#   define GLOBAL
#endif

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
#define RELATIVE_FILET (_T(__FILE__) + (sizeof(FILE_BASE_DIR) - 4 /* "\."\0 */))

#ifdef _WIN64
typedef __int64 ssize_t;
#else
typedef __int32 ssize_t;
#endif

#define MIN(a, b) (((a) < (b)) ? (a): (b))
#define MAX(a, b) (((a) > (b)) ? (a): (b))

typedef union tag_INT_64
{
    LARGE_INTEGER large;
    int64_t plain;
} INT_64;

