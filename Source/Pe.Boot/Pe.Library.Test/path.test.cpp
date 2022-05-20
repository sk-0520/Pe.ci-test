#include "pch.h"

extern "C" {
#   include "../Pe.Library/path.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(path_test)
    {
    public:

        TEST_METHOD(get_parent_directory_path_test)
        {
            auto tests = {
                DATA(wrap("C:\\dir\\file"), wrap("C:\\dir\\file\\")),
                DATA(wrap("C:\\dir"), wrap("C:\\dir\\file")),
                DATA(wrap("C:\\"), wrap("C:\\dir")),
                DATA(wrap(""), wrap("abc")),
                DATA(wrap("C:/"), wrap("C:/")),
                DATA(wrap("C:\\"), wrap("C:\\")),
                DATA(wrap(""), wrap("C:")),
                DATA(wrap(""), wrap("C")),
            };
            for (auto test : tests) {
                TEXT arg1 = std::get<0>(test.inputs);

                TEXT actual = get_parent_directory_path(&arg1, DEFAULT_MEMORY);
                Assert::AreEqual(test.expected.value, actual.value);

                release_text(&actual);
            }
        }

        TEST_METHOD(combine_path_test)
        {
            auto tests = {
                DATA(wrap("C:\\dir\\file"), wrap("C:\\dir"), wrap("file")),
                DATA(wrap("a\\b"), wrap("a"), wrap("b")),
                DATA(wrap("a\\b"), wrap("a\\"), wrap("b\\")),
            };
            for (auto test : tests) {
                TEXT arg1 = std::get<0>(test.inputs);
                TEXT arg2 = std::get<1>(test.inputs);

                TEXT actual = combine_path(&arg1, &arg2, DEFAULT_MEMORY);
                Assert::AreEqual(test.expected.value, actual.value);

                release_text(&actual);
            }
        }

        TEST_METHOD(join_path_test)
        {
            TEXT input1 = wrap("C:\\");
            TEXT input2[] = {
                wrap("dir1\\"),
                wrap("dir2"),
                wrap("."),
                wrap(".."),
                wrap("dir2+"),
                wrap(""),
            };
            TEXT expected = wrap("C:\\dir1\\dir2+");

            TEXT actual = join_path(&input1, input2, sizeof(input2) / sizeof(input2[0]), DEFAULT_MEMORY);
            Assert::AreEqual(expected.value, actual.value);

            release_text(&actual);
        }

        TEST_METHOD(canonicalize_path_test)
        {
            TEXT input = wrap("C:\\dir\\file\\..\\x\\.\\file2");
            TCHAR expected[] = _T("C:\\dir\\x\\file2");
            TEXT actual = canonicalize_path(&input, DEFAULT_MEMORY);

            Assert::AreEqual(expected, actual.value);

            release_text(&actual);
        }
    };
}
