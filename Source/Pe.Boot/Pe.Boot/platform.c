#include "platform.h"


TEXT get_environment_variable(const TEXT* key)
{
    DWORD env_length = GetEnvironmentVariable(key->value, NULL, 0);
    if (!env_length) {
        return create_invalid_text();
    }

    TCHAR* env_value = allocate_string(env_length - 1);
    GetEnvironmentVariable(key->value, env_value, env_length);

    return wrap_text_with_length(env_value, env_length - 1, true);
}

bool set_environment_variable(const TEXT* key, const TEXT* value)
{
    return SetEnvironmentVariable(key->value, value->value);
}

