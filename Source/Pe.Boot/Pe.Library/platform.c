#include "platform.h"


TEXT RC_HEAP_FUNC(get_environment_variable, const TEXT* key)
{
    DWORD env_length = GetEnvironmentVariable(key->value, NULL, 0);
    if (!env_length) {
        return create_invalid_text();
    }

    TCHAR* env_value = RC_HEAP_CALL(allocate_string, (size_t)env_length - 1, DEFAULT_MEMORY);
    GetEnvironmentVariable(key->value, env_value, env_length);

    return wrap_text_with_length(env_value, (size_t)env_length - 1, true, DEFAULT_MEMORY);
}

bool set_environment_variable(const TEXT* key, const TEXT* value)
{
    return SetEnvironmentVariable(key->value, value->value);
}

TEXT RC_HEAP_FUNC(expand_environment_variable, const TEXT* text)
{
    if (!is_enabled_text(text)) {
        return create_invalid_text();
    }

    DWORD buffer_size = ExpandEnvironmentStrings(text->value, NULL, 0);
    if (!buffer_size) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, buffer_size, DEFAULT_MEMORY);
    DWORD result = ExpandEnvironmentStrings(text->value, buffer, buffer_size);
    if (!result) {
        RC_HEAP_CALL(free_string, buffer, DEFAULT_MEMORY);
        return create_invalid_text();
    }

    return wrap_text_with_length(buffer, buffer_size, true, DEFAULT_MEMORY);
}
