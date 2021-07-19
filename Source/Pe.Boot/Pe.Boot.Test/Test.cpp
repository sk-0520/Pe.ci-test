#include "pch.h"
#ifdef MEM_CHECK
extern "C" {
#   include "../Pe.Boot/memory.h"
}
#endif

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
#ifdef MEM_CHECK
    static void output(TCHAR* message)
    {
        Logger::WriteMessage(message);
    }
#endif

    TEST_MODULE_CLEANUP(cleanup)
    {
#ifdef MEM_CHECK
        mem_check__print_allocate_memory(true, output, false);
#endif
    }
}


