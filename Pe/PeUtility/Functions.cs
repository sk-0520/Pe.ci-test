/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 15:32
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PeUtility
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
		/// 
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
		/// min &gt;= value &lt;= max
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
		/// 丸め
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
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="datas"></param>
		/// <returns></returns>
		public static bool IsIn<T>(this T value, params T[] datas)
			where T : IComparable
		{
			if (datas == null || datas.Length == 0) {
				throw new ArgumentException(string.Format("null -> {0}, length -> {1}", datas == null, datas.Length));
			}
			return datas.Any(data => value.CompareTo(data) == 0);
		}

		/// <summary>
		/// 集合の処理
		/// </summary>
		/// <param name="seq"></param>
		/// <param name="pred"></param>
		/// <returns>集合自体</returns>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> seq, Action<T> pred)
		{
			foreach(var element in seq) {
				pred(element);
				yield return element;
			}
		}
		/// <summary>
		/// 集合の処理
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
		
		public static IEnumerable<T> Slice<T>(this IEnumerable<T> seq, int fromIndex, int toIndex)
		{
			var takeCount = fromIndex - toIndex - 1;
			return seq.Skip(fromIndex).Take(takeCount);
		}
	}
}
