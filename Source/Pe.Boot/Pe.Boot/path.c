#include <windows.h>
#include <crtdbg.h>


#include "tstring.h"
#include "path.h"
#include "logging.h"

TEXT getParentDirectoryPath2(const TEXT* path)
{
    TCHAR* buffer = cloneString(path->value);
    if (PathRemoveFileSpec(buffer)) {
        TEXT result = newText(buffer);
        freeString(buffer);
        return result;
    }

    freeString(buffer);

    return createInvalidText();
}

TEXT combinePath2(const TEXT* basePath, const TEXT* relativePath)
{
    size_t totalLength = basePath->length + relativePath->length + sizeof(TCHAR)/* \ */;
    TCHAR* buffer = allocateString(totalLength);
    PathCombine(buffer, basePath->value, relativePath->value);

    return wrapTextWithLength(buffer, getStringLength(buffer), true);
}

TEXT joinPath(const TEXT* basePath, const TEXT paths[], size_t pathsLength)
{
    size_t totalPathLength = basePath->length + 1 + pathsLength; // ディレクトリ区切り

    for (size_t i = 0; i < pathsLength; i++) {
        const TEXT* path = &paths[i];
        totalPathLength += path->length;
    }

    TCHAR* buffer = allocateString(totalPathLength);
    copyString(buffer, basePath->value);

    for (size_t i = 0; i < pathsLength; i++) {
        const TEXT* path = &paths[i];
        PathCombine(buffer, buffer, path->value);
    }
    TCHAR* tempBuffer = cloneString(buffer);
    PathCanonicalize(buffer, tempBuffer);
    freeString(tempBuffer);

    return wrapTextWithLength(buffer, getStringLength(buffer), true);

}

TEXT canonicalizePath(const TEXT* path)
{
    TCHAR* buffer = allocateString(path->length);
    PathCanonicalize(buffer, path->value);

    return wrapTextWithLength(buffer, getStringLength(buffer), true);
}

TEXT getModulePath(HINSTANCE hInstance)
{
    size_t length = MAX_PATH;
    size_t pathLength = 0;
    TCHAR* path = NULL;

    while (!path) {
        path = allocateString(length);
        if (!path) {
            return createInvalidText();
        }

        DWORD modulePathLength = GetModuleFileName(hInstance, path, (DWORD)length);
        if (!modulePathLength) {
            pathLength = 0;
            break;
        } else if (modulePathLength >= length - 1) {
            freeMemory(path);
            length *= 2;
        } else {
            pathLength = modulePathLength;
            break;
        }
    }

    TEXT result = pathLength
        ? newTextWithLength(path, pathLength)
        : createInvalidText()
        ;
    freeString(path);

    return result;
}

