/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/16
 * 時刻: 20:56
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.Logic;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	partial class Pe
	{
		public void ShowBalloon(ToolTipIcon icon, string title, string message)
		{
			this._notifyIcon.ShowBalloonTip(0, title, message, icon);
		}
		public void ChangeLauncherItems(ToolbarItem toolbarItem, HashSet<LauncherItem> items)
		{
			throw new NotImplementedException();
		}
		
		public void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key)
		{
			switch(hotKeyId) {
				case HotKeyId.HiddenFile:
					ChangeShowSysEnv(SystemEnv.IsHiddenFileShow, SystemEnv.SetHiddenFileShow, "balloon/hidden-file/title", "balloon/hidden-file/show", "balloon/hidden-file/hide", "balloon/hidden-file/error");
					break;
					
				case HotKeyId.Extension:
					ChangeShowSysEnv(SystemEnv.IsExtensionShow, SystemEnv.SetExtensionShow, "balloon/extension/title", "balloon/extension/show", "balloon/extension/hide", "balloon/extension/error");
					break;
					
				default:
					break;
			}
		}
	}
}
