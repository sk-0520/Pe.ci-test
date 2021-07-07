#pragma once
#include <stdbool.h>
#include <stddef.h>

/// <summary>
/// 指定したサイズ以上のヒープ領域を確保。
/// </summary>
/// <param name="bytes"></param>
/// <returns></returns>
void* allocateMemory(size_t bytes, bool zeroFill);
void* allocateClearMemory(size_t count, size_t typeSize);

/// <summary>
/// allocateMemory で確保した領域を解放。
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
void freeMemory(void* p);
