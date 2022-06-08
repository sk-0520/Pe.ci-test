#include "pch.h"

extern "C" {
#   include "../Pe.Library/command_line.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(command_line_test)
    {
    public:
        TEST_METHOD(parse_command_line_with_command_test)
        {
            TEXT input1 = wrap("abc def");
            COMMAND_LINE_OPTION actual1_1 = parse_command_line(&input1, false, DEFAULT_MEMORY);
            Assert::AreEqual((size_t)2, actual1_1.count);
            Assert::AreEqual(_T("abc"), actual1_1.arguments[0].value);
            Assert::AreEqual(_T("def"), actual1_1.arguments[1].value);
            Assert::IsNull(actual1_1.library.command);
            release_command_line(&actual1_1);

            COMMAND_LINE_OPTION actual1_2 = parse_command_line(&input1, true, DEFAULT_MEMORY);
            Assert::AreEqual((size_t)1, actual1_2.count);
            Assert::AreEqual(_T("def"), actual1_2.arguments[0].value);
            Assert::IsNotNull(actual1_2.library.command);
            Assert::AreEqual(_T("abc"), actual1_2.library.command->value);
            release_command_line(&actual1_2);


            TEXT input2 = wrap("");
            COMMAND_LINE_OPTION actual2_1 = parse_command_line(&input2, false, DEFAULT_MEMORY);
            Assert::AreEqual((size_t)0, actual2_1.count);
            Assert::IsNull(actual2_1.library.command);
            release_command_line(&actual2_1);

            COMMAND_LINE_OPTION actual2_2 = parse_command_line(&input2, true, DEFAULT_MEMORY);
            Assert::AreEqual((size_t)0, actual2_2.count);
            Assert::IsNull(actual2_2.library.command);
            release_command_line(&actual2_2);
        }

        TEST_METHOD(parse_command_line_item_flag_test)
        {
            TEXT expecteds[] = {
                wrap("def"),
                wrap("ghi"),
                wrap("jkl"),
            };
            TEXT input = wrap("abc --def -ghi /jkl");
            COMMAND_LINE_OPTION actual = parse_command_line(&input, true, DEFAULT_MEMORY);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                const COMMAND_LINE_ITEM* item = get_command_line_item(&actual, &expecteds[i]);
                Assert::IsFalse(has_value_command_line_item(item));
            }

            release_command_line(&actual);
        }

        TEST_METHOD(parse_command_line_item_value_space_test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("GHI") },
                { wrap("jkl"), wrap("JKL") },
            };
            TEXT input = wrap("abc --def DEF -ghi GHI /jkl JKL");
            COMMAND_LINE_OPTION actual = parse_command_line(&input, true, DEFAULT_MEMORY);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expected_key = &expecteds[i][0];
                TEXT* expected_value = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = get_command_line_item(&actual, expected_key);
                Assert::IsNotNull(item);
                Assert::AreEqual(expected_value->value, item->value.value);
            }

            release_command_line(&actual);
        }

        TEST_METHOD(parse_command_line_item_value_equal_test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("GHI") },
                { wrap("jkl"), wrap("JKL") },
            };
            TEXT input = wrap("abc --def=DEF -ghi=GHI /jkl=JKL");
            COMMAND_LINE_OPTION actual = parse_command_line(&input, true, DEFAULT_MEMORY);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expected_key = &expecteds[i][0];
                TEXT* expected_value = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = get_command_line_item(&actual, expected_key);
                Assert::IsNotNull(item);
                Assert::AreEqual(expected_value->value, item->value.value);
            }

            release_command_line(&actual);
        }

        TEST_METHOD(parse_command_line_item_value_space_with_equal_test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("GHI") },
                { wrap("jkl"), wrap("JKL") },
            };
            TEXT input = wrap("abc --def=DEF -ghi GHI /jkl=JKL");
            COMMAND_LINE_OPTION actual = parse_command_line(&input, true, DEFAULT_MEMORY);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expected_key = &expecteds[i][0];
                TEXT* expected_value = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = get_command_line_item(&actual, expected_key);
                Assert::IsNotNull(item);
                Assert::AreEqual(expected_value->value, item->value.value);
            }

            release_command_line(&actual);
        }

        TEST_METHOD(parse_command_line_item_switch_width_value_test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("") },
                { wrap("jkl"), wrap("") },
                { wrap("mno"), wrap("MNO") },
                { wrap("pqr"), create_invalid_text() },
                { wrap("stu"), create_invalid_text() },
            };
            TEXT input = wrap("abc --def=DEF -ghi= --jkl \"\" /mno=MNO --pqr --stu");
            COMMAND_LINE_OPTION actual = parse_command_line(&input, true, DEFAULT_MEMORY);

            const COMMAND_LINE_ITEM* item1 = get_command_line_item(&actual, expecteds[0]);
            const COMMAND_LINE_ITEM* item2 = get_command_line_item(&actual, expecteds[1]);
            const COMMAND_LINE_ITEM* item3 = get_command_line_item(&actual, expecteds[2]);
            const COMMAND_LINE_ITEM* item4 = get_command_line_item(&actual, expecteds[3]);
            const COMMAND_LINE_ITEM* item5 = get_command_line_item(&actual, expecteds[4]);
            const COMMAND_LINE_ITEM* item6 = get_command_line_item(&actual, expecteds[5]);

            Assert::IsTrue(has_value_command_line_item(item1));
            Assert::AreEqual(expecteds[0][1].value, item1->value.value);

            Assert::IsTrue(has_value_command_line_item(item2));
            Assert::AreEqual(expecteds[1][1].value, item2->value.value);

            Assert::IsTrue(has_value_command_line_item(item3));
            Assert::AreEqual(expecteds[2][1].value, item3->value.value);

            Assert::IsTrue(has_value_command_line_item(item4));
            Assert::AreEqual(expecteds[3][1].value, item4->value.value);

            Assert::IsFalse(has_value_command_line_item(item5));

            Assert::IsFalse(has_value_command_line_item(item6));

            release_command_line(&actual);
        }

        TEST_METHOD(parse_command_line_item_value_with_space_test)
        {
            TEXT expecteds[][2] = {
                { wrap("0"), wrap("0 0") },
                { wrap("1"), wrap("1 1 ") },
                { wrap("2"), wrap(" 2 2") },
                { wrap("3"), wrap(" 3 3") },
            };
            TEXT input = wrap("abc --0=\"0 0\" -1 \"1 1 \" /2 \" 2 2\" \"-3= 3 3\"");
            COMMAND_LINE_OPTION actual = parse_command_line(&input, true, DEFAULT_MEMORY);
            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                TEXT* expected_key = &expecteds[i][0];
                TEXT* expected_value = &expecteds[i][1];
                const COMMAND_LINE_ITEM* item = get_command_line_item(&actual, expected_key);
                Assert::IsNotNull(item);
                Assert::AreEqual(expected_value->value, item->value.value);
            }

            release_command_line(&actual);
        }

        TEST_METHOD(is_inputted_command_line_item_test)
        {
            TEXT expecteds[][2] = {
                { wrap("def"), wrap("DEF") },
                { wrap("ghi"), wrap("") },
                { wrap("jkl"), wrap("") },
                { wrap("mno"), wrap("MNO") },
                { wrap("pqr"), create_invalid_text() },
                { wrap("stu"), create_invalid_text() },
            };
            TEXT input = wrap("abc --def=DEF -ghi= --jkl \"\" /mno=MNO --pqr --stu");
            COMMAND_LINE_OPTION actual = parse_command_line(&input, true, DEFAULT_MEMORY);

            const COMMAND_LINE_ITEM* item1 = get_command_line_item(&actual, expecteds[0]);
            const COMMAND_LINE_ITEM* item2 = get_command_line_item(&actual, expecteds[1]);
            const COMMAND_LINE_ITEM* item3 = get_command_line_item(&actual, expecteds[2]);
            const COMMAND_LINE_ITEM* item4 = get_command_line_item(&actual, expecteds[3]);
            const COMMAND_LINE_ITEM* item5 = get_command_line_item(&actual, expecteds[4]);
            const COMMAND_LINE_ITEM* item6 = get_command_line_item(&actual, expecteds[5]);

            Assert::IsTrue(is_inputted_command_line_item(item1));
            Assert::AreEqual(expecteds[0][1].value, item1->value.value);

            Assert::IsFalse(is_inputted_command_line_item(item2));
            Assert::AreEqual(expecteds[1][1].value, item2->value.value);

            Assert::IsFalse(is_inputted_command_line_item(item3));
            Assert::AreEqual(expecteds[2][1].value, item3->value.value);

            Assert::IsTrue(is_inputted_command_line_item(item4));
            Assert::AreEqual(expecteds[3][1].value, item4->value.value);

            Assert::IsFalse(is_inputted_command_line_item(item5));

            Assert::IsFalse(is_inputted_command_line_item(item6));

            release_command_line(&actual);
        }

        TEST_METHOD(to_command_line_argument_test)
        {
            auto tests = {
                DATA(_T("a"), std::vector<TEXT>({ wrap("a") })),
                DATA(_T("a b"), std::vector<TEXT>({ wrap("a"), wrap("b") })),
                DATA(_T("a b c"), std::vector<TEXT>({ wrap("a"), wrap("b"), wrap("c") })),
                DATA(_T("a=A"), std::vector<TEXT>({ wrap("a=A") })),
                DATA(_T("-a \"A A\""), std::vector<TEXT>({ wrap("-a"), wrap("A A") })),
                DATA(_T("\"-a=A A\""), std::vector<TEXT>({ wrap("-a=A A") })),
                DATA(_T("-a=\"A A\""), std::vector<TEXT>({ wrap("-a=\"A A\"") })),
                DATA(_T("--a=\"A A\" --b \"B B\" /c \" C C C \""), std::vector<TEXT>({ wrap("--a=\"A A\""), wrap("--b"), wrap("B B"), wrap("/c"), wrap(" C C C ") })),
            };
            for (auto test : tests) {
                auto arg1 = std::get<0>(test.inputs);
                TEXT actual = to_command_line_argument(arg1.data(), arg1.size(), DEFAULT_MEMORY);
                Assert::AreEqual(test.expected, actual.value);
                Assert::AreEqual(get_string_length(test.expected), (size_t)actual.length);
                release_text(&actual);
            }
        }
    };
}
