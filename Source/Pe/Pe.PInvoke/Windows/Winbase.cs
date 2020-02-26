using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    public enum FILE_ATTRIBUTE
    {
        FILE_ATTRIBUTE_NORMAL = 0x00000080,
    }

    [Flags]
    public enum LOAD_LIBRARY : uint
    {
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
        LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
    }

    public enum ResType
    {
        CURSOR = 1,
        BITMAP = 2,
        ICON = 3,
        MENU = 4,
        DIALOG = 5,
        STRING = 6,
        FONTDIR = 7,
        FONT = 8,
        ACCELERATOR = 9,
        RCDATA = 10,
        MESSAGETABLE = 11,
        GROUP_CURSOR = 12,
        GROUP_ICON = 14,
        VERSION = 16,
        DLGINCLUDE = 17,
        PLUGPLAY = 19,
        VXD = 20,
        ANICURSOR = 21,
        ANIICON = 22,
        HTML = 23,
        MANIFEST = 24
    }

    [FlagsAttribute]
    public enum ES : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
        // Legacy flag, should not be used.
        // ES_USER_PRESENT = 0x00000004
    }
    /// <summary>
    /// The CharSet must match the CharSet of the corresponding PInvoke signature
    ///
    /// http://www.pinvoke.net/default.aspx/Structures/WIN32_FIND_DATA.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WIN32_FIND_DATA
    {
        public uint dwFileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;
    }


    public delegate bool EnumResNameProc(IntPtr hModule, IntPtr type, IntPtr name, IntPtr lp);

    partial class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LOAD_LIBRARY dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr FindResource(IntPtr hModule, string lpName, string lpType);
        [DllImport("kernel32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpID, IntPtr lpType);

        [DllImport("kernel32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool EnumResourceNames(IntPtr hModule, string lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam);

        [DllImport("kernel32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool EnumResourceNames(IntPtr hModule, int dwID, EnumResNameProc lpEnumFunc, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [SuppressUnmanagedCodeSecurity]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern void SetLastError(uint dwErrCode);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern ES SetThreadExecutionState(ES esFlags);

        [DllImport("kernel32.dll")]
        public static extern bool GetProcessWorkingSetSize(IntPtr hProcess, out UIntPtr lpMinimumWorkingSetSize, out UIntPtr lpMaximumWorkingSetSize);
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);


    }
}
