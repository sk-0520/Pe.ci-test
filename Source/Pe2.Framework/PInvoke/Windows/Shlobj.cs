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
    public enum SHCNE
    {
        SHCNE_ALLEVENTS = 0x7FFFFFFF,
        /// <summary>
        /// ファイルタイプの関連付けに変更された
        /// </summary>
        SHCNE_ASSOCCHANGED = 0x8000000,
        /// <summary>
        ///  既存のフォルダの内容が変化したが、(フォルダ名の変化はない)
        /// </summary>
        SHCNE_UPDATEDIR = 0x1000,
    }

    public enum SHCNF
    {
        /// <summary>
        ///  	dwItem1、dwItem2はアイテムIDリストのアドレス
        /// </summary>
        SHCNF_IDLIST = 0x0000,
        /// <summary>
        /// dwItem1、dwItem2はDWORD値
        /// </summary>
        SHCNF_DWORD = 0x0003,
    }

    public enum SHOP: uint
    {
        /// <summary>
        /// lpObject points to a printer friendly name
        /// </summary>
        SHOP_PRINTERNAME = 0x01,
        /// <summary>
        /// lpObject points to a fully qualified path+file name
        /// </summary>
        SHOP_FILEPATH = 0x02,
        /// <summary>
        /// lpObject points to a Volume GUID
        /// </summary>
        SHOP_VOLUMEGUID = 0x04,
    }

    partial class NativeMethods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(SHCNE wEventId, SHCNF uFlags, IntPtr dwItem1, IntPtr dwItem2);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern bool SHObjectProperties(IntPtr hwnd, SHOP shopObjectType, [MarshalAs(UnmanagedType.LPWStr)] string pszObjectName, [MarshalAs(UnmanagedType.LPWStr)] string pszPropertyPage);
    }
}
