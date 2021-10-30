#include "pch.h"

extern "C" {
#   include "../Pe.Library/text.h"
#   include "../Pe.Script/lexical_analyzer.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace ScriptTest
{
    TEST_CLASS(lexical_analyzer_test)
    {
    public:

        TEST_METHOD(analyze_empty_test)
        {
            PROJECT_SETTING setting;
            TOKEN_RESULT actual = analyze(NULL, &setting);
            Assert::AreEqual((size_t)0, actual.token.length);
            Assert::AreEqual((size_t)0, actual.result.length);
            free_token_result(&actual);
        }
    };
}
