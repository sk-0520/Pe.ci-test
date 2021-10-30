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
            OBJECT_LIST list = create_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null);

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

            free_object_list(&list);
        }

        static bool foreach_object_list_func(const void* value, size_t index, size_t length, void* data)
        {
            int* number = (int*)value;
            OBJECT_LIST* list = (OBJECT_LIST*)data;

            int conv_value = *number * 10;

            push_object_list(list, &conv_value);

            return true;
        }

        TEST_METHOD(foreach_object_list_func_test)
        {
            OBJECT_LIST list = create_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null);

            int inputs[] = {
                1,
                2,
                3,
                4,
            };

            OBJECT_LIST data_list = create_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null);

            add_range_object_list(&list, inputs, sizeof(inputs) / sizeof(inputs[0]));
            size_t count = foreach_object_list(&list, foreach_object_list_func, &data_list);
            Assert::AreEqual((size_t)4, count);
            Assert::AreEqual(10, *(int*)get_object_list(&data_list, 0).value);
            Assert::AreEqual(20, *(int*)get_object_list(&data_list, 1).value);
            Assert::AreEqual(30, *(int*)get_object_list(&data_list, 2).value);
            Assert::AreEqual(40, *(int*)get_object_list(&data_list, 3).value);

            free_object_list(&list);
            free_object_list(&data_list);
        }

        static bool foreach_object_list_skip(const void* value, size_t index, size_t length, void* data)
        {
            if (2 <= index) {
                return false;
            }

            int* number = (int*)value;
            OBJECT_LIST* list = (OBJECT_LIST*)data;

            int conv_value = *number * 100;

            push_object_list(list, &conv_value);

            return true;
        }

        TEST_METHOD(foreach_object_list_skip_test)
        {
            OBJECT_LIST list = create_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null);

            int inputs[] = {
                1,
                2,
                3,
                4,
            };

            OBJECT_LIST data_list = create_object_list(sizeof(int), OBJECT_LIST_DEFAULT_CAPACITY_COUNT, compare_object_list_value_null, free_object_list_value_null);

            add_range_object_list(&list, inputs, sizeof(inputs) / sizeof(inputs[0]));
            size_t count = foreach_object_list(&list, foreach_object_list_skip, &data_list);
            Assert::AreEqual((size_t)2, count);
            Assert::AreEqual(100, *(int*)get_object_list(&data_list, 0).value);
            Assert::AreEqual(200, *(int*)get_object_list(&data_list, 1).value);

            free_object_list(&list);
            free_object_list(&data_list);
        }

    };
}
