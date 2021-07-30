#pragma once
#include <tchar.h>
#include <shlwapi.h>

#include "text.h"


/// <summary>
/// パスから親ディレクトリパスを取得。
/// </summary>
/// <param name="path">対象パス。</param>
/// <returns>親ディレクトリパス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(get_parent_directory_path, const TEXT* path);
#ifdef RES_CHECK
#   define get_parent_directory_path(path) RC_HEAP_WRAP(get_parent_directory_path, (path))
#endif

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="relative_path">結合するパス。</param>
/// <returns>結合パス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(combine_path, const TEXT* base_path, const TEXT* relative_path);
#ifdef RES_CHECK
#   define combine_path(base_path, relative_path) RC_HEAP_WRAP(combine_path, (base_path), (relative_path))
#endif

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="paths">結合するパス。</param>
/// <param name="count">結合するパスの個数。</param>
/// <returns>結合パス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(join_path, const TEXT* base_path, const TEXT_LIST paths, size_t count);
#ifdef RES_CHECK
#   define join_path(base_path, paths, count) RC_HEAP_WRAP(join_path, (base_path), (paths), (count))
#endif

/// <summary>
/// パスの正規化。
/// </summary>
/// <param name="path"></param>
/// <returns>正規化されたパス。</returns>
TEXT RC_HEAP_FUNC(canonicalize_path, const TEXT* path);
#ifdef RES_CHECK
#   define canonicalize_path(path) RC_HEAP_WRAP(canonicalize_path, (path))
#endif

/// <summary>
/// 実行中モジュールパスの取得
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <returns></returns>
TEXT RC_HEAP_FUNC(get_module_path, HINSTANCE hInstance);
#ifdef RES_CHECK
#   define get_module_path(hInstance) RC_HEAP_WRAP(get_module_path, (hInstance))
#endif

