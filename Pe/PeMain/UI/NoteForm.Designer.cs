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
			this.contextMenu_title = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.contextMenu_body = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_copy = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_lock = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_compact = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_hidden = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_remove = new System.Windows.Forms.ToolStripMenuItem();
			this.inputTitle = new System.Windows.Forms.TextBox();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// inputBody
			// 
			this.inputBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inputBody.Location = new System.Drawing.Point(56, 56);
			this.inputBody.Margin = new System.Windows.Forms.Padding(0);
			this.inputBody.Multiline = true;
			this.inputBody.Name = "inputBody";
			this.inputBody.Size = new System.Drawing.Size(160, 96);
			this.inputBody.TabIndex = 0;
			this.inputBody.TabStop = false;
			this.inputBody.Visible = false;
			this.inputBody.Leave += new System.EventHandler(this.Input_Leave);
			// 
			// contextMenu_title
			// 
			this.contextMenu_title.Name = "contextMenu_title";
			this.contextMenu_title.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_title.Text = ":note/menu/title";
			this.contextMenu_title.Click += new System.EventHandler(this.ContextMenu_title_Click);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.contextMenu_title,
									this.contextMenu_body,
									this.toolStripSeparator1,
									this.contextMenu_copy,
									this.toolStripSeparator2,
									this.contextMenu_lock,
									this.contextMenu_compact,
									this.contextMenu_hidden,
									this.contextMenu_remove});
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.Size = new System.Drawing.Size(203, 170);
			// 
			// contextMenu_body
			// 
			this.contextMenu_body.Name = "contextMenu_body";
			this.contextMenu_body.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_body.Text = ":note/menu/body";
			this.contextMenu_body.Click += new System.EventHandler(this.ContextMenu_body_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
			// 
			// contextMenu_copy
			// 
			this.contextMenu_copy.Name = "contextMenu_copy";
			this.contextMenu_copy.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_copy.Text = ":note/menu/copy";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(199, 6);
			// 
			// contextMenu_lock
			// 
			this.contextMenu_lock.Name = "contextMenu_lock";
			this.contextMenu_lock.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_lock.Text = ":note/menu/lock";
			// 
			// contextMenu_compact
			// 
			this.contextMenu_compact.Name = "contextMenu_compact";
			this.contextMenu_compact.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_compact.Text = ":note/menu/compact";
			// 
			// contextMenu_hidden
			// 
			this.contextMenu_hidden.Name = "contextMenu_hidden";
			this.contextMenu_hidden.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_hidden.Text = ":note/menu/hidden";
			// 
			// contextMenu_remove
			// 
			this.contextMenu_remove.Name = "contextMenu_remove";
			this.contextMenu_remove.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_remove.Text = ":note/menu/remove";
			// 
			// inputTitle
			// 
			this.inputTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inputTitle.Location = new System.Drawing.Point(24, 24);
			this.inputTitle.Margin = new System.Windows.Forms.Padding(0);
			this.inputTitle.Name = "inputTitle";
			this.inputTitle.Size = new System.Drawing.Size(100, 12);
			this.inputTitle.TabIndex = 1;
			this.inputTitle.TabStop = false;
			this.inputTitle.Visible = false;
			this.inputTitle.Leave += new System.EventHandler(this.Input_Leave);
			// 
			// NoteForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(256, 186);
			this.ContextMenuStrip = this.contextMenu;
			this.ControlBox = false;
			this.Controls.Add(this.inputTitle);
			this.Controls.Add(this.inputBody);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NoteForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "NoteForm";
			this.Activated += new System.EventHandler(this.NoteForm_Activated);
			this.Deactivate += new System.EventHandler(this.NoteForm_Deactivate);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.NoteForm_Paint);
			this.DoubleClick += new System.EventHandler(this.NoteForm_DoubleClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NoteForm_MouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NoteForm_MouseUp);
			this.Move += new System.EventHandler(this.NoteForm_Move);
			this.Resize += new System.EventHandler(this.NoteForm_Resize);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox inputTitle;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_remove;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_hidden;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_compact;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_lock;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_copy;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_body;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_title;
		private System.Windows.Forms.TextBox inputBody;
	}
}
