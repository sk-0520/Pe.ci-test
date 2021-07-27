#pragma once
#include <windows.h>

#include "text.h"

typedef enum tag_ENCODE_TYPE
{
    STRINIG_ENCODING_SYSTEM,
    STRINIG_ENCODING_ASCII,
    STRINIG_ENCODING_UTF8,
    STRINIG_ENCODING_UTF16LE,
    STRINIG_ENCODING_UTF16BE,
    STRINIG_ENCODING_UTF32LE,
    STRINIG_ENCODING_UTF32BE,
} ENCODE_TYPE;

/// <summary>
/// エンコード結果。
/// </summary>
typedef struct tag_ENCODE_RESULT
{
    /// <summary>
    /// エンコードデータ格納バッファ。
    /// </summary>
    void* buffer;
    /// <summary>
    /// <c>buffer</c>のバイト単位の長さ。
    /// </summary>
    size_t bytes;
    /// <summary>
    /// <c>buffer</c>の最小要素幅。
    /// </summary>
    size_t width;
} ENCODE_RESULT;

//bool free_encode_result(ENCODE_RESULT* encode_result);

//ENCODE_RESULT encode_to_string(const TEXT* input, ENCODE_TYPE encode_type);


