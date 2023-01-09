#include "pch.h"

extern "C" {
#   include "../Pe.Library/object_list.h"
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(object_list_test)
    {
    public:
        TEST_METHOD(life_int_test)
        {
            OBJECT_LIST list = new_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, NULL, compare_object_list_value_null, release_object_list_value_null, DEFAULT_MEMORY_ARENA);

            int input_1 = 100;
            int* result_1 = (int*)push_object_list(&list, &input_1);
            Assert::AreNotEqual(&input_1, result_1);
            Assert::AreEqual(input_1, *result_1);
            Assert::AreEqual((size_t)1, list.length);

            int input_2 = 200;
            int* result_2 = (int*)push_object_list(&list, &input_2);
            Assert::AreNotEqual(&input_2, result_2);
            Assert::AreEqual(input_2, *result_2);
            Assert::AreEqual((size_t)2, list.length);

            OBJECT_RESULT_VALUE value_1 = get_object_list(&list, 0);
            Assert::IsTrue(value_1.exists);
            Assert::AreEqual(result_1, (int*)value_1.value);
            Assert::AreEqual(input_1, *(int*)value_1.value);

            OBJECT_RESULT_VALUE value_2 = get_object_list(&list, 1);
            Assert::IsTrue(value_2.exists);
            Assert::AreEqual(result_2, (int*)value_2.value);
            Assert::AreEqual(input_2, *(int*)value_2.value);

            OBJECT_RESULT_VALUE value_3 = get_object_list(&list, 2);
            Assert::IsFalse(value_3.exists);

            int set_value_2 = 222;
            bool set_result_2 = set_object_list(&list, 1, &set_value_2, true);
            Assert::IsTrue(set_result_2);

            int* peek_2 = (int*)peek_object_list(&list);
            Assert::AreEqual(set_value_2, *peek_2);

            int pop_value_2;
            bool pop_result_2 = pop_object_list(&pop_value_2, &list);
            Assert::IsTrue(pop_result_2);
            Assert::AreEqual(set_value_2, pop_value_2);

            Assert::AreEqual((size_t)1, list.length);

            int values_4[] = { 9, 99, 999 };
            add_range_object_list(&list, values_4, sizeof(values_4) / sizeof(values_4[0]));
            OBJECT_RESULT_VALUE values_4_1 = get_object_list(&list, 1);
            OBJECT_RESULT_VALUE values_4_2 = get_object_list(&list, 2);
            OBJECT_RESULT_VALUE values_4_3 = get_object_list(&list, 3);

            Assert::AreEqual(values_4[0], *(int*)values_4_1.value);
            Assert::AreEqual(values_4[1], *(int*)values_4_2.value);
            Assert::AreEqual(values_4[2], *(int*)values_4_3.value);
            Assert::AreEqual((size_t)4, list.length);

            release_object_list(&list, true);
        }

        TEST_METHOD(reference_object_list_value_test)
        {
            int inputs[] = {
                1,
                2,
                3,
                4,
            };

            OBJECT_LIST list = new_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, NULL, compare_object_list_value_null, release_object_list_value_null, DEFAULT_MEMORY_ARENA);

            add_range_object_list(&list, inputs, sizeof(inputs) / sizeof(inputs[0]));

            const int* actual = reference_value_object_list(int, list);
            for (size_t i = 0; i < list.length; i++) {
                Assert::AreEqual(inputs[i], actual[i]);
            }

            release_object_list(&list, true);
        }

        TEST_METHOD(reference_object_list_pointer_test)
        {
            int inputs[] = {
                1,
                2,
                3,
                4,
            };

            OBJECT_LIST list = new_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, NULL, compare_object_list_value_null, release_object_list_value_null, DEFAULT_MEMORY_ARENA);

            add_range_object_list(&list, inputs, sizeof(inputs) / sizeof(inputs[0]));

            const OBJECT_LIST* p = &list;

            const int* actual = reference_ref_object_list(int, p);
            for (size_t i = 0; i < p->length; i++) {
                Assert::AreEqual(inputs[i], actual[i]);
            }

            release_object_list(&list, true);
        }
    };
}
