#include "pch.h"

extern "C" {
#   include "../Pe.Boot/commandline.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(commandlineTest)
    {
    public:
        TEST_METHOD(parseCommandLine_withCommand_Test)
        {
            TEXT input1 = wrap("abc def");
            COMMAND_LINE_OPTION actual1_1 = parseCommandLine(&input1, false);
            Assert::AreEqual((size_t)2, actual1_1.count);
            Assert::AreEqual(_T("abc"), actual1_1.arguments[0].value);
            Assert::AreEqual(_T("def"), actual1_1.arguments[1].value);
            Assert::IsNull(actual1_1.library.command);
            freeCommandLine(&actual1_1);

            COMMAND_LINE_OPTION actual1_2 = parseCommandLine(&input1, true);
            Assert::AreEqual((size_t)1, actual1_2.count);
            Assert::AreEqual(_T("def"), actual1_2.arguments[0].value);
            Assert::IsNotNull(actual1_2.library.command);
            Assert::AreEqual(_T("abc"), actual1_2.library.command->value);
            freeCommandLine(&actual1_2);


            TEXT input2 = wrap("");
            COMMAND_LINE_OPTION actual2_1 = parseCommandLine(&input2, false);
            Assert::AreEqual((size_t)0, actual2_1.count);
            Assert::IsNull(actual2_1.library.command);
            freeCommandLine(&actual2_1);

            COMMAND_LINE_OPTION actual2_2 = parseCommandLine(&input2, true);
            Assert::AreEqual((size_t)0, actual2_2.count);
            Assert::IsNull(actual2_2.library.command);
            freeCommandLine(&actual2_2);
        }

        TEST_METHOD(parseCommandLine_map_Test)
        {
            TEXT input1 = wrap("abc --def -ghi /jkl");
            COMMAND_LINE_OPTION actual1 = parseCommandLine(&input1, true);
            freeCommandLine(&actual1);
        }
    };
}
