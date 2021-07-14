#include "app_path.h"


TEXT getMainModulePath2(const TEXT* rootDirPath)
{
    TEXT joinPaths[] = {
        wrapText(_T("bin")),
        wrapText(_T("Pe.Main.exe")),
    };
    size_t joinPathsLength = sizeof(joinPaths) / sizeof(joinPaths[0]);

    return joinPath(rootDirPath, joinPaths, joinPathsLength);
}

void initializeAppPathItems(APP_PATH_ITEMS2* result, HMODULE hInstance)
{
    result->application = getModulePath(hInstance);
    result->rootDirectory = getParentDirectoryPath2(&result->application);
    result->mainModule = getMainModulePath2(&result->rootDirectory);
}

void uninitializeAppPathItems(APP_PATH_ITEMS2* items)
{
    freeText(&items->application);
    freeText(&items->rootDirectory);
    freeText(&items->mainModule);
}

