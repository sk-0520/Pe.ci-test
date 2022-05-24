#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(text_parse_test)
    {
    public:

        TEST_METHOD(parse_i32_from_text_success_test)
        {
            auto tests = {
                DATA(false, wrap(""), PARSE_BASE_NUMBER_D),
                DATA(true, wrap("1"), PARSE_BASE_NUMBER_D),
                DATA(false, wrap("0xf"), PARSE_BASE_NUMBER_D),

                DATA(false, wrap(""), PARSE_BASE_NUMBER_X),
                DATA(true, wrap("1"), PARSE_BASE_NUMBER_X),
                DATA(true, wrap("0xf"), PARSE_BASE_NUMBER_X),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                PARSE_BASE_NUMBER arg2 = std::get<1>(test.inputs);
                auto actual = parse_i32_from_text(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parse_i32_from_text_value_test)
        {
            auto tests = {
                DATA(1, wrap("1"), PARSE_BASE_NUMBER_D),
                DATA(123, wrap("123"), PARSE_BASE_NUMBER_D),

                DATA(1, wrap("1"), PARSE_BASE_NUMBER_X),
                DATA(15, wrap("0xf"), PARSE_BASE_NUMBER_X),
                DATA(8191, wrap("0x1fff"), PARSE_BASE_NUMBER_X),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                PARSE_BASE_NUMBER arg2 = std::get<1>(test.inputs);
                auto actual = parse_i32_from_text(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }


#ifdef _WIN64
        TEST_METHOD(parse_i64_from_text_success_test)
        {
            auto tests = {
                DATA(false, wrap(""), PARSE_BASE_NUMBER_D),
                DATA(true, wrap("1"), PARSE_BASE_NUMBER_D),
                DATA(false, wrap("0xf"), PARSE_BASE_NUMBER_D),

                DATA(false, wrap(""), PARSE_BASE_NUMBER_X),
                DATA(true, wrap("1"), PARSE_BASE_NUMBER_X),
                DATA(true, wrap("0xf"), PARSE_BASE_NUMBER_X),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                PARSE_BASE_NUMBER arg2 = std::get<1>(test.inputs);
                auto actual = parse_i64_from_text(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parse_i64_from_text_value_test)
        {
            auto tests = {
                DATA((__int64)1, wrap("1"), PARSE_BASE_NUMBER_D),
                DATA((__int64)123, wrap("123"), PARSE_BASE_NUMBER_D),

                DATA((__int64)1, wrap("1"), PARSE_BASE_NUMBER_X),
                DATA((__int64)15, wrap("0xf"), PARSE_BASE_NUMBER_X),
                DATA((__int64)8191, wrap("0x1fff"), PARSE_BASE_NUMBER_X),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                PARSE_BASE_NUMBER arg2 = std::get<1>(test.inputs);
                auto actual = parse_i64_from_text(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(parse_i64_from_text_sentinel_test)
        {
            auto tests = {
                DATA((__int64)1, wrap("1"), PARSE_BASE_NUMBER_D),
                DATA((__int64)123, wrap("123"), PARSE_BASE_NUMBER_D),

                DATA((__int64)1, wrap("1"), PARSE_BASE_NUMBER_X),
                DATA((__int64)15, wrap("0xf"), PARSE_BASE_NUMBER_X),
                DATA((__int64)8191, wrap("0x1fff"), PARSE_BASE_NUMBER_X),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                arg1.library.sentinel = false;
                PARSE_BASE_NUMBER arg2 = std::get<1>(test.inputs);
                auto actual = parse_i64_from_text(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }
#endif

        TEST_METHOD(parse_i32_from_2_text_test)
        {
            auto tests = {
                DATA((int32_t)0, wrap("0")),
                DATA((int32_t)1, wrap("1")),

                DATA((int32_t)0, wrap("0000")),
                DATA((int32_t)1, wrap("0001")),
                DATA((int32_t)2, wrap("0010")),
                DATA((int32_t)3, wrap("0011")),
                DATA((int32_t)4, wrap("0100")),
                DATA((int32_t)5, wrap("0101")),
                DATA((int32_t)6, wrap("0110")),
                DATA((int32_t)7, wrap("0111")),
                DATA((int32_t)8, wrap("1000")),

                DATA((int32_t)0, wrap("0b0000")),
                DATA((int32_t)1, wrap("0b0001")),
                DATA((int32_t)2, wrap("0b0010")),
                DATA((int32_t)3, wrap("0b0011")),
                DATA((int32_t)4, wrap("0b0100")),
                DATA((int32_t)5, wrap("0b0101")),
                DATA((int32_t)6, wrap("0b0110")),
                DATA((int32_t)7, wrap("0b0111")),
                DATA((int32_t)8, wrap("0b1000")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT_PARSED_I32_RESULT actual = parse_i32_from_text(&arg1, PARSE_BASE_NUMBER_B);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

#ifdef _WIN64
        TEST_METHOD(parse_i64_from_2_text_test)
        {
            auto tests = {
                DATA((int64_t)0, wrap("0")),
                DATA((int64_t)1, wrap("1")),

                DATA((int64_t)0, wrap("0000")),
                DATA((int64_t)1, wrap("0001")),
                DATA((int64_t)2, wrap("0010")),
                DATA((int64_t)3, wrap("0011")),
                DATA((int64_t)4, wrap("0100")),
                DATA((int64_t)5, wrap("0101")),
                DATA((int64_t)6, wrap("0110")),
                DATA((int64_t)7, wrap("0111")),
                DATA((int64_t)8, wrap("1000")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT_PARSED_I64_RESULT actual = parse_i64_from_text(&arg1, PARSE_BASE_NUMBER_B);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }
#endif
    };
}
