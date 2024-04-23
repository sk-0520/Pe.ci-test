#include "fsio.h"


FILE_RESOURCE create_invalid_file_resource()
{
    FILE_RESOURCE result = {
        .path = create_invalid_text(),
        .handle = NULL,
        .library = {
            .memory_arena_resource = NULL,
        }
    };
    return result;
}

FILE_RESOURCE RC_FILE_FUNC(new_file_resource, const TEXT* path, FILE_ACCESS_MODE access_mode, FILE_SHARE_MODE shared_mode, FILE_OPEN_MODE open_mode, DWORD attributes, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!path) {
        return create_invalid_file_resource();
    }
    if (!path->value) {
        return create_invalid_file_resource();
    }

    HANDLE handle = CreateFile(path->value, access_mode, shared_mode, NULL, open_mode, attributes, NULL);
    if (handle == INVALID_HANDLE_VALUE) {
        return create_invalid_file_resource();
    }

    FILE_RESOURCE result = {
        .path = clone_text(path, memory_arena_resource),
        .handle = handle,
        .library = {
            .memory_arena_resource = memory_arena_resource,
        }
    };

#ifdef RES_CHECK
    library_rc_file_check(result.handle, result.path.value, true, RES_CHECK_CALL_ARGS);
#endif

    return result;
}

FILE_RESOURCE RC_FILE_FUNC(create_file_resource, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return RC_FILE_CALL(new_file_resource, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_NEW, 0, memory_arena_resource);
}

FILE_RESOURCE RC_FILE_FUNC(open_file_resource, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return RC_FILE_CALL(new_file_resource, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN, 0, memory_arena_resource);
}

FILE_RESOURCE RC_FILE_FUNC(open_or_create_file_resource, const TEXT* path, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return RC_FILE_CALL(new_file_resource, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN_OR_CREATE, 0, memory_arena_resource);
}

bool RC_FILE_FUNC(release_file_resource, FILE_RESOURCE* file_resource)
{
    if (!file_resource) {
        return false;
    }

    if (!is_enabled_file_resource(file_resource)) {
        return false;
    }

#ifdef RES_CHECK
    library_rc_file_check(file_resource->handle, NULL, false, RES_CHECK_CALL_ARGS);
#endif

    release_text(&file_resource->path);
    bool result = CloseHandle(file_resource->handle);
    file_resource->handle = NULL;

    return result;
}

bool is_enabled_file_resource(const FILE_RESOURCE* file_resource)
{
    if (!file_resource) {
        return false;
    }

    if (!is_enabled_text(&file_resource->path)) {
        return false;
    }

    if (!file_resource->handle) {
        return false;
    }

    return true;
}

ssize_t read_file_resource(const FILE_RESOURCE* file_resource, void* buffer, byte_t bytes)
{
    DWORD read_length = 0;
    if (ReadFile(file_resource->handle, (void*)buffer, (DWORD)bytes, &read_length, NULL)) {
        return read_length;
    }

    return -1;
}

ssize_t write_file_resource(const FILE_RESOURCE* file_resource, const void* buffer, byte_t bytes)
{
    DWORD write_length = 0;
    if (WriteFile(file_resource->handle, (void*)buffer, (DWORD)bytes, &write_length, NULL)) {
        return write_length;
    }

    return -1;
}

bool flush_file_resource(const FILE_RESOURCE* file_resource)
{
    return FlushFileBuffers(file_resource->handle);
}

bool seek_begin_file_resource(const FILE_RESOURCE* file_resource)
{
    return SetFilePointer(file_resource->handle, 0, 0, FILE_BEGIN) != INVALID_SET_FILE_POINTER;
}

bool seek_end_file_resource(const FILE_RESOURCE* file_resource)
{
    return SetFilePointer(file_resource->handle, 0, 0, FILE_END) != INVALID_SET_FILE_POINTER;
}

bool seek_current_file_resource(const FILE_RESOURCE* file_resource, const DATA_INT64* relative_position)
{
    return SetFilePointerEx(file_resource->handle, relative_position->large, NULL, FILE_CURRENT);
}

bool set_position_file_resource(const FILE_RESOURCE* file_resource, const DATA_INT64* position)
{
    return SetFilePointerEx(file_resource->handle, position->large, NULL, FILE_BEGIN);
}

DATA_INT64 get_position_file_resource(const FILE_RESOURCE* file_resource)
{
    DATA_INT64 result = { 0 };

    result.large.LowPart = SetFilePointer(file_resource->handle, 0, &result.large.HighPart, FILE_CURRENT);

    return result;
}

bool set_current_position_file_resource(const FILE_RESOURCE* file_resource)
{
    return SetEndOfFile(file_resource->handle);
}

DATA_INT64 get_size_file_resource(const FILE_RESOURCE* file_resource)
{
    DATA_INT64 data = { 0 };

    if (!GetFileSizeEx(file_resource->handle, &data.large)) {
        data.plain = -1;
    }

    return data;
}
