#include "pch.h"

extern "C" {
#   include "../Pe.Boot/app_path.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(appPathTest)
    {
    public:

        TEST_METHOD(getMainModulePath2Test)
        {
            TEXT expected = wrap("C:\\abc\\bin\\Pe.Main.exe");
            TEXT input = wrap("C:\\abc\\");
            TEXT actual = getMainModulePath2(&input);

            Assert::AreEqual(expected.value, actual.value);
            freeText(&actual);
        }
    };
}
