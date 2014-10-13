/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/13
 * 時刻: 11:33
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeMain.IF
{
	/// <summary>
	/// 解放が必要なアイテムのインターフェイス。
	/// </summary>
	public interface IDisposableItem: IDisposable
	{
		bool IsDisposed { get; set; }
	}

	public interface INameItem
	{
		/// <summary>
		/// 名前
		/// </summary>
		string Name { get; set; }
	}
}
