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

namespace PeMain.UI
{
	public partial class Pe
	{
		void ApplyLanguageMainMenu()
		{
			var rootMenu = this._contextMenu.MenuItems;
			rootMenu[menuNameAbout].Text = this._commonData.Language["main/menu/about"];

			var windowMenu = (MenuItem)rootMenu[menuNameWindow];
			windowMenu.Text = this._commonData.Language["main/menu/window"];
			windowMenu.MenuItems[menuNameWindowToolbar].Text = this._commonData.Language["main/menu/window/toolbar"];
			windowMenu.MenuItems[menuNameWindowNote].Text = this._commonData.Language["main/menu/window/note"];
			windowMenu.MenuItems[menuNameWindowLogger].Text = this._commonData.Language["main/menu/window/logger"];
			
			var noteMenu = (MenuItem)windowMenu.MenuItems[menuNameWindowNote];
			noteMenu.MenuItems[menuNameWindowNoteCreate].Text = this._commonData.Language["main/menu/window/note/create"];
			noteMenu.MenuItems[menuNameWindowNoteHidden].Text = this._commonData.Language["main/menu/window/note/hidden"];
			noteMenu.MenuItems[menuNameWindowNoteCompact].Text =this._commonData.Language["main/menu/window/note/compact"]; 

			var systemEnvMenu = (MenuItem)rootMenu[menuNameSystemEnv];
			systemEnvMenu.Text = this._commonData.Language["main/menu/system-env"];
			systemEnvMenu.MenuItems[menuNameSystemEnvHiddenFile].Text = this._commonData.Language["main/menu/system-env/show-hiddne-file"];
			systemEnvMenu.MenuItems[menuNameSystemEnvExtension].Text = this._commonData.Language["main/menu/system-env/show-extension"];
		
			rootMenu[menuNameSetting].Text = this._commonData.Language["main/menu/setting"];
			rootMenu[menuNameExit].Text = this._commonData.Language["common/menu/exit"];
		}
		
		
		void ApplyLanguage()
		{
			Debug.Assert(this._commonData.Language != null);
			
			ApplyLanguageMainMenu();
		}
	}
}
