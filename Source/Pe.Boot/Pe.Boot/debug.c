#include "common.h"
#include "debug.h"

void outputDebug(const TCHAR* s)
{
#if _DEBUG
    OutputDebugString(s);
    OutputDebugString(NEWLINET);
#endif
}
