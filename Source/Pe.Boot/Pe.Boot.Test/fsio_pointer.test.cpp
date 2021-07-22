#include "pch.h"

extern "C" {
#   include "../Pe.Boot/fsio.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(fsio_pointer_test)
    {
    public:
        TEST_METHOD(create_file_test)
        {
            TEST_INIT_DIR;

            TEST_GET_PATH(path1, _T("file"));

            FILE_POINTER actual1 = create_file(&path1);
        }
    };
}
