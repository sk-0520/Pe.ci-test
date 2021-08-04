#include <shlwapi.h>

#include "fsio.h"

bool is_directory_path(const TEXT* path)
{
    return PathIsDirectory(path->value);
}

bool exists_file_path(const TEXT* path)
{
    return PathFileExists(path->value) && !is_directory_path(path);
}

bool exists_directory_path(const TEXT* path)
{
    return PathFileExists(path->value) && is_directory_path(path);
}
