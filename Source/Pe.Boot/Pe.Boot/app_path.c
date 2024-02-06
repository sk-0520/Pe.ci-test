#include "app_path.h"


TEXT get_main_module_path(const TEXT* root_directory_path)
{
    TEXT join_paths[] = {
        wrap_text(_T("bin")),
        wrap_text(_T("Pe.Main.exe")),
    };

    return join_path(root_directory_path, join_paths, SIZEOF_ARRAY(join_paths), DEFAULT_MEMORY_ARENA);
}

void initialize_app_path_items(APP_PATH_ITEMS* result, HMODULE hInstance)
{
    result->application = get_module_path(hInstance, DEFAULT_MEMORY_ARENA);
    result->root_directory = get_parent_directory_path(&result->application, DEFAULT_MEMORY_ARENA);
    result->main_module = get_main_module_path(&result->root_directory);
}

void finalize_app_path_items(APP_PATH_ITEMS* items)
{
    release_text(&items->application);
    release_text(&items->root_directory);
    release_text(&items->main_module);
}

