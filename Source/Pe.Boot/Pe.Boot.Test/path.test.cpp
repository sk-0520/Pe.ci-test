#include "pch.h"

extern "C" {
#   include "../Pe.Boot/path.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(path_test)
    {
    public:

        TEST_METHOD(get_parent_directory_path_test)
        {
            TEXT expected = wrap("C:\\dir");
            TEXT input = wrap("C:\\dir\\file");

            TEXT actual = get_parent_directory_path(&input);
            Assert::AreEqual(expected.value, actual.value);

            free_text(&actual);
        }

        TEST_METHOD(combine_path_test)
        {
            TEXT expected = wrap("C:\\dir\\file");
            TEXT input1 = wrap("C:\\dir");
            TEXT input2 = wrap("file");

            TEXT actual = combine_path(&input1, &input2);
            Assert::AreEqual(expected.value, actual.value);

            free_text(&actual);
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

            TEXT actual = join_path(&input1, input2, sizeof(input2) / sizeof(input2[0]));
            Assert::AreEqual(expected.value, actual.value);

            free_text(&actual);
        }

        TEST_METHOD(canonicalize_path_test)
        {
            TEXT input = wrap("C:\\dir\\file\\..\\x\\.\\file2");
            TCHAR expected[] = _T("C:\\dir\\x\\file2");
            TEXT actual = canonicalize_path(&input);

            Assert::AreEqual(expected, actual.value);

            free_text(&actual);
        }
    };
}
