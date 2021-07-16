#include <windows.h>
#include <shlwapi.h>

#include "text.h"


TEXT findText(const TEXT* haystack, const TEXT* needle, bool ignoreCase)
{
    TCHAR* s = ignoreCase
        ? StrStrI(haystack->value, needle->value)
        : StrStr(haystack->value, needle->value)
        ;

    if (!s) {
        return createEmptyText();
    }

    return wrapText(s);
}

size_t getTextLength(const TEXT* text)
{
    if (!isEnabledText(text)) {
        return 0;
    }

    return text->length;
}
