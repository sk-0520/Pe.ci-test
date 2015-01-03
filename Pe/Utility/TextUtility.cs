/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/11/04
 * 時刻: 1:56
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// 
	/// </summary>
	public static class TextUtility
	{
		/// <summary>
		/// 集合の中から単一の何かを取得する。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="list"></param>
		/// <param name="dg">nullの場合はデフォルト動作</param>
		/// <returns></returns>
		public static string ToUnique(string target, IEnumerable<string> list, Func<string, int, string> dg)
		{
			if(dg == null) {
				dg = (string source, int index) => string.Format("{0}({1})", source, index);
			}
			
			var changeName = target;
			int n = 1;
			RETRY:
			foreach(var value in list) {
				if(value == changeName) {
					changeName = dg(target, ++n);
					goto RETRY;
				}
			}
			
			return changeName;
		}
		public static string ToUniqueDefault(string target, IEnumerable<string> list)
		{
			return ToUnique(target, list, null);
		}
		
		public static string RegexPatternToWildcard(string regPattern) 
		{
			return Regex.Escape(regPattern)
				.Replace(@"\*", ".*")
				.Replace(@"\?", ".")
			;
		}
		
		public static string ReplaceRange(this string src, string head, string tail, Func<string, string> dg)
		{
			var escHead = Regex.Escape(head);
			var escTail = Regex.Escape(tail);
			var pattern = escHead + "(.*?)" + escTail;
			var replacedText = Regex.Replace(src, pattern, (Match m) => dg(m.Groups[1].Value));
			return replacedText;
		}
		
		public static string ReplaceRangeFromDictionary(this string src, string head, string tail, IDictionary<string, string> map)
		{
			return src.ReplaceRange(head, tail, s => map.ContainsKey(s) ? map[s]: head + s + tail);
		}

		public static string WhitespaceToQuotation(this string s)
		{
			if(s.Any(c => char.IsWhiteSpace(c))) {
				return "\"" + s + "\"";
			}

			return s;
		}
		public static IEnumerable<string> WhitespaceToQuotation(this IEnumerable<string> seq)
		{
			foreach(var s in seq) {
				yield return WhitespaceToQuotation(s);
			}
		}
		
		/// <summary>
		/// 文字列を改行で区切る。
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static IEnumerable<string> SplitLines(this string lines)
		{
			using(var stream = new StringReader(lines)) {
				string line = null;
				while ((line = stream.ReadLine()) != null) {
					yield return line;
				}
			}
		}
	}
}
