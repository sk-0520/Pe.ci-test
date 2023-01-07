using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:—ñ‹“’l‚Ì‘O‚ÉŒ^–¼‚ð•t‚¯‚È‚¢‚Å‚­‚¾‚³‚¢", Justification = "WindowsAPI")]
    public enum TVS
    {
        TVS_NOHSCROLL = 0x8000,
        TVS_EX_AUTOHSCROLL = 0x0020,
        TVS_EX_FADEINOUTEXPANDOS = 0x0040,
    }

    /// <summary>
    /// http://pinvoke.net/default.aspx/Structures/IMAGELISTDRAWPARAMS.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGELISTDRAWPARAMS
    {
        public int cbSize;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr himl;
        public int i;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xBitmap;
        public int yBitmap;
        public int rgbBk;
        public int rgbFg;
        public int fStyle;
        public int dwRop;
        public int fState;
        public int Frame;
        public int crEffect;
    }

    /// <summary>
    /// http://pinvoke.net/default.aspx/Structures/IMAGEINFO.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGEINFO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hbmImage;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public RECT rcImage;
    }

}
