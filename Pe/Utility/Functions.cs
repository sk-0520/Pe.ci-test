using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// Description of Functions.
	/// </summary>
	public static class Functions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		private static BaseNumberConverter CreateNumberVar<T>()
		{
			var typeInfo = typeof(T);

			if (typeInfo == typeof(Byte)) return new ByteConverter();
			if (typeInfo == typeof(Decimal)) return new DecimalConverter();
			if (typeInfo == typeof(Double)) return new DoubleConverter();
			if (typeInfo == typeof(Int16)) return new Int16Converter();
			if (typeInfo == typeof(Int32)) return new Int32Converter();
			if (typeInfo == typeof(Int64)) return new Int64Converter();
			if (typeInfo == typeof(SByte)) return new SByteConverter();
			if (typeInfo == typeof(Single)) return new SingleConverter();
			if (typeInfo == typeof(UInt16)) return new UInt32Converter();
			if (typeInfo == typeof(UInt32)) return new UInt32Converter();
			if (typeInfo == typeof(UInt64)) return new UInt32Converter();

			return null;
		}
		/// <summary>
		/// 数値へ変換。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static T ToNumber<T>(this string source)
		{
			try {
				var converter = CreateNumberVar<T>();
				if (converter != null && converter.CanConvertFrom(typeof(String))) {
					return (T)converter.ConvertFromString(source);
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex);
			}

			return default(T);
		}

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
		/// 集合の処理。
		/// </summary>
		/// <param name="seq"></param>
		/// <param name="pred"></param>
		/// <returns>処理したデータ</returns>
		public static IEnumerable<R> Map<T, R>(this IEnumerable<T> seq, Func<T, R> pred)
		{
			foreach(var element in seq) {
				yield return pred(element);
			}
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
			const string format = "{0:000}{1:000}{2:000}";
			var sa = string.Format(format, a.Item1, a.Item2, a.Item3);
			var sb = string.Format(format, b.Item1, b.Item2, b.Item3);
			var na = int.Parse(sa);
			var nb = int.Parse(sb);
			//Debug.WriteLine("{0}:{1}",sa, sb);
			return na - nb;
		}
		
		public static void ToDispose(this IDisposable target)
		{
			if(target != null) {
				target.Dispose();
			}
		}
	}
}
