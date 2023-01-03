#include "pch.h"

extern "C" {
#   include "../Pe.Library/tstring.h"
#   include "../Pe.Library/memory.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(string_test)
    {
    public:

        TEST_METHOD(get_string_length_test)
        {
            TCHAR input[] = _T("abcdefg");
            size_t expected = sizeof(input) / sizeof(TCHAR) - 1/*終端*/;
            auto actual = get_string_length(input);
            Assert::AreEqual(expected, actual);
        }

        TEST_METHOD(fromat_string_test)
        {
            TCHAR input[] = _T("abc %s 123 %d");
            TCHAR actual[1000] = { 0 };
            TCHAR expected[1000] = _T("abc def 123 456");
            format_string(actual, input, _T("def"), 456);

            Assert::AreEqual(expected, actual);
        }

        TEST_METHOD(concat_string_test)
        {
            TCHAR input[] = _T("def");
            TCHAR actual[1000] = _T("abc");
            TCHAR expected[1000] = _T("abcdef");
            concat_string(actual, input);

            Assert::AreEqual(expected, actual);
        }

        TEST_METHOD(copy_string_test)
        {
            TCHAR input[] = _T("def");
            TCHAR actual[1000] = _T("abc");
            TCHAR expected[1000] = _T("def");
            copy_string(actual, input);

            Assert::AreEqual(expected, actual);
        }

        TEST_METHOD(clone_string_test)
        {
            TCHAR input[] = _T("abc");
            TCHAR expected[] = _T("abc");
            TCHAR* actual = clone_string(input, DEFAULT_MEMORY_ARENA);

            Assert::AreEqual(expected, actual);
            Assert::IsFalse(input == actual);

            release_string(actual, DEFAULT_MEMORY_ARENA);
        }

        TEST_METHOD(clone_string_with_length_test)
        {
            TCHAR input[] = _T("abc");
            TCHAR expected[] = _T("ab");
            TCHAR* actual = clone_string_with_length(input, 2, DEFAULT_MEMORY_ARENA);

            Assert::AreEqual(expected, actual);
            Assert::IsFalse(input == actual);

            release_string(actual, DEFAULT_MEMORY_ARENA);
        }

    };
}
