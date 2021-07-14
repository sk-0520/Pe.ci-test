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
            TCHAR input[MAX_PATH] = _T("C:\\dir\\file");
            TCHAR actual[MAX_PATH] = { 0 };
            TCHAR expected[MAX_PATH]= _T("C:\\dir");
            size_t len = getParentDirectoryPath(actual, input);

            Assert::AreEqual(expected, actual);

            TEXT input2 = wrap("C:\\dir\\file");
            TEXT actual2 = getParentDirectoryPath2(&input2);
            Assert::AreEqual(expected, actual2.value);
            freeText(&actual2);
        }

        TEST_METHOD(combinePathTest)
        {
            TCHAR input1[MAX_PATH] = _T("C:\\dir");
            TCHAR input2[MAX_PATH] = _T("file");
            TCHAR actual[MAX_PATH] = { 0 };
            TCHAR expected[MAX_PATH] = _T("C:\\dir\\file");
            size_t len = combinePath(actual, input1, input2);

            Assert::AreEqual(expected, actual);

            TEXT input1t = wrapText(input1);
            TEXT input2t = wrapText(input2);

            TEXT actual2 = combinePath2(&input1t, &input2t);
            Assert::AreEqual(expected, actual2.value);
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
