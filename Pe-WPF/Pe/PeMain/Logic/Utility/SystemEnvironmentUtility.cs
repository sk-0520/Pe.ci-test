namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using Microsoft.Win32;

	public static class SystemEnvironmentUtility
	{
		#region define

		const string hideFileRootPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
		const string extensionRootPath = hideFileRootPath;
		const string hiddenKey = "Hidden";
		const string extensionKey = "HideFileExt";

		enum HideFileHiddenType
		{
			Show = 1,
			Hide = 2,
		}
		enum HideFileSuperHiddenType
		{
			Show = 1,
			Hide = 0,
		}

		enum ExtensionHideType
		{
			Hide = 1,
			Show = 0,
		}

		#endregion

		#region function

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public static bool IsHideFileShow()
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(hideFileRootPath)) {
				//Debug.WriteLine(subKey.GetValue("ShowSuperHidden"));
				var hiddenValue = (int)subKey.GetValue(hiddenKey);
				return hiddenValue == (int)HideFileHiddenType.Show;
			}
		}
		public static void SetHideFileShow(bool show)
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(hideFileRootPath, true)) {
				var hiddenValue = (int)(show ? HideFileHiddenType.Show : HideFileHiddenType.Hide);
				subKey.SetValue(hiddenKey, hiddenValue, RegistryValueKind.DWord);
			}
		}

		public static bool IsExtensionShow()
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(extensionRootPath)) {
				var extValue = (int)subKey.GetValue(extensionKey);
				return extValue == (int)ExtensionHideType.Show;
			}
		}
		public static void SetExtensionShow(bool show)
		{
			using(var subKey = Registry.CurrentUser.OpenSubKey(extensionRootPath, true)) {
				var extType = (int)(show ? ExtensionHideType.Show : ExtensionHideType.Hide);
				subKey.SetValue(extensionKey, extType, RegistryValueKind.DWord);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="hParentWnd"></param>
		private static void RefreshShell(IntPtr hParentWnd)
		{
			const string targetClassName = "SHELLDLL_DefView";
			var hWnd = IntPtr.Zero;
			var workClassName = new StringBuilder(WindowsUtility.classNameLength);
			while(true) {
				hWnd = NativeMethods.FindWindowEx(hParentWnd, hWnd, null, null);
				if(hWnd == IntPtr.Zero) {
					break;
				}
				NativeMethods.GetClassName(hWnd, workClassName, workClassName.Capacity);
				if(workClassName.ToString() == targetClassName) {
					NativeMethods.PostMessage(hWnd, WM.WM_COMMAND, new IntPtr((int)WM_COMMAND_SUB.Refresh), IntPtr.Zero);
				} else {
					RefreshShell(hWnd);
				}
			}
		}
		public static void RefreshShell()
		{
			RefreshShell(IntPtr.Zero);
		}

		#endregion
	}
}
