#include <shlwapi.h>

#include "debug.h"
#include "res_check.h"
#include "tstring.h"

size_t get_string_length(const TCHAR* s)
{
    return lstrlen(s);
}

TCHAR* concat_string(TCHAR* target, const TCHAR* value)
{
    return lstrcat(target, value);
}

TCHAR* copy_string(TCHAR* result, const TCHAR* value)
{
    return lstrcpy(result, value);
}

TCHAR* RC_HEAP_FUNC(clone_string, const TCHAR* source)
{
    if (!source) {
        return NULL;
    }

    size_t length = get_string_length(source);
    size_t bytes = (length * sizeof(TCHAR)) + sizeof(TCHAR);
    TCHAR* result = (TCHAR*)RC_HEAP_CALL(allocate_memory, bytes, false);
    copy_memory(result, (void*)source, length * sizeof(TCHAR));
    result[length] = 0;

    return result;
}

TCHAR* RC_HEAP_FUNC(allocate_string, size_t length)
{
    size_t bytes = sizeof(TCHAR) * length + sizeof(TCHAR);
    TCHAR* result = RC_HEAP_CALL(allocate_memory, bytes, false);
    result[0] = 0;
    return result;
}

void RC_HEAP_FUNC(free_string, const TCHAR* s)
{
    RC_HEAP_CALL(free_memory, (void*)s);
}

STRING_BUILDER RC_HEAP_FUNC(initialize_string_builder, const TCHAR* s, size_t capacity)
{
    assert_debug(s);

    size_t length = get_string_length(s);
    size_t bytes = length * sizeof(TCHAR);

    TCHAR* buffer = RC_HEAP_CALL(allocate_memory, bytes, false);
    copy_memory(buffer, s, bytes);

    STRING_BUILDER result = {
        .buffer = buffer,
        .length = length,
        .library = {
            .capacity = MAX(length, capacity),
    }
    };

    return result;
}

STRING_BUILDER RC_HEAP_FUNC(create_string_builder, size_t capacity)
{
    assert_debug(capacity);

    size_t bytes = capacity * sizeof(TCHAR);
    TCHAR* buffer = RC_HEAP_CALL(allocate_memory, bytes, false);
    buffer[0] = 0;

    STRING_BUILDER result = {
        .buffer = buffer,
        .length = 0,
        .library = {
            .capacity = capacity,
    }
    };

    return result;
}

bool RC_HEAP_FUNC(free_string_builder, STRING_BUILDER* string_builder)
{
    if (!string_builder) {
        return false;
    }
    if (!string_builder->buffer) {
        return false;
    }

    RC_HEAP_CALL(free_memory, string_builder->buffer);
    string_builder->buffer = NULL;
    string_builder->length = 0;
    string_builder->library.capacity = 0;

    return false;
}

