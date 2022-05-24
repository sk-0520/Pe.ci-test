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
#ifdef _UNICODE
        TEST_METHOD(multibyte_success_test)
        {
            auto input = wrap("あいうえお");
            auto tests = {
                MULTI_BYTE_CHARACTER_TYPE_UTF8,
                MULTI_BYTE_CHARACTER_TYPE_SJIS,
            };
            for (auto test : tests) {
                auto res = convert_to_multibyte_character(&input, test, DEFAULT_MEMORY);
                Assert::IsTrue(is_enabled_multibyte_character_result(&res));
                auto text = make_text_from_multibyte(res.buffer, res.length, test, DEFAULT_MEMORY);
                Assert::AreEqual(text.value, input.value);
                Assert::IsTrue(release_multibyte_character_result(&res, DEFAULT_MEMORY));
                Assert::IsTrue(release_text(&text));
                Assert::IsFalse(release_multibyte_character_result(&res, DEFAULT_MEMORY));
            }
        }
#endif
    };
}
