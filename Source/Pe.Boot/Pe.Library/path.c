#include <windows.h>


#include "tstring.h"
#include "path.h"
#include "logging.h"


TEXT RC_HEAP_FUNC(get_parent_directory_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource)
{
    TCHAR* buffer = clone_string(path->value, memory_resource);
    if (PathRemoveFileSpec(buffer)) {
        TEXT result = new_text(buffer, memory_resource);
        free_string(buffer, memory_resource);
        return result;
    }

    free_string(buffer, memory_resource);

    return create_invalid_text();
}

TEXT RC_HEAP_FUNC(combine_path, const TEXT* base_path, const TEXT* relative_path, const MEMORY_RESOURCE* memory_resource)
{
    size_t total_length = (size_t)base_path->length + relative_path->length + sizeof(TCHAR)/* \ */;
    TCHAR* buffer = RC_HEAP_CALL(allocate_string, total_length, memory_resource);
    PathCombine(buffer, base_path->value, relative_path->value);
    size_t buffer_length = get_string_length(buffer);
    TCHAR* c = buffer + (buffer_length - 1);
    if (*c == '\\' || *c == '/') {
        *c = 0;
        buffer_length -= 1;
    }
    return wrap_text_with_length(buffer, buffer_length, true, memory_resource);
}

TEXT RC_HEAP_FUNC(join_path, const TEXT* base_path, const TEXT_LIST paths, size_t count, const MEMORY_RESOURCE* memory_resource)
{
    size_t total_path_length = (size_t)base_path->length + 1 + count; // ディレクトリ区切り

    for (size_t i = 0; i < count; i++) {
        const TEXT* path = &paths[i];
        total_path_length += path->length;
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, total_path_length, memory_resource);
    copy_string(buffer, base_path->value);

    for (size_t i = 0; i < count; i++) {
        const TEXT* path = &paths[i];
        PathCombine(buffer, buffer, path->value);
    }
    TCHAR* temp_buffer = clone_string(buffer, memory_resource);
    PathCanonicalize(buffer, temp_buffer);
    free_string(temp_buffer, memory_resource);

    return wrap_text_with_length(buffer, get_string_length(buffer), true, memory_resource);
}

TEXT RC_HEAP_FUNC(canonicalize_path, const TEXT* path, const MEMORY_RESOURCE* memory_resource)
{
    TCHAR* buffer = RC_HEAP_CALL(allocate_string, path->length, memory_resource);
    PathCanonicalize(buffer, path->value);

    return wrap_text_with_length(buffer, get_string_length(buffer), true, memory_resource);
}

TEXT RC_HEAP_FUNC(get_module_path, HINSTANCE hInstance, const MEMORY_RESOURCE* memory_resource)
{
    size_t length = MAX_PATH;
    size_t path_length = 0;
    TCHAR* path = NULL;

    while (!path) {
        path = allocate_string(length, memory_resource);
        if (!path) {
            return create_invalid_text();
        }

        DWORD module_path_length = GetModuleFileName(hInstance, path, (DWORD)length);
        if (!module_path_length) {
            path_length = 0;
            break;
        } else if (module_path_length >= length - 1) {
            free_string(path, memory_resource);
            path = NULL;
            length *= 2;
        } else {
            path_length = module_path_length;
            break;
        }
    }

    TEXT result = path_length
        ? new_text_with_length(path, path_length, memory_resource)
        : create_invalid_text()
        ;
    free_string(path, memory_resource);

    return result;
}

