#include "pch.h"
#include "CppUnitTest.h"

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
        }
    };
}
