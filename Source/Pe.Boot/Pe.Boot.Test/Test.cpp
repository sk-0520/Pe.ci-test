#include "pch.h"

#ifdef RES_CHECK
extern "C" {
#   include "../Pe.Boot/res_check.h"
#   include "../Pe.Boot/path.h"
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
#endif

    TEST_MODULE_INITIALIZE(initialize)
    {
#ifdef RES_CHECK
        rc__initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
#endif
        TEST::initialize();
    }

    TEST_MODULE_CLEANUP(cleanup)
    {
        TEST::cleanup();
#ifdef RES_CHECK
        rc__print(true);
        auto  exists_resource_leak = rc__exists_resource_leak();
        rc__uninitialize();
        Assert::IsFalse(exists_resource_leak);
#endif
    }

    // 静的メンバの実体だけおいとく
    bool TEST::is_initialized = false;
    tstring TEST::test_root_directory_path;
}


