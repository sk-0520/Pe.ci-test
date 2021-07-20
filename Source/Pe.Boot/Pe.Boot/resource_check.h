#pragma once
#include <stdbool.h>
#include <stddef.h>

#include <tchar.h>


#ifdef RES_CHECK

#define MEM_CHECK_CALLER_FILE_PATH 260

#define MEM_CHECK_ALLOC_STOCK_LENGTH (1024 * 4)
#define MEM_CHECK_PRINT_BUFFER_LENGTH (100 + (MEM_CHECK_CALLER_FILE_PATH * 2))

#define MEM_CHECK_ARG_FLIE caller_file
#define MEM_CHECK_ARG_LINE caller_line
#define MEM_CHECK_WRAP_ARGS _T(__FILE__), __LINE__
#define MEM_CHECK_FUNC_ARGS const TCHAR* MEM_CHECK_ARG_FLIE, size_t MEM_CHECK_ARG_LINE
#define MEM_CHECK_CALL_ARGS MEM_CHECK_ARG_FLIE, MEM_CHECK_ARG_LINE

typedef struct
{
    void* p;
    TCHAR file[MEM_CHECK_CALLER_FILE_PATH];
    size_t line;
} mem_check__ALLOC_STOCK_ITEM;

void mem_check__debugHeap(void* p, bool allocate, MEM_CHECK_FUNC_ARGS);

void mem_check__print_allocate_memory(bool leak, void(*output)(TCHAR*), bool add_new_line);

#endif

#ifdef RES_CHECK
/// リソースチェック処理呼び出し切り替え処理
#   define MC_CALL(function_name, ...) mem_check__##function_name(__VA_ARGS__, MEM_CHECK_CALL_ARGS)
#else
#   define MC_CALL(function_name, ...) function_name(__VA_ARGS__)
#endif
