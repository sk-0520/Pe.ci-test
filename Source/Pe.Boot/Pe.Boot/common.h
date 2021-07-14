#pragma once
#include <tchar.h>

#ifdef GLOBAL
#   define GLOBAL extern
#else
#   define GLOBAL
#endif

#define NEWLINE "\r\n"
#define NEWLINET _T(NEWLINE)
