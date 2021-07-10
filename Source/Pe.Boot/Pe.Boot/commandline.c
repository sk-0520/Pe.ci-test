#include "commandline.h"
#include "tstring.h"

/**
書式調整後の動的確保された文字列を返す。
呼び出し側で世話すること。
*/
TCHAR* tuneArg(const TCHAR* arg)
{
    int hasSpace = findCharacter(arg, ' ') != NULL;
    size_t len = (size_t)getStringLength(arg) + (hasSpace ? 2 : 0);
    TCHAR* s = allocateClearMemory(len + 1, sizeof(TCHAR*));
    Assert(s);
    if (hasSpace) {
        copyString(s + 1, arg);
        s[0] = '"';
        s[len - 1] = '"';
        s[len - 0] = 0; // ↑で +1 してるから安全安全
    }
    else {
        copyString(s, arg);
    }
    return s;
}
