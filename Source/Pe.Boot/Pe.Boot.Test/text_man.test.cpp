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

    };
}
