#include "pch.h"

extern "C" {
#   include "../Pe.Boot/map.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(mapTest)
    {
    public:
        TEST_METHOD(addMapTest)
        {
            MAP map = createMap(2, compareMapKeyDefault, freeMapValueNull);
            Assert::AreEqual((size_t)2, map.library.capacity);
            Assert::AreEqual((size_t)0, map.length);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            MAP_PAIR* pair1 = addMap(&map, &key1, &value1, false);
            Assert::IsNotNull(pair1);
            Assert::AreEqual(1, ((BOX_INT*)pair1->value)->value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            TEXT key2 = wrap("key2");
            BOX_INT value2 = create(2);
            MAP_PAIR* pair2 = addMap(&map, &key2, &value2, false);
            Assert::IsNotNull(pair2);
            Assert::AreEqual(2, ((BOX_INT*)pair2->value)->value);
            Assert::AreEqual((size_t)2, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            TEXT key3 = wrap("key3");
            BOX_INT value3 = create(3);
            MAP_PAIR* pair3 = addMap(&map, &key3, &value3, false);
            Assert::IsNotNull(pair3);
            Assert::AreEqual(3, ((BOX_INT*)pair3->value)->value);
            Assert::AreEqual((size_t)3, map.length);
            Assert::AreEqual((size_t)4, map.library.capacity);

            BOX_INT value4 = create(3);
            MAP_PAIR* pairFailed = addMap(&map, &key1, &value4, false);
            Assert::IsNull(pairFailed);
            Assert::AreEqual((size_t)3, map.length);
            Assert::AreEqual((size_t)4, map.library.capacity);

            freeMap(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        TEST_METHOD(setMapTest)
        {
            MAP map = createMap(2, compareMapKeyDefault, freeMapValueNull);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            MAP_PAIR* pair1 = setMap(&map, &key1, &value1, false);
            Assert::IsNotNull(pair1);
            Assert::AreEqual(1, ((BOX_INT*)pair1->value)->value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            TEXT key1_2 = wrap("key1");
            BOX_INT value1_2 = create(11);
            MAP_PAIR* pair1_2 = setMap(&map, &key1_2, &value1_2, false);
            Assert::IsNotNull(pair1_2);
            Assert::IsTrue(pair1_2 == pair1);
            Assert::AreEqual(11, ((BOX_INT*)pair1_2->value)->value);
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual((size_t)2, map.library.capacity);

            freeMap(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        TEST_METHOD(removeMapTest)
        {
            MAP map = createMap(2, compareMapKeyDefault, freeMapValueNull);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            MAP_PAIR* pair1 = addMap(&map, &key1, &value1, false);

            TEXT key2 = wrap("key2");
            BOX_INT value2 = create(2);
            MAP_PAIR* pair2 = addMap(&map, &key2, &value2, false);

            Assert::AreEqual((size_t)2, map.length);
            Assert::IsTrue(removeMap(&map, &key2));
            Assert::AreEqual((size_t)1, map.length);

            BOX_INT value2_2 = create(22);
            MAP_PAIR* pair2_2 = addMap(&map, &key2, &value2_2, false);
            Assert::AreEqual((size_t)2, map.length);
            Assert::IsTrue(pair2_2 == pair2); // ズレてないので一応使える(使用自体は想定していない)

            Assert::IsTrue(removeMap(&map, &key1));
            Assert::AreEqual((size_t)1, map.length);
            Assert::AreEqual(22, ((BOX_INT*)pair1->value)->value);// ズレてるので当時の後ろのやつを指している(使用自体は想定していない)

            BOX_INT value1_2 = create(11);
            MAP_PAIR* pair1_2 = addMap(&map, &key1, &value1_2, false);

            TEXT key3 = wrap("key3");
            BOX_INT value3 = create(3);
            MAP_PAIR* pair3 = addMap(&map, &key3, &value3, false);
            Assert::IsNotNull(pair3);
            Assert::AreEqual((size_t)3, map.length);

            Assert::IsTrue(removeMap(&map, &key1));
            Assert::AreEqual((size_t)2, map.length);
            Assert::AreEqual(22, ((BOX_INT*)pair1->value)->value);// ズレてるので当時の後ろのやつを指している(使用自体は想定していない)
            Assert::AreEqual(11, ((BOX_INT*)pair1_2->value)->value);// ズレてるので当時の後ろのやつを指している(使用自体は想定していない)

            freeMap(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        TEST_METHOD(getMapTest)
        {
            MAP map = createMap(2, compareMapKeyDefault, freeMapValueNull);

            TEXT key1 = wrap("key1");
            BOX_INT value1 = create(1);
            addMap(&map, &key1, &value1, false);

            TEXT key2 = wrap("key2");
            BOX_INT value2 = create(2);
            addMap(&map, &key2, &value2, false);

            TEXT key3 = wrap("key3");
            BOX_INT value3 = create(3);
            addMap(&map, &key3, &value3, false);

            MAP_RESULT_VALUE result1 = getMap(&map, &key1);
            Assert::IsTrue(result1.exists);
            Assert::AreEqual(1, ((BOX_INT*)result1.value)->value);

            MAP_RESULT_VALUE result2 = getMap(&map, &key2);
            Assert::IsTrue(result2.exists);
            Assert::AreEqual(2, ((BOX_INT*)result2.value)->value);

            MAP_RESULT_VALUE result3 = getMap(&map, &key3);
            Assert::IsTrue(result3.exists);
            Assert::AreEqual(3, ((BOX_INT*)result3.value)->value);

            removeMap(&map, &key2);

            MAP_RESULT_VALUE result2_2 = getMap(&map, &key2);
            Assert::IsFalse(result2_2.exists);

            freeMap(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        static void freeTextTest(MAP_PAIR* pair)
        {
            TEXT* p = (TEXT*)pair->value;
            freeText(p);
        }

        TEST_METHOD(freeMapTest)
        {
            MAP map = createMap(2, compareMapKeyDefault, freeTextTest);

            TEXT key1 = wrap("key1");
            TEXT value1 = text("あいうえお");
            addMap(&map, &key1, &value1, false);

            MAP_RESULT_VALUE result1 = getMap(&map, &key1);
            Assert::IsTrue(result1.exists);
            TEXT* text1 = (TEXT*)result1.value;
            Assert::AreEqual(value1.value, text1->value);
            Assert::IsTrue(&value1 == text1);

            TEXT value1_2 = text("アイウエオ");
            setMap(&map, &key1, &value1_2, true);
            Assert::IsFalse(value1.library.released);

            freeText(&value1);
            Assert::IsTrue(value1.library.released);

            TEXT value1_3 = text("安以宇衣於");
            setMap(&map, &key1, &value1_3, true);
            Assert::IsTrue(value1_2.library.released);

            freeMap(&map);
            Assert::IsNull(map.pairs);
            Assert::AreEqual((size_t)0, map.length);
        }

        static int compareMapKeyIgnoreCase(const TEXT* a, const TEXT* b)
        {
            return compareString(a->value, b->value, true);
        }

        TEST_METHOD(compareTest)
        {
            MAP map_default = createMap(2, compareMapKeyDefault, freeMapValueNull);

            TEXT key1 = wrap("key1");
            TEXT key2 = wrap("key2");
            TEXT key2_1 = wrap("KEY2");

            BOX_INT value1 = create(1);
            BOX_INT value2 = create(2);
            BOX_INT value2_1= create(22);

            addMap(&map_default, &key1, &value1, false);
            addMap(&map_default, &key2, &value2, false);
            Assert::IsNotNull(addMap(&map_default, &key2_1, &value2_1, false));
            Assert::IsNull(addMap(&map_default, &key2_1, &value2_1, false));
            Assert::AreEqual((size_t)3, map_default.length);

            MAP map_ignore = createMap(2, compareMapKeyIgnoreCase, freeMapValueNull);
            addMap(&map_ignore, &key1, &value1, false);
            addMap(&map_ignore, &key2, &value2, false);
            Assert::IsNull(addMap(&map_ignore, &key2_1, &value2_1, false));
            Assert::IsNull(addMap(&map_ignore, &key2_1, &value2_1, false));
            Assert::AreEqual((size_t)2, map_ignore.length);

            freeMap(&map_default);
            freeMap(&map_ignore);
        }
    };
}
