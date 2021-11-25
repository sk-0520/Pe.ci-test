#include "lib_math.h"

size_t power_of_2(size_t n)
{
    if (!n) {
        return 0;
    }

    if (!(n & (n - 1))) {
        return n;
    }

    size_t result = 1;
    for (; n; n >>= 1) {
        result <<= 1;
    }
    return result;
}

