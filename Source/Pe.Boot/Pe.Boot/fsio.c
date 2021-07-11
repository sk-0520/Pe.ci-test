#include "fsio.h"
#include "memory.h"

static FILE_POINTER createInvalidFile()
{
    FILE_POINTER result = { NULL, (HFILE)NULL };
    return result;
}

static FILE_POINTER openFileCore(const TCHAR* path, FILE_ACCESS_MODE accessMode, FILE_SHARE_MODE sharedMode, FILE_OPEN_MODE openMode, DWORD attributes)
{
    if (!path) {
        return createInvalidFile();
    }

    HFILE hFIle = (HFILE)CreateFile(path, accessMode, sharedMode, NULL, openMode, attributes, NULL);
    if (!hFIle) {
        return createInvalidFile();
    }

    FILE_POINTER result = {
        path,
        hFIle
    };

    return result;
}

FILE_POINTER createFile(const TCHAR* path)
{
    return openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_NEW, 0);
}

FILE_POINTER openFile(const TCHAR* path)
{
    return openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN, 0);
}

FILE_POINTER openOrCreateFile(const TCHAR* path)
{
    return openFileCore(path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN_OR_CREATE, 0);
}

bool closeFile(const FILE_POINTER* file)
{
    if (!file) {
        return false;
    }

    if (!isEnabledFile(file)) {
        return false;
    }

    return CloseHandle((HANDLE)file->handle);
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

