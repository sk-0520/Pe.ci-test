namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// 送信機。
	/// </summary>
	public interface IRootSender: IClipboardWatcher
	{
		/// <summary>
		/// バルーンの表示。
		/// </summary>
		/// <param name="icon">バルーンに表示するアイコン。</param>
		/// <param name="title">バルーンに表示するタイトル。</param>
		/// <param name="message">バルーンに表示するメッセージ。</param>
		void ShowBalloon(ToolTipIcon icon, string title, string message);

		/// <summary>
		/// 管理対象ウィンドウを追加。
		/// </summary>
		/// <param name="window"></param>
		void AppendWindow(Form window);

		/// <summary>
		/// Launcherのグループ内容が変更された。
		/// </summary>
		/// <param name="toolbarItem">送信ツールバー。</param>
		/// <param name="toolbarGroupItem">変更されたグループ。</param>
		void ChangedLauncherGroupItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem);

		/// <summary>
		/// クリップボードが変更された。
		/// </summary>
		void ChangedClipboard();

		/// <summary>
		/// ホットキーを送信。
		/// </summary>
		/// <param name="hotKeyId"></param>
		/// <param name="mod"></param>
		/// <param name="key"></param>
		void SendHotKey(HotKeyId hotKeyId, MOD mod, Keys key);
		
		/// <summary>
		/// デバイス情報が変更された。
		/// </summary>
		/// <param name="changedDevice"></param>
		void SendDeviceChanged(ChangedDevice changedDevice);
	}
}
