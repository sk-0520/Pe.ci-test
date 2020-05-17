using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// enumのしょうもない処理ユーティリティ。
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// メンバ一覧を取得。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetMembers<TEnum>(Type enumType)
            where TEnum : struct, Enum
        {
            if(enumType != typeof(TEnum)) {
                throw new ArgumentException($"{enumType} != {typeof(TEnum)}");
            }
            return Enum.GetValues(enumType).Cast<TEnum>();
        }

        /// <summary>
        /// 型からメンバ一覧を取得。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetMembers<TEnum>()
            where TEnum : struct, Enum
        {
            return GetMembers<TEnum>(typeof(TEnum));
        }

        /// <summary>
        /// 指定値を正規化された enum の値に変換する。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns>指定値がEnumに存在すれば指定値、存在しなければ<paramref name="defaultValue"/>を返す。</returns>
        public static TEnum Normalize<TEnum>(string value, TEnum defaultValue)
            where TEnum : struct, Enum
        {
            if(Enum.IsDefined(typeof(TEnum), value)) {
                return (TEnum)Enum.Parse(typeof(TEnum), value);
            } else {
                return defaultValue;
            }
        }

        /// <summary>
        ///列挙体メンバのパース。
        ///<para><see cref="Enum.Parse(Type, string)"/>のキャスト周りを簡略化した版。</para>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">変換する名前または値が含まれている文字列。</param>
        /// <param name="ignoreCase">大文字と小文字を区別しない場合は true。大文字と小文字を区別する場合は false。</param>
        /// <returns></returns>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase = true)
            where TEnum : struct, Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        public static bool TryParse<TEnum>(string value, out TEnum result, bool ignoreCase = true)
            where TEnum : struct, Enum
        {
            if(Enum.TryParse(typeof(TEnum), value, ignoreCase, out var temp)) {
                result = (TEnum)temp!;
                return true;
            }
            result = default;
            return false;
        }

    }
}
