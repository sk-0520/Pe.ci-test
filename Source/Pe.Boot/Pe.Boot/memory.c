#if MEM_CHECK
#   include <stdio.h>
#   include <tchar.h>
#endif

#include <windows.h>

#include "common.h"
#include "memory.h"

#if MEM_CHECK

#define MEM_CHECK_ALLOC_STOCK_LENGTH (1024 * 4)
#define MEM_CHECK_PRINT_BUFFER_LENGTH (1024 * 2)

typedef struct
{
    void* p;
    TCHAR file[/*MAX_PATH */1024];
    size_t line;
} MemCheckAllocStockItem;

static MemCheckAllocStockItem memCheckAllocStocks[MEM_CHECK_ALLOC_STOCK_LENGTH] = { 0 };
static size_t memCheckAllocStocksCount = 0;

static void _mem_check_debugHeap(void* p, bool allocate, const TCHAR* _file, size_t _line)
{
    TCHAR s[MEM_CHECK_PRINT_BUFFER_LENGTH] = { 0 };
    swprintf(s, (sizeof(s) - 1) / sizeof(TCHAR), _T("[MEM] %c %p %s(%zu)%s"), allocate ? '+' : '-', p, _file, _line, NEWLINET);
    OutputDebugString(s);
    if (allocate) {
        for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
            if (!memCheckAllocStocks[i].p) {
                MemCheckAllocStockItem data = {
                    .p = p,
                    .line = _line,
                };
                data.file[0] = 0;
                lstrcpy(data.file, _file);

                memCheckAllocStocks[i] = data;
                memCheckAllocStocksCount += 1;
                break;
            }
        }
    } else {
        for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
            bool exists = false;;
            MemCheckAllocStockItem existItem = { 0 };
            if (memCheckAllocStocks[i].p == p) {
                memCheckAllocStocksCount -= 1;
                existItem = memCheckAllocStocks[i];
                memset(&memCheckAllocStocks[i], 0, sizeof(MemCheckAllocStockItem));
                exists = true;
                break;
            }
            if (exists) {
                TCHAR e[MEM_CHECK_PRINT_BUFFER_LENGTH] = { 0 };
                swprintf(e, (sizeof(e) - 1) / sizeof(TCHAR), _T("[DMP:ERROR] %p %s(%zu) - %s(%zu)%s"), existItem.p, existItem.file, existItem.line, _file, _line, NEWLINET);
                OutputDebugString(e);
            }
        }
    }
}

void _mem_check_printAllocateMemory()
{
    TCHAR head[MEM_CHECK_PRINT_BUFFER_LENGTH];
    swprintf(head, (sizeof(head) - 1) / sizeof(TCHAR), _T("[MEM:%s:COUNT] %zu%s"), memCheckAllocStocksCount ? _T("ERROR") : _T("INFORMATION"), memCheckAllocStocksCount, NEWLINET);
    OutputDebugString(head);

    for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
        MemCheckAllocStockItem item = memCheckAllocStocks[i];
        if (item.p) {
            TCHAR body[MEM_CHECK_PRINT_BUFFER_LENGTH];
            swprintf(body, (sizeof(body) - 1) / sizeof(TCHAR), _T("[MEM:WARNING:LEAK] %p %s(%zu)%s"), item.p, item.file, item.line, NEWLINET);
            OutputDebugString(body);
        }
    }
}

#endif

#if MEM_CHECK
void* _mem_check_allocateMemory(size_t bytes, bool zeroFill, const TCHAR* _file, size_t _line)
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
    _mem_check_debugHeap(heap, true, _file, _line);
#endif
    return heap;
}

#if MEM_CHECK
void* _mem_check_allocateClearMemory(size_t count, size_t typeSize, const TCHAR* _file, size_t _line)
{
    return _mem_check_allocateMemory(count * typeSize, true, _file, _line);
}
#else
void* allocateClearMemory(size_t count, size_t typeSize)
{
    return allocateMemory(count * typeSize, true);
}
#endif

#if MEM_CHECK
void _mem_check_freeMemory(void* p, const TCHAR* _file, size_t _line)
#else
void freeMemory(void* p)
#endif
{
#if MEM_CHECK
    _mem_check_debugHeap(p, false, _file, _line);
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
