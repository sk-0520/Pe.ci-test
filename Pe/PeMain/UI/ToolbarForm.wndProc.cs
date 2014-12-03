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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class ToolbarForm
	{
		protected override void WndProc(ref Message m)
		{
			if(UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				switch(m.Msg) {
					case (int)WM.WM_SYSCOMMAND:
						{
							switch (m.WParam.ToInt32() & 0xfff0) {
								case (int)SC.SC_MINIMIZE:
								case (int)SC.SC_MAXIMIZE:
								case (int)SC.SC_RESTORE:
									return;
								default:
									break;
							}
						}
						break;
						
					case (int)WM.WM_NCPAINT:
						{
							if(CommonData != null) {
								var hDC = NativeMethods.GetWindowDC(Handle);
								try {
									using(var g = Graphics.FromHdc(hDC)) {
										DrawNoClient(g, new Rectangle(Point.Empty, Size), this == Form.ActiveForm);
									}
								} finally {
									NativeMethods.ReleaseDC(Handle, hDC);
								}
							}
						}
						break;
						
					case (int)WM.WM_NCHITTEST:
						{
							
							var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
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
						}
						break;
						
					case (int)WM.WM_SETCURSOR:
						{
							if(!this._menuOpening) {
								var hittest = WindowsUtility.HTFromLParam(m.LParam);
								if(hittest == HT.HTCAPTION) {
									NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, IDC.IDC_SIZEALL));
									return;
								}
							}
						}
						break;
						
						/*
					case (int)WM.WM_CONTEXTMENU:
						{
							//Debug.WriteLine(m.WParam);
							//NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, IDC.IDC_ARROW));
						}
						//return;
						break;
						 */
						
					case (int)WM.WM_MOVING:
						{
							var rect = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
							var workingArea = DockScreen.WorkingArea;
							
							if(rect.X < workingArea.X) {
								// 左
								rect.X = workingArea.X;
							} else if(rect.Right > workingArea.Right) {
								// 右
								rect.X = workingArea.Right - rect.Width;
							}
							
							if(rect.Y < workingArea.Y) {
								// 上
								rect.Y = workingArea.Y;
							} else if(rect.Bottom > workingArea.Bottom) {
								// 下
								rect.Y = workingArea.Bottom - rect.Height;
							}
							
							Marshal.StructureToPtr(rect, m.LParam, false);
						}
						break;
						
					case (int)WM.WM_DWMCOMPOSITIONCHANGED:
						{
							CommonData.Skin.Refresh(this);
						}
						break;
				}
			}
			base.WndProc(ref m);
		}
	}
}
