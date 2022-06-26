#include "error.h"

void set_last_error_code(ERROR_CODE error_code)
{
    SetLastError(error_code.code);
}

ERROR_CODE get_last_error_code()
{
    return (ERROR_CODE)
    {
        .code = GetLastError(),
    };
}

TEXT  RC_HEAP_FUNC(get_error_message, ERROR_CODE error_code, const MEMORY_RESOURCE* memory_resource)
{
    LPVOID message = NULL;
    size_t message_length = FormatMessage(
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL,
        error_code.code,
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (LPTSTR)&message,
        0,
        NULL
    );
    if (!message_length) {
        return create_invalid_text();
    }

    TEXT result = RC_HEAP_CALL(new_text_with_length, message, message_length, memory_resource);

    LocalFree(message);

    return result;
}
