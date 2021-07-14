#include "fsio.h"
#include "memory.h"
#include "tstring.h"

static FILE_POINTER _createInvalidFile()
{
    FILE_POINTER result = {
        .path = (TCHAR*)0,
        .handle = NULL,
    };
    return result;
}

static FILE_POINTER _openFileCore(const TCHAR* path, FILE_ACCESS_MODE accessMode, FILE_SHARE_MODE sharedMode, FILE_OPEN_MODE openMode, DWORD attributes)
{
    if (!path) {
        return _createInvalidFile();
    }

    HANDLE handle = CreateFile(path, accessMode, sharedMode, NULL, openMode, attributes, NULL);
    if (!handle) {
        return _createInvalidFile();
    }

    FILE_POINTER result = {
        .path = cloneString(path),
        .handle = handle
    };

    return result;
}

FILE_POINTER createFile(const TCHAR* path)
{
    return _openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_NEW, 0);
}

FILE_POINTER openFile(const TCHAR* path)
{
    return _openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN, 0);
}

FILE_POINTER openOrCreateFile(const TCHAR* path)
{
    return _openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN_OR_CREATE, 0);
}

bool closeFile(const FILE_POINTER* file)
{
    if (!file) {
        return false;
    }

    if (!isEnabledFile(file)) {
        return false;
    }

    freeString(file->path);
    return CloseHandle((HANDLE)(void*)file->handle);
}

bool isEnabledFile(const FILE_POINTER* file)
{
    if (!file) {
        return false;
    }

    if (!file->path) {
        return false;
    }

    if (!file->handle) {
        return false;
    }

    return true;
}

