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

        TEST_METHOD(findTextTest)
        {
            auto input = wrap("abcDEF-ABCdef");
            auto tests = {
                TestData(_T("abcDEF-ABCdef"), input, wrap("a"), false),
                TestData(_T("ABCdef"), input, wrap("A"), false),
                TestData(_T("def"), input, wrap("d"), false),
                TestData(_T("DEF-ABCdef"), input, wrap("D"), false),

                TestData(_T("abcDEF-ABCdef"), input, wrap("a"), true),
                TestData(_T("abcDEF-ABCdef"), input, wrap("A"), true),
                TestData(_T("DEF-ABCdef"), input, wrap("d"), true),
                TestData(_T("DEF-ABCdef"), input, wrap("D"), true),
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
            auto input2 = wrap("x");
            TEXT actual = findText(&input1, &input2, false);
            Assert::IsFalse(isEnabledText(&actual));
        }

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
    };
}
