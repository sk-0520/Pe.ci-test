#pragma once

#ifdef GLOBAL
#   define GLOBAL extern
#else
#   define GLOBAL
#endif

/// <summary>
/// 実行モード。
/// </summary>
typedef enum _TAG_EXECUTE_MODE
{
    /// <summary>
    /// 通常起動。
    /// </summary>
    EXECUTE_MODE_BOOT
} EXECUTE_MODE;
