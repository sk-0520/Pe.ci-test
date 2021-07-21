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

    static void initialize_fsio_test()
    {

    }

    TEST_MODULE_INITIALIZE(initialize)
    {
#ifdef RES_CHECK
        rc__initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
#endif
        TEST::initialize();
    }

    TEST_MODULE_CLEANUP(cleanup)
    {
#ifdef RES_CHECK
        rc__print(true);
        rc__uninitialize();
#endif
    }

    bool TEST::is_initialized = false;
    tstring TEST::test_root_directory_path;

    void TEST::initialize()
    {
        Assert::IsFalse(is_initialized);

        // https://stackoverflow.com/a/25151971
#define STRINGIFY(x) #x
#define EXPAND(x) STRINGIFY(x)
        auto ut_dir = tstring(_T(EXPAND(UT_DIR)));
        ut_dir.erase(0, 1);
#undef EXPAND
#undef STRINGIFY
        ut_dir.erase(ut_dir.size() - 2);
        test_root_directory_path = ut_dir;

        auto work_dir = get_path_from_test_dir(work_dir_name());
        initialize_directory_core(work_dir);
        is_initialized = true;
    }
}


