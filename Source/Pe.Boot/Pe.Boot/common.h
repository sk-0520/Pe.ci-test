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

typedef enum tag_LOCALE_TYPE
{
    /// <summary>
    /// ロケール非依存。
    /// </summary>
    LOCALE_TYPE_INVARIANT = LOCALE_INVARIANT,
    /// <summary>
    /// システムのロケール。
    /// </summary>
    LOCALE_TYPE_SYSTEM_DEFAULT = LOCALE_SYSTEM_DEFAULT,
    /// <summary>
    /// ユーザーのロケール。
    /// </summary>
    LOCALE_TYPE_USER_DEFAULT = LOCALE_USER_DEFAULT,
} LOCALE_TYPE;
