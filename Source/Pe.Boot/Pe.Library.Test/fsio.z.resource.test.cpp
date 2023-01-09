#include "pch.h"

extern "C" {
#   include "../Pe.Library/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(fsio_resource_test)
    {
    public:
        TEST_METHOD(create_file_resource_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("file"));

            FILE_RESOURCE actual1 = create_file_resource(&path1, DEFAULT_MEMORY_ARENA);
            FILE_RESOURCE actual2 = create_file_resource(&path1, DEFAULT_MEMORY_ARENA);

            Assert::IsTrue(is_enabled_file_resource(&actual1));
            Assert::IsFalse(is_enabled_file_resource(&actual2));

            Assert::IsTrue(release_file_resource(&actual1));
            Assert::IsFalse(release_file_resource(&actual2));
        }

        TEST_METHOD(open_file_resource_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("exists"));
            TEST_GET_PATH(path2, _T("not-exists"));

            TEST.create_empty_file(test_path1);

            FILE_RESOURCE actual1 = open_file_resource(&path1, DEFAULT_MEMORY_ARENA);
            FILE_RESOURCE actual2 = open_file_resource(&path2, DEFAULT_MEMORY_ARENA);

            Assert::IsTrue(is_enabled_file_resource(&actual1));
            Assert::IsFalse(is_enabled_file_resource(&actual2));

            Assert::IsTrue(release_file_resource(&actual1));
            Assert::IsFalse(release_file_resource(&actual2));
        }

        TEST_METHOD(open_or_create_file_resource_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("exists"));
            TEST_GET_PATH(path2, _T("not-exists"));

            TEST.create_empty_file(test_path1);

            FILE_RESOURCE actual1 = open_or_create_file_resource(&path1, DEFAULT_MEMORY_ARENA);
            FILE_RESOURCE actual2 = open_or_create_file_resource(&path2, DEFAULT_MEMORY_ARENA);

            Assert::IsTrue(is_enabled_file_resource(&actual1));
            Assert::IsTrue(is_enabled_file_resource(&actual2));

            Assert::IsTrue(release_file_resource(&actual1));
            Assert::IsTrue(release_file_resource(&actual2));
        }

        TEST_METHOD(write_read_file_resource_test)
        {
            // 細かい処理は成功する前提のテスト

            TEST_INIT_DIR;

            TEST_GET_PATH(path, _T("file"));

            auto fr1 = create_file_resource(&path, DEFAULT_MEMORY_ARENA);
            uint8_t write_values[] = {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            };
            ssize_t write_length = write_file_resource(&fr1, write_values, sizeof(write_values));
            Assert::AreEqual((ssize_t)sizeof(write_values), write_length);
            release_file_resource(&fr1);
            Assert::IsFalse(is_enabled_file_resource(&fr1));

            auto fr2 = open_file_resource(&path, DEFAULT_MEMORY_ARENA);
            Assert::IsTrue(is_enabled_file_resource(&fr2));

            uint8_t actual_values[sizeof(write_values)];
            ssize_t read_length = read_file_resource(&fr2, actual_values, sizeof(actual_values));
            Assert::AreEqual((ssize_t)sizeof(write_values), read_length);
            for (size_t i = 0; i < sizeof(actual_values); i++) {
                Assert::AreEqual(actual_values[i], write_values[i]);
            }

            release_file_resource(&fr2);
        }
    };
}
