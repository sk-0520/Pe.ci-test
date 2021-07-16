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
                TestData(_T("DEF-ABCdef"), input, wrap("D"), false),
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

    };
}
