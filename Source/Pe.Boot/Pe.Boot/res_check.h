#pragma once
#include <stdbool.h>
#include <stddef.h>

#include <tchar.h>


#ifdef RES_CHECK

#define RES_CHECK_CALLER_FILE_PATH (260)

#define RES_CHECK_HEAP_ALLOC_STOCK_LENGTH (1024 * 4)
#define RES_CHECK_HEAP_PRINT_BUFFER_LENGTH (100 + (RES_CHECK_CALLER_FILE_PATH * 2))

#define RES_CHECK_ARG_FLIE caller_file
#define RES_CHECK_ARG_LINE caller_line
#define RES_CHECK_WRAP_ARGS _T(__FILE__), __LINE__
#define RES_CHECK_FUNC_ARGS const TCHAR* RES_CHECK_ARG_FLIE, size_t RES_CHECK_ARG_LINE
#define RES_CHECK_CALL_ARGS RES_CHECK_ARG_FLIE, RES_CHECK_ARG_LINE

typedef struct
{
    void* p;
    TCHAR file[RES_CHECK_CALLER_FILE_PATH];
    size_t line;
} rc_heap__ALLOC_STOCK_ITEM;

void rc_heap__check(void* p, bool allocate, RES_CHECK_FUNC_ARGS);

void rc_heap__print_allocate_memory(bool leak, void(*output)(TCHAR*));

#endif

#ifdef RES_CHECK
/// リソースチェック処理呼び出し切り替え処理
#   define RC_HEAP_CALL(function_name, ...) rc_heap__##function_name(__VA_ARGS__, RES_CHECK_CALL_ARGS)
#else
#   define RC_HEAP_CALL(function_name, ...) function_name(__VA_ARGS__)
#endif
