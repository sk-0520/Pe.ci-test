/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/21
 * 時刻: 0:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_launcher.
	/// </summary>
	partial class SettingForm
	{
		LauncherType LauncherGetSelectedType()
		{
			var map = new Dictionary<RadioButton, LauncherType>() {
				{ this.selectLauncherType_file, LauncherType.File },
				{ this.selectLauncherType_directory, LauncherType.Directory },
				{ this.selectLauncherType_uri, LauncherType.URI },
				{ this.selectLauncherType_embedded, LauncherType.Embedded },
			};
			return map.Single(m => m.Key.Checked).Value;
			/*
			if(this.selectLauncherType_file.Checked) {
				return LauncherType.File;
			} else {
				Debug.Assert(this.selectLauncherType_uri.Checked);
				return LauncherType.URI;
			}
			 */
		}
		
		void LauncherSetSelectedType(LauncherType type)
		{
			this.selectLauncherType_file.Checked = type == LauncherType.File;
			this.selectLauncherType_directory.Checked  = type == LauncherType.Directory;
			this.selectLauncherType_uri.Checked  = type == LauncherType.URI;
			this.selectLauncherType_embedded.Checked  = type == LauncherType.Embedded;
			
			LauncherApplyType(type);
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
			foreach(var text in textList) {
				text.Text = string.Empty;
			}
			this.inputLauncherIconPath.IconIndex = 0;
			LauncherSetSelectedType(LauncherType.File);
			var checkList = new CheckBox[] {
				this.selectLauncherStdStream,
				this.selectLauncherAdmin,
			};
			foreach(var check in checkList) {
				check.Checked = false;
			}
			
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
			/*
			this.inputLauncherIconPath.Text = item.IconPath;
			this.inputLauncherIconPath.Tag = item.IconIndex;
			 */
			this.inputLauncherIconPath.Text = item.IconItem.Path;
			//this.inputLauncherIconPath.Tag = item.IconItem.Index;
			this.inputLauncherIconPath.IconIndex = item.IconItem.Index;
			
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
			/*
			var oldIcon = new {
				Path = item.IconPath,
				Index= item.IconIndex
			};
			 */
			var oldIcon = new IconItem(item.IconItem.Path, item.IconItem.Index);
			item.LauncherType = LauncherGetSelectedType();
			item.Name = this.inputLauncherName.Text.Trim();
			item.Command = this.inputLauncherCommand.Text.Trim();
			item.Option = this.inputLauncherOption.Text.Trim();
			item.WorkDirPath = this.inputLauncherWorkDirPath.Text.Trim();
			/*
			item.IconPath = this.inputLauncherIconPath.Text.Trim();
			item.IconIndex = this.inputLauncherIconPath.Tag != null ? (int)this.inputLauncherIconPath.Tag: 0;
			 */
			item.IconItem.Path = this.inputLauncherIconPath.Text.Trim();
			item.IconItem.Index = this.inputLauncherIconPath.IconIndex;
			
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
			
			if(!oldIcon.Equals(item)) {
				item.ClearIcon();
			}
			
			LauncherApplyType(item.LauncherType);
		}
		
		void LauncherApplyType(LauncherType type)
		{
			var enabledControls = new Control [] {
				this.commandLauncherFilePath,
				this.commandLauncherDirPath,
				this.commandLauncherOptionDirPath,
				this.commandLauncherOptionFilePath,
				this.commandLauncherWorkDirPath,
				this.commandLauncherIconPath,
				this.inputLauncherName,
				this.inputLauncherCommand,
				this.inputLauncherOption,
				this.inputLauncherWorkDirPath,
				this.inputLauncherIconPath,
				this.inputLauncherTag,
				this.inputLauncherNote,
				this.selectLauncherStdStream,
				this.selectLauncherAdmin,
				this.selectLauncherEnv,
				this.envLauncherUpdate,
				this.envLauncherRemove,
			};
			var disabledControls = new Control[]{};
			switch(type) {
				case LauncherType.File:
					break;
					
				case LauncherType.Directory:
					{
						disabledControls = new Control[] {
							this.commandLauncherFilePath,
							this.commandLauncherOptionFilePath,
							this.commandLauncherOptionDirPath,
							this.commandLauncherWorkDirPath,
							this.inputLauncherOption,
							this.inputLauncherWorkDirPath,
							this.selectLauncherStdStream,
							this.selectLauncherAdmin,
							this.selectLauncherEnv,
							this.envLauncherUpdate,
							this.envLauncherRemove,
						};
					}
					break;
					
				case LauncherType.URI:
					{
						disabledControls = new Control[] {
							this.commandLauncherFilePath,
							this.commandLauncherDirPath,
							this.commandLauncherOptionFilePath,
							this.commandLauncherOptionDirPath,
							this.commandLauncherWorkDirPath,
							this.inputLauncherOption,
							this.inputLauncherWorkDirPath,
							this.selectLauncherStdStream,
							this.selectLauncherAdmin,
							this.selectLauncherEnv,
							this.envLauncherUpdate,
							this.envLauncherRemove,
						};
					}
					break;
					
				case LauncherType.Embedded:
					Debug.Assert(false, type.ToString());
					break;
			}
			
			SuspendLayout();
			
			foreach(var control in enabledControls) {
				control.Enabled = true;
			}
			if(disabledControls != null) {
				foreach(var control in disabledControls) {
					control.Enabled = false;
				}
			}
			
			ResumeLayout(false);
		}
		
		bool LauncherItemValid()
		{
			if(!this.selecterLauncher.Items.Any(item => item.HasError)) {
				return true;
			} else {
				this.errorProvider.SetError(this.selecterLauncher, Language["setting/check/item-name-dup"]);
				return false;
			}
		}
		
		void LauncherOpenIcon()
		{
			var iconPath = Environment.ExpandEnvironmentVariables(this.inputLauncherIconPath.Text.Trim());
			var iconIndex= this.inputLauncherIconPath.IconIndex;
			using(var dialog = new OpenIconDialog()) {
				if(iconPath.Length > 0 && File.Exists(iconPath)) {
					dialog.IconPath.Path  = iconPath;
					dialog.IconPath.Index = iconIndex;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					this.inputLauncherIconPath.Text = dialog.IconPath.Path;
					this.inputLauncherIconPath.IconIndex = dialog.IconPath.Index;
				}
			}
		}
		
		void LauncherAddFile(string filePath)
		{
			var path = filePath;
			var useShortcut = false;
			// TODO: 処理重複
			if(PathUtility.IsShortcutPath(filePath)) {
				var result = MessageBox.Show(Language["common/dialog/d-d/shortcut/message"], Language["common/dialog/d-d/shortcut/caption"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				switch(result) {
					case DialogResult.Yes:
						try {
							var sf = new ShortcutFile(filePath, false);
							path = sf.TargetPath;
						} catch(ArgumentException ex) {
							Debug.WriteLine(ex);
						}
						break;
						
					case DialogResult.No:
						useShortcut = true;
						break;
						
					default:
						return;
				}
			}
			var item = LauncherItem.LoadFile(path, useShortcut);
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
	}
}
