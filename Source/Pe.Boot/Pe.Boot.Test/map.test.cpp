#include "pch.h"

extern "C" {
#   include "../Pe.Boot/map.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(map_test)
    {
    public:
        TEST_METHOD(initialize_test)
        {
            MAP map = create_map(2, equals_map_key_default, free_map_value_null);
            BOX_INT valies[] = {
                create(1),
                create(2),
                create(3),
                create(4),
                create(5),
            };
            MAP_INIT init[] = {
                { wrap("key1"), &valies[0] },
                { wrap("key2"), &valies[1] },
                { wrap("key3"), &valies[2] },
                { wrap("key4"), &valies[3] },
                { wrap("key5"), &valies[4] },
            };
            Assert::IsTrue(initialize_map(&map, init, sizeof(init) / sizeof(init[0]), false));

            Assert::AreEqual((size_t)5, map.length);
            Assert::AreEqual((size_t)8, map.library.capacity);

            free_map(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        TEST_METHOD(add_map_test)
        {
            MAP map = create_map(2, equals_map_key_default, free_map_value_null);
            Assert::AreEqual((size_t)2, map.library.capacity);
            Assert::AreEqual((size_t)0, map.length);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            MAP_PAIR* pair1 = add_map(&map, &key1, &value1, false);
            Assert::IsNotNull(pair1);
            Assert::AreEqual(1, ((BOX_INT*)pair1->value)->value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            TEXT key2 = wrap("key2");
            BOX_INT value2 = create(2);
            MAP_PAIR* pair2 = add_map(&map, &key2, &value2, false);
            Assert::IsNotNull(pair2);
            Assert::AreEqual(2, ((BOX_INT*)pair2->value)->value);
            Assert::AreEqual((size_t)2, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            TEXT key3 = wrap("key3");
            BOX_INT value3 = create(3);
            MAP_PAIR* pair3 = add_map(&map, &key3, &value3, false);
            Assert::IsNotNull(pair3);
            Assert::AreEqual(3, ((BOX_INT*)pair3->value)->value);
            Assert::AreEqual((size_t)3, map.length);
            Assert::AreEqual((size_t)4, map.library.capacity);

            BOX_INT value4 = create(3);
            MAP_PAIR* pairFailed = add_map(&map, &key1, &value4, false);
            Assert::IsNull(pairFailed);
            Assert::AreEqual((size_t)3, map.length);
            Assert::AreEqual((size_t)4, map.library.capacity);

            free_map(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        TEST_METHOD(set_map_test)
        {
            MAP map = create_map(2, equals_map_key_default, free_map_value_null);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            MAP_PAIR* pair1 = set_map(&map, &key1, &value1, false);
            Assert::IsNotNull(pair1);
            Assert::AreEqual(1, ((BOX_INT*)pair1->value)->value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            TEXT key1_2 = wrap("key1");
            BOX_INT value1_2 = create(11);
            MAP_PAIR* pair1_2 = set_map(&map, &key1_2, &value1_2, false);
            Assert::IsNotNull(pair1_2);
            Assert::IsTrue(pair1_2 == pair1);
            Assert::AreEqual(11, ((BOX_INT*)pair1_2->value)->value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            free_map(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        TEST_METHOD(remove_map_test)
        {
            MAP map = create_map(2, equals_map_key_default, free_map_value_null);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            MAP_PAIR* pair1 = add_map(&map, &key1, &value1, false);

            TEXT key2 = wrap("key2");
            BOX_INT value2 = create(2);
            MAP_PAIR* pair2 = add_map(&map, &key2, &value2, false);

            Assert::AreEqual((size_t)2, map.length);
            Assert::IsTrue(remove_map(&map, &key2));
            Assert::AreEqual((size_t)1, map.length);

            BOX_INT value2_2 = create(22);
            MAP_PAIR* pair2_2 = add_map(&map, &key2, &value2_2, false);
            Assert::AreEqual((size_t)2, map.length);
            Assert::IsTrue(pair2_2 == pair2); // ズレてないので一応使える(使用自体は想定していない)

            Assert::IsTrue(remove_map(&map, &key1));
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual(22, ((BOX_INT*)pair1->value)->value);// ズレてるので当時の後ろのやつを指している(使用自体は想定していない)

            BOX_INT value1_2 = create(11);
            MAP_PAIR* pair1_2 = add_map(&map, &key1, &value1_2, false);

            TEXT key3 = wrap("key3");
            BOX_INT value3 = create(3);
            MAP_PAIR* pair3 = add_map(&map, &key3, &value3, false);
            Assert::IsNotNull(pair3);
            Assert::AreEqual((size_t)3, map.length);

            Assert::IsTrue(remove_map(&map, &key1));
            Assert::AreEqual((size_t)2, map.length);
            Assert::AreEqual(22, ((BOX_INT*)pair1->value)->value);// ズレてるので当時の後ろのやつを指している(使用自体は想定していない)
            Assert::AreEqual(11, ((BOX_INT*)pair1_2->value)->value);// ズレてるので当時の後ろのやつを指している(使用自体は想定していない)

            free_map(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        TEST_METHOD(get_map_test)
        {
            MAP map = create_map(2, equals_map_key_default, free_map_value_null);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            add_map(&map, &key1, &value1, false);

            TEXT key2 = wrap("key2");
            BOX_INT value2 = create(2);
            add_map(&map, &key2, &value2, false);

            TEXT key3 = wrap("key3");
            BOX_INT value3 = create(3);
            add_map(&map, &key3, &value3, false);

            MAP_RESULT_VALUE result1 = get_map(&map, &key1);
            Assert::IsTrue(result1.exists);
            Assert::AreEqual(1, ((BOX_INT*)result1.value)->value);

            MAP_RESULT_VALUE result2 = get_map(&map, &key2);
            Assert::IsTrue(result2.exists);
            Assert::AreEqual(2, ((BOX_INT*)result2.value)->value);

            MAP_RESULT_VALUE result3 = get_map(&map, &key3);
            Assert::IsTrue(result3.exists);
            Assert::AreEqual(3, ((BOX_INT*)result3.value)->value);

            remove_map(&map, &key2);

            MAP_RESULT_VALUE result2_2 = get_map(&map, &key2);
            Assert::IsFalse(result2_2.exists);

            free_map(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        static void freeTextTest(MAP_PAIR* pair)
        {
            TEXT* p = (TEXT*)pair->value;
            free_text(p);
        }

        TEST_METHOD(free_map_test)
        {
            MAP map = create_map(2, equals_map_key_default, freeTextTest);

            TEXT key1 = wrap("key1");
            TEXT value1 = text("あいうえお");
            add_map(&map, &key1, &value1, false);

            MAP_RESULT_VALUE result1 = get_map(&map, &key1);
            Assert::IsTrue(result1.exists);
            TEXT* text1 = (TEXT*)result1.value;
            Assert::AreEqual(value1.value, text1->value);
            Assert::IsTrue(&value1 == text1);

            TEXT value1_2 = text("アイウエオ");
            set_map(&map, &key1, &value1_2, true);
            Assert::IsFalse(value1.library.released);

            free_text(&value1);
            Assert::IsTrue(value1.library.released);

            TEXT value1_3 = text("安以宇衣於");
            set_map(&map, &key1, &value1_3, true);
            Assert::IsTrue(value1_2.library.released);

            free_map(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        static bool equals_map_key_ignore_case(const TEXT* a, const TEXT* b)
        {
            return !compare_string(a->value, b->value, true);
        }

        TEST_METHOD(compare_test)
        {
            MAP map_default = create_map(2, equals_map_key_default, free_map_value_null);

            TEXT key1 = wrap("key1");
            TEXT key2 = wrap("key2");
            TEXT key2_1 = wrap("KEY2");

            BOX_INT value1 = create(1);
            BOX_INT value2 = create(2);
            BOX_INT value2_1= create(22);

            add_map(&map_default, &key1, &value1, false);
            add_map(&map_default, &key2, &value2, false);
            Assert::IsNotNull(add_map(&map_default, &key2_1, &value2_1, false));
            Assert::IsNull(add_map(&map_default, &key2_1, &value2_1, false));
            Assert::AreEqual((size_t)3, map_default.length);

            MAP map_ignore = create_map(2, equals_map_key_ignore_case, free_map_value_null);
            add_map(&map_ignore, &key1, &value1, false);
            add_map(&map_ignore, &key2, &value2, false);
            Assert::IsNull(add_map(&map_ignore, &key2_1, &value2_1, false));
            Assert::IsNull(add_map(&map_ignore, &key2_1, &value2_1, false));
            Assert::AreEqual((size_t)2, map_ignore.length);

            free_map(&map_default);
            free_map(&map_ignore);
        }
    };
}
