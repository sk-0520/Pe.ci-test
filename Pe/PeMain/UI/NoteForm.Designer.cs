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
			this.contextMenu_itemTitle = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.contextMenu_itemBody = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_itemCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_font = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_font_change = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_font_reset = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_fore = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_back = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_itemLock = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemCompact = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemHidden = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.inputTitle = new System.Windows.Forms.TextBox();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// inputBody
			// 
			this.inputBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inputBody.Location = new System.Drawing.Point(40, 56);
			this.inputBody.Margin = new System.Windows.Forms.Padding(0);
			this.inputBody.Multiline = true;
			this.inputBody.Name = "inputBody";
			this.inputBody.Size = new System.Drawing.Size(187, 120);
			this.inputBody.TabIndex = 0;
			this.inputBody.TabStop = false;
			this.inputBody.Visible = false;
			this.inputBody.Leave += new System.EventHandler(this.Input_Leave);
			// 
			// contextMenu_itemTitle
			// 
			this.contextMenu_itemTitle.Name = "contextMenu_itemTitle";
			this.contextMenu_itemTitle.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_itemTitle.Text = ":note/menu/title";
			this.contextMenu_itemTitle.Click += new System.EventHandler(this.ContextMenu_title_Click);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.contextMenu_itemTitle,
									this.contextMenu_itemBody,
									this.toolStripSeparator1,
									this.contextMenu_itemCopy,
									this.toolStripMenuItem3,
									this.contextMenu_font,
									this.contextMenu_fore,
									this.contextMenu_back,
									this.toolStripSeparator2,
									this.contextMenu_itemLock,
									this.contextMenu_itemCompact,
									this.contextMenu_itemHidden,
									this.contextMenu_itemRemove});
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.Size = new System.Drawing.Size(203, 264);
			// 
			// contextMenu_itemBody
			// 
			this.contextMenu_itemBody.Name = "contextMenu_itemBody";
			this.contextMenu_itemBody.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_itemBody.Text = ":note/menu/body";
			this.contextMenu_itemBody.Click += new System.EventHandler(this.ContextMenu_body_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
			// 
			// contextMenu_itemCopy
			// 
			this.contextMenu_itemCopy.Name = "contextMenu_itemCopy";
			this.contextMenu_itemCopy.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_itemCopy.Text = ":note/menu/copy";
			this.contextMenu_itemCopy.Click += new System.EventHandler(this.ContextMenu_itemCopy_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(199, 6);
			// 
			// contextMenu_font
			// 
			this.contextMenu_font.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.contextMenu_font_change,
									this.contextMenu_font_reset});
			this.contextMenu_font.Name = "contextMenu_font";
			this.contextMenu_font.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_font.Text = ":note/menu/font";
			// 
			// contextMenu_font_change
			// 
			this.contextMenu_font_change.Name = "contextMenu_font_change";
			this.contextMenu_font_change.Size = new System.Drawing.Size(222, 22);
			this.contextMenu_font_change.Text = ":note/menu/font/change";
			this.contextMenu_font_change.Click += new System.EventHandler(this.ContextMenu_font_change_Click);
			// 
			// contextMenu_font_reset
			// 
			this.contextMenu_font_reset.Name = "contextMenu_font_reset";
			this.contextMenu_font_reset.Size = new System.Drawing.Size(222, 22);
			this.contextMenu_font_reset.Text = ":note/menu/font/reset";
			this.contextMenu_font_reset.Click += new System.EventHandler(this.ContextMenu_font_reset_Click);
			// 
			// contextMenu_fore
			// 
			this.contextMenu_fore.Name = "contextMenu_fore";
			this.contextMenu_fore.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_fore.Text = ":note/menu/fore";
			// 
			// contextMenu_back
			// 
			this.contextMenu_back.Name = "contextMenu_back";
			this.contextMenu_back.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_back.Text = ":note/menu/back";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(199, 6);
			// 
			// contextMenu_itemLock
			// 
			this.contextMenu_itemLock.Name = "contextMenu_itemLock";
			this.contextMenu_itemLock.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_itemLock.Text = ":note/menu/lock";
			// 
			// contextMenu_itemCompact
			// 
			this.contextMenu_itemCompact.Name = "contextMenu_itemCompact";
			this.contextMenu_itemCompact.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_itemCompact.Text = ":note/menu/compact";
			// 
			// contextMenu_itemHidden
			// 
			this.contextMenu_itemHidden.Name = "contextMenu_itemHidden";
			this.contextMenu_itemHidden.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_itemHidden.Text = ":note/menu/hidden";
			// 
			// contextMenu_itemRemove
			// 
			this.contextMenu_itemRemove.Name = "contextMenu_itemRemove";
			this.contextMenu_itemRemove.Size = new System.Drawing.Size(202, 22);
			this.contextMenu_itemRemove.Text = ":note/menu/remove";
			// 
			// inputTitle
			// 
			this.inputTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inputTitle.Location = new System.Drawing.Point(28, 30);
			this.inputTitle.Margin = new System.Windows.Forms.Padding(0);
			this.inputTitle.Name = "inputTitle";
			this.inputTitle.Size = new System.Drawing.Size(117, 16);
			this.inputTitle.TabIndex = 1;
			this.inputTitle.TabStop = false;
			this.inputTitle.Visible = false;
			this.inputTitle.Leave += new System.EventHandler(this.Input_Leave);
			// 
			// NoteForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(247, 187);
			this.ContextMenuStrip = this.contextMenu;
			this.ControlBox = false;
			this.Controls.Add(this.inputTitle);
			this.Controls.Add(this.inputBody);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
			this.Load += new System.EventHandler(this.NoteForm_Load);
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
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font_reset;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font_change;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_back;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_fore;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.TextBox inputTitle;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemRemove;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemHidden;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemCompact;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemLock;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemCopy;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBody;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemTitle;
		private System.Windows.Forms.TextBox inputBody;
	}
}
