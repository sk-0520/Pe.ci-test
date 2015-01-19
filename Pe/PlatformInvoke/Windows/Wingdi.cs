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
	
	public enum AC: byte
	{
	    AC_SRC_OVER = 0x00,
        AC_SRC_ALPHA = 0x01,
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

	[StructLayout(LayoutKind.Sequential)]
	public struct RGBQUAD
	{
		public byte rgbBlue;
		public byte rgbGreen;
		public byte rgbRed;
		public byte rgbReserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BITMAP
	{
		public Int32 bmType;
		public Int32 bmWidth;
		public Int32 bmHeight;
		public Int32 bmWidthBytes;
		public Int16 bmPlanes;
		public Int16 bmBitsPixel;
		public IntPtr bmBits;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BITMAPINFOHEADER
	{
		public int biSize;
		public int biWidth;
		public int biHeight;
		public Int16 biPlanes;
		public Int16 biBitCount;
		public int biCompression;
		public int biSizeImage;
		public int biXPelsPerMeter;
		public int biYPelsPerMeter;
		public int biClrUsed;
		public int bitClrImportant;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DIBSECTION
	{
		public BITMAP dsBm;
		public BITMAPINFOHEADER dsBmih;
		public int dsBitField1;
		public int dsBitField2;
		public int dsBitField3;
		public IntPtr dshSection;
		public int dsOffset;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BLENDFUNCTION
	{
		public AC BlendOp;
		public byte BlendFlags;
		public byte SourceConstantAlpha;
		public AC AlphaFormat;

		public BLENDFUNCTION(AC op, byte flags, byte alpha, AC format)
		{
			BlendOp = op;
			BlendFlags = flags;
			SourceConstantAlpha = alpha;
			AlphaFormat = format;
		}
	}


	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct GRPICONDIRENTRY
	{
		public byte bWidth;
		public byte bHeight;
		public byte bColorCount;
		public byte bReserved;
		public ushort wPlanes;
		public ushort wBitCount;
		public uint dwBytesInRes;
		public ushort nID;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ICONDIR
	{
		public ushort idReserved;
		public ushort idType;
		public ushort idCount;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ICONDIRENTRY
	{
		public byte bWidth;
		public byte bHeight;
		public byte bColorCount;
		public byte bReserved;
		public ushort wPlanes;
		public ushort wBitCount;
		public uint dwBytesInRes;
		public uint dwImageOffset;
	}


	partial class NativeMethods
	{
		[System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("gdi32.dll", EntryPoint = "GetObject")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int GetObject(IntPtr hObject, int nCount, ref DIBSECTION lpObject);

		[DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
		public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest,
		   int nWidthDest, int nHeightDest,
		   IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
		   BLENDFUNCTION blendFunction);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteDC(IntPtr hdc);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int GdipCreateBitmapFromHBITMAP(IntPtr hbitmap, IntPtr hpalette, out IntPtr bitmap);
	}
}
