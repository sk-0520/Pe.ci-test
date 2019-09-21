using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows.root.CIMV2;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class ScreenOperator
    {
        public ScreenOperator(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        static string DeviceToId(string deviceName)
        {
            return new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
        }

        IEnumerable<Win32_DesktopMonitor> GetScreens(string deviceName)
        {
            string query = "SELECT * FROM Win32_DesktopMonitor";
            if(!string.IsNullOrWhiteSpace(deviceName)) {
                //var id = new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
                var id = DeviceToId(deviceName);
                query = string.Format("SELECT * FROM Win32_DesktopMonitor where DeviceID like \"DesktopMonitor{0}\"", id);
            }
            var q = new ObjectQuery(query);
            ManagementPath path = new ManagementPath("//./root/cimv2");
            var ms = new ManagementScope(path);
            using(var searcher = new ManagementObjectSearcher(ms, q)) {
                foreach(ManagementBaseObject mng in searcher.Get()) {
                    var item = new Win32_DesktopMonitor();
                    try {
                        item.Import(mng);
                    } catch(Exception ex) {
                        Logger.LogWarning(ex, ex.Message);
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
        public string GetName(Screen screen)
        {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            foreach(var screem in GetScreens(screen.DeviceName)) {
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                if(!string.IsNullOrWhiteSpace(screem.Name)) {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                    var id = DeviceToId(screen.DeviceName);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                    return string.Format("{0}. {1}", id, screem.Name);
                }
                break;
            }

            var device = new DISPLAY_DEVICE();
            device.cb = Marshal.SizeOf(device);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            NativeMethods.EnumDisplayDevices(screen.DeviceName, 0, ref device, 1);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。

            //return screen.DeviceName;
            return device.DeviceString;
        }

        public bool RegisterDatabase(Screen screen, IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, IDatabaseCommonStatus databaseCommonStatus)
        {
            var screensDao = new ScreensEntityDao(commander, statementLoader, implementation, LoggerFactory);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            if(!screensDao.SelectExistsScreen(screen.DeviceName)) {
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                return screensDao.InsertScreen(screen, databaseCommonStatus);
            }

            return false;
        }

        #endregion
    }
}
