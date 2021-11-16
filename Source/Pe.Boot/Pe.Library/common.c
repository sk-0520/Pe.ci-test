#include "common.h"

byte_t to_byte_size(size_t byte_size)
{
    byte_t result = {
        .length = byte_size
    };
    return result;
}

size_t get_byte_size(byte_t byte)
{
    return byte.length;
}
