#pragma once
#include <vector>
#include <tuple>
#include <fstream>
#include <filesystem>
#include <iostream>
#include <string>

#include <tchar.h>
#include <atlstr.h>

extern "C" {
#   include "../Pe.Library/logging.h"
}

namespace fs = std::filesystem;
namespace mstest = Microsoft::VisualStudio::CppUnitTestFramework;

#ifdef RES_CHECK
#   define text(s) library_rc_heap_new_text(RELATIVE_FILET, __LINE__, _T(s), DEFAULT_MEMORY_ARENA)
#else
#   define text(s) new_text(_T(s), DEFAULT_MEMORY_ARENA)
#endif

#ifdef _UNICODE
typedef std::wstring tstring;
#else
typedef std::string tstring;
#endif

#define wrap(s) wrap_text(_T(s))

/// テストパータン内で使用するディレクトリの初期化処理
#define TEST_INIT_DIR TEST.initialize_test_pattern_work_directory(_T(__FUNCTION__))
/// テストパターン内で使用可能なファイルパスを (C++)test_var, (C)var に設定する
#define TEST_GET_PATH(var, file_name) tstring test_##var; TEST.get_test_pattern_work_path(test_##var, _T(__FUNCTION__), file_name); TEXT var = wrap_text(test_##var.c_str())

template<typename TExpected, typename TInput1, typename... TInputN>
struct DATA
{
    /// <summary>
    /// テストデータ生成。
    /// <para>簡易実行: <c>run(test_func)</c></para>
    /// <para>通常実行: <c>std::apply(test_func, DATA.inputs)</c></para>
    /// <para>引数指定: <c>test_func(std::get&lt;0&gt;(DATA.inputs))</c></para>
    /// </summary>
    /// <param name="expected">期待値。</param>
    /// <param name="input1">入力値1。</param>
    /// <param name="...inputN">入力値N。</param>
    DATA(TExpected expected, TInput1 input1, TInputN... inputN)
    {
        this->expected = expected;
        this->inputs = { input1, inputN... };
    }

    /// <summary>
    /// 期待値。
    /// </summary>
    TExpected expected;
    /// <summary>
    /// 入力値一覧。
    /// </summary>
    std::tuple<TInput1, TInputN...> inputs;

    /// <summary>
    /// 指定した関数に対して入力値一覧を適用して実行。
    /// </summary>
    /// <param name="func">テストする関数。</param>
    /// <returns>実行結果。</returns>
    template <class _Callable>
    constexpr decltype(auto) run(_Callable&& func)
    {
        return std::apply(func, this->inputs);
    }
};

template<typename TPrimitive>
struct BOX
{
    TPrimitive value;

    static BOX<TPrimitive> create(TPrimitive value)
    {
        BOX<TPrimitive> result = {
            value,
        };
        return result;
    }

};
using BOX_INT = BOX<int>;
using BOX_SIZE_T = BOX<size_t>;

/// <summary>
/// pch.h の中で TEST として外部参照宣言しておいて各テストプロジェクトの test.cpp に実体を作っておくこと。
/// </summary>
class TestImpl
{
private:
    tstring test_root_directory_path;
    tstring test_namespace_name;

    void get_path_from_test_dir_core(tstring& result, std::initializer_list<tstring> path_items)
    {
        std::filesystem::path path = test_root_directory_path;
        for (auto path_item : path_items) {
            path /= path_item;
        }
        result = path;
    }

    template<typename... more_sub_item_path>
    void get_path_from_test_dir(tstring& result, tstring sub_item_path, more_sub_item_path... sub_item_paths)
    {
        auto list = std::initializer_list<tstring>{ sub_item_path, sub_item_paths... };
        get_path_from_test_dir_core(result, list);
    }

    void initialize_directory_core(tstring path)
    {
        if (std::filesystem::exists(path)) {
            std::filesystem::remove_all(path);
        }
        std::filesystem::create_directories(path);
    }

    void make_test_pattern_work_directory_path(tstring& result, const TCHAR* msvc_function, const TCHAR* sub_path = nullptr)
    {
        tstring  name_space = test_namespace_name + _T("::");
        const TCHAR* class_method_pair = msvc_function + name_space.length();
        const TCHAR separator[] = _T("::");
        const TCHAR* raw_separator = StrStrI(class_method_pair, separator);

        auto class_name = tstring(class_method_pair, raw_separator - class_method_pair);
        auto method_name = tstring(raw_separator + (sizeof(separator) / sizeof(TCHAR)) - 1);

        if (sub_path) {
            get_path_from_test_dir(result, work_dir_name, class_name, method_name, tstring(sub_path));
        } else {
            get_path_from_test_dir(result, work_dir_name, class_name, method_name);
        }
    }

    static void output(const TCHAR* message)
    {
        Microsoft::VisualStudio::CppUnitTestFramework::Logger::WriteMessage(message);
    }

    void logging(const LOG_ITEM* log_item)
    {
        tstring message(_T("["));
        message += this->test_namespace_name;
        message += _T("] ");

        switch (log_item->log_level) {
            case LOG_LEVEL_TRACE:
                message += _T("trace");
                break;

            case LOG_LEVEL_DEBUG:
                message += _T("debug");
                break;

            case LOG_LEVEL_INFO:
                message += _T("info ");
                break;

            case LOG_LEVEL_WARNING:
                message += _T("warn ");
                break;

            case LOG_LEVEL_ERROR:
                message += _T("error");
                break;
        }

        message += _T(" | ");

        message += log_item->message->value;

        message += _T(" | ");
        message += log_item->caller_file->value;
        message += _T("(");
        message += std::to_wstring(log_item->caller_line);
        message += _T(")");

        output(message.c_str());
    }

    static void logging(const LOG_ITEM* log_item, void* data)
    {
        auto _this = (TestImpl*)data;
        _this->logging(log_item);
    }


public:
    tstring work_dir_name = tstring(_T("work"));

    /// <summary>
    /// テスト用ヘルパの初期化。
    /// </summary>
    TestImpl(tstring namespace_name)
    {
        test_namespace_name = namespace_name;

#ifdef RES_CHECK
        library_rc_initialize(output, RES_CHECK_INIT_PATH_LENGTH, RES_CHECK_INIT_BUFFER_LENGTH, RES_CHECK_INIT_HEAP_COUNT, RES_CHECK_INIT_FILE_COUNT);
#endif


        LOGGER logger;
        logger.function = logging;
        logger.data = this,
        attach_logger(&logger);
        initialize_logger(DEFAULT_MEMORY_ARENA);
        logger_put_info(_T("TEST START"));

        // https://stackoverflow.com/a/25151971
        auto ut_dir = tstring(_T(TO_STRING(UT_DIR)));
        ut_dir.erase(0, 1);
        ut_dir.erase(ut_dir.size() - 2);
        test_root_directory_path = ut_dir;

        tstring work_dir;
        get_path_from_test_dir(work_dir, work_dir_name);
        initialize_directory_core(work_dir);
    }

    /// <summary>
    /// テスト用ヘルパの最終処理。
    /// </summary>
    ~TestImpl()
    {
        tstring work_dir;
        get_path_from_test_dir(work_dir, work_dir_name);
        mstest::Logger::WriteMessage((tstring(_T("[FILE LIST] -> ")) + work_dir).c_str());

        for (const auto& file : std::filesystem::recursive_directory_iterator(work_dir)) {
            auto path = tstring(file.path().c_str() + work_dir.size() + 1/* \ */);
            mstest::Logger::WriteMessage((tstring(_T("> ")) + (tstring(file.is_directory() ? _T("[D] ") : _T("[F] "))) + path).c_str());
        }

        logger_put_info(_T("TEST END"));

#ifdef RES_CHECK
        library_rc_print(true);
        auto  exists_resource_leak = library_rc_exists_resource_leak();
        library_rc_finalize();

        //#ifdef DEBUG
        Microsoft::VisualStudio::CppUnitTestFramework::Assert::IsFalse(exists_resource_leak);
        //#endif
#endif
    }


    /// <summary>
    /// テストパターン内で使用するディレクトリ初期化処理。
    /// <para>原則、明示的な使用はせず<c>TEST_INIT_DIR</c>を用いること。</para>
    /// </summary>
    void initialize_test_pattern_work_directory(const TCHAR* msvc_function)
    {
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
    void get_test_pattern_work_path(tstring& result, const TCHAR* msvc_function, TCHAR* path)
    {
        make_test_pattern_work_directory_path(result, msvc_function, path);
    }

    /// <summary>
    /// テスト用ディレクトリ作成ヘルパ。
    /// </summary>
    /// <param name="path"></param>
    void create_directory(tstring path)
    {
        std::filesystem::create_directories(path);
    }

    /// <summary>
    /// テスト用ファイル作成ヘルパ。
    /// </summary>
    /// <param name="path"></param>
    void create_empty_file(tstring path)
    {
        std::wofstream f(path);
        f.close();
    }
};

