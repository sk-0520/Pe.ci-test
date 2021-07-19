#include "app_path.h"


TEXT get_main_module_path(const TEXT* root_directory_path)
{
    TEXT join_paths[] = {
        wrap_text(_T("bin")),
        wrap_text(_T("Pe.Main.exe")),
    };
    size_t join_paths_length = sizeof(join_paths) / sizeof(join_paths[0]);

    return join_path(root_directory_path, join_paths, join_paths_length);
}

void initialize_app_path_items(APP_PATH_ITEMS* result, HMODULE hInstance)
{
    result->application = get_module_path(hInstance);
    result->rootDirectory = get_parent_directory_path(&result->application);
    result->mainModule = get_main_module_path(&result->rootDirectory);
}

void uninitialize_app_path_items(APP_PATH_ITEMS* items)
{
    free_text(&items->application);
    free_text(&items->rootDirectory);
    free_text(&items->mainModule);
}

