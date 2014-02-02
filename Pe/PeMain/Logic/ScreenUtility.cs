/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/02
 * 時刻: 17:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Linq;
using System.Windows.Forms;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of ScreenUtility.
	/// </summary>
	public static class ScreenUtility
	{
		public static string ToScreenName(Screen screen) 
		{
			// TODO: ディスプレイ名称
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
	}
}
