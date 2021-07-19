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

        TEST_METHOD(parseCommandLine_item_flag_Test)
        {
            TEXT expecteds[] = {
                wrap("def"),
                wrap("ghi"),
                wrap("jkl"),
            };
            TEXT input = wrap("abc --def -ghi /jkl");
            COMMAND_LINE_OPTION actual = parseCommandLine(&input, true);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                const COMMAND_LINE_ITEM* item = getCommandLineItem(&actual, &expecteds[i]);
                Assert::IsFalse(hasValueCommandLineItem(item));
            }

            freeCommandLine(&actual);
        }

        TEST_METHOD(parseCommandLine_item_value_space_Test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("GHI") },
                { wrap("jkl"), wrap("JKL") },
            };
            TEXT input = wrap("abc --def DEF -ghi GHI /jkl JKL");
            COMMAND_LINE_OPTION actual = parseCommandLine(&input, true);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expectedKey = &expecteds[i][0];
                TEXT* expectedValue = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = getCommandLineItem(&actual, expectedKey);
                Assert::IsNotNull(item);
                Assert::AreEqual(expectedValue->value, item->value.value);
            }

            freeCommandLine(&actual);
        }

        TEST_METHOD(parseCommandLine_item_value_equal_Test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("GHI") },
                { wrap("jkl"), wrap("JKL") },
            };
            TEXT input = wrap("abc --def=DEF -ghi=GHI /jkl=JKL");
            COMMAND_LINE_OPTION actual = parseCommandLine(&input, true);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expectedKey = &expecteds[i][0];
                TEXT* expectedValue = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = getCommandLineItem(&actual, expectedKey);
                Assert::IsNotNull(item);
                Assert::AreEqual(expectedValue->value, item->value.value);
            }

            freeCommandLine(&actual);
        }

        TEST_METHOD(parseCommandLine_item_value_space_with_equal_Test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("GHI") },
                { wrap("jkl"), wrap("JKL") },
            };
            TEXT input = wrap("abc --def=DEF -ghi GHI /jkl=JKL");
            COMMAND_LINE_OPTION actual = parseCommandLine(&input, true);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expectedKey = &expecteds[i][0];
                TEXT* expectedValue = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = getCommandLineItem(&actual, expectedKey);
                Assert::IsNotNull(item);
                Assert::AreEqual(expectedValue->value, item->value.value);
            }

            freeCommandLine(&actual);
        }

        TEST_METHOD(parseCommandLine_item_switch_width_value_Test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("") },
                { wrap("jkl"), wrap("") },
                { wrap("mno"), wrap("MNO") },
                { wrap("pqr"), createInvalidText() },
                { wrap("stu"), createInvalidText() },
            };
            TEXT input = wrap("abc --def=DEF -ghi= --jkl \"\" /mno=MNO --pqr --stu");
            COMMAND_LINE_OPTION actual = parseCommandLine(&input, true);

            const COMMAND_LINE_ITEM* item1 = getCommandLineItem(&actual, expecteds[0]);
            const COMMAND_LINE_ITEM* item2 = getCommandLineItem(&actual, expecteds[1]);
            const COMMAND_LINE_ITEM* item3 = getCommandLineItem(&actual, expecteds[2]);
            const COMMAND_LINE_ITEM* item4 = getCommandLineItem(&actual, expecteds[3]);
            const COMMAND_LINE_ITEM* item5 = getCommandLineItem(&actual, expecteds[4]);
            const COMMAND_LINE_ITEM* item6 = getCommandLineItem(&actual, expecteds[5]);

            Assert::IsTrue(hasValueCommandLineItem(item1));
            Assert::AreEqual(expecteds[0][1].value, item1->value.value);

            Assert::IsTrue(hasValueCommandLineItem(item2));
            Assert::AreEqual(expecteds[1][1].value, item2->value.value);

            Assert::IsTrue(hasValueCommandLineItem(item3));
            Assert::AreEqual(expecteds[2][1].value, item3->value.value);

            Assert::IsTrue(hasValueCommandLineItem(item4));
            Assert::AreEqual(expecteds[3][1].value, item4->value.value);

            Assert::IsFalse(hasValueCommandLineItem(item5));

            Assert::IsFalse(hasValueCommandLineItem(item6));

            freeCommandLine(&actual);
        }

        TEST_METHOD(parseCommandLine_item_value_with_space_Test)
        {
            TEXT expecteds[][2] = {
                { wrap("0"), wrap("0 0") },
                { wrap("1"), wrap("1 1 ") },
                { wrap("2"), wrap(" 2 2") },
                { wrap("3"), wrap(" 3 3") },
            };
            TEXT input = wrap("abc --0=\"0 0\" -1 \"1 1 \" /2 \" 2 2\" \"-3= 3 3\"");
            COMMAND_LINE_OPTION actual = parseCommandLine(&input, true);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expectedKey = &expecteds[i][0];
                TEXT* expectedValue = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = getCommandLineItem(&actual, expectedKey);
                Assert::IsNotNull(item);
                Assert::AreEqual(expectedValue->value, item->value.value);
            }

            freeCommandLine(&actual);
        }
    };
}
