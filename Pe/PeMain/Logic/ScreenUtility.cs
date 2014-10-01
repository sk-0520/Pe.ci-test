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
using System.Windows.Forms;

namespace PeMain.Logic
{
	/// <summary>
	/// スクリーン共通処理。
	/// </summary>
	public static class ScreenUtility
	{
		/// <summary>
		/// スクリーンの名前を取得。
		/// 
		/// TODO: 名称がダサい。
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static string ToScreenName(Screen screen)
		{
			/*
			using(var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor")) {
				foreach(var currentObj in searcher.Get()) {
					String name = currentObj["Name"].ToString();
					String device_id = currentObj["DeviceID"].ToString();
					Debug.WriteLine("{0} - {1}, {2}", name, device_id.Length, currentObj);
				}
			}
			*/
			return screen.DeviceName;
		}
		public static string ToScreenName(string screenName)
		{
			var screen = Screen.AllScreens.SingleOrDefault(s => s.DeviceName == screenName);
			if(screen != null) {
				ToScreenName(screen);
			}
			return screenName;
		}
		
		public static Screen GetCurrent()
		{
			return Screen.FromPoint(Cursor.Position);
		}
	}
}
