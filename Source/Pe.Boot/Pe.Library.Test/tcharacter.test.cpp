#include "pch.h"

extern "C" {
#   include "../Pe.Library/tcharacter.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(character_test)
    {
    public:

        TEST_METHOD(is_newline_character_test)
        {
            auto tests = {
                DATA(true, '\r'),
                DATA(true, '\r'),

                DATA(false, '\0'),
                DATA(false, '\f'),
                DATA(false, '\v'),
            };

            for (auto test : tests) {
                auto actual = test.run(is_newline_character);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(is_digit_character_test)
        {
            auto tests = {
                DATA(true, '0'),
                DATA(true, '1'),
                DATA(true, '2'),
                DATA(true, '3'),
                DATA(true, '4'),
                DATA(true, '5'),
                DATA(true, '6'),
                DATA(true, '7'),
                DATA(true, '8'),
                DATA(true, '9'),

                DATA(false, '!'),
                DATA(false, 'A'),
                DATA(false, 'a'),
            };

            for (auto test : tests) {
                auto actual = std::apply(is_digit_character, test.inputs);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(is_lower_character_test)
        {
            auto tests = {
                DATA(true, 'a'),
                DATA(true, 'z'),

                DATA(false, 'A'),
                DATA(false, 'Z'),

                DATA(false, '0'),
                DATA(false, '!'),
            };

            for (auto test : tests) {
                auto actual = std::apply(is_lower_character, test.inputs);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(is_upper_character_test)
        {
            auto tests = {
                DATA(false, 'a'),
                DATA(false, 'z'),

                DATA(true, 'A'),
                DATA(true, 'Z'),

                DATA(false, '0'),
                DATA(false, '!'),
            };

            for (auto test : tests) {
                auto actual = std::apply(is_upper_character, test.inputs);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(is_alphabet_character_test)
        {
            auto tests = {
                DATA(true, 'a'),
                DATA(true, 'z'),

                DATA(true, 'A'),
                DATA(true, 'Z'),

                DATA(false, '0'),
                DATA(false, '!'),
            };

            for (auto test : tests) {
                auto actual = std::apply(is_alphabet_character, test.inputs);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(to_lower_character_test)
        {
            auto tests = {
                DATA(_T('a'), _T('a')),
                DATA(_T('z'), _T('z')),

                DATA(_T('a'), _T('A')),

                DATA(_T('0'), _T('0')),
                DATA(_T('!'), _T('!')),
            };

            for (auto test : tests) {
                auto actual = std::apply(to_lower_character, test.inputs);
                Assert::AreEqual(test.expected, actual);
            }
        }

        TEST_METHOD(to_upper_character_test)
        {
            auto tests = {
                DATA(_T('A'), _T('a')),
                DATA(_T('Z'), _T('z')),

                DATA(_T('A'), _T('A')),

                DATA(_T('0'), _T('0')),
                DATA(_T('!'), _T('!')),
            };

            for (auto test : tests) {
                auto actual = std::apply(to_upper_character, test.inputs);
                Assert::AreEqual(test.expected, actual);
            }
        }

        TEST_METHOD(exists_character_test)
        {
            auto tests = {
                DATA(true, _T('a'), _T("a"), 1),
                DATA(false, _T('a'), _T("b"), 1),

                DATA(true, _T('a'), _T("abc"), 3),
                DATA(true, _T('b'), _T("abc"), 3),
                DATA(true, _T('c'), _T("abc"), 3),

                DATA(false, _T('d'), _T("abc"), 3),

                DATA(false, _T('A'), _T("abc"), 3),
                DATA(false, _T('B'), _T("abc"), 3),
                DATA(false, _T('C'), _T("abc"), 3),
            };

            for (auto test : tests) {
                auto actual = std::apply(exists_character, test.inputs);
                Assert::AreEqual(test.expected, actual);
            }
        }

    };
}
