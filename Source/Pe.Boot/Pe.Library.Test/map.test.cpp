#include "pch.h"

extern "C" {
#   include "../Pe.Library/map.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(map_test)
    {
    public:
        TEST_METHOD(new_release_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            release_map(&map);
        }

        TEST_METHOD(add_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT input_key_1 = wrap("key-1");
            int input_value_1 = 10;
            bool actual_1 = add_map(&map, &input_key_1, &input_value_1);
            Assert::IsTrue(actual_1);

            TEXT input_key_2 = wrap("key-2");
            int input_value_2 = 20;
            bool actual_2 = add_map(&map, &input_key_2, &input_value_2);
            Assert::IsTrue(actual_2);

            TEXT input_key_3 = wrap("key-1");
            int input_value_3 = 30;
            bool actual_3 = add_map(&map, &input_key_3, &input_value_3);
            Assert::IsFalse(actual_3);

            release_map(&map);
        }

        TEST_METHOD(get_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT input_key_0 = wrap("key-0");
            MAP_RESULT_VALUE actual_0 = get_map(&map, &input_key_0);
            Assert::IsFalse(actual_0.exists);

            TEXT input_key_1 = wrap("key-1");
            int input_value_1 = 10;
            add_map(&map, &input_key_1, &input_value_1);

            TEXT input_key_2 = wrap("key-2");
            int input_value_2 = 20;
            add_map(&map, &input_key_2, &input_value_2);

            TEXT input_key_3 = wrap("key-3");
            int input_value_3 = 30;
            add_map(&map, &input_key_3, &input_value_3);

            TEXT input_key_4 = wrap("key-4");

            MAP_RESULT_VALUE actual_1 = get_map(&map, &input_key_1);
            Assert::IsTrue(actual_1.exists);
            Assert::AreEqual(input_value_1, *(int*)actual_1.value);

            MAP_RESULT_VALUE actual_2 = get_map(&map, &input_key_2);
            Assert::IsTrue(actual_2.exists);
            Assert::AreEqual(input_value_2, *(int*)actual_2.value);

            MAP_RESULT_VALUE actual_3 = get_map(&map, &input_key_3);
            Assert::IsTrue(actual_3.exists);
            Assert::AreEqual(input_value_3, *(int*)actual_3.value);

            MAP_RESULT_VALUE actual_4 = get_map(&map, &input_key_4);
            Assert::IsFalse(actual_4.exists);

            release_map(&map);
        }

        TEST_METHOD(set_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT input_key = wrap("key-1");

            int input_value_1 = 10;

            set_map(&map, &input_key, &input_value_1);
            MAP_RESULT_VALUE actual_1 = get_map(&map, &input_key);
            Assert::IsTrue(actual_1.exists);
            Assert::AreEqual(input_value_1, *(int*)actual_1.value);

            int input_value_2 = 20;

            set_map(&map, &input_key, &input_value_2);
            MAP_RESULT_VALUE actual_2 = get_map(&map, &input_key);
            Assert::IsTrue(actual_2.exists);
            Assert::AreEqual(input_value_2, *(int*)actual_2.value);

            release_map(&map);
        }

        TEST_METHOD(remove_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);

            TEXT key = wrap("key");
            int value = 10;

            Assert::IsFalse(remove_map(&map, &key));
            Assert::AreEqual((size_t)0, map.length);

            set_map(&map, &key, &value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::IsTrue(remove_map(&map, &key));

            Assert::AreEqual((size_t)0, map.length);

            release_map(&map);
        }

        TEST_METHOD(extend_test)
        {
            MAP map = new_map(sizeof(int), 2, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            //size_t length = 64;
            size_t length = 8;
            TEXT format = wrap("key-%d");
            for (size_t i = 0; i < length; i++) {
                TEXT key = format_text(DEFAULT_MEMORY_ARENA, &format, i);
                add_map(&map, &key, &i);
                release_text(&key);
            }
            Assert::AreEqual(length, map.length);

            release_map(&map);
        }

        static bool foreach_map_test(const KEY_VALUE_PAIR* pair, size_t index, size_t length, void* arg)
        {
            //TEXT format = wrap("key-%d");
            //TEXT key = format_text(DEFAULT_MEMORY_ARENA, &format, index);
            //int* inputs = (int*)arg;

            //Assert::IsTrue(is_equals_text(&pair->key, &key, false));
            //Assert::AreEqual(*(int*)pair->value, inputs[index]);

            //release_text(&key);

            return true;
        }

        TEST_METHOD(foreach_map_test)
        {
            MAP map = new_map(sizeof(int), MAP_DEFAULT_CAPACITY, MAP_DEFAULT_LOAD_FACTOR, release_linked_list_value_null, calc_map_hash_default, equals_map_key_default, DEFAULT_MEMORY_ARENA, DEFAULT_MEMORY_ARENA);
            int inputs[] = {
                1, 2, 3, 4, 5
            };
            for (int input : inputs) {
                TEXT format = wrap("key-%d");
                TEXT key = format_text(DEFAULT_MEMORY_ARENA, &format, input);
                add_map(&map, &key, &input);
                release_text(&key);
            }

            foreach_map(&map, foreach_map_test, inputs);

            release_map(&map);
        }
    };
}
