#pragma once
#include "path.h"

/// <summary>
/// 各種パス情報。
/// </summary>
typedef struct tag_APP_PATH_ITEMS2
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
} APP_PATH_ITEMS2;

TEXT getMainModulePath2(const TEXT* rootDirPath);

void initializeAppPathItems(APP_PATH_ITEMS2* result, HMODULE hInstance);
void uninitializeAppPathItems(APP_PATH_ITEMS2* items);
