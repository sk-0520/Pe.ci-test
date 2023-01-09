#pragma once
#include <stdint.h>

#include "common.h"
#include "fsio.h"

/// <summary>
/// ファイルリソース。
/// </summary>
typedef struct tag_FILE_RESOURCE
{
    /// <summary>
    /// ファイルパス。
    /// <para>ファイルリソース生成時に複製されるので渡す側は気にする必要なし。</para>
    /// </summary>
    TEXT path;
    /// <summary>
    /// ファイルハンドル(ポインタ)。
    /// <c>NULL</c>の場合無効(その場合<see cref="path"/>も無効)。
    /// </summary>
    HANDLE handle;
    struct
    {
        const MEMORY_ARENA_RESOURCE* memory_arena_resource;
    } library;
} FILE_RESOURCE;

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

/// <summary>
/// 開き方。
/// </summary>
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
/// 無効ファイルリソースの生成。
/// </summary>
/// <returns></returns>
FILE_RESOURCE create_invalid_file_resource(void);

/// <summary>
/// ファイルリソースの生成。
/// </summary>
/// <param name="path">テキストは複製されるため呼び出し側で任意破棄可能。</param>
/// <param name="access_mode">アクセスモード。</param>
/// <param name="shared_mode">共有方法。</param>
/// <param name="open_mode">開き方。</param>
/// <param name="attributes">ここだけWindowsAPIでファイル属性。</param>
/// <param name="memory_arena_resource"></param>
/// <returns></returns>
FILE_RESOURCE RC_FILE_FUNC(new_file_resource, const TEXT* path, FILE_ACCESS_MODE access_mode, FILE_SHARE_MODE shared_mode, FILE_OPEN_MODE open_mode, DWORD attributes, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define new_file_resource(path, access_mode, shared_mode, open_mode, attributes, memory_arena_resource) RC_FILE_WRAP(new_file_resource, (path), (access_mode), (shared_mode), (open_mode), (attributes), memory_arena_resource)
#endif

/// <summary>
/// ファイルを新規作成。
/// <para>既にファイルが存在する場合は失敗する。</para>
/// </summary>
/// <param name="path">作成するファイルパス。テキストは複製されるため呼び出し側で任意破棄可能。</param>
/// <returns>作成したファイルリソース。成功状態は<c>is_enabled_file</c>で確認する。解放が必要。</returns>
FILE_RESOURCE RC_FILE_FUNC(create_file_resource, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define create_file_resource(path, memory_arena_resource) RC_FILE_WRAP(create_file_resource, path, memory_arena_resource)
#endif

/// <summary>
/// 既存ファイルを開く。
/// <para>ファイルが存在しない場合は失敗する。</para>
/// </summary>
/// <param name="path">開くファイルパス。テキストは複製されるため呼び出し側で任意破棄可能。</param>
/// <returns>開いたファイルリソース。成功状態は<c>is_enabled_file</c>で確認する。解放が必要。</returns>
FILE_RESOURCE RC_FILE_FUNC(open_file_resource, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define open_file_resource(path, memory_arena_resource) RC_FILE_WRAP(open_file_resource, path, memory_arena_resource)
#endif

/// <summary>
/// ファイルが存在すれば開き、存在しない場合は作成する。
/// </summary>
/// <param name="path">ファイルパス。テキストは複製されるため呼び出し側で任意破棄可能。</param>
/// <returns>ファイルリソース。成功状態は<c>is_enabled_file</c>で確認する。解放が必要。</returns>
FILE_RESOURCE RC_FILE_FUNC(open_or_create_file_resource, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define open_or_create_file_resource(path, memory_arena_resource) RC_FILE_WRAP(open_or_create_file_resource, path, memory_arena_resource)
#endif

/// <summary>
/// ファイルを閉じる。
/// </summary>
/// <param name="file">対象ファイルリソース。</param>
/// <returns>成功状態。</returns>
bool RC_FILE_FUNC(release_file_resource, FILE_RESOURCE* file_resource);
#if RES_CHECK
#   define release_file_resource(file_resource) RC_FILE_WRAP(release_file_resource, (file_resource))
#endif

/// <summary>
/// 指定されたファイルリソースが有効か。
/// </summary>
/// <param name="file_resource"></param>
/// <returns></returns>
bool is_enabled_file_resource(const FILE_RESOURCE* file_resource);

/// <summary>
/// ファイルリソースからデータ読み込み。
/// <para>読み込んだ分だけ現在地は進められる。</para>
/// </summary>
/// <param name="file_resource">対象ファイル。</param>
/// <param name="buffer">読み込みデータ格納先。</param>
/// <param name="bytes">読み込みデータサイズ。</param>
/// <returns>読み込んだサイズ。0の場合は終端。読み込みに失敗している場合は負数。</returns>
ssize_t read_file_resource(const FILE_RESOURCE* file_resource, void* buffer, byte_t bytes);

/// <summary>
/// ファイルリソースからデータ書き込み。
/// <para>書き込んだ分だけ現在地は進められる。</para>
/// </summary>
/// <param name="file_resource">対象ファイル。</param>
/// <param name="bytes">読み込みデータ格納先。</param>
/// <param name="length">読み込みデータサイズ。</param>
/// <returns>書き込んだサイズ。書き込み失敗時は負数。</returns>
ssize_t write_file_resource(const FILE_RESOURCE* file_resource, const void* buffer, byte_t bytes);

/// <summary>
/// バッファをクリア。
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
bool flush_file_resource(const FILE_RESOURCE* file_resource);

/// <summary>
/// ファイルリソースの現在地を先頭に移動。
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
bool seek_begin_file_resource(const FILE_RESOURCE* file_resource);
/// <summary>
/// ファイルリソースの現在地を終端に移動。
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
bool seek_end_file_resource(const FILE_RESOURCE* file_resource);
/// <summary>
/// ファイルリソースの現在位置を相対位置に移動。
/// </summary>
/// <param name="file_resource"></param>
/// <param name="relative_position"></param>
/// <returns></returns>
bool seek_current_file_resource(const FILE_RESOURCE* file_resource, const DATA_INT64* relative_position);
/// <summary>
/// ファイルリソースの現在位置を取得。
/// </summary>
/// <param name="file_resource"></param>
/// <param name="position"></param>
/// <returns></returns>
bool set_position_file_resource(const FILE_RESOURCE* file_resource, const DATA_INT64* position);
/// <summary>
/// ファイルリソースの現在地を取得。
/// </summary>
/// <param name="file_resource"></param>
/// <returns></returns>
DATA_INT64 get_position_file_resource(const FILE_RESOURCE* file_resource);
/// <summary>
/// ファイルリソースの現在地をファイル終端に設定する。
/// </summary>
/// <param name="file_resource"></param>
/// <returns></returns>
bool set_current_position_file_resource(const FILE_RESOURCE* file_resource);
/// <summary>
/// ファイルリソースからファイルサイズを取得。
/// </summary>
/// <param name="file_resource"></param>
/// <returns></returns>
DATA_INT64 get_size_file_resource(const FILE_RESOURCE* file_resource);
