#include "pch.h"

#if 0
extern "C" {
#   include "../Pe.Boot/tstring.h"
}

namespace PeBootTest
{
    void Test::failCore(const TCHAR* message, const TestInfo& info)
    {
        Microsoft::VisualStudio::CppUnitTestFramework::__LineInfo lineInfo = Microsoft::VisualStudio::CppUnitTestFramework::__LineInfo(
            info.file,
            info.function,
            info.line
        );
        Microsoft::VisualStudio::CppUnitTestFramework::Assert::Fail(message, &lineInfo);
    }

    void Test::isTrue(bool condition, const TestInfo& info)
    {
    }

    void Test::isFalse(bool condition, const TestInfo& info)
    {
    }

    void Test::isNull(void* p, const TestInfo& info)
    {

    }

    void Test::areEqual(int expected, int actual, const TestInfo& info)
    {
        if (expected != actual) {
            TCHAR message[1024];
            formatString(message, _T("expected: %d, actual: %d"), expected, actual);
            failCore(message, info);
        }
    }

    void Test::areEqual(const TCHAR* expected, const TCHAR* actual, const TestInfo& info)
    {

    }

}

#endif
