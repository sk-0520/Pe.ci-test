/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/11/01
 * 時刻: 2:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;

namespace PeMain.UI
{
	public abstract class ExContextMenuStrip: ContextMenuStrip
	{ }
	
	/// <summary>
	/// コンテキストメニューで使用。
	/// </summary>
	public sealed class AppContextMenuStrip: ExContextMenuStrip
	{
		/// <summary>
		/// メニューは表示されているか。
		/// </summary>
		public bool ShowContextMenu { get; private set; }
		
		protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
		{
			base.OnOpening(e);
			if(!e.Cancel) {
				ShowContextMenu = true;
			}
		}
		
		protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
		{
			base.OnClosing(e);
			if(!e.Cancel) {
				ShowContextMenu = false;
			}
		}
	}
}
