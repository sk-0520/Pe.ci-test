#pragma once
#include <stdbool.h>

#include <windows.h>

#include "text.h"

/// <summary>
/// ファイルのエンコーディング。
/// </summary>
typedef enum tag_FILE_ENCODING
{
    /// <summary>
    /// ソース内文字コードをそのまま使用。
    /// <para>原則使用しない。(非 _UNICODE コンパイルとか知らんけど)</para>
    /// </summary>
    FILE_ENCODING_NATIVE,
#ifdef _UNICODE
    /// <summary>
    /// UTF8。
    /// </summary>
    FILE_ENCODING_UTF8,
    /// <summary>
    /// UTF-16 LE。
    /// </summary>
    FILE_ENCODING_UTF16LE,
#endif
} FILE_ENCODING;

/// <summary>
/// パスはディレクトリか。
/// <para>パスが存在すること前提の処理。<see cref="exists_file_path" />, <see cref="exists_directory_path" />内部で使用されるためアプリケーション側であえて呼び出す必要なし。</para>
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
