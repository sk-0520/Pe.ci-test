#include "without_crt.h"

#include <Windows.h>
#include <intrin.h>

#pragma function(memset)
void* __cdecl memset(void* dest, int value, size_t num)
{
    __stosb((unsigned char*)dest, (unsigned char)value, num);
    return dest;
}

#pragma function(memcpy)
void* __cdecl memcpy(void* dest, const void* src, size_t num)
{
    __movsb((unsigned char*)dest, (unsigned char*)src, num);
    return dest;
}
