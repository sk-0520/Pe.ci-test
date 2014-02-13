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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PeUtility;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_launcher.
	/// </summary>
	public partial class SettingForm
	{
		LauncherType LauncherGetSelectedType()
		{
			if(this.selectLauncherType_file.Checked) {
				return LauncherType.File;
			} else {
				Debug.Assert(this.selectLauncherType_uri.Checked);
				return LauncherType.URI;
			}
		}
		
		void LauncherSetSelectedType(LauncherType type)
		{
			this.selectLauncherType_file.Checked = type == LauncherType.File;
			this.selectLauncherType_uri.Checked  = type == LauncherType.URI;
		}
		
		void LauncherInputClear()
		{
			this._launcherSelectedItem = null;
			this._launcherItemEvent = false;
			
			var textList = new Control[] {
				this.inputLauncherName,
				this.inputLauncherCommand,
				this.inputLauncherOption,
				this.inputLauncherWorkDirPath,
				this.inputLauncherIconPath,
				this.inputLauncherTag,
				this.inputLauncherNote,
			};
			textList.ForEach(item => item.Text = string.Empty);
			this.inputLauncherIconPath.Tag = 0;
			LauncherSetSelectedType(LauncherType.File);
			var checkList = new CheckBox[] {
				this.selectLauncherStdStream,
				this.selectLauncherAdmin,
			};
			checkList.ForEach(item => item.Checked = false);
			
			this.envLauncherUpdate.Clear();
			this.envLauncherRemove.Clear();
			
			this._launcherItemEvent = true;
		}
		
		void LauncherSelectItem(LauncherItem item)
		{
			LauncherInputClear();
			this._launcherItemEvent = false;
			
			this._launcherSelectedItem = item;
			
			LauncherSetSelectedType(item.LauncherType);
			this.inputLauncherName.Text = item.Name;
			this.inputLauncherCommand.Text = item.Command;
			this.inputLauncherOption.Text = item.Option;
			this.inputLauncherWorkDirPath.Text = item.WorkDirPath;
			this.inputLauncherIconPath.Text = item.IconPath;
			this.inputLauncherIconPath.Tag = item.IconIndex;
			this.inputLauncherTag.Text = string.Join(", ", item.Tag.ToArray());
			this.inputLauncherNote.Text = item.Note;
			this.selectLauncherStdStream.Checked = item.StdOutputWatch;
			this.selectLauncherAdmin.Checked = item.Administrator;
			this.selectLauncherEnv.Checked = !this.selectLauncherEnv.Checked;
			this.selectLauncherEnv.Checked = item.EnvironmentSetting.EditEnvironment;
			this.envLauncherUpdate.SetItem(item.EnvironmentSetting.Update.ToDictionary(pair => pair.First, pair => pair.Second));
			this.envLauncherRemove.SetItem(item.EnvironmentSetting.Remove);
			
			this._launcherItemEvent = true;
		}
		
		void LauncherInputValueToItem(LauncherItem item)
		{
			Debug.Assert(item != null);
			var oldIcon = new {
				Path = item.IconPath,
				Index= item.IconIndex
			};
			item.LauncherType = LauncherGetSelectedType();
			item.Name = this.inputLauncherName.Text.Trim();
			item.Command = this.inputLauncherCommand.Text.Trim();
			item.Option = this.inputLauncherOption.Text.Trim();
			item.WorkDirPath = this.inputLauncherWorkDirPath.Text.Trim();
			item.IconPath = this.inputLauncherIconPath.Text.Trim();
			item.IconIndex = this.inputLauncherIconPath.Tag != null ? (int)this.inputLauncherIconPath.Tag: 0;
			item.Tag = this.inputLauncherTag.Text.Split(',').Map(s => s.Trim()).ToList();
			item.Note = this.inputLauncherNote.Text.Trim();
			item.StdOutputWatch = this.selectLauncherStdStream.Checked;
			item.Administrator = this.selectLauncherAdmin.Checked;
			item.EnvironmentSetting.EditEnvironment = this.selectLauncherEnv.Checked;
			item.EnvironmentSetting.Update.Clear();
			item.EnvironmentSetting.Update.AddRange(this.envLauncherUpdate.Items);
			item.EnvironmentSetting.Remove.Clear();
			item.EnvironmentSetting.Remove.AddRange(this.envLauncherRemove.Items);

			item.HasError = this.selecterLauncher.Items.Where(i => i != item).Any(i => i.Equals(item));
			if(oldIcon.Index != item.IconIndex || oldIcon.Path != item.IconPath) {
				item.ClearIcon();
			}
		}
		
		bool LauncherItemValid()
		{
			return !this.selecterLauncher.Items.Any(item => item.HasError);
		}
		
		void LauncherOpenIcon()
		{
			var iconPath = Environment.ExpandEnvironmentVariables(this.inputLauncherIconPath.Text.Trim());
			var iconIndex= this.inputLauncherIconPath.Tag != null ? (int)this.inputLauncherIconPath.Tag: 0;
			using(var dialog = new OpenIconDialog()) {
				if(iconPath.Length > 0 && File.Exists(iconPath)) {
					dialog.IconPath  = iconPath;
					dialog.IconIndex = iconIndex;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					this.inputLauncherIconPath.Text = dialog.IconPath;
					this.inputLauncherIconPath.Tag = dialog.IconIndex;
				}
			}
		}
		
		void LauncherAddFile(string filePath)
		{
			var item = LauncherItem.FileLoad(filePath, false);
			var uniqueName = LauncherItem.GetUniqueName(item, this.selecterLauncher.Items);
			item.Name = uniqueName;
			this.selecterLauncher.AddItem(item);
		}
		
		void LauncherInputChange()
		{
			if(this._launcherSelectedItem != null) {
				LauncherInputValueToItem(this._launcherSelectedItem);
				this.selecterLauncher.Refresh();
			}
		}
		
		void LauncherExportSetting(LauncherSetting setting)
		{
			setting.Items.Clear();
			foreach(var item in this.selecterLauncher.Items) {
				setting.Items.Add(item);
			}
		}
	}
}
