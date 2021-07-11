#pragma once
#include <tchar.h>

#include <vector>

namespace PeBootTest
{
    template<typename TExpected, typename TInput1, typename... TInputN>
    struct TestData
    {
#pragma region constructor

        TestData(TExpected expected, TInput1 input1, TInputN... inputN)
        {
            this->expected = expected;
            this->inputs = { input1, inputN... };
        }

#pragma endregion

#pragma region variable

        TExpected expected;
        std::tuple<TInput1, TInputN...> inputs;

#pragma endregion

    };

}


