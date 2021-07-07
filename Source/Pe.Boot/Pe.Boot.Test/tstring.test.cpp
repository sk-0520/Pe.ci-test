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

        TEST_METHOD(tryParseIntegerTest)
        {
            int a1;
            Assert::IsTrue(tryParseInteger(_T("123"), &a1));
            Assert::AreEqual(123, a1);

            int a2;
            Assert::IsTrue(tryParseInteger(_T("-123"), &a2));
            Assert::AreEqual(-123, a2);

            int a3;
            Assert::IsTrue(tryParseInteger(_T("+123"), &a3));
            Assert::AreEqual(123, a3);

            int a4;
            Assert::IsTrue(tryParseInteger(_T("123abc"), &a4));
            Assert::AreEqual(123, a4);
        }

        TEST_METHOD(tryParseHexOrIntegerTest)
        {
            int a1;
            Assert::IsTrue(tryParseHexOrInteger(_T("123"), &a1));
            Assert::AreEqual(123, a1);

            int a2;
            Assert::IsTrue(tryParseHexOrInteger(_T("-123"), &a2));
            Assert::AreEqual(-123, a2);

            int a3;
            Assert::IsTrue(tryParseHexOrInteger(_T("+123"), &a3));
            Assert::AreEqual(123, a3);

            int a4;
            Assert::IsTrue(tryParseHexOrInteger(_T("123abc"), &a4));
            Assert::AreEqual(123, a4);

            int h1;
            Assert::IsTrue(tryParseHexOrInteger(_T("0xf"), &h1));
            Assert::AreEqual(15, h1);

            int h2;
            Assert::IsTrue(tryParseHexOrInteger(_T("0x0f"), &h2));
            Assert::AreEqual(15, h2);
        }

        TEST_METHOD(tryParseLongTest)
        {
            long long a1;
            Assert::IsTrue(tryParseLong(_T("123"), &a1));
            Assert::AreEqual((long long)123, a1);

            long long a2;
            Assert::IsTrue(tryParseLong(_T("-123"), &a2));
            Assert::AreEqual((long long)-123, a2);

            long long a3;
            Assert::IsTrue(tryParseLong(_T("+123"), &a3));
            Assert::AreEqual((long long)123, a3);

            long long a4;
            Assert::IsTrue(tryParseLong(_T("123abc"), &a4));
            Assert::AreEqual((long long)123, a4);
        }

        TEST_METHOD(tryParseHexOrLongTest)
        {
            long long a1;
            Assert::IsTrue(tryParseHexOrLong(_T("123"), &a1));
            Assert::AreEqual((long long)123, a1);

            long long a2;
            Assert::IsTrue(tryParseHexOrLong(_T("-123"), &a2));
            Assert::AreEqual((long long)-123, a2);

            long long a3;
            Assert::IsTrue(tryParseHexOrLong(_T("+123"), &a3));
            Assert::AreEqual((long long)123, a3);

            long long a4;
            Assert::IsTrue(tryParseHexOrLong(_T("123abc"), &a4));
            Assert::AreEqual((long long)123, a4);

            long long h1;
            Assert::IsTrue(tryParseHexOrLong(_T("0xf"), &h1));
            Assert::AreEqual((long long)15, h1);

            long long h2;
            Assert::IsTrue(tryParseHexOrLong(_T("0x0f"), &h2));
            Assert::AreEqual((long long)15, h2);
        }
    };
}
