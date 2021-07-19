#include "common.h"
#include "debug.h"

void output_debug(const TCHAR* s)
{
#if _DEBUG
    OutputDebugString(s);
    OutputDebugString(NEWLINET);
#endif
}
