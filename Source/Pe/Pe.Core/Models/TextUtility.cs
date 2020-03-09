using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class TextUtility
    {
        #region function

        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="seq">集合</param>
        /// <param name="comparisonType">比較処理。</param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static string ToUnique(string target, IReadOnlyCollection<string> seq, StringComparison comparisonType, Func<string, int, string> converter)
        {
            if(target == null) {
                throw new ArgumentNullException(nameof(target));
            }
            if(seq == null) {
                throw new ArgumentNullException(nameof(seq));
            }
            if(converter == null) {
                throw new ArgumentNullException(nameof(converter));
            }

            var changeName = target;

            int n = 1;
            RETRY:
            foreach(var value in seq) {
                if(string.Equals(value, changeName, comparisonType)) {
                    changeName = converter(target, ++n);
                    goto RETRY;
                }
            }

            return changeName;
        }

        /// <summary>
        /// 指定データを集合の中から単一である値に変換する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="seq"></param>
        /// <param name="comparisonType"></param>
        /// <returns>集合の中に同じものがなければtarget, 存在すれば<paramref name="target"/>(n)。</returns>
        public static string ToUniqueDefault(string target, IReadOnlyCollection<string> seq, StringComparison comparisonType)
        {
            return ToUnique(target, seq, comparisonType, (string source, int index) => string.Format("{0}({1})", source, index));
        }

#if false
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
#endif

        /// <summary>
        /// 指定範囲の値を指定処理で置き換える。
        /// </summary>
        /// <param name="src">対象。</param>
        /// <param name="head">置き換え開始文字列。</param>
        /// <param name="tail">置き換え終了文字列。</param>
        /// <param name="dg">処理。</param>
        /// <returns></returns>
        public static string ReplacePlaceholder(string src, string head, string tail, Func<string, string> dg)
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
        public static string ReplacePlaceholderFromDictionary(string src, string head, string tail, IReadOnlyDictionary<string, string> map)
        {
            return ReplacePlaceholder(src, head, tail, s => map.ContainsKey(s) ? map[s] : head + s + tail);
        }
        /// <summary>
        /// ${key}をvalueに置き変える。
        /// </summary>
        /// <param name="src"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static string ReplaceFromDictionary(string src, IReadOnlyDictionary<string, string> map)
        {
            return ReplacePlaceholderFromDictionary(src, "${", "}", map);
        }


        public static IEnumerable<string> ReadLines(string text)
        {
            using(var reader = new StringReader(text)) {
                string? line;
                while((line = reader.ReadLine()) != null) {
                    yield return line;
                }
            }
        }

        public static IEnumerable<string> ReadLines(TextReader reader)
        {
            if(reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            string? line;
            while((line = reader.ReadLine()) != null) {
                yield return line;
            }
        }

        public static int TextWidth(string s)
        {
            if(s == null) {
                return 0;
            }

            var si = new StringInfo(s);
            return si.LengthInTextElements;
        }

        public static IEnumerable<string> GetCharacters(string s)
        {
            var textElements = StringInfo.GetTextElementEnumerator(s);
            while(textElements.MoveNext()) {
                yield return (string)textElements.Current;
            }
        }

        public static string SafeTrim(string? s)
        {
            if(s == null) {
                return string.Empty;
            }

            return s.Trim();
        }

        #endregion
    }
}
