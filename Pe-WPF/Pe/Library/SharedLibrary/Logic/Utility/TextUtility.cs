namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

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
		public static string ToUnique(string target, IEnumerable<string> list, Func<string, int, string> dg)
		{
			Debug.Assert(dg != null);

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

		/// <summary>
		/// 指定データを集合の中から単一である値に変換する。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="list"></param>
		/// <returns>集合の中に同じものがなければtarget, 存在すればtarget(n)。</returns>
		public static string ToUniqueDefault(string target, IEnumerable<string> list)
		{
			return ToUnique(target, list, (string source, int index) => string.Format("{0}({1})", source, index));
		}

		public static StringCollection ToStringCollection(IEnumerable<string> seq)
		{
			var sc = new StringCollection();
			sc.AddRange(seq.ToArray());

			return sc;
		}
	}
}
