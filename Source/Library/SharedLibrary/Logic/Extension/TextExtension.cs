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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
    public static class TextExtension
    {
        /// <summary>
        /// 指定範囲の値を指定処理で置き換える。
        /// </summary>
        /// <param name="src">対象。</param>
        /// <param name="head">置き換え開始文字列。</param>
        /// <param name="tail">置き換え終了文字列。</param>
        /// <param name="dg">処理。</param>
        /// <returns></returns>
        public static string ReplaceRange(this string src, string head, string tail, Func<string, string> dg)
        {
            var escHead = Regex.Escape(head);
            var escTail = Regex.Escape(tail);
            var pattern = escHead + "(.+?)" + escTail;
            var replacedText = Regex.Replace(src, pattern, (Match m) => dg(m.Groups[1].Value));
            return replacedText;
        }

        /// <summary>
        /// 指定範囲の値を指定のコレクションで置き換える。
        /// </summary>
        /// <param name="src">対象。</param>
        /// <param name="head">置き換え開始文字列。</param>
        /// <param name="tail">置き換え終了文字列。</param>
        /// <param name="map">置き換え対象文字列と置き換え後文字列のペアであるコレクション。</param>
        /// <returns></returns>
        public static string ReplaceRangeFromDictionary(this string src, string head, string tail, IDictionary<string, string> map)
        {
            return src.ReplaceRange(head, tail, s => map.ContainsKey(s) ? map[s] : head + s + tail);
        }

        /// <summary>
        /// 文字列を連想配列のキーから値に変換する。
        /// </summary>
        /// <param name="src"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static string ReplaceFromDictionary(this string src, IDictionary<string, string> map)
        {
            var pattern = string.Format("(?<HIT>{0})", string.Join("|", map.Keys.Select(s => Regex.Escape(s)).Select(s => string.Format("({0})", s))));
            var reg = new Regex(pattern);
            return reg.Replace(src, (Match m) => {
                var key = m.Groups["HIT"].Value;
                return map[key];
            });
        }

        /// <summary>
        /// 文字列を改行で区切る。
        /// </summary>
        /// <param name="lines">何らかの文字列</param>
        /// <returns>改行を含めない各行。</returns>
        public static IEnumerable<string> SplitLines(this string lines)
        {
            if(lines != null) {
                using(var stream = new StringReader(lines)) {
                    string line = null;
                    while((line = stream.ReadLine()) != null) {
                        yield return line;
                    }
                }
            }
        }

        /// <summary>
        /// 文字列が条件に該当すればくくる。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="func"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static string SetParentheses(this string s, Func<string, bool> func, string left, string right)
        {
            if(func(s)) {
                return left + s + right;
            } else {
                return s;
            }
        }
    }
}
