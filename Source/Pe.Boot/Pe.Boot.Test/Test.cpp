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
    static void output(const TCHAR* message)
    {
        Logger::WriteMessage(message);
    }

    TEST_MODULE_INITIALIZE(initialize)
    {
        rc__initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
    }
#endif

    TEST_MODULE_CLEANUP(cleanup)
    {
#ifdef RES_CHECK
        rc__print(true);
        rc__uninitialize();
#endif
    }
}


