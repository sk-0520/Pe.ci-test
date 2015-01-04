using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// 送信機。
	/// </summary>
	public interface IRootSender
	{
		void ShowBalloon(ToolTipIcon icon, string title, string message);
	
		void ChangeLauncherGroupItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem);

		void AppendWindow(Form window);
		
		void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key);
		
		void ReceiveDeviceChanged(ChangeDevice changeDevice);

		//bool EnabledClipboard { get; set; }
		void ChangeClipboard();
	}
}
