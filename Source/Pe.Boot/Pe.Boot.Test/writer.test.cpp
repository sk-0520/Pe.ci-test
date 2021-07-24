#include "pch.h"
#include <functional>

extern "C" {
#   include "../Pe.Boot/memory.h"
#   include "../Pe.Boot/writer.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(writer_test)
    {
        template<size_t size>
        struct STRING_BUFFER
        {
            TCHAR values[size] = { 0 };
            size_t length = 0;

            static bool receive_c(const WRITE_CHARACTER_DATA* data)
            {
                values[length++] = data->value;
                values[length] = 0;
                return true;
            }

            bool receive_s(const WRITE_STRING_DATA* data)
            {
                copy_memory(values + length, data->value, data->length * sizeof(TCHAR));
                length += data->length;
                values[length] = 0;
                return true;
            }

            static bool write_s(void* receiver, const TCHAR c)
            {
                return static_cast<STRING_BUFFER<size>*>(receiver)->receive_c(c);
            }

            static bool write_s(void* receiver, const WRITE_STRING_DATA* data)
            {
                return static_cast<STRING_BUFFER<size>*>(receiver)->receive_s(data);
            }
        };
        using BUF = STRING_BUFFER<1024>;

    public:

        TEST_METHOD(write_to_primitive_boolean_test)
        {
            auto tests = {
                DATA(_T("TRUE"), true, true),
                DATA(_T("true"), true, false),
                DATA(_T("FALSE"), false, true),
                DATA(_T("false"), false, false),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2, arg3] = test.inputs;
                write_to_primitive_boolean(&BUF::write_s, &actual, arg2, arg3);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_to_primitive_integer_test)
        {
            auto tests = {
                DATA(_T("0"), 0, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, false, 0, '\0'),
                DATA(_T("1234"), 1234, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, false, 0, '\0'),
                DATA(_T("12345678"), 12345678, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, false, 0, '\0'),

                DATA(_T("+0"), 0, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, true, 0, '\0'),
                DATA(_T("+123"), 123, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, true, 0, '\0'),
                DATA(_T("-123"), -123, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, true, 0, '\0'),

                DATA(_T("0"), 0, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, false, 4, '\0'),
                DATA(_T("   0"), 0, WRITE_PADDING_NONE, WRITE_ALIGN_RIGHT, false, 4, '\0'),
                DATA(_T("1234"), 1234, WRITE_PADDING_NONE, WRITE_ALIGN_RIGHT, false, 4, '\0'),
                DATA(_T("12345"), 12345, WRITE_PADDING_NONE, WRITE_ALIGN_RIGHT, false, 4, '\0'),

                DATA(_T("+1"), 1, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, true, 4, '\0'),
                DATA(_T("  +1"), 1, WRITE_PADDING_NONE, WRITE_ALIGN_RIGHT, true, 4, '\0'),
                DATA(_T("-1"), -1, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, true, 4, '\0'),
                DATA(_T("  -1"), -1, WRITE_PADDING_NONE, WRITE_ALIGN_RIGHT, true, 4, '\0'),
                DATA(_T("-1"), -1, WRITE_PADDING_NONE, WRITE_ALIGN_LEFT, false, 4, '\0'),
                DATA(_T("  -1"), -1, WRITE_PADDING_NONE, WRITE_ALIGN_RIGHT, false, 4, '\0'),

                DATA(_T("0001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, false, 4, '\0'),
                DATA(_T("0001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, false, 4, '\0'),
                DATA(_T("-001"), -1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, false, 4, '\0'),
                DATA(_T("-001"), -1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, false, 4, '\0'),
                DATA(_T("+001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, true, 4, '\0'),
                DATA(_T("+001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 4, '\0'),

            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2, arg3, arg4, arg5, arg6, arg7] = test.inputs;
                write_to_primitive_integer(&BUF::write_s, &actual, arg2, arg3, arg4, arg5, arg6, arg7);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

    };
}
