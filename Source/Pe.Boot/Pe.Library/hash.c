#include "hash.h"

#ifdef _WIN64
#   define FNV_OFFSET (14695981039346656037U)
#   define FNV_PRIME (1099511628211LLU)
#else
#   define FNV_OFFSET (2166136261U)
#   define FNV_PRIME (16777619U)
#endif

size_t calc_hash_fnv1(const uint8_t* value, size_t length)
{
    size_t result = FNV_OFFSET;

    for (size_t i = 0; i < length; i++) {
        result += (FNV_PRIME * result) ^ value[i];
    }

    return result;
}

size_t calc_hash_fnv1_from_text(const TEXT* text)
{
    if (!text || !is_enabled_text(text)) {
        return 0;
    }
    
    size_t length = text->length * sizeof(TCHAR);
    return calc_hash_fnv1((const uint8_t*)text->value, length);
}
