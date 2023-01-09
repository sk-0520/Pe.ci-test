#include "pch.h"

extern "C" {
#   include "../Pe.Library/platform.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(command_line_test)
    {
    public:
        TEST_METHOD(expand_environment_variable_test)
        {
            auto tests = {
                DATA(wrap("[VALUE1]"), wrap("KEY1"), wrap("VALUE1"), wrap("[%KEY1%]")),
                DATA(wrap("[VALUE2][VALUE2]"), wrap("KEY2"), wrap("VALUE2"), wrap("[%KEY2%][%KEY2%]")),
            };
            for (auto test : tests) {
                TEXT key = std::get<0>(test.inputs);
                TEXT value = std::get<1>(test.inputs);
                TEXT input = std::get<2>(test.inputs);

                set_environment_variable(&key, &value);
                TEXT actual = expand_environment_variable(&input, DEFAULT_MEMORY_ARENA);

                Assert::AreEqual(test.expected.value, actual.value, input.value);

                release_text(&actual);
            }
        }
    };
}
