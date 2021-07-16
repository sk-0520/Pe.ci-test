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

        TEST_METHOD(getTextLengthTest)
        {
            auto tests = {
                TestData((size_t)0, createEmptyText()),
                TestData((size_t)0, wrap("")),
                TestData((size_t)1, wrap("1")),
                TestData((size_t)2, wrap("22")),
                TestData((size_t)1, wrap("あ")),
                TestData((size_t)2, wrap("🏇")),
            };
            for (auto test : tests) {
                TEXT& input = std::get<0>(test.inputs);
                auto actual = getTextLength(&input);
                Assert::AreEqual(test.expected, actual);
            }
        }

        TEST_METHOD(findTextTest)
        {
            auto input = wrap("abcDEF-ABCdef");
            auto tests = {
                TestData(_T("abcDEF-ABCdef"), input, wrap("abc"), false),
                TestData(_T("ABCdef"), input, wrap("ABC"), false),
                TestData(_T("def"), input, wrap("def"), false),
                TestData(_T("DEF-ABCdef"), input, wrap("DEF"), false),

                TestData(_T("abcDEF-ABCdef"), input, wrap("abc"), true),
                TestData(_T("abcDEF-ABCdef"), input, wrap("ABC"), true),
                TestData(_T("DEF-ABCdef"), input, wrap("def"), true),
                TestData(_T("DEF-ABCdef"), input, wrap("DEF"), true),
                TestData(_T("DEF-ABCdef"), input, wrap("dEf"), true),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                bool& arg3 = std::get<2>(test.inputs);
                TEXT actual = findText(&arg1, &arg2, arg3);
                Assert::IsFalse(actual.library.needRelease);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(findText_notfound_Test)
        {
            auto input1 = wrap("abcDEF-ABCdef");
            auto input2 = wrap("Abc");
            TEXT actual = findText(&input1, &input2, false);
            Assert::IsFalse(isEnabledText(&actual));
        }

        TEST_METHOD(findCharacter2Test)
        {
            auto input = wrap("abcDEF-ABCdef");
            auto tests = {
                TestData(_T("abcDEF-ABCdef"), input, _T('a')),
                TestData(_T("ABCdef"), input, _T('A')),
                TestData(_T("def"), input, _T('d')),
                TestData(_T("DEF-ABCdef"), input, _T('D')),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TCHAR arg2 = std::get<1>(test.inputs);
                TEXT actual = findCharacter2(&arg1, arg2);
                Assert::IsFalse(actual.library.needRelease);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(findCharacter2_notfound_Test)
        {
            auto input1 = wrap("abcDEF-ABCdef");
            TEXT actual = findCharacter2(&input1, 'x');
            Assert::IsFalse(isEnabledText(&actual));
        }

        TEST_METHOD(indexCharacter2Test)
        {
            TEXT input = wrap("abcdefgABCDEFG");
            auto tests = {
                TestData((ssize_t)0, input, _T('a')),
                TestData((ssize_t)1, input, _T('b')),
                TestData((ssize_t)7, input, _T('A')),
                TestData((ssize_t)-1, input, _T('x')),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TCHAR arg2 = std::get<1>(test.inputs);
                auto actual = indexOfCharacter(&arg1, arg2);
                Assert::AreEqual(test.expected, actual);
            }
        }

        TEST_METHOD(compareTextTest)
        {
            auto tests = {
                TestData(0, wrap("abc"), wrap("abc"), false),
                TestData(-1, wrap("abc"), wrap("def"), false),
                TestData(+1, wrap("def"), wrap("abc"), false),
                TestData(-1, wrap("abc"), wrap("ABC"), false),
                TestData(-1, wrap("abc"), wrap("DEF"), false),
                TestData(+1, wrap("def"), wrap("ABC"), false),

                TestData(0, wrap("abc"), wrap("abc"), true),
                TestData(-1, wrap("abc"), wrap("def"), true),
                TestData(+1, wrap("def"), wrap("abc"), true),
                TestData(0, wrap("abc"), wrap("ABC"), true),
                TestData(-1, wrap("abc"), wrap("DEF"), true),
                TestData(+1, wrap("def"), wrap("ABC"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                bool arg3 = std::get<2>(test.inputs);
                auto actual = compareText(&arg1, &arg2, arg3);
                switch (test.expected) {
                    case -1:
                        Assert::IsTrue(actual < 0);
                        break;
                    case 0:
                        Assert::IsTrue(actual == 0);
                        break;
                    case +1:
                        Assert::IsTrue(0 < actual);
                        break;
                    default:
                        _ASSERT(false);
                }
            }
        }

        TEST_METHOD(compareTextDetail_compareTextTest_Test)
        {
            auto tests = {
                TestData(0, wrap("abc"), wrap("abc"), false),
                TestData(-1, wrap("abc"), wrap("def"), false),
                TestData(+1, wrap("def"), wrap("abc"), false),
                TestData(-1, wrap("abc"), wrap("ABC"), false),
                TestData(-1, wrap("abc"), wrap("DEF"), false),
                TestData(+1, wrap("def"), wrap("ABC"), false),

                TestData(0, wrap("abc"), wrap("abc"), true),
                TestData(-1, wrap("abc"), wrap("def"), true),
                TestData(+1, wrap("def"), wrap("abc"), true),
                TestData(0, wrap("abc"), wrap("ABC"), true),
                TestData(-1, wrap("abc"), wrap("DEF"), true),
                TestData(+1, wrap("def"), wrap("ABC"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                TEXT_COMPARE_MODE arg3 = std::get<2>(test.inputs) ? TEXT_COMPARE_MODE_IGNORE_CASE : TEXT_COMPARE_MODE_NONE;
#pragma warning(push)
#pragma warning(disable:26812)
                auto actual = compareTextDetail(&arg1, &arg2, -1, arg3, LOCALE_TYPE_INVARIANT);
#pragma warning(pop)
                Assert::IsTrue(actual.success);
                switch (test.expected) {
                    case -1:
                        Assert::IsTrue(actual.compare < 0);
                        break;
                    case 0:
                        Assert::IsTrue(actual.compare == 0);
                        break;
                    case +1:
                        Assert::IsTrue(0 < actual.compare);
                        break;
                    default:
                        _ASSERT(false);
                }
            }

        }

        TEST_METHOD(parseInteger_success_Test)
        {
            auto tests = {
                TestData(false, wrap(""), false),
                TestData(true, wrap("1"), false),
                TestData(true, wrap("0xf"), false),

                TestData(false, wrap(""), true),
                TestData(true, wrap("1"), true),
                TestData(true, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parseInteger(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parseInteger_value_Test)
        {
            auto tests = {
                TestData(1, wrap("1"), false),
                TestData(0, wrap("0xf"), false),

                TestData(1, wrap("1"), true),
                TestData(15, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parseInteger(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(parseLong_success_Test)
        {
            auto tests = {
                TestData(false, wrap(""), false),
                TestData(true, wrap("1"), false),
                TestData(true, wrap("0xf"), false),

                TestData(false, wrap(""), true),
                TestData(true, wrap("1"), true),
                TestData(true, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parseLong(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parseLong_value_Test)
        {
            auto tests = {
                TestData((__int64)1, wrap("1"), false),
                TestData((__int64)0, wrap("0xf"), false),

                TestData((__int64)1, wrap("1"), true),
                TestData((__int64)15, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parseLong(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

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
            TEXT b = createEmptyText();

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

        TEST_METHOD(startsWithTest)
        {
            auto tests = {
                //TestData(true, wrap("abc"), wrap("a")),
                //TestData(false, wrap("abc"), wrap("b")),

                //TestData(true, wrap("abc"), wrap("ab")),
                //TestData(false, wrap("abc"), wrap("bc")),
                TestData(false, wrap("abc"), wrap("ac")),

                //TestData(true, wrap("abc"), wrap("abc")),
                //TestData(false, wrap("abc"), wrap("abcd")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                auto actual = startsWithText(&arg1, &arg2);
                if (test.expected) {
                    Assert::IsTrue(actual, arg2.value);
                } else {
                    Assert::IsFalse(actual, arg2.value);
                }
            }
        }
    };
}
