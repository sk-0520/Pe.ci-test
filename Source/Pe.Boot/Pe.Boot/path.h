#pragma once
#include <tchar.h>
#include <shlwapi.h>

#include "text.h"


/// <summary>
/// パスから親ディレクトリパスを取得。
/// </summary>
/// <param name="path">対象パス。</param>
/// <returns>親ディレクトリパス。</returns>
TEXT get_parent_directory_path(const TEXT* path);

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="relative_path">結合するパス。</param>
/// <returns>結合パス。解放が必要。</returns>
#ifdef RES_CHECK
TEXT rc_heap__combine_path(const TEXT* base_path, const TEXT* relative_path, RES_CHECK_FUNC_ARGS);
#   define combine_path(base_path, relative_path) rc_heap__combine_path((base_path), (relative_path), RES_CHECK_WRAP_ARGS)
#else
TEXT combine_path(const TEXT* base_path, const TEXT* relative_path);
#endif

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="paths">結合するパス。</param>
/// <param name="count">結合するパスの個数。</param>
/// <returns>結合パス。解放が必要。</returns>
#ifdef RES_CHECK
TEXT rc_heap__join_path(const TEXT* base_path, const TEXT_LIST paths, size_t count, RES_CHECK_FUNC_ARGS);
#   define join_path(base_path, paths, count) rc_heap__join_path((base_path), (paths), (count), RES_CHECK_WRAP_ARGS)
#else
TEXT join_path(const TEXT* base_path, const TEXT_LIST paths, size_t count);
#endif

/// <summary>
/// パスの正規化。
/// </summary>
/// <param name="path"></param>
/// <returns>正規化されたパス。</returns>
#ifdef RES_CHECK
TEXT rc_heap__canonicalize_path(const TEXT* path, RES_CHECK_FUNC_ARGS);
#   define canonicalize_path(path) rc_heap__canonicalize_path(path, RES_CHECK_WRAP_ARGS)
#else
TEXT canonicalize_path(const TEXT* path);
#endif

/// <summary>
/// 実行中モジュールパスの取得
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <returns></returns>
TEXT get_module_path(HINSTANCE hInstance);

