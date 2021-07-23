#include "common.h"
#include "debug.h"

void output_debug(const TCHAR* s)
{
#if _DEBUG
    OutputDebugString(s);
    OutputDebugString(NEWLINET);
#endif
}

void assert_debug_impl(const TCHAR* message, const TCHAR* expression, const TCHAR* caller_file, size_t caller_line)
{
    output_debug(message);
    ExitProcess(9);
}
