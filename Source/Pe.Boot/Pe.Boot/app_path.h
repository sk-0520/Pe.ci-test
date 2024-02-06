#pragma once
#include "../Pe.Library/path.h"

/// <summary>
/// 各種パス情報。
/// </summary>
typedef struct tag_APP_PATH_ITEMS
{
    /// <summary>
    /// 起動用アプリケーションファイルパス。
    /// </summary>
    TEXT application;

    /// <summary>
    /// 起動用アプリケーションファイル親ディレクトリパス。
    /// </summary>
    TEXT root_directory;

    /// <summary>
    /// 本体ファイルパス。
    /// </summary>
    TEXT main_module;
} APP_PATH_ITEMS;

/// <summary>
/// 本体アプリケーションのパスを取得。
/// </summary>
/// <param name="root_directory_path">Peのルートディレクトリパス。</param>
/// <returns>解放が必要。</returns>
TEXT get_main_module_path(const TEXT* root_directory_path);

/// <summary>
/// アプリケーションパスの取得。
/// </summary>
/// <param name="result"></param>
/// <param name="hInstance">解放が必要</param>
void initialize_app_path_items(APP_PATH_ITEMS* result, HMODULE hInstance);
/// <summary>
/// アプリケーションパスの解放。
/// </summary>
/// <param name="items"></param>
void finalize_app_path_items(APP_PATH_ITEMS* items);
