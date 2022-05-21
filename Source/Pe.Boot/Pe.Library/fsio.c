#include "fsio.h"

bool exists_directory_fsio(const TEXT* path)
{
    DWORD attr = GetFileAttributes(path->value);
    if (attr == -1) {
        return false;
    }
    return (attr & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY;
}

bool exists_file_fsio(const TEXT* path)
{
    DWORD attr = GetFileAttributes(path->value);
    if (attr == -1) {
        return false;
    }
    return (attr & FILE_ATTRIBUTE_DIRECTORY) != FILE_ATTRIBUTE_DIRECTORY;
}

bool exists_fsio(const TEXT* path)
{
    DWORD attr = GetFileAttributes(path->value);
    if (attr == -1) {
        return false;
    }

    return true;
}
