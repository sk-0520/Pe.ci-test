﻿#pragma once
#include "path.h"

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
    TEXT rootDirectory;

    /// <summary>
    /// 本体ファイルパス。
    /// </summary>
    TEXT mainModule;
} APP_PATH_ITEMS;

TEXT get_main_module_path(const TEXT* root_directory_path);

void initialize_app_path_items(APP_PATH_ITEMS* result, HMODULE hInstance);
void uninitialize_app_path_items(APP_PATH_ITEMS* items);
