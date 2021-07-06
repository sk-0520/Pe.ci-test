#include "pch.h"
#include "CppUnitTest.h"

extern "C" {
    #include "../Pe.Boot/tstring.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(tstringTest)
    {
    public:

        TEST_METHOD(tstrstrTest)
        {
            auto a1 = tstrstr(_T("abcdefg"), _T("abc"));
            Assert::IsTrue(a1[0] == 'a');
        }
    };
}
