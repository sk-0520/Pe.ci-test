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
            TCHAR input[] = _T("あいう");
            TEXT c = createText(input);
            TEXT w = wrapText(input);

            Assert::IsTrue(c._needRelease);
            Assert::IsFalse(w._needRelease);

            Assert::IsFalse(c.value == input);
            Assert::IsTrue(w.value == input);

            Assert::AreEqual(getStringLength(input), c.length);
            Assert::AreEqual(getStringLength(input), w.length);

            input[0] = _T('え');
            input[1] = _T('お');
            input[2] = 0;

            Assert::AreNotEqual(_T("えお"), c.value);
            Assert::AreEqual(_T("えお"), w.value);
            Assert::AreNotEqual(getStringLength(input), c.length);

            Assert::IsTrue(freeText(&c));
            Assert::IsFalse(freeText(&w));

            Assert::IsTrue(c._released);
            Assert::IsFalse(w._released);
        }

    };
}
