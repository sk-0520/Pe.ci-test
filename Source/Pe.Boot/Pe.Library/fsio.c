#include "fsio.h"

static DWORD get_file_attributes_core(const TEXT* path)
{
    if (!path) {
        return INVALID_FILE_ATTRIBUTES;
    }

    if (path->library.sentinel) {
        return GetFileAttributes(path->value);
    }

    TEXT text = get_sentinel_text(path);
    DWORD attr = GetFileAttributes(text.value);
    release_text(&text);

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

bool create_directory_fsio(const TEXT* path)
{
    SECURITY_ATTRIBUTES* attributes = NULL;

    if (path->library.sentinel) {
        return CreateDirectory(path->value, attributes);
    }

    TEXT text = get_sentinel_text(path);
    BOOL result = CreateDirectory(text.value, attributes);
    release_text(&text);

    return result;
}
