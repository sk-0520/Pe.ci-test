/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;

namespace PeMain.UI
{
	/// <summary>
	/// </summary>
	public partial class AppbarForm
	{
		void Initialize()
		{
			BarSize = Size;
			DockScreen = Screen.PrimaryScreen;
			DockType = DockType.None;
			IsDocking = false;
			MessageString = "AppDesktopToolbar";
		}
	}
}
