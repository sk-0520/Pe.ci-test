using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:列挙値の前に型名を付けないでください", Justification = "WindowsAPI")]
    public enum FILE_ATTRIBUTE
    {
        FILE_ATTRIBUTE_NORMAL = 0x00000080,
    }

    [Flags]
    public enum LOAD_LIBRARY: uint
    {
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
        LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
    }

    /// <summary>
    /// リソースの種類。
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/ja-jp/windows/win32/menurc/resource-types"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:列挙値の前に型名を付けないでください", Justification = "WindowsAPI")]
    public enum RT
    {
        /// <summary>
        /// ハードウェアに依存するカーソルリソース。
        /// </summary>
        RT_CURSOR = 1,
        /// <summary>
        /// ビットマップリソース。
        /// </summary>
        RT_BITMAP = 2,
        /// <summary>
        /// ハードウェアに依存するアイコンリソース。
        /// </summary>
        RT_ICON = 3,
        /// <summary>
        /// Menu リソース。
        /// </summary>
        RT_MENU = 4,
        /// <summary>
        /// ダイアログボックス。
        /// </summary>
        RT_DIALOG = 5,
        /// <summary>
        /// 文字列-テーブルエントリ。
        /// </summary>
        RT_STRING = 6,
        /// <summary>
        /// フォントディレクトリリソース。
        /// </summary>
        RT_FONTDIR = 7,
        /// <summary>
        /// フォントリソース。
        /// </summary>
        RT_FONT = 8,
        /// <summary>
        /// アクセラレータテーブル。
        /// </summary>
        RT_ACCELERATOR = 9,
        /// <summary>
        /// アプリケーションで定義されたリソース (生データ)。
        /// </summary>
        RT_RCDATA = 10,
        /// <summary>
        /// メッセージテーブルエントリ。
        /// </summary>
        RT_MESSAGETABLE = 11,
        /// <summary>
        /// ハードウェアに依存しないカーソルリソース。
        /// </summary>
        RT_GROUP_CURSOR = 12,
        /// <summary>
        /// ハードウェアに依存しないアイコンリソース。
        /// </summary>
        RT_GROUP_ICON = 14,
        /// <summary>
        /// バージョンリソース。
        /// </summary>
        RT_VERSION = 16,
        /// <summary>
        /// リソース編集ツールで文字列を .rc ファイルに関連付けることができるようにします。
        /// </summary>
        /// <remarks>
        /// <para>通常、文字列は、シンボル名を提供するヘッダーファイルの名前です。 リソースコンパイラは文字列を解析しますが、それ以外の場合は値を無視します。 たとえば、次のように入力します。</para>
        /// <code>1 DLGINCLUDE "MyFile.h"</code>
        /// </remarks>
        RT_DLGINCLUDE = 17,
        /// <summary>
        /// プラグアンドプレイリソース。
        /// </summary>
        RT_PLUGPLAY = 19,
        /// <summary>
        /// VXD.
        /// </summary>
        RT_VXD = 20,
        /// <summary>
        /// アニメーション化したカーソル。
        /// </summary>
        RT_ANICURSOR = 21,
        /// <summary>
        /// アニメーション化アイコン。
        /// </summary>
        RT_ANIICON = 22,
        /// <summary>
        /// HTML リソース。
        /// </summary>
        RT_HTML = 23,
        /// <summary>
        /// Side-by-side アセンブリマニフェスト。
        /// </summary>
        RT_MANIFEST = 24
    }

    [FlagsAttribute]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:列挙値の前に型名を付けないでください", Justification = "WindowsAPI")]
    public enum ES: uint
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:識別子は、不適切なサフィックスを含むことはできません", Justification = "WindowsAPI")]
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
