#include "pch.h"

extern "C" {
#   include "../Pe.Library/hash_map.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(hash_map_test)
    {
    public:
        TEST_METHOD(new_release_test)
        {
            HASH_MAP map = new_hash_map(sizeof(int), HASH_MAP_DEFAULT_CAPACITY, HASH_MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_hash_map_key_default, DEFAULT_MEMORY, DEFAULT_MEMORY);
            release_hash_map(&map);
        }

        TEST_METHOD(add_hash_map_test)
        {
            HASH_MAP map = new_hash_map(sizeof(int), HASH_MAP_DEFAULT_CAPACITY, HASH_MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_hash_map_key_default, DEFAULT_MEMORY, DEFAULT_MEMORY);

            TEXT input_key_1 = wrap("key-1");
            int input_value_1 = 10;
            bool actual_1 = add_hash_map(&map, &input_key_1, &input_value_1);
            Assert::IsTrue(actual_1);

            TEXT input_key_2 = wrap("key-2");
            int input_value_2 = 20;
            bool actual_2 = add_hash_map(&map, &input_key_2, &input_value_2);
            Assert::IsTrue(actual_2);

            TEXT input_key_3 = wrap("key-1");
            int input_value_3 = 30;
            bool actual_3 = add_hash_map(&map, &input_key_3, &input_value_3);
            Assert::IsFalse(actual_3);

            release_hash_map(&map);
        }

        TEST_METHOD(get_hash_map_test)
        {
            HASH_MAP map = new_hash_map(sizeof(int), HASH_MAP_DEFAULT_CAPACITY, HASH_MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_hash_map_key_default, DEFAULT_MEMORY, DEFAULT_MEMORY);

            TEXT input_key_0 = wrap("key-0");
            HASH_MAP_RESULT_VALUE actual_0 = get_hash_map(&map, &input_key_0);
            Assert::IsFalse(actual_0.exists);

            TEXT input_key_1 = wrap("key-1");
            int input_value_1 = 10;
            add_hash_map(&map, &input_key_1, &input_value_1);

            TEXT input_key_2 = wrap("key-2");
            int input_value_2 = 20;
            add_hash_map(&map, &input_key_2, &input_value_2);

            TEXT input_key_3 = wrap("key-3");
            int input_value_3 = 30;
            add_hash_map(&map, &input_key_3, &input_value_3);

            TEXT input_key_4 = wrap("key-4");

            HASH_MAP_RESULT_VALUE actual_1 = get_hash_map(&map, &input_key_1);
            Assert::IsTrue(actual_1.exists);
            Assert::AreEqual(input_value_1, *(int*)actual_1.value);

            HASH_MAP_RESULT_VALUE actual_2 = get_hash_map(&map, &input_key_2);
            Assert::IsTrue(actual_2.exists);
            Assert::AreEqual(input_value_2, *(int*)actual_2.value);

            HASH_MAP_RESULT_VALUE actual_3 = get_hash_map(&map, &input_key_3);
            Assert::IsTrue(actual_3.exists);
            Assert::AreEqual(input_value_3, *(int*)actual_3.value);

            HASH_MAP_RESULT_VALUE actual_4 = get_hash_map(&map, &input_key_4);
            Assert::IsFalse(actual_4.exists);

            release_hash_map(&map);
        }

        TEST_METHOD(set_hash_map_test)
        {
            HASH_MAP map = new_hash_map(sizeof(int), HASH_MAP_DEFAULT_CAPACITY, HASH_MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_hash_map_key_default, DEFAULT_MEMORY, DEFAULT_MEMORY);

            TEXT input_key = wrap("key-1");

            int input_value_1 = 10;

            set_hash_map(&map, &input_key, &input_value_1);
            HASH_MAP_RESULT_VALUE actual_1 = get_hash_map(&map, &input_key);
            Assert::IsTrue(actual_1.exists);
            Assert::AreEqual(input_value_1, *(int*)actual_1.value);

            int input_value_2 = 20;

            set_hash_map(&map, &input_key, &input_value_2);
            HASH_MAP_RESULT_VALUE actual_2 = get_hash_map(&map, &input_key);
            Assert::IsTrue(actual_2.exists);
            Assert::AreEqual(input_value_2, *(int*)actual_2.value);

            release_hash_map(&map);
        }

        TEST_METHOD(remove_hash_map_test)
        {
            HASH_MAP map = new_hash_map(sizeof(int), HASH_MAP_DEFAULT_CAPACITY, HASH_MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_hash_map_key_default, DEFAULT_MEMORY, DEFAULT_MEMORY);

            TEXT key = wrap("key");
            int value = 10;

            Assert::IsFalse(remove_hash_map(&map, &key));
            Assert::AreEqual((size_t)0, map.length);

            set_hash_map(&map, &key, &value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::IsTrue(remove_hash_map(&map, &key));

            Assert::AreEqual((size_t)0, map.length);

            release_hash_map(&map);
        }

        TEST_METHOD(extend_test)
        {
            HASH_MAP map = new_hash_map(sizeof(int), 2, HASH_MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_hash_map_key_default, DEFAULT_MEMORY, DEFAULT_MEMORY);
            size_t length = 64;
            TEXT format = wrap("key-%d");
            for (size_t i = 0; i < length; i++) {
                TEXT key = format_text(DEFAULT_MEMORY, &format, i);
                add_hash_map(&map, &key, &i);
                release_text(&key);
            }
            Assert::AreEqual(length, map.length);

            release_hash_map(&map);
        }

        static bool foreach_map_test(const KEY_VALUE_PAIR* pair, size_t index, size_t length, void* arg)
        {
            //TEXT format = wrap("key-%d");
            //TEXT key = format_text(DEFAULT_MEMORY, &format, index);
            //int* inputs = (int*)arg;

            //Assert::IsTrue(is_equals_text(&pair->key, &key, false));
            //Assert::AreEqual(*(int*)pair->value, inputs[index]);

            //release_text(&key);

            return true;
        }

        TEST_METHOD(foreach_map_test)
        {
            HASH_MAP map = new_hash_map(sizeof(int), HASH_MAP_DEFAULT_CAPACITY, HASH_MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_hash_map_key_default, DEFAULT_MEMORY, DEFAULT_MEMORY);
            int inputs[] = {
                1, 2, 3, 4, 5
            };
            for (int input : inputs) {
                TEXT format = wrap("key-%d");
                TEXT key = format_text(DEFAULT_MEMORY, &format, input);
                add_hash_map(&map, &key, &input);
                release_text(&key);
            }

            foreach_map(&map, foreach_map_test, inputs);

            release_hash_map(&map);
        }
    };
}
