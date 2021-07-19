#include "pch.h"

extern "C" {
#   include "../Pe.Boot/memory.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(memory_test)
    {
    public:

        TEST_METHOD(allocate_test)
        {
            size_t len = 16;
            auto p1 = (unsigned char*)allocate_memory(len, false);
            for (size_t i = 0; i < len; i++) {
                p1[i] = (unsigned char)i;
            }

            auto p2 = (unsigned char*)allocate_memory(len, true);
            for (size_t i = 0; i < len; i++) {
                unsigned char c = p2[i];
                Assert::IsFalse(c);
                p2[i] = (unsigned char)i;
            }

            for (size_t i = 0; i < len; i++) {
                Assert::AreEqual(p1[i], p2[i]);
            }

            free_memory(p1);
            free_memory(p2);
        }
    };
}
