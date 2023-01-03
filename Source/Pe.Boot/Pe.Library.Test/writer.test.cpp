#include "pch.h"
#include <functional>

extern "C" {
#   include "../Pe.Library/memory.h"
#   include "../Pe.Library/writer.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(writer_test)
    {
        template<size_t size>
        struct STRING_BUFFER
        {
            TCHAR values[size] = { 0 };
            size_t length = 0;

            WRITE_RESULT receive_c(const WRITE_CHARACTER_DATA* data)
            {
                values[length++] = data->value;
                values[length] = 0;
                return write_success(1);
            }

            WRITE_RESULT receive_s(const WRITE_STRING_DATA* data)
            {
                copy_memory(values + length, data->value, data->length * sizeof(TCHAR));
                length += data->length;
                values[length] = 0;
                return write_success(data->length);
            }

            static WRITE_RESULT write_c(const WRITE_CHARACTER_DATA* data)
            {
                Assert::IsNotNull(data);
                Assert::IsNotNull(data->receiver);
                auto _this = static_cast<STRING_BUFFER<size>*>(data->receiver);
                return _this->receive_c(data);
            }

            static WRITE_RESULT write_s(const WRITE_STRING_DATA* data)
            {
                Assert::IsNotNull(data);
                Assert::IsNotNull(data->receiver);
                auto _this = static_cast<STRING_BUFFER<size>*>(data->receiver);
                return _this->receive_s(data);
            }
        };
        using BUF = STRING_BUFFER<1024>;

    public:

        TEST_METHOD(get_write_format_flags_test)
        {
            struct EXPECTED
            {
                size_t length;
                WRITE_FORMAT_FLAGS flags;
            };

            auto tests = {
                //DATA<EXPECTED, TEXT>({ 0, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, false, false } }, wrap("")),
                //DATA<EXPECTED, TEXT>({ 0, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, false, false } }, wrap("x")),
                //DATA<EXPECTED, TEXT>({ 1, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, false, false } }, wrap("-")),
                //DATA<EXPECTED, TEXT>({ 1, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, false, true } }, wrap("+")),
                //DATA<EXPECTED, TEXT>({ 1, { WRITE_ALIGN_RIGHT, WRITE_PADDING_SPACE, false, false } }, wrap(" ")),
                DATA<EXPECTED, TEXT>({ 1, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, true, false } }, wrap("#")),
                DATA<EXPECTED, TEXT>({ 1, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, false, true } }, wrap("+")),
                DATA<EXPECTED, TEXT>({ 2, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, true, true } }, wrap("#+")),
                DATA<EXPECTED, TEXT>({ 2, { WRITE_ALIGN_LEFT, WRITE_PADDING_SPACE, true, true } }, wrap("+#")),
                DATA<EXPECTED, TEXT>({ 4, { WRITE_ALIGN_LEFT, WRITE_PADDING_ZERO, true, true } }, wrap("#+0 ")),
                DATA<EXPECTED, TEXT>({ 3, { WRITE_ALIGN_LEFT, WRITE_PADDING_ZERO, true, true } }, wrap("#+0x")),
                DATA<EXPECTED, TEXT>({ 3, { WRITE_ALIGN_LEFT, WRITE_PADDING_ZERO, true, true } }, wrap("#+0x ")),
            };
            for (auto test : tests) {
                WRITE_FORMAT_FLAGS actual_flags = {
                    /*.align = */ WRITE_ALIGN_LEFT,
                    /*.padding = */ WRITE_PADDING_SPACE,
                    /*.alternate_form = */ false,
                    /*.show_sign = */ false,
                };
                auto [arg2] = test.inputs;
                size_t actual_length = get_write_format_flags(&actual_flags, &arg2);
                Assert::AreEqual(test.expected.length, actual_length);
                Assert::AreEqual<int>(test.expected.flags.align, actual_flags.align);
                Assert::AreEqual<int>(test.expected.flags.padding, actual_flags.padding);
                Assert::AreEqual(test.expected.flags.alternate_form, actual_flags.alternate_form);
                Assert::AreEqual(test.expected.flags.show_sign, actual_flags.show_sign);
            }
        }

        TEST_METHOD(write_primitive_boolean_test)
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
                write_primitive_boolean(&BUF::write_s, &actual, arg2, arg3);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_primitive_integer_test)
        {
            auto tests = {
                DATA(_T("0"), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T('\0')),
                DATA(_T("1234"), 1234, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T('\0')),
                DATA(_T("12345678"), 12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T('\0')),

                DATA(_T("+0"), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 0, _T('\0')),
                DATA(_T("+123"), 123, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 0, _T('\0')),
                DATA(_T("-123"), -123, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 0, _T('\0')),

                DATA(_T("0   "), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 4, _T('\0')),
                DATA(_T("   0"), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),
                DATA(_T("1234"), 1234, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),
                DATA(_T("12345"), 12345, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),

                DATA(_T("+1  "), 1, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 4, _T('\0')),
                DATA(_T("  +1"), 1, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 4, _T('\0')),
                DATA(_T("-1  "), -1, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 4, _T('\0')),
                DATA(_T("  -1"), -1, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 4, _T('\0')),
                DATA(_T("-1  "), -1, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 4, _T('\0')),
                DATA(_T("  -1"), -1, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),

                DATA(_T("0001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, false, 4, _T('\0')),
                DATA(_T("0001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),
                DATA(_T("-001"), -1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, false, 4, _T('\0')),
                DATA(_T("-001"), -1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),
                DATA(_T("+001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, true, 4, _T('\0')),
                DATA(_T("+001"), 1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 4, _T('\0')),

                DATA(_T("0"), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(',')),
                DATA(_T("1,234"), 1234, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(',')),
                DATA(_T("12,345,678"), 12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(',')),

                DATA(_T("       0"), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 8, _T(',')),
                DATA(_T("   1,234"), 1234, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 8, _T(',')),
                DATA(_T("12,345,678"), 12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 8, _T(',')),

                DATA(_T("      +0"), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("  +1,234"), 1234, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("+12,345,678"), 12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 8, _T(',')),

                DATA(_T("+0000000"), 0, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("+001,234"), 1234, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("+12,345,678"), 12345678, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2, arg3, arg4, arg5, arg6, arg7] = test.inputs;
                write_primitive_integer(&BUF::write_s, &actual, DEFAULT_MEMORY_ARENA, arg2, arg3, arg4, arg5, arg6, arg7);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_primitive_uinteger_test)
        {
            auto tests = {
                DATA(_T("0"), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T('\0')),
                DATA(_T("1234"), (size_t)1234, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T('\0')),
                DATA(_T("12345678"), (size_t)12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T('\0')),

                DATA(_T("+0"), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 0, _T('\0')),
                DATA(_T("+123"), (size_t)123, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 0, _T('\0')),

                DATA(_T("0   "), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 4, _T('\0')),
                DATA(_T("   0"), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),
                DATA(_T("1234"), (size_t)1234, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),
                DATA(_T("12345"), (size_t)12345, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),

                DATA(_T("+1  "), (size_t)1, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, 4, _T('\0')),
                DATA(_T("  +1"), (size_t)1, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 4, _T('\0')),

                DATA(_T("0001"), (size_t)1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, false, 4, _T('\0')),
                DATA(_T("0001"), (size_t)1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, false, 4, _T('\0')),
                DATA(_T("+001"), (size_t)1, WRITE_PADDING_ZERO, WRITE_ALIGN_LEFT, true, 4, _T('\0')),
                DATA(_T("+001"), (size_t)1, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 4, _T('\0')),

                DATA(_T("0"), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(',')),
                DATA(_T("1,234"), (size_t)1234, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(',')),
                DATA(_T("12,345,678"), (size_t)12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, 0, _T(',')),

                DATA(_T("       0"), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 8, _T(',')),
                DATA(_T("   1,234"), (size_t)1234, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 8, _T(',')),
                DATA(_T("12,345,678"), (size_t)12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, false, 8, _T(',')),

                DATA(_T("      +0"), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("  +1,234"), (size_t)1234, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("+12,345,678"), (size_t)12345678, WRITE_PADDING_SPACE, WRITE_ALIGN_RIGHT, true, 8, _T(',')),

                DATA(_T("+0000000"), (size_t)0, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("+001,234"), (size_t)1234, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
                DATA(_T("+12,345,678"), (size_t)12345678, WRITE_PADDING_ZERO, WRITE_ALIGN_RIGHT, true, 8, _T(',')),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2, arg3, arg4, arg5, arg6, arg7] = test.inputs;
                write_primitive_uinteger(&BUF::write_s, &actual, DEFAULT_MEMORY_ARENA, arg2, arg3, arg4, arg5, arg6, arg7);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        //TODO: そもそも間違ってる
        TEST_METHOD(write_primitive_hex_test)
        {
            auto tests = {
                DATA(_T("0"), 0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("1"), 1, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("9"), 9, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("a"), 10, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("f"), 15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("10"), 16, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),

                DATA(_T("A"), 10, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, false, 0),
                DATA(_T("F"), 15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, false, 0),

                DATA(_T("0xf"), 15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, true, 0),
                DATA(_T("0x10"), 16, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, true, 0),
                DATA(_T("0XF"), 15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, true, 0),
                DATA(_T("0X10"), 16, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, true, 0),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2, arg3, arg4, arg5, arg6, arg7] = test.inputs;
                write_primitive_hex(&BUF::write_s, &actual, DEFAULT_MEMORY_ARENA, arg2, arg3, arg4, arg5, arg6, arg7);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        //TODO: そもそも間違ってる
        TEST_METHOD(write_primitive_uhex_test)
        {
            auto tests = {
                DATA(_T("0"), (size_t)0, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("1"), (size_t)1, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("9"), (size_t)9, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("a"), (size_t)10, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("f"), (size_t)15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),
                DATA(_T("10"), (size_t)16, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, false, 0),

                DATA(_T("A"), (size_t)10, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, false, 0),
                DATA(_T("F"), (size_t)15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, false, 0),

                DATA(_T("0xf"), (size_t)15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, true, 0),
                DATA(_T("0x10"), (size_t)16, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, false, true, 0),
                DATA(_T("0XF"), (size_t)15, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, true, 0),
                DATA(_T("0X10"), (size_t)16, WRITE_PADDING_SPACE, WRITE_ALIGN_LEFT, true, true, 0),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2, arg3, arg4, arg5, arg6, arg7] = test.inputs;
                write_primitive_uhex(&BUF::write_s, &actual, DEFAULT_MEMORY_ARENA, arg2, arg3, arg4, arg5, arg6, arg7);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_primitive_character_test)
        {
            auto tests = {
                DATA(_T("A"), _T('A'), WRITE_ALIGN_LEFT, 0),
                DATA(_T("a"), _T('a'), WRITE_ALIGN_LEFT, 0),
                DATA(_T("a  "), _T('a'), WRITE_ALIGN_LEFT, 3),
                DATA(_T("  a"), _T('a'), WRITE_ALIGN_RIGHT, 3),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2, arg3, arg4] = test.inputs;
                write_primitive_character(&BUF::write_s, &actual, DEFAULT_MEMORY_ARENA, arg2, arg3, arg4);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_primitive_pointer_test)
        {
            void* np1 = NULL;
            void* np2 = (void*)1000;
            auto tests = {
#ifdef _WIN64
                DATA(_T("0000000000000000"), np1),
                DATA(_T("00000000000003e8"), np2),
#else
                DATA(_T("00000000"), np1),
                DATA(_T("000003e8"), np2),
#endif

            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2 ] = test.inputs;
                write_primitive_pointer(&BUF::write_s, &actual, arg2);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_address_string_test)
        {
            auto tests = {
                DATA(_T("abc"), _T("abc")),
                DATA(_T("💣"), _T("💣")),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2] = test.inputs;
                write_address_string(&BUF::write_s, &actual, arg2, WRITE_ALIGN_LEFT, 0);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_address_text_test)
        {
            auto tests = {
                DATA(_T("abc"), wrap("abc")),
                DATA(_T("💣"), wrap("💣")),
            };
            for (auto test : tests) {
                BUF actual;
                auto [arg2] = test.inputs;
                write_address_text(&BUF::write_s, &actual, &arg2, WRITE_ALIGN_LEFT, 0);
                Assert::AreEqual(test.expected, actual.values);
            }
        }

        TEST_METHOD(write_format_plaintext_test)
        {
            TCHAR* expected1 = _T("abc");
            TEXT format1 = wrap("abc");
            BUF actual1;
            write_format(&BUF::write_s, &BUF::write_c, &actual1, DEFAULT_MEMORY_ARENA, &format1);
            Assert::AreEqual(expected1, actual1.values);

            TCHAR* expected2 = _T("%a%b%c%");
            TEXT format2 = wrap("%%a%%b%%c%%");
            BUF actual2;
            write_format(&BUF::write_s, &BUF::write_c, &actual2, DEFAULT_MEMORY_ARENA, &format2);
            Assert::AreEqual(expected2, actual2.values);
        }

        TEST_METHOD(write_format_d_test)
        {
            TCHAR* expected1 = _T("123 [ 456] -123 +456");
            TEXT format1 = wrap("%d [% 4d] %d %+d");
            BUF actual1;
            write_format(&BUF::write_s, &BUF::write_c, &actual1, DEFAULT_MEMORY_ARENA, &format1, 123, 456, -123, 456);
            Assert::AreEqual(expected1, actual1.values);
        }

        TEST_METHOD(write_format_c_test)
        {
            TCHAR* expected1 = _T("1 a A");
            TEXT format1 = wrap("%c %c %c");
            BUF actual1;
            write_format(&BUF::write_s, &BUF::write_c, &actual1, DEFAULT_MEMORY_ARENA, &format1, _T('1'), _T('a'), _T('A'));
            Assert::AreEqual(expected1, actual1.values);
        }

        TEST_METHOD(write_format_b_test)
        {
            TCHAR* expected1 = _T("true false TRUE FALSE");
            TEXT format1 = wrap("%b %b %B %B");
            BUF actual1;
            write_format(&BUF::write_s, &BUF::write_c, &actual1, DEFAULT_MEMORY_ARENA, &format1, true, false, true, false);
            Assert::AreEqual(expected1, actual1.values);
        }

        TEST_METHOD(write_format_s_test)
        {
            TCHAR* expected1 = _T("[🎍]");
            TEXT format1 = wrap("[%s]");
            BUF actual1;
            write_format(&BUF::write_s, &BUF::write_c, &actual1, DEFAULT_MEMORY_ARENA, &format1, _T("🎍"));
            Assert::AreEqual(expected1, actual1.values);
        }


        TEST_METHOD(write_format_t_test)
        {
            TCHAR* expected1 = _T("[🚬]");
            TEXT format1 = wrap("[%t]");
            BUF actual1;
            TEXT input = wrap("🚬");
            write_format(&BUF::write_s, &BUF::write_c, &actual1, DEFAULT_MEMORY_ARENA, &format1, &input);
            Assert::AreEqual(expected1, actual1.values);
        }

    };
}
