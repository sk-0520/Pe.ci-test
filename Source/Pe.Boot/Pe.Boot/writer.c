#include "writer.h"

#define TRUE_UPPER "TRUE"
#define TRUE_LOWER "true"
#define FALSE_UPPER "FALSE"
#define FALSE_LOWER "false"

bool write_to_primitive_boolean(func_string_writer writer, void* receiver, bool value, bool is_uppper)
{
    WRITE_STRING_DATA data;
    if (value) {
        if (is_uppper) {
            data.value = _T(TRUE_UPPER);
            data.length = sizeof(TRUE_UPPER) - 1;
        } else {
            data.value = _T(TRUE_LOWER);
            data.length = sizeof(TRUE_LOWER) - 1;
        }
    } else {
        if (is_uppper) {
            data.value = _T(FALSE_UPPER);
            data.length = sizeof(FALSE_UPPER) - 1;
        } else {
            data.value = _T(FALSE_LOWER);
            data.length = sizeof(FALSE_LOWER) - 1;
        }
    }

    return writer(receiver, &data);
}
