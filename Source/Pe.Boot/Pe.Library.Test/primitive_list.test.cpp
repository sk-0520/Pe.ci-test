#include "pch.h"

extern "C" {
#   include "../Pe.Library/primitive_list.gen.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(primitive_list_test)
    {
    public:
        TEST_METHOD(life_test)
        {
            PRIMITIVE_LIST_UINT32 list = new_primitive_list(PRIMITIVE_LIST_TYPE_UINT32, 2, DEFAULT_MEMORY);
            Assert::AreEqual((size_t)0, list.length);
            Assert::AreEqual((size_t)2 * sizeof(uint32_t), list.library.capacity_bytes);

            Assert::IsFalse(pop_list_uint32(NULL, &list));

            push_list_uint32(&list, 100);
            Assert::AreEqual((size_t)1, list.length);
            Assert::AreEqual((size_t)2 * sizeof(uint32_t), list.library.capacity_bytes);

            push_list_uint32(&list, 200);
            Assert::AreEqual((size_t)2, list.length);
            Assert::AreEqual((size_t)2 * sizeof(uint32_t), list.library.capacity_bytes);

            push_list_uint32(&list, 300);
            Assert::AreEqual((size_t)3, list.length);
            Assert::AreEqual((size_t)4 * sizeof(uint32_t), list.library.capacity_bytes);

            uint32_t result1;
            Assert::IsFalse(get_list_uint32(&result1, &list, 3));
            Assert::IsTrue(get_list_uint32(&result1, &list, 1));
            Assert::AreEqual((uint32_t)200, result1);

            Assert::IsTrue(pop_list_uint32(NULL, &list));
            Assert::AreEqual((size_t)2, list.length);

            uint32_t result2;
            pop_list_uint32(&result2, &list);
            Assert::AreEqual((uint32_t)200, result2);
            Assert::AreEqual((size_t)1, list.length);

            Assert::IsTrue(release_primitive_list(&list));
        }

        TEST_METHOD(type_error_test)
        {
            PRIMITIVE_LIST_UINT32 list = new_primitive_list(PRIMITIVE_LIST_TYPE_UINT32, 2, DEFAULT_MEMORY);

            Assert::IsFalse(push_list_uint8(&list, 1));
            Assert::IsFalse(push_list_uint16(&list, 1));
            Assert::IsTrue(push_list_uint32(&list, 1));

            uint8_t u8;
            uint16_t u16;
            uint32_t u32;
            Assert::IsFalse(get_list_uint8(&u8, &list, 0));
            Assert::IsFalse(get_list_uint16(&u16, &list, 0));
            Assert::IsTrue(get_list_uint32(&u32, &list, 0));

            release_primitive_list(&list);
        }

        TEST_METHOD(add_range_test)
        {
            PRIMITIVE_LIST_INT32 list = new_primitive_list(PRIMITIVE_LIST_TYPE_INT32, 1, DEFAULT_MEMORY);
            int32_t values[] = {
                1, 2, 3, 4, 5,
            };
            add_range_list_int32(&list, values, sizeof(values));
            for (size_t i = 0; i < SIZEOF_ARRAY(values); i++) {
                int32_t value;
                Assert::IsTrue(get_list_int32(&value, &list, i));
                Assert::AreEqual(values[i], value);
            }

            release_primitive_list(&list);
        }
    };
}
