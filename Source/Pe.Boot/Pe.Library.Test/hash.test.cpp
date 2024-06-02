#include "pch.h"

extern "C" {
#   include "../Pe.Library/hash.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(fsio_test)
    {
    public:
        TEST_METHOD(calc_fnv1_from_text_hash_test)
        {
            auto tests = {
                DATA(true, wrap("A"), wrap("A")),
                DATA(false, wrap("A"), wrap("a")),
                DATA(false, wrap("ABC"), wrap("AB")),
            };

            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                TEXT& arg2 = std::get<1>(test.inputs);
                auto actual1 = calc_hash_fnv1_from_text(&arg1);
                auto actual2 = calc_hash_fnv1_from_text(&arg2);
                Assert::AreEqual(test.expected, actual1 == actual2);
            }
        }
    };
}
