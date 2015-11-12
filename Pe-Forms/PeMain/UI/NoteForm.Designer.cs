/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace ContentTypeTextNet.Pe.PeMain.UI
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
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.contextMenu_itemTitle = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemBody = new System.Windows.Forms.ToolStripMenuItem();
			this.DisableCloseToolStripSeparator1 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.contextMenu_itemCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
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
			this.DisableCloseToolStripSeparator5 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
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
			this.DisableCloseToolStripSeparator6 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.contextMenu_itemBackColor_itemCustom = new System.Windows.Forms.ToolStripMenuItem();
			this.DisableCloseToolStripSeparator2 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.contextMenu_itemLock = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemCompact = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemTopmost = new System.Windows.Forms.ToolStripMenuItem();
			this.DisableCloseToolStripSeparator3 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.contextMenu_itemHidden = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.DisableCloseToolStripSeparator4 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.contextMenu_itemExport = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu_itemImport = new System.Windows.Forms.ToolStripMenuItem();
			this.inputTitle = new System.Windows.Forms.TextBox();
			this.inputBody = new ContentTypeTextNet.Pe.PeMain.UI.Ex.NoteTextBox();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenu_itemTitle,
            this.contextMenu_itemBody,
            this.DisableCloseToolStripSeparator1,
            this.contextMenu_itemCopy,
            this.toolStripMenuItem3,
            this.contextMenu_font,
            this.contextMenu_itemForeColor,
            this.contextMenu_itemBackColor,
            this.DisableCloseToolStripSeparator2,
            this.contextMenu_itemLock,
            this.contextMenu_itemCompact,
            this.contextMenu_itemTopmost,
            this.DisableCloseToolStripSeparator3,
            this.contextMenu_itemHidden,
            this.contextMenu_itemRemove,
            this.DisableCloseToolStripSeparator4,
            this.contextMenu_itemExport,
            this.contextMenu_itemImport});
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.Size = new System.Drawing.Size(213, 320);
			this.contextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.ContextMenu_Closed);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
			this.contextMenu.Opened += new System.EventHandler(this.contextMenu_Opened);
			// 
			// contextMenu_itemTitle
			// 
			this.contextMenu_itemTitle.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.contextMenu_itemTitle.Name = "contextMenu_itemTitle";
			this.contextMenu_itemTitle.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemTitle.Text = ":note/menu/title";
			this.contextMenu_itemTitle.Click += new System.EventHandler(this.ContextMenu_title_Click);
			// 
			// contextMenu_itemBody
			// 
			this.contextMenu_itemBody.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.contextMenu_itemBody.Name = "contextMenu_itemBody";
			this.contextMenu_itemBody.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemBody.Text = ":note/menu/body";
			this.contextMenu_itemBody.Click += new System.EventHandler(this.contextMenu_itemBody_Click);
			// 
			// DisableCloseToolStripSeparator1
			// 
			this.DisableCloseToolStripSeparator1.Name = "DisableCloseToolStripSeparator1";
			this.DisableCloseToolStripSeparator1.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_itemCopy
			// 
			this.contextMenu_itemCopy.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
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
			this.contextMenu_font.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.contextMenu_font.Name = "contextMenu_font";
			this.contextMenu_font.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_font.Text = ":note/menu/font";
			// 
			// contextMenu_font_change
			// 
			this.contextMenu_font_change.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
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
            this.DisableCloseToolStripSeparator5,
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
			// DisableCloseToolStripSeparator5
			// 
			this.DisableCloseToolStripSeparator5.Name = "DisableCloseToolStripSeparator5";
			this.DisableCloseToolStripSeparator5.Size = new System.Drawing.Size(225, 6);
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
            this.DisableCloseToolStripSeparator6,
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
			// DisableCloseToolStripSeparator6
			// 
			this.DisableCloseToolStripSeparator6.Name = "DisableCloseToolStripSeparator6";
			this.DisableCloseToolStripSeparator6.Size = new System.Drawing.Size(225, 6);
			// 
			// contextMenu_itemBackColor_itemCustom
			// 
			this.contextMenu_itemBackColor_itemCustom.Name = "contextMenu_itemBackColor_itemCustom";
			this.contextMenu_itemBackColor_itemCustom.Size = new System.Drawing.Size(228, 22);
			this.contextMenu_itemBackColor_itemCustom.Text = ":note/menu/color/custom";
			this.contextMenu_itemBackColor_itemCustom.Click += new System.EventHandler(this.ContextMenu_itemBackColor_itemCustom_Click);
			// 
			// DisableCloseToolStripSeparator2
			// 
			this.DisableCloseToolStripSeparator2.Name = "DisableCloseToolStripSeparator2";
			this.DisableCloseToolStripSeparator2.Size = new System.Drawing.Size(209, 6);
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
			this.contextMenu_itemTopmost.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.contextMenu_itemTopmost.Name = "contextMenu_itemTopmost";
			this.contextMenu_itemTopmost.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemTopmost.Text = ":note/menu/topmost";
			this.contextMenu_itemTopmost.Click += new System.EventHandler(this.ContextMenu_itemTopmost_Click);
			// 
			// DisableCloseToolStripSeparator3
			// 
			this.DisableCloseToolStripSeparator3.Name = "DisableCloseToolStripSeparator3";
			this.DisableCloseToolStripSeparator3.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_itemHidden
			// 
			this.contextMenu_itemHidden.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.contextMenu_itemHidden.Name = "contextMenu_itemHidden";
			this.contextMenu_itemHidden.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemHidden.Text = ":note/menu/hidden";
			this.contextMenu_itemHidden.Click += new System.EventHandler(this.ContextMenu_itemHidden_Click);
			// 
			// contextMenu_itemRemove
			// 
			this.contextMenu_itemRemove.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.contextMenu_itemRemove.Name = "contextMenu_itemRemove";
			this.contextMenu_itemRemove.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemRemove.Text = ":note/menu/remove";
			this.contextMenu_itemRemove.Click += new System.EventHandler(this.ContextMenu_itemRemove_Click);
			// 
			// DisableCloseToolStripSeparator4
			// 
			this.DisableCloseToolStripSeparator4.Name = "DisableCloseToolStripSeparator4";
			this.DisableCloseToolStripSeparator4.Size = new System.Drawing.Size(209, 6);
			// 
			// contextMenu_itemExport
			// 
			this.contextMenu_itemExport.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.contextMenu_itemExport.Name = "contextMenu_itemExport";
			this.contextMenu_itemExport.Size = new System.Drawing.Size(212, 22);
			this.contextMenu_itemExport.Text = ":note/menu/export";
			this.contextMenu_itemExport.Click += new System.EventHandler(this.NotemenuexportToolStripMenuItem_Click);
			// 
			// contextMenu_itemImport
			// 
			this.contextMenu_itemImport.Enabled = false;
			this.contextMenu_itemImport.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
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
			// inputBody
			// 
			this.inputBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inputBody.ContextMenuStrip = this.contextMenu;
			this.inputBody.Location = new System.Drawing.Point(40, 56);
			this.inputBody.Margin = new System.Windows.Forms.Padding(0);
			this.inputBody.Name = "inputBody";
			this.inputBody.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.inputBody.Size = new System.Drawing.Size(187, 120);
			this.inputBody.TabIndex = 0;
			this.inputBody.TabStop = false;
			this.inputBody.Text = "";
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
			this.DoubleBuffered = true;
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
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NoteForm_MouseDown);
			this.MouseLeave += new System.EventHandler(this.NoteForm_MouseLeave);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NoteForm_MouseUp);
			this.Move += new System.EventHandler(this.NoteForm_Move);
			this.Resize += new System.EventHandler(this.NoteForm_Resize);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemRemove;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemCustom;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemPurple;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemOrange;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemYellow;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemBlue;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemGreen;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemRed;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemWhite;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor_itemBlack;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor_itemCustom;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator5;
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
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator4;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemTopmost;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font_reset;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font_change;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBackColor;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemForeColor;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_font;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.TextBox inputTitle;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemHidden;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemCompact;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemLock;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemCopy;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemBody;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem contextMenu_itemTitle;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.NoteTextBox inputBody;
	}
}
