/*
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.PInvoke.Windows;
using Microsoft.Win32;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
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

        const string webbrowserEmulationPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        const string webbrowserRenderingPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_DOCUMENT_COMPATIBLE_MODE";
        const int webbrowserDefaultVersion = 7000;
        const string internetExplorerPath = @"Software\Microsoft\Internet Explorer";
        const string internetExplorer10Key = "svcVersion";
        const string internetExplorer9Key = "Version";

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

        static int ToIEVersion(Version version)
        {
            // TODO: 小数点以下は要調査
            var versionNumber = version.Major * 1000;
            return versionNumber;
        }

        /// <summary>
        /// .NET Frameworkで使用するIEバージョンを設定する。
        /// </summary>
        /// <param name="version">IEバージョン<para>https://msdn.microsoft.com/en-us/library/ee330730%28v=vs.85%29.aspx#browser_emulation</para></param>
        /// <param name="programName">対象とするプログラムのファイル名</param>
        public static void SetUsingBrowserVersion(int version, string programName)
        {
            if(version == webbrowserDefaultVersion) {
                ResetUsingBrowserVersion(programName);
            } else {
                using(var key = Registry.CurrentUser.CreateSubKey(webbrowserEmulationPath)) {
                    key.SetValue(programName, version, RegistryValueKind.DWord);
                }
                using(var key = Registry.CurrentUser.CreateSubKey(webbrowserRenderingPath)) {
                    key.SetValue(programName, version, RegistryValueKind.DWord);
                }
            }
        }
        public static void SetUsingBrowserVersion(Version version, string programName)
        {
            SetUsingBrowserVersion(ToIEVersion(version), programName);
        }

        static string GetExecutingAssemblyFileName()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var name = Path.GetFileName(path);

            return name;
        }

#if DEBUG
        static string GetDebugName(string programName)
        {
            var ext = Path.GetExtension(programName);
            return Path.ChangeExtension(programName, "vshost" + ext);
        }
#endif

        /// <summary>
        /// 現在実行中のアセンブリに対して.NET Frameworkで使用するIEバージョンを設定する。
        /// <para>デバッグバージョンであればvshostも対象とする</para>
        /// </summary>
        /// <param name="version">IEバージョン<para>https://msdn.microsoft.com/en-us/library/ee330730%28v=vs.85%29.aspx#browser_emulation</para></param>
        public static void SetUsingBrowserVersionForExecutingAssembly(int version)
        {
            var name = GetExecutingAssemblyFileName();
            SetUsingBrowserVersion(version, name);
#if DEBUG
            SetUsingBrowserVersion(version, GetDebugName(name));
#endif
        }
        public static void SetUsingBrowserVersionForExecutingAssembly(Version version)
        {
            SetUsingBrowserVersionForExecutingAssembly(ToIEVersion(version));
        }
        /// <summary>
        /// .NET Frameworkで使用するIEバージョンを初期状態に戻す。
        /// </summary>
        /// <param name="programName">対象とするプログラムのファイル名</param>
        public static void ResetUsingBrowserVersion(string programName)
        {
            using(var key = Registry.CurrentUser.OpenSubKey(webbrowserEmulationPath, true)) {
                if(key != null) {
                    key.DeleteValue(programName, false);
                }
            }
            using(var key = Registry.CurrentUser.OpenSubKey(webbrowserRenderingPath, true)) {
                if(key != null) {
                    key.DeleteValue(programName, false);
                }
            }
        }

        /// <summary>
        /// 現在実行中のアセンブリに対して.NET Frameworkで使用するIEバージョンを初期状態に戻す。
        /// </summary>
        public static void ResetUsingBrowserVersionForExecutingAssembly()
        {
            var name = GetExecutingAssemblyFileName();
            ResetUsingBrowserVersion(name);
#if DEBUG
            ResetUsingBrowserVersion(GetDebugName(name));
#endif
        }

        public static Version GetInternetExplorerVersion()
        {
            using(var key = Registry.LocalMachine.OpenSubKey(internetExplorerPath)) {
                if(key == null) {
                    return new Version(7, 0);
                } else {
                    var rawVersion = (string)(key.GetValue(internetExplorer10Key) ?? key.GetValue(internetExplorer9Key) ?? "7.0");
                    return new Version(rawVersion);
                }
            }
        }

        #endregion
    }
}
