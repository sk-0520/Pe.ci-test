#pragma once
#include <stdint.h>

/*
* 💩＜ようこそ！
*/

extern int _fltused;

void* __cdecl memset(void* dest, int c, size_t bytes);
#pragma intrinsic(memset)

void* __cdecl memcpy(void* dest, const void* src, size_t bytes);
#pragma intrinsic(memcpy)

void* __cdecl memmove(void* dest, const void* src, size_t bytes);
#pragma intrinsic(memmove)

int __cdecl memcmp(const void* buf1, const void* buf2, size_t bytes);
#pragma intrinsic(memcmp)

