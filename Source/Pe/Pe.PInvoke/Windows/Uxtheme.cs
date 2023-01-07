using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    /// <summary>
    /// http://www.pinvoke.net/default.aspx/Enums/DTT.html
    /// </summary>
    [Flags]
    public enum DTT: uint
    {
        TextColor = (1 << 0),
        BorderColor = (1 << 1),
        ShadowColor = (1 << 2),
        ShadowType = (1 << 3),
        ShadowOffset = (1 << 4),
        BorderSize = (1 << 5),
        FontProp = (1 << 6),
        ColorProp = (1 << 7),
        StateID = (1 << 8),
        CalcRect = (1 << 9),
        ApplyOverlay = (1 << 10),
        GlowSize = (1 << 11),
        Callback = (1 << 12),
        Composited = (1 << 13)
    }

    /// <summary>
    /// http://www.pinvoke.net/default.aspx/Enums/TEXTSHADOWTYPE.html
    /// </summary>
    public enum TEXTSHADOWTYPE: int
    {
        None = 0,
        Single = 1,
        Continuous = 2,
    }

    /// <summary>
    /// http://www.pinvoke.net/default.aspx/Structures/DTTOPTS.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DTTOPTS
    {
        public uint dwSize;
        public DTT dwFlags;
        public COLORREF crText;
        public COLORREF crBorder;
        public COLORREF crShadow;
        public TEXTSHADOWTYPE iTextShadowType;
        public POINT ptShadowOffset;
        public int iBorderSize;
        public int iFontPropId;
        public int iColorPropId;
        public int iStateId;
        public bool fApplyOverlay;
        public int iGlowSize;
        public IntPtr pfnDrawTextCallback;
        public IntPtr lParam;
    }

    partial class NativeMethods
    {

        [DllImport("UxTheme.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:識別子は、不適切なサフィックスを含むことはできません", Justification = "WindowsAPI")]
        public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref RECT pRect, ref DTTOPTS pOptions);
    }
}
