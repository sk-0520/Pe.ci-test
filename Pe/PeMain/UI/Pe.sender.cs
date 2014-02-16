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
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	partial class Pe
	{
		public void ShowTips(string tezt)
		{
			throw new NotImplementedException();
		}
		public void ChangeLauncherItems(ToolbarItem toolbarItem, HashSet<LauncherItem> items)
		{
			throw new NotImplementedException();
		}
		
		public void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key)
		{
			Debug.WriteLine("{0}, {1}, {2}", hotKeyId, mod, key);
		}
	}
}
