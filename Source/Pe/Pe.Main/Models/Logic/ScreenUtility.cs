using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows.root.CIMV2;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public static class ScreenUtility
    {

        #region function

        private static string DeviceToId(string? deviceName)
        {
            if(string.IsNullOrEmpty(deviceName)) {
                return string.Empty;
            }

            return new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
        }

        private static IEnumerable<Win32_DesktopMonitor> GetScreens(string? deviceName, ILoggerFactory loggerFactory)
        {
            ILogger? logger = null;
            string query = "SELECT * FROM Win32_DesktopMonitor";
            if(!string.IsNullOrWhiteSpace(deviceName)) {
                //var id = new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
                var id = DeviceToId(deviceName);
                query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM Win32_DesktopMonitor where DeviceID like \"DesktopMonitor{0}\"", id);
            }
            //var q = new ObjectQuery(query);
            //ManagementPath path = new ManagementPath("//./root/cimv2");
            //var ms = new ManagementScope(default(ManagementPath)!);
            //ms.Path.Path = "//./root/cimv2";
            using(var searcher = new ManagementObjectSearcher(query)) {
                foreach(ManagementBaseObject mng in searcher.Get()) {
                    var item = new Win32_DesktopMonitor();
                    try {
                        item.Import(mng);
                    } catch(Exception ex) {
                        if(logger == null) {
                            logger = loggerFactory.CreateLogger(typeof(ScreenUtility));
                        }
                        logger.LogWarning(ex, ex.Message);
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
        public static string GetName(IScreen screen, ILoggerFactory loggerFactory)
        {
            foreach(var screenItem in GetScreens(screen.DeviceName, loggerFactory)) {
                if(!string.IsNullOrWhiteSpace(screenItem.Name)) {
                    var id = DeviceToId(screen.DeviceName);
                    return string.Format(CultureInfo.InvariantCulture, "{0}. {1}", id, screenItem.Name);
                }
            }

            var device = new DISPLAY_DEVICE();
            device.cb = Marshal.SizeOf(device);
            NativeMethods.EnumDisplayDevices(screen.DeviceName, 0, ref device, 1);

            //return screen.DeviceName;
            return device.DeviceString;
        }

        public static System.Windows.Point GetDpiScale(IScreen screen)
        {
            //foreach(var screem in GetScreens(screen.DeviceName)) {
            //    if(screem.PixelsPerXLogicalInch.HasValue && screem.PixelsPerYLogicalInch.HasValue) {
            //        return new System.Windows.Point(
            //            screem.PixelsPerXLogicalInch.Value / 96.0,
            //            screem.PixelsPerXLogicalInch.Value / 96.0
            //        );
            //    }
            //}
            var hDC = NativeMethods.CreateDC("DISPLAY", screen.DeviceName, null!, IntPtr.Zero);
            try {
                var dpiX = NativeMethods.GetDeviceCaps(hDC, DeviceCap.LOGPIXELSX);
                var dpiY = NativeMethods.GetDeviceCaps(hDC, DeviceCap.LOGPIXELSY);
                return new System.Windows.Point(
                    dpiX / 96.0,
                    dpiY / 96.0
                );
            } finally {
                if(hDC != IntPtr.Zero) {
                    NativeMethods.DeleteDC(hDC);
                }
            }
        }

        public static bool RegisterDatabase(IScreen screen, IDatabaseContext context, IDatabaseStatementLoader databaseStatementLoader, IDatabaseImplementation implementation, IDatabaseCommonStatus databaseCommonStatus, ILoggerFactory loggerFactory)
        {
            var screensDao = new ScreensEntityDao(context, databaseStatementLoader, implementation, loggerFactory);
            if(!screensDao.SelectExistsScreen(screen.DeviceName)) {
                screensDao.InsertScreen(screen, databaseCommonStatus);
                return true;
            }

            return false;
        }

        #endregion
    }
}
