#pragma once
/* 自動生成: primitive_list.h.tt */
#include <stdint.h>

#include "common.h"
#include "memory.h"
#include "res_check.h"

/// <summary>
/// デフォルトキャパ。
/// </summary>
#define PRIMITIVE_LIST_DEFAULT_CAPACITY (32)

/// <summary>
/// 設定可能な型。
/// </summary>
typedef enum tag_PRIMITIVE_LIST_TYPE
{
    /// <summary>
    /// <see cref="int8_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_INT8,
    /// <summary>
    /// <see cref="uint8_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_UINT8,
    /// <summary>
    /// <see cref="int16_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_INT16,
    /// <summary>
    /// <see cref="uint16_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_UINT16,
    /// <summary>
    /// <see cref="int32_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_INT32,
    /// <summary>
    /// <see cref="uint32_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_UINT32,
    /// <summary>
    /// <see cref="size_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_SIZE,
    /// <summary>
    /// <see cref="ssize_t" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_SSIZE,
    /// <summary>
    /// <see cref="TCHAR" />を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_TCHAR,
} PRIMITIVE_LIST_TYPE;

/// <summary>
/// 組み込み型(≠構造体)の動的リスト。
/// </summary>
typedef struct tag_PRIMITIVE_LIST
{
    /// <summary>
    /// 型に合わせた長さ。
    /// </summary>
    size_t length;
    /// <summary>
    /// 実体。
    /// </summary>
    void* items;
    struct
    {
        /// <summary>
        /// メモリリソース。
        /// </summary>
        const MEMORY_RESOURCE* memory_resource;
        /// <summary>
        /// 型。
        /// </summary>
        PRIMITIVE_LIST_TYPE type;
        /// <summary>
        /// 確保済みサイズ(バイト幅)。
        /// </summary>
        byte_t capacity_bytes;
    } library;

} PRIMITIVE_LIST;

/// <summary>
/// <c>int8_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_INT8;
/// <summary>
/// <c>uint8_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_UINT8;
/// <summary>
/// <c>int16_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_INT16;
/// <summary>
/// <c>uint16_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_UINT16;
/// <summary>
/// <c>int32_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_INT32;
/// <summary>
/// <c>uint32_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_UINT32;
/// <summary>
/// <c>size_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_SIZE;
/// <summary>
/// <c>ssize_t</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_SSIZE;
/// <summary>
/// <c>TCHAR</c>を扱う<c>PRIMITIVE_LIST</c>の別名。
/// <para>弱い型なのであくまでソース上の見た目判断用。</para>
/// </summary>
typedef PRIMITIVE_LIST PRIMITIVE_LIST_TCHAR;

/// <summary>
/// リストの生成。
/// </summary>
/// <param name="list_type">リストで使用する型。</param>
/// <param name="capacity">予約サイズ。list_typeに影響されない理論的なサイズ。</param>
/// <returns>解放が必要。</returns>
PRIMITIVE_LIST RC_HEAP_FUNC(new_primitive_list, PRIMITIVE_LIST_TYPE list_type, size_t capacity, const MEMORY_RESOURCE* memory_resource);
#ifdef RES_CHECK
#   define new_primitive_list(list_type, capacity, memory_resource) RC_HEAP_WRAP(new_primitive_list, (list_type), (capacity), memory_resource)
#endif

/// <summary>
/// リストの解放。
/// </summary>
/// <param name="list"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(release_primitive_list, PRIMITIVE_LIST* list);
#ifdef RES_CHECK
#   define release_primitive_list(list) RC_HEAP_WRAP(release_primitive_list, (list))
#endif

/// <summary>
/// リストに<see cref="int8_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_int8(PRIMITIVE_LIST_INT8* list, int8_t value);
/// <summary>
/// リストに<see cref="uint8_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_uint8(PRIMITIVE_LIST_UINT8* list, uint8_t value);
/// <summary>
/// リストに<see cref="int16_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_int16(PRIMITIVE_LIST_INT16* list, int16_t value);
/// <summary>
/// リストに<see cref="uint16_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_uint16(PRIMITIVE_LIST_UINT16* list, uint16_t value);
/// <summary>
/// リストに<see cref="int32_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_int32(PRIMITIVE_LIST_INT32* list, int32_t value);
/// <summary>
/// リストに<see cref="uint32_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_uint32(PRIMITIVE_LIST_UINT32* list, uint32_t value);
/// <summary>
/// リストに<see cref="size_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_size(PRIMITIVE_LIST_SIZE* list, size_t value);
/// <summary>
/// リストに<see cref="ssize_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_ssize(PRIMITIVE_LIST_SSIZE* list, ssize_t value);
/// <summary>
/// リストに<see cref="TCHAR"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_tchar(PRIMITIVE_LIST_TCHAR* list, TCHAR value);

/// <summary>
/// リストに<see cref="int8_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_int8(PRIMITIVE_LIST_INT8* list, const int8_t values[], size_t count);
/// <summary>
/// リストに<see cref="uint8_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_uint8(PRIMITIVE_LIST_UINT8* list, const uint8_t values[], size_t count);
/// <summary>
/// リストに<see cref="int16_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_int16(PRIMITIVE_LIST_INT16* list, const int16_t values[], size_t count);
/// <summary>
/// リストに<see cref="uint16_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_uint16(PRIMITIVE_LIST_UINT16* list, const uint16_t values[], size_t count);
/// <summary>
/// リストに<see cref="int32_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_int32(PRIMITIVE_LIST_INT32* list, const int32_t values[], size_t count);
/// <summary>
/// リストに<see cref="uint32_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_uint32(PRIMITIVE_LIST_UINT32* list, const uint32_t values[], size_t count);
/// <summary>
/// リストに<see cref="size_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_size(PRIMITIVE_LIST_SIZE* list, const size_t values[], size_t count);
/// <summary>
/// リストに<see cref="ssize_t"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_ssize(PRIMITIVE_LIST_SSIZE* list, const ssize_t values[], size_t count);
/// <summary>
/// リストに<see cref="TCHAR"/>データ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_tchar(PRIMITIVE_LIST_TCHAR* list, const TCHAR values[], size_t count);

/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_int8(int8_t* result, PRIMITIVE_LIST_INT8* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_uint8(uint8_t* result, PRIMITIVE_LIST_UINT8* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_int16(int16_t* result, PRIMITIVE_LIST_INT16* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_uint16(uint16_t* result, PRIMITIVE_LIST_UINT16* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_int32(int32_t* result, PRIMITIVE_LIST_INT32* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_uint32(uint32_t* result, PRIMITIVE_LIST_UINT32* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_size(size_t* result, PRIMITIVE_LIST_SIZE* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_ssize(ssize_t* result, PRIMITIVE_LIST_SSIZE* list);
/// <summary>
/// リストの末尾データを破棄。
/// </summary>
/// <param name="result">末尾データ。<c>NULL</c>指定で無視。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功状態。</returns>
bool pop_list_tchar(TCHAR* result, PRIMITIVE_LIST_TCHAR* list);

/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_int8(int8_t* result, const PRIMITIVE_LIST_INT8* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_uint8(uint8_t* result, const PRIMITIVE_LIST_UINT8* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_int16(int16_t* result, const PRIMITIVE_LIST_INT16* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_uint16(uint16_t* result, const PRIMITIVE_LIST_UINT16* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_int32(int32_t* result, const PRIMITIVE_LIST_INT32* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_uint32(uint32_t* result, const PRIMITIVE_LIST_UINT32* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_size(size_t* result, const PRIMITIVE_LIST_SIZE* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_ssize(ssize_t* result, const PRIMITIVE_LIST_SSIZE* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_tchar(TCHAR* result, const PRIMITIVE_LIST_TCHAR* list, size_t index);

/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
int8_t* reference_list_int8(const PRIMITIVE_LIST_INT8* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
uint8_t* reference_list_uint8(const PRIMITIVE_LIST_UINT8* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
int16_t* reference_list_int16(const PRIMITIVE_LIST_INT16* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
uint16_t* reference_list_uint16(const PRIMITIVE_LIST_UINT16* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
int32_t* reference_list_int32(const PRIMITIVE_LIST_INT32* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
uint32_t* reference_list_uint32(const PRIMITIVE_LIST_UINT32* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
size_t* reference_list_size(const PRIMITIVE_LIST_SIZE* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
ssize_t* reference_list_ssize(const PRIMITIVE_LIST_SSIZE* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <returns>成功した場合は型指定されたポインタ。中身いじってもいいけどご安全に。失敗時はNULL。</returns>
TCHAR* reference_list_tchar(const PRIMITIVE_LIST_TCHAR* list);

/// <summary>
/// リストを空にする。
/// <para>領域自体はそのまま残る点に注意。</para>
/// </summary>
/// <param name="list">対象リスト。</param>
void clear_primitive_list(PRIMITIVE_LIST* list);
