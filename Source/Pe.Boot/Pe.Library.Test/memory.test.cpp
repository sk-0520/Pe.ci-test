#include "pch.h"

extern "C" {
#   include "../Pe.Library/memory.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(memory_test)
    {
    public:

        TEST_METHOD(allocate_test)
        {
            size_t len = 16;
            auto p1 = (unsigned char*)allocate_raw_memory(len, false, DEFAULT_MEMORY);
            for (size_t i = 0; i < len; i++) {
                p1[i] = (unsigned char)i;
            }

            auto p2 = (unsigned char*)allocate_raw_memory(len, true, DEFAULT_MEMORY);
            for (size_t i = 0; i < len; i++) {
                unsigned char c = p2[i];
                Assert::IsFalse(c);
                p2[i] = (unsigned char)i;
            }

            for (size_t i = 0; i < len; i++) {
                Assert::AreEqual(p1[i], p2[i]);
            }

            release_memory(p1, DEFAULT_MEMORY);
            release_memory(p2, DEFAULT_MEMORY);
        }

        TEST_METHOD(library__extend_capacity_if_not_enough_bytes_x2_test)
        {
            const byte_t default_cap = 2;
            void* p1 = allocate_raw_memory(8, false, DEFAULT_MEMORY);

            void* old_p1 = p1;

            Assert::AreEqual((byte_t)0, library__extend_capacity_if_not_enough_bytes_x2(&p1, 0, 8, 7, default_cap, DEFAULT_MEMORY));
            Assert::AreEqual(old_p1, p1);
            Assert::AreEqual((byte_t)0, library__extend_capacity_if_not_enough_bytes_x2(&p1, 0, 8, 8, default_cap, DEFAULT_MEMORY));
            Assert::AreEqual(old_p1, p1);

            byte_t extends1 = library__extend_capacity_if_not_enough_bytes_x2(&p1, 0, 8, 9, default_cap, DEFAULT_MEMORY);
            Assert::AreEqual((byte_t)16, extends1);
            Assert::AreNotEqual(old_p1, p1);

            void* old_p2 = p1;

            Assert::AreEqual((byte_t)0, library__extend_capacity_if_not_enough_bytes_x2(&p1, 14, 16, 1, default_cap, DEFAULT_MEMORY));
            Assert::AreEqual((byte_t)0, library__extend_capacity_if_not_enough_bytes_x2(&p1, 15, 16, 1, default_cap, DEFAULT_MEMORY));

            byte_t extends2 = library__extend_capacity_if_not_enough_bytes_x2(&p1, 15, 16, 2, default_cap, DEFAULT_MEMORY);
            Assert::AreEqual((byte_t)32, extends2);
            Assert::AreNotEqual(old_p2, p1);

            byte_t extends3 = library__extend_capacity_if_not_enough_bytes_x2(&p1, 0, 8, 4097, default_cap, DEFAULT_MEMORY);
            Assert::AreEqual((byte_t)8192, extends3);


            release_memory(p1, DEFAULT_MEMORY);
        }
    };
}
