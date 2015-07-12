namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;

	public interface IAppSender
	{
		/// <summary>
		/// ウィンドウを追加。
		/// </summary>
		/// <param name="window"></param>
		void SendWindowAppend(Window window);
		/// <summary>
		/// ウィンドウを破棄。
		/// </summary>
		/// <param name="window"></param>
		void SendWindowRemove(Window window);
		/// <summary>
		/// デバイスが変更されたことを通知。
		/// </summary>
		/// <param name="changedDevice"></param>
		void SendDeviceChanged(ChangedDevice changedDevice);
		/// <summary>
		/// クリップボードが変更された際に通知。
		/// </summary>
		void SendClipboardChanged();
		/// <summary>
		/// ホットキー。
		/// </summary>
		/// <param name="hotKeyId"></param>
		/// <param name="hotKeyModel"></param>
		void SendHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel);
	}
}
