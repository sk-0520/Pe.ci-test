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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using PInvoke.Windows;

namespace PeMain.Logic
{
	/// <summary>
	/// システムというかなんというか、ね。
	/// </summary>
	public static class SystemEnv
	{
		const string hiddenFileRootPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
		const string extensionRootPath = hiddenFileRootPath;
		const string hiddenKey = "Hidden";
		const string extensionKey = "HideFileExt";
		
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
		public static bool IsHiddenFileShow()
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(hiddenFileRootPath)) {
				//Debug.WriteLine(subKey.GetValue("ShowSuperHidden"));
				var hiddenValue = (int)subKey.GetValue(hiddenKey);
				return hiddenValue == (int)HiddenFileHiddenType.Show;
			}
		}
		public static void SetHiddenFileShow(bool show)
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(hiddenFileRootPath, true)) {
				var hiddenValue = (int)(show ? HiddenFileHiddenType.Show: HiddenFileHiddenType.Hidden);
				subKey.SetValue(hiddenKey, hiddenValue, RegistryValueKind.DWord);
			}
		}
		
		public static bool IsExtensionShow()
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(extensionRootPath)) {
				var extValue = (int)subKey.GetValue(extensionKey);
				return extValue == (int)ExtensionHiddenType.Show;
			}
		}
		public static void SetExtensionShow(bool show)
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(extensionRootPath, true)) {
				var extType = (int)(show ? ExtensionHiddenType.Show: ExtensionHiddenType.Hidden);
				subKey.SetValue(extensionKey, extType, RegistryValueKind.DWord);
			}
		}
		
		/// <summary>
		/// TODO: 外部化
		/// </summary>
		/// <param name="hParentWnd"></param>
		private static void RefreshShell(IntPtr hParentWnd)
		{
			var targetClassName = "SHELLDLL_DefView";
			var hWnd = IntPtr.Zero;
			var workClassName = new StringBuilder(256);
			while(true) {
				hWnd = API.FindWindowEx(hParentWnd, hWnd, null, null);
				if(hWnd == IntPtr.Zero) {
					break;
				}
				API.GetClassName(hWnd, workClassName, workClassName.Capacity);
				if(workClassName.ToString() == targetClassName) {
					API.PostMessage(hWnd, WM.WM_COMMAND, new IntPtr((int)WM_COMMAND_SUB.Refresh), IntPtr.Zero);
				} else {
					RefreshShell(hWnd);
				}
			}
		}
		public static void RefreshShell()
		{
			RefreshShell(IntPtr.Zero);
		}
	}
}
