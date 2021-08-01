#pragma once
#include "../Pe.Library/text.h"

/// <summary>
/// ラインタイムパスを環境変数に設定。
/// </summary>
/// <param name="root_directory_path"></param>
void add_visual_cpp_runtime_redist_env_path(const TEXT* root_directory_path);
