#include "app_path.h"


TEXT getMainModulePath(const TEXT* rootDirPath)
{
    TEXT joinPaths[] = {
        wrapText(_T("bin")),
        wrapText(_T("Pe.Main.exe")),
    };
    size_t joinPathsLength = sizeof(joinPaths) / sizeof(joinPaths[0]);

    return joinPath(rootDirPath, joinPaths, joinPathsLength);
}

void initializeAppPathItems(APP_PATH_ITEMS* result, HMODULE hInstance)
{
    result->application = getModulePath(hInstance);
    result->rootDirectory = getParentDirectoryPath(&result->application);
    result->mainModule = getMainModulePath(&result->rootDirectory);
}

void uninitializeAppPathItems(APP_PATH_ITEMS* items)
{
    freeText(&items->application);
    freeText(&items->rootDirectory);
    freeText(&items->mainModule);
}

