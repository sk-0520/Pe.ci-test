/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/21
 * 時刻: 22:01
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;

namespace PeUtility
{
	/// <summary>
	/// Description of PathUtility.
	/// </summary>
	public static class PathUtility
	{

		static bool IsTargetExt(string path, Func<string, bool> dg)
		{
			var dotExt = Path.GetExtension(path);
			if(dotExt.Length > ".x".Length) {
				var ext = dotExt.Substring(1).ToLower();
				return dg(ext);
			}
			return false;
		}
		
		/// <summary>
		/// パスは実行形式として扱われるか
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool IsExecutePath(string path)
		{
			return IsTargetExt(path.ToLower(), s => s.IsIn("exe", "com", "bat"));
		}
		
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
	}
}
