#include "pch.h"

extern "C" {
#   include "../Pe.Library/lib_math.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(lib_math_test)
    {
    public:
        TEST_METHOD(power_of_2_test)
        {
            auto tests = {
                DATA(0, 0),
                DATA(2, 1),
                DATA(2, 2),
                DATA(4, 3),
                DATA(4, 4),
                DATA(8, 5),
                DATA(8, 8),
                DATA(16, 9),
                DATA(16, 15),
                DATA(16, 16),
                DATA(32, 17),
                DATA(32, 31),
                DATA(32, 32),
                DATA(1024, 1023),
                DATA(1024, 1024),
            };

            for (auto test : tests) {
                test.run(power_of_2);
            }
        }
    };
}
