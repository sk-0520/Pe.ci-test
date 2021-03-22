using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    partial class NativeMethods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shlwapi.dll", EntryPoint = "PathIsUNCW", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PathIsUNC([MarshalAs(UnmanagedType.LPTStr)] string pszPath);

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }
}
