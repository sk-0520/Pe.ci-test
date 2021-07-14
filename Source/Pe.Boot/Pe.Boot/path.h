#pragma once
#include <tchar.h>
#include <shlwapi.h>

#include "text.h"


/// <summary>
/// パスから親ディレクトリパスを取得。
/// </summary>
/// <param name="path">対象パス。</param>
/// <returns>親ディレクトリパス。</returns>
TEXT getParentDirectoryPath2(const TEXT* path);

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="basePath">ベースのパス。</param>
/// <param name="relativePath">結合するパス。</param>
/// <returns>結合パス。</returns>
TEXT combinePath2(const TEXT* basePath, const TEXT* relativePath);

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="basePath">ベースのパス。</param>
/// <param name="paths">結合するパス。</param>
/// <param name="pathsLength">結合するパスの個数。</param>
/// <returns>結合パス。</returns>
TEXT joinPath(const TEXT* basePath, const TEXT paths[], size_t pathsLength);

/// <summary>
/// パスの正規化。
/// </summary>
/// <param name="path"></param>
/// <returns>正規化されたパス。</returns>
TEXT canonicalizePath(const TEXT* path);

/// <summary>
/// 実行中モジュールパスの取得
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <returns></returns>
TEXT getModulePath(HINSTANCE hInstance);

