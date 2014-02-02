/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/06
 * 時刻: 21:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ExToolStrip.
	/// </summary>
	public abstract class ExToolStrip: ToolStrip
	{
	}
	
	public class ToolbarToolStrip: ExToolStrip
	{
		/// <summary>
		/// thanks: http://d.hatena.ne.jp/Kazzz/20061106/p1
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if ( m.Msg == (int)WM.WM_MOUSEACTIVATE && m.Result == (IntPtr)MA.MA_ACTIVATEANDEAT) {
				base.WndProc(ref m);
				m.Result = (IntPtr)MA.MA_ACTIVATE;
			} else {
				base.WndProc(ref m);
			}
		}
	}

}
