#include <windows.h>
#include <crtdbg.h>


#include "tstring.h"
#include "path.h"
#include "logging.h"


TEXT get_parent_directory_path(const TEXT* path)
{
    TCHAR* buffer = clone_string(path->value);
    if (PathRemoveFileSpec(buffer)) {
        TEXT result = new_text(buffer);
        free_string(buffer);
        return result;
    }

    free_string(buffer);

    return create_invalid_text();
}

TEXT combine_path(const TEXT* base_path, const TEXT* relative_path)
{
    size_t total_length = base_path->length + relative_path->length + sizeof(TCHAR)/* \ */;
    TCHAR* buffer = allocate_string(total_length);
    PathCombine(buffer, base_path->value, relative_path->value);

    return wrap_text_with_length(buffer, get_string_length(buffer), true);
}

TEXT join_path(const TEXT* base_path, const TEXT_LIST paths, size_t count)
{
    size_t total_path_length = base_path->length + 1 + count; // ディレクトリ区切り

    for (size_t i = 0; i < count; i++) {
        const TEXT* path = &paths[i];
        total_path_length += path->length;
    }

    TCHAR* buffer = allocate_string(total_path_length);
    copy_string(buffer, base_path->value);

    for (size_t i = 0; i < count; i++) {
        const TEXT* path = &paths[i];
        PathCombine(buffer, buffer, path->value);
    }
    TCHAR* temp_buffer = clone_string(buffer);
    PathCanonicalize(buffer, temp_buffer);
    free_string(temp_buffer);

    return wrap_text_with_length(buffer, get_string_length(buffer), true);

}

TEXT canonicalize_path(const TEXT* path)
{
    TCHAR* buffer = allocate_string(path->length);
    PathCanonicalize(buffer, path->value);

    return wrap_text_with_length(buffer, get_string_length(buffer), true);
}

TEXT get_module_path(HINSTANCE hInstance)
{
    size_t length = MAX_PATH;
    size_t path_length = 0;
    TCHAR* path = NULL;

    while (!path) {
        path = allocate_string(length);
        if (!path) {
            return create_invalid_text();
        }

        DWORD module_path_length = GetModuleFileName(hInstance, path, (DWORD)length);
        if (!module_path_length) {
            path_length = 0;
            break;
        } else if (module_path_length >= length - 1) {
            free_memory(path);
            length *= 2;
        } else {
            path_length = module_path_length;
            break;
        }
    }

    TEXT result = path_length
        ? new_text_with_length(path, path_length)
        : create_invalid_text()
        ;
    free_string(path);

    return result;
}

