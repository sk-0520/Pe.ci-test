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
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using System.Diagnostics;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class App
	{
		/// <summary>
		/// Description of Pe_MessageWindow_wndProc.
		/// </summary>
		partial class MessageWindow
		{
			protected override void WndProc(ref Message m) {
				switch(m.Msg) {
					case (int)WM.WM_HOTKEY:
						{
							var id = (HotKeyId)m.WParam;
							var mod = (MOD)unchecked((short)(long)m.LParam);
							var key = (Keys)unchecked((ushort)((long)m.LParam >> 16));
							CommonData.RootSender.ReceiveHotKey(id, mod, key);
						}
						break;
						
					case (int)WM.WM_DEVICECHANGE:
						{
							var changeDevice = new ChangeDevice(m);
							CommonData.RootSender.ReceiveDeviceChanged(changeDevice);
						}
						break;
						/*
					case (int)WM.WM_CHANGECBCHAIN: 
						{
							if(m.WParam == NextWndHandle) {
								NextWndHandle = m.LParam;
							} else if(NextWndHandle != null) {
								NativeMethods.SendMessage(NextWndHandle, (WM)m.Msg, m.WParam, m.LParam);
							}
						}
						break;
						
					case (int)WM.WM_DRAWCLIPBOARD: 
						{
							//if(CommonData.RootSender.EnabledClipboard) {
							CommonData.RootSender.ChangeClipboard();
							//}
							if(NextWndHandle != null) {
								NativeMethods.SendMessage(NextWndHandle, (WM)m.Msg, m.WParam, m.LParam);
							}
						}
						break;
						*/
					case (int)WM.WM_CLIPBOARDUPDATE: 
						{
							CommonData.RootSender.ChangeClipboard();
						}
						break;

					default:
						break;
				}
				base.WndProc(ref m);
			}
		}
	}
}
