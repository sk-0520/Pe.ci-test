#include "debug.h"

void outputDebug(const TCHAR* s)
{
#if _DEBUG
    OutputDebugString(s);
    OutputDebugString(_T("\r\n"));
#endif
}
