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

namespace PeUtility
{
	/// <summary>
	/// 
	/// </summary>
	public static class Text
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <param name="list"></param>
		/// <param name="dg"></param>
		/// <returns></returns>
		public static string ToUnique(this string target, IEnumerable<string> list, Func<string, int, string> dg = null)
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
	}
}
