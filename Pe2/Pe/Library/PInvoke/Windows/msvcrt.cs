namespace ContentTypeTextNet.Pe.Library.PInvoke.Windows
{
	using System;
	using System.Runtime.InteropServices;

	partial class NativeMethods
	{
		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int memcmp(byte[] b1, byte[] b2, long count);

		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int memcmp(IntPtr b1, IntPtr b2, long count);
	}
}