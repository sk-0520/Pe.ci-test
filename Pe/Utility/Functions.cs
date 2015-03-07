namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;

	/// <summary>
	/// 共通処理。
	/// </summary>
	public static class Functions
	{
		/// <summary>
		/// min &lt;= value &lt;= max
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static bool Between<T>(this T value, T min, T max)
			where T : IComparable
		{
			return min.CompareTo(value) <= 0 && 0 <= max.CompareTo(value);
		}

		/// <summary>
		/// 丸め。
		/// 
		/// valueがmin未満かmaxより多ければminかmaxの適応する方に丸める。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static T Rounding<T>(this T value, T min, T max)
			where T : IComparable
		{
			if (min.CompareTo(value) > 0) {
				return min;
			} else if (max.CompareTo(value) < 0) {
				return max;
			} else {
				return value;
			}
		}
		
		/// <summary>
		/// データの存在。
		/// 
		/// Anyでいいなこれ。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="datas"></param>
		/// <returns></returns>
		//[Obsolete]
		public static bool IsIn<T>(this T value, params T[] datas)
			where T : IComparable
		{
			if (datas == null || datas.Length == 0) {
				throw new ArgumentException(string.Format("null -> {0}, length -> {1}", datas == null, datas.Length));
			}
			return datas.Any(data => value.CompareTo(data) == 0);
		}
		
		/// <summary>
		/// スライス。
		/// </summary>
		/// <param name="seq"></param>
		/// <param name="fromIndex"></param>
		/// <param name="toIndex"></param>
		/// <returns></returns>
		public static IEnumerable<T> Slice<T>(this IList<T> seq, int fromIndex, int toIndex)
		{
			var takeCount = fromIndex - toIndex - 1;
			return seq.Skip(fromIndex).Take(takeCount);
		}
		
		/// <summary>
		/// バージョン文字列をタプルに変換。
		/// 
		/// *.*.* までが対象となる。
		/// 
		/// <para>Versionでいいじゃん(いいじゃん)</para>
		/// </summary>
		/// <param name="versionText"></param>
		/// <returns></returns>
		public static Tuple<ushort, ushort, ushort> ConvertVersionTuple(string versionText)
		{
			var v = versionText
				.Split('.')
				.Take(3)
				.Select(n => ushort.Parse(n))
				.ToArray()
			;
			if(v.Length < 3) {
				throw new ArgumentException(string.Format("src = {0}, split = {1}", versionText, v));
			}
			return new Tuple<ushort, ushort, ushort>(v[0], v[1], v[2]);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>
		/// +: a &gt; b
		/// 0: a = b
		/// -: a &lt; b
		/// </returns>
		public static int VersionCheck(Tuple<ushort,ushort,ushort> a, Tuple<ushort,ushort,ushort> b)
		{
			const string format = "{0:D2}{1:D3}{2:D5}";
			var sa = string.Format(format, a.Item1, a.Item2, a.Item3);
			var sb = string.Format(format, b.Item1, b.Item2, b.Item3);
			var na = int.Parse(sa);
			var nb = int.Parse(sb);
			//Debug.WriteLine("{0}:{1}",sa, sb);
			return na - nb;
		}
		
		/// <summary>
		/// IDisposableオブジェクトをnullでもDisposeしてみる。
		/// </summary>
		/// <param name="target"></param>
		public static void ToDispose(this IDisposable target)
		{
			if(target != null) {
				target.Dispose();
			}
		}
	}
}
