/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/21
 * 時刻: 0:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PeUtility;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_launcher.
	/// </summary>
	public partial class SettingForm
	{
		void LauncherInputClear()
		{
			this._launcherSelectedItem = null;
			
			var textList = new Control[] {
				this.inputLauncherName,
				this.inputLauncherCommand,
				this.inputLauncherWorkDirPath,
				this.inputLauncherIconPath,
				this.inputLauncherTag,
				this.inputLauncherNote,
			};
			textList.Map(item => item.Text = string.Empty);
			this.inputLauncherIconIndex.Value = 0;
			this.selectLauncherType_file.Checked = true;
			this.selectLauncherType_uri.Checked = false;
		}
		void LauncherSelectItem(LauncherItem item)
		{
			LauncherInputClear();
			this._launcherSelectedItem = item;
			
			this.inputLauncherName.Text = item.Name;
			this.inputLauncherCommand.Text = item.Command;
			this.inputLauncherWorkDirPath.Text = item.WorkDirPath;
			this.inputLauncherIconPath.Text = item.IconPath;
			this.inputLauncherIconIndex.Value = item.IconIndex;
			this.inputLauncherTag.Text = string.Join(", ", item.Tag.ToArray());
			this.inputLauncherNote.Text = item.Note;
		}
		void LauncherSetInputValue(LauncherItem item)
		{
			Debug.Assert(item != null);
			item.Name = this.inputLauncherName.Text;
			
		}
		
	}
}
