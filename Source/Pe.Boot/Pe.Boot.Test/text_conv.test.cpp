#include "pch.h"

extern "C" {
#   include "../Pe.Boot/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(text_convert_test)
    {
    public:

        TEST_METHOD(parse_integer_from_text_success_test)
        {
            auto tests = {
                DATA(false, wrap(""), false),
                DATA(true, wrap("1"), false),
                DATA(true, wrap("0xf"), false),

                DATA(false, wrap(""), true),
                DATA(true, wrap("1"), true),
                DATA(true, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_integer_from_text(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parse_integer_from_text_value_test)
        {
            auto tests = {
                DATA(1, wrap("1"), false),
                DATA(0, wrap("0xf"), false),

                DATA(1, wrap("1"), true),
                DATA(15, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_integer_from_text(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(parse_long_from_text_success_test)
        {
            auto tests = {
                DATA(false, wrap(""), false),
                DATA(true, wrap("1"), false),
                DATA(true, wrap("0xf"), false),

                DATA(false, wrap(""), true),
                DATA(true, wrap("1"), true),
                DATA(true, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_long_from_text(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parse_long_from_text_value_test)
        {
            auto tests = {
                DATA((__int64)1, wrap("1"), false),
                DATA((__int64)0, wrap("0xf"), false),

                DATA((__int64)1, wrap("1"), true),
                DATA((__int64)15, wrap("0xf"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool arg2 = std::get<1>(test.inputs);
                auto actual = parse_long_from_text(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }
    };
}
