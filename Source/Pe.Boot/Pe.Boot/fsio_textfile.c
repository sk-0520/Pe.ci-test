#include "common.h"
#include "fsio_textfile.h"
#include "fsio_resource.h"
#include "debug.h"


static const uint8_t library__unicode_utf8_bom[] = { 0xef, 0xbb, 0xbf };
static const uint8_t library__unicode_utf16le_bom[] = { 0xff, 0xef };

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
            bom.length = sizeof(library__unicode_utf8_bom);
            bom.value = library__unicode_utf8_bom;
            break;

        case FILE_ENCODING_UTF16LE:
            bom.length = sizeof(library__unicode_utf16le_bom);
            bom.value = library__unicode_utf16le_bom;
            break;

#endif
        default:
            break;
    }

    if (bom.value) {
        write_file_resource(file_resource, bom.value, bom.length);
    }
}

FILE_READER RC_FILE_FUNC(new_file_reader, const TEXT* path, FILE_ENCODING encoding)
{

}

bool RC_FILE_FUNC(free_file_reader, FILE_READER* file_reader)
{
    return RC_FILE_CALL(close_file_resource, &file_reader->resource);
}

bool is_enabled_file_reader(const FILE_READER* file_reader)
{
    if (!file_reader) {
        return false;
    }

    return is_enabled_file_resource(&file_reader->resource);
}

FILE_WRITER RC_FILE_FUNC(new_file_writer, const TEXT* path, FILE_ENCODING encoding, FILE_OPEN_MODE open_mode, FILE_WRITER_OPTIONS options)
{
    FILE_WRITER result;
    result.library.encoding = encoding;
    result.library.string_builder = RC_HEAP_CALL(create_string_builder, FILE_WRITER_BUFFER_SIZE);
    result.library.buffer_size = FILE_WRITER_BUFFER_SIZE;

    result.resource = RC_FILE_CALL(new_file_resource, path, FILE_ACCESS_MODE_READ | FILE_ACCESS_MODE_WRITE, FILE_SHARE_MODE_READ, open_mode, 0);
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
            assert_debug(false);
    }

    return result;
}

bool RC_FILE_FUNC(free_file_writer, FILE_WRITER* file_writer)
{
    flush_file_writer(file_writer, true);

    bool fr = RC_FILE_CALL(close_file_resource, &file_writer->resource);
    fr &= RC_HEAP_CALL(free_string_builder, &file_writer->library.string_builder);
    if (!fr) {
        return false;
    }
    file_writer->resource = create_invalid_file();

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
        if (file_writer->library.string_builder.list.length < file_writer->library.buffer_size) {
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
            MULTIBYTE_CHARACTER_RESULT mbcr = convert_to_multibyte_character(&text, MULTI_BYTE_CHARACTER_TYPE_UTF8);
            write_file_resource(&file_writer->resource, mbcr.buffer, mbcr.length);
            free_multibyte_character_result(&mbcr);
        }
        break;

        default:
            assert_debug(false);
    }
#else
#   error しらんがな
#endif

    flush_file_resource(&file_writer->resource);
    free_text(&text);
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

