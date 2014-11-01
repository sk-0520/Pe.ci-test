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

using PeMain.Logic;

namespace PeMain.UI
{
	partial class App
	{
		void ApplyLanguageNoteMenu(ToolStripMenuItem parentItem)
		{
			parentItem.DropDownItems[menuNameWindowNoteCreate].Text = this._commonData.Language["main/menu/window/note/create"];
			parentItem.DropDownItems[menuNameWindowNoteHidden].Text = this._commonData.Language["main/menu/window/note/hidden"];
			parentItem.DropDownItems[menuNameWindowNoteCompact].Text =this._commonData.Language["main/menu/window/note/compact"];
			parentItem.DropDownItems[menuNameWindowNoteShowFront].Text =this._commonData.Language["main/menu/window/note/show-front"];
		}
		
		void ApplyLanguageSystemEnvWindowMenu(ToolStripMenuItem parentItem)
		{
			parentItem.DropDownItems[menuNameSystemEnvWindowSave].Text =  this._commonData.Language["main/menu/system-env/window/save"];
			parentItem.DropDownItems[menuNameSystemEnvWindowLoad].Text =  this._commonData.Language["main/menu/system-env/window/load"];
		}
		
		void ApplyLanguageSystemEnvMenu(ToolStripMenuItem parentItem)
		{
			parentItem.DropDownItems[menuNameSystemEnvHiddenFile].Text = this._commonData.Language["main/menu/system-env/show-hiddne-file"];
			parentItem.DropDownItems[menuNameSystemEnvExtension].Text = this._commonData.Language["main/menu/system-env/show-extension"];
			
			// ウィンドウ
			var itemWindow = (ToolStripMenuItem)parentItem.DropDownItems[menuNameSystemEnvWindow];
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
			
			var noteMenu = (ToolStripMenuItem)rootMenu[menuNameWindowNote];
			ApplyLanguageNoteMenu(noteMenu);

			var systemEnvMenu = (ToolStripMenuItem)rootMenu[menuNameSystemEnv];
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
