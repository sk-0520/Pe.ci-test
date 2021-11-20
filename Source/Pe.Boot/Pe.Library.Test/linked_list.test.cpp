#include "pch.h"

extern "C" {
#   include "../Pe.Library/linked_list.h"
#   include "../Pe.Library/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(linked_list_test)
    {
    public:
        TEST_METHOD(new_release_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY, DEFAULT_MEMORY);
            Assert::AreEqual((size_t)0, linked_list.length);
            Assert::IsNull(linked_list.library.head);
            Assert::IsNull(linked_list.library.tail);
            bool actual = release_linked_list(&linked_list);
            Assert::IsTrue(actual);
        }

        TEST_METHOD(add_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY, DEFAULT_MEMORY);

            int input_1 = 1;
            int acutal_1 = *(int*)add_linked_list(&linked_list, &input_1);
            Assert::AreEqual(input_1, acutal_1);
            Assert::AreEqual((size_t)1, linked_list.length);

            int input_2 = 2;
            int acutal_2 = *(int*)add_linked_list(&linked_list, &input_2);
            Assert::AreEqual(input_2, acutal_2);
            Assert::AreEqual((size_t)2, linked_list.length);

            release_linked_list(&linked_list);
        }

        TEST_METHOD(get_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY, DEFAULT_MEMORY);

            int inputs[] = {
                1, 2, 3, 4, 5
            };
            for (size_t i = 0; i < SIZEOF_ARRAY(inputs); i++) {
                add_linked_list(&linked_list, inputs + i);
            }

            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs), linked_list.length);

            for (size_t i = 0; i < SIZEOF_ARRAY(inputs); i++) {
                LINK_RESULT_VALUE actual = get_linked_list(&linked_list, i);
                Assert::IsTrue(actual.exists);
                Assert::AreEqual(inputs[i], *(int*)actual.value);
            }

            LINK_RESULT_VALUE actual = get_linked_list(&linked_list, SIZEOF_ARRAY(inputs));
            Assert::IsFalse(actual.exists);

            release_linked_list(&linked_list);
        }

        TEST_METHOD(insert_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY, DEFAULT_MEMORY);

            int inputs[] = {
                10, 20, 30, 40, 50,
            };
            int expecteds[] = {
                99/*0*/, 1/*1*/, 2/*2*/, 10, 15/*4*/, 20, 25/*6*/, 30, 35/*8*/, 40, 45/*10*/, 50,
            };
            for (size_t i = 0; i < SIZEOF_ARRAY(inputs); i++) {
                add_linked_list(&linked_list, inputs + i);
            }
            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs), linked_list.length);

            insert_linked_list(&linked_list, 0, &expecteds[2] /* 2 */);
            Assert::AreEqual(expecteds[2], *(int*)get_linked_list(&linked_list, 0).value);

            insert_linked_list(&linked_list, 0, &expecteds[1] /* 1 */);
            Assert::AreEqual(expecteds[1], *(int*)get_linked_list(&linked_list, 0).value);

            insert_linked_list(&linked_list, 0, &expecteds[0] /* 99 */);
            Assert::AreEqual(expecteds[0], *(int*)get_linked_list(&linked_list, 0).value);

            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs) + 3, linked_list.length);

            // もともと20がいる
            insert_linked_list(&linked_list, 4, &expecteds[4] /* 15 */);
            Assert::AreEqual(expecteds[4], *(int*)get_linked_list(&linked_list, 4).value);

            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs) + 3 + 1, linked_list.length);

            insert_linked_list(&linked_list, 6, &expecteds[6] /* 25 */);
            Assert::AreEqual(expecteds[6], *(int*)get_linked_list(&linked_list, 6).value);

            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs) + 3 + 1 + 1, linked_list.length);

            insert_linked_list(&linked_list, 8, &expecteds[8] /* 35 */);
            Assert::AreEqual(expecteds[8], *(int*)get_linked_list(&linked_list, 8).value);

            insert_linked_list(&linked_list, 10, &expecteds[10] /* 45 */);
            Assert::AreEqual(expecteds[10], *(int*)get_linked_list(&linked_list, 10).value);

            Assert::AreEqual((size_t)SIZEOF_ARRAY(expecteds), linked_list.length);

            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                LINK_RESULT_VALUE actual = get_linked_list(&linked_list, i);
                Assert::IsTrue(actual.exists);
                Assert::AreEqual(expecteds[i], *(int*)actual.value);
            }

            int add_value = 999;
            bool add_value_actual = insert_linked_list(&linked_list, SIZEOF_ARRAY(expecteds), &add_value);
            Assert::IsFalse(add_value_actual);

            int last_value = 1000;
            bool last_value_actual = insert_linked_list(&linked_list, SIZEOF_ARRAY(expecteds) - 1, &last_value);
            Assert::IsTrue(last_value_actual);

            Assert::AreEqual((size_t)SIZEOF_ARRAY(expecteds) + 1, linked_list.length);

            release_linked_list(&linked_list);
        }

        TEST_METHOD(remove_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY, DEFAULT_MEMORY);

            int inputs[] = {
                10, 20, 30, 40, 50,
            };
            for (size_t i = 0; i < SIZEOF_ARRAY(inputs); i++) {
                add_linked_list(&linked_list, inputs + i);
            }
            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs), linked_list.length);

            remove_linked_list(&linked_list, 4);
            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs) - 1, linked_list.length);

            remove_linked_list(&linked_list, 0);
            Assert::AreEqual((size_t)SIZEOF_ARRAY(inputs) - 2, linked_list.length);

            int expecteds_1[] = { 20, 30, 40 };
            for (size_t i = 0; i < 3; i++) {
                LINK_RESULT_VALUE actual = get_linked_list(&linked_list, i);
                Assert::IsTrue(actual.exists);
                Assert::AreEqual(expecteds_1[i], *(int*)actual.value);
            }

            remove_linked_list(&linked_list, 1);
            Assert::AreEqual((size_t)2, linked_list.length);
            Assert::AreEqual(20, *(int*)get_linked_list(&linked_list, 0).value);
            Assert::AreEqual(40, *(int*)get_linked_list(&linked_list, 1).value);

            remove_linked_list(&linked_list, 0);
            remove_linked_list(&linked_list, 0);

            Assert::AreEqual((size_t)0, linked_list.length);

            bool actual_remove = remove_linked_list(&linked_list, 0);
            Assert::IsFalse(actual_remove);

            release_linked_list(&linked_list);
        }

        TEST_METHOD(set_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY, DEFAULT_MEMORY);

            int inputs[] = {
                10, 20, 30, 40, 50,
            };
            for (size_t i = 0; i < SIZEOF_ARRAY(inputs); i++) {
                add_linked_list(&linked_list, inputs + i);
            }

            int expecteds[] = {
                100, 20, 300, 40, 500,
            };

            int new_input_0 = 100;
            set_linked_list(&linked_list, 0, &new_input_0, true);
            Assert::AreEqual(new_input_0, *(int*)get_linked_list(&linked_list, 0).value);

            int new_input_2 = 300;
            set_linked_list(&linked_list, 2, &new_input_2, true);
            Assert::AreEqual(new_input_2, *(int*)get_linked_list(&linked_list, 2).value);

            int new_input_4 = 500;
            set_linked_list(&linked_list, 4, &new_input_4, true);
            Assert::AreEqual(new_input_4, *(int*)get_linked_list(&linked_list, 4).value);

            for (size_t i = 0; i < SIZEOF_ARRAY(expecteds); i++) {
                LINK_RESULT_VALUE actual = get_linked_list(&linked_list, i);
                Assert::IsTrue(actual.exists);
                Assert::AreEqual(expecteds[i], *(int*)actual.value);
            }

            release_linked_list(&linked_list);
        }
    };
}
