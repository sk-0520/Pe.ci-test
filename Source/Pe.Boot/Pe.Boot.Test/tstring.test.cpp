#include "pch.h"

extern "C" {
#   include "../Pe.Boot/tstring.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(stringTest)
    {
    public:

        TEST_METHOD(findStringTest)
        {
            auto actual = findString(_T("abcdefg"), _T("abc"));
            Test_IsTrue(actual[0] == 'a');

            Test_IsNull(findString(_T("abcdefg"), _T("xyz")));
        }

        TEST_METHOD(getStringLengthTest)
        {
            TCHAR input[] = _T("abcdefg");
            size_t expected = sizeof(input) / sizeof(TCHAR) - 1/*終端*/;
            auto actual = getStringLength(input);
            Test_AreEqual(expected, actual);
        }


        TEST_METHOD(findCharacterTest)
        {
            TCHAR input[] = _T("abcdefg");
            auto actual = findCharacter(input, 'c');
            Test_AreEqual(_T('c'), actual[0]);

            Test_IsNull(findCharacter(input, 'z'));
        }

        TEST_METHOD(tryParseIntegerTest)
        {
            int a1;
            Test_IsTrue(tryParseInteger(&a1, _T("123")));
            Test_AreEqual(123, a1);

            int a2;
            Test_IsTrue(tryParseInteger(&a2, _T("-123")));
            Test_AreEqual(-123, a2);

            int a3;
            Test_IsTrue(tryParseInteger(&a3, _T("+123")));
            Test_AreEqual(123, a3);

            int a4;
            Test_IsTrue(tryParseInteger(&a4, _T("123abc")));
            Test_AreEqual(123, a4);
        }

        TEST_METHOD(tryParseHexOrIntegerTest)
        {
            int a1;
            Test_IsTrue(tryParseHexOrInteger(&a1, _T("123")));
            Test_AreEqual(123, a1);

            int a2;
            Test_IsTrue(tryParseHexOrInteger(&a2, _T("-123")));
            Test_AreEqual(-123, a2);

            int a3;
            Test_IsTrue(tryParseHexOrInteger(&a3, _T("+123")));
            Test_AreEqual(123, a3);

            int a4;
            Test_IsTrue(tryParseHexOrInteger(&a4, _T("123abc")));
            Test_AreEqual(123, a4);

            int h1;
            Test_IsTrue(tryParseHexOrInteger(&h1, _T("0xf")));
            Test_AreEqual(15, h1);

            int h2;
            Test_IsTrue(tryParseHexOrInteger(&h2, _T("0x0f")));
            Test_AreEqual(15, h2);
        }

        TEST_METHOD(tryParseLongTest)
        {
            long long a1;
            Test_IsTrue(tryParseLong(&a1, _T("123")));
            Test_AreEqual((long long)123, a1);

            long long a2;
            Test_IsTrue(tryParseLong(&a2, _T("-123")));
            Test_AreEqual((long long)-123, a2);

            long long a3;
            Test_IsTrue(tryParseLong(&a3, _T("+123")));
            Test_AreEqual((long long)123, a3);

            long long a4;
            Test_IsTrue(tryParseLong(&a4, _T("123abc")));
            Test_AreEqual((long long)123, a4);
        }

        TEST_METHOD(tryParseHexOrLongTest)
        {
            long long a1;
            Test_IsTrue(tryParseHexOrLong(&a1, _T("123")));
            Test_AreEqual((long long)123, a1);

            long long a2;
            Test_IsTrue(tryParseHexOrLong(&a2, _T("-123")));
            Test_AreEqual((long long)-123, a2);

            long long a3;
            Test_IsTrue(tryParseHexOrLong(&a3, _T("+123")));
            Test_AreEqual((long long)123, a3);

            long long a4;
            Test_IsTrue(tryParseHexOrLong(&a4, _T("123abc")));
            Test_AreEqual((long long)123, a4);

            long long h1;
            Test_IsTrue(tryParseHexOrLong(&h1, _T("0xf")));
            Test_AreEqual((long long)15, h1);

            long long h2;
            Test_IsTrue(tryParseHexOrLong(&h2, _T("0x0f")));
            Test_AreEqual((long long)15, h2);
        }

        TEST_METHOD(fromatStringTest)
        {
            TCHAR input[] = _T("abc %s 123 %d");
            TCHAR actual[1000] = { 0 };
            TCHAR expected[1000] = _T("abc def 123 456");
            formatString(actual, input, _T("def"), 456);

            Test_AreEqual(expected, actual);
        }

        TEST_METHOD(concatStringTest)
        {
            TCHAR input[] = _T("def");
            TCHAR actual[1000] = _T("abc");
            TCHAR expected[1000] = _T("abcdef");
            concatString(actual, input);

            Test_AreEqual(expected, actual);
        }


        TEST_METHOD(copyStringTest)
        {
            TCHAR input[] = _T("def");
            TCHAR actual[1000] = _T("abc");
            TCHAR expected[1000] = _T("def");
            copyString(actual, input);

            Test_AreEqual(expected, actual);
        }

    };
}
