/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/02
 * 時刻: 17:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PeMain.Data;
using PInvoke.Windows;
using PInvoke.Windows.root.CIMV2;

namespace PeMain.Logic
{
	/// <summary>
	/// スクリーン共通処理。
	/// </summary>
	public static class ScreenUtility
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static string ToScreenName(Screen screen)
		{
			return ToScreenName(screen, null);
		}
		
		/// <summary>
		/// スクリーンの名前を取得。
		/// 
		/// TODO: 名称がダサい。
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static string ToScreenName(Screen screen, ILogger logger)
		{
			var id = new string(screen.DeviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
			var query = string.Format("SELECT * FROM Win32_DesktopMonitor where DeviceID like \"DesktopMonitor{0}\"", id);
			using(var searcher = new ManagementObjectSearcher(query)) {
				foreach(ManagementObject mng in searcher.Get()) {
					try {
						var item = new Win32_DesktopMonitor();
						item.Import(mng);
						if(!string.IsNullOrWhiteSpace(item.Name)) {
							return item.Name;
						}
					} catch(Exception ex) {
						if(logger != null) {
							logger.Puts(LogType.Warning, ex.Message, ex);
						} else {
							Debug.WriteLine(ex);
						}
					}
					break;
				}
			}
			
			//*/
			var device = new DISPLAY_DEVICE();
			device.cb = Marshal.SizeOf(device);
			API.EnumDisplayDevices(screen.DeviceName, 0, ref device, 1);
			
			//return screen.DeviceName;
			return device.DeviceString;
		}
		public static string ToScreenName(string screenName, ILogger logger)
		{
			var screen = Screen.AllScreens.SingleOrDefault(s => s.DeviceName == screenName);
			if(screen != null) {
				ToScreenName(screen, logger);
			}
			return screenName;
		}
		
		public static Screen GetCurrent()
		{
			return Screen.FromPoint(Cursor.Position);
		}
	}
}
