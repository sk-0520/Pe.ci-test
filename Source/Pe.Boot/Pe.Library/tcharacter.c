#include "tcharacter.h"

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
        return c - 'a'  + 'A';
    }

    return c;
}

