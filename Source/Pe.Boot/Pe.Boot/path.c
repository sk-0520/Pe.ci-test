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
