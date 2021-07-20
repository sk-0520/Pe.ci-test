#include "platform.h"


#ifdef RES_CHECK
TEXT rc_heap__get_environment_variable(const TEXT* key, MEM_CHECK_FUNC_ARGS)
#else
TEXT get_environment_variable(const TEXT* key)
#endif
{
    DWORD env_length = GetEnvironmentVariable(key->value, NULL, 0);
    if (!env_length) {
        return create_invalid_text();
    }

    TCHAR* env_value =
#ifdef RES_CHECK
        rc_heap__allocate_string(env_length - 1, MEM_CHECK_CALL_ARGS);
#else
        allocate_string(env_length - 1);
#endif
    GetEnvironmentVariable(key->value, env_value, env_length);

    return wrap_text_with_length(env_value, env_length - 1, true);
}

bool set_environment_variable(const TEXT* key, const TEXT* value)
{
    return SetEnvironmentVariable(key->value, value->value);
}

