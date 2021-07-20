#pragma once
#include <tchar.h>

#include "common.h"
#include "tstring.h"

static const TCHAR library__whitespace_characters[] = { _T(' '), _T('\t') };

/// <summary>
/// 不変文字列ラッパー。
/// </summary>
typedef struct tag_TEXT
{
    /// <summary>
    /// 値。
    /// </summary>
    const TCHAR* value;
    /// <summary>
    /// 長さ。
    /// </summary>
    size_t length;

    /// <summary>
    /// 管理データ。
    /// </summary>
    struct
    {
        /// <summary>
        /// 解放が必要か。
        /// <para>アプリケーション内では使用しない。</para>
        /// </summary>
        bool need_release : 1;
        ///// <summary>
        ///// value終端が0、つまりは通常のC文字列か。
        ///// <para>偽の場合通常文字列としては使用できない。</para>
        ///// </summary>
        //bool sentinel : 1;
        /// <summary>
        /// 解放済みか。
        /// <para>アプリケーション内では使用しない。</para>
        /// </summary>
        bool released : 1;
    } library;
} TEXT;

/// <summary>
/// テキストの配列的なもの。
/// <para>構造体のポインタとして扱いたいのかテキスト配列を扱いたいのかパッと見分からないのでこれで見た目をよくする。</para>
/// </summary>
typedef TEXT* TEXT_LIST;

/// <summary>
/// 空のテキストを生成。
/// </summary>
/// <returns>領域自体がNULLの不変文字列(通常使用は出来ない)。</returns>
TEXT create_invalid_text();

/// <summary>
/// テキストが使用可能か。
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
bool is_enabled_text(const TEXT* text);

/// <summary>
/// テキストを生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <param name="length">対象文字列の長さ。</param>
/// <returns>不変文字列。解放が必要。</returns>
#ifdef MEM_CHECK
TEXT mem_check__new_text_with_length(const TCHAR* source, size_t length, const TCHAR* caller_file, size_t caller_line);
#   define new_text_with_length(source, length) mem_check__new_text_with_length((source), (length), _T(__FILE__), __LINE__)
#else
TEXT new_text_with_length(const TCHAR* source, size_t length);
#endif

/// <summary>
/// テキストを生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>テキスト。解放が必要。</returns>
#ifdef MEM_CHECK
TEXT mem_check__new_text(const TCHAR* source, const TCHAR* caller_file, size_t caller_line);
#   define new_text(source) mem_check__new_text((source), _T(__FILE__), __LINE__)
#else
TEXT new_text(const TCHAR* source);
#endif

#ifdef MEM_CHECK
#define new_empty_text() mem_check__new_text(_T(""), _T(__FILE__), __LINE__)
#else
#define new_empty_text() new_text(_T(""))
#endif

/// <summary>
/// 文字列からテキストにラップ。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <param name="length">対象文字列の長さ。</param>
/// <param name="need_release">解放が必要か。真の場合、<c>wrapText</c>と異なり呼び出し側で確保した領域を信頼して持ち運ぶ。</param>
/// <returns></returns>
TEXT wrap_text_with_length(const TCHAR* source, size_t length, bool need_release);

/// <summary>
/// 文字列からテキストにラップ。
/// <para>スタック内で元文字列を変更せずに使用することが前提条件。</para>
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>テキスト。解放不要。</returns>
TEXT wrap_text(const TCHAR* source);
#define wrap_empty_text() wrap_text(_T(""))


/// <summary>
/// 不変文字列の複製。
/// </summary>
/// <param name="source">入力不変文字列。</param>
/// <returns>複製された不変文字列。解放が必要。</returns>
#ifdef MEM_CHECK
TEXT mem_check__clone_text(const TEXT* source, MEM_CHECK_PORT_ARGS);
#   define clone_text(source) mem_check__clone_text(source, MEM_CHECK_WRAP_ARGS)
#else
TEXT clone_text(const TEXT* source);
#endif
/// <summary>
/// テキストを参照として複製。
/// </summary>
/// <param name="source">入力テキストを。</param>
/// <returns>参照として複製されたテキストを。参照元が生きている限り生きている。解放不要。</returns>
TEXT reference_text(const TEXT* source);

/// <summary>
/// テキストをの解放。
/// <para>不要な場合は処理しない。</para>
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
#ifdef MEM_CHECK
bool mem_check__free_text(TEXT* text, MEM_CHECK_PORT_ARGS);
#   define free_text(text) mem_check__free_text(text, MEM_CHECK_WRAP_ARGS)
#else
bool free_text(TEXT* text);
#endif

/* 文字列操作ラッパー */
#include "text_search.h"
#include "text_conv.h"
#include "text_man.h"
