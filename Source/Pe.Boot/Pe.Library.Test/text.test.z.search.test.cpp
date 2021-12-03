#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(text_search_test)
    {
    public:

        TEST_METHOD(search_text_test)
        {
            auto input = wrap("abcDEF-ABCdef");
            auto tests = {
                DATA(_T("abcDEF-ABCdef"), input, wrap("abc"), false),
                DATA(_T("ABCdef"), input, wrap("ABC"), false),
                DATA(_T("def"), input, wrap("def"), false),
                DATA(_T("DEF-ABCdef"), input, wrap("DEF"), false),

                DATA(_T("abcDEF-ABCdef"), input, wrap("abc"), true),
                DATA(_T("abcDEF-ABCdef"), input, wrap("ABC"), true),
                DATA(_T("DEF-ABCdef"), input, wrap("def"), true),
                DATA(_T("DEF-ABCdef"), input, wrap("DEF"), true),
                DATA(_T("DEF-ABCdef"), input, wrap("dEf"), true),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                bool& arg3 = std::get<2>(test.inputs);
                TEXT actual = search_text(&arg1, &arg2, arg3);
                Assert::IsFalse(actual.library.need_release);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(search_text_notfound_test)
        {
            auto input1 = wrap("abcDEF-ABCdef");
            auto input2 = wrap("Abc");
            TEXT actual = search_text(&input1, &input2, false);
            Assert::IsFalse(is_enabled_text(&actual));
        }

        TEST_METHOD(find_character_test)
        {
            auto input = wrap("abcDEF-ABCdef");
            auto tests = {
                DATA(_T("abcDEF-ABCdef"), input, _T('a')),
                DATA(_T("ABCdef"), input, _T('A')),
                DATA(_T("def"), input, _T('d')),
                DATA(_T("DEF-ABCdef"), input, _T('D')),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TCHAR arg2 = std::get<1>(test.inputs);
                TEXT actual = find_character(&arg1, arg2);
                Assert::IsFalse(actual.library.need_release);
                Assert::AreEqual(test.expected, actual.value);
            }
        }

        TEST_METHOD(find_character_notfound_test)
        {
            auto input1 = wrap("abcDEF-ABCdef");
            TEXT actual = find_character(&input1, 'x');
            Assert::IsFalse(is_enabled_text(&actual));
        }

        TEST_METHOD(index_character_test)
        {
            TEXT input = wrap("abcdefgABCDEFG");
            auto tests = {
                DATA((ssize_t)0, input, _T('a')),
                DATA((ssize_t)1, input, _T('b')),
                DATA((ssize_t)7, input, _T('A')),
                DATA((ssize_t)-1, input, _T('x')),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TCHAR arg2 = std::get<1>(test.inputs);
                auto actual = index_of_character(&arg1, arg2);
                Assert::AreEqual(test.expected, actual);
            }
        }

        TEST_METHOD(is_equals_text_test)
        {
            TCHAR buffers[] = _T("abcDEFABCdef");
            TEXT abc = wrap_text_with_length(buffers + (3 * 0), 3, false, NULL);
            TEXT DEF = wrap_text_with_length(buffers + (3 * 1), 3, false, NULL);
            TEXT ABC = wrap_text_with_length(buffers + (3 * 2), 3, false, NULL);
            TEXT def = wrap_text_with_length(buffers + (3 * 3), 3, false, NULL);

            auto tests = {
                DATA(true, wrap("abc"), wrap("abc"), false),

                DATA(false, wrap("abc"), wrap("Abc"), false),
                DATA(false, wrap("aBc"), wrap("ABc"), false),
                DATA(false, wrap("abC"), wrap("ABC"), false),
                DATA(false, wrap("abc"), wrap("ABC"), false),

                DATA(true, wrap("abc"), wrap("Abc"), true),
                DATA(true, wrap("aBc"), wrap("ABc"), true),
                DATA(true, wrap("abC"), wrap("ABC"), true),
                DATA(true, wrap("abc"), wrap("ABC"), true),

                DATA(false, wrap("abcd"), wrap("ABC"), false),
                DATA(false, wrap("abc"), wrap("ABCD"), false),
                DATA(false, wrap("abcd"), wrap("ABC"), true),
                DATA(false, wrap("abc"), wrap("ABCD"), true),

                DATA(true, abc, abc, false),
                DATA(false, abc, ABC, false),
                DATA(true, def, def, false),
                DATA(false, def, DEF, false),

                DATA(true, abc, abc, true),
                DATA(true, abc, ABC, true),
                DATA(true, def, def, true),
                DATA(true, def, DEF, true),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                bool arg3 = std::get<2>(test.inputs);
                auto actual = is_equals_text(&arg1, &arg2, arg3);
                if (test.expected) {
                    Assert::IsTrue(actual);
                } else {
                    Assert::IsFalse(actual);
                }
            }
        }

        TEST_METHOD(compare_text_test)
        {
            auto tests = {
                DATA(0, wrap("abc"), wrap("abc"), false),
                DATA(-1, wrap("abc"), wrap("def"), false),
                DATA(+1, wrap("def"), wrap("abc"), false),
                DATA(-1, wrap("abc"), wrap("ABC"), false),
                DATA(-1, wrap("abc"), wrap("DEF"), false),
                DATA(+1, wrap("def"), wrap("ABC"), false),

                DATA(0, wrap("abc"), wrap("abc"), true),
                DATA(-1, wrap("abc"), wrap("def"), true),
                DATA(+1, wrap("def"), wrap("abc"), true),
                DATA(0, wrap("abc"), wrap("ABC"), true),
                DATA(-1, wrap("abc"), wrap("DEF"), true),
                DATA(+1, wrap("def"), wrap("ABC"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                bool arg3 = std::get<2>(test.inputs);
                auto actual = compare_text(&arg1, &arg2, arg3);
                switch (test.expected) {
                    case -1:
                        Assert::IsTrue(actual < 0);
                        break;
                    case 0:
                        Assert::IsTrue(actual == 0);
                        break;
                    case +1:
                        Assert::IsTrue(0 < actual);
                        break;
                    default:
                        _ASSERT(false);
                }
            }
        }

        TEST_METHOD(compare_text_length_test)
        {
            TCHAR buffers[] = _T("abcDEFABCdef");
            TEXT abc = wrap_text_with_length(buffers + (3 * 0), 3, false, NULL);
            TEXT DEF = wrap_text_with_length(buffers + (3 * 1), 3, false, NULL);
            TEXT ABC = wrap_text_with_length(buffers + (3 * 2), 3, false, NULL);
            TEXT def = wrap_text_with_length(buffers + (3 * 3), 3, false, NULL);

            auto tests = {
                DATA(0, abc, abc, false),
                DATA(-1, abc, def, false),
                DATA(+1, def, abc, false),
                DATA(-1, abc, ABC, false),
                DATA(-1, abc, DEF, false),
                DATA(+1, def, ABC, false),

                DATA(0, abc, abc, true),
                DATA(-1, abc, def, true),
                DATA(+1, def, abc, true),
                DATA(0, abc, ABC, true),
                DATA(-1, abc, DEF, true),
                DATA(+1, def, ABC, true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                bool arg3 = std::get<2>(test.inputs);
                auto actual = compare_text(&arg1, &arg2, arg3);
                switch (test.expected) {
                    case -1:
                        Assert::IsTrue(actual < 0);
                        break;
                    case 0:
                        Assert::IsTrue(actual == 0);
                        break;
                    case +1:
                        Assert::IsTrue(0 < actual);
                        break;
                    default:
                        _ASSERT(false);
                }
            }
        }

        TEST_METHOD(compare_text_detail_compare_text_test_test)
        {
            auto tests = {
                DATA(0, wrap("abc"), wrap("abc"), false),
                DATA(-1, wrap("abc"), wrap("def"), false),
                DATA(+1, wrap("def"), wrap("abc"), false),
                DATA(-1, wrap("abc"), wrap("ABC"), false),
                DATA(-1, wrap("abc"), wrap("DEF"), false),
                DATA(+1, wrap("def"), wrap("ABC"), false),

                DATA(0, wrap("abc"), wrap("abc"), true),
                DATA(-1, wrap("abc"), wrap("def"), true),
                DATA(+1, wrap("def"), wrap("abc"), true),
                DATA(0, wrap("abc"), wrap("ABC"), true),
                DATA(-1, wrap("abc"), wrap("DEF"), true),
                DATA(+1, wrap("def"), wrap("ABC"), true),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
#pragma warning(push)
#pragma warning(disable:26812)
                TEXT_COMPARE_MODE arg3 = std::get<2>(test.inputs) ? TEXT_COMPARE_MODE_IGNORE_CASE : TEXT_COMPARE_MODE_NONE;
#pragma warning(pop)
                auto actual = compare_text_detail(&arg1, &arg2, -1, arg3, LOCALE_TYPE_INVARIANT);
                Assert::IsTrue(actual.success);
                switch (test.expected) {
                    case -1:
                        Assert::IsTrue(actual.compare < 0);
                        break;
                    case 0:
                        Assert::IsTrue(actual.compare == 0);
                        break;
                    case +1:
                        Assert::IsTrue(0 < actual.compare);
                        break;
                    default:
                        _ASSERT(false);
                }
            }

        }

        TEST_METHOD(starts_with_text_test)
        {
            auto tests = {
                DATA(true, wrap("abc"), wrap("a")),
                DATA(false, wrap("abc"), wrap("b")),

                DATA(true, wrap("abc"), wrap("ab")),
                DATA(false, wrap("abc"), wrap("bc")),
                DATA(false, wrap("abc"), wrap("ac")),

                DATA(true, wrap("abc"), wrap("abc")),
                DATA(false, wrap("abc"), wrap("abcd")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                auto actual = starts_with_text(&arg1, &arg2);
                if (test.expected) {
                    Assert::IsTrue(actual, arg2.value);
                } else {
                    Assert::IsFalse(actual, arg2.value);
                }
            }
        }
    };
}
