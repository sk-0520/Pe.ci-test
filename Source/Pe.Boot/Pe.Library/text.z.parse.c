
/* 自動生成: text.z.parse.c.tt */
#include "text.h"
#include "debug.h"

static bool check_has_i_sign(const TEXT* text)
{
    assert(text);
    assert(text->length);

    return text->value[0] == '-' || text->value[0] == '+';
}

static bool check_has_u_sign(const TEXT* text)
{
    assert(text);
    assert(text->length);

    return text->value[0] == '+';
}







