#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(text_convert_test)
    {
    public:

        TEST_METHOD(to_lower_text_test)
        {
            auto tests = {
                DATA(wrap("abc"), wrap("abc")),
                DATA(wrap("abc"), wrap("ABC")),
                DATA(wrap("123"), wrap("123")),
                DATA(wrap("sa🍶ke"), wrap("SA🍶KE")),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);

                auto actual = to_lower_text(&arg1, DEFAULT_MEMORY_ARENA);
                Assert::IsTrue(is_equals_text(&test.expected, &actual, false), test.expected.value);
                release_text(&actual);
            }
        }

        TEST_METHOD(to_upper_text_test)
        {
            auto tests = {
                DATA(wrap("ABC"), wrap("abc")),
                DATA(wrap("ABC"), wrap("ABC")),
                DATA(wrap("123"), wrap("123")),
                DATA(wrap("SA🍶KE"), wrap("sa🍶ke")),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);

                auto actual = to_upper_text(&arg1, DEFAULT_MEMORY_ARENA);
                Assert::IsTrue(is_equals_text(&test.expected, &actual, false), test.expected.value);
                release_text(&actual);
            }
        }

#ifdef _UNICODE
        TEST_METHOD(multibyte_success_test)
        {
            auto input = wrap("あいうえお");
            auto tests = {
                MULTI_BYTE_CHARACTER_TYPE_UTF8,
                MULTI_BYTE_CHARACTER_TYPE_SJIS,
            };
            for (auto test : tests) {
                auto res = convert_to_multibyte_character(&input, test, DEFAULT_MEMORY_ARENA);
                Assert::IsTrue(is_enabled_multibyte_character_result(&res));
                auto text = make_text_from_multibyte(res.buffer, res.length, test, DEFAULT_MEMORY_ARENA);
                Assert::AreEqual(text.value, input.value);
                Assert::IsTrue(release_multibyte_character_result(&res, DEFAULT_MEMORY_ARENA));
                Assert::IsTrue(release_text(&text));
                Assert::IsFalse(release_multibyte_character_result(&res, DEFAULT_MEMORY_ARENA));
            }
        }
#endif
    };
}
