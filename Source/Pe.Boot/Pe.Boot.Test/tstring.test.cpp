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
            TestData x = TestData(_T("defghi"), _T("abcdefghi"), _T("def"));
            auto actual2 = std::apply(findString, x.inputs);
            Assert::AreEqual(x.expected, actual2);

            auto actual = findString(_T("abcdefg"), _T("abc"));
            Assert::IsTrue(actual[0] == 'a');

            Assert::IsNull(findString(_T("abcdefg"), _T("xyz")));
        }

        TEST_METHOD(getStringLengthTest)
        {
            TCHAR input[] = _T("abcdefg");
            size_t expected = sizeof(input) / sizeof(TCHAR) - 1/*終端*/;
            auto actual = getStringLength(input);
            Assert::AreEqual(expected, actual);
        }


        TEST_METHOD(findCharacterTest)
        {
            TCHAR input[] = _T("abcdefg");
            auto actual = findCharacter(input, 'c');
            Assert::AreEqual(_T('c'), actual[0]);

            Assert::IsNull(findCharacter(input, 'z'));
        }

        TEST_METHOD(tryParseIntegerTest)
        {
            int a1;
            Assert::IsTrue(tryParseInteger(&a1, _T("123")));
            Assert::AreEqual(123, a1);

            int a2;
            Assert::IsTrue(tryParseInteger(&a2, _T("-123")));
            Assert::AreEqual(-123, a2);

            int a3;
            Assert::IsTrue(tryParseInteger(&a3, _T("+123")));
            Assert::AreEqual(123, a3);

            int a4;
            Assert::IsTrue(tryParseInteger(&a4, _T("123abc")));
            Assert::AreEqual(123, a4);
        }

        TEST_METHOD(tryParseHexOrIntegerTest)
        {
            int a1;
            Assert::IsTrue(tryParseHexOrInteger(&a1, _T("123")));
            Assert::AreEqual(123, a1);

            int a2;
            Assert::IsTrue(tryParseHexOrInteger(&a2, _T("-123")));
            Assert::AreEqual(-123, a2);

            int a3;
            Assert::IsTrue(tryParseHexOrInteger(&a3, _T("+123")));
            Assert::AreEqual(123, a3);

            int a4;
            Assert::IsTrue(tryParseHexOrInteger(&a4, _T("123abc")));
            Assert::AreEqual(123, a4);

            int h1;
            Assert::IsTrue(tryParseHexOrInteger(&h1, _T("0xf")));
            Assert::AreEqual(15, h1);

            int h2;
            Assert::IsTrue(tryParseHexOrInteger(&h2, _T("0x0f")));
            Assert::AreEqual(15, h2);
        }

        TEST_METHOD(tryParseLongTest)
        {
            long long a1;
            Assert::IsTrue(tryParseLong(&a1, _T("123")));
            Assert::AreEqual((long long)123, a1);

            long long a2;
            Assert::IsTrue(tryParseLong(&a2, _T("-123")));
            Assert::AreEqual((long long)-123, a2);

            long long a3;
            Assert::IsTrue(tryParseLong(&a3, _T("+123")));
            Assert::AreEqual((long long)123, a3);

            long long a4;
            Assert::IsTrue(tryParseLong(&a4, _T("123abc")));
            Assert::AreEqual((long long)123, a4);
        }

        TEST_METHOD(tryParseHexOrLongTest)
        {
            long long a1;
            Assert::IsTrue(tryParseHexOrLong(&a1, _T("123")));
            Assert::AreEqual((long long)123, a1);

            long long a2;
            Assert::IsTrue(tryParseHexOrLong(&a2, _T("-123")));
            Assert::AreEqual((long long)-123, a2);

            long long a3;
            Assert::IsTrue(tryParseHexOrLong(&a3, _T("+123")));
            Assert::AreEqual((long long)123, a3);

            long long a4;
            Assert::IsTrue(tryParseHexOrLong(&a4, _T("123abc")));
            Assert::AreEqual((long long)123, a4);

            long long h1;
            Assert::IsTrue(tryParseHexOrLong(&h1, _T("0xf")));
            Assert::AreEqual((long long)15, h1);

            long long h2;
            Assert::IsTrue(tryParseHexOrLong(&h2, _T("0x0f")));
            Assert::AreEqual((long long)15, h2);
        }

        TEST_METHOD(fromatStringTest)
        {
            TCHAR input[] = _T("abc %s 123 %d");
            TCHAR actual[1000] = { 0 };
            TCHAR expected[1000] = _T("abc def 123 456");
            formatString(actual, input, _T("def"), 456);

            Assert::AreEqual(expected, actual);
        }

        TEST_METHOD(concatStringTest)
        {
            TCHAR input[] = _T("def");
            TCHAR actual[1000] = _T("abc");
            TCHAR expected[1000] = _T("abcdef");
            concatString(actual, input);

            Assert::AreEqual(expected, actual);
        }


        TEST_METHOD(copyStringTest)
        {
            TCHAR input[] = _T("def");
            TCHAR actual[1000] = _T("abc");
            TCHAR expected[1000] = _T("def");
            copyString(actual, input);

            Assert::AreEqual(expected, actual);
        }

    };
}
