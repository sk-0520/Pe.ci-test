#pragma once
#include <tchar.h>
#include <shlwapi.h>

#include "text.h"

/// ディレクトリの区切り文字
#define DIRECTORY_SEPARATOR_CHARACTER _T('\\')
/// ディレクトリの代替区切り文字
#define ALT_DIRECTORY_SEPARATOR_CHARACTER _T('/')

/// <summary>
/// ディレクトリ区切りか。
/// </summary>
/// <param name="c"></param>
/// <returns>ディレクトリ区切り文字の場合に真。</returns>
bool is_directory_separator(TCHAR c);

/// <summary>
/// ルートが含まれているか。
/// <para>Windowsの絶対パス難しいから適当にやる</para>
/// </summary>
/// <param name="text"></param>
/// <returns>ルートが含まれている場合に真。</returns>
bool has_root_path(const TEXT* text);

/// <summary>
/// パスから親ディレクトリパスを取得。
/// </summary>
/// <param name="path">対象パス。</param>
/// <returns>親ディレクトリパス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(get_parent_directory_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define get_parent_directory_path(path, memory_resource) RC_HEAP_WRAP(get_parent_directory_path, (path), memory_resource)
#endif

/// <summary>
/// パスをディレクトリ区切りで分割。
/// </summary>
/// <param name="path"></param>
/// <param name="memory_resource"></param>
/// <returns>分割後ディレクトリ・ファイル名のリスト。解放が必要。</returns>
OBJECT_LIST RC_HEAP_FUNC(split_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define split_path(path, memory_resource) RC_HEAP_WRAP(split_path, (path), memory_resource)
#endif

/// <summary>
/// パスの正規化。
/// </summary>
/// <param name="path"></param>
/// <param name="memory_resource"></param>
/// <returns>正規化されたパス。</returns>
TEXT RC_HEAP_FUNC(canonicalize_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define canonicalize_path(path, memory_resource) RC_HEAP_WRAP(canonicalize_path, (path), memory_resource)
#endif

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="relative_path">結合するパス。</param>
/// <returns>結合パス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(combine_path, const TEXT* base_path, const TEXT* relative_path, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define combine_path(base_path, relative_path, memory_resource) RC_HEAP_WRAP(combine_path, (base_path), (relative_path), memory_resource)
#endif

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="base_path">ベースのパス。</param>
/// <param name="paths">結合するパス。</param>
/// <param name="count">結合するパスの個数。</param>
/// <returns>結合パス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(join_path, const TEXT* base_path, const TEXT_LIST paths, size_t count, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define join_path(base_path, paths, count, memory_resource) RC_HEAP_WRAP(join_path, (base_path), (paths), (count), memory_resource)
#endif

/// <summary>
/// 実行中モジュールパスの取得
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <returns>実行中モジュールパス。解放が必要。</returns>
TEXT RC_HEAP_FUNC(get_module_path, HINSTANCE hInstance, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define get_module_path(hInstance, memory_resource) RC_HEAP_WRAP(get_module_path, (hInstance), memory_resource)
#endif

