#include "without_crt.h"

#include <Windows.h>
#include <intrin.h>
#include <stdint.h>

int _fltused = 0x9875;

#pragma function(memset)
void* __cdecl memset(void* dest, int value, size_t bytes)
{
#pragma warning(push)
#pragma warning(disable:6001)
    __stosb((uint8_t*)dest, (uint8_t)value, bytes);
#pragma warning(pop)
    return dest;
}

#pragma function(memcpy)
void* __cdecl memcpy(void* dest, const void* src, size_t bytes)
{
#pragma warning(push)
#pragma warning(disable:6001)
    __movsb((uint8_t*)dest, (uint8_t*)src, bytes);
#pragma warning(pop)
    return dest;
}

#pragma function(memmove)
void* __cdecl memmove(void* dest, const void* src, size_t bytes)
{
    if (src == dest || !bytes) {
        return dest;
    }

    if (src < dest) {
        // 後ろからコピー
        uint8_t* d = ((uint8_t*)dest) + bytes;
        const uint8_t* s = ((const uint8_t*)src) + bytes;
        do {
            *--d = *--s;
        } while (--bytes);
        return dest;
    }

    // アドレスが被らないので前からコピー
    return memcpy(dest, src, bytes);
}

#pragma function(memcmp)
int __cdecl memcmp(const void* buf1, const void* buf2, size_t bytes)
{
    const uint8_t* m1 = (const uint8_t*)buf1;
    const uint8_t* m2 = (const uint8_t*)buf2;

    if (!bytes) {
        return 0;
    }

    while (bytes--) {
        if (*m1++ != *m2++) {
            return *m1 < *m2 ? -1 : 1;
        }
    }

    return 0;
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
