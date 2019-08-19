/*
This file is part of PInvoke.

PInvoke is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PInvoke is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with PInvoke.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Library.PInvoke.Windows
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
