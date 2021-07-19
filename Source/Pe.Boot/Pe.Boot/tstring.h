#pragma once
#include <stdbool.h>

#include <windows.h>

#include "memory.h"

/// <summary>
/// 文字列 haystack の先頭から文字列 needle を探し、見つかったときにはその位置をポインタで返却し、見つからなかったときにはNULLを返却。
/// </summary>
/// <param name="haystack">検索対象文字列</param>
/// <param name="needle">検索文字列</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>一致文字のアドレス, 見つからない場合は <c>NULL</c></returns>
TCHAR* findString(const TCHAR* haystack, const TCHAR* needle, bool ignoreCase);
/// <summary>
/// 文字列長を取得。
/// </summary>
/// <param name="s">対象文字列。</param>
/// <returns>長さ。</returns>
size_t getStringLength(const TCHAR* s);

/// <summary>
/// 文字列中の文字を検索。
/// </summary>
/// <param name="haystack">検索対象文字列。</param>
/// <param name="needle">検索文字。</param>
/// <returns>一致文字のアドレス。見つからない場合は <c>NULL</c></returns>
TCHAR* findCharacter(const TCHAR* haystack, TCHAR needle);
/// <summary>
/// 文字列中の文字を検索。
/// </summary>
/// <param name="haystack"></param>
/// <param name="needle"></param>
/// <param name="haystack">検索対象文字列。</param>
/// <param name="needle">検索文字。</param>
/// <returns>一致文字のインデックス。見つからない場合は -1</returns>
SSIZE_T indexCharacter(const TCHAR* haystack, TCHAR needle);

/// <summary>
/// 文字列比較。
/// </summary>
/// <param name="a">比較対象文字列1。</param>
/// <param name="b">比較対象文字列2。</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>a &lt; b: 負, a = b: 0, a &gt; b: 正。</returns>
int compareString(const TCHAR* a, const TCHAR* b, bool ignoreCase);

bool tryParseInteger(int* result, const TCHAR* input);
bool tryParseHexOrInteger(int* result, const TCHAR* input);

bool tryParseLong(long long* result, const TCHAR* input);
bool tryParseHexOrLong(long long* result, const TCHAR* input);

#define formatString(result, format, ...) do { wsprintf(result, format,  __VA_ARGS__); } while(0)

/// <summary>
/// 文字列を結合。
/// </summary>
/// <param name="target">結合対象文字列。</param>
/// <param name="value">追加する文字列。</param>
/// <returns>結合された文字列。</returns>
TCHAR* concatString(TCHAR* target, const TCHAR* value);
/// <summary>
/// 文字列をコピー。
/// </summary>
/// <param name="result">コピー後の文字列の格納先。</param>
/// <param name="value">コピー対象文字列。</param>
/// <returns>コピーされた文字列。</returns>
TCHAR* copyString(TCHAR* result, const TCHAR* value);

/// <summary>
/// 文字列を複製。
/// </summary>
/// <param name="source"></param>
/// <returns>複製された文字列。<c>freeString(freeMemory)</c>にて解放する必要あり。</returns>
TCHAR* cloneString(const TCHAR* source);


/// <summary>
/// 文字列を確保。
/// </summary>
/// <param name="length">文字列の長さ。</param>
/// <returns>先頭 0 の番兵を考慮した領域(length + 1)。freeStringによる解放が必要。</returns>
#ifdef MEM_CHECK
TCHAR* mem_check__allocateString(size_t length, const TCHAR* callerFile, size_t callerLine);
#   define allocateString(length) mem_check__allocateString((length), _T(__FILE__), __LINE__)
#else
TCHAR* allocateString(size_t length);
#endif

/// <summary>
/// 確保した文字列を解放。
/// ドメインとしての関数で<c>freeMemory</c>のラッパー。
/// </summary>
/// <param name="s"></param>
#ifdef MEM_CHECK
void mem_check__freeString(const TCHAR * s, const TCHAR * callerFile, size_t callerLine);
#   define freeString(s) mem_check__freeString((s), _T(__FILE__), __LINE__)
#else
void freeString(const TCHAR * s);
#endif


