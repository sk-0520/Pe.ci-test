/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/17
 * 時刻: 20:09
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class SettingForm
	{
		void ExportCommandSetting(CommandSetting commandSetting)
		{
			/*
			commandSetting.HotKey.Key = this.inputCommandHotkey.Hotkey;
			commandSetting.HotKey.Modifiers = this.inputCommandHotkey.Modifiers;
			commandSetting.HotKey.Registered = this.inputCommandHotkey.Registered;
			 */
			commandSetting.HotKey = this.inputCommandHotkey.HotKeySetting;
			
			commandSetting.FontSetting = this.commandCommandFont.FontSetting;
		}
		
		void ExportLauncherSetting(LauncherSetting setting)
		{
			setting.Items.Clear();
			foreach(var item in this.selecterLauncher.Items) {
				setting.Items.Add(item);
			}
		}
		void ExportLogSetting(LogSetting logSetting)
		{
			logSetting.Visible = this.selectLogVisible.Checked;
			logSetting.AddShow = this.selectLogAddShow.Checked;
			logSetting.FullDetail = this.selectLogFullDetail.Checked;
			
			var trigger = new Dictionary<CheckBox, LogType>() {
				{ this.selectLogTrigger_information, LogType.Information },
				{ this.selectLogTrigger_warning,     LogType.Warning },
				{ this.selectLogTrigger_error,       LogType.Error },
			};
			var logType = LogType.None;
			foreach(var t in trigger) {
				if(t.Key.Checked) {
					logType |= t.Value;
				}
			}
			logSetting.AddShowTrigger = logType;
		}	
		
		void ExportSystemEnvSetting(SystemEnvSetting systemEnvSetting)
		{
			/*
			systemEnvSetting.HiddenFileShowHotKey.Key = this.inputSystemEnvHiddenFile.Hotkey;
			systemEnvSetting.HiddenFileShowHotKey.Modifiers = this.inputSystemEnvHiddenFile.Modifiers;
			systemEnvSetting.HiddenFileShowHotKey.Registered = this.inputSystemEnvHiddenFile.Registered;
			
			systemEnvSetting.ExtensionShowHotKey.Key = this.inputSystemEnvExt.Hotkey;
			systemEnvSetting.ExtensionShowHotKey.Modifiers = this.inputSystemEnvExt.Modifiers;
			systemEnvSetting.ExtensionShowHotKey.Registered = this.inputSystemEnvExt.Registered;
			 */
			systemEnvSetting.HiddenFileShowHotKey = this.inputSystemEnvHiddenFile.HotKeySetting;
			systemEnvSetting.ExtensionShowHotKey  = this.inputSystemEnvExt.HotKeySetting;
		}
		
		void ExportRunningInfoSetting(RunningInfo setting)
		{
			setting.CheckUpdate = this.selectUpdateCheck.Checked;
			setting.CheckUpdateRC = this.selectUpdateCheckRC.Checked;
		}
		
		void ExportLanguageSetting(MainSetting  setting)
		{
			var lang = this.selectMainLanguage.SelectedValue as Language;
			if(lang != null) {
				setting.LanguageName = lang.BaseName;
			}
		}
		
		void ExportMainSetting(MainSetting mainSetting)
		{
			ExportLogSetting(mainSetting.Log);
			ExportSystemEnvSetting(mainSetting.SystemEnv);
			ExportRunningInfoSetting(mainSetting.RunningInfo);
			
			ExportLanguageSetting(mainSetting);
		}
		
		void ExportNoteSetting(NoteSetting noteSetting)
		{
			// ホットキー
			noteSetting.CreateHotKey = this.inputNoteCreate.HotKeySetting;
			noteSetting.HiddenHotKey = this.inputNoteHidden.HotKeySetting;
			noteSetting.CompactHotKey= this.inputNoteCompact.HotKeySetting;
			noteSetting.ShowFrontHotKey= this.inputNoteShowFront.HotKeySetting;
			
			// フォント
			noteSetting.CaptionFontSetting = this.commandNoteCaptionFont.FontSetting;
		}
		
		void ExportToolbarSetting(ToolbarSetting toolbarSetting)
		{
			ToolbarSetSelectedItem(this._toolbarSelectedToolbarItem);
			foreach(var itemData in this.selectToolbarItem.Items.Cast<ToolbarDisplayValue>()) {
				var item = itemData.Value;
				if(toolbarSetting.Items.Contains(item)) {
					toolbarSetting.Items.Remove(item);
				}
				toolbarSetting.Items.Add(item);
			}
			
			// ツリーからグループ項目構築
			foreach(TreeNode groupNode in this.treeToolbarItemGroup.Nodes) {
				var toolbarGroupItem = new ToolbarGroupItem();
				
				// グループ項目
				var groupName = groupNode.Text;
				toolbarGroupItem.Name = groupName;
				
				// グループに紐付くアイテム名
				toolbarGroupItem.ItemNames.AddRange(groupNode.Nodes.Cast<TreeNode>().Select(node => node.Text));

				toolbarSetting.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
		}
	}
}
