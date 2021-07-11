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
            //std::tuple<TExpected, TInput1, TInputN...>
        }
        /*

        TestData(TExpected expected, TInput input1, TInput input2, TInput input3)
        {
            this->expected = expected;
            this->inputs = { input1, input2, input3 };
        }

        TestData(TExpected expected, std::initializer_list<TInput> inputs)
        {
            this->expected = expected;
            this->inputs = inputs;
        }*/

#pragma endregion

#pragma region variable

        TExpected expected;
        std::tuple<TInput1, TInputN...> inputs;

#pragma endregion

    };

}


