#include "pch.h"

extern "C" {
#   include "../Pe.Library/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(fsio_textfile_test)
    {
    public:
        TEST_METHOD(writer_native_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_NATIVE, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_NONE, DEFAULT_MEMORY_ARENA);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(release_file_writer(&file_writer));
        }

#ifdef _UNICODE
        TEST_METHOD(writer_utf8n_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_NONE, DEFAULT_MEMORY_ARENA);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(release_file_writer(&file_writer));
        }

        TEST_METHOD(writer_utf8bom_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_BOM, DEFAULT_MEMORY_ARENA);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(release_file_writer(&file_writer));
        }
#endif
    };
}
