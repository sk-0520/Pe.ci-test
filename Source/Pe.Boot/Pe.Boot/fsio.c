#include <shlwapi.h>

#include "fsio.h"

bool is_directory(const TEXT* path)
{
    return PathIsDirectory(path->value);
}

bool exists_file(const TEXT* path)
{
    return PathFileExists(path->value) && !is_directory(path);
}

bool exists_directory(const TEXT* path)
{
    return PathFileExists(path->value) && is_directory(path);
}
