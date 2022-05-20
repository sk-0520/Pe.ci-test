#include "tcharacter.h"
#include "debug.h"

bool is_newline_character(TCHAR c)
{
    return
        c == '\r'
        ||
        c == '\n'
        ;
}

bool is_digit_character(TCHAR c)
{
    return '0' <= c && c <= '9';
}

bool is_lower_character(TCHAR c)
{
    return 'a' <= c && c <= 'z';
}

bool is_upper_character(TCHAR c)
{
    return 'A' <= c && c <= 'Z';
}

bool is_alphabet_character(TCHAR c)
{
    return
        ('A' <= c && c <= 'Z')
        ||
        ('a' <= c && c <= 'z')
        ;
}

TCHAR to_lower_character(TCHAR c)
{
    if (is_upper_character(c)) {
        return c - 'A' + 'a';
    }

    return c;

}

TCHAR to_upper_character(TCHAR c)
{
    if (is_lower_character(c)) {
        return c - 'a' + 'A';
    }

    return c;
}

bool contains_characters(TCHAR c, const TCHAR* characters, size_t length)
{
    assert(characters);

    for (size_t i = 0; i < length; i++) {
        if (c == characters[i]) {
            return true;
        }
    }

    return false;
}


