#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"




#ifdef _UNICODE

bool is_enabled_multibyte_character_result(const MULTIBYTE_CHARACTER_RESULT* mbcr)
{
    if (!mbcr) {
        return false;
    }

    if (!mbcr->buffer) {
        return false;
    }

    return true;
}

MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_RESOURCE* memory_resource)
{
    DWORD flags = 0;
    CHAR default_char = '?';

    int mc_length1 = WideCharToMultiByte(mbc_type, flags, input->value, (int)input->length, NULL, 0, &default_char, NULL);
    if (!mc_length1) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    uint8_t* buffer = RC_HEAP_CALL(allocate_raw_memory, mc_length1 * sizeof(uint8_t) + sizeof(uint8_t), true, memory_resource);
    int mc_length2 = WideCharToMultiByte(mbc_type, flags, input->value, (int)input->length, (LPSTR)buffer, mc_length1, &default_char, NULL);
    if (!mc_length2) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    if (mc_length1 != mc_length2) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    MULTIBYTE_CHARACTER_RESULT result = {
        .buffer = buffer,
        .length = mc_length2,
    };
    return result;
}

bool RC_HEAP_FUNC(release_multibyte_character_result, MULTIBYTE_CHARACTER_RESULT* mbcr, const MEMORY_RESOURCE* memory_resource)
{
    if (!mbcr) {
        return false;
    }
    if (!mbcr->buffer) {
        return false;
    }

    bool result = RC_HEAP_CALL(release_memory, mbcr->buffer, memory_resource);

    mbcr->buffer = NULL;
    mbcr->length = 0;

    return result;
}

TEXT RC_HEAP_FUNC(make_text_from_multibyte, const uint8_t* input, size_t length, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_RESOURCE* memory_resource)
{
    DWORD flags = 0;
    int wc_length1 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, NULL, 0);
    if (!wc_length1) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, wc_length1, memory_resource);
    int wc_length2 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, buffer, wc_length1);
    if (!wc_length2) {
        RC_HEAP_CALL(release_string, buffer, memory_resource);
        return create_invalid_text();
    }
    if (wc_length1 != wc_length2) {
        RC_HEAP_CALL(release_string, buffer, memory_resource);
        return create_invalid_text();
    }

    return wrap_text_with_length(buffer, wc_length2, true, memory_resource);
}


#endif // UNICODE
