#pragma once
#include <tchar.h>

namespace PeBootTest
{
    struct TestInfo
    {
        TestInfo(const TCHAR* function, const TCHAR* file, int line) {
            this->function = function;
            this->file = file;
            this->line = line;
        }

        const TCHAR* function;
        const TCHAR* file;
        int line;
    };

    class Test {
    public:
        static void IsTrue(bool actual, const TestInfo& info);
        static void IsFalse(bool actual, const TestInfo& info);
        static void AreEqual(int expected, int actual, const TestInfo& info);
        static void AreEqual(const TCHAR* expected, const TCHAR* actual, const TestInfo& info);
    };

#define INFO TestInfo(__FUNCTIONW__, __FILEW__, __LINE__)
#define Test_IsTrue(condition) Test::IsTrue((condition), INFO)
#define Test_IsFalse(condition) Test::IsFalse((condition), INFO)
#define Test_AreEqual(expected, actual) Test::AreEqual((expected), (actual), INFO)

}


