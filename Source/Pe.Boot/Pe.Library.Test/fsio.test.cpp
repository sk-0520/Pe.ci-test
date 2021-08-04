#include "pch.h"

extern "C" {
#   include "../Pe.Library/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(fsio_test)
    {
    public:
        TEST_METHOD(is_directory_path_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(dir_path, _T("dir"));
            TEST.create_directory(test_dir_path);

            TEST_GET_PATH(file_path, _T("file"));
            TEST.create_empty_file(test_file_path);

            TEST_GET_PATH(unknown_path, _T("unknown"));

            Assert::IsTrue(is_directory_path(&dir_path));
            Assert::IsFalse(is_directory_path(&file_path));
            Assert::IsFalse(is_directory_path(&unknown_path));
        }

        TEST_METHOD(exists_file_path_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(dir_path, _T("dir"));
            TEST.create_directory(test_dir_path);

            TEST_GET_PATH(file_path, _T("file"));
            TEST.create_empty_file(test_file_path);

            TEST_GET_PATH(unknown_path, _T("unknown"));

            Assert::IsFalse(exists_file_path(&dir_path));
            Assert::IsTrue(exists_file_path(&file_path));
            Assert::IsFalse(exists_file_path(&unknown_path));
        }

        TEST_METHOD(exists_directory_path_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(dir_path, _T("dir"));
            TEST.create_directory(test_dir_path);

            TEST_GET_PATH(file_path, _T("file"));
            TEST.create_empty_file(test_file_path);

            TEST_GET_PATH(unknown_path, _T("unknown"));

            Assert::IsTrue(exists_directory_path(&dir_path));
            Assert::IsFalse(exists_directory_path(&file_path));
            Assert::IsFalse(exists_directory_path(&unknown_path));
        }

    };
}
