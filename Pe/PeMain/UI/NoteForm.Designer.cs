/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class NoteForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// NoteForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(256, 186);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.Name = "NoteForm";
			this.ShowInTaskbar = false;
			this.Text = "NoteForm";
			this.Activated += new System.EventHandler(this.NoteForm_Activated);
			this.Deactivate += new System.EventHandler(this.NoteForm_Deactivate);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.NoteForm_Paint);
			this.ResumeLayout(false);
		}
	}
}
