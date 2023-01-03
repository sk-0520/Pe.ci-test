#include "platform.h"

TEXT RC_HEAP_FUNC(get_environment_variable, const TEXT* key, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    //new_stack_or_heap_array(env_key_buffer, env_key_array, TCHAR, key->length + 1, 128, memory_arena_resource);
    //copy_memory(env_key_buffer, key->value, sizeof(TCHAR) * key->length + 1);
    //env_key_buffer[key->length + 1] = '\0';
    DWORD env_length = GetEnvironmentVariable(key->value, NULL, 0);
    //DWORD env_length = GetEnvironmentVariable(env_key_buffer, NULL, 0);
    if (!env_length) {
        //release_stack_or_heap_array(env_key_array);
        return create_invalid_text();
    }

    TCHAR* env_value = RC_HEAP_CALL(allocate_string, (size_t)env_length - 1, memory_arena_resource);
    GetEnvironmentVariable(key->value, env_value, env_length);
    //GetEnvironmentVariable(env_key_buffer, env_value, env_length);

    //release_stack_or_heap_array(env_key_array);

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
