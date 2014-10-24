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
			this.contextMenu_itemForeColor = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemBlack = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemWhite = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemRed = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemGreen = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemBlue = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemYellow = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemOrange = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemForeColor_itemPurple = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_itemForeColor_itemCustom = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemBlack = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemWhite = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemRed = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemGreen = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemBlue = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemYellow = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemOrange = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBackColor_itemPurple = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_itemBackColor_itemCustom = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_itemLock = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemCompact = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemTopmost = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_itemHidden = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenu_itemExport = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemImport = new System.Windows.Forms.ToolStripMenuItem();
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
			this.contextMenu_itemTitle.Image = global::PeMain.Properties.Images.NoteTitle;
			this.contextMenu_itemTitle.Name = "contextMenu_itemTitle";
			this.contextMenu_itemTitle.Size = new System.Drawing.Size(212, 22);
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
									this.contextMenu_itemForeColor,
									this.contextMenu_itemBackColor,
									this.toolStripSeparator2,
									this.contextMenu_itemLock,
									this.contextMenu_itemCompact,
									this.contextMenu_itemTopmost,
									this.toolStripSeparator3,
									this.contextMenu_itemHidden,
									this.toolStripSeparator4,
									this.contextMenu_itemExport,
									this.contextMenu_itemImport});
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.Size = new System.Drawing.Size(213, 298);
			this.contextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.ContextMenu_Closed);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
			// 
			// contextMenu_itemBody
			// 
			this.contextMenu_itemBody.Image = global::PeMain.Properties.Images.NoteBody;
			this.contextMenu_itemBody.Name = "contextMenu_itemBody";
			this.contextMenu_itemBody.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemBody.Text = ":note/menu/body";
			this.contextMenu_itemBody.Click += new System.EventHandler(this.ContextMenu_body_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_itemCopy
			// 
			this.contextMenu_itemCopy.Image = global::PeMain.Properties.Images.ClipboardText;
			this.contextMenu_itemCopy.Name = "contextMenu_itemCopy";
			this.contextMenu_itemCopy.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemCopy.Text = ":note/menu/copy";
			this.contextMenu_itemCopy.Click += new System.EventHandler(this.ContextMenu_itemCopy_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_font
			// 
			this.contextMenu_font.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.contextMenu_font_change,
									this.contextMenu_font_reset});
			this.contextMenu_font.Image = global::PeMain.Properties.Images.Font;
			this.contextMenu_font.Name = "contextMenu_font";
			this.contextMenu_font.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_font.Text = ":note/menu/font";
			// 
			// contextMenu_font_change
			// 
			this.contextMenu_font_change.Name = "contextMenu_font_change";
			this.contextMenu_font_change.Size = new System.Drawing.Size(211, 22);
			this.contextMenu_font_change.Text = "<FONT-CHANGE>";
			this.contextMenu_font_change.Click += new System.EventHandler(this.ContextMenu_font_change_Click);
			// 
			// contextMenu_font_reset
			// 
			this.contextMenu_font_reset.Name = "contextMenu_font_reset";
			this.contextMenu_font_reset.Size = new System.Drawing.Size(211, 22);
			this.contextMenu_font_reset.Text = ":note/menu/font/reset";
			this.contextMenu_font_reset.Click += new System.EventHandler(this.ContextMenu_font_reset_Click);
			// 
			// contextMenu_itemForeColor
			// 
			this.contextMenu_itemForeColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.contextMenu_itemForeColor_itemBlack,
									this.contextMenu_itemForeColor_itemWhite,
									this.contextMenu_itemForeColor_itemRed,
									this.contextMenu_itemForeColor_itemGreen,
									this.contextMenu_itemForeColor_itemBlue,
									this.contextMenu_itemForeColor_itemYellow,
									this.contextMenu_itemForeColor_itemOrange,
									this.contextMenu_itemForeColor_itemPurple,
									this.toolStripSeparator5,
									this.contextMenu_itemForeColor_itemCustom});
			this.contextMenu_itemForeColor.Name = "contextMenu_itemForeColor";
			this.contextMenu_itemForeColor.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemForeColor.Text = ":note/menu/color-fore";
			// 
			// contextMenu_itemForeColor_itemBlack
			// 
			this.contextMenu_itemForeColor_itemBlack.Name = "contextMenu_itemForeColor_itemBlack";
			this.contextMenu_itemForeColor_itemBlack.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemBlack.Text = ":note/menu/color/black";
			this.contextMenu_itemForeColor_itemBlack.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// contextMenu_itemForeColor_itemWhite
			// 
			this.contextMenu_itemForeColor_itemWhite.Name = "contextMenu_itemForeColor_itemWhite";
			this.contextMenu_itemForeColor_itemWhite.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemWhite.Text = ":note/menu/color/white";
			this.contextMenu_itemForeColor_itemWhite.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// contextMenu_itemForeColor_itemRed
			// 
			this.contextMenu_itemForeColor_itemRed.Name = "contextMenu_itemForeColor_itemRed";
			this.contextMenu_itemForeColor_itemRed.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemRed.Text = ":note/menu/color/red";
			this.contextMenu_itemForeColor_itemRed.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// contextMenu_itemForeColor_itemGreen
			// 
			this.contextMenu_itemForeColor_itemGreen.Name = "contextMenu_itemForeColor_itemGreen";
			this.contextMenu_itemForeColor_itemGreen.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemGreen.Text = ":note/menu/color/green";
			this.contextMenu_itemForeColor_itemGreen.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// contextMenu_itemForeColor_itemBlue
			// 
			this.contextMenu_itemForeColor_itemBlue.Name = "contextMenu_itemForeColor_itemBlue";
			this.contextMenu_itemForeColor_itemBlue.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemBlue.Text = ":note/menu/color/blue";
			this.contextMenu_itemForeColor_itemBlue.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// contextMenu_itemForeColor_itemYellow
			// 
			this.contextMenu_itemForeColor_itemYellow.Name = "contextMenu_itemForeColor_itemYellow";
			this.contextMenu_itemForeColor_itemYellow.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemYellow.Text = ":note/menu/color/yellow";
			this.contextMenu_itemForeColor_itemYellow.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// contextMenu_itemForeColor_itemOrange
			// 
			this.contextMenu_itemForeColor_itemOrange.Name = "contextMenu_itemForeColor_itemOrange";
			this.contextMenu_itemForeColor_itemOrange.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemOrange.Text = ":note/menu/color/orange";
			this.contextMenu_itemForeColor_itemOrange.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// contextMenu_itemForeColor_itemPurple
			// 
			this.contextMenu_itemForeColor_itemPurple.Name = "contextMenu_itemForeColor_itemPurple";
			this.contextMenu_itemForeColor_itemPurple.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemPurple.Text = ":note/menu/color/purple";
			this.contextMenu_itemForeColor_itemPurple.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemClick);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(225, 6);
			// 
			// contextMenu_itemForeColor_itemCustom
			// 
			this.contextMenu_itemForeColor_itemCustom.Name = "contextMenu_itemForeColor_itemCustom";
			this.contextMenu_itemForeColor_itemCustom.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemForeColor_itemCustom.Text = ":note/menu/color/custom";
			this.contextMenu_itemForeColor_itemCustom.Click += new System.EventHandler(this.ContextMenu_itemForeColor_itemCustom_Click);
			// 
			// contextMenu_itemBackColor
			// 
			this.contextMenu_itemBackColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.contextMenu_itemBackColor_itemBlack,
									this.contextMenu_itemBackColor_itemWhite,
									this.contextMenu_itemBackColor_itemRed,
									this.contextMenu_itemBackColor_itemGreen,
									this.contextMenu_itemBackColor_itemBlue,
									this.contextMenu_itemBackColor_itemYellow,
									this.contextMenu_itemBackColor_itemOrange,
									this.contextMenu_itemBackColor_itemPurple,
									this.toolStripSeparator6,
									this.contextMenu_itemBackColor_itemCustom});
			this.contextMenu_itemBackColor.Name = "contextMenu_itemBackColor";
			this.contextMenu_itemBackColor.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemBackColor.Text = ":note/menu/color-back";
			// 
			// contextMenu_itemBackColor_itemBlack
			// 
			this.contextMenu_itemBackColor_itemBlack.Name = "contextMenu_itemBackColor_itemBlack";
			this.contextMenu_itemBackColor_itemBlack.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemBlack.Text = ":note/menu/color/black";
			this.contextMenu_itemBackColor_itemBlack.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// contextMenu_itemBackColor_itemWhite
			// 
			this.contextMenu_itemBackColor_itemWhite.Name = "contextMenu_itemBackColor_itemWhite";
			this.contextMenu_itemBackColor_itemWhite.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemWhite.Text = ":note/menu/color/white";
			this.contextMenu_itemBackColor_itemWhite.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// contextMenu_itemBackColor_itemRed
			// 
			this.contextMenu_itemBackColor_itemRed.Name = "contextMenu_itemBackColor_itemRed";
			this.contextMenu_itemBackColor_itemRed.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemRed.Text = ":note/menu/color/red";
			this.contextMenu_itemBackColor_itemRed.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// contextMenu_itemBackColor_itemGreen
			// 
			this.contextMenu_itemBackColor_itemGreen.Name = "contextMenu_itemBackColor_itemGreen";
			this.contextMenu_itemBackColor_itemGreen.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemGreen.Text = ":note/menu/color/green";
			this.contextMenu_itemBackColor_itemGreen.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// contextMenu_itemBackColor_itemBlue
			// 
			this.contextMenu_itemBackColor_itemBlue.Name = "contextMenu_itemBackColor_itemBlue";
			this.contextMenu_itemBackColor_itemBlue.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemBlue.Text = ":note/menu/color/blue";
			this.contextMenu_itemBackColor_itemBlue.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// contextMenu_itemBackColor_itemYellow
			// 
			this.contextMenu_itemBackColor_itemYellow.Name = "contextMenu_itemBackColor_itemYellow";
			this.contextMenu_itemBackColor_itemYellow.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemYellow.Text = ":note/menu/color/yellow";
			this.contextMenu_itemBackColor_itemYellow.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// contextMenu_itemBackColor_itemOrange
			// 
			this.contextMenu_itemBackColor_itemOrange.Name = "contextMenu_itemBackColor_itemOrange";
			this.contextMenu_itemBackColor_itemOrange.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemOrange.Text = ":note/menu/color/orange";
			this.contextMenu_itemBackColor_itemOrange.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// contextMenu_itemBackColor_itemPurple
			// 
			this.contextMenu_itemBackColor_itemPurple.Name = "contextMenu_itemBackColor_itemPurple";
			this.contextMenu_itemBackColor_itemPurple.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemPurple.Text = ":note/menu/color/purple";
			this.contextMenu_itemBackColor_itemPurple.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemClick);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(225, 6);
			// 
			// contextMenu_itemBackColor_itemCustom
			// 
			this.contextMenu_itemBackColor_itemCustom.Name = "contextMenu_itemBackColor_itemCustom";
			this.contextMenu_itemBackColor_itemCustom.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemCustom.Text = ":note/menu/color/custom";
			this.contextMenu_itemBackColor_itemCustom.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemCustom_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_itemLock
			// 
			this.contextMenu_itemLock.Name = "contextMenu_itemLock";
			this.contextMenu_itemLock.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemLock.Text = ":note/menu/lock";
			this.contextMenu_itemLock.Click += new System.EventHandler(this.ContextMenu_itemLock_Click);
			// 
			// contextMenu_itemCompact
			// 
			this.contextMenu_itemCompact.Name = "contextMenu_itemCompact";
			this.contextMenu_itemCompact.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemCompact.Text = ":note/menu/compact";
			this.contextMenu_itemCompact.Click += new System.EventHandler(this.ContextMenu_itemCompact_Click);
			// 
			// contextMenu_itemTopmost
			// 
			this.contextMenu_itemTopmost.Image = global::PeMain.Properties.Images.Pin;
			this.contextMenu_itemTopmost.Name = "contextMenu_itemTopmost";
			this.contextMenu_itemTopmost.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemTopmost.Text = ":note/menu/topmost";
			this.contextMenu_itemTopmost.Click += new System.EventHandler(this.ContextMenu_itemTopmost_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_itemHidden
			// 
			this.contextMenu_itemHidden.Image = global::PeMain.Properties.Images.Remove;
			this.contextMenu_itemHidden.Name = "contextMenu_itemHidden";
			this.contextMenu_itemHidden.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemHidden.Text = ":note/menu/hidden";
			this.contextMenu_itemHidden.Click += new System.EventHandler(this.ContextMenu_itemHidden_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_itemExport
			// 
			this.contextMenu_itemExport.Image = global::PeMain.Properties.Images.Disk;
			this.contextMenu_itemExport.Name = "contextMenu_itemExport";
			this.contextMenu_itemExport.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemExport.Text = ":note/menu/export";
			this.contextMenu_itemExport.Click += new System.EventHandler(this.NotemenuexportToolStripMenuItem_Click);
			// 
			// contextMenu_itemImport
			// 
			this.contextMenu_itemImport.Enabled = false;
			this.contextMenu_itemImport.Image = global::PeMain.Properties.Images.OpenDir;
			this.contextMenu_itemImport.Name = "contextMenu_itemImport";
			this.contextMenu_itemImport.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemImport.Text = ":note/menu/import";
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
			this.MouseLeave += new System.EventHandler(this.NoteForm_MouseLeave);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NoteForm_MouseUp);
			this.Move += new System.EventHandler(this.NoteForm_Move);
			this.Resize += new System.EventHandler(this.NoteForm_Resize);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemCustom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemPurple;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemOrange;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemYellow;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemBlue;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemGreen;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemRed;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemWhite;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemBlack;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemCustom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemPurple;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemOrange;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemYellow;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemBlue;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemGreen;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemRed;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemWhite;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemBlack;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemImport;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemExport;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemTopmost;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font_reset;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font_change;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.TextBox inputTitle;
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
