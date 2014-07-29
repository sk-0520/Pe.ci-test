/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;
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
			DrawFullActivaChanged(false);
		}
	}
}
