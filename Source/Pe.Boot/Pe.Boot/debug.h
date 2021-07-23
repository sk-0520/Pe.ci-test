#pragma once
#include <windows.h>
#include <assert.h>

/// <summary>
/// デバッグ時のみ使用可能なログ出力。
///
/// 内部的には<c>OutputDebugString</c>だが<c>DEBUG</c>定義がある場合にのみ使用される。
/// </summary>
/// <param name="s"></param>
void output_debug(const TCHAR * s);
#define debug(s) output_debug(_T(s))

#ifdef NDEBUG
#   define assert_debug(expr)
#else
    void assert_debug_impl(const TCHAR* message, const TCHAR* expression, const TCHAR* caller_file, size_t caller_line);
#   define assert_debug(expr) \
do { \
    if (!(expr)) { \
        assert_debug_impl(_T("ASSERT: ") _T(#expr) _T(" -> ") _T(__FILE__) _T(":") _T(TO_STRING(__LINE__)), _T(#expr), _T(__FILE__), __LINE__); \
    } \
} while(0)
#endif

