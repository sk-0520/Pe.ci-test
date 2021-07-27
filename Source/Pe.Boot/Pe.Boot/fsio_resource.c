#include "fsio.h"


static FILE_RESOURCE create_invalid_file()
{
    FILE_RESOURCE result = {
        .path = create_invalid_text(),
        .handle = NULL,
    };
    return result;
}

static FILE_RESOURCE RC_FILE_FUNC(open_file_resource_core, const TEXT* path, FILE_ACCESS_MODE accessMode, FILE_SHARE_MODE sharedMode, FILE_OPEN_MODE openMode, DWORD attributes)
{
    if (!path) {
        return create_invalid_file();
    }
    if (!path->value) {
        return create_invalid_file();
    }

    HANDLE handle = CreateFile(path->value, accessMode, sharedMode, NULL, openMode, attributes, NULL);
    if (handle == INVALID_HANDLE_VALUE) {
        return create_invalid_file();
    }

    FILE_RESOURCE result = {
        .path = clone_text(path),
        .handle = handle
    };

#ifdef RES_CHECK
    rc__file_check(result.handle, result.path.value, true, RES_CHECK_CALL_ARGS);
#endif

    return result;
}

FILE_RESOURCE RC_FILE_FUNC(create_file_resource, const TEXT* path)
{
    return RC_FILE_CALL(open_file_resource_core, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_NEW, 0);
}

FILE_RESOURCE RC_FILE_FUNC(open_file_resource, const TEXT* path)
{
    return RC_FILE_CALL(open_file_resource_core, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN, 0);
}

FILE_RESOURCE RC_FILE_FUNC(open_or_create_file_resource, const TEXT* path)
{
    return RC_FILE_CALL(open_file_resource_core, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN_OR_CREATE, 0);
}

bool RC_FILE_FUNC(close_file_resource, FILE_RESOURCE* file)
{
    if (!file) {
        return false;
    }

    if (!is_enabled_file_resource(file)) {
        return false;
    }

#ifdef RES_CHECK
    rc__file_check(file->handle, NULL, false, RES_CHECK_CALL_ARGS);
#endif

    free_text(&file->path);
    bool result = CloseHandle(file->handle);
    file->handle = NULL;

    return result;
}

bool is_enabled_file_resource(const FILE_RESOURCE* file)
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

ssize_t read_file_resource(const FILE_RESOURCE* file, void* buffer, size_t length)
{
    DWORD read_length = 0;
    if (ReadFile(file->handle, (void*)buffer, (DWORD)length, &read_length, NULL)) {
        return read_length;
    }

    return -1;
}

ssize_t write_file_resource(const FILE_RESOURCE* file, void* buffer, size_t length)
{
    DWORD write_length = 0;
    if (WriteFile(file->handle, (void*)buffer, (DWORD)length, &write_length, NULL)) {
        return write_length;
    }

    return -1;
}

bool seek_begin_file_resource(const FILE_RESOURCE* file)
{
    return SetFilePointer(file->handle, 0, 0, FILE_BEGIN) != INVALID_SET_FILE_POINTER;
}

bool seek_end_file_resource(const FILE_RESOURCE* file)
{
    return SetFilePointer(file->handle, 0, 0, FILE_END) != INVALID_SET_FILE_POINTER;
}

bool cut_current_position_file_resource(const FILE_RESOURCE* file)
{
    return SetEndOfFile(file->handle);
}
