#pragma once

#if 0

#include <tchar.h>

namespace PeBootTest
{
    struct TestInfo
    {
        TestInfo(const TCHAR* args, const char* function, const TCHAR* file, int line) {
            this->args = args;
            this->function = function;
            this->file = file;
            this->line = line;
        }

        const TCHAR* args;
        const char* function;
        const TCHAR* file;
        int line;
    };

    class Test {
    private:
        static void failCore(const TCHAR* message, const TestInfo& info);

    public:
        static void isTrue(bool actual, const TestInfo& info);
        static void isFalse(bool actual, const TestInfo& info);
        static void isNull(void* p, const TestInfo& info);
        static void areEqual(int expected, int actual, const TestInfo& info);
        static void areEqual(const TCHAR* expected, const TCHAR* actual, const TestInfo& info);
    };

#define INFO(args) TestInfo(_T(#args), __func__, __FILEW__, __LINE__)
#define Test_IsTrue(condition) Test::isTrue((condition), INFO(condition))
#define Test_IsFalse(condition) Test::isFalse((condition), INFO(condition))
#define Test_IsNull(p) Test::isNull((p), INFO(p))
#define Test_AreEqual(expected, actual) Test::areEqual((expected), (actual), INFO(expected "," actual))

}

#endif
