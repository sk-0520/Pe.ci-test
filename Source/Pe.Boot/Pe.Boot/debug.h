#pragma once
#include <tchar.h>

#include <Windows.h>

#if defined(_DEBUG)
#   define Assert(expr) do { if(!(expr)) DebugBreak(); } while(0)
#else
#   define Assert(ignore) ((void)0)
#endif


void outputDebug(const TCHAR * s);

