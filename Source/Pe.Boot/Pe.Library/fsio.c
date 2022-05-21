#include "fsio.h"

static DWORD get_file_attributes_core(const TEXT* path)
{
    if (!path) {
        return INVALID_FILE_ATTRIBUTES;
    }

    if (path->library.sentinel) {
        return GetFileAttributes(path->value);
    }

    const MEMORY_RESOURCE* memory_resource = path->library.memory_resource
        ? path->library.memory_resource
        : get_default_memory_resource()
        ;

    TCHAR* cstr = text_to_string(path, memory_resource);
    DWORD attr = GetFileAttributes(path->value);
    release_string(cstr, memory_resource);

    return attr;
}

bool exists_directory_fsio(const TEXT* path)
{
    DWORD attr = get_file_attributes_core(path);
    if (attr == INVALID_FILE_ATTRIBUTES) {
        return false;
    }
    return (attr & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY;
}

bool exists_file_fsio(const TEXT* path)
{
    DWORD attr = get_file_attributes_core(path);
    if (attr == INVALID_FILE_ATTRIBUTES) {
        return false;
    }
    return (attr & FILE_ATTRIBUTE_DIRECTORY) != FILE_ATTRIBUTE_DIRECTORY;
}

bool exists_fsio(const TEXT* path)
{
    DWORD attr = get_file_attributes_core(path);
    if (attr == INVALID_FILE_ATTRIBUTES) {
        return false;
    }

    return true;
}
