using System;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// enumのしょうもない処理ユーティリティ。
    /// </summary>
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
    }
}
