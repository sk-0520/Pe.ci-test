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
					this._flag &= ~this._flag;
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
				case (int)WM.WM_NCHITTEST:
					{
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
						area.Y -= edgePadding.Bottom;
						area.Height = edgePadding.Bottom;
						pos.Bottom = area.Contains(point);
						// 左
						area = noteArea;
						area.Width = edgePadding.Left;
						pos.Left = area.Contains(point);
						// 右
						area = noteArea;
						area.X -= edgePadding.Right;
						area.Width = edgePadding.Right;
						pos.Right = area.Contains(point);
						
						if(pos.HasTrue) {
							if(pos.Left) {
								if(pos.Top) {
									
								} else if(pos.Bottom) {
									
								} else {
									
								}
							} else if(pos.Right) {
								if(pos.Top) {
									
								} else if(pos.Bottom) {
									
								} else {
									
								}
							} else if(pos.Top) {
								if(pos.Left) {
									
								} else if(pos.Right) {
									
								} else {
									
								}
							} else if(pos.Bottom) {
								if(pos.Left) {
									
								} else if(pos.Right) {
									
								} else {
									
								}
							}
						}
						
						if(hitTest != HT.HTNOWHERE) {
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
