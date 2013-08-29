/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/06/25
 * 時刻: 23:07
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ShareLib
{
	public delegate string GetUniqueDg(string source, int index);
	
	public static class Extension
	{
		/// <summary>
		/// 文字列の有効性チェック
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsEmpty(this string s)
		{
			return s == null || s.Length == 0;
		}
		
		public static string[] SplitLines(this string s)
		{
			if (s.IsEmpty()) {
				return null;
			}
			
			var result = new List<string>();
			using (var stream = new StringReader(s)) {
				string line;
				while ((line = stream.ReadLine()) != null) {
					result.Add(line);
				}
			}

			return result.ToArray();
		}
		
	}
	
	public static class Common
	{
		/// <summary>
		/// パスから親までのフォルダを作成
		/// </summary>
		/// <param name="expandEnvPath">環境変数展開済みパス。</param>
		/// <remarks>親フォルダ名</remarks>
		public static string MakeParentFolder(string expandEnvPath)
		{
			// 親フォルダの取得と作成
			var parentFolder = Path.GetDirectoryName(expandEnvPath);
			if (!Directory.Exists(parentFolder)) {
				Directory.CreateDirectory(parentFolder);
			}
			return parentFolder;
		}
		public static string MyPath
		{
			get { return System.Reflection.Assembly.GetExecutingAssembly().Location; }
		}
		public static string MyFolder
		{
			get { return Path.GetDirectoryName(MyPath); }
		}

		public static string StartUpPath
		{
			get { return Environment.GetFolderPath(Environment.SpecialFolder.Startup); }
		}
		
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
		public static T ToNumber<T>(string source)
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
		
		public static string GetUnique(string changeName, IEnumerable<string> pool, GetUniqueDg dg = null)
		{
			if(dg == null) {
				dg = delegate(string source, int index)
				{
					return string.Format("{0}({1})", source, index);
				};
			}
			
			var tempName = changeName;
			var n = 1;
			
			var array = pool.ToArray();
		
			RETRY: for(var i = 0; i < array.Length; i++) {
				if(array[i] == tempName) {
					tempName = dg(changeName, ++n);
					goto RETRY;
				}
			}
			
			return tempName;
		}
		
		public static string FileSizeToString(long size)
		{
			const int bufferLength = 32;
			var buffer = new StringBuilder(bufferLength);
			WindowsAPI.StrFormatByteSize(size, buffer, bufferLength);
			return buffer.ToString();
		}
		
		/// <summary>
		/// min &gt;= value &lt;= max
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static bool Between<T>(T value, T min, T max)
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
		public static T Rounding<T>(T value, T min, T max)
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
		public static bool IsIn<T>(T value, params T[] datas)
			where T : IComparable
		{
			if (datas == null || datas.Length == 0) {
				throw new ArgumentException(string.Format("null -> {0}, length -> {1}", datas == null, datas.Length));
			}
			foreach (var data in datas) {
				if (value.CompareTo(data) == 0) {
					return true;
				}
			}
			return false;
		}
	}
	
	public class LList<TCAR, TCDR>
	{
		public TCAR car;
		public TCDR cdr;
		
		public LList(TCAR car, TCDR cdr)
		{
			this.car = car;
			this.cdr = cdr;
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class TestAndInfo: LList<bool, string>
	{
		public static TestAndInfo Success(object tag = null)
		{
			var result = new TestAndInfo();
			
			result.Test = true;
			result.Tag = tag;
			
			return result;
		}
		public static TestAndInfo Failure(string info, object tag = null)
		{
			var result = new TestAndInfo();
			
			result.Test = false;
			result.Info = info;
			result.Tag = tag;
			
			return result;
		}
		
		public TestAndInfo(): base(default(bool), default(string)) { }
		
		public bool Test 
		{
			get { return this.car; } 
			set { this.car = value; }
		}
		public string Info 
		{
			get { return this.cdr; } 
			set { this.cdr = value; }
		}
		public object Tag { get; set; }
	}
}
