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

        void getParentDirectoryPathTestCore()
        {

        }

        TEST_METHOD(getParentDirectoryPathTest)
        {
            TCHAR input[MAX_PATH] = _T("C:\\dir\\file");
            TCHAR actual[MAX_PATH] = { 0 };
            TCHAR expected[MAX_PATH]= _T("C:\\dir");
            size_t len = getParentDirectoryPath(actual, input);

            Assert::AreEqual(expected, actual);

            TEXT input2 = wct("C:\\dir\\file");
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
        }

    };
}
