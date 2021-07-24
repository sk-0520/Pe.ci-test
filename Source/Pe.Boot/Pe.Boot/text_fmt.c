#include <stdarg.h>

#include "text.h"
#include "string_builder.h"

#define FORMAT_LENGTH (256)

TEXT vformat_text(const TEXT* format, va_list ap)
{
    STRING_BUILDER sb = create_string_builder(FORMAT_LENGTH);

    bool now_format = false;
    size_t format_begin_index = 0;

    for (size_t i = 0; i < format->length; i++) {
        TCHAR c = format->value[i];

        if (now_format) {
        } else {
            if (c == _T('%')) {
                now_format = true;
                format_begin_index = i;
            } else {
                append_builder_character(&sb, c, false);
            }
        }
    }

    return create_invalid_text();
}

TEXT format_text(const TEXT* format, ...)
{
    va_list ap;

    va_start(ap, format);

    TEXT result = vformat_text(format, ap);

    va_end(ap);

    return result;
}

