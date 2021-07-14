#include "app_path.h"

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

