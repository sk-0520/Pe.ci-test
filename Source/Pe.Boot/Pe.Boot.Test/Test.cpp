#include "pch.h"
#ifdef MEM_CHECK
extern "C" {
#   include "../Pe.Boot/memory.h"
}
#endif

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    static void output(TCHAR* message)
    {
        Logger::WriteMessage(message);
    }

    TEST_MODULE_CLEANUP(cleanup)
    {
#ifdef MEM_CHECK
        mem_check__printAllocateMemory(true, output, false);
#endif
    }
}


