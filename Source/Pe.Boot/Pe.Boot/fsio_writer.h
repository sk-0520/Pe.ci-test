#pragma once
#include <stdarg.h>

#include "fsio_resource.h"
#include "string_builder.h"


#define FILE_WRITER_BUFFER_SIZE (1024 * 4)
#define FILE_WRITER_BUILDER_CAPACITY ((FILE_WRITER_BUFFER_SIZE / 10) * 12)


typedef enum tag_FILE_WRITER_OPTIONS
{
    FILE_WRITER_OPTIONS_NONE = 0x00000000,
    FILE_WRITER_OPTIONS_BOM = 0x00000001,
} FILE_WRITER_OPTIONS;

typedef struct tag_FILE_WRITER
{
    FILE_RESOURCE resource;
    struct
    {
        FILE_ENCODING encoding;
        STRING_BUILDER string_builder;
        /// <summary>
        /// このサイズを超過するまでため込んでおく。
        /// </summary>
        size_t buffer_size;
    } library;
} FILE_WRITER;

/// <summary>
/// ファイル書き込み処理の生成。
/// </summary>
/// <param name="path"></param>
/// <param name="encoding"></param>
/// <param name="open_mode"></param>
/// <param name="options"></param>
/// <returns>解放が必要。</returns>
FILE_WRITER RC_FILE_FUNC(new_file_writer, const TEXT* path, FILE_ENCODING encoding, FILE_OPEN_MODE open_mode, FILE_WRITER_OPTIONS options);
#if RES_CHECK
#   define new_file_writer(path, encoding, open_mode, options) RC_FILE_WRAP(new_file_writer, (path), (encoding), (open_mode), (options))
#endif

/// <summary>
/// ファイル書き込み処理の解放。
/// </summary>
/// <param name="file_writer"></param>
/// <returns></returns>
bool RC_FILE_FUNC(free_file_writer, FILE_WRITER* file_writer);
#if RES_CHECK
#   define free_file_writer(file_writer) RC_FILE_WRAP(free_file_writer, (file_writer))
#endif

/// <summary>
/// バッファ内のファイル書き込み処理を実施。
/// </summary>
/// <param name="file_writer"></param>
/// <param name="force"></param>
void flush_file_buffer(FILE_WRITER* file_writer, bool force);

void RC_FILE_FUNC(write_string_file_writer, FILE_WRITER* file_writer, const TCHAR* s, bool newline);
#if RES_CHECK
#   define write_string_file_writer(file_writer, s, newline) RC_FILE_WRAP(write_string_file_writer, (file_writer), (s), (newline))
#endif
void RC_FILE_FUNC(write_text_file_writer, FILE_WRITER* file_writer, const TEXT* text, bool newline);
#if RES_CHECK
#   define write_text_file_writer(file_writer, text, newline) RC_FILE_WRAP(write_text_file_writer, (file_writer), (text), (newline))
#endif
void RC_FILE_FUNC(write_character_file_writer, FILE_WRITER* file_writer, TCHAR c, bool newline);
#if RES_CHECK
#   define write_character_file_writer(file_writer, c, newline) RC_FILE_WRAP(write_character_file_writer, (file_writer), (c), (newline))
#endif
void RC_FILE_FUNC(write_int_file_writer, FILE_WRITER* file_writer, ssize_t value, bool newline);
#if RES_CHECK
#   define write_int_file_writer(file_writer, value, newline) RC_FILE_WRAP(write_int_file_writer, (file_writer), (value), (newline))
#endif
void RC_FILE_FUNC(write_uint_file_writer, FILE_WRITER* file_writer, size_t value, bool newline);
#if RES_CHECK
#   define write_uint_file_writer(file_writer, value, newline) RC_FILE_WRAP(write_uint_file_writer, (file_writer), (value), (newline))
#endif
void RC_FILE_FUNC(write_bool_file_writer, FILE_WRITER* file_writer, bool value, bool newline);
#if RES_CHECK
#   define write_bool_file_writer(file_writer, value, newline) RC_FILE_WRAP(write_bool_file_writer, (file_writer), (value), (newline))
#endif
void RC_FILE_FUNC(write_pointer_file_writer, FILE_WRITER* file_writer, const void* pointer, bool newline);
#if RES_CHECK
#   define write_pointer_file_writer(file_writer, value, newline) RC_FILE_WRAP(write_pointer_file_writer, (file_writer), (value), (newline))
#endif

void RC_HEAP_FUNC(write_vformat_file_writer, FILE_WRITER* file_writer, const TEXT* format, va_list ap);
#if RES_CHECK
#   define write_vformat_file_writer(file_writer, format, ...) RC_HEAP_WRAP(write_vformat_file_writer, (file_writer), (format), (ap))
#endif
void RC_FILE_FUNC(write_format_file_writer, FILE_WRITER* file_writer, const TEXT* format, ...);
#if RES_CHECK
#   define write_format_file_writer(file_writer, format, ...) RC_FILE_WRAP(write_format_file_writer, (file_writer), (format), __VA_ARGS__)
#endif
