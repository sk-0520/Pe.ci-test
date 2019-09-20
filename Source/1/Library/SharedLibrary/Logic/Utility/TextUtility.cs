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
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    /// <summary>
    /// 文字列処理共通。
    /// </summary>
    public static class TextUtility
    {
        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        /// <param name="dg">nullの場合はデフォルト動作</param>
        /// <returns></returns>
        [Obsolete("old -> ToUnique(string, IEnumerable<string>, StringComparison, Func<string, int, string>)")]
        public static string ToUnique(string target, IEnumerable<string> list, Func<string, int, string> dg)
        {
            return ToUnique(target, list, StringComparison.Ordinal, dg);
        }

        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        /// <param name="comparisonType">比較</param>
        /// <param name="dg">nullの場合はデフォルト動作</param>
        /// <returns></returns>
        public static string ToUnique(string target, IEnumerable<string> list, StringComparison comparisonType, Func<string, int, string> dg)
        {
            Debug.Assert(dg != null);

            var changeName = target;

            int n = 1;
            RETRY:
            foreach(var value in list) {
                if(string.Equals(value, changeName, comparisonType)) {
                    changeName = dg(target, ++n);
                    goto RETRY;
                }
            }

            return changeName;
        }

        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        /// <returns>集合の中に同じものがなければtarget, 存在すればtarget(n)。</returns>
        [Obsolete("old -> ToUniqueDefault(string, IEnumerable<string>, StringComparison)")]
        public static string ToUniqueDefault(string target, IEnumerable<string> list)
        {
            return ToUnique(target, list, (string source, int index) => string.Format("{0}({1})", source, index));
        }

        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        /// <param name="comparisonType"></param>
        /// <returns>集合の中に同じものがなければtarget, 存在すればtarget(n)。</returns>
        public static string ToUniqueDefault(string target, IEnumerable<string> list, StringComparison comparisonType)
        {
            return ToUnique(target, list, comparisonType, (string source, int index) => string.Format("{0}({1})", source, index));
        }
        /// <summary>
        /// 指定文字列集合を<see cref="StringCollection"/>に変換する。
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static StringCollection ToStringCollection(IEnumerable<string> seq)
        {
            var sc = new StringCollection();
            sc.AddRange(seq.ToArray());

            return sc;
        }

        /// <summary>
        /// ホワイトスペースがあれば " で括る。
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static IEnumerable<string> WhitespaceToQuotation(this IEnumerable<string> seq)
        {
            return seq.Select(word => word.SetParentheses(s => s.Any(c => char.IsWhiteSpace(c)), "\"", "\""));
        }

    }
}
