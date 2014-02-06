/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 22:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace PeMain.Logic
{
	/// <summary>
	/// システムというかなんというか、ね。
	/// </summary>
	public static class SystemEnv
	{
		const string hiddenFileRootPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
		const string extensionRootPath = hiddenFileRootPath;
		
		enum HiddenFileHiddenType 
		{
			Show = 1,
			Hidden = 2,
		}
		enum HiddenFileSuperHiddenType
		{
			Show = 1,
			Hidden = 0,
		}
		
		enum ExtensionHiddenType
		{
			Hidden = 1,
			Show = 0,
		}
		
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public static bool IsHiddenfFileShow()
		{
			var subKey = Registry.CurrentUser.OpenSubKey(hiddenFileRootPath);
			//Debug.WriteLine(subKey.GetValue("ShowSuperHidden"));
			var hiddenValue = (int)subKey.GetValue("Hidden");
			return hiddenValue == (int)HiddenFileHiddenType.Show;
		}
		public static bool IsExtensionShow()
		{
			var subKey = Registry.CurrentUser.OpenSubKey(extensionRootPath);
			var extValue = (int)subKey.GetValue("HideFileExt");
			return extValue == (int)ExtensionHiddenType.Show;
		}
	}
}
