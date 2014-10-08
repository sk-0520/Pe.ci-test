/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/26
 * 時刻: 20:32
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PInvoke.Windows;

namespace PeMain.UI
{
	partial class Pe
	{
		/// <summary>
		/// Description of Pe_MessageWindow_wndProc.
		/// </summary>
		partial class MessageWindow
		{
			protected override void WndProc(ref Message m) {
				if(m.Msg == (int)WM.WM_HOTKEY) {
					var id = (HotKeyId)m.WParam;
					var mod = (MOD)unchecked((short)(long)m.LParam);
					var key = (Keys)unchecked((ushort)((long)m.LParam >> 16));
					CommonData.RootSender.ReceiveHotKey(id, mod, key);
				}
				base.WndProc(ref m);
			}
		}
	}
}
