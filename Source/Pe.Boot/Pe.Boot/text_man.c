#include <windows.h>
#include <shlwapi.h>

#include "text.h"


size_t getTextLength(const TEXT* text)
{
    if (!isEnabledText(text)) {
        return 0;
    }

    return text->length;
}

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

static TCHAR* findCharacterCore(const TCHAR* haystack, TCHAR needle)
{
    while (*haystack != needle) {
        if (!*haystack) {
            return NULL;
        }
        haystack++;
    }

    return (TCHAR*)haystack;
}

TEXT findCharacter2(const TEXT* haystack, TCHAR needle)
{
    TCHAR* s = findCharacterCore(haystack->value, needle);
    if (!s) {
        return createEmptyText();
    }

    return wrapText(s);
}

ssize_t indexOfCharacter(const TEXT* haystack, TCHAR needle)
{
    TCHAR* s = findCharacterCore(haystack->value, needle);
    if (!s) {
        return -1;
    }

    return s - haystack->value;
}
