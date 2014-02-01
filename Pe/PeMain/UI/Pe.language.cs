﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;

namespace PeMain.UI
{
	public partial class Pe
	{
		void ApplyLanguage()
		{
			Debug.Assert(this._language != null);
			
			var rootMenu = this._notificationMenu.MenuItems;
			rootMenu[menuNameAbout].Text = this._language["main/menu/about"];
			var windowMenu = rootMenu[menuNameWindow];
			windowMenu.Text = this._language["main/menu/window"];
			windowMenu.MenuItems[menuNameWindowToolbar].Text = this._language["main/menu/window/toolbar"];
			windowMenu.MenuItems[menuNameWindowLogger].Text = this._language["main/menu/window/logger"];
		
			rootMenu[menuNameSetting].Text = this._language["main/menu/setting"];
			rootMenu[menuNameExit].Text = this._language["common/menu/exit"];
			
		}
	}
}
