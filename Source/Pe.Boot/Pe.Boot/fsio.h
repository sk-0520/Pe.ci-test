#pragma once
#include <stdbool.h>

#include <windows.h>

#include "text.h"

/// <summary>
/// パスはディレクトリか。
/// <para>パスが存在すること前提の処理。<c>exists_file</c>, <c>exists_directory</c>内部で使用されるためアプリケーション側であえて呼び出す必要なし。</para>
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
bool is_directory(const TEXT* path);

/// <summary>
/// ファイルが存在するか。
/// </summary>
/// <param name="path">ファイルパス。</param>
/// <returns></returns>
bool exists_file(const TEXT* path);

/// <summary>
/// ディレクトリが存在するか。
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
bool exists_directory(const TEXT* path);

// ファイル処理諸々
#include "fsio_pointer.h"
