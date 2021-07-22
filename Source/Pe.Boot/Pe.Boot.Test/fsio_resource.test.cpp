#include "pch.h"

extern "C" {
#   include "../Pe.Boot/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(fsio_resource_test)
    {
    public:
        TEST_METHOD(create_file_resource_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("file"));

            FILE_RESOURCE actual1 = create_file_resource(&path1);
            FILE_RESOURCE actual2 = create_file_resource(&path1);

            Assert::IsTrue(is_enabled_file_resource(&actual1));
            Assert::IsFalse(is_enabled_file_resource(&actual2));

            Assert::IsTrue(close_file_resource(&actual1));
            Assert::IsFalse(close_file_resource(&actual2));
        }

        TEST_METHOD(open_file_resource_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("exists"));
            TEST_GET_PATH(path2, _T("not-exists"));

            TEST::create_empty_file(test_path1);

            FILE_RESOURCE actual1 = open_file_resource(&path1);
            FILE_RESOURCE actual2 = open_file_resource(&path2);

            Assert::IsTrue(is_enabled_file_resource(&actual1));
            Assert::IsFalse(is_enabled_file_resource(&actual2));

            Assert::IsTrue(close_file_resource(&actual1));
            Assert::IsFalse(close_file_resource(&actual2));
        }

        TEST_METHOD(open_or_create_file_resource_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("exists"));
            TEST_GET_PATH(path2, _T("not-exists"));

            TEST::create_empty_file(test_path1);

            FILE_RESOURCE actual1 = open_or_create_file_resource(&path1);
            FILE_RESOURCE actual2 = open_or_create_file_resource(&path2);

            Assert::IsTrue(is_enabled_file_resource(&actual1));
            Assert::IsTrue(is_enabled_file_resource(&actual2));

            Assert::IsTrue(close_file_resource(&actual1));
            Assert::IsTrue(close_file_resource(&actual2));
        }
    };
}
