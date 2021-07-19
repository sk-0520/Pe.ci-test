#include "pch.h"

extern "C" {
#   include "../Pe.Boot/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(textConvertTest)
    {
    public:

        TEST_METHOD(parseIntegerFromText_success_Test)
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
                auto actual = parseIntegerFromText(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parseIntegerFromText_value_Test)
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
                auto actual = parseIntegerFromText(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(parseLongFromText_success_Test)
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
                auto actual = parseLongFromText(&arg1, arg2);
                if (test.expected) {
                    Assert::IsTrue(actual.success);
                } else {
                    Assert::IsFalse(actual.success);
                }
            }
        }

        TEST_METHOD(parseLongFromText_value_Test)
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
                auto actual = parseLongFromText(&arg1, arg2);
                Assert::IsTrue(actual.success);
                Assert::AreEqual(test.expected, actual.value);
            }
        }
    };
}
