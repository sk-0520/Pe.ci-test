#pragma once
#include <stdbool.h>

#include <windows.h>

#include "text.h"

typedef enum tag_FILE_ENCODING
{
    FILE_ENCODING_NATIVE,
#ifdef _UNICODE
    FILE_ENCODING_UTF8,
    FILE_ENCODING_UTF16LE,
#endif
} FILE_ENCODING;

/// <summary>
/// パスはディレクトリか。
/// <para>パスが存在すること前提の処理。<c>exists_file_path</c>, <c>exists_directory_path</c>内部で使用されるためアプリケーション側であえて呼び出す必要なし。</para>
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
bool is_directory_path(const TEXT* path);

/// <summary>
/// ファイルが存在するか。
/// </summary>
/// <param name="path">ファイルパス。</param>
/// <returns></returns>
bool exists_file_path(const TEXT* path);

/// <summary>
/// ディレクトリが存在するか。
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
bool exists_directory_path(const TEXT* path);

// ファイル処理諸々
#include "fsio_resource.h"
#include "fsio_textfile.h"
