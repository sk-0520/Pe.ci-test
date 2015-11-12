/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.PInvoke.Windows;
    using Microsoft.Win32;

    public static class SystemEnvironmentUtility
    {
        #region define

        const string hideFileRootPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        const string extensionRootPath = hideFileRootPath;
        const string hiddenKey = "Hidden";
        const string extensionKey = "HideFileExt";

        enum HiddenFileHiddenType
        {
            Show = 1,
            Hide = 2,
        }
        enum HiddenFileSuperHiddenType
        {
            Show = 1,
            Hide = 0,
        }

        enum ExtensionHideType
        {
            Hide = 1,
            Show = 0,
        }

        #endregion

        #region function

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static bool IsHiddenFileShow()
        {
            using(var subKey = Registry.CurrentUser.OpenSubKey(hideFileRootPath)) {
                //Debug.WriteLine(subKey.GetValue("ShowSuperHidden"));
                var hiddenValue = (int)subKey.GetValue(hiddenKey);
                return hiddenValue == (int)HiddenFileHiddenType.Show;
            }
        }
        public static void SetHiddenFileShow(bool show)
        {
            using(var subKey = Registry.CurrentUser.OpenSubKey(hideFileRootPath, true)) {
                var hiddenValue = (int)(show ? HiddenFileHiddenType.Show : HiddenFileHiddenType.Hide);
                subKey.SetValue(hiddenKey, hiddenValue, RegistryValueKind.DWord);
            }
        }

        public static bool IsExtensionShow()
        {
            using(var subKey = Registry.CurrentUser.OpenSubKey(extensionRootPath)) {
                var extValue = (int)subKey.GetValue(extensionKey);
                return extValue == (int)ExtensionHideType.Show;
            }
        }
        public static void SetExtensionShow(bool show)
        {
            using(var subKey = Registry.CurrentUser.OpenSubKey(extensionRootPath, true)) {
                var extType = (int)(show ? ExtensionHideType.Show : ExtensionHideType.Hide);
                subKey.SetValue(extensionKey, extType, RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="hParentWnd"></param>
        private static void RefreshShell(IntPtr hParentWnd)
        {
            const string targetClassName = "SHELLDLL_DefView";
            var hWnd = IntPtr.Zero;
            var workClassName = new StringBuilder(WindowsUtility.classNameLength);
            while(true) {
                hWnd = NativeMethods.FindWindowEx(hParentWnd, hWnd, null, null);
                if(hWnd == IntPtr.Zero) {
                    break;
                }
                NativeMethods.GetClassName(hWnd, workClassName, workClassName.Capacity);
                if(workClassName.ToString() == targetClassName) {
                    NativeMethods.PostMessage(hWnd, WM.WM_COMMAND, new IntPtr((int)WM_COMMAND_SUB.Refresh), IntPtr.Zero);
                } else {
                    RefreshShell(hWnd);
                }
            }
        }
        public static void RefreshShell()
        {
            RefreshShell(IntPtr.Zero);
        }

        #endregion
    }
}
