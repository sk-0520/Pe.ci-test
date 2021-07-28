#include "common.h"
#include "debug.h"

void output_debug(const TCHAR* s)
{
#ifndef NDEBUG
    OutputDebugString(s);
    OutputDebugString(NEWLINET);
#endif
}

#ifndef NDEBUG

void assert_impl(const TCHAR* message, const TCHAR* expression, const TCHAR* caller_file, size_t caller_line)
{
    output_debug(message);
    DebugBreak();
    ExitProcess(9);
}

#endif
