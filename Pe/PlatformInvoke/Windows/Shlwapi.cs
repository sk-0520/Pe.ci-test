﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/11/07
 * 時刻: 19:40
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */

namespace ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows
{
	using System;
	using System.Runtime.InteropServices;

	partial class NativeMethods
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("shlwapi.dll", EntryPoint = "PathIsUNCW", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PathIsUNC([MarshalAs(UnmanagedType.LPTStr)]string pszPath);
	}
}
