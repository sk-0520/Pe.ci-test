#include <Windows.h>

#include "debug.h"
#include "tcharacter.h"
#include "text.h"

/// <summary>
/// 小文字/大文字テキストに変換。
/// </summary>
/// <param name="to_lower">小文字にするか。</param>
/// <param name="text"></param>
/// <param name="memory_arena_resource"></param>
/// <returns>解放が必要。</returns>
static TEXT RC_HEAP_FUNC(to_lower_or_upper_text, bool to_lower, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    size_t length = get_text_length(text);
    TCHAR* edit_characters = RC_HEAP_CALL(clone_string_with_length, text->value, length, memory_arena_resource);

    const TCHAR(*to_character_func)(TCHAR) = to_lower
        ? to_lower_character
        : to_upper_character
        ;

    for (size_t i = 0; i < length; i++) {
        edit_characters[i] = to_character_func(edit_characters[i]);
    }

    return wrap_text_with_length(edit_characters, length, true, memory_arena_resource);
}

TEXT RC_HEAP_FUNC(to_lower_text, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return RC_HEAP_CALL(to_lower_or_upper_text, true, text, memory_arena_resource);
}

TEXT RC_HEAP_FUNC(to_upper_text, const TEXT* text, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    return RC_HEAP_CALL(to_lower_or_upper_text, false, text, memory_arena_resource);
}

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

MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
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

    uint8_t* buffer = RC_HEAP_CALL(allocate_raw_memory, mc_length1 * sizeof(uint8_t) + sizeof(uint8_t), true, memory_arena_resource);
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

bool RC_HEAP_FUNC(release_multibyte_character_result, MULTIBYTE_CHARACTER_RESULT* mbcr, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    if (!mbcr) {
        return false;
    }
    if (!mbcr->buffer) {
        return false;
    }

    bool result = RC_HEAP_CALL(release_memory, mbcr->buffer, memory_arena_resource);

    mbcr->buffer = NULL;
    mbcr->length = 0;

    return result;
}

TEXT RC_HEAP_FUNC(make_text_from_multibyte, const uint8_t* input, size_t length, MULTIBYTE_CHARACTER_TYPE mbc_type, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    DWORD flags = 0;
    int wc_length1 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, NULL, 0);
    if (!wc_length1) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, wc_length1, memory_arena_resource);
    int wc_length2 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, buffer, wc_length1);
    if (!wc_length2) {
        RC_HEAP_CALL(release_string, buffer, memory_arena_resource);
        return create_invalid_text();
    }
    if (wc_length1 != wc_length2) {
        RC_HEAP_CALL(release_string, buffer, memory_arena_resource);
        return create_invalid_text();
    }

    return wrap_text_with_length(buffer, wc_length2, true, memory_arena_resource);
}


#endif // UNICODE
