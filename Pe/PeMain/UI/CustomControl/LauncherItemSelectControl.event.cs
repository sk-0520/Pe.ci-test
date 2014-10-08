/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 1:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;

namespace PeMain.UI
{
	public abstract class ItemEventArgs: EventArgs
	{
		public LauncherItem Item { get; set; }
	}
	public class CreateItemEventArg: ItemEventArgs
	{
	}
	
	public class RemovedItemEventArg: ItemEventArgs
	{
	}
	
	public class SelectedItemEventArg: ItemEventArgs
	{
	}
	
	/// <summary>
	/// Description of LauncherItemSelectControl_event.
	/// </summary>
	partial class LauncherItemSelectControl
	{
		public event EventHandler<CreateItemEventArg> CreateItem;
		public event EventHandler<RemovedItemEventArg> RemovedItem;
		public event EventHandler<SelectedItemEventArg> SelectChangedItem;
	}
}
