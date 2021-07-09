#pragma once
#include <tchar.h>
#include <shlwapi.h>

/// <summary>
/// 各種パス情報。
/// </summary>
typedef struct _TAG_APP_PATH_ITEMS
{
    /// <summary>
    /// 起動用アプリケーションファイルパス。
    /// </summary>
    TCHAR application[MAX_PATH];
    size_t applicationLength;

    /// <summary>
    /// 起動用アプリケーションファイル親ディレクトリパス。
    /// </summary>
    TCHAR rootDirectory[MAX_PATH];
    size_t rootDirectoryLength;

    /// <summary>
    /// 本体ファイルパス。
    /// </summary>
    TCHAR mainModule[MAX_PATH];
    size_t mainModuleLength;
} APP_PATH_ITEMS;

/// <summary>
/// パスから親ディレクトリパスを取得。
/// </summary>
/// <param name="result">親ディレクトリパスの格納先。</param>
/// <param name="path">対象パス。</param>
/// <returns>ディレクトリパスの文字列長。</returns>
size_t getParentDirectoryPath(TCHAR* result, const TCHAR* path);

/// <summary>
/// パスを結合する。
/// </summary>
/// <param name="result">結合パスの格納先。</param>
/// <param name="basePath">ベースのパス。</param>
/// <param name="relativePath">結合するパス。</param>
/// <returns>結合結果の長さ。失敗時は0。</returns>
size_t combinePath(TCHAR* result, const TCHAR* basePath, const TCHAR* relativePath);

/// <summary>
/// 実行中モジュールパスの取得。
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <param name="result">実行中モジュールパスの格納先。</param>
/// <returns>実行中モジュールパスの文字列長。</returns>
size_t getApplicationPath(HINSTANCE hInstance, TCHAR* result);

/// <summary>
/// Pe 本体ファイルパスの取得。
/// <para>Pe は起動用EXE(このモジュール)から本体(Pe.Main)を起動するのでその本体ファイルパスを取得する。</para>
/// </summary>
/// <param name="result">Pe 本体ファイルパスの格納先。</param>
/// <param name="rootDirPath">Peのルートディレクトリパス。</param>
/// <returns>Pe 本体ファイルパスの文字列長。</returns>
size_t getMainModulePath(TCHAR* result, const TCHAR* rootDirPath);

/// <summary>
/// 各種パス情報の取得。
/// </summary>
/// <param name="hInstance">実行モジュールインスタンスハンドル。</param>
/// <param name="result">各種パス情報の格納先。</param>
void getAppPathItems(HMODULE hInstance, APP_PATH_ITEMS* result);
