#pragma once
#include <stdbool.h>

#include <Windows.h>

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
/// 指定パスはディレクトリか。
/// </summary>
/// <param name="path">パス。</param>
/// <returns>ディレクトリとして存在する場合に真。</returns>
bool exists_directory_fsio(const TEXT* path);

/// <summary>
/// 指定パスはファイルか。
/// </summary>
/// <param name="path">パス。</param>
/// <returns>ファイルとして存在する場合に真。</returns>
bool exists_file_fsio(const TEXT* path);

/// <summary>
/// パスが存在するか。
/// <para>それがファイル・ディレクトリを問わない。</para>
/// </summary>
/// <param name="path"></param>
/// <returns>存在する場合に真。</returns>
bool exists_fsio(const TEXT* path);

/// <summary>
/// ディレクトリを生成。
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
bool create_directory_fsio(const TEXT* path);

// ファイル処理諸々
#include "fsio.z.resource.h"
#include "fsio.z.textfile.h"
