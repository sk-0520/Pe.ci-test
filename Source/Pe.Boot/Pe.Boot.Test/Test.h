#pragma once
#include <vector>
#include <tuple>
#include <filesystem>

#include <tchar.h>
#include <atlstr.h>


#ifdef RES_CHECK
#   define text(s) rc_heap__new_text(_T(s), _T(__FILE__), __LINE__)
#else
#   define text(s) new_text(_T(s))
#endif

#ifdef _UNICODE
typedef std::wstring tstring;
#else
typedef std::string tstring;
#endif

#define wrap(s) wrap_text(_T(s))

/// テストパータン内で使用するディレクトリの初期化処理
#define TEST_INIT_DIR PeBootTest::TEST::initialize_test_pattern_work_directory(_T(__FUNCTION__));
/// テストパターン内で使用可能なファイルパスをvarに設定する
#define TEST_GET_PATH(var, file_name) tstring var; PeBootTest::TEST::get_test_pattern_work_path(var, _T(__FUNCTION__), file_name)


namespace PeBootTest
{
    template<typename TExpected, typename TInput1, typename... TInputN>
    struct DATA
    {
#pragma region constructor

        DATA(TExpected expected, TInput1 input1, TInputN... inputN)
        {
            this->expected = expected;
            this->inputs = { input1, inputN... };
        }

#pragma endregion

#pragma region variable

        TExpected expected;
        std::tuple<TInput1, TInputN...> inputs;

#pragma endregion

    };

    template<typename TPrimitive>
    struct BOX
    {
        TPrimitive value;
    };
    using BOX_INT = BOX<int>;
    using BOX_SIZE_T = BOX<size_t>;

    template<typename TPrimitive>
    BOX<TPrimitive> create(TPrimitive value)
    {
        BOX<TPrimitive> result = {
            value,
        };
        return result;
    }

    class TEST
    {
    private:
        static bool TEST::is_initialized;
        static tstring test_root_directory_path;

        static void get_path_from_test_dir_core(tstring& result, std::initializer_list<tstring> path_items)
        {
            std::filesystem::path path = test_root_directory_path;
            for (auto path_item : path_items) {
                path /= path_item;
            }
            result = path;
        }

        template<typename... more_sub_item_path>
        static void get_path_from_test_dir(tstring& result, tstring sub_item_path, more_sub_item_path... sub_item_paths)
        {
            auto list = std::initializer_list<tstring>{ sub_item_path, sub_item_paths... };
            get_path_from_test_dir_core(result, list);
        }

        static void initialize_directory_core(tstring path)
        {
            if (std::filesystem::exists(path)) {
                std::filesystem::remove_all(path);
            }
            std::filesystem::create_directories(path);
        }

        static tstring work_dir_name()
        {
            return tstring(_T("work"));
        }

        static void make_test_pattern_work_directory_path(tstring& result, const TCHAR* msvc_function, const TCHAR* sub_path = nullptr)
        {
            const TCHAR name_space[] = _T("PeBootTest::");
            const TCHAR* class_method_pair = msvc_function + (sizeof(name_space) / sizeof(TCHAR)) - 1;
            const TCHAR separator[] = _T("::");
            const TCHAR* raw_separator = StrStrI(class_method_pair, separator);

            auto class_name = tstring(class_method_pair, raw_separator - class_method_pair);
            auto method_name = tstring(raw_separator + (sizeof(separator) / sizeof(TCHAR)) - 1);

            if (sub_path) {
                get_path_from_test_dir(result, work_dir_name(), class_name, method_name, tstring(sub_path));
            } else {
                get_path_from_test_dir(result, work_dir_name(), class_name, method_name);
            }
        }

    public:
        /// <summary>
        /// テスト用ヘルパの初期化。
        /// </summary>
        static void initialize();

        /// <summary>
        /// テスト用ヘルパの最終処理。
        /// </summary>
        static void cleanup();

        /// <summary>
        /// テストパターン内で使用するディレクトリ初期化処理。
        /// <para>原則、明示的な使用はせず<c>TEST_INIT_DIR</c>を用いること。</para>
        /// </summary>
        static void initialize_test_pattern_work_directory(const TCHAR* msvc_function)
        {
            Microsoft::VisualStudio::CppUnitTestFramework::Assert::IsTrue(is_initialized);

            tstring work_dir;
            make_test_pattern_work_directory_path(work_dir, msvc_function);
            initialize_directory_core(work_dir);
        }

        /// <summary>
        /// テストパターン内で使用可能なファイルパスの取得処理。
        /// <para>原則、明示的な使用はせず<c>TEST_GET_PATH</c>を用いること。</para>
        /// </summary>
        /// <param name="msvc_function"></param>
        /// <param name="result"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        static void get_test_pattern_work_path(tstring& result, const TCHAR* msvc_function, TCHAR* path)
        {
            Microsoft::VisualStudio::CppUnitTestFramework::Assert::IsTrue(is_initialized);

            make_test_pattern_work_directory_path(result, msvc_function, path);
        }

    };

}


