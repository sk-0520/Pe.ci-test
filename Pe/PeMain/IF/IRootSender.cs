using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Kind;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// 送信機。
	/// </summary>
	public interface IRootSender
	{
		void ShowBalloon(ToolTipIcon icon, string title, string message);

		void AppendWindow(Form window);

		void ChangeLauncherGroupItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem);

		void ChangeClipboard();

		/// <summary>
		/// ホットキー。
		/// </summary>
		/// <param name="hotKeyId"></param>
		/// <param name="mod"></param>
		/// <param name="key"></param>
		void SendHotKey(HotKeyId hotKeyId, MOD mod, Keys key);
		
		void SendDeviceChanged(ChangeDevice changeDevice);

	}
}
