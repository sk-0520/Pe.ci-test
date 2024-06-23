#include "platform.h"

TEXT RC_HEAP_FUNC(get_environment_variable, const TEXT* key, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    DWORD env_length = GetEnvironmentVariable(key->value, NULL, 0);
    if (!env_length) {
        return create_invalid_text();
    }

    TCHAR* env_value = RC_HEAP_CALL(allocate_string, (size_t)env_length - 1, memory_arena_resource);
    GetEnvironmentVariable(key->value, env_value, env_length);

    return wrap_text_with_length(env_value, (size_t)env_length - 1, true, memory_arena_resource);
}

bool set_environment_variable(const TEXT* key, const TEXT* value)
{
    return SetEnvironmentVariable(key->value, value->value);
}

TEXT RC_HEAP_FUNC(expand_environment_variable, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!is_enabled_text(text)) {
        return create_invalid_text();
    }

    DWORD buffer_size = ExpandEnvironmentStrings(text->value, NULL, 0);
    if (!buffer_size) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, buffer_size, memory_arena_resource);
    DWORD result = ExpandEnvironmentStrings(text->value, buffer, buffer_size);
    if (!result) {
        RC_HEAP_CALL(release_string, buffer, memory_arena_resource);
        return create_invalid_text();
    }

    return wrap_text_with_length(buffer, buffer_size, true, memory_arena_resource);
}
