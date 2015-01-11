using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows
{
	[Flags]
	public enum DISPLAY_DEVICE_STATE
	{
		DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001,
		DISPLAY_DEVICE_DISCONNECT          = 0x02000000,
		DISPLAY_DEVICE_MIRRORING_DRIVER    = 0x00000008,
		DISPLAY_DEVICE_MODESPRUNED         = 0x08000000,
		DISPLAY_DEVICE_MULTI_DRIVER        = 0x00000002,
		DISPLAY_DEVICE_PRIMARY_DEVICE      = 0x00000004,
		DISPLAY_DEVICE_REMOTE              = 0x04000000,
		DISPLAY_DEVICE_REMOVABLE           = 0x00000020,
		DISPLAY_DEVICE_VGA_COMPATIBLE      = 0x00000010,
	}
	
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	public struct DISPLAY_DEVICE
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cb;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
		public string DeviceName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
		public string DeviceString;
		[MarshalAs(UnmanagedType.U4)]
		public DISPLAY_DEVICE_STATE StateFlags;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
		public string DeviceID;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
		public string DeviceKey;
	}

	partial class NativeMethods
	{
		[System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
	}
}
