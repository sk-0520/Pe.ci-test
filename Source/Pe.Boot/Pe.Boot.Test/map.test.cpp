﻿#include "pch.h"

extern "C" {
#   include "../Pe.Boot/map.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(mapTest)
    {
    private:
        struct BOX_INT
        {
            int value;
        };

        BOX_INT create(int value)
        {
            BOX_INT result{
                value,
            };
            return result;
        }

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
    };
}
