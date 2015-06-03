namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
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
		/// <returns></returns>
		public static string ToSafeName(string name, Func<char, char> fn)
		{
			var pattern = Regex.Escape(string.Concat(Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars())));
			var reg = new Regex("([" + pattern + "])");
			return reg.Replace(name.Trim(), m => new string(fn(m.Groups[0].Value[0]), 1));
		}

	}
}
