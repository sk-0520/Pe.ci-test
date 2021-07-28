#pragma once
#include <stdint.h>

#include "res_check.h"

/// <summary>
/// 設定可能な型。
/// </summary>
typedef enum tag_PRIMITIVE_LIST_TYPE
{
    /// <summary>
    /// <c>int8_t</c>を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_INT8,
    /// <summary>
    /// <c>uint8_t</c>を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_UINT8,
    /// <summary>
    /// <c>int16_t</c>を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_INT16,
    /// <summary>
    /// <c>uint16_t</c>を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_UINT16,
    /// <summary>
    /// <c>int32_t</c>を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_INT32,
    /// <summary>
    /// <c>uint32_t</c>を設定。
    /// </summary>
    PRIMITIVE_LIST_TYPE_UINT32,
    /// <summary>
    /// <c>TCHAR</c>を設定。
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
    void* buffer;
    struct
    {
        /// <summary>
        /// 型。
        /// </summary>
        PRIMITIVE_LIST_TYPE type;
        /// <summary>
        /// 確保済みサイズ(バイト幅)。
        /// </summary>
        size_t capacity_bytes;
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
PRIMITIVE_LIST RC_HEAP_FUNC(new_primitive_list, PRIMITIVE_LIST_TYPE list_type, size_t capacity);
#ifdef RES_CHECK
#   define new_primitive_list(list_type, capacity) RC_HEAP_WRAP(new_primitive_list, (list_type), (capacity))
#endif

/// <summary>
/// リストの解放。
/// </summary>
/// <param name="list"></param>
/// <returns></returns>
bool RC_HEAP_FUNC(free_primitive_list, PRIMITIVE_LIST* list);
#ifdef RES_CHECK
#   define free_primitive_list(list) RC_HEAP_WRAP(free_primitive_list, (list))
#endif

/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_int8(PRIMITIVE_LIST_INT8* list, int8_t value);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_uint8(PRIMITIVE_LIST_UINT8* list, uint8_t value);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_int16(PRIMITIVE_LIST_INT16* list, int16_t value);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_uint16(PRIMITIVE_LIST_UINT16* list, uint16_t value);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_int32(PRIMITIVE_LIST_INT32* list, int32_t value);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_uint32(PRIMITIVE_LIST_UINT32* list, uint32_t value);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="value">追加する値。</param>
/// <returns>成功状態。</returns>
bool push_list_tchar(PRIMITIVE_LIST_TCHAR* list, TCHAR value);

/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_int8(PRIMITIVE_LIST_INT8* list, const int8_t* values, size_t count);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_uint8(PRIMITIVE_LIST_UINT8* list, const uint8_t* values, size_t count);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_int16(PRIMITIVE_LIST_INT16* list, const int16_t* values, size_t count);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_uint16(PRIMITIVE_LIST_UINT16* list, const uint16_t* values, size_t count);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_int32(PRIMITIVE_LIST_INT32* list, const int32_t* values, size_t count);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_uint32(PRIMITIVE_LIST_UINT32* list, const uint32_t* values, size_t count);
/// <summary>
/// リストにデータ追加。
/// </summary>
/// <param name="list">対象リスト。</param>
/// <param name="values">追加する値の一覧。</param>
/// <param name="count">個数。</param>
/// <returns>成功状態。</returns>
bool add_range_list_tchar(PRIMITIVE_LIST_TCHAR* list, const TCHAR* values, size_t count);

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
bool pop_list_tchar(TCHAR* result, PRIMITIVE_LIST_TCHAR* list);

/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_int8(int8_t* result, PRIMITIVE_LIST_INT8* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_uint8(uint8_t* result, PRIMITIVE_LIST_UINT8* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_int16(int16_t* result, PRIMITIVE_LIST_INT16* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_uint16(uint16_t* result, PRIMITIVE_LIST_UINT16* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_int32(int32_t* result, PRIMITIVE_LIST_INT32* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_uint32(uint32_t* result, PRIMITIVE_LIST_UINT32* list, size_t index);
/// <summary>
/// リストからデータを取得。
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功状態。</returns>
bool get_list_tchar(TCHAR* result, PRIMITIVE_LIST_TCHAR* list, size_t index);

/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功した場合は型指定されたポインタ。失敗時はNULL。</returns>
const int8_t* reference_list_int8(PRIMITIVE_LIST_INT8* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功した場合は型指定されたポインタ。失敗時はNULL。</returns>
const uint8_t* reference_list_uint8(PRIMITIVE_LIST_UINT8* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功した場合は型指定されたポインタ。失敗時はNULL。</returns>
const int16_t* reference_list_int16(PRIMITIVE_LIST_INT16* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功した場合は型指定されたポインタ。失敗時はNULL。</returns>
const uint16_t* reference_list_uint16(PRIMITIVE_LIST_UINT16* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功した場合は型指定されたポインタ。失敗時はNULL。</returns>
const int32_t* reference_list_int32(PRIMITIVE_LIST_INT32* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功した場合は型指定されたポインタ。失敗時はNULL。</returns>
const uint32_t* reference_list_uint32(PRIMITIVE_LIST_UINT32* list);
/// <summary>
/// リストのデータを参照。
/// <para>参照データ使用時はリストに対する処理を行ってはならない。</para>
/// </summary>
/// <param name="result">取得データ。</param>
/// <param name="list">対象リスト。</param>
/// <param name="index">0基点の取得位置。</param>
/// <returns>成功した場合は型指定されたポインタ。失敗時はNULL。</returns>
const TCHAR* reference_list_tchar(PRIMITIVE_LIST_TCHAR* list);
