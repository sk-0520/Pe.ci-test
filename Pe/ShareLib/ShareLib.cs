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
using System.Drawing;

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
		
		public static System.Drawing.Color ToNegative(this System.Drawing.Color color)
		{
			return System.Drawing.Color.FromArgb(color.ToArgb() ^ 0xFFFFFF);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static IEnumerable<string> SplitLines(this string s)
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

			return result;
		}
		
	}
	
	/// <summary>
	/// 
	/// </summary>
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

		/// <summary>
		/// ファイルをバイナリとして読み込む
		/// </summary>
		/// <param name="filePath">展開済みファイルパス</param>
		/// <param name="startIndex">読み出し位置</param>
		/// <param name="readLength">読み出しサイズ</param>
		/// <returns></returns>
		public static byte[] FileToBinary(string filePath, int startIndex, int readLength)
		{
			byte[] buffer;

			using (var stream = new BinaryReader(new FileStream(filePath, FileMode.Open, FileAccess.Read))) {
				buffer = new byte[readLength];
				stream.Read(buffer, startIndex, readLength);
			}

			return buffer;
		}
		/// <summary>
		/// ファイルをバイナリとして読み込む
		/// </summary>
		/// <param name="filePath">展開済みファイルパス</param>
		/// <returns></returns>
		public static byte[] FileToBinary(string filePath)
		{
			var fileInfo = new System.IO.FileInfo(filePath);
			return FileToBinary(filePath, 0, (int)fileInfo.Length);
		}

		public static byte[] BinaryListToArray(List<byte[]> list)
		{
			int totalLength = 0;
			foreach (var buffer in list) {
				totalLength += buffer.Length;
			}
			if (totalLength == 0) {
				return null;
			}
			var result = new byte[totalLength];
			int index = 0;
			foreach (var buffer in list) {
				Buffer.BlockCopy(buffer, 0, result, index, buffer.Length);
				index += buffer.Length;
			}
			return result;
		}

		/// <summary>
		/// 0番兵文字列をC#stringへ変換。
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string ToSafeString(string s)
		{
			for (var i = 0; i < s.Length; i++) {
				if (s[i] == '\0') {
					return s.Substring(0, i);
				}
			}

			return s;
		}

		/// <summary>
		/// すらいすー
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourceArray"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static T[] ArraySlice<T>(T[] sourceArray, int start, int end)
		{
			if (start < 0) {
				start = sourceArray.Length + start;
			}
			if (end < 0) {
				end = sourceArray.Length + end;
			}
			Debug.Assert(end - start > 0);
			var result = new T[end - start];
			Buffer.BlockCopy(sourceArray, start, result, 0, result.Length);
			return result;
		}
		public static T[] ArraySlice<T>(T[] sourceArray, int start)
		{
			return ArraySlice(sourceArray, start, sourceArray.Length);
		}

		/// <summary>
		/// ラジアンから度
		/// </summary>
		/// <param name="radian"></param>
		/// <returns></returns>
		public static decimal RadianToDegree(decimal radian)
		{
			return radian * 180.0m / (decimal)Math.PI;
		}
		public static double RadianToDegree(double radian)
		{
			return radian * 180.0 / Math.PI;
		}
		/// <summary>
		/// 度からラジアン
		/// </summary>
		/// <param name="degree"></param>
		/// <returns></returns>
		public static decimal DegreeToRadian(decimal degree)
		{
			return degree * (decimal)Math.PI / 180.0m;
		}
		public static double DegreeToRadian(double degree)
		{
			return degree * Math.PI / 180.0;
		}
		/// <summary>
		/// 2点間の距離
		/// </summary>
		public static double Distance(Point p1, Point p2)
		{
			var x = p1.X - p2.Y;
			var y = p1.Y - p2.Y;
			return Math.Sqrt(x * x + y * y);
		}

	}
	
	/// <summary>
	/// 
	/// </summary>
	public class LList<TCAR, TCDR>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="car"></param>
		/// <param name="cdr"></param>
		public LList(TCAR car, TCDR cdr)
		{
			this.Car = car;
			this.Cdr = cdr;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public TCAR Car { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public TCDR Cdr { get; set; }
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
			get { return this.Car; } 
			set { this.Car = value; }
		}
		public string Info 
		{
			get { return this.Cdr; } 
			set { this.Cdr = value; }
		}
		public object Tag { get; set; }
	}
}
