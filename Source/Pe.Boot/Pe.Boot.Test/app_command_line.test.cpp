#include "pch.h"

extern "C" {
#   include "../Pe.Boot/app_command_line.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(app_command_line_test)
    {
    public:

        TEST_METHOD(get_execute_mode_test)
        {
            auto tests = {
                DATA(EXECUTE_MODE_BOOT, wrap("app --_mode boot")),
                DATA(EXECUTE_MODE_CONSOLE, wrap("app --_mode console")),
                DATA(EXECUTE_MODE_BOOT, wrap("app")),
                DATA(EXECUTE_MODE_BOOT, wrap("app /_mode=help")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                COMMAND_LINE_OPTION command_line_optioin = parse_command_line(&arg1, true, DEFAULT_MEMORY_ARENA);
#pragma warning(push)
#pragma warning(disable:26812)
                EXECUTE_MODE actual = get_execute_mode(&command_line_optioin);
#pragma warning(pop)
                Assert::AreEqual((int)test.expected, (int)actual);

                release_command_line(&command_line_optioin);
            }
        }

        TEST_METHOD(get_wait_time_test)
        {
            auto tests = {
                DATA(1, wrap("app --_boot-wait 1")),
                DATA(10, wrap("app --_boot-wait 10 --_boot-wait 20")),
                DATA(20, wrap("app --__boot-wait 10 --_boot-wait 20")),
                DATA(-1, wrap("app --_boot-wait 0")),
                DATA(-1, wrap("app --_boot-wait -2")),
                DATA(-1, wrap("app --_boot-wait=")),
                DATA(-1, wrap("app --_boot-wait")),
                DATA(-1, wrap("app --_boot-wait dummy")),
                DATA(-1, wrap("app --_boot-wait --dummy")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                COMMAND_LINE_OPTION command_line_optioin = parse_command_line(&arg1, true, DEFAULT_MEMORY_ARENA);
                WAIT_TIME_ARG actual = get_wait_time(&command_line_optioin);
                if (test.expected == -1) {
                    Assert::IsFalse(actual.enabled);
                } else {
                    Assert::IsTrue(actual.enabled);
                    Assert::AreEqual(test.expected, actual.time);
                }

                release_command_line(&command_line_optioin);
            }
        }

    };
}
