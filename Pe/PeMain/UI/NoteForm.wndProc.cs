/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 5:20
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of NoteForm.
	/// </summary>
	partial class NoteForm
	{
		protected override void WndProc(ref Message m)
		{
			switch(m.Msg) {
				case (int)WM.WM_NCHITTEST:
					{
						var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
						/*
						var padding = Padding;
						
						var hitTest = HT.HTNOWHERE;
						var captionArea = CommonData.Skin.GetToolbarCaptionArea(UseToolbarItem.ToolbarPosition, Size);
						if(captionArea.Contains(point)) {
							hitTest = HT.HTCAPTION;
						} else {
							var leftArea = new Rectangle(0, 0, padding.Left, Height);
							var rightArea = new Rectangle(Width - padding.Right, 0, padding.Right, Height);
							if(leftArea.Contains(point)) {
								hitTest = HT.HTLEFT;
							} else if(rightArea.Contains(point)) {
								hitTest = HT.HTRIGHT;
							}
						}
						if(hitTest != HT.HTNOWHERE) {
							m.Result = (IntPtr)hitTest;
							return;
						}
						*/
						
						if(!EditMode) {
							var hitTest = HT.HTCAPTION;
							m.Result = (IntPtr)hitTest;
							return;
						}
						break;
					}
					
				default:
					break;
			}
			base.WndProc(ref m);
		}

	}
}
