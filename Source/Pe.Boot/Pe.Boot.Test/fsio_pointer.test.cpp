#include "pch.h"

extern "C" {
#   include "../Pe.Boot/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(fsio_pointer_test)
    {
    public:
        TEST_METHOD(create_file_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("file"));

            FILE_POINTER actual1 = create_file(&path1);
            FILE_POINTER actual2 = create_file(&path1);

            Assert::IsTrue(is_enabled_file(&actual1));
            Assert::IsFalse(is_enabled_file(&actual2));

            Assert::IsTrue(close_file(&actual1));
            Assert::IsFalse(close_file(&actual2));
        }

        TEST_METHOD(open_file_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("exists"));
            TEST_GET_PATH(path2, _T("not-exists"));

            TEST::create_empty_file(test_path1);

            FILE_POINTER actual1 = open_file(&path1);
            FILE_POINTER actual2 = open_file(&path2);

            Assert::IsTrue(is_enabled_file(&actual1));
            Assert::IsFalse(is_enabled_file(&actual2));

            Assert::IsTrue(close_file(&actual1));
            Assert::IsFalse(close_file(&actual2));
        }

        TEST_METHOD(open_or_create_file_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("exists"));
            TEST_GET_PATH(path2, _T("not-exists"));

            TEST::create_empty_file(test_path1);

            FILE_POINTER actual1 = open_or_create_file(&path1);
            FILE_POINTER actual2 = open_or_create_file(&path2);

            Assert::IsTrue(is_enabled_file(&actual1));
            Assert::IsTrue(is_enabled_file(&actual2));

            Assert::IsTrue(close_file(&actual1));
            Assert::IsTrue(close_file(&actual2));
        }
    };
}
