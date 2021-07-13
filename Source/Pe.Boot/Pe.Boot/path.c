#include <windows.h>

#include "tstring.h"
#include "path.h"
#include "logging.h"

size_t getParentDirectoryPath(TCHAR* result, const TCHAR* path)
{
    copyString(result, path);
    PathRemoveFileSpec(result);
    outputDebug(result);
    return getStringLength(result);
}

TEXT getParentDirectoryPath2(const TEXT* path)
{
    TCHAR* buffer = cloneString(path->value);
    if (PathRemoveFileSpec(buffer)) {
        TEXT result = createText(buffer);
        freeString(buffer);
        return result;
    }

    freeString(buffer);

    return createEmptyText();
}


size_t combinePath(TCHAR* result, const TCHAR* basePath, const TCHAR* relativePath)
{
    TCHAR* ret = PathCombine(result, basePath, relativePath);
    if (ret == result) {
        return getStringLength(result);
    }

    return 0;
}

size_t getApplicationPath(HINSTANCE hInstance, TCHAR* result)
{
    TCHAR appRawPath[MAX_PATH];
    GetModuleFileName(hInstance, appRawPath, MAX_PATH);
    // 正規化しておく
    PathCanonicalize(result, appRawPath);
    outputDebug(result);
    return getStringLength(result);
}

TEXT getModulePath(HINSTANCE hInstance)
{
    size_t length = MAX_PATH;
    size_t pathLength = 0;
    TCHAR* path = NULL;

    while (!path) {
        path = (TCHAR*)allocateMemory(length * sizeof(TCHAR), false);
        if (!path) {
            return createEmptyText();
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
        ? createTextWithLength(path, pathLength)
        : createEmptyText()
        ;
    freeMemory(path);

    return result;
}


size_t getMainModulePath(TCHAR* result, const TCHAR* rootDirPath)
{
    TCHAR binPath[MAX_PATH];
    binPath[0] = 0;
    combinePath(binPath, rootDirPath, _T("bin"));
    combinePath(result, binPath, _T("Pe.Main.exe"));
    outputDebug(result);
    return getStringLength(result);
}

void getAppPathItems(HMODULE hInstance, APP_PATH_ITEMS* result)
{
    result->applicationLength = getApplicationPath(hInstance, result->application);

    result->rootDirectoryLength = getParentDirectoryPath(result->rootDirectory, result->application);

    result->mainModuleLength = getMainModulePath(result->mainModule, result->rootDirectory);
}

void getAppPathItems2(APP_PATH_ITEMS2* result, HMODULE hInstance)
{

}

