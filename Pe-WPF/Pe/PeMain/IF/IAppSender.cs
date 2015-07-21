namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;

	public interface IAppSender
	{
		/// <summary>
		/// ウィンドウを追加。
		/// </summary>
		/// <param name="window"></param>
		void SendAppendWindow(Window window);
		/// <summary>
		/// ウィンドウを破棄。
		/// </summary>
		/// <param name="window"></param>
		void SendRemoveWindow(Window window);
		/// <summary>
		/// 対象インデックスから指定IDを削除。
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="indexKind"></param>
		void SendRemoveIndex(IndexKind indexKind, Guid guid);
		/// <summary>
		/// 対象インデックスを保存。
		/// </summary>
		/// <param name="indexKind"></param>
		void SendSaveIndex(IndexKind indexKind);
		/// <summary>
		/// 対象インデックスのボディ部を取得。
		/// </summary>
		/// <param name="indexKind"></param>
		/// <param name="guid"></param>
		/// <returns></returns>
		IndexBodyItemModelBase SendGetIndexBody(IndexKind indexKind, Guid guid);
		/// <summary>
		/// 対象インデックスのボディ部を保存。
		/// </summary>
		/// <param name="indexBody"></param>
		/// <param name="guid"></param>
		void SendSaveIndexBody(IndexBodyItemModelBase indexBody, Guid guid);
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
		void SendInputHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel);
	}
}
