/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/21
 * 時刻: 0:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_launcher.
	/// </summary>
	public partial class SettingForm
	{
		void LauncherSelectItem(LauncherItem item)
		{
			this.inputLauncherName.Text = item.Name;
			this._launcherSelectedItem = item;
		}
	}
}
