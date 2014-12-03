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
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeSkin;
using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of NoteForm.
	/// </summary>
	partial class NoteForm
	{
		struct HitState
		{
			private const uint leftBit = 0x0001;
			private const uint rightBit = 0x0002;
			private const uint topBit = 0x0004;
			private const uint bottomBit = 0x0008;
			
			private uint _flag;
			
			public bool HasTrue { get { return this._flag != 0; } }
			
			private bool Get(uint bit)
			{
				return (this._flag & bit) == bit;
			}
			private void Set(uint bit, bool value)
			{
				if(value) {
					this._flag |= bit;
				} else {
					this._flag &= ~(this._flag & bit);
				}
			}

			public bool Left
			{
				get { return Get(leftBit); }
				set { Set(leftBit, value); }
			}
			public bool Right
			{
				get { return Get(rightBit); }
				set { Set(rightBit, value); }
			}
			public bool Top
			{
				get { return Get(topBit); }
				set { Set(topBit, value); }
			}
			public bool Bottom
			{
				get { return Get(bottomBit); }
				set { Set(bottomBit, value); }
			}
		}
		protected override void WndProc(ref Message m)
		{
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
						break;
					}
					
				case (int)WM.WM_NCPAINT:
					{
						//if(CommonData != null && (!this.inputBody.Visible || !this.inputTitle.Visible)) {
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
						if(!NoteItem.Locked) {
							var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
							var hitTest = HT.HTNOWHERE;
							
							var edgePadding = CommonData.Skin.GetNoteWindowEdgePadding();
							
							var noteArea = new Rectangle(Point.Empty, Size);
							Rectangle area;
							var pos = new HitState();
							// 上
							area = noteArea;
							area.Height = edgePadding.Top;
							pos.Top = area.Contains(point);
							// 下
							area = noteArea;
							area.Y = noteArea.Height - edgePadding.Bottom;
							area.Height = edgePadding.Bottom;
							pos.Bottom = area.Contains(point);
							// 左
							area = noteArea;
							area.Width = edgePadding.Left;
							pos.Left = area.Contains(point);
							// 右
							area = noteArea;
							area.X = noteArea.Width - edgePadding.Right;
							area.Width = edgePadding.Right;
							pos.Right = area.Contains(point);
							
							if(pos.HasTrue && !NoteItem.Compact) {
								if(pos.Left) {
									if(pos.Top) {
										hitTest = HT.HTTOPLEFT;
									} else if(pos.Bottom) {
										hitTest = HT.HTBOTTOMLEFT;
									} else {
										hitTest = HT.HTLEFT;
									}
								} else if(pos.Right) {
									if(pos.Top) {
										hitTest = HT.HTTOPRIGHT;
									} else if(pos.Bottom) {
										hitTest = HT.HTBOTTOMRIGHT;
									} else {
										hitTest = HT.HTRIGHT;
									}
								} else if(pos.Top) {
									hitTest = HT.HTTOP;
								} else if(pos.Bottom) {
									hitTest = HT.HTBOTTOM;
								}
							} else {
								var throwHittest = true;
								DrawCommand(
									point,
									(isIn, nowState) => {
										if(isIn) {
											throwHittest = false;
											if(nowState == SkinButtonState.Pressed) {
												return SkinButtonState.Pressed;
											} else {
												return SkinButtonState.Selected;
											}
										} else {
											return SkinButtonState.Normal;
										}
									},
									null,
									() => {
										if(throwHittest) {
											hitTest = HT.HTCAPTION;
										}
									},
									true
								);
							}
							
							if(hitTest != HT.HTNOWHERE) {
								m.Result = (IntPtr)hitTest;
								return;
							}
						}
					}
					break;
					
				case (int)WM.WM_SETCURSOR:
					{
						var hittest = WindowsUtility.HTFromLParam(m.LParam);
						if(hittest == HT.HTCAPTION) {
							NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, IDC.IDC_SIZEALL));
							return;
						} else {
							
						}
					}
					break;
					
				case (int)WM.WM_NCLBUTTONDOWN:
					{
						if(!NoteItem.Locked) {
							if(this.inputTitle.Visible) {
								HiddenInputTitleArea();
							}
							if(this.inputBody.Visible) {
								HiddenInputBodyArea();
							}
						}
					}
					break;
					
				case (int)WM.WM_NCRBUTTONUP:
					{
						if(!NoteItem.Locked) {
							switch (m.WParam.ToInt32()) {
								case (int)HT.HTCAPTION:
									var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
									ShowContextMenu(point);
									break;
								default:
									break;
							}
						}
					}
					break;
					
				default:
					break;
			}
			base.WndProc(ref m);
		}

	}
}
