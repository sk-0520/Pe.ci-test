#include "without_crt.h"

#include <Windows.h>
#include <intrin.h>
#include <stdint.h>

#pragma function(memset)
void* __cdecl memset(void* dest, int value, size_t bytes)
{
    __stosb((uint8_t*)dest, (uint8_t)value, bytes);
    return dest;
}

#pragma function(memcpy)
void* __cdecl memcpy(void* dest, const void* src, size_t bytes)
{
    __movsb((uint8_t*)dest, (uint8_t*)src, bytes);
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
