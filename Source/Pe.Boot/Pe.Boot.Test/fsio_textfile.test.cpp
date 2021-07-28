#include "pch.h"

extern "C" {
#   include "../Pe.Boot/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(fsio_textfile_test)
    {
    public:
        TEST_METHOD(writer_native_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_NATIVE, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_NONE);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(free_file_writer(&file_writer));
        }

#ifdef _UNICODE
        TEST_METHOD(writer_utf8n_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_NONE);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(free_file_writer(&file_writer));
        }

        TEST_METHOD(writer_utf8bom_test)
        {
            TEST_INIT_DIR;
            TEST_GET_PATH(path, _T("path.txt"));

            FILE_WRITER file_writer = new_file_writer(&path, FILE_ENCODING_UTF8, FILE_OPEN_MODE_NEW, FILE_WRITER_OPTIONS_BOM);
            write_string_file_writer(&file_writer, _T("𩸽🍶"), true);

            Assert::IsTrue(free_file_writer(&file_writer));
        }
#endif
    };
}
