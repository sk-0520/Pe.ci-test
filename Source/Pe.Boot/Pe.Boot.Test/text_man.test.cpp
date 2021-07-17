#include "pch.h"

extern "C" {
#   include "../Pe.Boot/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(textManipulationTest)
    {
    public:

        TEST_METHOD(addText_normal_Test)
        {
            auto tests = {
                TestData(_T("ab"), wrap("a"), wrap("b")),
                TestData(_T("a"), wrap("a"), wrap("")),
                TestData(_T("b"), wrap(""), wrap("b")),
                TestData(_T("🐭🐮🐯🐰🐉🐍🐴🐏🐵🐔🐶🐷"), wrap("🐭🐮🐯🐰🐉🐍"), wrap("🐴🐏🐵🐔🐶🐷")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                TEXT actual = addText(&arg1, &arg2);
                Assert::AreEqual(test.expected, actual.value);
                freeText(&actual);
            }
        }

        TEST_METHOD(addText_fail_Test)
        {
            TEXT a = wrap("");
            TEXT b = createInvalidText();

            TEXT actual_ab = addText(&a, &b);
            Assert::IsTrue(isEnabledText(&actual_ab));

            TEXT actual_ba = addText(&b, &a);
            Assert::IsTrue(isEnabledText(&actual_ba));

            TEXT actual_bb = addText(&b, &b);
            Assert::IsFalse(isEnabledText(&actual_bb));

            freeText(&actual_ab);
            freeText(&actual_ba);
            freeText(&actual_bb);
        }

        TEST_METHOD(joinTextTest)
        {
            TCHAR* expected1 = _T("1,2,3");
            TEXT input1[] = {
                wrap("1"),
                wrap("2"),
                wrap("3"),
            };
            TEXT sep1 = wrap(",");
            TEXT actual1 = joinText(&sep1, input1, SIZEOF_ARRAY(input1), IGNORE_EMPTY_NONE);
            Assert::AreEqual(expected1, actual1.value);
            freeText(&actual1);

            TCHAR* expected2 = _T("123");
            TEXT input2[] = {
                wrap(""),
                wrap("1"),
                wrap(""),
                wrap("2"),
                wrap(""),
                wrap("3"),
                wrap(""),
            };
            TEXT sep2 = wrap("");
            TEXT actual2 = joinText(&sep2, input2, SIZEOF_ARRAY(input2), IGNORE_EMPTY_NONE);
            Assert::AreEqual(expected2, actual2.value);
            freeText(&actual2);

            TCHAR* expected3_1 = _T(",1,,2, ,,3,");
            TCHAR* expected3_2 = _T("1,2, ,3");
            TCHAR* expected3_3 = _T("1,2,3");
            TEXT input3[] = {
                wrap(""),
                wrap("1"),
                wrap(""),
                wrap("2"),
                wrap(" "),
                wrap(""),
                wrap("3"),
                wrap(""),
            };
            TEXT sep3 = wrap(",");
            TEXT actual3_1 = joinText(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_NONE);
            Assert::AreEqual(expected3_1, actual3_1.value);
            freeText(&actual3_1);

            TEXT actual3_2 = joinText(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_ONLY);
            Assert::AreEqual(expected3_2, actual3_2.value);
            freeText(&actual3_2);

            TEXT actual3_3 = joinText(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_WHITESPACE);
            Assert::AreEqual(expected3_3, actual3_3.value);
            freeText(&actual3_3);
        }

        TEST_METHOD(isEmptyTextTest)
        {
            auto tests = {
                TestData(false, wrap("a")),
                TestData(false, wrap(" ")),
                TestData(false, wrap("\t")),
                TestData(true, wrap("")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool actual = isEmptyText(&arg1);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(isWhiteSpaceTextTest)
        {
            auto tests = {
                TestData(false, wrap("a")),
                TestData(true, wrap(" ")),
                TestData(true, wrap("\t")),
                TestData(true, wrap("")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool actual = isWhiteSpaceText(&arg1);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }
    };
}
