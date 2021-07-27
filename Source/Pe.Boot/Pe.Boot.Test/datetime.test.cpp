#include "pch.h"

extern "C" {
#   include "../Pe.Boot/datetime.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(datetime_test)
    {
    public:
#if 0
        TEST_METHOD(local_jp_test)
        {
            auto local_input = create_datetime(false, 2021, 7, 27, 0, 0, 0, 0);
            auto local_utc = datetime_to_timestamp(&local_input, true);
            auto local_local = datetime_to_timestamp(&local_input, false);
            Assert::AreEqual(26, (int)local_utc.day);
            Assert::AreEqual(15, (int)local_utc.hour);
            Assert::AreEqual(27, (int)local_local.day);
            Assert::AreEqual(0, (int)local_local.hour);

            auto utc_input = create_datetime(true, 2021, 7, 27, 16, 0, 0, 0);
            auto utc_utc = datetime_to_timestamp(&utc_input, true);
            auto utc_local = datetime_to_timestamp(&utc_input, false);
            Assert::AreEqual(27, (int)utc_utc.day);
            Assert::AreEqual(16, (int)utc_utc.hour);
            Assert::AreEqual(28, (int)utc_local.day);
            Assert::AreEqual(1, (int)utc_local.hour);
        }
#endif
    };
}
