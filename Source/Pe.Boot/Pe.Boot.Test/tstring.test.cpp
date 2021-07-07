#include "pch.h"
#include "CppUnitTest.h"

extern "C" {
#   include "../Pe.Boot/tstring.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(tstringTest)
    {
    public:

        TEST_METHOD(tstrstrTest)
        {
            auto actual = findString(_T("abcdefg"), _T("abc"));
            Assert::IsTrue(actual[0] == 'a');

            Assert::IsNull(findString(_T("abcdefg"), _T("xyz")));
        }

        TEST_METHOD(tstrlenTest)
        {
            TCHAR input[] = _T("abcdefg");
            size_t expected = sizeof(input) / sizeof(TCHAR) - 1/*終端*/;
            auto actual = getStringLength(input);
            Assert::AreEqual(expected, actual);
        }


        TEST_METHOD(tstrchrTest)
        {
            TCHAR input[] = _T("abcdefg");
            auto actual = findCharacter(input, 'c');
            Assert::AreEqual(_T('c'), actual[0]);

            Assert::IsNull(findCharacter(input, 'z'));
        }

    };
}
