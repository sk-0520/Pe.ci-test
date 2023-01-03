#pragma once
#include <stdbool.h>
#include <stdint.h>

#include <tchar.h>

#include "res_check.h"

/*
* 自前で生成した<c>MEMORY_ARENA_RESOURCE</c>を指定する場合は<c>RES_CHECK</c>検知対象外となる。
* <c>MEMORY_ARENA_RESOURCE</c>を指定しない場合はライブラリ側で取得した<c>MEMORY_ARENA_RESOURCE</c>を用い、<c>RES_CHECK</c>が有効な場合には検知対象になる。
*/

typedef byte_t(*func_calc_extend_capacity)(byte_t input_bytes);

/// <summary>
/// メモリアリーナ管理データ。
/// <para>アプリケーション内では使用しない。</para>
/// </summary>
typedef struct tag_MEMORY_ARENA_RESOURCE
{
    HANDLE handle;
    byte_t maximum_size;
} MEMORY_ARENA_RESOURCE;

/// <summary>
/// メモリアリーナ管理: 自動初期サイズ。
/// </summary>
#define MEMORY_ARENA_AUTO_INITIAL_SIZE (0)
/// <summary>
/// メモリアリーナ管理: 自動拡張最大サイズ。
/// </summary>
#define MEMORY_ARENA_EXTENDABLE_MAXIMUM_SIZE (0)

/// <summary>
/// 可能であればスタックに確保、無理ならヒープに配置。
/// <para><c>release_array_or_memory</c>で解放(必須)。</para>
/// </summary>
#define new_stack_or_heap_array(var_name, cpp_name, type, count, stack_count, memory_arena_resource) \
struct { \
    type* elements; \
    struct { \
        type buffer[stack_count]; \
        const MEMORY_ARENA_RESOURCE* mr; \
    } library; \
} cpp_name; \
set_memory(&cpp_name, 0, sizeof(cpp_name));  \
do { \
    cpp_name.library.mr = NULL; \
    size_t cpp_name ## _stack_count = stack_count * sizeof(type); \
    size_t cpp_name ## _heap_count = count * sizeof(type); \
    if(cpp_name ## _stack_count < cpp_name ## _heap_count) { \
        cpp_name.elements = new_memory(cpp_name ## _heap_count, sizeof(type), cpp_name.library.mr = memory_arena_resource); \
    } else { \
        cpp_name.elements = cpp_name.library.buffer; \
    } \
} while(0); \
type* var_name = cpp_name.elements; \

#define is_stack_array(array_or_memory_item) (array_or_memory_item.elements == array_or_memory_item.library.buffer)

#define release_stack_or_heap_array(array_or_memory_item) \
if(!is_stack_array(array_or_memory_item)) { \
    release_memory(array_or_memory_item.elements, array_or_memory_item.library.mr); \
}

/// <summary>
/// デフォルトのメモリアリーナリソースを取得。
/// <para>呼び出し側でメモリ処理しないようなインターフェイスの場合、Pe.Libraryで明示的に使用することはない。</para>
/// </summary>
/// <returns></returns>
MEMORY_ARENA_RESOURCE* get_default_memory_arena_resource();
/// <summary>
/// デフォルトメモリアリーナリソースの簡易呼び出し。
/// <para>Pe.Libraryで明示的に使用することはない。</para>
/// </summary>
#define DEFAULT_MEMORY_ARENA get_default_memory_arena_resource()

/// <summary>
/// メモリアリーナリソースの生成。
/// </summary>
/// <param name="initial_size">初期サイズ。</param>
/// <param name="maximum_size">最大サイズ。</param>
/// <returns>生成されたメモリ管理データ。解放が必要</returns>
MEMORY_ARENA_RESOURCE new_memory_arena_resource(byte_t initial_size, byte_t maximum_size);

/// <summary>
/// メモリアリーナリソースの解放。
/// </summary>
/// <param name="memory_arena_resource"></param>
/// <returns></returns>
bool release_memory_arena_resource(MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// メモリ管理データが有効か。
/// </summary>
/// <param name="memory_arena_resource"></param>
/// <returns></returns>
bool is_enabled_memory_resource(const MEMORY_ARENA_RESOURCE* memory_arena_resource);

/// <summary>
/// 指定したサイズ以上のヒープ領域を確保。
/// <para>アプリケーション側では原則使用しない。</para>
/// </summary>
/// <param name="bytes">確保サイズ</param>
/// <returns>確保した領域。<see cref="release_memory"/>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
void* RC_HEAP_FUNC(allocate_raw_memory, byte_t bytes, bool zero_fill, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define allocate_raw_memory(bytes, zero_fill, memory_arena_resource) RC_HEAP_WRAP(allocate_raw_memory, bytes, zero_fill, memory_arena_resource)
#endif

/// <summary>
/// 指定したサイズ以上のヒープ領域を0クリアで確保。
/// <para>ライブラリ層以外では原則こちらを使用する。</para>
/// <para>TODO: オーバーフローの確認。</para>
/// </summary>
/// <param name="count">確保する個数。</param>
/// <param name="type_size">型サイズ。</param>
/// <returns>確保した領域。<see cref="release_memory"/>にて開放が必要。失敗時は<c>NULL</c>を返す。</returns>
void* RC_HEAP_FUNC(new_memory, size_t count, byte_t type_size, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define new_memory(count, type_size, memory_arena_resource) RC_HEAP_WRAP(new_memory, count, type_size, memory_arena_resource)
#endif

/// <summary>
/// 確保した領域を解放。
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(release_memory, void* p, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
#if RES_CHECK
#   define release_memory(p, memory_arena_resource) RC_HEAP_WRAP(release_memory, p, memory_arena_resource)
#endif



/// <summary>
/// <see cref="memset" />する。
/// </summary>
/// <param name="target">対象領域。</param>
/// <param name="value">値。</param>
/// <param name="bytes">範囲。</param>
/// <returns>target</returns>
void* set_memory(void* target, uint8_t value, byte_t bytes);

/// <summary>
/// <see cref="memcpy" />する。
/// </summary>
/// <param name="destination">コピー先。</param>
/// <param name="source">コピー元。</param>
/// <param name="bytes">コピーサイズ。</param>
/// <returns></returns>
void* copy_memory(void* destination, const void* source, byte_t bytes);

/// <summary>
/// <see cref="memmove" />する。
/// </summary>
/// <param name="destination">移動先。</param>
/// <param name="source">移動元。</param>
/// <param name="bytes">移動サイズ。</param>
/// <returns></returns>
void* move_memory(void* destination, const void* source, byte_t bytes);

/// <summary>
/// <see cref="memcmp" />する。
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <param name="bytes"></param>
/// <returns>a &lt; b: 負, a = b: 0, a &gt; b: 正。</returns>
int compare_memory(const void* a, const void* b, byte_t bytes);

/// <summary>
/// メモリリソースから予約領域を持つバッファを拡張する基底処理。
/// <para>ライブラリ側で使用する前提処理。アプリケーション側からは使用しない。</para>
/// <para><c>RES_CHECK</c>検知対象外。</para>
/// </summary>
/// <param name="buffer">対象領域のポインタ。</param>
/// <param name="current_bytes">現在のバイト数。</param>
/// <param name="current_capacity_bytes">現在の予約バイト数。</param>
/// <param name="need_bytes">必要なバイト数。</param>
/// <param name="default_capacity_bytes">予約領域の標準値。</param>
/// <param name="calc_extend_capacity">予約領域拡張方法。</param>
/// <returns>拡張後の総バイト数。未実施の場合は0を返す。</returns>
byte_t library__extend_capacity_if_not_enough_bytes(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, func_calc_extend_capacity calc_extend_capacity, const MEMORY_ARENA_RESOURCE* memory_arena_resource);

byte_t library__extend_capacity_if_not_enough_bytes_x2(void** target, byte_t current_bytes, byte_t current_capacity_bytes, byte_t need_bytes, byte_t default_capacity_bytes, const MEMORY_ARENA_RESOURCE* memory_arena_resource);
