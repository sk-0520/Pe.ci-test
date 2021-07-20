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
                // 今の時点では何しても EXECUTE_MODE_BOOT になる
                DATA(EXECUTE_MODE_BOOT, wrap("app")),
                DATA(EXECUTE_MODE_BOOT, wrap("app /_mode=help")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                COMMAND_LINE_OPTION command_line_optioin = parse_command_line(&arg1, true);
                EXECUTE_MODE actual = get_execute_mode(&command_line_optioin);
                Assert::AreEqual((int)test.expected, (int)actual);
            }
        }
    };
}
