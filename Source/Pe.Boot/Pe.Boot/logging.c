#include "logging.h"

void outputDebug(const TCHAR* s)
{
#if _DEBUG
    OutputDebugString(s);
    OutputDebugString(_T("\r\n"));
#endif
}



void log(LOG_LEVEL logLevel, const TCHAR* format, ...)
{
    //TODO: OutputDebugString に渡す感じでいいと思う
}
