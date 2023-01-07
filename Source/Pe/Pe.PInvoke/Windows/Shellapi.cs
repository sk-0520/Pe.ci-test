using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    public enum ABM: int
    {
        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/bb787951%28v=vs.85%29.aspx
        /// </summary>
        ABM_NEW = 0,
        ABM_REMOVE = 1,
        ABM_QUERYPOS = 2,
        ABM_SETPOS = 3,
        ABM_GETSTATE = 4,
        ABM_GETTASKBARPOS = 5,
        ABM_ACTIVATE = 6,
        ABM_GETAUTOHIDEBAR = 7,
        ABM_SETAUTOHIDEBAR = 8,
        ABM_WINDOWPOSCHANGED = 9,
        ABM_SETSTATE = 10,
    }

    public enum ABN: int
    {
        ABN_STATECHANGE = 0,
        ABN_POSCHANGED = 1,
        ABN_FULLSCREENAPP = 2,
        ABN_WINDOWARRANGE = 3,
    }

    public enum ABE: int
    {
        ABE_LEFT = 0,
        ABE_TOP = 1,
        ABE_RIGHT = 2,
        ABE_BOTTOM = 3,
    }

    /// <summary>
    /// http://www.pinvoke.net/default.aspx/shell32/APPBARDATA%20.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        //  typedef struct _AppBarData
        //      {
        //        DWORD cbSize;
        //        HWND hWnd;
        //        UINT uCallbackMessage;
        //        UINT uEdge;
        //        RECT rc;
        //        LPARAM lParam;
        //      } APPBARDATA, *PAPPBARDATA;
        public int cbSize;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public ABE uEdge;
        public RECT rc;
        public int lParam;

        public APPBARDATA(IntPtr hWnd)
        {
            this.cbSize = Marshal.SizeOf(typeof(APPBARDATA));
            this.hWnd = hWnd;
            this.uCallbackMessage = 0;
            this.uEdge = ABE.ABE_LEFT;
            this.rc = new RECT();
            this.lParam = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEINFO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    /// <summary>
    /// http://pinvoke.net/default.aspx/Enums/FileInfoFlags.html
    /// </summary>
    [Flags]
    public enum SHGFI: uint
    {
        SHGFI_ICON = 0x000000100,     // get icon
        SHGFI_DISPLAYNAME = 0x000000200,     // get display name
        SHGFI_TYPENAME = 0x000000400,     // get type name
        SHGFI_ATTRIBUTES = 0x000000800,     // get attributes
        SHGFI_ICONLOCATION = 0x000001000,     // get icon location
        SHGFI_EXETYPE = 0x000002000,     // return exe type
        SHGFI_SYSICONINDEX = 0x000004000,     // get system icon index
        SHGFI_LINKOVERLAY = 0x000008000,     // put a link overlay on icon
        SHGFI_SELECTED = 0x000010000,     // show icon in selected state
        SHGFI_ATTR_SPECIFIED = 0x000020000,     // get only specified attributes
        SHGFI_LARGEICON = 0x000000000,     // get large icon
        SHGFI_SMALLICON = 0x000000001,     // get small icon
        SHGFI_OPENICON = 0x000000002,     // get open icon
        SHGFI_SHELLICONSIZE = 0x000000004,     // get shell size icon
        SHGFI_PIDL = 0x000000008,     // pszPath is a pidl
        SHGFI_USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
        SHGFI_ADDOVERLAYS = 0x000000020,     // apply the appropriate overlays
        SHGFI_OVERLAYINDEX = 0x000000040,     // Get the index of the overlay in
                                              // the upper 8 bits of the iIcon
    }

    public enum SHIL
    {
        SHIL_LARGE = 0,   // normally 32x32
        SHIL_SMALL = 1,   // normally 16x16
        SHIL_EXTRALARGE = 2,
        SHIL_SYSSMALL = 3,   // like SHIL_SMALL, but tracks system small icon metric correctly
        SHIL_LAST = SHIL_SYSSMALL,
        SHIL_JUMBO = 0x04,
    }

    partial class NativeMethods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "#62")]
        public extern static bool SHChangeIconDialog(IntPtr hOwner, StringBuilder szFilename, int Reserved, ref int lpIconIndex);

        /// <summary>
        /// http://www.pinvoke.net/default.aspx/shell32/SHAppBarMessage.html
        /// </summary>
        /// <param name="dwMessage"></param>
        /// <param name="pData"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shell32.dll")]
        public static extern IntPtr SHAppBarMessage(ABM dwMessage, ref APPBARDATA pData);

        /// <summary>
        /// http://pinvoke.net/default.aspx/shell32/SHGetFileInfo.html
        /// </summary>
        /// <param name="pszPath"></param>
        /// <param name="dwFileAttributes"></param>
        /// <param name="psfi"></param>
        /// <param name="cbFileInfo"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        /// <summary>
        /// http://pinvoke.net/default.aspx/shell32/SHGetImageList.html
        /// </summary>
        /// <param name="iImageList"></param>
        /// <param name="riid"></param>
        /// <param name="ppv"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static HRESULT SHGetImageList(int iImageList, ref Guid riid, out IImageList ppv);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:識別子は、不適切なサフィックスを含むことはできません", Justification = "WindowsAPI")]
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIc);

        #region ぬぬぬ
        [DllImport("shell32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);
        #endregion
    }
}
