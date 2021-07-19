#ifdef MEM_CHECK
#   include <stdio.h>
#   include <tchar.h>
#endif

#include <windows.h>

#include "common.h"
#include "memory.h"

#ifdef MEM_CHECK

static mem_check__ALLOC_STOCK_ITEM mem_check__allocStocks[MEM_CHECK_ALLOC_STOCK_LENGTH] = { 0 };
static size_t mem_check__allocStocksCount = 0;

#define mem_check__debug_output(format, ...) do { \
    TCHAR mem_check__debug_buffer[MEM_CHECK_PRINT_BUFFER_LENGTH] = { 0 }; \
    size_t mem_check__debug_buffer_length = (sizeof(mem_check__debug_buffer) - 1) / sizeof(TCHAR); \
    swprintf(mem_check__debug_buffer, mem_check__debug_buffer_length, _T(format) NEWLINET, __VA_ARGS__); \
    OutputDebugString(mem_check__debug_buffer); \
} while (0)

static void mem_check__debugHeap(void* p, bool allocate, const TCHAR * caller_file, size_t caller_line)
{
    if (allocate) {
        bool stocked = false;
        for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
            if (!mem_check__allocStocks[i].p) {
                mem_check__debug_output("[MEM:+] %p %s(%zu)", p, caller_file, caller_line);

                mem_check__ALLOC_STOCK_ITEM data = {
                    .p = p,
                    .line = caller_line,
                };
                data.file[0] = 0;
                lstrcpy(data.file, caller_file);

                mem_check__allocStocks[i] = data;
                mem_check__allocStocksCount += 1;
                stocked = true;
                break;
            }
        }

        if (!stocked) {
            mem_check__debug_output("[MEM:STOCK:ERROR] %p %s(%zu)", p, caller_file, caller_line);
        }
    } else {
        bool exists = false;;
        for (size_t i = 0; p && i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
            if (mem_check__allocStocks[i].p == p) {
                mem_check__debug_output("[MEM:-] %p %s(%zu) - %s(%zu)", p, caller_file, caller_line, mem_check__allocStocks[i].file, mem_check__allocStocks[i].line);

                mem_check__allocStocksCount -= 1;
                memset(&mem_check__allocStocks[i], 0, sizeof(mem_check__ALLOC_STOCK_ITEM));
                exists = true;
                break;
            }
        }

        if (!exists) {
            mem_check__debug_output("[MEM:NOTFOUND:ERROR] %p %s(%zu)", p, caller_file, caller_line);
        }
    }
}

void mem_check__print_allocate_memory(bool leak, void(*output)(TCHAR*), bool add_new_line)
{
    TCHAR head[MEM_CHECK_PRINT_BUFFER_LENGTH];
    swprintf(head, (sizeof(head) - 1) / sizeof(TCHAR), _T("[MEM:%s:COUNT] %zu%s"), mem_check__allocStocksCount && leak ? _T("ERROR") : _T("INFORMATION"), mem_check__allocStocksCount, add_new_line ? NEWLINET: _T(""));
    output(head);

    for (size_t i = 0; i < MEM_CHECK_ALLOC_STOCK_LENGTH; i++) {
        mem_check__ALLOC_STOCK_ITEM item = mem_check__allocStocks[i];
        if (item.p) {
            TCHAR body[MEM_CHECK_PRINT_BUFFER_LENGTH];
            TCHAR* format = leak
                ? _T("[MEM:WARNING:LEAK] %p %s(%zu)%s")
                : _T("[MEM:STOCK] %p %s(%zu)%s")
                ;
            swprintf(body, (sizeof(body) - 1) / sizeof(TCHAR), format, item.p, item.file, item.line, add_new_line ? NEWLINET : _T(""));
            output(body);
        }
    }
}

#endif

#ifdef MEM_CHECK
void* mem_check__allocate_memory(size_t bytes, bool zero_fill, const TCHAR * caller_file, size_t caller_line)
#else
void* allocate_memory(size_t bytes, bool zero_fill)
#endif
{
    HANDLE hHeap = GetProcessHeap();
    if (!hHeap) {
        return NULL;
    }

    void* heap = HeapAlloc(hHeap, zero_fill ? HEAP_ZERO_MEMORY : 0, bytes);
#ifdef MEM_CHECK
    mem_check__debugHeap(heap, true, caller_file, caller_line);
#endif
    return heap;
}

#ifdef MEM_CHECK
void* mem_check__allocate_clear_memory(size_t count, size_t type_size, const TCHAR * caller_file, size_t caller_line)
{
    return mem_check__allocate_memory(count * type_size, true, caller_file, caller_line);
}
#else
void* allocate_clear_memory(size_t count, size_t type_size)
{
    return allocate_memory(count * type_size, true);
}
#endif

#ifdef MEM_CHECK
void mem_check__free_memory(void* p, const TCHAR * caller_file, size_t caller_line)
#else
void free_memory(void* p)
#endif
{
#ifdef MEM_CHECK
    mem_check__debugHeap(p, false, caller_file, caller_line);
#endif

    HeapFree(GetProcessHeap(), 0, p);
}

void* set_memory(void* target, unsigned char value, size_t bytes)
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

void* copy_memory(void* destination, const void* source, size_t bytes)
{
    //NOTE: CRT!
    return CopyMemory(destination, source, bytes);
}

void* move_memory(void* destination, const void* source, size_t bytes)
{
    //NOTE: CRT!
    return MoveMemory(destination, source, bytes);
}

int compare_memory(const void* a, const void* b, size_t bytes)
{
    //NOTE: CRT!
    return memcmp(a, b, bytes);
}
