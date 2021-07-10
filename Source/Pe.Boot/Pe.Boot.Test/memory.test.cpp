#include "pch.h"

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
            Test_AreEqual(123,456);

            size_t len = 16;
            auto p1 = (unsigned char*)allocateMemory(len, false);
            for (size_t i = 0; i < len; i++) {
                p1[i] = (unsigned char)i;
            }

            auto p2 = (unsigned char*)allocateMemory(len, true);
            for (size_t i = 0; i < len; i++) {
                unsigned char c = p2[i];
                Test_IsFalse(c);
                p2[i] = (unsigned char)i;
            }

            for (size_t i = 0; i < len; i++) {
                Test_AreEqual(p1[i], p2[i]);
            }

            freeMemory(p1);
            freeMemory(p2);
        }
    };
}
