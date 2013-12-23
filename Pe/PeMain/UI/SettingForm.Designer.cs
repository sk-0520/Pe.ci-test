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
			this.tabSetting = new System.Windows.Forms.TabControl();
			this.pageMain = new System.Windows.Forms.TabPage();
			this.pageLauncher = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.selecterLauncher = new PeMain.UI.LauncherItemSelectControl();
			this.labelLauncherNote = new System.Windows.Forms.Label();
			this.inputLauncherIconIndex = new System.Windows.Forms.NumericUpDown();
			this.labelLauncherTag = new System.Windows.Forms.Label();
			this.labelLauncherIconPath = new System.Windows.Forms.Label();
			this.labelLauncherWorkDirPath = new System.Windows.Forms.Label();
			this.labelLauncherName = new System.Windows.Forms.Label();
			this.labelLauncherCommand = new System.Windows.Forms.Label();
			this.inputLauncherNote = new System.Windows.Forms.TextBox();
			this.inputLauncherTag = new System.Windows.Forms.TextBox();
			this.inputLauncherIconPath = new System.Windows.Forms.TextBox();
			this.inputLauncherWorkDirPath = new System.Windows.Forms.TextBox();
			this.inputLauncherName = new System.Windows.Forms.TextBox();
			this.inputLauncherCommand = new System.Windows.Forms.TextBox();
			this.commandLauncherIconPath = new System.Windows.Forms.Button();
			this.commandLauncherWorkDirPath = new System.Windows.Forms.Button();
			this.commandLauncherFilePath = new System.Windows.Forms.Button();
			this.labelLauncherItemState = new System.Windows.Forms.Label();
			this.groupLauncherType = new System.Windows.Forms.GroupBox();
			this.selectLauncherType_uri = new System.Windows.Forms.RadioButton();
			this.selectLauncherType_file = new System.Windows.Forms.RadioButton();
			this.pageCommand = new System.Windows.Forms.TabPage();
			this.inputCommandHideTime = new System.Windows.Forms.NumericUpDown();
			this.selectCommandTopmost = new System.Windows.Forms.CheckBox();
			this.titleCommandHideTime = new System.Windows.Forms.Label();
			this.commandCommandFont = new System.Windows.Forms.Button();
			this.titleCommandFont = new System.Windows.Forms.Label();
			this.pageToolbar = new System.Windows.Forms.TabPage();
			this.selecterToolbar = new PeMain.UI.LauncherItemSelectControl();
			this.treeToolbarItemGroup = new System.Windows.Forms.TreeView();
			this.labelToolbarIcon = new System.Windows.Forms.Label();
			this.selectToolbarIcon = new System.Windows.Forms.ComboBox();
			this.labelToolbarPosition = new System.Windows.Forms.Label();
			this.selectToolbarPosition = new System.Windows.Forms.ComboBox();
			this.selectToolbarShow = new System.Windows.Forms.CheckBox();
			this.selectToolbarTopmost = new System.Windows.Forms.CheckBox();
			this.commandToolbarFont = new System.Windows.Forms.Button();
			this.labelToolbarFonr = new System.Windows.Forms.Label();
			this.pageNote = new System.Windows.Forms.TabPage();
			this.pageDisplay = new System.Windows.Forms.TabPage();
			this.commandCancel = new System.Windows.Forms.Button();
			this.commandSubmit = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.commandLauncherDirPath = new System.Windows.Forms.Button();
			this.tabSetting.SuspendLayout();
			this.pageLauncher.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputLauncherIconIndex)).BeginInit();
			this.groupLauncherType.SuspendLayout();
			this.pageCommand.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).BeginInit();
			this.pageToolbar.SuspendLayout();
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
			this.tabSetting.Size = new System.Drawing.Size(578, 338);
			this.tabSetting.TabIndex = 0;
			// 
			// pageMain
			// 
			this.pageMain.Location = new System.Drawing.Point(4, 24);
			this.pageMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Name = "pageMain";
			this.pageMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Size = new System.Drawing.Size(570, 310);
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
			this.pageLauncher.Size = new System.Drawing.Size(570, 310);
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
			this.splitContainer1.Panel1.Controls.Add(this.selecterLauncher);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherNote);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherIconIndex);
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherTag);
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherIconPath);
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherWorkDirPath);
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherName);
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherCommand);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherNote);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherTag);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherIconPath);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherWorkDirPath);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherName);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherCommand);
			this.splitContainer1.Panel2.Controls.Add(this.commandLauncherIconPath);
			this.splitContainer1.Panel2.Controls.Add(this.commandLauncherWorkDirPath);
			this.splitContainer1.Panel2.Controls.Add(this.commandLauncherDirPath);
			this.splitContainer1.Panel2.Controls.Add(this.commandLauncherFilePath);
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherItemState);
			this.splitContainer1.Panel2.Controls.Add(this.groupLauncherType);
			this.splitContainer1.Size = new System.Drawing.Size(564, 302);
			this.splitContainer1.SplitterDistance = 217;
			this.splitContainer1.TabIndex = 0;
			// 
			// selecterLauncher
			// 
			this.selecterLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selecterLauncher.FilterType = PeMain.UI.LauncherItemSelecterType.Full;
			this.selecterLauncher.IconSize = PeUtility.IconSize.Small;
			this.selecterLauncher.ItemEdit = true;
			this.selecterLauncher.Location = new System.Drawing.Point(0, 0);
			this.selecterLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterLauncher.Name = "selecterLauncher";
			this.selecterLauncher.Size = new System.Drawing.Size(217, 302);
			this.selecterLauncher.TabIndex = 0;
			this.selecterLauncher.SelectChnagedItem += new System.EventHandler<PeMain.UI.SelectedItemEventArg>(this.SelecterLauncher_SelectChnagedItem);
			// 
			// labelLauncherNote
			// 
			this.labelLauncherNote.Location = new System.Drawing.Point(13, 223);
			this.labelLauncherNote.Name = "labelLauncherNote";
			this.labelLauncherNote.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherNote.TabIndex = 6;
			this.labelLauncherNote.Text = "{NOTE}";
			// 
			// inputLauncherIconIndex
			// 
			this.inputLauncherIconIndex.Location = new System.Drawing.Point(225, 155);
			this.inputLauncherIconIndex.Name = "inputLauncherIconIndex";
			this.inputLauncherIconIndex.Size = new System.Drawing.Size(33, 23);
			this.inputLauncherIconIndex.TabIndex = 5;
			// 
			// labelLauncherTag
			// 
			this.labelLauncherTag.Location = new System.Drawing.Point(13, 189);
			this.labelLauncherTag.Name = "labelLauncherTag";
			this.labelLauncherTag.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherTag.TabIndex = 4;
			this.labelLauncherTag.Text = "{TAG}";
			// 
			// labelLauncherIconPath
			// 
			this.labelLauncherIconPath.Location = new System.Drawing.Point(13, 152);
			this.labelLauncherIconPath.Name = "labelLauncherIconPath";
			this.labelLauncherIconPath.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherIconPath.TabIndex = 4;
			this.labelLauncherIconPath.Text = "{ICON}";
			// 
			// labelLauncherWorkDirPath
			// 
			this.labelLauncherWorkDirPath.Location = new System.Drawing.Point(13, 123);
			this.labelLauncherWorkDirPath.Name = "labelLauncherWorkDirPath";
			this.labelLauncherWorkDirPath.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherWorkDirPath.TabIndex = 4;
			this.labelLauncherWorkDirPath.Text = "{WORK_DIR}";
			// 
			// labelLauncherName
			// 
			this.labelLauncherName.Location = new System.Drawing.Point(13, 67);
			this.labelLauncherName.Name = "labelLauncherName";
			this.labelLauncherName.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherName.TabIndex = 4;
			this.labelLauncherName.Text = "{NAME}";
			// 
			// labelLauncherCommand
			// 
			this.labelLauncherCommand.Location = new System.Drawing.Point(13, 94);
			this.labelLauncherCommand.Name = "labelLauncherCommand";
			this.labelLauncherCommand.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherCommand.TabIndex = 4;
			this.labelLauncherCommand.Text = "{COMMAND}";
			// 
			// inputLauncherNote
			// 
			this.inputLauncherNote.Location = new System.Drawing.Point(119, 218);
			this.inputLauncherNote.Multiline = true;
			this.inputLauncherNote.Name = "inputLauncherNote";
			this.inputLauncherNote.Size = new System.Drawing.Size(216, 75);
			this.inputLauncherNote.TabIndex = 3;
			// 
			// inputLauncherTag
			// 
			this.inputLauncherTag.Location = new System.Drawing.Point(119, 189);
			this.inputLauncherTag.Name = "inputLauncherTag";
			this.inputLauncherTag.Size = new System.Drawing.Size(216, 23);
			this.inputLauncherTag.TabIndex = 3;
			// 
			// inputLauncherIconPath
			// 
			this.inputLauncherIconPath.Location = new System.Drawing.Point(119, 155);
			this.inputLauncherIconPath.Name = "inputLauncherIconPath";
			this.inputLauncherIconPath.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherIconPath.TabIndex = 3;
			// 
			// inputLauncherWorkDirPath
			// 
			this.inputLauncherWorkDirPath.Location = new System.Drawing.Point(119, 124);
			this.inputLauncherWorkDirPath.Name = "inputLauncherWorkDirPath";
			this.inputLauncherWorkDirPath.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherWorkDirPath.TabIndex = 3;
			// 
			// inputLauncherName
			// 
			this.inputLauncherName.Location = new System.Drawing.Point(119, 68);
			this.inputLauncherName.Name = "inputLauncherName";
			this.inputLauncherName.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherName.TabIndex = 3;
			// 
			// inputLauncherCommand
			// 
			this.inputLauncherCommand.Location = new System.Drawing.Point(119, 95);
			this.inputLauncherCommand.Name = "inputLauncherCommand";
			this.inputLauncherCommand.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherCommand.TabIndex = 3;
			// 
			// commandLauncherIconPath
			// 
			this.commandLauncherIconPath.Location = new System.Drawing.Point(260, 153);
			this.commandLauncherIconPath.Name = "commandLauncherIconPath";
			this.commandLauncherIconPath.Size = new System.Drawing.Size(75, 23);
			this.commandLauncherIconPath.TabIndex = 2;
			this.commandLauncherIconPath.Text = "button1";
			this.commandLauncherIconPath.UseVisualStyleBackColor = true;
			// 
			// commandLauncherWorkDirPath
			// 
			this.commandLauncherWorkDirPath.Location = new System.Drawing.Point(225, 123);
			this.commandLauncherWorkDirPath.Name = "commandLauncherWorkDirPath";
			this.commandLauncherWorkDirPath.Size = new System.Drawing.Size(75, 23);
			this.commandLauncherWorkDirPath.TabIndex = 2;
			this.commandLauncherWorkDirPath.Text = "button1";
			this.commandLauncherWorkDirPath.UseVisualStyleBackColor = true;
			// 
			// commandLauncherFilePath
			// 
			this.commandLauncherFilePath.Location = new System.Drawing.Point(225, 94);
			this.commandLauncherFilePath.Name = "commandLauncherFilePath";
			this.commandLauncherFilePath.Size = new System.Drawing.Size(75, 23);
			this.commandLauncherFilePath.TabIndex = 2;
			this.commandLauncherFilePath.Text = "button1";
			this.commandLauncherFilePath.UseVisualStyleBackColor = true;
			// 
			// labelLauncherItemState
			// 
			this.labelLauncherItemState.Location = new System.Drawing.Point(6, 31);
			this.labelLauncherItemState.Name = "labelLauncherItemState";
			this.labelLauncherItemState.Size = new System.Drawing.Size(107, 23);
			this.labelLauncherItemState.TabIndex = 1;
			this.labelLauncherItemState.Text = "{EDIT}/{NEW}";
			// 
			// groupLauncherType
			// 
			this.groupLauncherType.Controls.Add(this.selectLauncherType_uri);
			this.groupLauncherType.Controls.Add(this.selectLauncherType_file);
			this.groupLauncherType.Location = new System.Drawing.Point(119, 3);
			this.groupLauncherType.Name = "groupLauncherType";
			this.groupLauncherType.Size = new System.Drawing.Size(221, 59);
			this.groupLauncherType.TabIndex = 0;
			this.groupLauncherType.TabStop = false;
			this.groupLauncherType.Text = "{ITEM_TYPE}";
			// 
			// selectLauncherType_uri
			// 
			this.selectLauncherType_uri.Location = new System.Drawing.Point(117, 23);
			this.selectLauncherType_uri.Name = "selectLauncherType_uri";
			this.selectLauncherType_uri.Size = new System.Drawing.Size(104, 24);
			this.selectLauncherType_uri.TabIndex = 1;
			this.selectLauncherType_uri.TabStop = true;
			this.selectLauncherType_uri.Text = "{ITEM_URI}";
			this.selectLauncherType_uri.UseVisualStyleBackColor = true;
			// 
			// selectLauncherType_file
			// 
			this.selectLauncherType_file.Location = new System.Drawing.Point(7, 23);
			this.selectLauncherType_file.Name = "selectLauncherType_file";
			this.selectLauncherType_file.Size = new System.Drawing.Size(104, 24);
			this.selectLauncherType_file.TabIndex = 0;
			this.selectLauncherType_file.TabStop = true;
			this.selectLauncherType_file.Text = "{ITEM_FILE}";
			this.selectLauncherType_file.UseVisualStyleBackColor = true;
			// 
			// pageCommand
			// 
			this.pageCommand.Controls.Add(this.inputCommandHideTime);
			this.pageCommand.Controls.Add(this.selectCommandTopmost);
			this.pageCommand.Controls.Add(this.titleCommandHideTime);
			this.pageCommand.Controls.Add(this.commandCommandFont);
			this.pageCommand.Controls.Add(this.titleCommandFont);
			this.pageCommand.Location = new System.Drawing.Point(4, 24);
			this.pageCommand.Name = "pageCommand";
			this.pageCommand.Padding = new System.Windows.Forms.Padding(3);
			this.pageCommand.Size = new System.Drawing.Size(570, 310);
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
			// selectCommandTopmost
			// 
			this.selectCommandTopmost.Location = new System.Drawing.Point(35, 125);
			this.selectCommandTopmost.Name = "selectCommandTopmost";
			this.selectCommandTopmost.Size = new System.Drawing.Size(104, 24);
			this.selectCommandTopmost.TabIndex = 4;
			this.selectCommandTopmost.Text = "{TOPMOST}";
			this.selectCommandTopmost.UseVisualStyleBackColor = true;
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
			this.pageToolbar.Controls.Add(this.selecterToolbar);
			this.pageToolbar.Controls.Add(this.treeToolbarItemGroup);
			this.pageToolbar.Controls.Add(this.labelToolbarIcon);
			this.pageToolbar.Controls.Add(this.selectToolbarIcon);
			this.pageToolbar.Controls.Add(this.labelToolbarPosition);
			this.pageToolbar.Controls.Add(this.selectToolbarPosition);
			this.pageToolbar.Controls.Add(this.selectToolbarShow);
			this.pageToolbar.Controls.Add(this.selectToolbarTopmost);
			this.pageToolbar.Controls.Add(this.commandToolbarFont);
			this.pageToolbar.Controls.Add(this.labelToolbarFonr);
			this.pageToolbar.Location = new System.Drawing.Point(4, 24);
			this.pageToolbar.Name = "pageToolbar";
			this.pageToolbar.Size = new System.Drawing.Size(570, 310);
			this.pageToolbar.TabIndex = 3;
			this.pageToolbar.Text = "{TOOLBAR}";
			this.pageToolbar.UseVisualStyleBackColor = true;
			// 
			// selecterToolbar
			// 
			this.selecterToolbar.FilterType = PeMain.UI.LauncherItemSelecterType.Full;
			this.selecterToolbar.ItemEdit = false;
			this.selecterToolbar.Location = new System.Drawing.Point(415, 48);
			this.selecterToolbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterToolbar.Name = "selecterToolbar";
			this.selecterToolbar.Size = new System.Drawing.Size(150, 150);
			this.selecterToolbar.TabIndex = 14;
			// 
			// treeToolbarItemGroup
			// 
			this.treeToolbarItemGroup.Location = new System.Drawing.Point(288, 48);
			this.treeToolbarItemGroup.Name = "treeToolbarItemGroup";
			this.treeToolbarItemGroup.Size = new System.Drawing.Size(121, 150);
			this.treeToolbarItemGroup.TabIndex = 13;
			// 
			// labelToolbarIcon
			// 
			this.labelToolbarIcon.Location = new System.Drawing.Point(23, 128);
			this.labelToolbarIcon.Name = "labelToolbarIcon";
			this.labelToolbarIcon.Size = new System.Drawing.Size(100, 23);
			this.labelToolbarIcon.TabIndex = 12;
			this.labelToolbarIcon.Text = "{ICON}";
			// 
			// selectToolbarIcon
			// 
			this.selectToolbarIcon.FormattingEnabled = true;
			this.selectToolbarIcon.Location = new System.Drawing.Point(129, 131);
			this.selectToolbarIcon.Name = "selectToolbarIcon";
			this.selectToolbarIcon.Size = new System.Drawing.Size(121, 23);
			this.selectToolbarIcon.TabIndex = 11;
			// 
			// labelToolbarPosition
			// 
			this.labelToolbarPosition.Location = new System.Drawing.Point(23, 105);
			this.labelToolbarPosition.Name = "labelToolbarPosition";
			this.labelToolbarPosition.Size = new System.Drawing.Size(100, 23);
			this.labelToolbarPosition.TabIndex = 10;
			this.labelToolbarPosition.Text = "{POSITION}";
			// 
			// selectToolbarPosition
			// 
			this.selectToolbarPosition.FormattingEnabled = true;
			this.selectToolbarPosition.Location = new System.Drawing.Point(129, 102);
			this.selectToolbarPosition.Name = "selectToolbarPosition";
			this.selectToolbarPosition.Size = new System.Drawing.Size(121, 23);
			this.selectToolbarPosition.TabIndex = 9;
			// 
			// selectToolbarShow
			// 
			this.selectToolbarShow.Location = new System.Drawing.Point(23, 78);
			this.selectToolbarShow.Name = "selectToolbarShow";
			this.selectToolbarShow.Size = new System.Drawing.Size(104, 24);
			this.selectToolbarShow.TabIndex = 8;
			this.selectToolbarShow.Text = "{SHOW}";
			this.selectToolbarShow.UseVisualStyleBackColor = true;
			// 
			// selectToolbarTopmost
			// 
			this.selectToolbarTopmost.Location = new System.Drawing.Point(23, 48);
			this.selectToolbarTopmost.Name = "selectToolbarTopmost";
			this.selectToolbarTopmost.Size = new System.Drawing.Size(104, 24);
			this.selectToolbarTopmost.TabIndex = 7;
			this.selectToolbarTopmost.Text = "{TOPMOST}";
			this.selectToolbarTopmost.UseVisualStyleBackColor = true;
			// 
			// commandToolbarFont
			// 
			this.commandToolbarFont.Location = new System.Drawing.Point(133, 18);
			this.commandToolbarFont.Name = "commandToolbarFont";
			this.commandToolbarFont.Size = new System.Drawing.Size(171, 23);
			this.commandToolbarFont.TabIndex = 6;
			this.commandToolbarFont.Text = "{FAMILY} {PT} ...";
			this.commandToolbarFont.UseVisualStyleBackColor = true;
			// 
			// labelToolbarFonr
			// 
			this.labelToolbarFonr.Location = new System.Drawing.Point(27, 22);
			this.labelToolbarFonr.Name = "labelToolbarFonr";
			this.labelToolbarFonr.Size = new System.Drawing.Size(100, 23);
			this.labelToolbarFonr.TabIndex = 5;
			this.labelToolbarFonr.Text = "{FONT}";
			// 
			// pageNote
			// 
			this.pageNote.Location = new System.Drawing.Point(4, 24);
			this.pageNote.Name = "pageNote";
			this.pageNote.Size = new System.Drawing.Size(570, 310);
			this.pageNote.TabIndex = 6;
			this.pageNote.Text = "{NOTE}";
			this.pageNote.UseVisualStyleBackColor = true;
			// 
			// pageDisplay
			// 
			this.pageDisplay.Location = new System.Drawing.Point(4, 24);
			this.pageDisplay.Name = "pageDisplay";
			this.pageDisplay.Size = new System.Drawing.Size(570, 310);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 389);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.commandSubmit);
			this.flowLayoutPanel1.Controls.Add(this.commandCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(395, 349);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 37);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// commandLauncherDirPath
			// 
			this.commandLauncherDirPath.Location = new System.Drawing.Point(306, 94);
			this.commandLauncherDirPath.Name = "commandLauncherDirPath";
			this.commandLauncherDirPath.Size = new System.Drawing.Size(75, 23);
			this.commandLauncherDirPath.TabIndex = 2;
			this.commandLauncherDirPath.Text = "button1";
			this.commandLauncherDirPath.UseVisualStyleBackColor = true;
			// 
			// SettingForm
			// 
			this.AcceptButton = this.commandSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(584, 389);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "SettingForm";
			this.Text = "{SETTING}";
			this.tabSetting.ResumeLayout(false);
			this.pageLauncher.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.inputLauncherIconIndex)).EndInit();
			this.groupLauncherType.ResumeLayout(false);
			this.pageCommand.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).EndInit();
			this.pageToolbar.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button commandLauncherDirPath;
		private System.Windows.Forms.TextBox inputLauncherTag;
		private System.Windows.Forms.TextBox inputLauncherNote;
		private System.Windows.Forms.Label labelLauncherTag;
		private System.Windows.Forms.Label labelLauncherNote;
		private System.Windows.Forms.Label labelLauncherItemState;
		private System.Windows.Forms.Button commandLauncherFilePath;
		private System.Windows.Forms.Button commandLauncherWorkDirPath;
		private System.Windows.Forms.Button commandLauncherIconPath;
		private System.Windows.Forms.TextBox inputLauncherCommand;
		private System.Windows.Forms.TextBox inputLauncherName;
		private System.Windows.Forms.TextBox inputLauncherWorkDirPath;
		private System.Windows.Forms.TextBox inputLauncherIconPath;
		private System.Windows.Forms.Label labelLauncherCommand;
		private System.Windows.Forms.Label labelLauncherName;
		private System.Windows.Forms.Label labelLauncherWorkDirPath;
		private System.Windows.Forms.Label labelLauncherIconPath;
		private System.Windows.Forms.NumericUpDown inputLauncherIconIndex;
		private System.Windows.Forms.RadioButton selectLauncherType_file;
		private System.Windows.Forms.RadioButton selectLauncherType_uri;
		private System.Windows.Forms.GroupBox groupLauncherType;
		private PeMain.UI.LauncherItemSelectControl selecterToolbar;
		private PeMain.UI.LauncherItemSelectControl selecterLauncher;
		private System.Windows.Forms.TreeView treeToolbarItemGroup;
		private System.Windows.Forms.ComboBox selectToolbarPosition;
		private System.Windows.Forms.Label labelToolbarPosition;
		private System.Windows.Forms.ComboBox selectToolbarIcon;
		private System.Windows.Forms.Label labelToolbarIcon;
		private System.Windows.Forms.CheckBox selectToolbarShow;
		private System.Windows.Forms.Label labelToolbarFonr;
		private System.Windows.Forms.Button commandToolbarFont;
		private System.Windows.Forms.CheckBox selectToolbarTopmost;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabPage pageNote;
		private System.Windows.Forms.TabPage pageDisplay;
		private System.Windows.Forms.TabPage pageToolbar;
		private System.Windows.Forms.NumericUpDown inputCommandHideTime;
		private System.Windows.Forms.Label titleCommandHideTime;
		private System.Windows.Forms.CheckBox selectCommandTopmost;
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
