#include "pch.h"
#ifdef RES_CHECK
extern "C" {
#   include "../Pe.Boot/res_check.h"
}
#endif

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
#ifdef RES_CHECK
    static void output(TCHAR* message)
    {
        Logger::WriteMessage(message);
    }
#endif

    TEST_MODULE_CLEANUP(cleanup)
    {
#ifdef RES_CHECK
        rc_heap__print_allocate_memory(true, output, false);
#endif
    }
}


