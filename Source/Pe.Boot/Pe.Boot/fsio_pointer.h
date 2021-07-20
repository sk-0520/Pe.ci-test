#pragma once
#include "fsio.h"

/// <summary>
/// ファイルポインタ(ハンドル)ラッパー。
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

/// <summary>
/// ファイルを新規作成。
/// <para>既にファイルが存在する場合は失敗する。</para>
/// </summary>
/// <param name="path">作成するファイルパス。</param>
/// <returns>作成したファイルポインタ。成功状態は<c>is_enabled_file</c>で確認する。解放が必要。</returns>
FILE_POINTER create_file(const TEXT* path);
/// <summary>
/// 既存ファイルを開く。
/// <para>ファイルが存在しない場合は失敗する。</para>
/// </summary>
/// <param name="path">開くファイルパス。</param>
/// <returns>開いたファイルポインタ。成功状態は<c>is_enabled_file</c>で確認する。解放が必要。</returns>
FILE_POINTER open_file(const TEXT* path);
/// <summary>
/// ファイルが存在すれば開き、存在しない場合は作成する。
/// </summary>
/// <param name="path">ファイルパス。</param>
/// <returns>ファイルポインタ。成功状態は<c>is_enabled_file</c>で確認する。解放が必要。</returns>
FILE_POINTER open_or_create_file(const TEXT* path);
/// <summary>
/// ファイルを閉じる。
/// </summary>
/// <param name="file">対象ファイルポインタ。</param>
/// <returns>成功状態。</returns>
bool close_file(FILE_POINTER* file);

/// <summary>
/// 指定されたファイルポインタが有効か。
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
bool is_enabled_file(const FILE_POINTER* file);

// 64bit値をいい感じに使うのがめんどいので頭かケツにしか移動できませーん
/// <summary>
/// ファイルポインタの現在地を先頭に移動。
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
bool seek_begin_file_pointer(const FILE_POINTER* file);
/// <summary>
/// ファイルポインタの現在地を終端に移動。
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
bool seek_end_file_pointer(const FILE_POINTER* file);
