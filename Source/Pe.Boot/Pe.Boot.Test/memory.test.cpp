#include "pch.h"
#include "CppUnitTest.h"

extern "C" {
#   include "../Pe.Boot/memory.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(memoryTest)
    {
    public:

        TEST_METHOD(allocateTest)
        {
            size_t len = 16;
            auto p1 = (unsigned char*)allocateMemory(len, false);
            for (size_t i = 0; i < len; i++) {
                p1[i] = (unsigned char)i;
            }

            auto p2 = (unsigned char*)allocateMemory(len, true);
            for (size_t i = 0; i < len; i++) {
                Assert::AreEqual((int)p2[i], 0);
                p2[i] = (unsigned char)i;
            }

            for (size_t i = 0; i < len; i++) {
                Assert::AreEqual(p1[i], p2[i]);
            }

            freeMemory(p1);
            freeMemory(p2);
        }
    };
}
