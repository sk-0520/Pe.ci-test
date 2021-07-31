#include "pch.h"

#ifdef RES_CHECK
extern "C" {
#   include "../Pe.Library/res_check.h"
#   include "../Pe.Library/path.h"
}
#endif

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

#ifdef RES_CHECK
static void output(const TCHAR* message)
{
    Logger::WriteMessage(message);
}
#endif

TestImpl TEST(tstring(_T("PeBootTest")));

TEST_MODULE_INITIALIZE(initialize)
{
#ifdef RES_CHECK
    rc__initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
#endif
}

TEST_MODULE_CLEANUP(cleanup)
{
#ifdef RES_CHECK
    rc__print(true);
    auto  exists_resource_leak = rc__exists_resource_leak();
    rc__uninitialize();
    Assert::IsFalse(exists_resource_leak);
#endif
}
