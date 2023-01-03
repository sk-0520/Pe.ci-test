#include "pch.h"

extern "C" {
#   include "../Pe.Library/tstring.h"
#   include "../Pe.Library/string_builder.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(string_builder_test)
    {
    public:
        TEST_METHOD(new_string_builder_test)
        {
            auto tests = {
                DATA((size_t)4, 4),
                DATA((size_t)1, 1),
                DATA((size_t)2, 2),
            };

            for (auto test : tests) {
                auto [arg1] = test.inputs;
                auto actual = new_string_builder(arg1, DEFAULT_MEMORY_ARENA);
                //Assert::AreEqual(test.expected, actual.library.capacity);

                Assert::IsTrue(release_string_builder(&actual));
            }
        }

        TEST_METHOD(release_string_builder_test)
        {
            STRING_BUILDER* input1 = NULL;
            Assert::IsFalse(release_string_builder(input1));

            STRING_BUILDER input2 = { 0 };
            Assert::IsFalse(release_string_builder(&input2));

            STRING_BUILDER input3 = new_string_builder(1, DEFAULT_MEMORY_ARENA);
            Assert::IsTrue(release_string_builder(&input3));
        }

        TEST_METHOD(append_string_builder_test)
        {
            auto expected = _T("ABCDEFGtrue-1false1");
            auto sb = new_string_builder(3, DEFAULT_MEMORY_ARENA);

            append_builder_string(&sb, _T("ABC"), false);

            TEXT text = wrap("DEF");
            append_builder_text(&sb, &text, false);

            append_builder_character(&sb, _T('G'), false);

            append_builder_bool(&sb, true, false);
            append_builder_int(&sb, -1, false);
            append_builder_bool(&sb, false, false);
            append_builder_uint(&sb, 1, false);

            TEXT actual = build_text_string_builder(&sb);

            Assert::AreEqual(expected, actual.value);

            TEXT free1 = reference_text_string_builder(&sb);
            TEXT free2 = reference_text_string_builder(&sb);
            TEXT free3 = reference_text_string_builder(&sb);


            Assert::IsTrue(release_text(&actual));
            Assert::IsTrue(release_string_builder(&sb));
        }

        TEST_METHOD(append_builder_format_test)
        {
            auto sb = new_string_builder(3, DEFAULT_MEMORY_ARENA);

            auto expected = _T("1 -1 +100 -100 -200 A abc def");
            TEXT format = wrap("%d %d %+d %d %+d %c %s %t");
            auto t = wrap("def");
            append_builder_format(&sb, &format, 1, -1, 100, -100, -200, _T('A'), _T("abc"), &t);
            TEXT actual = build_text_string_builder(&sb);

            Assert::AreEqual(expected, actual.value);

            Assert::IsTrue(release_text(&actual));
            Assert::IsTrue(release_string_builder(&sb));
        }
    };
}
