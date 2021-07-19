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
#ifdef MEM_CHECK
TEXT mem_check__combine_path(const TEXT* base_path, const TEXT* relative_path, MEM_CHECK_HEAD_ARGS);
#   define combine_path(base_path, relative_path) mem_check__combine_path((base_path), (relative_path), MEM_CHECK_HEAD_DEF)
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
#ifdef MEM_CHECK
TEXT mem_check__join_path(const TEXT* base_path, const TEXT_LIST paths, size_t count, MEM_CHECK_HEAD_ARGS);
#   define join_path(base_path, paths, count) mem_check__join_path((base_path), (paths), (count), MEM_CHECK_HEAD_DEF)
#else
TEXT join_path(const TEXT* base_path, const TEXT_LIST paths, size_t count);
#endif

/// <summary>
/// パスの正規化。
/// </summary>
/// <param name="path"></param>
/// <returns>正規化されたパス。</returns>
#ifdef MEM_CHECK
TEXT mem_check__canonicalize_path(const TEXT* path, MEM_CHECK_HEAD_ARGS);
#   define canonicalize_path(path) mem_check__canonicalize_path(path, MEM_CHECK_HEAD_DEF)
#else
TEXT canonicalize_path(const TEXT* path);
#endif

/// <summary>
/// 実行中モジュールパスの取得
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <returns></returns>
TEXT get_module_path(HINSTANCE hInstance);

