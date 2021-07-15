#pragma once
#include <tchar.h>

#include <vector>

#define text(s) newText(_T(s))
#define wrap(s) wrapText(_T(s))

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

    template<typename TPrimitive>
    struct BOX
    {
        TPrimitive value;
    };
    using BOX_INT = BOX<int>;
    using BOX_SIZE_T = BOX<size_t>;

    template<typename TPrimitive>
    BOX<TPrimitive> create(TPrimitive value)
    {
        BOX<TPrimitive> result = {
            value,
        };
        return result;
    }
}


