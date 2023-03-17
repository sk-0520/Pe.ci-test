#pragma once
#include <stdarg.h>

#include "fsio.z.resource.h"
#include "string_builder.h"


#define FILE_READER_BUFFER_SIZE (1024 * 2)

#define FILE_WRITER_BUFFER_SIZE (1024 * 4)
#define FILE_WRITER_BUILDER_CAPACITY ((FILE_WRITER_BUFFER_SIZE / 10) * 12)

/// <summary>
/// テキストファイル読み込み処理。
/// </summary>
typedef struct tag_FILE_READER
{
    FILE_RESOURCE resource;
    /// <summary>
    /// BOMを読み込んだか。
    /// </summary>
    bool has_bom;
    struct
    {
        FILE_ENCODING encoding;
    } library;
} FILE_READER;

typedef enum tag_FILE_WRITER_OPTIONS
{
    FILE_WRITER_OPTIONS_NONE = 0x00000000,
    FILE_WRITER_OPTIONS_BOM = 0x00000001,
} FILE_WRITER_OPTIONS;

/// <summary>
/// テキストファイル書き込み処理。
/// </summary>
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
/// ファイル読み取り処理の生成。
/// </summary>
/// <param name="path"></param>
/// <returns><c>release_file_reader</c>による解放が必要。</returns>
FILE_READER RC_FILE_FUNC(new_file_reader, const TEXT* path, FILE_ENCODING encoding, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define new_file_reader(path, encoding, memory_arena_resource) RC_FILE_WRAP(new_file_reader, (path), (encoding), memory_arena_resource)
#endif


bool RC_FILE_FUNC(release_file_reader, FILE_READER* file_reader);
#if RES_CHECK
#   define release_file_reader(file_reader) RC_FILE_WRAP(release_file_reader, (file_reader))
#endif

/// <summary>
/// テキストファイル読み込み処理の生成。
/// </summary>
/// <param name="file_reader"></param>
/// <returns></returns>
bool is_enabled_file_reader(const FILE_READER* file_reader);

/// <summary>
/// テキストファイルの一括取得。
/// </summary>
/// <param name="file_reader"></param>
/// <returns><c>release_file_reader</c>による解放が必要。</returns>
TEXT RC_FILE_FUNC(read_content_file_reader, FILE_READER* file_reader);
#if RES_CHECK
#   define read_content_file_reader(file_reader) RC_FILE_WRAP(read_content_file_reader, (file_reader))
#endif


/// <summary>
/// テキストファイル書き込み処理の生成。
/// </summary>
/// <param name="path"></param>
/// <param name="encoding"></param>
/// <param name="open_mode"></param>
/// <param name="options"></param>
/// <returns><c>release_file_writer</c>による解放が必要。</returns>
FILE_WRITER RC_FILE_FUNC(new_file_writer, const TEXT* path, FILE_ENCODING encoding, FILE_OPEN_MODE open_mode, FILE_WRITER_OPTIONS options, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define new_file_writer(path, encoding, open_mode, options, memory_arena_resource) RC_FILE_WRAP(new_file_writer, (path), (encoding), (open_mode), (options), memory_arena_resource)
#endif

/// <summary>
/// 無効なテキスト書き込み処理の生成。
/// </summary>
/// <returns></returns>
FILE_WRITER create_invalid_file_writer(void);

/// <summary>
/// テキストファイル書き込み処理の解放。
/// </summary>
/// <param name="file_writer"></param>
/// <returns></returns>
bool RC_FILE_FUNC(release_file_writer, FILE_WRITER* file_writer);
#if RES_CHECK
#   define release_file_writer(file_writer) RC_FILE_WRAP(release_file_writer, (file_writer))
#endif

/// <summary>
/// ファイル書き込み処理は有効か。
/// </summary>
/// <param name="file_writer"></param>
/// <returns></returns>
bool is_enabled_file_writer(const FILE_WRITER* file_writer);

/// <summary>
/// バッファ内のファイル書き込み処理を実施。
/// </summary>
/// <param name="file_writer"></param>
/// <param name="force"></param>
void flush_file_writer(FILE_WRITER* file_writer, bool force);

void write_string_file_writer(FILE_WRITER* file_writer, const TCHAR* s, bool newline);
void write_text_file_writer(FILE_WRITER* file_writer, const TEXT* text, bool newline);
void write_character_file_writer(FILE_WRITER* file_writer, TCHAR c, bool newline);
void write_int_file_writer(FILE_WRITER* file_writer, ssize_t value, bool newline);
void write_uint_file_writer(FILE_WRITER* file_writer, size_t value, bool newline);
void write_bool_file_writer(FILE_WRITER* file_writer, bool value, bool newline);
void write_pointer_file_writer(FILE_WRITER* file_writer, const void* pointer, bool newline);

void write_vformat_file_writer(FILE_WRITER* file_writer, const TEXT* format, va_list ap);
void write_format_file_writer(FILE_WRITER* file_writer, const TEXT* format, ...);
