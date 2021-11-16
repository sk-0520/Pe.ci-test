#include "pch.h"

extern "C" {
#   include "../Pe.Boot/app_path.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(app_path_test)
    {
    public:

        TEST_METHOD(get_main_module_path_test)
        {
            TEXT expected = wrap("C:\\abc\\bin\\Pe.Main.exe");
            TEXT input = wrap("C:\\abc\\");
            TEXT actual = get_main_module_path(&input);

            Assert::AreEqual(expected.value, actual.value);
            release_text(&actual);
        }
    };
}
