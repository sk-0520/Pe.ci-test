#include "fsio.h"

static FILE_POINTER create_invalid_file()
{
    FILE_POINTER result = {
        .path = create_invalid_text(),
        .handle = NULL,
    };
    return result;
}

static FILE_POINTER openFileCore(const TEXT* path, FILE_ACCESS_MODE accessMode, FILE_SHARE_MODE sharedMode, FILE_OPEN_MODE openMode, DWORD attributes)
{
    if (!path) {
        return create_invalid_file();
    }
    if (!path->value) {
        return create_invalid_file();
    }

    HANDLE handle = CreateFile(path->value, accessMode, sharedMode, NULL, openMode, attributes, NULL);
    if (!handle) {
        return create_invalid_file();
    }

    FILE_POINTER result = {
        .path = clone_text(path),
        .handle = handle
    };

    return result;
}

FILE_POINTER create_file(const TEXT* path)
{
    return openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_NEW, 0);
}

FILE_POINTER open_file(const TEXT* path)
{
    return openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN, 0);
}

FILE_POINTER open_or_create_file(const TEXT* path)
{
    return openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN_OR_CREATE, 0);
}

bool close_file(FILE_POINTER* file)
{
    if (!file) {
        return false;
    }

    if (!is_enabled_file(file)) {
        return false;
    }

    free_text(&file->path);
    bool result = CloseHandle(file->handle);
    file->handle = NULL;

    return result;
}

bool is_enabled_file(const FILE_POINTER* file)
{
    if (!file) {
        return false;
    }

    if (!is_enabled_text(&file->path)) {
        return false;
    }

    if (!file->handle) {
        return false;
    }

    return true;
}

