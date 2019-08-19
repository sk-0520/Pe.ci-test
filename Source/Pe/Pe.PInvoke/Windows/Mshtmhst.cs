using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [ComImport, Guid("C4D244B0-D43E-11CF-893B-00AA00BDCE1A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDocHostShowUI
    {
        [return: MarshalAs(UnmanagedType.U4)]
        [PreserveSig]
        int ShowMessage(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.LPWStr)] string lpstrText,
            [MarshalAs(UnmanagedType.LPWStr)] string lpstrCaption,
            int dwType,
            [MarshalAs(UnmanagedType.LPWStr)] string lpstrHelpFile,
            int dwHelpContext,
            out int lpResult
        );

        [return: MarshalAs(UnmanagedType.U4)]
        [PreserveSig]
        int ShowHelp(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.LPWStr)] string pszHelpFile,
            int uCommand,
            int dwData,
            POINT ptMouse,
            [MarshalAs(UnmanagedType.IDispatch)] object pDispatchObjectHit
        );
    }
}
