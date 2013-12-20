/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 21:39
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class SettingForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
			this.tabSetting = new System.Windows.Forms.TabControl();
			this.pageMain = new System.Windows.Forms.TabPage();
			this.pageLauncher = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listLauncherItems = new System.Windows.Forms.ListBox();
			this.toolLauncherItems = new System.Windows.Forms.ToolStrip();
			this.toolLauncherItems_filter = new System.Windows.Forms.ToolStripButton();
			this.toolLauncherItems_type = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolLauncherItems_type_full = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_type_name = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_type_display = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_type_tag = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_input = new System.Windows.Forms.ToolStripTextBox();
			this.pageCommand = new System.Windows.Forms.TabPage();
			this.inputCommandHideTime = new System.Windows.Forms.NumericUpDown();
			this.checkCommandTopmost = new System.Windows.Forms.CheckBox();
			this.titleCommandHideTime = new System.Windows.Forms.Label();
			this.commandCommandFont = new System.Windows.Forms.Button();
			this.titleCommandFont = new System.Windows.Forms.Label();
			this.pageToolbar = new System.Windows.Forms.TabPage();
			this.pageNote = new System.Windows.Forms.TabPage();
			this.pageDisplay = new System.Windows.Forms.TabPage();
			this.commandCancel = new System.Windows.Forms.Button();
			this.commandSubmit = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.toolLauncherItems_create = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolLauncherItems_remove = new System.Windows.Forms.ToolStripButton();
			this.tabSetting.SuspendLayout();
			this.pageLauncher.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolLauncherItems.SuspendLayout();
			this.pageCommand.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// tabSetting
			// 
			this.tabSetting.Controls.Add(this.pageMain);
			this.tabSetting.Controls.Add(this.pageLauncher);
			this.tabSetting.Controls.Add(this.pageCommand);
			this.tabSetting.Controls.Add(this.pageToolbar);
			this.tabSetting.Controls.Add(this.pageNote);
			this.tabSetting.Controls.Add(this.pageDisplay);
			this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSetting.Location = new System.Drawing.Point(3, 4);
			this.tabSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting.Name = "tabSetting";
			this.tabSetting.SelectedIndex = 0;
			this.tabSetting.Size = new System.Drawing.Size(468, 233);
			this.tabSetting.TabIndex = 0;
			// 
			// pageMain
			// 
			this.pageMain.Location = new System.Drawing.Point(4, 24);
			this.pageMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Name = "pageMain";
			this.pageMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Size = new System.Drawing.Size(460, 205);
			this.pageMain.TabIndex = 0;
			this.pageMain.Text = "{Pe}";
			this.pageMain.UseVisualStyleBackColor = true;
			// 
			// pageLauncher
			// 
			this.pageLauncher.Controls.Add(this.splitContainer1);
			this.pageLauncher.Location = new System.Drawing.Point(4, 24);
			this.pageLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageLauncher.Name = "pageLauncher";
			this.pageLauncher.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageLauncher.Size = new System.Drawing.Size(460, 205);
			this.pageLauncher.TabIndex = 1;
			this.pageLauncher.Text = "{LAUNCHER}";
			this.pageLauncher.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 4);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listLauncherItems);
			this.splitContainer1.Panel1.Controls.Add(this.toolLauncherItems);
			this.splitContainer1.Size = new System.Drawing.Size(454, 197);
			this.splitContainer1.SplitterDistance = 176;
			this.splitContainer1.TabIndex = 0;
			this.splitContainer1.SizeChanged += new System.EventHandler(this.SplitContainer1SizeChanged);
			// 
			// listLauncherItems
			// 
			this.listLauncherItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listLauncherItems.FormattingEnabled = true;
			this.listLauncherItems.ItemHeight = 15;
			this.listLauncherItems.Location = new System.Drawing.Point(0, 25);
			this.listLauncherItems.Name = "listLauncherItems";
			this.listLauncherItems.Size = new System.Drawing.Size(176, 172);
			this.listLauncherItems.TabIndex = 2;
			// 
			// toolLauncherItems
			// 
			this.toolLauncherItems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolLauncherItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolLauncherItems_create,
									this.toolLauncherItems_remove,
									this.toolStripSeparator1,
									this.toolLauncherItems_filter,
									this.toolLauncherItems_type,
									this.toolLauncherItems_input});
			this.toolLauncherItems.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.toolLauncherItems.Location = new System.Drawing.Point(0, 0);
			this.toolLauncherItems.Name = "toolLauncherItems";
			this.toolLauncherItems.Size = new System.Drawing.Size(176, 25);
			this.toolLauncherItems.TabIndex = 1;
			this.toolLauncherItems.Text = "toolStrip1";
			// 
			// toolLauncherItems_filter
			// 
			this.toolLauncherItems_filter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_filter.Image = ((System.Drawing.Image)(resources.GetObject("toolLauncherItems_filter.Image")));
			this.toolLauncherItems_filter.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_filter.Name = "toolLauncherItems_filter";
			this.toolLauncherItems_filter.Size = new System.Drawing.Size(23, 20);
			this.toolLauncherItems_filter.Text = "toolStripButton1";
			// 
			// toolLauncherItems_type
			// 
			this.toolLauncherItems_type.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_type.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolLauncherItems_type_full,
									this.toolLauncherItems_type_name,
									this.toolLauncherItems_type_display,
									this.toolLauncherItems_type_tag});
			this.toolLauncherItems_type.Image = ((System.Drawing.Image)(resources.GetObject("toolLauncherItems_type.Image")));
			this.toolLauncherItems_type.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_type.Name = "toolLauncherItems_type";
			this.toolLauncherItems_type.Size = new System.Drawing.Size(29, 20);
			this.toolLauncherItems_type.Text = "toolStripSplitButton1";
			// 
			// toolLauncherItems_type_full
			// 
			this.toolLauncherItems_type_full.Name = "toolLauncherItems_type_full";
			this.toolLauncherItems_type_full.Size = new System.Drawing.Size(193, 22);
			this.toolLauncherItems_type_full.Text = "toolStripMenuItem1";
			// 
			// toolLauncherItems_type_name
			// 
			this.toolLauncherItems_type_name.Name = "toolLauncherItems_type_name";
			this.toolLauncherItems_type_name.Size = new System.Drawing.Size(193, 22);
			this.toolLauncherItems_type_name.Text = "toolStripMenuItem2";
			// 
			// toolLauncherItems_type_display
			// 
			this.toolLauncherItems_type_display.Name = "toolLauncherItems_type_display";
			this.toolLauncherItems_type_display.Size = new System.Drawing.Size(193, 22);
			this.toolLauncherItems_type_display.Text = "toolStripMenuItem3";
			// 
			// toolLauncherItems_type_tag
			// 
			this.toolLauncherItems_type_tag.Name = "toolLauncherItems_type_tag";
			this.toolLauncherItems_type_tag.Size = new System.Drawing.Size(193, 22);
			this.toolLauncherItems_type_tag.Text = "toolStripMenuItem1";
			// 
			// toolLauncherItems_input
			// 
			this.toolLauncherItems_input.Name = "toolLauncherItems_input";
			this.toolLauncherItems_input.Size = new System.Drawing.Size(20, 25);
			// 
			// pageCommand
			// 
			this.pageCommand.Controls.Add(this.inputCommandHideTime);
			this.pageCommand.Controls.Add(this.checkCommandTopmost);
			this.pageCommand.Controls.Add(this.titleCommandHideTime);
			this.pageCommand.Controls.Add(this.commandCommandFont);
			this.pageCommand.Controls.Add(this.titleCommandFont);
			this.pageCommand.Location = new System.Drawing.Point(4, 24);
			this.pageCommand.Name = "pageCommand";
			this.pageCommand.Padding = new System.Windows.Forms.Padding(3);
			this.pageCommand.Size = new System.Drawing.Size(460, 205);
			this.pageCommand.TabIndex = 2;
			this.pageCommand.Text = "{COMMAND}";
			this.pageCommand.UseVisualStyleBackColor = true;
			// 
			// inputCommandHideTime
			// 
			this.inputCommandHideTime.Location = new System.Drawing.Point(156, 76);
			this.inputCommandHideTime.Name = "inputCommandHideTime";
			this.inputCommandHideTime.Size = new System.Drawing.Size(120, 23);
			this.inputCommandHideTime.TabIndex = 5;
			// 
			// checkCommandTopmost
			// 
			this.checkCommandTopmost.Location = new System.Drawing.Point(35, 125);
			this.checkCommandTopmost.Name = "checkCommandTopmost";
			this.checkCommandTopmost.Size = new System.Drawing.Size(104, 24);
			this.checkCommandTopmost.TabIndex = 4;
			this.checkCommandTopmost.Text = "{TOPMOST}";
			this.checkCommandTopmost.UseVisualStyleBackColor = true;
			// 
			// titleCommandHideTime
			// 
			this.titleCommandHideTime.Location = new System.Drawing.Point(35, 78);
			this.titleCommandHideTime.Name = "titleCommandHideTime";
			this.titleCommandHideTime.Size = new System.Drawing.Size(100, 23);
			this.titleCommandHideTime.TabIndex = 2;
			this.titleCommandHideTime.Text = "{HIDE TIME}";
			// 
			// commandCommandFont
			// 
			this.commandCommandFont.Location = new System.Drawing.Point(156, 23);
			this.commandCommandFont.Name = "commandCommandFont";
			this.commandCommandFont.Size = new System.Drawing.Size(171, 23);
			this.commandCommandFont.TabIndex = 1;
			this.commandCommandFont.Text = "{FAMILY} {PT} ...";
			this.commandCommandFont.UseVisualStyleBackColor = true;
			// 
			// titleCommandFont
			// 
			this.titleCommandFont.Location = new System.Drawing.Point(35, 28);
			this.titleCommandFont.Name = "titleCommandFont";
			this.titleCommandFont.Size = new System.Drawing.Size(100, 23);
			this.titleCommandFont.TabIndex = 0;
			this.titleCommandFont.Text = "{FONT}";
			// 
			// pageToolbar
			// 
			this.pageToolbar.Location = new System.Drawing.Point(4, 24);
			this.pageToolbar.Name = "pageToolbar";
			this.pageToolbar.Size = new System.Drawing.Size(460, 205);
			this.pageToolbar.TabIndex = 3;
			this.pageToolbar.Text = "{TOOLBAR}";
			this.pageToolbar.UseVisualStyleBackColor = true;
			// 
			// pageNote
			// 
			this.pageNote.Location = new System.Drawing.Point(4, 24);
			this.pageNote.Name = "pageNote";
			this.pageNote.Size = new System.Drawing.Size(460, 205);
			this.pageNote.TabIndex = 6;
			this.pageNote.Text = "{NOTE}";
			this.pageNote.UseVisualStyleBackColor = true;
			// 
			// pageDisplay
			// 
			this.pageDisplay.Location = new System.Drawing.Point(4, 24);
			this.pageDisplay.Name = "pageDisplay";
			this.pageDisplay.Size = new System.Drawing.Size(460, 205);
			this.pageDisplay.TabIndex = 5;
			this.pageDisplay.Text = "{DISPLAY}";
			this.pageDisplay.UseVisualStyleBackColor = true;
			// 
			// commandCancel
			// 
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(96, 4);
			this.commandCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(87, 29);
			this.commandCancel.TabIndex = 1;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			// 
			// commandSubmit
			// 
			this.commandSubmit.Location = new System.Drawing.Point(3, 4);
			this.commandSubmit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandSubmit.Name = "commandSubmit";
			this.commandSubmit.Size = new System.Drawing.Size(87, 29);
			this.commandSubmit.TabIndex = 0;
			this.commandSubmit.Text = "{OK}";
			this.commandSubmit.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tabSetting, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(474, 284);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.commandSubmit);
			this.flowLayoutPanel1.Controls.Add(this.commandCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(285, 244);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 37);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// toolLauncherItems_create
			// 
			this.toolLauncherItems_create.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_create.Image = ((System.Drawing.Image)(resources.GetObject("toolLauncherItems_create.Image")));
			this.toolLauncherItems_create.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_create.Name = "toolLauncherItems_create";
			this.toolLauncherItems_create.Size = new System.Drawing.Size(23, 20);
			this.toolLauncherItems_create.Text = "toolStripButton1";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
			// 
			// toolLauncherItems_remove
			// 
			this.toolLauncherItems_remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_remove.Image = ((System.Drawing.Image)(resources.GetObject("toolLauncherItems_remove.Image")));
			this.toolLauncherItems_remove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_remove.Name = "toolLauncherItems_remove";
			this.toolLauncherItems_remove.Size = new System.Drawing.Size(23, 20);
			this.toolLauncherItems_remove.Text = "toolStripButton1";
			// 
			// SettingForm
			// 
			this.AcceptButton = this.commandSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(474, 284);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "SettingForm";
			this.Text = "{SETTING}";
			this.tabSetting.ResumeLayout(false);
			this.pageLauncher.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.toolLauncherItems.ResumeLayout(false);
			this.toolLauncherItems.PerformLayout();
			this.pageCommand.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolStripButton toolLauncherItems_remove;
		private System.Windows.Forms.ToolStripButton toolLauncherItems_create;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_tag;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_display;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_name;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_full;
		private System.Windows.Forms.ListBox listLauncherItems;
		private System.Windows.Forms.ToolStripTextBox toolLauncherItems_input;
		private System.Windows.Forms.ToolStripDropDownButton toolLauncherItems_type;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolLauncherItems_filter;
		private System.Windows.Forms.ToolStrip toolLauncherItems;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabPage pageNote;
		private System.Windows.Forms.TabPage pageDisplay;
		private System.Windows.Forms.TabPage pageToolbar;
		private System.Windows.Forms.NumericUpDown inputCommandHideTime;
		private System.Windows.Forms.Label titleCommandHideTime;
		private System.Windows.Forms.CheckBox checkCommandTopmost;
		private System.Windows.Forms.Button commandCommandFont;
		private System.Windows.Forms.Label titleCommandFont;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.TabPage pageCommand;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.Button commandSubmit;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.TabPage pageLauncher;
		private System.Windows.Forms.TabPage pageMain;
		private System.Windows.Forms.TabControl tabSetting;
		
	}
}
