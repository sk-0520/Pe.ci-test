#ifdef RES_CHECK
#   include <tchar.h>
#endif

#include <windows.h>

#include "res_check.h"
#include "debug.h"
#include "common.h"

#ifdef RES_CHECK

#ifdef _WIN64
#   define FMT_D_ZU _T("%I64u")
#else
#   define FMT_D_ZU _T("%lu")
#endif

typedef enum
{
    RES_CHECK_TYPE_HEAP = 0,
    RES_CHECK_TYPE_FILE = 1,
} RES_CHECK_TYPE;

static const struct RES_CHECK_FORAMT
{
    const TCHAR* alloc_msg;
    const TCHAR* alloc_err;
    const TCHAR* free_mgs;
    const TCHAR* free_err;
    const TCHAR* stock_count;
    const TCHAR* stock_list;
    const TCHAR* stock_leak;
} res_check__formats[] = {
    {
        .alloc_msg = _T("[HEAP:+] %p (%s) %s:") FMT_D_ZU,
        .alloc_err = _T("[HEAP:STOCK:ERROR] %p (%s) %s") FMT_D_ZU,
        .free_mgs = _T("[HEAP:-] %p (%s) %s:") FMT_D_ZU _T(" - %s:") FMT_D_ZU,
        .free_err = _T("[HEAP:NOTFOUND:ERROR] %p (%s) %s:") FMT_D_ZU,
        .stock_count = _T("[HEAP:%s:COUNT] ") FMT_D_ZU,
        .stock_list = _T("[HEAP:STOCK] %p (%s) %s:") FMT_D_ZU,
        .stock_leak = _T("[HEAP:WARNING:LEAK] %p (%s) %s:") FMT_D_ZU,
    },
    {
        .alloc_msg = _T("[FILE:+] %p (%s) %s:") FMT_D_ZU _T(" -> %s"),
        .alloc_err = _T("[FILE:STOCK:ERROR] %p (%s) %s:") FMT_D_ZU,
        .free_mgs = _T("[FILE:-] %p (%s) %s:") FMT_D_ZU _T(" - %s:") FMT_D_ZU,
        .free_err = _T("[FILE:NOTFOUND:ERROR] %p (%s) %s:") FMT_D_ZU,
        .stock_count = _T("[FILE:%s:COUNT] ") FMT_D_ZU,
        .stock_list = _T("[FILE:STOCK] %p (%s) %s:") FMT_D_ZU,
        .stock_leak = _T("[FILE:WARNING:LEAK] %p (%s) %s:") FMT_D_ZU,
    },
};

typedef struct tag_RES_CHECK_ITEM
{
    RES_CHECK_STOCK_ITEM* stock_items;
    size_t stock_items_length;
    size_t* stock_item_count;
    const struct RES_CHECK_FORAMT* formats;
} RES_CHECK_ITEM;

static func_rc__output rc__output;
static size_t rc__path_length;
static size_t  rc__buffer_length;

static RES_CHECK_HEAP_STOCK_ITEM* rc_heap__stock_items;
static size_t rc_heap__stock_items_length;
static size_t rc_heap__stock_item_count;

static RES_CHECK_FILE_STOCK_ITEM* rc_file__stock_items;
static size_t rc_file__stock_items_length;
static size_t rc_file__stock_item_count;

static HANDLE rc__heap = NULL;

#pragma warning(push)
#pragma warning(disable:6386)
#define output_core(format, ...) do { \
    if(rc__output) { \
        TCHAR* rc__buffer = HeapAlloc(rc__heap, 0, rc__buffer_length * sizeof(TCHAR)); \
        assert(rc__buffer); \
        wsprintf(rc__buffer, /*rc__buffer_length - 1, */format, __VA_ARGS__); \
        rc__output(rc__buffer); \
        HeapFree(rc__heap, 0, rc__buffer); \
    } \
} while (0)
#pragma warning(push)

#ifdef RES_CHECK_NO_OUTPUT
#   define output(format, ...)
#else
#   define output(format, ...) output_core(format, __VA_ARGS__)
#endif



static RES_CHECK_ITEM rc_get_item(RES_CHECK_TYPE type)
{
    switch (type) {
        case RES_CHECK_TYPE_HEAP:
        {
            RES_CHECK_ITEM result = {
                .stock_items = rc_heap__stock_items,
                .stock_items_length = rc_heap__stock_items_length,
                .stock_item_count = &rc_heap__stock_item_count,
                .formats = res_check__formats + type,
            };
            return result;
        }

        case RES_CHECK_TYPE_FILE:
        {
            RES_CHECK_ITEM result = {
                .stock_items = rc_file__stock_items,
                .stock_items_length = rc_file__stock_items_length,
                .stock_item_count = &rc_file__stock_item_count,
                .formats = res_check__formats + type,
            };
            return result;
        }

        default:
            assert(false);
    }

    RES_CHECK_ITEM none = {
        .stock_items = NULL,
        .stock_items_length = 0,
        .stock_item_count = 0,
        .formats = 0,
    };
    return none;
}

static void rc_check_core(void* p, const void* data, bool allocate, RES_CHECK_TYPE type, RES_CHECK_FUNC_ARGS)
{
    RES_CHECK_ITEM rc_item = rc_get_item(type);

    if (allocate) {
        bool stocked = false;
        for (size_t i = 0; i < rc_item.stock_items_length; i++) {
            if (!rc_item.stock_items[i].p) {
                if (type == RES_CHECK_TYPE_FILE) {
                    output(rc_item.formats->alloc_msg, p, RES_CHECK_CALL_ARGS, (TCHAR*)data);
                } else {
                    output(rc_item.formats->alloc_msg, p, RES_CHECK_CALL_ARGS);
                }

                RES_CHECK_STOCK_ITEM item = {
                    .p = p,
                    .line = RES_CHECK_ARG_LINE,
                };
                item.file = (TCHAR*)HeapAlloc(rc__heap, HEAP_ZERO_MEMORY, rc__path_length * sizeof(TCHAR));
#pragma warning(push)
#pragma warning(disable:6387)
                lstrcpy(item.file, RES_CHECK_ARG_FLIE); // tstring.h を取り込みたくないのでAPIを直接呼び出し
#pragma warning(pop)

                rc_item.stock_items[i] = item;
                *rc_item.stock_item_count = *rc_item.stock_item_count + 1;
                stocked = true;
                break;
            }
        }

        if (!stocked) {
            output(rc_item.formats->alloc_err, p, RES_CHECK_CALL_ARGS);
        }
    } else {
        bool exists = false;
        for (size_t i = 0; p && i < rc_item.stock_items_length; i++) {
            if (rc_item.stock_items[i].p == p) {
                output(rc_item.formats->free_mgs, p, RES_CHECK_CALL_ARGS, rc_item.stock_items[i].file, rc_item.stock_items[i].line);
                //HeapFree(rc__heap, 0, rc_item.stock_items->file);

                *rc_item.stock_item_count = *rc_item.stock_item_count - 1;
                memset(&rc_item.stock_items[i], 0, sizeof(RES_CHECK_STOCK_ITEM));
                exists = true;
                break;
            }
        }

        if (!exists) {
            output(rc_item.formats->free_err, p, RES_CHECK_CALL_ARGS);
        }
    }
}

void rc__heap_check(void* p, bool allocate, RES_CHECK_FUNC_ARGS)
{
    assert(p);
    rc_check_core(p, NULL, allocate, RES_CHECK_TYPE_HEAP, RES_CHECK_CALL_ARGS);
}

void rc__file_check(void* p, const TCHAR* path, bool allocate, RES_CHECK_FUNC_ARGS)
{
    assert(p);
    rc_check_core(p, path, allocate, RES_CHECK_TYPE_FILE, RES_CHECK_CALL_ARGS);
}

static void rc__print_core(bool leak, RES_CHECK_TYPE type)
{
    RES_CHECK_ITEM rc_item = rc_get_item(type);

    output_core(rc_item.formats->stock_count, *rc_item.stock_item_count && leak ? _T("ERROR") : _T("INFORMATION"), *rc_item.stock_item_count);

    for (size_t i = 0; i < rc_item.stock_items_length; i++) {
        RES_CHECK_STOCK_ITEM* item = rc_item.stock_items + i;
        if (item->p) {
            const TCHAR* format = leak
                ? res_check__formats->stock_list
                : res_check__formats->stock_leak
                ;
            output_core(format, item->p, item->file, item->line);
        }
    }
}

void rc__print(bool leak)
{
    for (size_t i = 0; i < SIZEOF_ARRAY(res_check__formats); i++) {
        rc__print_core(leak, i);
    }
}

bool rc__exists_resource_leak()
{
    return rc_heap__stock_item_count || rc_file__stock_item_count;
}

void rc__initialize(func_rc__output output, size_t path_length, size_t buffer_length, size_t heap_count, size_t file_count)
{
    rc__heap = HeapCreate(0, 0, 0);
    assert(rc__heap);

    rc__output = output;
    rc__path_length = path_length;
    rc__buffer_length = buffer_length;

    rc_heap__stock_items = HeapAlloc(rc__heap, HEAP_ZERO_MEMORY, heap_count * sizeof(RES_CHECK_HEAP_STOCK_ITEM));
    rc_heap__stock_items_length = heap_count;
    rc_heap__stock_item_count = 0;

    rc_file__stock_items = HeapAlloc(rc__heap, HEAP_ZERO_MEMORY, file_count * sizeof(RES_CHECK_FILE_STOCK_ITEM));
    rc_file__stock_items_length = file_count;
    rc_file__stock_item_count = 0;
}

void rc__uninitialize(void)
{
    HeapFree(rc__heap, 0, rc_heap__stock_items);
    HeapFree(rc__heap, 0, rc_file__stock_items);
    HeapDestroy(rc__heap);
}


#endif
