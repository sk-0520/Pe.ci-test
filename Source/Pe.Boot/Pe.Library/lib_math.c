#include "lib_math.h"

int _fltused = 0x9875;

size_t power_of_2(size_t n)
{
    if (!n) {
        return 0;
    }

    if (!(n & (n - 1))) {
        return n;
    }

    size_t result = 1;
    for (; n; n >>= 1) {
        result <<= 1;
    }
    return result;
}

#ifdef _M_IX86

// https://hero.handmade.network/forums/code-discussion/t/94-guide_-_how_to_avoid_c_c++_runtime_on_windows
// float to int64 cast
// on /arch:IA32
// on /arch:SSE
// on /arch:SSE2 with /d2noftol3 compiler switch
__declspec(naked) void _ftol2()
{
    __asm
    {
        fistp qword ptr[esp - 8]
        mov   edx, [esp - 4]
        mov   eax, [esp - 8]
        ret
    }
}

// float to int64 cast on /arch:IA32
__declspec(naked) void _ftol2_sse()
{
    __asm
    {
        fistp dword ptr[esp - 4]
        mov   eax, [esp - 4]
        ret
    }
}
/*
// float to uint32 cast on / arch:SSE2
__declspec(naked) void _ftoui3()
{

}

// float to int64 cast on / arch:SSE2
__declspec(naked) void _ftol3()
{

}

// float to uint64 cast on / arch:SSE2
__declspec(naked) void _ftoul3()
{

}

// int64 to float cast on / arch:SSE2
__declspec(naked) void _ltod3()
{

}

// uint64 to float cast on / arch:SSE2
__declspec(naked) void _ultod3()
{

}
*/
#endif
