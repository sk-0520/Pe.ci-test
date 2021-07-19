#pragma once
#include <stdbool.h>

#include <windows.h>

#include "text.h"

/// <summary>
/// ファイルハンドル(ポインタ)ラッパー。
/// </summary>
typedef struct tag_FILE_POINTER
{
    /// <summary>
    /// ファイルパス。
    /// </summary>
    TEXT path;
    /// <summary>
    /// ファイルハンドル(ポインタ)。
    /// <c>NULL</c>の場合無効(その場合pathも無効)。
    /// </summary>
    HANDLE handle;
} FILE_POINTER;

/// <summary>
/// アクセスモード。
/// </summary>
typedef enum tag_FILE_ACCESS_MODE
{
    FILE_ACCESS_MODE_NONE = 0,
    /// <summary>
    /// 読み取りアクセス。
    /// </summary>
    FILE_ACCESS_MODE_READ = GENERIC_READ,
    /// <summary>
    /// 書き込みアクセス
    /// </summary>
    FILE_ACCESS_MODE_WRITE = GENERIC_WRITE,
} FILE_ACCESS_MODE;

/// <summary>
/// 共有方法。
/// </summary>
typedef enum tag_FILE_SHARE_MODE
{
    FILE_SHARE_MODE_NONE = 0,
    /// <summary>
    /// 削除を許可。
    /// </summary>
    FILE_SHARE_MODE_DELETE = FILE_SHARE_DELETE,
    /// <summary>
    /// 読み込みを許可。
    /// </summary>
    FILE_SHARE_MODE_READ = FILE_SHARE_READ,
    /// <summary>
    /// 書き込みを許可。
    /// </summary>
    FILE_SHARE_MODE_WRITE = FILE_SHARE_WRITE,
} FILE_SHARE_MODE;

typedef enum tag_FILE_OPEN_MODE
{
    /// <summary>
    /// ファイルを作成。
    /// 存在する場合は失敗する。
    /// </summary>
    FILE_OPEN_MODE_NEW = CREATE_NEW,
    /// <summary>
    /// ファイルを開く。
    /// 存在しない場合は作成する。
    /// </summary>
    FILE_OPEN_MODE_OPEN_OR_CREATE = OPEN_ALWAYS,
    /// <summary>
    /// ファイルを開く。
    /// 存在しない場合失敗する。
    /// </summary>
    FILE_OPEN_MODE_OPEN = OPEN_EXISTING,
    /// <summary>
    /// ファイルを開いてサイズを 0 バイトにする。
    /// 存在しない場合失敗する。
    /// </summary>
    FILE_OPEN_MODE_TRUNCATE = TRUNCATE_EXISTING,
} FILE_OPEN_MODE;

FILE_POINTER create_file(const TEXT* path);
FILE_POINTER open_file(const TEXT* path);
FILE_POINTER open_or_create_file(const TEXT* path);
bool close_file(FILE_POINTER* file);

/// <summary>
/// 指定された FILE_POINTER が有効か。
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
bool is_enabled_file(const FILE_POINTER* file);
