using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// <see langword="enum" />のしょうもない処理ユーティリティ。
    /// </summary>
    /// <remarks>
    /// <para>.NET Framework 時代から考えると結構色々削られた。</para>
    /// </remarks>
    public static class EnumUtility
    {
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
        /// 内部的に <see langword="enum" /> を文字列化する処理。
        /// </summary>
        /// <remarks>
        /// <para>属性とか全部無視して小文字にする。</para>
        /// </remarks>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString<TEnum>(TEnum value)
            where TEnum : struct, Enum
        {
            return value.ToString().ToLowerInvariant();
        }
    }
}
