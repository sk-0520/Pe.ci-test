using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [Flags]
    public enum DISPLAY_DEVICE_STATE
    {
        DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001,
        DISPLAY_DEVICE_DISCONNECT = 0x02000000,
        DISPLAY_DEVICE_MIRRORING_DRIVER = 0x00000008,
        DISPLAY_DEVICE_MODESPRUNED = 0x08000000,
        DISPLAY_DEVICE_MULTI_DRIVER = 0x00000002,
        DISPLAY_DEVICE_PRIMARY_DEVICE = 0x00000004,
        DISPLAY_DEVICE_REMOTE = 0x04000000,
        DISPLAY_DEVICE_REMOVABLE = 0x00000020,
        DISPLAY_DEVICE_VGA_COMPATIBLE = 0x00000010,
    }

    public enum AC: byte
    {
        AC_SRC_OVER = 0x00,
        AC_SRC_ALPHA = 0x01,
    }

    public enum DT
    {
        DT_TOP = 0x00000000,
        DT_LEFT = 0x00000000,
        /// <summary>
        /// テキストを長方形領域内で横方向に中央揃えで表示します。
        /// </summary>
        DT_CENTER = 0x00000001,
        DT_RIGHT = 0x00000002,
        DT_VCENTER = 0x00000004,
        /// <summary>
        /// 長方形領域の下端にテキストを揃えます。DT_SINGLELINE と同時に指定しなければなりません。
        /// </summary>
        DT_BOTTOM = 0x00000008,
        DT_WORDBREAK = 0x00000010,
        DT_SINGLELINE = 0x00000020,
        DT_EXPANDTABS = 0x00000040,
        DT_TABSTOP = 0x00000080,
        DT_NOCLIP = 0x00000100,
        DT_EXTERNALLEADING = 0x00000200,
        /// <summary>
        /// 指定されたテキストを表示するために必要な長方形領域の幅と高さを調べます。複数行テキストの場合は、lpRect パラメータで指定された長方形領域の幅を使い、長方形領域の下端をテキストの最終行の下側の境界線にまで広げます。テキストを 1 行で表示する場合は、長方形領域の右端を行の最後の文字の右側の境界線に合うように変更します。どちらの場合も、DrawText 関数は、テキストの描画は行わず、整形されたテキストの高さを返します。
        /// </summary>
        DT_CALCRECT = 0x00000400,
        DT_NOPREFIX = 0x00000800,
        DT_INTERNAL = 0x00001000,
        /// <summary>
        /// 文字列の最後の部分が長方形領域に納まり切らない場合、はみ出す部分が切り取られ、末尾に省略符号（...）が追加されます。文字列の最後ではない場所にある単語が長方形領域からはみ出す場合は、省略記号なしで切り取られます。
        /// 
        /// DT_MODIFYSTRING フラグがセットされていない限り、文字列が変更されることはありません。
        /// 
        /// DT_PATH_ELLIPSIS および DT_WORD_ELLIPSIS の説明も参照してください。
        /// </summary>
        DT_END_ELLIPSIS = 0x00008000,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DISPLAY_DEVICE
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        [MarshalAs(UnmanagedType.U4)]
        public DISPLAY_DEVICE_STATE StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
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

    /// <summary>
    ///     Specifies a raster-operation code. These codes define how the color data for the
    ///     source rectangle is to be combined with the color data for the destination
    ///     rectangle to achieve the final color.
    /// </summary>
    public enum ROP: uint
    {
        /// <summary>dest = source</summary>
        SRCCOPY = 0x00CC0020,
        /// <summary>dest = source OR dest</summary>
        SRCPAINT = 0x00EE0086,
        /// <summary>dest = source AND dest</summary>
        SRCAND = 0x008800C6,
        /// <summary>dest = source XOR dest</summary>
        SRCINVERT = 0x00660046,
        /// <summary>dest = source AND (NOT dest)</summary>
        SRCERASE = 0x00440328,
        /// <summary>dest = (NOT source)</summary>
        NOTSRCCOPY = 0x00330008,
        /// <summary>dest = (NOT src) AND (NOT dest)</summary>
        NOTSRCERASE = 0x001100A6,
        /// <summary>dest = (source AND pattern)</summary>
        MERGECOPY = 0x00C000CA,
        /// <summary>dest = (NOT source) OR dest</summary>
        MERGEPAINT = 0x00BB0226,
        /// <summary>dest = pattern</summary>
        PATCOPY = 0x00F00021,
        /// <summary>dest = DPSnoo</summary>
        PATPAINT = 0x00FB0A09,
        /// <summary>dest = pattern XOR dest</summary>
        PATINVERT = 0x005A0049,
        /// <summary>dest = (NOT dest)</summary>
        DSTINVERT = 0x00550009,
        /// <summary>dest = BLACK</summary>
        BLACKNESS = 0x00000042,
        /// <summary>dest = WHITE</summary>
        WHITENESS = 0x00FF0062,
        /// <summary>
        /// Capture window as seen on screen.  This includes layered windows
        /// such as WPF windows with AllowsTransparency="true"
        /// </summary>
        CAPTUREBLT = 0x40000000
    }

    /// <summary>
    /// http://pinvoke.net/default.aspx/Structures/BITMAPINFO.html
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        /// <summary>
        /// A BITMAPINFOHEADER structure that contains information about the dimensions of color format.
        /// </summary>
        public BITMAPINFOHEADER bmiHeader;

        /// <summary>
        /// An array of RGBQUAD. The elements of the array that make up the color table.
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
        public RGBQUAD[] bmiColors;
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
            this.BlendOp = op;
            this.BlendFlags = flags;
            this.SourceConstantAlpha = alpha;
            this.AlphaFormat = format;
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

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int GdipCreateBitmapFromHBITMAP(IntPtr hbitmap, IntPtr hpalette, out IntPtr bitmap);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BITMAPINFO pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        /// <summary>
        ///    Performs a bit-block transfer of the color data corresponding to a
        ///    rectangle of pixels from the specified source device context into
        ///    a destination device context.
        /// </summary>
        /// <param name="hdc">Handle to the destination device context.</param>
        /// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
        /// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
        /// <param name="hdcSrc">Handle to the source device context.</param>
        /// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
        /// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
        /// <param name="dwRop">A raster-operation code.</param>
        /// <returns>
        ///    <c>true</c> if the operation succeedes, <c>false</c> otherwise. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, ROP dwRop);

    }
}
