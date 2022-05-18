#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(text_test)
    {
    public:

        TEST_METHOD(new_test)
        {
            TCHAR input[] = _T("あいう");
            TEXT c = new_text(input, DEFAULT_MEMORY);
            TEXT w = wrap_text(input);

            Assert::IsTrue(c.library.need_release);
            Assert::IsFalse(w.library.need_release);

            Assert::IsFalse(c.value == input);
            Assert::IsTrue(w.value == input);

            Assert::AreEqual(get_string_length(input), (size_t)c.length);
            Assert::AreEqual(get_string_length(input), (size_t)w.length);

            input[0] = _T('え');
            input[1] = _T('お');
            input[2] = 0;

            Assert::AreNotEqual(_T("えお"), c.value);
            Assert::AreEqual(_T("えお"), w.value);
            Assert::AreNotEqual(get_string_length(input), (size_t)c.length);

            TEXT dc = clone_text(&c, DEFAULT_MEMORY);
            TEXT dw = clone_text(&w, DEFAULT_MEMORY);

            Assert::IsTrue(dc.library.need_release);
            Assert::IsTrue(dw.library.need_release);

            Assert::AreEqual(c.value, dc.value);
            Assert::AreEqual(w.value, dw.value);

            Assert::IsTrue(release_text(&c));
            Assert::IsFalse(release_text(&w));

            Assert::IsTrue(c.library.released);
            Assert::IsFalse(w.library.released);

            Assert::IsTrue(release_text(&dc));
            Assert::IsTrue(release_text(&dw));
        }

        TEST_METHOD(format_text_test)
        {
            TCHAR* expected = _T("123 abc ABC");
            TEXT format = wrap("%d %s %t");
            int input1 = 123;
            TCHAR* input2 = _T("abc");
            TEXT input3 = wrap("ABC");

            TEXT actual = format_text(DEFAULT_MEMORY, &format, input1, input2, &input3);

            Assert::AreEqual(expected, actual.value);

            release_text(&actual);
        }

        TEST_METHOD(text_to_string_test)
        {
            auto tests = {
                DATA(_T(""), wrap("")),
                DATA(_T("1"), wrap("1")),
                DATA(_T(""), create_invalid_text()),
            };

            for (auto test : tests) {
                auto [arg1] = test.inputs;
                TCHAR* actual = text_to_string(&arg1, DEFAULT_MEMORY);

                if (is_enabled_text(&arg1)) {
                    Assert::AreEqual(arg1.value, actual);
                } else {
                    Assert::AreEqual(_T(""), actual);
                }

                release_string(actual, DEFAULT_MEMORY);
            }
        }
            
    };
}
