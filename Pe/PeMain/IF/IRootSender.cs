/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 2:01
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System.Windows.Forms;
using PeMain.Data;
using PInvoke.Windows;

namespace PeMain.IF
{
	/// <summary>
	/// 送信機。
	/// </summary>
	public interface IRootSender
	{
		void ShowBalloon(ToolTipIcon icon, string title, string message);
	
		void ChangeLauncherGroupItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem);
		
		void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key);
		
		void ReceiveDeviceChanged(ChangeDevice changeDevice);
	}
}
