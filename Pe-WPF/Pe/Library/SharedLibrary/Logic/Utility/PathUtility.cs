namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

	public static class PathUtility
	{
		/// <summary>
		/// パスに拡張子を追加する。
		/// </summary>
		/// <param name="path"></param>
		/// <param name="ext"></param>
		/// <returns></returns>
		public static string AppendExtension(string path, string ext)
		{
			return path + "." + ext;
		}

		/// <summary>
		/// ファイル名をそれとなく安全な名称に変更する。
		/// </summary>
		/// <param name="name"></param>
		/// <param name="fn"></param>
		/// <returns></returns>
		public static string ToSafeName(string name, Func<char, string> fn)
		{
			Debug.Assert(fn != null);

			var pattern = Regex.Escape(string.Concat(Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars())));
			var reg = new Regex("([" + pattern + "])");
			return reg.Replace(name.Trim(), m => fn(m.Groups[0].Value[0]));
		}
		/// <summary>
		/// ファイル名のシステムで使用できない文字を '_' に置き換える。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string ToSafeNameDefault(string name)
		{
			return ToSafeName(name, c => "_");
		}

		public static bool HasExtension(string path, params string[] extList)
		{
			var dotExt = Path.GetExtension(path);
			if (string.IsNullOrEmpty(dotExt)) {
				return false;
			}

			var ext = dotExt.Substring(1);
			return extList
				.Select(s => s.ToLower())
				.Any(s => s == ext);
		}

		public static bool HasIconPath(string path)
		{
			return HasExtension(path, "exe", "dll");
		}

	}
}
