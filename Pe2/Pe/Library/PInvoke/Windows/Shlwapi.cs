
namespace ContentTypeTextNet.Pe.Library.PInvoke.Windows
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
