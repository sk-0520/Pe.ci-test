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
/// <returns>結合パス。</returns>
TEXT combine_path(const TEXT* base_path, const TEXT* relative_path);

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="paths">結合するパス。</param>
/// <param name="count">結合するパスの個数。</param>
/// <returns>結合パス。</returns>
TEXT join_path(const TEXT* base_path, const TEXT paths[], size_t count);

/// <summary>
/// パスの正規化。
/// </summary>
/// <param name="path"></param>
/// <returns>正規化されたパス。</returns>
TEXT canonicalize_path(const TEXT* path);

/// <summary>
/// 実行中モジュールパスの取得
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <returns></returns>
TEXT get_module_path(HINSTANCE hInstance);

