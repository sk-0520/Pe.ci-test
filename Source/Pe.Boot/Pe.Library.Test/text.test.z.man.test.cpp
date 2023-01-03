#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(text_manipulate_test)
    {
    public:

        TEST_METHOD(add_text_normal_test)
        {
            auto tests = {
                DATA(_T("ab"), wrap("a"), wrap("b")),
                DATA(_T("a"), wrap("a"), wrap("")),
                DATA(_T("b"), wrap(""), wrap("b")),
                DATA(_T("🐭🐮🐯🐰🐉🐍🐴🐏🐵🐔🐶🐷"), wrap("🐭🐮🐯🐰🐉🐍"), wrap("🐴🐏🐵🐔🐶🐷")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                TEXT actual = add_text(&arg1, &arg2, DEFAULT_MEMORY_ARENA);
                Assert::AreEqual(test.expected, actual.value);
                release_text(&actual);
            }
        }

        TEST_METHOD(add_text_fail_test)
        {
            TEXT a = wrap("");
            TEXT b = create_invalid_text();

            TEXT actual_ab = add_text(&a, &b, DEFAULT_MEMORY_ARENA);
            Assert::IsTrue(is_enabled_text(&actual_ab));

            TEXT actual_ba = add_text(&b, &a, DEFAULT_MEMORY_ARENA);
            Assert::IsTrue(is_enabled_text(&actual_ba));

            TEXT actual_bb = add_text(&b, &b, DEFAULT_MEMORY_ARENA);
            Assert::IsFalse(is_enabled_text(&actual_bb));

            release_text(&actual_ab);
            release_text(&actual_ba);
            release_text(&actual_bb);
        }

        TEST_METHOD(join_text_test)
        {
            TCHAR* expected1 = _T("1,2,3");
            TEXT input1[] = {
                wrap("1"),
                wrap("2"),
                wrap("3"),
            };
            TEXT sep1 = wrap(",");
            TEXT actual1 = join_text(&sep1, input1, SIZEOF_ARRAY(input1), IGNORE_EMPTY_NONE, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(expected1, actual1.value);
            release_text(&actual1);

            TCHAR* expected2 = _T("123");
            TEXT input2[] = {
                wrap(""),
                wrap("1"),
                wrap(""),
                wrap("2"),
                wrap(""),
                wrap("3"),
                wrap(""),
            };
            TEXT sep2 = wrap("");
            TEXT actual2 = join_text(&sep2, input2, SIZEOF_ARRAY(input2), IGNORE_EMPTY_NONE, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(expected2, actual2.value);
            release_text(&actual2);

            TCHAR* expected3_1 = _T(",1,,2, ,,3,");
            TCHAR* expected3_2 = _T("1,2, ,3");
            TCHAR* expected3_3 = _T("1,2,3");
            TEXT input3[] = {
                wrap(""),
                wrap("1"),
                wrap(""),
                wrap("2"),
                wrap(" "),
                wrap(""),
                wrap("3"),
                wrap(""),
            };
            TEXT sep3 = wrap(",");
            TEXT actual3_1 = join_text(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_NONE, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(expected3_1, actual3_1.value);
            release_text(&actual3_1);

            TEXT actual3_2 = join_text(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_ONLY, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(expected3_2, actual3_2.value);
            release_text(&actual3_2);

            TEXT actual3_3 = join_text(&sep3, input3, SIZEOF_ARRAY(input3), IGNORE_EMPTY_WHITESPACE, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(expected3_3, actual3_3.value);
            release_text(&actual3_3);
        }

        TEST_METHOD(is_empty_text_test)
        {
            auto tests = {
                DATA(false, wrap("a")),
                DATA(false, wrap(" ")),
                DATA(false, wrap("\t")),
                DATA(true, wrap("")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool actual = is_empty_text(&arg1);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(is_white_space_text_test)
        {
            auto tests = {
                DATA(false, wrap("a")),
                DATA(true, wrap(" ")),
                DATA(true, wrap("\t")),
                DATA(true, wrap("")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                bool actual = is_whitespace_text(&arg1);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(trim_text_test)
        {
            auto tests = {
                DATA(_T("a"), wrap(" a "), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', })),
                DATA(_T("a "), wrap(" a "), TRIM_TARGETS_HEAD, std::vector<TCHAR>({ ' ', })),
                DATA(_T(" a"), wrap(" a "), TRIM_TARGETS_TAIL, std::vector<TCHAR>({ ' ', })),
                DATA(_T("a b"), wrap("      a b    "), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', })),
                DATA(_T("a"), wrap(" \t a\t \t"), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T("a\tb c"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(" \t a\tb c"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_TAIL, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T("a\tb c\t \t"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_HEAD, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(" \t a\tb c\t \t"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_NONE, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(" \t \t \t"), wrap(" \t \t \t"), TRIM_TARGETS_NONE, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(""), wrap(" \t \t \t"), TRIM_TARGETS_HEAD, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(_T(""), wrap(" \t \t \t"), TRIM_TARGETS_TAIL, std::vector<TCHAR>({ ' ', '\t' })),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TRIM_TARGETS arg2 = std::get<1>(test.inputs);
                auto arg3 = std::get<2>(test.inputs);
                TEXT actual = trim_text(&arg1, arg2, arg3.data(), arg3.size(), DEFAULT_MEMORY_ARENA);
                Assert::AreEqual(test.expected, actual.value);
                Assert::IsTrue(actual.library.need_release);
                release_text(&actual);
            }
        }

        TEST_METHOD(trim_white_space_text_test)
        {
            auto tests = {
                DATA(_T("a"), wrap(" a ")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT actual = trim_whitespace_text(&arg1, DEFAULT_MEMORY_ARENA);
                Assert::AreEqual(test.expected, actual.value);
                Assert::IsTrue(actual.library.need_release);
                release_text(&actual);
            }
        }

        TEST_METHOD(trim_text_stack_test)
        {
            auto tests = {
                DATA(wrap("a"), wrap(" a "), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', })),
                DATA(wrap("a "), wrap(" a "), TRIM_TARGETS_HEAD, std::vector<TCHAR>({ ' ', })),
                DATA(wrap(" a"), wrap(" a "), TRIM_TARGETS_TAIL, std::vector<TCHAR>({ ' ', })),
                DATA(wrap("a b"), wrap("      a b    "), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', })),
                DATA(wrap("a"), wrap(" \t a\t \t"), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(wrap("a\tb c"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_BOTH, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(wrap(" \t a\tb c"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_TAIL, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(wrap("a\tb c\t \t"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_HEAD, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(wrap(" \t a\tb c\t \t"), wrap(" \t a\tb c\t \t"), TRIM_TARGETS_NONE, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(wrap(" \t \t \t"), wrap(" \t \t \t"), TRIM_TARGETS_NONE, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(wrap(""), wrap(" \t \t \t"), TRIM_TARGETS_HEAD, std::vector<TCHAR>({ ' ', '\t' })),
                DATA(wrap(""), wrap(" \t \t \t"), TRIM_TARGETS_TAIL, std::vector<TCHAR>({ ' ', '\t' })),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TRIM_TARGETS arg2 = std::get<1>(test.inputs);
                auto arg3 = std::get<2>(test.inputs);
                TEXT actual = trim_text_stack(&arg1, arg2, arg3.data(), arg3.size());
                Assert::IsTrue(is_equals_text(&test.expected, &actual, false), test.expected.value);
                Assert::IsFalse(actual.library.need_release);
            }
        }

        TEST_METHOD(trim_white_space_text_stack_test)
        {
            auto tests = {
                DATA(wrap("a"), wrap(" a ")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT actual = trim_whitespace_text_stack(&arg1);
                Assert::IsTrue(is_equals_text(&test.expected, &actual, false), test.expected.value);
                Assert::IsFalse(actual.library.need_release);
            }
        }

        static TEXT split_text_EASY_CSV(const TEXT* source, size_t* next_index, const MEMORY_ARENA_RESOURCE* memory_arena_resource)
        {
            ssize_t index = index_of_character(source, _T(','), INDEX_START_POSITION_HEAD);
            if (index == -1) {
                *next_index = source->length;
                return wrap_text_with_length(source->value, source->length, false, memory_arena_resource);
            }

            *next_index = index + 1;
            return wrap_text_with_length(source->value, index, false, memory_arena_resource);
        }

        TEST_METHOD(split_text_EASY_CSV_test)
        {
            TCHAR* expected[] = {
                _T("a"),
                _T("b"),
                _T("c"),
                _T("d"),
                _T(""),
                _T(" "),
                _T("e"),
                _T(""),
            };
            TEXT input = wrap("a,b,c,d,, ,e,");
            OBJECT_LIST actual = split_text(&input, split_text_EASY_CSV, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(sizeof(expected) / sizeof(expected[0]), actual.length);
            for (size_t i = 0; i < actual.length; i++) {
                OBJECT_RESULT_VALUE result = get_object_list(&actual, i);
                TEXT* t = (TEXT*)result.value;
                TCHAR* s = expected[i];
                Assert::AreEqual(s, t->value);
            }
            release_object_list(&actual, true);
        }

        TEST_METHOD(split_newline_text_test)
        {
            TCHAR* expected[] = {
                _T("abc"),
                _T("def"),
                _T("ghi"),
                _T("jkl"),
                _T(""),
                _T(""),
                _T(""),
                _T("xyz"),
                _T(""),
            };
            TEXT input = wrap_text(
                _T("abc\r")
                _T("def\n")
                _T("ghi\r\n")
                _T("jkl\r\n")
                _T("\r") //CRLF
                _T("\n") //----
                _T("\n")
                _T("\r")
                _T("xyz\r\n")
            );
            OBJECT_LIST actual = split_newline_text(&input, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(sizeof(expected) / sizeof(expected[0]), actual.length);
            for (size_t i = 0; i < actual.length; i++) {
                OBJECT_RESULT_VALUE result = get_object_list(&actual, i);
                TEXT* t = (TEXT*)result.value;
                TCHAR* s = expected[i];
                Assert::AreEqual(s, t->value);
            }
            release_object_list(&actual, true);
        }
    };
}
