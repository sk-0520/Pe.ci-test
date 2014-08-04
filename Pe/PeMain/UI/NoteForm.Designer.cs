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
			this.components = new System.ComponentModel.Container();
			this.inputBody = new System.Windows.Forms.TextBox();
			this.langnotemenutitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// inputBody
			// 
			this.inputBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inputBody.Location = new System.Drawing.Point(56, 56);
			this.inputBody.Multiline = true;
			this.inputBody.Name = "inputBody";
			this.inputBody.Size = new System.Drawing.Size(160, 96);
			this.inputBody.TabIndex = 0;
			this.inputBody.Visible = false;
			this.inputBody.Leave += new System.EventHandler(this.InputBody_Leave);
			// 
			// langnotemenutitleToolStripMenuItem
			// 
			this.langnotemenutitleToolStripMenuItem.Name = "langnotemenutitleToolStripMenuItem";
			this.langnotemenutitleToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
			this.langnotemenutitleToolStripMenuItem.Text = "lang:note/menu/title";
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.langnotemenutitleToolStripMenuItem});
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.Size = new System.Drawing.Size(200, 26);
			// 
			// NoteForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(256, 186);
			this.ContextMenuStrip = this.contextMenu;
			this.ControlBox = false;
			this.Controls.Add(this.inputBody);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NoteForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "NoteForm";
			this.Activated += new System.EventHandler(this.NoteForm_Activated);
			this.Deactivate += new System.EventHandler(this.NoteForm_Deactivate);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.NoteForm_Paint);
			this.DoubleClick += new System.EventHandler(this.NoteForm_DoubleClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NoteForm_MouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NoteForm_MouseUp);
			this.Resize += new System.EventHandler(this.NoteForm_Resize);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem langnotemenutitleToolStripMenuItem;
		private System.Windows.Forms.TextBox inputBody;
	}
}
