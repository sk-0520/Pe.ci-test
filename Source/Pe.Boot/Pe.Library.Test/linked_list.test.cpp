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
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            Assert::AreEqual((size_t)0, linked_list.length);
            Assert::IsNull(linked_list.library.head);
            Assert::IsNull(linked_list.library.tail);
            bool actual = release_linked_list(&linked_list, true);
            Assert::IsTrue(actual);
        }

        TEST_METHOD(add_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            int input_1 = 1;
            int actual_1 = *(int*)add_linked_list(&linked_list, &input_1);
            Assert::AreEqual(input_1, actual_1);
            Assert::AreEqual((size_t)1, linked_list.length);

            int input_2 = 2;
            int actual_2 = *(int*)add_linked_list(&linked_list, &input_2);
            Assert::AreEqual(input_2, actual_2);
            Assert::AreEqual((size_t)2, linked_list.length);

            release_linked_list(&linked_list, true);
        }

        TEST_METHOD(get_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

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

            release_linked_list(&linked_list, true);
        }

        TEST_METHOD(insert_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

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

            release_linked_list(&linked_list, true);
        }

        TEST_METHOD(remove_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

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

            release_linked_list(&linked_list, true);
        }

        TEST_METHOD(set_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

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

            release_linked_list(&linked_list, true);
        }

        static bool search_linked_list_num(const void* needle, const void* value, void* data, void* arg)
        {
            int needle_num = *(int*)needle;
            int value_num = *(int*)value;

            return needle_num == value_num;
        }

        TEST_METHOD(search_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            int inputs[] = {
                10, 20, 30, 40, 50,
            };
            for (size_t i = 0; i < SIZEOF_ARRAY(inputs); i++) {
                add_linked_list(&linked_list, inputs + i);
            }

            auto tests = {
                DATA(true, 10),
                DATA(false, 11),
            };
            for (auto test: tests) {
                auto arg1 = std::get<0>(test.inputs);
                auto actual = search_linked_list(&linked_list, &arg1, NULL, search_linked_list_num);
                if (test.expected) {
                    Assert::IsNotNull(actual);
                    const void* value = get_link_node_value(actual);
                    Assert::AreEqual(arg1, *(const int*)value);
                } else {
                    Assert::IsNull(actual);
                }
            }

            release_linked_list(&linked_list, true);
        }

        TEST_METHOD(to_object_from_linked_list_test)
        {
            LINKED_LIST linked_list = new_linked_list(sizeof(int), NULL, compare_linked_list_value_null, release_linked_list_value_null, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            int inputs[] = {
                10, 20, 30, 40, 50,
            };
            for (size_t i = 0; i < SIZEOF_ARRAY(inputs); i++) {
                add_linked_list(&linked_list, inputs + i);
            }

            OBJECT_LIST object_list = to_object_list_from_linked_list(&linked_list);
            Assert::AreEqual(SIZEOF_ARRAY(inputs), object_list.length);

            for (size_t i = 0; i < object_list.length; i++) {
                OBJECT_RESULT_VALUE actual = get_object_list(&object_list, i);;
                Assert::IsTrue(actual.exists);
                Assert::AreEqual(inputs[i], *(int*)actual.value);
            }
            release_object_list(&object_list, true);

            release_linked_list(&linked_list, true);
        }

    };
}
