/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
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
        {
            return Enum.GetValues(enumType).Cast<TEnum>();
        }

        /// <summary>
        /// 型からメンバ一覧を取得。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetMembers<TEnum>()
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
        public static TEnum GetNormalization<TEnum>(object value, TEnum defaultValue)
        {
            if(Enum.IsDefined(typeof(TEnum), value)) {
                return (TEnum)value;
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
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
    }
}
