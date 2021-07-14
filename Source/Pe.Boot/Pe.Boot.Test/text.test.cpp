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
            TEXT c = newText(input);
            TEXT w = wrapText(input);

            Assert::IsTrue(c._mng.needRelease);
            Assert::IsFalse(w._mng.needRelease);

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

            TEXT dc = cloneText(&c);
            TEXT dw = cloneText(&w);

            Assert::IsTrue(dc._mng.needRelease);
            Assert::IsTrue(dw._mng.needRelease);

            Assert::AreEqual(c.value, dc.value);
            Assert::AreEqual(w.value, dw.value);

            Assert::IsTrue(freeText(&c));
            Assert::IsFalse(freeText(&w));

            Assert::IsTrue(c._mng.released);
            Assert::IsFalse(w._mng.released);
        }

    };
}
