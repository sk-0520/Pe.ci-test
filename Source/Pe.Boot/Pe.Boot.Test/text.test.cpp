#include "pch.h"

extern "C" {
#   include "../Pe.Boot/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(textTest)
    {
    public:

        TEST_METHOD(newTest)
        {
            TEXT c = createText(_T("あいう"));
            TEXT w = wrapText(_T("あいう"));

            Assert::IsTrue(c._needRelease);
            Assert::IsFalse(w._needRelease);
        }

    };
}
