/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/16
 * 時刻: 20:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PI.Windows;

namespace PeMain.UI
{
	public partial class ToolbarForm
	{
		protected override void WndProc(ref Message m)
		{
			if(UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				if(m.Msg == (int)WM.WM_NCHITTEST) {
					var point = PointToClient(
						new Point(
							(int)(m.LParam.ToInt64() & 0xFFFF),
							(int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16)
						)
					);
					var padding = Padding;

					var hitTest = HT.HTNOWHERE;
					var captionArea = GetCaptionArea(UseToolbarItem.ToolbarPosition);
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
				}
			}
			base.WndProc(ref m);
		}
	}
}
