#include "pch.h"

extern "C" {
#   include "../Pe.Library/object_list.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(object_list_test)
    {
    public:
        TEST_METHOD(life_test)
        {
            OBJECT_LIST list = create_object_list(2, compare_object_list_value_null, free_object_list_value_null);
            Assert::AreEqual((size_t)0, list.length);
            Assert::AreEqual((size_t)2, list.library.capacity);
            Assert::IsFalse(get_object_list(&list, 0).exists);

            auto input1 = create(1);
            push_object_list(&list, &input1, false);
            Assert::AreEqual((size_t)1, list.length);
            Assert::AreEqual((size_t)2, list.library.capacity);

            auto input2 = create(2);
            push_object_list(&list, &input2, false);
            Assert::AreEqual((size_t)2, list.length);
            Assert::AreEqual((size_t)2, list.library.capacity);

            auto input3 = create(3);
            push_object_list(&list, &input3, false);
            Assert::AreEqual((size_t)3, list.length);
            Assert::AreEqual((size_t)4, list.library.capacity);

            BOX_INT* result3 = new BOX_INT;
            Assert::IsTrue(pop_object_list((void**)&result3, &list));
            Assert::AreEqual(input3.value, result3->value);
            Assert::AreEqual((size_t)2, list.length);
            Assert::AreEqual((size_t)4, list.library.capacity);
            //delete result3;

            auto input3_2 = create(32);
            push_object_list(&list, &input3_2, false);

            Assert::IsFalse(get_object_list(&list, 3).exists);
            Assert::IsTrue(get_object_list(&list, 2).exists);
            Assert::AreEqual(input3_2.value, ((BOX_INT*)(get_object_list(&list, 2).value))->value);

            auto input3_3 = create(33);
            Assert::IsFalse(set_object_list(&list, 3, &input3_3, false));
            Assert::IsTrue(set_object_list(&list, 2, &input3_3, false));
            Assert::IsTrue(get_object_list(&list, 2).exists);
            Assert::AreNotEqual(input3_2.value, ((BOX_INT*)(get_object_list(&list, 2).value))->value);
            Assert::AreEqual(input3_3.value, ((BOX_INT*)(get_object_list(&list, 2).value))->value);

            clear_object_list(&list);
            Assert::AreEqual((size_t)0, list.length);
            Assert::AreEqual((size_t)4, list.library.capacity);

            free_object_list(&list);
        }
    };
}
