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
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	partial class App
	{
		public void ShowBalloon(ToolTipIcon icon, string title, string message)
		{
			this._notifyIcon.ShowBalloonTip(0, title, message, icon);
		}
		
		public void ChangeLauncherGroupItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem)
		{
			foreach(var toolbar in this._toolbarForms.Values.Where(t => t.UseToolbarItem != toolbarItem)) {
				toolbar.ReceiveChangedLauncherItems(toolbarItem, toolbarGroupItem);
			}
		}
		
	}
}
