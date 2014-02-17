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
		void ApplyLanguage()
		{
			Debug.Assert(this._commonData.Language != null);
			
			var rootMenu = this._notificationMenu.Items;
			rootMenu[menuNameAbout].Text = this._commonData.Language["main/menu/about"];

			var windowMenu = (ToolStripMenuItem)rootMenu[menuNameWindow];
			windowMenu.Text = this._commonData.Language["main/menu/window"];
			windowMenu.DropDownItems[menuNameWindowToolbar].Text = this._commonData.Language["main/menu/window/toolbar"];
			windowMenu.DropDownItems[menuNameWindowLogger].Text = this._commonData.Language["main/menu/window/logger"];

			var systemEnvMenu = (ToolStripMenuItem)rootMenu[menuNameSystemEnv];
			systemEnvMenu.Text = this._commonData.Language["main/menu/system-env"];
			systemEnvMenu.DropDownItems[menuNameSystemEnvHiddenFile].Text = this._commonData.Language["main/menu/system-env/show-hiddne-file"];
			systemEnvMenu.DropDownItems[menuNameSystemEnvExtension].Text = this._commonData.Language["main/menu/system-env/show-extension"];
		
			rootMenu[menuNameSetting].Text = this._commonData.Language["main/menu/setting"];
			rootMenu[menuNameExit].Text = this._commonData.Language["common/menu/exit"];
			
		}
	}
}
