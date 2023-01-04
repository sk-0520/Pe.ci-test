#include "common.h"
#include "fsio.z.textfile.h"
#include "fsio.z.resource.h"
#include "debug.h"


static const uint8_t library_unicode_utf8_bom[] = { 0xef, 0xbb, 0xbf };
static const uint8_t library_unicode_utf16le_bom[] = { 0xff, 0xef };

static void write_bom_if_unicode(const FILE_RESOURCE* file_resource, FILE_ENCODING encoding)
{
    struct
    {
        size_t length;
        const uint8_t* value;
    } bom = {
        .length = 0,
        .value = NULL,
    };

    switch (encoding) {
#ifdef _UNICODE
        case FILE_ENCODING_UTF8:
            bom.length = sizeof(library_unicode_utf8_bom);
            bom.value = library_unicode_utf8_bom;
            break;

        case FILE_ENCODING_UTF16LE:
            bom.length = sizeof(library_unicode_utf16le_bom);
            bom.value = library_unicode_utf16le_bom;
            break;

#endif
        default:
            break;
    }

    if (bom.value) {
        write_file_resource(file_resource, bom.value, bom.length);
    }
}

FILE_READER RC_FILE_FUNC(new_file_reader, const TEXT* path, FILE_ENCODING encoding, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    FILE_READER result = {
        .has_bom = false,
        .resource = RC_FILE_CALL(new_file_resource, path, FILE_ACCESS_MODE_READ, FILE_SHARE_MODE_READ, FILE_OPEN_MODE_OPEN, 0, memory_arena_resource),
        .library = {
            .encoding = encoding,
        },
    };

    return result;
}

bool RC_FILE_FUNC(release_file_reader, FILE_READER* file_reader)
{
    return RC_FILE_CALL(release_file_resource, &file_reader->resource);
}

bool is_enabled_file_reader(const FILE_READER* file_reader)
{
    if (!file_reader) {
        return false;
    }

    return is_enabled_file_resource(&file_reader->resource);
}

static size_t get_bom_info(FILE_READER* file_reader, uint8_t* buffer, size_t length)
{
    const uint8_t* bom = NULL;
    size_t bom_length = 0;

    switch (file_reader->library.encoding) {
        case FILE_ENCODING_NATIVE:
            break;

#ifdef _UNICODE
        case FILE_ENCODING_UTF8:
            bom = library_unicode_utf8_bom;
            bom_length = sizeof(library_unicode_utf8_bom);
            break;

        case FILE_ENCODING_UTF16LE:
            bom = library_unicode_utf16le_bom;
            bom_length = sizeof(library_unicode_utf16le_bom);
            break;
#endif
        default:
            assert(false);
    }

    if (bom_length <= length) {
        if (!compare_memory(bom, buffer, bom_length)) {
            return bom_length;
        }
    }

    return 0;
}

TEXT RC_FILE_FUNC(read_content_file_reader, FILE_READER* file_reader)
{
    DATA_INT64 file_size = get_size_file_resource(&file_reader->resource);
    if (file_size.plain < 0) {
        return create_invalid_text();
    }
    if (!file_size.plain) {
        return new_empty_text(file_reader->resource.library.memory_arena_resource);
    }

    //TODO: x86は32bit値が限界
    size_t file_length = (size_t)file_size.plain;

    const MEMORY_ARENA_RESOURCE* memory_arena_resource = file_reader->resource.library.memory_arena_resource;

    size_t total_read_length = 0;
    uint8_t* buffer = RC_HEAP_CALL(allocate_raw_memory, file_length, false, memory_arena_resource);
    uint8_t read_buffer[FILE_READER_BUFFER_SIZE];

    seek_begin_file_resource(&file_reader->resource);

    do {
        ssize_t read_length = read_file_resource(&file_reader->resource, read_buffer, sizeof(read_buffer));
        if (read_length < 0) {
            RC_HEAP_CALL(release_memory, buffer, memory_arena_resource);
            return create_invalid_text();
        }
        if (read_length) {
            copy_memory(buffer + total_read_length, read_buffer, (size_t)read_length);
            total_read_length += read_length;
        }
        // 0 完了はいいんじゃなかろうかと。
    } while (total_read_length != file_length);

    size_t skip = get_bom_info(file_reader, buffer, file_length);

    uint8_t* conv_buffer = buffer + skip;
    size_t conv_buffer_length = total_read_length - skip;

#ifndef _UNICODE
#   error むりのすけ
#endif

    switch (file_reader->library.encoding) {
        case FILE_ENCODING_NATIVE:
        case FILE_ENCODING_UTF16LE:
            return wrap_text_with_length((TCHAR*)conv_buffer, conv_buffer_length / sizeof(TCHAR), true, memory_arena_resource);

        case FILE_ENCODING_UTF8:
        {
            TEXT text = RC_HEAP_CALL(make_text_from_multibyte, conv_buffer, conv_buffer_length, MULTI_BYTE_CHARACTER_TYPE_UTF8, memory_arena_resource);
            release_memory(buffer, memory_arena_resource);
            return text;
        }

        default:
            assert(false);
    }

    return create_invalid_text();
}

FILE_WRITER RC_FILE_FUNC(new_file_writer, const TEXT* path, FILE_ENCODING encoding, FILE_OPEN_MODE open_mode, FILE_WRITER_OPTIONS options, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
{
    FILE_WRITER result = {
        .resource = RC_FILE_CALL(new_file_resource, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, open_mode, 0, memory_arena_resource),
        .library = {
            .encoding = encoding,
            .string_builder = RC_HEAP_CALL(new_string_builder, FILE_WRITER_BUFFER_SIZE, memory_arena_resource),
            .buffer_size = FILE_WRITER_BUFFER_SIZE,
        }
    };

    if (!is_enabled_file_resource(&result.resource)) {
        return result;
    }

    switch (open_mode) {
        case FILE_OPEN_MODE_NEW:
        case FILE_OPEN_MODE_TRUNCATE:
            if ((options & FILE_WRITER_OPTIONS_BOM) == FILE_WRITER_OPTIONS_BOM) {
                write_bom_if_unicode(&result.resource, result.library.encoding);
            }
            break;

        case FILE_OPEN_MODE_OPEN_OR_CREATE:
        case FILE_OPEN_MODE_OPEN:
            if ((options & FILE_WRITER_OPTIONS_BOM) == FILE_WRITER_OPTIONS_BOM) {
                DATA_INT64 pos = get_position_file_resource(&result.resource);
                if (!pos.plain) {
                    write_bom_if_unicode(&result.resource, result.library.encoding);
                }
            }
            break;

        default:
            assert(false);
    }

    return result;
}

FILE_WRITER create_invalid_file_writer()
{
    FILE_WRITER result;
    set_memory(&result, 0, sizeof(result));
    result.resource = create_invalid_file_resource();
    result.library.string_builder.library.list.items = NULL;
    result.library.string_builder.library.list.length = 0;
    result.library.string_builder.library.list.library.capacity_bytes = 0;

    return result;
}


bool RC_FILE_FUNC(release_file_writer, FILE_WRITER* file_writer)
{
    flush_file_writer(file_writer, true);

    bool fr = RC_FILE_CALL(release_file_resource, &file_writer->resource);
    fr &= RC_HEAP_CALL(release_string_builder, &file_writer->library.string_builder);
    if (!fr) {
        return false;
    }
    file_writer->resource = create_invalid_file_resource();

    return true;
}

bool is_enabled_file_writer(const FILE_WRITER* file_writer)
{
    if (!file_writer) {
        return false;
    }

    return is_enabled_file_resource(&file_writer->resource);
}

void flush_file_writer(FILE_WRITER* file_writer, bool force)
{
    if (!force) {
        if (file_writer->library.string_builder.library.list.length < file_writer->library.buffer_size) {
            return;
        }
    }

    TEXT text = build_text_string_builder(&file_writer->library.string_builder);
#ifdef _UNICODE
    switch (file_writer->library.encoding) {
        case FILE_ENCODING_NATIVE:
        case FILE_ENCODING_UTF16LE:
        {
            write_file_resource(&file_writer->resource, text.value, text.length * sizeof(TCHAR));
        }
        break;

        case FILE_ENCODING_UTF8:
        {
            MULTIBYTE_CHARACTER_RESULT mbcr = convert_to_multibyte_character(&text, MULTI_BYTE_CHARACTER_TYPE_UTF8, file_writer->resource.library.memory_arena_resource);
            write_file_resource(&file_writer->resource, mbcr.buffer, mbcr.length);
            release_multibyte_character_result(&mbcr, file_writer->resource.library.memory_arena_resource);
        }
        break;

        default:
            assert(false);
    }
#else
#   error しらんがな
#endif

    flush_file_resource(&file_writer->resource);
    release_text(&text);
    clear_builder(&file_writer->library.string_builder);
}

void write_string_file_writer(FILE_WRITER* file_writer, const TCHAR* s, bool newline)
{
    append_builder_string(&file_writer->library.string_builder, s, newline);

    flush_file_writer(file_writer, false);
}
void write_text_file_writer(FILE_WRITER* file_writer, const TEXT* text, bool newline)
{
    append_builder_text(&file_writer->library.string_builder, text, newline);

    flush_file_writer(file_writer, false);
}
void write_character_file_writer(FILE_WRITER* file_writer, TCHAR c, bool newline)
{
    append_builder_character(&file_writer->library.string_builder, c, newline);

    flush_file_writer(file_writer, false);
}
void write_int_file_writer(FILE_WRITER* file_writer, ssize_t value, bool newline)
{
    append_builder_int(&file_writer->library.string_builder, value, newline);

    flush_file_writer(file_writer, false);
}
void write_uint_file_writer(FILE_WRITER* file_writer, size_t value, bool newline)
{
    append_builder_uint(&file_writer->library.string_builder, value, newline);

    flush_file_writer(file_writer, false);
}
void write_bool_file_writer(FILE_WRITER* file_writer, bool value, bool newline)
{
    append_builder_bool(&file_writer->library.string_builder, value, newline);

    flush_file_writer(file_writer, false);
}
void write_pointer_file_writer(FILE_WRITER* file_writer, const void* pointer, bool newline)
{
    append_builder_pointer(&file_writer->library.string_builder, pointer, newline);

    flush_file_writer(file_writer, false);
}

void write_vformat_file_writer(FILE_WRITER* file_writer, const TEXT* format, va_list ap)
{
    append_builder_vformat(&file_writer->library.string_builder, format, ap);

    flush_file_writer(file_writer, false);
}
void write_format_file_writer(FILE_WRITER* file_writer, const TEXT* format, ...)
{
    va_list ap;
    va_start(ap, format);

    write_vformat_file_writer(file_writer, format, ap);

    va_end(ap);
}

