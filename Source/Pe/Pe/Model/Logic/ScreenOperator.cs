using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.PInvoke.Windows.root.CIMV2;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public class ScreenOperator
    {
        public ScreenOperator(ILogger logger)
        {
            Logger = logger;
        }

        public ScreenOperator(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region function

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
            using(var searcher = new ManagementObjectSearcher(query)) {
                foreach(ManagementBaseObject mng in searcher.Get()) {
                    var item = new Win32_DesktopMonitor();
                    try {
                        item.Import(mng);
                    } catch(Exception ex) {
                        Logger.Warning(ex);
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
            foreach(var screem in GetScreens(screen.DeviceName)) {
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

        public bool RegisterDatabase(Screen screen, IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, IDatabaseCommonStatus databaseCommonStatus)
        {
            var screensDao = new ScreensEntityDao(commander, statementLoader, implementation, Logger.Factory);
            if(!screensDao.SelectExistsScreen(screen.DeviceName)) {
                return screensDao.InsertScreen(screen, databaseCommonStatus);
            }

            return false;
        }

        #endregion
    }
}
