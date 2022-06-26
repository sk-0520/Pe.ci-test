#pragma once
#include <stddef.h>

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
    text_t length;

    /// <summary>
    /// 管理データ。
    /// </summary>
    struct
    {
        /// <summary>
        /// 確保時のメモリリソース。
        /// <para>アプリケーション内では使用しない。</para>
        /// <para><c>need_release</c>が有効な場合に使用されるため、静的・解放不要の場合は設定しない。</para>
        /// </summary>
        const MEMORY_RESOURCE* memory_resource;
        /// <summary>
        /// 解放が必要か。
        /// <para>アプリケーション内では使用しない。</para>
        /// </summary>
        bool need_release : 1;
        /// <summary>
        /// value終端が0、つまりは通常のC文字列か。
        /// <para>偽の場合通常文字列としては使用できない。</para>
        /// <para>TODO: 現状番兵が存在する前提処理が多数のため偽に設定した場合は大抵死ぬ。</para>
        /// </summary>
        bool sentinel : 1;
        /// <summary>
        /// 解放済みか。
        /// <para>アプリケーション内では使用しない。</para>
        /// </summary>
        bool released : 1;
    } library;
} TEXT;



/// <summary>
/// 内部用 静的初期化処理。
/// <para><c>_T()</c>の面倒は見ない。</para>
/// </summary>
#define static_text_core(s) { .value = s, .length = SIZEOF_ARRAY(s) - 1, .library = { .need_release = false, .sentinel = true, .released = false, } }
/// <summary>
/// 静的初期化処理。
/// <para>グローバル変数とか<c>static</c>変数のお供。</para>
/// </summary>
/// <param name="s">入力文字列リテラル。自動的に<c>_T(s)</c>される。</param>
#define static_text(s) static_text_core(_T(s))

extern const TEXT NEWLINE_CR_TEXT;
extern const TEXT NEWLINE_LF_TEXT;
extern const TEXT NEWLINE_CRLF_TEXT;
extern const TEXT NEWLINE_TEXT;

/// <summary>
/// テキストの配列的なもの。
/// <para>構造体のポインタとして扱いたいのかテキスト配列を扱いたいのかパッと見分からないのでこれで見た目をよくする。</para>
/// </summary>
typedef TEXT* TEXT_LIST;

/// <summary>
/// 無効テキストを生成。
/// </summary>
/// <returns>領域自体が<c>NULL</c>のテキスト(通常使用は出来ない)。</returns>
TEXT create_invalid_text(void);

/// <summary>
/// テキストが使用可能か。
/// <para><c>create_invalid_text</c>で作られたやつなんかは使用不可になる。</para>
/// </summary>
/// <param name="text"></param>
/// <returns>使用可能か。</returns>
bool is_enabled_text(const TEXT* text);

/// <summary>
/// 文字列長は<c>TEXT</c>で有効か。
/// <para>内部では<c>text_t</c>を使っているので<c>size_t</c>に収まらない。</para>
/// </summary>
/// <param name="length"></param>
/// <returns>有効。</returns>
bool check_text_length(size_t length);

/// <summary>
/// テキストを生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <param name="length">対象文字列の長さ。</param>
/// <returns>不変文字列。解放が必要。</returns>
TEXT RC_HEAP_FUNC(new_text_with_length, const TCHAR* source, size_t length, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define new_text_with_length(source, length, memory_resource) RC_HEAP_WRAP(new_text_with_length, (source), (length), memory_resource)
#endif

/// <summary>
/// テキストを生成。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>テキスト。解放が必要。</returns>
TEXT RC_HEAP_FUNC(new_text, const TCHAR* source, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define new_text(source, memory_resource) RC_HEAP_WRAP(new_text, (source), memory_resource)
#endif

TEXT RC_HEAP_FUNC(new_empty_text, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define new_empty_text(memory_resource) RC_HEAP_WRAP(new_empty_text, memory_resource)
#endif

/// <summary>
/// 文字列からテキストにラップ。
/// </summary>
/// <param name="source">対象文字列。</param>
/// <param name="length">対象文字列の長さ。</param>
/// <param name="need_release">解放が必要か。真の場合、<c>wrapText</c>と異なり呼び出し側で確保した領域を信頼して持ち運ぶ。</param>
/// <param name="memory_resource"><param name="need_release" />が真の場合に必要。</param>
/// <returns>番兵使用不可のテキスト。</returns>
TEXT wrap_text_with_length(const TCHAR* source, size_t length, bool need_release, const MEMORY_RESOURCE* memory_resource);

/// <summary>
/// 文字列からテキストにラップ。
/// <para>スタック内で元文字列を変更せずに使用することが前提条件。</para>
/// </summary>
/// <param name="source">対象文字列。</param>
/// <returns>テキスト。解放不要。</returns>
TEXT wrap_text(const TCHAR* source);
#define wrap_empty_text() wrap_text(_T(""))

/// <summary>
/// テキストの複製。
/// </summary>
/// <param name="source">入力テキスト。</param>
/// <returns>複製されたテキスト。解放が必要。</returns>
TEXT RC_HEAP_FUNC(clone_text, const TEXT* source, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define clone_text(source, memory_resource) RC_HEAP_WRAP(clone_text, (source), memory_resource)
#endif

/// <summary>
/// テキストを桁数指定で参照として複製。
/// <para><see cref="wrap_text"/>のテキスト入力版みたいな感じ。</para>
/// </summary>
/// <param name="source">入力テキスト。</param>
/// <param name="index">開始位置。</param>
/// <param name="length">長さ。0を指定すれば残りすべて。</param>
/// <returns>参照として複製されたテキスト。参照元が生きている限り生きている。解放不要。番兵使用不可。参照できない場合は無効テキスト。</returns>
TEXT reference_text_width_length(const TEXT* source, size_t index, size_t length);

/// <summary>
/// テキストを参照として複製。
/// </summary>
/// <param name="source">入力テキスト。</param>
/// <returns>参照として複製されたテキスト。参照元が生きている限り生きている。解放不要。</returns>
TEXT reference_text(const TEXT* source);

/// <summary>
/// テキストをの解放。
/// <para>不要な場合は処理しない。</para>
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(release_text, TEXT* text);
#ifdef RES_CHECK
#   define release_text(text) RC_HEAP_WRAP(release_text, (text))
#endif

/// <summary>
/// 書式指定したテキストの生成。
/// </summary>
/// <param name="format"></param>
/// <returns>生成テキスト。解放が必要。</returns>
TEXT RC_HEAP_FUNC(format_text, const MEMORY_RESOURCE* memory_resource, const TEXT* format, ...);
#ifdef RES_CHECK
#   define format_text(memory_resource, format, ...) RC_HEAP_WRAP(format_text, memory_resource, (format), __VA_ARGS__)
#endif

/// <summary>
/// テキストから文字列を生成。
/// </summary>
/// <param name="text">対象文字列。</param>
/// <param name="memory_resource"></param>
/// <returns>文字列。<see cref="release_string"/>による解放が必要。</returns>
TCHAR* RC_HEAP_FUNC(text_to_string, const TEXT* text, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define text_to_string(text, memory_resource) RC_HEAP_WRAP(text_to_string, text, memory_resource)
#endif

/// <summary>
/// 番兵を持つテキストの生成。
/// <para>本関数はメモリ管理データを渡さない非アプリケーション層で呼び出されることを想定しており番兵を持たないことを期待するため、上流で判定すること。</para>
/// </summary>
/// <param name="text">番兵を持たないテキスト。</param>
/// <returns>生成テキスト。解放が必要。</returns>
TEXT get_sentinel_text(const TEXT* text);

// 文字列操作ラッパー
#include "text.z.search.h"
#include "text.z.parse.h"
#include "text.z.conv.h"
#include "text.z.man.h"
