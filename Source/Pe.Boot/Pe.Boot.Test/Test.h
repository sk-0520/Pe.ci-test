#pragma once
#include <tchar.h>

#include <vector>

namespace PeBootTest
{
    template<typename TExpected, typename TInput>
    struct TestData
    {
#pragma region constructor

        TestData(TExpected expected, TInput input1)
        {
            this->expected = expected;
            this->inputs = { input1 };
        }

        TestData(TExpected expected, TInput input1, TInput input2)
        {
            this->expected = expected;
            this->inputs = { input1, input2 };
        }

        TestData(TExpected expected, TInput input1, TInput input2, TInput input3)
        {
            this->expected = expected;
            this->inputs = { input1, input2, input3 };
        }

        TestData(TExpected expected, std::initializer_list<TInput> inputs)
        {
            this->expected = expected;
            this->inputs = inputs;
        }

#pragma endregion

#pragma region variable

        TExpected expected;
        std::vector<TInput> inputs;

#pragma endregion

    };

}


