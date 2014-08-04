/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of NoteForm.
	/// </summary>
	public partial class NoteForm : Form, ISetCommonData
	{
		public NoteForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void NoteForm_Paint(object sender, PaintEventArgs e)
		{
			using(var bmp = new Bitmap(Width, Height, e.Graphics)) {
				using(var memG = Graphics.FromImage(bmp)) {
					var rect = new Rectangle(Point.Empty, Size);
					DrawFull(memG, rect, this == Form.ActiveForm);
					e.Graphics.DrawImage(bmp, 0, 0);
				}
			}
		}
		
		void NoteForm_Activated(object sender, EventArgs e)
		{
			DrawFullActivaChanged(true);
		}
		
		void NoteForm_Deactivate(object sender, EventArgs e)
		{
			HiddenInputArea();
			DrawFullActivaChanged(false);
		}
		
		
		void NoteForm_MouseDown(object sender, MouseEventArgs e)
		{
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					if(isIn) {
						return ButtonState.Pressed;
					} else {
						return ButtonState.Normal;
					}
				},
				null,
				null,
				false
			);
		}
		
		void NoteForm_MouseUp(object sender, MouseEventArgs e)
		{
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					if(isIn) {
						return ButtonState.Selected;
					} else {
						return ButtonState.Normal;
					}
				},
				command => {
					if(this._commandStateMap[command] == ButtonState.Pressed) {
						clickCommand(command);
					}
				},
				null,
				true
			);
		}
		
		void NoteForm_DoubleClick(object sender, EventArgs e)
		{
			ShowInputArea();
		}
		
		void NoteForm_Resize(object sender, EventArgs e)
		{
			ResizeInputArea();
		}
		
		void InputBody_Leave(object sender, EventArgs e)
		{
			HiddenInputArea();
		}
	}
}