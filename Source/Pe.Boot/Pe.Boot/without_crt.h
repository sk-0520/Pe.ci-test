#pragma once
#include <stddef.h>

void* __cdecl memset(void* dest, int c, size_t num);
#pragma intrinsic(memset)

void* __cdecl memcpy(void* dest, const void* src, size_t num);
#pragma intrinsic(memcpy)
