#include "pch.h"

#include <fstream>
#include <filesystem>

extern "C" {
#   include "../Pe.Boot/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(fsio_test)
    {
    public:
        TEST_METHOD(is_directory_path_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(test_dir_path, _T("dir"));
            std::filesystem::create_directory(test_dir_path);

            TEST_GET_PATH(test_file_path, _T("file"));
            std::wofstream f(test_file_path);
            f.close();

            TEXT dir_path = wrap_text(test_dir_path.c_str());
            TEXT file_path = wrap_text(test_file_path.c_str());

            Assert::IsTrue(is_directory_path(&dir_path));
            Assert::IsFalse(is_directory_path(&file_path));
        }
    };
}
