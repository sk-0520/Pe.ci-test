/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class App
	{
		void ApplyLanguageNoteMenu(ToolStripDropDownItem parentItem)
		{
			var keyItems = new[] {
				new {
					Name = menuNameWindowNoteCreate,
					Lang = "main/menu/window/note/create",
					Key  = this._commonData.MainSetting.Note.CreateHotKey
				},
				new {
					Name = menuNameWindowNoteHidden,
					Lang = "main/menu/window/note/hidden",
					Key  = this._commonData.MainSetting.Note.HiddenHotKey
				},
				new {
					Name = menuNameWindowNoteCompact,
					Lang = "main/menu/window/note/compact",
					Key  = this._commonData.MainSetting.Note.CompactHotKey
				},
				new {
					Name = menuNameWindowNoteShowFront,
					Lang = "main/menu/window/note/show-front",
					Key  = this._commonData.MainSetting.Note.ShowFrontHotKey
				},
			};

			foreach(var keyItem in keyItems) {
				var menuItem = (ToolStripMenuItem)parentItem.DropDownItems[keyItem.Name];
				menuItem.Text = this._commonData.Language[keyItem.Lang];
				if(keyItem.Key.Enabled) {
					menuItem.ShortcutKeyDisplayString = LanguageUtility.HotkeySettingToDisplayText(this._commonData.Language, keyItem.Key);
				}
			}
		}
		
		void ApplyLanguageSystemEnvWindowMenu(ToolStripDropDownItem parentItem)
		{
			parentItem.DropDownItems[menuNameSystemEnvWindowSave].Text =  this._commonData.Language["main/menu/system-env/window/save"];
			parentItem.DropDownItems[menuNameSystemEnvWindowLoad].Text =  this._commonData.Language["main/menu/system-env/window/load"];
		}
		
		void ApplyLanguageSystemEnvMenu(ToolStripDropDownItem parentItem)
		{
			var keyItems = new [] {
				new {
					Name = menuNameSystemEnvHiddenFile,
					Lang = "main/menu/system-env/show-hiddne-file",
					Key  = this._commonData.MainSetting.SystemEnv.HiddenFileShowHotKey
				},
				new {
					Name = menuNameSystemEnvExtension,
					Lang = "main/menu/system-env/show-extension",
					Key  = this._commonData.MainSetting.SystemEnv.ExtensionShowHotKey
				},
				new {
					Name = menuNameSystemEnvClipboard,
					Lang = "main/menu/system-env/clipboard",
					Key  = new HotKeySetting()
				},
			};

			foreach(var keyItem in keyItems) {
				var menuItem = (ToolStripMenuItem)parentItem.DropDownItems[keyItem.Name];
				menuItem.Text = this._commonData.Language[keyItem.Lang];
				if(keyItem.Key.Enabled) {
					menuItem.ShortcutKeyDisplayString = LanguageUtility.HotkeySettingToDisplayText(this._commonData.Language, keyItem.Key);
				}
			}

			// ウィンドウ
			var itemWindow = (ToolStripDropDownItem)parentItem.DropDownItems[menuNameSystemEnvWindow];
			itemWindow.Text = this._commonData.Language["main/menu/system-env/window"];
			ApplyLanguageSystemEnvWindowMenu(itemWindow);
		}
		
		void ApplyLanguageMainMenu()
		{
			var rootMenu = this._contextMenu.Items;

			rootMenu[menuNameWindowToolbar].Text = this._commonData.Language["main/menu/window/toolbar"];
			rootMenu[menuNameWindowNote].Text = this._commonData.Language["main/menu/window/note"];
			rootMenu[menuNameWindowLogger].Text = this._commonData.Language["main/menu/window/logger"];
			rootMenu[menuNameSystemEnv].Text = this._commonData.Language["main/menu/system-env"];
			
			var noteMenu = (ToolStripDropDownItem)rootMenu[menuNameWindowNote];
			ApplyLanguageNoteMenu(noteMenu);

			var systemEnvMenu = (ToolStripDropDownItem)rootMenu[menuNameSystemEnv];
			ApplyLanguageSystemEnvMenu(systemEnvMenu);
			
			rootMenu[menuNameSetting].Text = this._commonData.Language["main/menu/setting"];
			rootMenu[menuNameAbout].Text = this._commonData.Language["main/menu/about"];
			rootMenu[menuNameHelp].Text = this._commonData.Language["main/menu/help"];
			rootMenu[menuNameExit].Text = this._commonData.Language["common/menu/exit"];
		}
		
		void ApplyLanguage()
		{
			Debug.Assert(this._commonData.Language != null);
			
			ApplyLanguageMainMenu();
		}
	}
}
