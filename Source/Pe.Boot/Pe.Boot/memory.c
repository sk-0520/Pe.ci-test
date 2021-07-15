#if MEM_CHECK
#   include <stdio.h>
#   include <tchar.h>
#endif

#include <windows.h>

#include "common.h"
#include "memory.h"

#if MEM_CHECK

#define MEM_CHECK_CALLER_FILE_PATH MAX_PATH

#define MEM_CHECK_ALLOC_STOCK_LENGTH (1024 * 4)
#define MEM_CHECK_PRINT_BUFFER_LENGTH (100 + (MEM_CHECK_CALLER_FILE_PATH * 2))


typedef struct
{
    void* p;
    TCHAR file[MEM_CHECK_CALLER_FILE_PATH];
    size_t line;
} mem_check__ALLOC_STOCK_ITEM;

static mem_check__ALLOC_STOCK_ITEM mem_check__allocStocks[MEM_CHECK_ALLOC_STOCK_LENGTH] = { 0 };
static size_t mem_check__allocStocksCount = 0;

#define mem_check__debugOutput(format, ...) do { \
    TCHAR mem_check__debugBuffer[MEM_CHECK_PRINT_BUFFER_LENGTH] = { 0 }; \
    size_t mem_check__debugBufferLength = (sizeof(mem_check__debugBuffer) - 1) / sizeof(TCHAR); \
    swprintf(mem_check__debugBuffer, mem_check__debugBufferLength, _T(format) NEWLINET, __VA_ARGS__); \
    OutputDebugString(mem_check__debugBuffer); \
} while (0)

static void mem_check__debugHeap(void* p, bool allocate, const TCHAR * callerFile, size_t callerLine)
{
    if (allocate) {
        bool stocked = false;
        for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
            if (!mem_check__allocStocks[i].p) {
                mem_check__debugOutput("[MEM:+] %p %s(%zu)", p, callerFile, callerLine);

                mem_check__ALLOC_STOCK_ITEM data = {
                    .p = p,
                    .line = callerLine,
                };
                data.file[0] = 0;
                lstrcpy(data.file, callerFile);

                mem_check__allocStocks[i] = data;
                mem_check__allocStocksCount += 1;
                stocked = true;
                break;
            }
        }

        if (!stocked) {
            mem_check__debugOutput("[MEM:STOCK:ERROR] %p %s(%zu)", p, callerFile, callerLine);
        }
    } else {
        bool exists = false;;
        for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
            if (mem_check__allocStocks[i].p == p) {
                mem_check__debugOutput("[MEM:-] %p %s(%zu) - %s(%zu)", p, callerFile, callerLine, mem_check__allocStocks[i].file, mem_check__allocStocks[i].line);

                mem_check__allocStocksCount -= 1;
                memset(&mem_check__allocStocks[i], 0, sizeof(mem_check__ALLOC_STOCK_ITEM));
                exists = true;
            }
        }

        if (!exists) {
            mem_check__debugOutput("[MEM:NOTFOUND:ERROR] %p %s(%zu)", p, callerFile, callerLine);
        }
    }
}

void mem_check__printAllocateMemory(bool leak)
{
    TCHAR head[MEM_CHECK_PRINT_BUFFER_LENGTH];
    swprintf(head, (sizeof(head) - 1) / sizeof(TCHAR), _T("[MEM:%s:COUNT] %zu%s"), mem_check__allocStocksCount && leak ? _T("ERROR") : _T("INFORMATION"), mem_check__allocStocksCount, NEWLINET);
    OutputDebugString(head);

    for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
        mem_check__ALLOC_STOCK_ITEM item = mem_check__allocStocks[i];
        if (item.p) {
            TCHAR body[MEM_CHECK_PRINT_BUFFER_LENGTH];
            TCHAR* format = leak
                ? _T("[MEM:WARNING:LEAK] %p %s(%zu)%s")
                : _T("[MEM:STOCK] %p %s(%zu)%s")
                ;
            swprintf(body, (sizeof(body) - 1) / sizeof(TCHAR), format, item.p, item.file, item.line, NEWLINET);
            OutputDebugString(body);
        }
    }
}

#endif

#if MEM_CHECK
void* mem_check__allocateMemory(size_t bytes, bool zeroFill, const TCHAR * callerFile, size_t callerLine)
#else
void* allocateMemory(size_t bytes, bool zeroFill)
#endif
{
    HANDLE hHeap = GetProcessHeap();
    if (!hHeap) {
        return NULL;
    }

    void* heap = HeapAlloc(hHeap, zeroFill ? HEAP_ZERO_MEMORY : 0, bytes);
#if MEM_CHECK
    mem_check__debugHeap(heap, true, callerFile, callerLine);
#endif
    return heap;
}

#if MEM_CHECK
void* mem_check__allocateClearMemory(size_t count, size_t typeSize, const TCHAR * callerFile, size_t callerLine)
{
    return mem_check__allocateMemory(count * typeSize, true, callerFile, callerLine);
}
#else
void* allocateClearMemory(size_t count, size_t typeSize)
{
    return allocateMemory(count * typeSize, true);
}
#endif

#if MEM_CHECK
void mem_check__freeMemory(void* p, const TCHAR * callerFile, size_t callerLine)
#else
void freeMemory(void* p)
#endif
{
#if MEM_CHECK
    mem_check__debugHeap(p, false, callerFile, callerLine);
#endif

    HeapFree(GetProcessHeap(), 0, p);
}

void* setMemory(void* target, int value, size_t bytes)
{
    /*
    unsigned char* p = (unsigned char*)target;
    const unsigned char v = (unsigned char)value;

    while (bytes--) {
        *p++ = v;
    }

    return target;
    */
    //NOTE: CRT!
    return FillMemory(target, bytes, value);
}

void* copyMemory(void* destination, void* source, size_t bytes)
{
    //NOTE: CRT!
    return CopyMemory(destination, source, bytes);
}

void* moveMemory(void* destination, void* source, size_t bytes)
{
    //NOTE: CRT!
    return MoveMemory(destination, source, bytes);
}
