namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.IO;
	using System.Linq;

	/// <summary>
	/// パス関連共通処理。
	/// </summary>
	public static class PathUtility
	{
		static bool IsTargetExt(string path, Func<string, bool> dg)
		{
			var dotExt = Path.GetExtension(path);
			if(dotExt.Length >= ".x".Length) {
				var ext = dotExt.Substring(1).ToLower();
				return dg(ext);
			}
			return false;
		}
		
		/// <summary>
		/// パスは実行形式として扱われるか。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool IsExecutePath(string path)
		{
			return IsTargetExt(path.ToLower(), s => s.IsIn("exe", "com", "bat"));
		}
		
		/// <summary>
		/// アイコンを保持するパスか。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool HasIconPath(string path)
		{
			return IsTargetExt(path.ToLower(), s => s.IsIn("exe", "dll"));
		}
		
		/// <summary>
		/// パスはショートカット形式として扱われるか
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool IsShortcutPath(string path)
		{
			return IsTargetExt(path.ToLower(), s => s.IsIn("lnk", "url"));
		}

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
		public static string ToSafeName(string name)
		{
			var result = name.Trim();
			foreach(var c in Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars())) {
				result = result.Replace(c, '_');
			}
			return result;
		}
	}
}
