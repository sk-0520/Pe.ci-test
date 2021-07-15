#include <windows.h>

#include "logging.h"

void setupDefaultLog(FILE_POINTER* filePointer, LOG_LEVEL logLevel)
{
    s_defaultLogLevel = logLevel;
    s_defaultLogFilePointer = *filePointer;
}
void cleanupDefaultLog()
{
    closeFile(&s_defaultLogFilePointer);
}

void logging(LOG_LEVEL logLevel, const TCHAR* format, const TCHAR* file, size_t line, ...)
{
    //TODO: OutputDebugString に渡す感じでいいと思う
}
