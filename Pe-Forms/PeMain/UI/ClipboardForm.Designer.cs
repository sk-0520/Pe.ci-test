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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipboardForm));
			this.panelMain = new System.Windows.Forms.ToolStripContainer();
			this.statusClipboard = new System.Windows.Forms.StatusStrip();
			this.statusClipboard_itemSelectedIndex = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusClipboard_itemCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusClipboard_itemLimitLeft = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusClipboard_itemLimitCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusClipboard_itemLimitRight = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelClipboard = new System.Windows.Forms.SplitContainer();
			this.panelItemStack = new System.Windows.Forms.ToolStripContainer();
			this.listItemStack = new System.Windows.Forms.ListBox();
			this.toolItemStack = new System.Windows.Forms.ToolStrip();
			this.toolItemStack_itemFiltering = new System.Windows.Forms.ToolStripButton();
			this.toolItemStack_itemFilter = new ContentTypeTextNet.Pe.PeMain.UI.Ex.FillToolStripTextBox();
			this.tabPreview = new System.Windows.Forms.TabControl();
			this.tabPreview_pageText = new System.Windows.Forms.TabPage();
			this.viewText = new System.Windows.Forms.TextBox();
			this.tabPreview_pageRtf = new System.Windows.Forms.TabPage();
			this.viewRtf = new System.Windows.Forms.RichTextBox();
			this.tabPreview_pageHtml = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.viewHtml = new ContentTypeTextNet.Pe.PeMain.UI.ClipboardForm.ClipboardWebBrowser();
			this.labelHtmlUri = new System.Windows.Forms.Label();
			this.commandHtmlUri = new System.Windows.Forms.Button();
			this.viewHtmlUri = new System.Windows.Forms.TextBox();
			this.tabPreview_pageImage = new System.Windows.Forms.TabPage();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.panelImage = new System.Windows.Forms.Panel();
			this.viewImage = new System.Windows.Forms.PictureBox();
			this.toolImage = new ContentTypeTextNet.Pe.PeMain.UI.Ex.ActiveToolStrip();
			this.toolImage_itemFill = new System.Windows.Forms.ToolStripButton();
			this.toolImage_itemRaw = new System.Windows.Forms.ToolStripButton();
			this.tabPreview_pageFile = new System.Windows.Forms.TabPage();
			this.viewFile = new System.Windows.Forms.ListView();
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextFileMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.contextFileMenu_itemExecute = new System.Windows.Forms.ToolStripMenuItem();
			this.contextFileMenu_itemOpenParentDirectory = new System.Windows.Forms.ToolStripMenuItem();
			this.tabPreview_pageRawTemplate = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.labelTemplateName = new System.Windows.Forms.Label();
			this.inputTemplateName = new System.Windows.Forms.TextBox();
			this.selectTemplateReplace = new System.Windows.Forms.CheckBox();
			this.selectTemplateProgram = new System.Windows.Forms.CheckBox();
			this.panelTemplateSource = new System.Windows.Forms.SplitContainer();
			this.inputTemplateSource = new System.Windows.Forms.TextBox();
			this.listReplace = new System.Windows.Forms.ListBox();
			this.tabPreview_pageReplaceTemplate = new System.Windows.Forms.TabPage();
			this.viewReplaceTemplate = new System.Windows.Forms.RichTextBox();
			this.imageTab = new System.Windows.Forms.ImageList(this.components);
			this.toolClipboard = new ContentTypeTextNet.Pe.PeMain.UI.Ex.ActiveToolStrip();
			this.toolClipboard_itemEnabled = new System.Windows.Forms.ToolStripButton();
			this.toolClipboard_itemTopmost = new System.Windows.Forms.ToolStripButton();
			this.DisableCloseToolStripSeparator2 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.toolClipboard_itemSave = new System.Windows.Forms.ToolStripButton();
			this.toolClipboard_itemRemove = new System.Windows.Forms.ToolStripSplitButton();
			this.toolClipboard_itemClear = new System.Windows.Forms.ToolStripMenuItem();
			this.toolClipboard_itemEmpty = new System.Windows.Forms.ToolStripMenuItem();
			this.DisableCloseToolStripSeparator1 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.toolClipboard_itemType = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolClipboard_itemType_itemClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.toolClipboard_itemType_itemTemplate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolClipboard_itemOutputClipboard = new System.Windows.Forms.ToolStripButton();
			this.panelMain.BottomToolStripPanel.SuspendLayout();
			this.panelMain.ContentPanel.SuspendLayout();
			this.panelMain.TopToolStripPanel.SuspendLayout();
			this.panelMain.SuspendLayout();
			this.statusClipboard.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelClipboard)).BeginInit();
			this.panelClipboard.Panel1.SuspendLayout();
			this.panelClipboard.Panel2.SuspendLayout();
			this.panelClipboard.SuspendLayout();
			this.panelItemStack.ContentPanel.SuspendLayout();
			this.panelItemStack.TopToolStripPanel.SuspendLayout();
			this.panelItemStack.SuspendLayout();
			this.toolItemStack.SuspendLayout();
			this.tabPreview.SuspendLayout();
			this.tabPreview_pageText.SuspendLayout();
			this.tabPreview_pageRtf.SuspendLayout();
			this.tabPreview_pageHtml.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tabPreview_pageImage.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.panelImage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewImage)).BeginInit();
			this.toolImage.SuspendLayout();
			this.tabPreview_pageFile.SuspendLayout();
			this.contextFileMenu.SuspendLayout();
			this.tabPreview_pageRawTemplate.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelTemplateSource)).BeginInit();
			this.panelTemplateSource.Panel1.SuspendLayout();
			this.panelTemplateSource.Panel2.SuspendLayout();
			this.panelTemplateSource.SuspendLayout();
			this.tabPreview_pageReplaceTemplate.SuspendLayout();
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
			this.panelMain.ContentPanel.Size = new System.Drawing.Size(608, 244);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.LeftToolStripPanelVisible = false;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panelMain.Name = "panelMain";
			this.panelMain.RightToolStripPanelVisible = false;
			this.panelMain.Size = new System.Drawing.Size(608, 291);
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
            this.statusClipboard_itemCount,
            this.statusClipboard_itemLimitLeft,
            this.statusClipboard_itemLimitCount,
            this.statusClipboard_itemLimitRight});
			this.statusClipboard.Location = new System.Drawing.Point(0, 0);
			this.statusClipboard.Name = "statusClipboard";
			this.statusClipboard.Size = new System.Drawing.Size(608, 22);
			this.statusClipboard.TabIndex = 0;
			// 
			// statusClipboard_itemSelectedIndex
			// 
			this.statusClipboard_itemSelectedIndex.Name = "statusClipboard_itemSelectedIndex";
			this.statusClipboard_itemSelectedIndex.Size = new System.Drawing.Size(19, 17);
			this.statusClipboard_itemSelectedIndex.Text = "☃";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(12, 17);
			this.toolStripStatusLabel2.Text = "/";
			// 
			// statusClipboard_itemCount
			// 
			this.statusClipboard_itemCount.Name = "statusClipboard_itemCount";
			this.statusClipboard_itemCount.Size = new System.Drawing.Size(19, 17);
			this.statusClipboard_itemCount.Text = "☃";
			// 
			// statusClipboard_itemLimitLeft
			// 
			this.statusClipboard_itemLimitLeft.Name = "statusClipboard_itemLimitLeft";
			this.statusClipboard_itemLimitLeft.Size = new System.Drawing.Size(12, 17);
			this.statusClipboard_itemLimitLeft.Text = "(";
			// 
			// statusClipboard_itemLimitCount
			// 
			this.statusClipboard_itemLimitCount.Name = "statusClipboard_itemLimitCount";
			this.statusClipboard_itemLimitCount.Size = new System.Drawing.Size(19, 17);
			this.statusClipboard_itemLimitCount.Text = "☃";
			// 
			// statusClipboard_itemLimitRight
			// 
			this.statusClipboard_itemLimitRight.Name = "statusClipboard_itemLimitRight";
			this.statusClipboard_itemLimitRight.Size = new System.Drawing.Size(12, 17);
			this.statusClipboard_itemLimitRight.Text = ")";
			// 
			// panelClipboard
			// 
			this.panelClipboard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelClipboard.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.panelClipboard.Location = new System.Drawing.Point(0, 0);
			this.panelClipboard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.panelClipboard.Name = "panelClipboard";
			// 
			// panelClipboard.Panel1
			// 
			this.panelClipboard.Panel1.Controls.Add(this.panelItemStack);
			// 
			// panelClipboard.Panel2
			// 
			this.panelClipboard.Panel2.Controls.Add(this.tabPreview);
			this.panelClipboard.Size = new System.Drawing.Size(608, 244);
			this.panelClipboard.SplitterDistance = 225;
			this.panelClipboard.SplitterWidth = 3;
			this.panelClipboard.TabIndex = 0;
			this.panelClipboard.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.panelClipboard_SplitterMoved);
			// 
			// panelItemStack
			// 
			this.panelItemStack.BottomToolStripPanelVisible = false;
			// 
			// panelItemStack.ContentPanel
			// 
			this.panelItemStack.ContentPanel.Controls.Add(this.listItemStack);
			this.panelItemStack.ContentPanel.Size = new System.Drawing.Size(225, 219);
			this.panelItemStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelItemStack.LeftToolStripPanelVisible = false;
			this.panelItemStack.Location = new System.Drawing.Point(0, 0);
			this.panelItemStack.Name = "panelItemStack";
			this.panelItemStack.RightToolStripPanelVisible = false;
			this.panelItemStack.Size = new System.Drawing.Size(225, 244);
			this.panelItemStack.TabIndex = 1;
			this.panelItemStack.Text = "toolStripContainer2";
			// 
			// panelItemStack.TopToolStripPanel
			// 
			this.panelItemStack.TopToolStripPanel.Controls.Add(this.toolItemStack);
			// 
			// listItemStack
			// 
			this.listItemStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listItemStack.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listItemStack.IntegralHeight = false;
			this.listItemStack.ItemHeight = 15;
			this.listItemStack.Location = new System.Drawing.Point(0, 0);
			this.listItemStack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.listItemStack.Name = "listItemStack";
			this.listItemStack.ScrollAlwaysVisible = true;
			this.listItemStack.Size = new System.Drawing.Size(225, 219);
			this.listItemStack.TabIndex = 0;
			this.listItemStack.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listClipboard_DrawItem);
			this.listItemStack.SelectedIndexChanged += new System.EventHandler(this.listClipboard_SelectedIndexChanged);
			this.listItemStack.DoubleClick += new System.EventHandler(this.listClipboard_DoubleClick);
			this.listItemStack.MouseLeave += new System.EventHandler(this.listClipboard_MouseLeave);
			this.listItemStack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listClipboard_MouseMove);
			// 
			// toolItemStack
			// 
			this.toolItemStack.Dock = System.Windows.Forms.DockStyle.None;
			this.toolItemStack.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolItemStack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolItemStack_itemFiltering,
            this.toolItemStack_itemFilter});
			this.toolItemStack.Location = new System.Drawing.Point(0, 0);
			this.toolItemStack.Name = "toolItemStack";
			this.toolItemStack.Size = new System.Drawing.Size(225, 25);
			this.toolItemStack.Stretch = true;
			this.toolItemStack.TabIndex = 0;
			// 
			// toolItemStack_itemFiltering
			// 
			this.toolItemStack_itemFiltering.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolItemStack_itemFiltering.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolItemStack_itemFiltering.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolItemStack_itemFiltering.Name = "toolItemStack_itemFiltering";
			this.toolItemStack_itemFiltering.Size = new System.Drawing.Size(23, 22);
			this.toolItemStack_itemFiltering.Text = ":clipboard/tool/filtering";
			this.toolItemStack_itemFiltering.Click += new System.EventHandler(this.toolItemStack_itemFiltering_Click);
			// 
			// toolItemStack_itemFilter
			// 
			this.toolItemStack_itemFilter.Name = "toolItemStack_itemFilter";
			this.toolItemStack_itemFilter.Size = new System.Drawing.Size(168, 25);
			this.toolItemStack_itemFilter.TextChanged += new System.EventHandler(this.toolItemStack_itemFilter_TextChanged);
			// 
			// tabPreview
			// 
			this.tabPreview.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabPreview.Controls.Add(this.tabPreview_pageText);
			this.tabPreview.Controls.Add(this.tabPreview_pageRtf);
			this.tabPreview.Controls.Add(this.tabPreview_pageHtml);
			this.tabPreview.Controls.Add(this.tabPreview_pageImage);
			this.tabPreview.Controls.Add(this.tabPreview_pageFile);
			this.tabPreview.Controls.Add(this.tabPreview_pageRawTemplate);
			this.tabPreview.Controls.Add(this.tabPreview_pageReplaceTemplate);
			this.tabPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPreview.ImageList = this.imageTab;
			this.tabPreview.Location = new System.Drawing.Point(0, 0);
			this.tabPreview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview.Name = "tabPreview";
			this.tabPreview.SelectedIndex = 0;
			this.tabPreview.Size = new System.Drawing.Size(380, 244);
			this.tabPreview.TabIndex = 0;
			this.tabPreview.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabPreview_Selecting);
			// 
			// tabPreview_pageText
			// 
			this.tabPreview_pageText.Controls.Add(this.viewText);
			this.tabPreview_pageText.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageText.Name = "tabPreview_pageText";
			this.tabPreview_pageText.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageText.Size = new System.Drawing.Size(372, 213);
			this.tabPreview_pageText.TabIndex = 0;
			this.tabPreview_pageText.Text = "#ClipboardType.Text";
			this.tabPreview_pageText.UseVisualStyleBackColor = true;
			// 
			// viewText
			// 
			this.viewText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewText.Location = new System.Drawing.Point(3, 2);
			this.viewText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.viewText.Multiline = true;
			this.viewText.Name = "viewText";
			this.viewText.ReadOnly = true;
			this.viewText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.viewText.Size = new System.Drawing.Size(366, 209);
			this.viewText.TabIndex = 0;
			this.viewText.WordWrap = false;
			// 
			// tabPreview_pageRtf
			// 
			this.tabPreview_pageRtf.Controls.Add(this.viewRtf);
			this.tabPreview_pageRtf.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageRtf.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageRtf.Name = "tabPreview_pageRtf";
			this.tabPreview_pageRtf.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageRtf.Size = new System.Drawing.Size(372, 213);
			this.tabPreview_pageRtf.TabIndex = 1;
			this.tabPreview_pageRtf.Text = "#ClipboardType.Rtf";
			this.tabPreview_pageRtf.UseVisualStyleBackColor = true;
			// 
			// viewRtf
			// 
			this.viewRtf.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewRtf.EnableAutoDragDrop = true;
			this.viewRtf.HideSelection = false;
			this.viewRtf.Location = new System.Drawing.Point(3, 2);
			this.viewRtf.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.viewRtf.Name = "viewRtf";
			this.viewRtf.ReadOnly = true;
			this.viewRtf.ShortcutsEnabled = false;
			this.viewRtf.Size = new System.Drawing.Size(366, 209);
			this.viewRtf.TabIndex = 0;
			this.viewRtf.Text = "";
			this.viewRtf.WordWrap = false;
			// 
			// tabPreview_pageHtml
			// 
			this.tabPreview_pageHtml.Controls.Add(this.tableLayoutPanel2);
			this.tabPreview_pageHtml.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageHtml.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageHtml.Name = "tabPreview_pageHtml";
			this.tabPreview_pageHtml.Size = new System.Drawing.Size(372, 213);
			this.tabPreview_pageHtml.TabIndex = 4;
			this.tabPreview_pageHtml.Text = "#ClipboardType.Html";
			this.tabPreview_pageHtml.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.viewHtml, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.labelHtmlUri, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.commandHtmlUri, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.viewHtmlUri, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(372, 213);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// viewHtml
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.viewHtml, 3);
			this.viewHtml.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewHtml.IsWebBrowserContextMenuEnabled = false;
			this.viewHtml.Location = new System.Drawing.Point(3, 33);
			this.viewHtml.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.viewHtml.MinimumSize = new System.Drawing.Size(20, 20);
			this.viewHtml.Name = "viewHtml";
			this.viewHtml.ScriptErrorsSuppressed = true;
			this.viewHtml.Size = new System.Drawing.Size(366, 178);
			this.viewHtml.TabIndex = 0;
			this.viewHtml.ShowMessage += new System.EventHandler<ContentTypeTextNet.Pe.PeMain.UI.Ex.ShowMessageEventArgs>(this.viewHtml_ShowMessage);
			// 
			// labelHtmlUri
			// 
			this.labelHtmlUri.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelHtmlUri.AutoSize = true;
			this.labelHtmlUri.Location = new System.Drawing.Point(3, 8);
			this.labelHtmlUri.Name = "labelHtmlUri";
			this.labelHtmlUri.Size = new System.Drawing.Size(118, 15);
			this.labelHtmlUri.TabIndex = 3;
			this.labelHtmlUri.Text = ":clipboard/label/uri";
			// 
			// commandHtmlUri
			// 
			this.commandHtmlUri.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandHtmlUri.Location = new System.Drawing.Point(341, 3);
			this.commandHtmlUri.Name = "commandHtmlUri";
			this.commandHtmlUri.Size = new System.Drawing.Size(28, 25);
			this.commandHtmlUri.TabIndex = 2;
			this.commandHtmlUri.UseVisualStyleBackColor = true;
			this.commandHtmlUri.Click += new System.EventHandler(this.commandHtmlUri_Click);
			// 
			// viewHtmlUri
			// 
			this.viewHtmlUri.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.viewHtmlUri.Location = new System.Drawing.Point(127, 4);
			this.viewHtmlUri.Name = "viewHtmlUri";
			this.viewHtmlUri.ReadOnly = true;
			this.viewHtmlUri.Size = new System.Drawing.Size(208, 23);
			this.viewHtmlUri.TabIndex = 1;
			// 
			// tabPreview_pageImage
			// 
			this.tabPreview_pageImage.Controls.Add(this.toolStripContainer1);
			this.tabPreview_pageImage.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageImage.Name = "tabPreview_pageImage";
			this.tabPreview_pageImage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageImage.Size = new System.Drawing.Size(372, 213);
			this.tabPreview_pageImage.TabIndex = 2;
			this.tabPreview_pageImage.Text = "#ClipboardType.Image";
			this.tabPreview_pageImage.UseVisualStyleBackColor = true;
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.panelImage);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(366, 184);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(3, 2);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(366, 209);
			this.toolStripContainer1.TabIndex = 2;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolImage);
			// 
			// panelImage
			// 
			this.panelImage.AutoScroll = true;
			this.panelImage.Controls.Add(this.viewImage);
			this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelImage.Location = new System.Drawing.Point(0, 0);
			this.panelImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.panelImage.Name = "panelImage";
			this.panelImage.Size = new System.Drawing.Size(366, 184);
			this.panelImage.TabIndex = 1;
			// 
			// viewImage
			// 
			this.viewImage.Location = new System.Drawing.Point(3, 2);
			this.viewImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.viewImage.Name = "viewImage";
			this.viewImage.Size = new System.Drawing.Size(100, 50);
			this.viewImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.viewImage.TabIndex = 0;
			this.viewImage.TabStop = false;
			// 
			// toolImage
			// 
			this.toolImage.Dock = System.Windows.Forms.DockStyle.None;
			this.toolImage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolImage_itemFill,
            this.toolImage_itemRaw});
			this.toolImage.Location = new System.Drawing.Point(0, 0);
			this.toolImage.Name = "toolImage";
			this.toolImage.Size = new System.Drawing.Size(366, 25);
			this.toolImage.Stretch = true;
			this.toolImage.TabIndex = 0;
			// 
			// toolImage_itemFill
			// 
			this.toolImage_itemFill.Image = ((System.Drawing.Image)(resources.GetObject("toolImage_itemFill.Image")));
			this.toolImage_itemFill.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolImage_itemFill.Name = "toolImage_itemFill";
			this.toolImage_itemFill.Size = new System.Drawing.Size(145, 22);
			this.toolImage_itemFill.Text = ":clipboard/image/fill";
			this.toolImage_itemFill.Click += new System.EventHandler(this.toolImage_itemRaw_Click);
			// 
			// toolImage_itemRaw
			// 
			this.toolImage_itemRaw.Image = ((System.Drawing.Image)(resources.GetObject("toolImage_itemRaw.Image")));
			this.toolImage_itemRaw.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolImage_itemRaw.Name = "toolImage_itemRaw";
			this.toolImage_itemRaw.Size = new System.Drawing.Size(154, 22);
			this.toolImage_itemRaw.Text = ":clipboard/image/raw";
			this.toolImage_itemRaw.Click += new System.EventHandler(this.toolImage_itemRaw_Click);
			// 
			// tabPreview_pageFile
			// 
			this.tabPreview_pageFile.Controls.Add(this.viewFile);
			this.tabPreview_pageFile.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageFile.Name = "tabPreview_pageFile";
			this.tabPreview_pageFile.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageFile.Size = new System.Drawing.Size(372, 213);
			this.tabPreview_pageFile.TabIndex = 3;
			this.tabPreview_pageFile.Text = "#ClipboardType.File";
			this.tabPreview_pageFile.UseVisualStyleBackColor = true;
			// 
			// viewFile
			// 
			this.viewFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnPath});
			this.viewFile.ContextMenuStrip = this.contextFileMenu;
			this.viewFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewFile.FullRowSelect = true;
			this.viewFile.GridLines = true;
			this.viewFile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.viewFile.Location = new System.Drawing.Point(3, 2);
			this.viewFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.viewFile.MultiSelect = false;
			this.viewFile.Name = "viewFile";
			this.viewFile.Size = new System.Drawing.Size(366, 209);
			this.viewFile.TabIndex = 0;
			this.viewFile.UseCompatibleStateImageBehavior = false;
			this.viewFile.View = System.Windows.Forms.View.Details;
			this.viewFile.DoubleClick += new System.EventHandler(this.viewFile_DoubleClick);
			this.viewFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.viewFile_KeyDown);
			// 
			// columnName
			// 
			this.columnName.Text = ":clipboard/header/name";
			// 
			// columnPath
			// 
			this.columnPath.Text = ":clipboard/header/path";
			// 
			// contextFileMenu
			// 
			this.contextFileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextFileMenu_itemExecute,
            this.contextFileMenu_itemOpenParentDirectory});
			this.contextFileMenu.Name = "contextFileMenu";
			this.contextFileMenu.Size = new System.Drawing.Size(291, 48);
			this.contextFileMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextFileMenu_Opening);
			// 
			// contextFileMenu_itemExecute
			// 
			this.contextFileMenu_itemExecute.Name = "contextFileMenu_itemExecute";
			this.contextFileMenu_itemExecute.Size = new System.Drawing.Size(290, 22);
			this.contextFileMenu_itemExecute.Text = ":clipboard/menu/file/open-file";
			this.contextFileMenu_itemExecute.Click += new System.EventHandler(this.contextFileMenu_itemExecute_Click);
			// 
			// contextFileMenu_itemOpenParentDirectory
			// 
			this.contextFileMenu_itemOpenParentDirectory.Name = "contextFileMenu_itemOpenParentDirectory";
			this.contextFileMenu_itemOpenParentDirectory.Size = new System.Drawing.Size(290, 22);
			this.contextFileMenu_itemOpenParentDirectory.Text = ":clipboard/menu/file/open-parent-dir";
			this.contextFileMenu_itemOpenParentDirectory.Click += new System.EventHandler(this.contextFileMenu_itemOpenParentDirectory_Click);
			// 
			// tabPreview_pageRawTemplate
			// 
			this.tabPreview_pageRawTemplate.Controls.Add(this.tableLayoutPanel1);
			this.tabPreview_pageRawTemplate.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageRawTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageRawTemplate.Name = "tabPreview_pageRawTemplate";
			this.tabPreview_pageRawTemplate.Size = new System.Drawing.Size(372, 213);
			this.tabPreview_pageRawTemplate.TabIndex = 5;
			this.tabPreview_pageRawTemplate.Text = ":clipboard/page/raw-template";
			this.tabPreview_pageRawTemplate.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panelTemplateSource, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(372, 213);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.labelTemplateName);
			this.flowLayoutPanel1.Controls.Add(this.inputTemplateName);
			this.flowLayoutPanel1.Controls.Add(this.selectTemplateReplace);
			this.flowLayoutPanel1.Controls.Add(this.selectTemplateProgram);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 2);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(366, 73);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// labelTemplateName
			// 
			this.labelTemplateName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelTemplateName.AutoSize = true;
			this.labelTemplateName.Location = new System.Drawing.Point(3, 6);
			this.labelTemplateName.Name = "labelTemplateName";
			this.labelTemplateName.Size = new System.Drawing.Size(194, 15);
			this.labelTemplateName.TabIndex = 3;
			this.labelTemplateName.Text = ":clipboard/label/template-name";
			// 
			// inputTemplateName
			// 
			this.inputTemplateName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.inputTemplateName.Location = new System.Drawing.Point(203, 2);
			this.inputTemplateName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.inputTemplateName.Name = "inputTemplateName";
			this.inputTemplateName.Size = new System.Drawing.Size(157, 23);
			this.inputTemplateName.TabIndex = 0;
			// 
			// selectTemplateReplace
			// 
			this.selectTemplateReplace.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.selectTemplateReplace.AutoSize = true;
			this.selectTemplateReplace.Location = new System.Drawing.Point(3, 29);
			this.selectTemplateReplace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.selectTemplateReplace.Name = "selectTemplateReplace";
			this.selectTemplateReplace.Size = new System.Drawing.Size(228, 19);
			this.selectTemplateReplace.TabIndex = 1;
			this.selectTemplateReplace.Text = ":clipboard/check/template-replace";
			this.selectTemplateReplace.UseVisualStyleBackColor = true;
			this.selectTemplateReplace.CheckedChanged += new System.EventHandler(this.selectTemplateReplace_CheckedChanged);
			// 
			// selectTemplateProgram
			// 
			this.selectTemplateProgram.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.selectTemplateProgram.AutoSize = true;
			this.selectTemplateProgram.Location = new System.Drawing.Point(3, 52);
			this.selectTemplateProgram.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.selectTemplateProgram.Name = "selectTemplateProgram";
			this.selectTemplateProgram.Size = new System.Drawing.Size(236, 19);
			this.selectTemplateProgram.TabIndex = 4;
			this.selectTemplateProgram.Text = ":clipboard/check/template-program";
			this.selectTemplateProgram.UseVisualStyleBackColor = true;
			this.selectTemplateProgram.CheckedChanged += new System.EventHandler(this.selectTemplateMacro_CheckedChanged);
			// 
			// panelTemplateSource
			// 
			this.panelTemplateSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTemplateSource.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.panelTemplateSource.Location = new System.Drawing.Point(3, 79);
			this.panelTemplateSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.panelTemplateSource.Name = "panelTemplateSource";
			// 
			// panelTemplateSource.Panel1
			// 
			this.panelTemplateSource.Panel1.Controls.Add(this.inputTemplateSource);
			// 
			// panelTemplateSource.Panel2
			// 
			this.panelTemplateSource.Panel2.Controls.Add(this.listReplace);
			this.panelTemplateSource.Size = new System.Drawing.Size(366, 132);
			this.panelTemplateSource.SplitterDistance = 263;
			this.panelTemplateSource.SplitterWidth = 3;
			this.panelTemplateSource.TabIndex = 7;
			this.panelTemplateSource.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.panelTemplateSource_SplitterMoved);
			// 
			// inputTemplateSource
			// 
			this.inputTemplateSource.AcceptsReturn = true;
			this.inputTemplateSource.AcceptsTab = true;
			this.inputTemplateSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputTemplateSource.HideSelection = false;
			this.inputTemplateSource.Location = new System.Drawing.Point(0, 0);
			this.inputTemplateSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.inputTemplateSource.Multiline = true;
			this.inputTemplateSource.Name = "inputTemplateSource";
			this.inputTemplateSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.inputTemplateSource.Size = new System.Drawing.Size(263, 132);
			this.inputTemplateSource.TabIndex = 0;
			this.inputTemplateSource.WordWrap = false;
			// 
			// listReplace
			// 
			this.listReplace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listReplace.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listReplace.FormattingEnabled = true;
			this.listReplace.IntegralHeight = false;
			this.listReplace.Location = new System.Drawing.Point(0, 0);
			this.listReplace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.listReplace.Name = "listReplace";
			this.listReplace.Size = new System.Drawing.Size(100, 132);
			this.listReplace.TabIndex = 0;
			this.listReplace.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listReplace_DrawItem);
			this.listReplace.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listReplace_MeasureItem);
			this.listReplace.DoubleClick += new System.EventHandler(this.listReplace_DoubleClick);
			this.listReplace.Resize += new System.EventHandler(this.listReplace_Resize);
			// 
			// tabPreview_pageReplaceTemplate
			// 
			this.tabPreview_pageReplaceTemplate.Controls.Add(this.viewReplaceTemplate);
			this.tabPreview_pageReplaceTemplate.Location = new System.Drawing.Point(4, 27);
			this.tabPreview_pageReplaceTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPreview_pageReplaceTemplate.Name = "tabPreview_pageReplaceTemplate";
			this.tabPreview_pageReplaceTemplate.Size = new System.Drawing.Size(372, 213);
			this.tabPreview_pageReplaceTemplate.TabIndex = 6;
			this.tabPreview_pageReplaceTemplate.Text = ":clipboard/page/replace-template";
			this.tabPreview_pageReplaceTemplate.UseVisualStyleBackColor = true;
			// 
			// viewReplaceTemplate
			// 
			this.viewReplaceTemplate.BackColor = System.Drawing.Color.White;
			this.viewReplaceTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewReplaceTemplate.ForeColor = System.Drawing.Color.Black;
			this.viewReplaceTemplate.Location = new System.Drawing.Point(0, 0);
			this.viewReplaceTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.viewReplaceTemplate.Name = "viewReplaceTemplate";
			this.viewReplaceTemplate.ReadOnly = true;
			this.viewReplaceTemplate.Size = new System.Drawing.Size(372, 213);
			this.viewReplaceTemplate.TabIndex = 0;
			this.viewReplaceTemplate.Text = "";
			this.viewReplaceTemplate.WordWrap = false;
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
            this.toolClipboard_itemEnabled,
            this.toolClipboard_itemTopmost,
            this.DisableCloseToolStripSeparator2,
            this.toolClipboard_itemSave,
            this.toolClipboard_itemRemove,
            this.DisableCloseToolStripSeparator1,
            this.toolClipboard_itemType,
            this.toolClipboard_itemOutputClipboard});
			this.toolClipboard.Location = new System.Drawing.Point(0, 0);
			this.toolClipboard.Name = "toolClipboard";
			this.toolClipboard.Size = new System.Drawing.Size(608, 25);
			this.toolClipboard.Stretch = true;
			this.toolClipboard.TabIndex = 0;
			// 
			// toolClipboard_itemEnabled
			// 
			this.toolClipboard_itemEnabled.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemEnabled.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemEnabled.Name = "toolClipboard_itemEnabled";
			this.toolClipboard_itemEnabled.Size = new System.Drawing.Size(199, 22);
			this.toolClipboard_itemEnabled.Text = ":clipboard/command/enabled";
			this.toolClipboard_itemEnabled.ToolTipText = ":clipboard/tips/enabled";
			this.toolClipboard_itemEnabled.Click += new System.EventHandler(this.toolClipboard_itemEnabled_Click);
			// 
			// toolClipboard_itemTopmost
			// 
			this.toolClipboard_itemTopmost.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolClipboard_itemTopmost.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemTopmost.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemTopmost.Name = "toolClipboard_itemTopmost";
			this.toolClipboard_itemTopmost.Size = new System.Drawing.Size(23, 22);
			this.toolClipboard_itemTopmost.Text = ":clipboard/tips/topmost";
			this.toolClipboard_itemTopmost.Click += new System.EventHandler(this.toolClipboard_itemTopmost_Click);
			// 
			// DisableCloseToolStripSeparator2
			// 
			this.DisableCloseToolStripSeparator2.Name = "DisableCloseToolStripSeparator2";
			this.DisableCloseToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolClipboard_itemSave
			// 
			this.toolClipboard_itemSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolClipboard_itemSave.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemSave.Name = "toolClipboard_itemSave";
			this.toolClipboard_itemSave.Size = new System.Drawing.Size(23, 22);
			this.toolClipboard_itemSave.Text = ":clipboard/command/save";
			this.toolClipboard_itemSave.Click += new System.EventHandler(this.toolClipboard_itemSave_Click);
			// 
			// toolClipboard_itemRemove
			// 
			this.toolClipboard_itemRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolClipboard_itemRemove.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClipboard_itemClear,
            this.toolClipboard_itemEmpty});
			this.toolClipboard_itemRemove.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemRemove.Name = "toolClipboard_itemRemove";
			this.toolClipboard_itemRemove.Size = new System.Drawing.Size(32, 22);
			this.toolClipboard_itemRemove.Text = ":clipboard/command/remove";
			this.toolClipboard_itemRemove.ButtonClick += new System.EventHandler(this.toolClipboard_itemClear_ButtonClick);
			// 
			// toolClipboard_itemClear
			// 
			this.toolClipboard_itemClear.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemClear.Name = "toolClipboard_itemClear";
			this.toolClipboard_itemClear.Size = new System.Drawing.Size(239, 22);
			this.toolClipboard_itemClear.Text = ":clipboard/command/clear";
			this.toolClipboard_itemClear.Click += new System.EventHandler(this.toolClipboard_itemClear_Click);
			// 
			// toolClipboard_itemEmpty
			// 
			this.toolClipboard_itemEmpty.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemEmpty.Name = "toolClipboard_itemEmpty";
			this.toolClipboard_itemEmpty.Size = new System.Drawing.Size(239, 22);
			this.toolClipboard_itemEmpty.Text = ":clipboard/command/empty";
			this.toolClipboard_itemEmpty.Click += new System.EventHandler(this.toolClipboard_itemEmpty_Click);
			// 
			// DisableCloseToolStripSeparator1
			// 
			this.DisableCloseToolStripSeparator1.Name = "DisableCloseToolStripSeparator1";
			this.DisableCloseToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolClipboard_itemType
			// 
			this.toolClipboard_itemType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClipboard_itemType_itemClipboard,
            this.toolClipboard_itemType_itemTemplate});
			this.toolClipboard_itemType.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_NotImpl;
			this.toolClipboard_itemType.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemType.Name = "toolClipboard_itemType";
			this.toolClipboard_itemType.Size = new System.Drawing.Size(165, 22);
			this.toolClipboard_itemType.Text = ":clipboard/select/type";
			// 
			// toolClipboard_itemType_itemClipboard
			// 
			this.toolClipboard_itemType_itemClipboard.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemType_itemClipboard.Name = "toolClipboard_itemType_itemClipboard";
			this.toolClipboard_itemType_itemClipboard.Size = new System.Drawing.Size(246, 22);
			this.toolClipboard_itemType_itemClipboard.Text = "#ClipboardListType.History";
			this.toolClipboard_itemType_itemClipboard.Click += new System.EventHandler(this.toolClipboard_itemType_itemClipboard_Click);
			// 
			// toolClipboard_itemType_itemTemplate
			// 
			this.toolClipboard_itemType_itemTemplate.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolClipboard_itemType_itemTemplate.Name = "toolClipboard_itemType_itemTemplate";
			this.toolClipboard_itemType_itemTemplate.Size = new System.Drawing.Size(246, 22);
			this.toolClipboard_itemType_itemTemplate.Text = "#ClipboardListType.Template";
			this.toolClipboard_itemType_itemTemplate.Click += new System.EventHandler(this.toolClipboard_itemType_itemClipboard_Click);
			// 
			// toolClipboard_itemOutputClipboard
			// 
			this.toolClipboard_itemOutputClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolClipboard_itemOutputClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemOutputClipboard.Name = "toolClipboard_itemOutputClipboard";
			this.toolClipboard_itemOutputClipboard.Size = new System.Drawing.Size(232, 22);
			this.toolClipboard_itemOutputClipboard.Text = ":clipboard/tool/output-using-clipboard";
			this.toolClipboard_itemOutputClipboard.Click += new System.EventHandler(this.toolClipboard_itemOutputClipboard_Click);
			// 
			// ClipboardForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(608, 291);
			this.Controls.Add(this.panelMain);
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
			this.VisibleChanged += new System.EventHandler(this.ClipboardForm_VisibleChanged);
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
			this.panelItemStack.ContentPanel.ResumeLayout(false);
			this.panelItemStack.TopToolStripPanel.ResumeLayout(false);
			this.panelItemStack.TopToolStripPanel.PerformLayout();
			this.panelItemStack.ResumeLayout(false);
			this.panelItemStack.PerformLayout();
			this.toolItemStack.ResumeLayout(false);
			this.toolItemStack.PerformLayout();
			this.tabPreview.ResumeLayout(false);
			this.tabPreview_pageText.ResumeLayout(false);
			this.tabPreview_pageText.PerformLayout();
			this.tabPreview_pageRtf.ResumeLayout(false);
			this.tabPreview_pageHtml.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tabPreview_pageImage.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.panelImage.ResumeLayout(false);
			this.panelImage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewImage)).EndInit();
			this.toolImage.ResumeLayout(false);
			this.toolImage.PerformLayout();
			this.tabPreview_pageFile.ResumeLayout(false);
			this.contextFileMenu.ResumeLayout(false);
			this.tabPreview_pageRawTemplate.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.panelTemplateSource.Panel1.ResumeLayout(false);
			this.panelTemplateSource.Panel1.PerformLayout();
			this.panelTemplateSource.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelTemplateSource)).EndInit();
			this.panelTemplateSource.ResumeLayout(false);
			this.tabPreview_pageReplaceTemplate.ResumeLayout(false);
			this.toolClipboard.ResumeLayout(false);
			this.toolClipboard.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer panelMain;
		private System.Windows.Forms.StatusStrip statusClipboard;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.ActiveToolStrip toolClipboard;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemSave;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator1;
		private System.Windows.Forms.SplitContainer panelClipboard;
		private System.Windows.Forms.TabControl tabPreview;
		private System.Windows.Forms.TabPage tabPreview_pageText;
		private System.Windows.Forms.TabPage tabPreview_pageRtf;
		private System.Windows.Forms.TabPage tabPreview_pageImage;
		private System.Windows.Forms.TabPage tabPreview_pageFile;
		private System.Windows.Forms.ToolStripDropDownButton toolClipboard_itemType;
		private System.Windows.Forms.ToolStripMenuItem toolClipboard_itemType_itemClipboard;
		private System.Windows.Forms.ToolStripMenuItem toolClipboard_itemType_itemTemplate;
		private System.Windows.Forms.ListBox listItemStack;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemTopmost;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator2;
		private System.Windows.Forms.ImageList imageTab;
		private System.Windows.Forms.TextBox viewText;
		private System.Windows.Forms.RichTextBox viewRtf;
		private System.Windows.Forms.PictureBox viewImage;
		private System.Windows.Forms.ListView viewFile;
		private System.Windows.Forms.ToolStripStatusLabel statusClipboard_itemSelectedIndex;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripStatusLabel statusClipboard_itemCount;
		private System.Windows.Forms.ToolStripSplitButton toolClipboard_itemRemove;
		private System.Windows.Forms.ToolStripMenuItem toolClipboard_itemEmpty;
		private System.Windows.Forms.Panel panelImage;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnPath;
		private System.Windows.Forms.TabPage tabPreview_pageHtml;
		private ClipboardWebBrowser viewHtml;
		private System.Windows.Forms.ToolStripStatusLabel statusClipboard_itemLimitLeft;
		private System.Windows.Forms.ToolStripStatusLabel statusClipboard_itemLimitCount;
		private System.Windows.Forms.ToolStripStatusLabel statusClipboard_itemLimitRight;
		private System.Windows.Forms.ToolStripMenuItem toolClipboard_itemClear;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemEnabled;
		private System.Windows.Forms.TabPage tabPreview_pageRawTemplate;
		private System.Windows.Forms.TextBox inputTemplateName;
		private System.Windows.Forms.TabPage tabPreview_pageReplaceTemplate;
		private System.Windows.Forms.RichTextBox viewReplaceTemplate;
		private System.Windows.Forms.Label labelTemplateName;
		private System.Windows.Forms.CheckBox selectTemplateReplace;
		private System.Windows.Forms.TextBox inputTemplateSource;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.SplitContainer panelTemplateSource;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ListBox listReplace;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TextBox viewHtmlUri;
		private System.Windows.Forms.Button commandHtmlUri;
		private System.Windows.Forms.Label labelHtmlUri;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.ActiveToolStrip toolImage;
		private System.Windows.Forms.ToolStripButton toolImage_itemRaw;
		private System.Windows.Forms.ToolStripButton toolImage_itemFill;
		private System.Windows.Forms.ToolStripContainer panelItemStack;
		private System.Windows.Forms.ToolStrip toolItemStack;
		private Ex.FillToolStripTextBox toolItemStack_itemFilter;
		private System.Windows.Forms.ToolStripButton toolItemStack_itemFiltering;
		private System.Windows.Forms.CheckBox selectTemplateProgram;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemOutputClipboard;
		private System.Windows.Forms.ContextMenuStrip contextFileMenu;
		private System.Windows.Forms.ToolStripMenuItem contextFileMenu_itemExecute;
		private System.Windows.Forms.ToolStripMenuItem contextFileMenu_itemOpenParentDirectory;
	}
}