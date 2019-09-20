/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.PInvoke.Windows.root.CIMV2;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    /// <summary>
    /// スクリーン共通処理。
    /// </summary>
    public static class ScreenUtility
    {
        private static string DeviceToId(string deviceName)
        {
            return new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
        }

        private static IEnumerable<Win32_DesktopMonitor> GetScreens(string deviceName, ILogger logger = null)
        {
            string query = "SELECT * FROM Win32_DesktopMonitor";
            if(!string.IsNullOrWhiteSpace(deviceName)) {
                //var id = new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
                var id = DeviceToId(deviceName);
                query = string.Format("SELECT * FROM Win32_DesktopMonitor where DeviceID like \"DesktopMonitor{0}\"", id);
            }
            using(var searcher = new ManagementObjectSearcher(query)) {
                foreach(ManagementBaseObject mng in searcher.Get()) {
                    var item = new Win32_DesktopMonitor();
                    try {
                        item.Import(mng);
                    } catch(Exception ex) {
                        logger.SafeWarning(ex);
                        continue;
                    }

                    yield return item;
                }
            }
        }

        /// <summary>
        /// スクリーンの名前を取得。
        /// </summary>
        /// <param name="screen"></param>
        /// <param name = "logger"></param>
        /// <returns></returns>
        public static string GetScreenName(ScreenModel screen, ILogger logger = null)
        {
            foreach(var screem in GetScreens(screen.DeviceName, logger)) {
                if(!string.IsNullOrWhiteSpace(screem.Name)) {
                    var id = DeviceToId(screen.DeviceName);
                    return string.Format("{0}. {1}", id, screem.Name);
                }
                break;
            }

            var device = new DISPLAY_DEVICE();
            device.cb = Marshal.SizeOf(device);
            NativeMethods.EnumDisplayDevices(screen.DeviceName, 0, ref device, 1);

            //return screen.DeviceName;
            return device.DeviceString;
        }

        public static string GetScreenName(string screenDeviceName, ILogger logger = null)
        {
            var screen = Screen.AllScreens.SingleOrDefault(s => s.DeviceName == screenDeviceName);
            if(screen != null) {
                return GetScreenName(screen, logger);
            }
            return screenDeviceName;
        }

        //public static Screen GetCurrentCursor()
        //{
        //	return Screen.FromPoint(Cursor.Position);
        //}

        /// <summary>
        /// ウィンドウを指定スクリーンの中央に移動。
        /// </summary>
        /// <param name="window"></param>
        /// <param name="screen"></param>
        public static void MoveCenter(Window window, ScreenModel screen)
        {
            var logicalWindowSize = new Size(window.Width, window.Height);
            var deviceWindowSize = UIUtility.ToDevicePixel(window, logicalWindowSize);
            var deviceWindowPosition = new Point(
                screen.DeviceBounds.Left + screen.DeviceBounds.Width / 2 - deviceWindowSize.Width / 2,
                screen.DeviceBounds.Top + screen.DeviceBounds.Height / 2 - deviceWindowSize.Height / 2
            );
            var logicalWindowPotision = UIUtility.ToLogicalPixel(window, deviceWindowPosition);
            window.Left = logicalWindowPotision.X;
            window.Top = logicalWindowPotision.Y;
        }

        public static void AttachStartupMoveScreenCenter(Window window, ScreenModel screen)
        {
            MoveCenter(window, screen);
            window.SourceInitialized += Window_SourceInitialized;
        }

        static void Window_SourceInitialized(object sender, EventArgs e)
        {
            var window = (Window)sender;
            window.SourceInitialized -= Window_SourceInitialized;
        }
    }
}
