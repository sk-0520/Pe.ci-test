#pragma once
#include <tchar.h>

#ifdef GLOBAL
#   define GLOBAL extern
#else
#   define GLOBAL
#endif

#define NEWLINE "\r\n"
#define NEWLINET _T(NEWLINE)

#define SIZEOF_ARRAY(arr) (sizeof(arr) / sizeof(arr[0]))

#ifdef _WIN64
typedef __int64 ssize_t;
#else
typedef __int32 ssize_t;
#endif

#define MIN(a, b) (((a) < (b)) ? (a): (b))
#define MAX(a, b) (((a) > (b)) ? (a): (b))
