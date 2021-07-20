#ifdef RES_CHECK
#   include <stdio.h>
#   include <tchar.h>
#endif

#include <windows.h>

#include "res_check.h"
#include "common.h"

#ifdef RES_CHECK

static rc_heap__ALLOC_STOCK_ITEM rc_heap__allocStocks[RES_CHECK_HEAP_ALLOC_STOCK_LENGTH] = { 0 };
static size_t rc_heap__allocStocksCount = 0;

#define rc_heap__debug_output(format, ...) do { \
    TCHAR rc_heap__debug_buffer[RES_CHECK_HEAP_PRINT_BUFFER_LENGTH] = { 0 }; \
    size_t rc_heap__debug_buffer_length = (sizeof(rc_heap__debug_buffer) - 1) / sizeof(TCHAR); \
    swprintf(rc_heap__debug_buffer, rc_heap__debug_buffer_length, _T(format) NEWLINET, __VA_ARGS__); \
    OutputDebugString(rc_heap__debug_buffer); \
} while (0)

void rc_heap__check(void* p, bool allocate, RES_CHECK_FUNC_ARGS)
{
    if (allocate) {
        bool stocked = false;
        for (size_t i = 0; i < RES_CHECK_HEAP_ALLOC_STOCK_LENGTH; i++) {
            if (!rc_heap__allocStocks[i].p) {
                rc_heap__debug_output("[HEAP:+] %p %s(%zu)", p, caller_file, caller_line);

                rc_heap__ALLOC_STOCK_ITEM data = {
                    .p = p,
                    .line = caller_line,
                };
                data.file[0] = 0;
                lstrcpy(data.file, caller_file); // tstring.h を取り込みたくないのでAPIを直接呼び出し

                rc_heap__allocStocks[i] = data;
                rc_heap__allocStocksCount += 1;
                stocked = true;
                break;
            }
        }

        if (!stocked) {
            rc_heap__debug_output("[HEAP:STOCK:ERROR] %p %s(%zu)", p, caller_file, caller_line);
        }
    } else {
        bool exists = false;;
        for (size_t i = 0; p && i < RES_CHECK_HEAP_ALLOC_STOCK_LENGTH; i++) {
            if (rc_heap__allocStocks[i].p == p) {
                rc_heap__debug_output("[HEAP:-] %p %s(%zu) - %s(%zu)", p, caller_file, caller_line, rc_heap__allocStocks[i].file, rc_heap__allocStocks[i].line);

                rc_heap__allocStocksCount -= 1;
                memset(&rc_heap__allocStocks[i], 0, sizeof(rc_heap__ALLOC_STOCK_ITEM));
                exists = true;
                break;
            }
        }

        if (!exists) {
            rc_heap__debug_output("[HEAP:NOTFOUND:ERROR] %p %s(%zu)", p, caller_file, caller_line);
        }
    }
}

void rc_heap__print_allocate_memory(bool leak, void(*output)(TCHAR*))
{
    TCHAR head[RES_CHECK_HEAP_PRINT_BUFFER_LENGTH];
    swprintf(head, (sizeof(head) - 1) / sizeof(TCHAR), _T("[HEAP:%s:COUNT] %zu"), rc_heap__allocStocksCount && leak ? _T("ERROR") : _T("INFORMATION"), rc_heap__allocStocksCount);
    output(head);

    for (size_t i = 0; i < RES_CHECK_HEAP_ALLOC_STOCK_LENGTH; i++) {
        rc_heap__ALLOC_STOCK_ITEM item = rc_heap__allocStocks[i];
        if (item.p) {
            TCHAR body[RES_CHECK_HEAP_PRINT_BUFFER_LENGTH];
            TCHAR* format = leak
                ? _T("[HEAP:WARNING:LEAK] %p %s(%zu)")
                : _T("[HEAP:STOCK] %p %s(%zu)")
                ;
            swprintf(body, (sizeof(body) - 1) / sizeof(TCHAR), format, item.p, item.file, item.line);
            output(body);
        }
    }
}

#endif
