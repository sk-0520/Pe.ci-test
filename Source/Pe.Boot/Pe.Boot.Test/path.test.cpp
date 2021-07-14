#include "pch.h"

extern "C" {
#   include "../Pe.Boot/path.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(pathTest)
    {
    public:

        TEST_METHOD(getParentDirectoryPathTest)
        {
            TEXT expected = wrap("C:\\dir");
            TEXT input = wrap("C:\\dir\\file");

            TEXT actual = getParentDirectoryPath2(&input);
            Assert::AreEqual(expected.value, actual.value);

            freeText(&actual);
        }

        TEST_METHOD(combinePathTest)
        {
            TEXT expected = wrap("C:\\dir\\file");
            TEXT input1 = wrap("C:\\dir");
            TEXT input2 = wrap("file");

            TEXT actual = combinePath2(&input1, &input2);
            Assert::AreEqual(expected.value, actual.value);

            freeText(&actual);
        }

        TEST_METHOD(joinPathTest)
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

            TEXT actual = joinPath(&input1, input2, sizeof(input2) / sizeof(input2[0]));
            Assert::AreEqual(expected.value, actual.value);

            freeText(&actual);
        }

        TEST_METHOD(canonicalizePathTest)
        {
            TEXT input = wrap("C:\\dir\\file\\..\\x\\.\\file2");
            TCHAR expected[] = _T("C:\\dir\\x\\file2");
            TEXT actual = canonicalizePath(&input);

            Assert::AreEqual(expected, actual.value);

            freeText(&actual);
        }
    };
}
