namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class ClipboardForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.panelMain = new System.Windows.Forms.ToolStripContainer();
			this.statusClipboard = new System.Windows.Forms.StatusStrip();
			this.statusClipboard_itemSelectedIndex = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusClipboard_itemCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelClipboard = new System.Windows.Forms.SplitContainer();
			this.listClipboard = new System.Windows.Forms.ListBox();
			this.tabPreview = new System.Windows.Forms.TabControl();
			this.tabPreview_pageText = new System.Windows.Forms.TabPage();
			this.viewText = new System.Windows.Forms.TextBox();
			this.tabPreview_pageRichTextFormat = new System.Windows.Forms.TabPage();
			this.viewRtf = new System.Windows.Forms.RichTextBox();
			this.tabPreview_pageImage = new System.Windows.Forms.TabPage();
			this.panelImage = new System.Windows.Forms.Panel();
			this.viewImage = new System.Windows.Forms.PictureBox();
			this.tabPreview_pageFile = new System.Windows.Forms.TabPage();
			this.viewFile = new System.Windows.Forms.ListView();
			this.imageTab = new System.Windows.Forms.ImageList(this.components);
			this.toolClipboard = new System.Windows.Forms.ToolStrip();
			this.toolClipboard_itemTopmost = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolClipboard_itemSave = new System.Windows.Forms.ToolStripButton();
			this.toolClipboard_itemClear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolClipboard_itemType = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolClipboard_itemType_itemClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.toolClipboard_itemType_itemTemplate = new System.Windows.Forms.ToolStripMenuItem();
			this.panelMain.BottomToolStripPanel.SuspendLayout();
			this.panelMain.ContentPanel.SuspendLayout();
			this.panelMain.TopToolStripPanel.SuspendLayout();
			this.panelMain.SuspendLayout();
			this.statusClipboard.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelClipboard)).BeginInit();
			this.panelClipboard.Panel1.SuspendLayout();
			this.panelClipboard.Panel2.SuspendLayout();
			this.panelClipboard.SuspendLayout();
			this.tabPreview.SuspendLayout();
			this.tabPreview_pageText.SuspendLayout();
			this.tabPreview_pageRichTextFormat.SuspendLayout();
			this.tabPreview_pageImage.SuspendLayout();
			this.panelImage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewImage)).BeginInit();
			this.tabPreview_pageFile.SuspendLayout();
			this.toolClipboard.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelMain
			// 
			// 
			// panelMain.BottomToolStripPanel
			// 
			this.panelMain.BottomToolStripPanel.Controls.Add(this.statusClipboard);
			// 
			// panelMain.ContentPanel
			// 
			this.panelMain.ContentPanel.Controls.Add(this.panelClipboard);
			this.panelMain.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panelMain.ContentPanel.Size = new System.Drawing.Size(608, 230);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.LeftToolStripPanelVisible = false;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panelMain.Name = "panelMain";
			this.panelMain.RightToolStripPanelVisible = false;
			this.panelMain.Size = new System.Drawing.Size(608, 278);
			this.panelMain.TabIndex = 0;
			this.panelMain.Text = "toolStripContainer1";
			// 
			// panelMain.TopToolStripPanel
			// 
			this.panelMain.TopToolStripPanel.Controls.Add(this.toolClipboard);
			// 
			// statusClipboard
			// 
			this.statusClipboard.Dock = System.Windows.Forms.DockStyle.None;
			this.statusClipboard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusClipboard_itemSelectedIndex,
            this.toolStripStatusLabel2,
            this.statusClipboard_itemCount});
			this.statusClipboard.Location = new System.Drawing.Point(0, 0);
			this.statusClipboard.Name = "statusClipboard";
			this.statusClipboard.Size = new System.Drawing.Size(608, 23);
			this.statusClipboard.TabIndex = 0;
			// 
			// statusClipboard_itemSelectedIndex
			// 
			this.statusClipboard_itemSelectedIndex.Name = "statusClipboard_itemSelectedIndex";
			this.statusClipboard_itemSelectedIndex.Size = new System.Drawing.Size(20, 18);
			this.statusClipboard_itemSelectedIndex.Text = "☃";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 18);
			this.toolStripStatusLabel2.Text = "/";
			// 
			// statusClipboard_itemCount
			// 
			this.statusClipboard_itemCount.Name = "statusClipboard_itemCount";
			this.statusClipboard_itemCount.Size = new System.Drawing.Size(20, 18);
			this.statusClipboard_itemCount.Text = "☃";
			// 
			// panelClipboard
			// 
			this.panelClipboard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelClipboard.Location = new System.Drawing.Point(0, 0);
			this.panelClipboard.Name = "panelClipboard";
			// 
			// panelClipboard.Panel1
			// 
			this.panelClipboard.Panel1.Controls.Add(this.listClipboard);
			// 
			// panelClipboard.Panel2
			// 
			this.panelClipboard.Panel2.Controls.Add(this.tabPreview);
			this.panelClipboard.Size = new System.Drawing.Size(608, 230);
			this.panelClipboard.SplitterDistance = 225;
			this.panelClipboard.TabIndex = 0;
			// 
			// listClipboard
			// 
			this.listClipboard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listClipboard.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listClipboard.FormattingEnabled = true;
			this.listClipboard.IntegralHeight = false;
			this.listClipboard.ItemHeight = 15;
			this.listClipboard.Location = new System.Drawing.Point(0, 0);
			this.listClipboard.Name = "listClipboard";
			this.listClipboard.ScrollAlwaysVisible = true;
			this.listClipboard.Size = new System.Drawing.Size(225, 230);
			this.listClipboard.TabIndex = 0;
			this.listClipboard.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listClipboard_DrawItem);
			this.listClipboard.SelectedIndexChanged += new System.EventHandler(this.listClipboard_SelectedIndexChanged);
			this.listClipboard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listClipboard_MouseMove);
			// 
			// tabPreview
			// 
			this.tabPreview.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabPreview.Controls.Add(this.tabPreview_pageText);
			this.tabPreview.Controls.Add(this.tabPreview_pageRichTextFormat);
			this.tabPreview.Controls.Add(this.tabPreview_pageImage);
			this.tabPreview.Controls.Add(this.tabPreview_pageFile);
			this.tabPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPreview.ImageList = this.imageTab;
			this.tabPreview.Location = new System.Drawing.Point(0, 0);
			this.tabPreview.Name = "tabPreview";
			this.tabPreview.SelectedIndex = 0;
			this.tabPreview.Size = new System.Drawing.Size(379, 230);
			this.tabPreview.TabIndex = 0;
			// 
			// tabPreview_pageText
			// 
			this.tabPreview_pageText.Controls.Add(this.viewText);
			this.tabPreview_pageText.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageText.Name = "tabPreview_pageText";
			this.tabPreview_pageText.Padding = new System.Windows.Forms.Padding(3);
			this.tabPreview_pageText.Size = new System.Drawing.Size(371, 199);
			this.tabPreview_pageText.TabIndex = 0;
			this.tabPreview_pageText.Text = "#ClipboardType.Text";
			this.tabPreview_pageText.UseVisualStyleBackColor = true;
			// 
			// viewText
			// 
			this.viewText.Location = new System.Drawing.Point(31, 25);
			this.viewText.Multiline = true;
			this.viewText.Name = "viewText";
			this.viewText.Size = new System.Drawing.Size(140, 121);
			this.viewText.TabIndex = 0;
			// 
			// tabPreview_pageRichTextFormat
			// 
			this.tabPreview_pageRichTextFormat.Controls.Add(this.viewRtf);
			this.tabPreview_pageRichTextFormat.Location = new System.Drawing.Point(4, 26);
			this.tabPreview_pageRichTextFormat.Name = "tabPreview_pageRichTextFormat";
			this.tabPreview_pageRichTextFormat.Padding = new System.Windows.Forms.Padding(3);
			this.tabPreview_pageRichTextFormat.Size = new System.Drawing.Size(371, 200);
			this.tabPreview_pageRichTextFormat.TabIndex = 1;
			this.tabPreview_pageRichTextFormat.Text = "#ClipboardType.Rtf";
			this.tabPreview_pageRichTextFormat.UseVisualStyleBackColor = true;
			// 
			// viewRtf
			// 
			this.viewRtf.Location = new System.Drawing.Point(88, 80);
			this.viewRtf.Name = "viewRtf";
			this.viewRtf.Size = new System.Drawing.Size(100, 96);
			this.viewRtf.TabIndex = 0;
			this.viewRtf.Text = "";
			// 
			// tabPreview_pageImage
			// 
			this.tabPreview_pageImage.Controls.Add(this.panelImage);
			this.tabPreview_pageImage.Location = new System.Drawing.Point(4, 26);
			this.tabPreview_pageImage.Name = "tabPreview_pageImage";
			this.tabPreview_pageImage.Padding = new System.Windows.Forms.Padding(3);
			this.tabPreview_pageImage.Size = new System.Drawing.Size(371, 200);
			this.tabPreview_pageImage.TabIndex = 2;
			this.tabPreview_pageImage.Text = "#ClipboardType.Image";
			this.tabPreview_pageImage.UseVisualStyleBackColor = true;
			// 
			// panelImage
			// 
			this.panelImage.Controls.Add(this.viewImage);
			this.panelImage.Location = new System.Drawing.Point(20, 52);
			this.panelImage.Name = "panelImage";
			this.panelImage.Size = new System.Drawing.Size(200, 100);
			this.panelImage.TabIndex = 1;
			// 
			// viewImage
			// 
			this.viewImage.Location = new System.Drawing.Point(29, 31);
			this.viewImage.Name = "viewImage";
			this.viewImage.Size = new System.Drawing.Size(100, 50);
			this.viewImage.TabIndex = 0;
			this.viewImage.TabStop = false;
			// 
			// tabPreview_pageFile
			// 
			this.tabPreview_pageFile.Controls.Add(this.viewFile);
			this.tabPreview_pageFile.Location = new System.Drawing.Point(4, 26);
			this.tabPreview_pageFile.Name = "tabPreview_pageFile";
			this.tabPreview_pageFile.Padding = new System.Windows.Forms.Padding(3);
			this.tabPreview_pageFile.Size = new System.Drawing.Size(371, 200);
			this.tabPreview_pageFile.TabIndex = 3;
			this.tabPreview_pageFile.Text = "#ClipboardType.File";
			this.tabPreview_pageFile.UseVisualStyleBackColor = true;
			// 
			// viewFile
			// 
			this.viewFile.Location = new System.Drawing.Point(36, 38);
			this.viewFile.Name = "viewFile";
			this.viewFile.Size = new System.Drawing.Size(121, 97);
			this.viewFile.TabIndex = 0;
			this.viewFile.UseCompatibleStateImageBehavior = false;
			// 
			// imageTab
			// 
			this.imageTab.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageTab.ImageSize = new System.Drawing.Size(16, 16);
			this.imageTab.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolClipboard
			// 
			this.toolClipboard.Dock = System.Windows.Forms.DockStyle.None;
			this.toolClipboard.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolClipboard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClipboard_itemTopmost,
            this.toolStripSeparator2,
            this.toolClipboard_itemSave,
            this.toolClipboard_itemClear,
            this.toolStripSeparator1,
            this.toolClipboard_itemType});
			this.toolClipboard.Location = new System.Drawing.Point(0, 0);
			this.toolClipboard.Name = "toolClipboard";
			this.toolClipboard.Size = new System.Drawing.Size(608, 25);
			this.toolClipboard.Stretch = true;
			this.toolClipboard.TabIndex = 0;
			// 
			// toolClipboard_itemTopmost
			// 
			this.toolClipboard_itemTopmost.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolClipboard_itemTopmost.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Pin;
			this.toolClipboard_itemTopmost.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemTopmost.Name = "toolClipboard_itemTopmost";
			this.toolClipboard_itemTopmost.Size = new System.Drawing.Size(23, 22);
			this.toolClipboard_itemTopmost.Text = ":clipboard/tips/topmost";
			this.toolClipboard_itemTopmost.Click += new System.EventHandler(this.toolClipboard_itemTopmost_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolClipboard_itemSave
			// 
			this.toolClipboard_itemSave.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Save;
			this.toolClipboard_itemSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemSave.Name = "toolClipboard_itemSave";
			this.toolClipboard_itemSave.Size = new System.Drawing.Size(182, 22);
			this.toolClipboard_itemSave.Text = ":clipboard/command/save";
			// 
			// toolClipboard_itemClear
			// 
			this.toolClipboard_itemClear.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Clear;
			this.toolClipboard_itemClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemClear.Name = "toolClipboard_itemClear";
			this.toolClipboard_itemClear.Size = new System.Drawing.Size(183, 22);
			this.toolClipboard_itemClear.Text = ":clipboard/command/clear";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolClipboard_itemType
			// 
			this.toolClipboard_itemType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClipboard_itemType_itemClipboard,
            this.toolClipboard_itemType_itemTemplate});
			this.toolClipboard_itemType.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_NotImpl;
			this.toolClipboard_itemType.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemType.Name = "toolClipboard_itemType";
			this.toolClipboard_itemType.Size = new System.Drawing.Size(166, 22);
			this.toolClipboard_itemType.Text = ":clipboard/select/type";
			// 
			// toolClipboard_itemType_itemClipboard
			// 
			this.toolClipboard_itemType_itemClipboard.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardCopy;
			this.toolClipboard_itemType_itemClipboard.Name = "toolClipboard_itemType_itemClipboard";
			this.toolClipboard_itemType_itemClipboard.Size = new System.Drawing.Size(224, 22);
			this.toolClipboard_itemType_itemClipboard.Text = ":clipboard/type/clipboard";
			this.toolClipboard_itemType_itemClipboard.Click += new System.EventHandler(this.toolClipboard_itemType_itemClipboard_Click);
			// 
			// toolClipboard_itemType_itemTemplate
			// 
			this.toolClipboard_itemType_itemTemplate.Name = "toolClipboard_itemType_itemTemplate";
			this.toolClipboard_itemType_itemTemplate.Size = new System.Drawing.Size(224, 22);
			this.toolClipboard_itemType_itemTemplate.Text = ":clipboard/type/template";
			this.toolClipboard_itemType_itemTemplate.Visible = false;
			// 
			// ClipboardForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(608, 278);
			this.Controls.Add(this.panelMain);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ClipboardForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = ":window/clipboard";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClipboardForm_FormClosing);
			this.LocationChanged += new System.EventHandler(this.ClipboardForm_LocationChanged);
			this.SizeChanged += new System.EventHandler(this.ClipboardForm_SizeChanged);
			this.panelMain.BottomToolStripPanel.ResumeLayout(false);
			this.panelMain.BottomToolStripPanel.PerformLayout();
			this.panelMain.ContentPanel.ResumeLayout(false);
			this.panelMain.TopToolStripPanel.ResumeLayout(false);
			this.panelMain.TopToolStripPanel.PerformLayout();
			this.panelMain.ResumeLayout(false);
			this.panelMain.PerformLayout();
			this.statusClipboard.ResumeLayout(false);
			this.statusClipboard.PerformLayout();
			this.panelClipboard.Panel1.ResumeLayout(false);
			this.panelClipboard.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelClipboard)).EndInit();
			this.panelClipboard.ResumeLayout(false);
			this.tabPreview.ResumeLayout(false);
			this.tabPreview_pageText.ResumeLayout(false);
			this.tabPreview_pageText.PerformLayout();
			this.tabPreview_pageRichTextFormat.ResumeLayout(false);
			this.tabPreview_pageImage.ResumeLayout(false);
			this.panelImage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.viewImage)).EndInit();
			this.tabPreview_pageFile.ResumeLayout(false);
			this.toolClipboard.ResumeLayout(false);
			this.toolClipboard.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer panelMain;
		private System.Windows.Forms.StatusStrip statusClipboard;
		private System.Windows.Forms.ToolStrip toolClipboard;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemSave;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemClear;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.SplitContainer panelClipboard;
		private System.Windows.Forms.TabControl tabPreview;
		private System.Windows.Forms.TabPage tabPreview_pageText;
		private System.Windows.Forms.TabPage tabPreview_pageRichTextFormat;
		private System.Windows.Forms.TabPage tabPreview_pageImage;
		private System.Windows.Forms.TabPage tabPreview_pageFile;
		private System.Windows.Forms.ToolStripDropDownButton toolClipboard_itemType;
		private System.Windows.Forms.ToolStripMenuItem toolClipboard_itemType_itemClipboard;
		private System.Windows.Forms.ToolStripMenuItem toolClipboard_itemType_itemTemplate;
		private System.Windows.Forms.ListBox listClipboard;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemTopmost;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ImageList imageTab;
		private System.Windows.Forms.TextBox viewText;
		private System.Windows.Forms.RichTextBox viewRtf;
		private System.Windows.Forms.PictureBox viewImage;
		private System.Windows.Forms.ListView viewFile;
		private System.Windows.Forms.Panel panelImage;
		private System.Windows.Forms.ToolStripStatusLabel statusClipboard_itemSelectedIndex;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripStatusLabel statusClipboard_itemCount;
	}
}