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
			this.groupMainLog = new System.Windows.Forms.GroupBox();
			this.selectLogAddShow = new System.Windows.Forms.CheckBox();
			this.selectLogVisible = new System.Windows.Forms.CheckBox();
			this.selectMainLanguage = new System.Windows.Forms.ComboBox();
			this.labelMainLanguage = new System.Windows.Forms.Label();
			this.pageLauncher = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.selecterLauncher = new PeMain.UI.LauncherItemSelectControl();
			this.commandLauncherCommandSetter = new System.Windows.Forms.Button();
			this.selectLauncherStdStream = new System.Windows.Forms.CheckBox();
			this.labelLauncherOption = new System.Windows.Forms.Label();
			this.inputLauncherOption = new System.Windows.Forms.TextBox();
			this.commandLauncherOptionDirPath = new System.Windows.Forms.Button();
			this.commandLauncherOptionFilePath = new System.Windows.Forms.Button();
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
			this.commandLauncherDirPath = new System.Windows.Forms.Button();
			this.commandLauncherFilePath = new System.Windows.Forms.Button();
			this.groupLauncherType = new System.Windows.Forms.GroupBox();
			this.selectLauncherType_uri = new System.Windows.Forms.RadioButton();
			this.selectLauncherType_file = new System.Windows.Forms.RadioButton();
			this.pageCommand = new System.Windows.Forms.TabPage();
			this.labelCommandHotkey = new System.Windows.Forms.Label();
			this.inputCommandHotkey = new PeMain.UI.PeHotkeyControl();
			this.labelCommandIcon = new System.Windows.Forms.Label();
			this.selectCommandIcon = new System.Windows.Forms.ComboBox();
			this.inputCommandHideTime = new System.Windows.Forms.NumericUpDown();
			this.selectCommandTopmost = new System.Windows.Forms.CheckBox();
			this.titleCommandHideTime = new System.Windows.Forms.Label();
			this.commandCommandFont = new System.Windows.Forms.Button();
			this.titleCommandFont = new System.Windows.Forms.Label();
			this.pageToolbar = new System.Windows.Forms.TabPage();
			this.selectToolbarShowText = new System.Windows.Forms.CheckBox();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.treeToolbarItemGroup = new System.Windows.Forms.TreeView();
			this.toolToolbarGroup = new System.Windows.Forms.ToolStrip();
			this.toolToolbarGroup_addGroup = new System.Windows.Forms.ToolStripButton();
			this.toolToolbarGroup_addItem = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolToolbarGroup_up = new System.Windows.Forms.ToolStripButton();
			this.toolToolbarGroup_down = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolToolbarGroup_remove = new System.Windows.Forms.ToolStripButton();
			this.selecterToolbar = new PeMain.UI.LauncherItemSelectControl();
			this.labelToolbarIcon = new System.Windows.Forms.Label();
			this.selectToolbarIcon = new System.Windows.Forms.ComboBox();
			this.labelToolbarPosition = new System.Windows.Forms.Label();
			this.selectToolbarPosition = new System.Windows.Forms.ComboBox();
			this.selectToolbarVisible = new System.Windows.Forms.CheckBox();
			this.selectToolbarAutoHide = new System.Windows.Forms.CheckBox();
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
			this.tabSetting.SuspendLayout();
			this.pageMain.SuspendLayout();
			this.groupMainLog.SuspendLayout();
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
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.toolToolbarGroup.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// tabSetting
			// 
			this.tabSetting.Controls.Add(this.pageMain);
			this.tabSetting.Controls.Add(this.pageLauncher);
			this.tabSetting.Controls.Add(this.pageToolbar);
			this.tabSetting.Controls.Add(this.pageCommand);
			this.tabSetting.Controls.Add(this.pageNote);
			this.tabSetting.Controls.Add(this.pageDisplay);
			this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSetting.Location = new System.Drawing.Point(3, 4);
			this.tabSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting.Name = "tabSetting";
			this.tabSetting.SelectedIndex = 0;
			this.tabSetting.Size = new System.Drawing.Size(742, 338);
			this.tabSetting.TabIndex = 0;
			this.tabSetting.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabSetting_Selecting);
			// 
			// pageMain
			// 
			this.pageMain.Controls.Add(this.groupMainLog);
			this.pageMain.Controls.Add(this.selectMainLanguage);
			this.pageMain.Controls.Add(this.labelMainLanguage);
			this.pageMain.Location = new System.Drawing.Point(4, 24);
			this.pageMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Name = "pageMain";
			this.pageMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Size = new System.Drawing.Size(734, 310);
			this.pageMain.TabIndex = 0;
			this.pageMain.Text = "{Pe}";
			this.pageMain.UseVisualStyleBackColor = true;
			// 
			// groupMainLog
			// 
			this.groupMainLog.Controls.Add(this.selectLogAddShow);
			this.groupMainLog.Controls.Add(this.selectLogVisible);
			this.groupMainLog.Location = new System.Drawing.Point(66, 90);
			this.groupMainLog.Name = "groupMainLog";
			this.groupMainLog.Size = new System.Drawing.Size(200, 100);
			this.groupMainLog.TabIndex = 2;
			this.groupMainLog.TabStop = false;
			this.groupMainLog.Text = "{LOG}";
			// 
			// selectLogAddShow
			// 
			this.selectLogAddShow.Location = new System.Drawing.Point(55, 52);
			this.selectLogAddShow.Name = "selectLogAddShow";
			this.selectLogAddShow.Size = new System.Drawing.Size(104, 24);
			this.selectLogAddShow.TabIndex = 1;
			this.selectLogAddShow.Text = "{ADD_SHOW}";
			this.selectLogAddShow.UseVisualStyleBackColor = true;
			// 
			// selectLogVisible
			// 
			this.selectLogVisible.Location = new System.Drawing.Point(16, 22);
			this.selectLogVisible.Name = "selectLogVisible";
			this.selectLogVisible.Size = new System.Drawing.Size(104, 24);
			this.selectLogVisible.TabIndex = 0;
			this.selectLogVisible.Text = "{VISIBLE}";
			this.selectLogVisible.UseVisualStyleBackColor = true;
			// 
			// selectMainLanguage
			// 
			this.selectMainLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectMainLanguage.FormattingEnabled = true;
			this.selectMainLanguage.Location = new System.Drawing.Point(121, 19);
			this.selectMainLanguage.Name = "selectMainLanguage";
			this.selectMainLanguage.Size = new System.Drawing.Size(121, 23);
			this.selectMainLanguage.TabIndex = 1;
			// 
			// labelMainLanguage
			// 
			this.labelMainLanguage.Location = new System.Drawing.Point(15, 19);
			this.labelMainLanguage.Name = "labelMainLanguage";
			this.labelMainLanguage.Size = new System.Drawing.Size(100, 23);
			this.labelMainLanguage.TabIndex = 0;
			this.labelMainLanguage.Text = "{LANGUAGE}";
			// 
			// pageLauncher
			// 
			this.pageLauncher.AllowDrop = true;
			this.pageLauncher.Controls.Add(this.splitContainer1);
			this.pageLauncher.Location = new System.Drawing.Point(4, 24);
			this.pageLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageLauncher.Name = "pageLauncher";
			this.pageLauncher.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageLauncher.Size = new System.Drawing.Size(734, 310);
			this.pageLauncher.TabIndex = 1;
			this.pageLauncher.Text = "{LAUNCHER}";
			this.pageLauncher.UseVisualStyleBackColor = true;
			this.pageLauncher.DragDrop += new System.Windows.Forms.DragEventHandler(this.PageLauncher_DragDrop);
			this.pageLauncher.DragEnter += new System.Windows.Forms.DragEventHandler(this.PageLauncher_DragEnter);
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
			this.splitContainer1.Panel2.Controls.Add(this.commandLauncherCommandSetter);
			this.splitContainer1.Panel2.Controls.Add(this.selectLauncherStdStream);
			this.splitContainer1.Panel2.Controls.Add(this.labelLauncherOption);
			this.splitContainer1.Panel2.Controls.Add(this.inputLauncherOption);
			this.splitContainer1.Panel2.Controls.Add(this.commandLauncherOptionDirPath);
			this.splitContainer1.Panel2.Controls.Add(this.commandLauncherOptionFilePath);
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
			this.splitContainer1.Panel2.Controls.Add(this.groupLauncherType);
			this.splitContainer1.Size = new System.Drawing.Size(728, 302);
			this.splitContainer1.SplitterDistance = 191;
			this.splitContainer1.TabIndex = 0;
			// 
			// selecterLauncher
			// 
			this.selecterLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selecterLauncher.Filtering = false;
			this.selecterLauncher.FilterType = PeMain.UI.LauncherItemSelecterType.Full;
			this.selecterLauncher.IconSize = PeUtility.IconSize.Small;
			this.selecterLauncher.ItemEdit = true;
			this.selecterLauncher.Location = new System.Drawing.Point(0, 0);
			this.selecterLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterLauncher.Name = "selecterLauncher";
			this.selecterLauncher.SelectedItem = null;
			this.selecterLauncher.Size = new System.Drawing.Size(191, 302);
			this.selecterLauncher.TabIndex = 0;
			this.selecterLauncher.CreateItem += new System.EventHandler<PeMain.UI.CreateItemEventArg>(this.SelecterLauncher_CreateItem);
			this.selecterLauncher.SelectChangedItem += new System.EventHandler<PeMain.UI.SelectedItemEventArg>(this.SelecterLauncher_SelectChnagedItem);
			// 
			// commandLauncherCommandSetter
			// 
			this.commandLauncherCommandSetter.Location = new System.Drawing.Point(225, 68);
			this.commandLauncherCommandSetter.Name = "commandLauncherCommandSetter";
			this.commandLauncherCommandSetter.Size = new System.Drawing.Size(75, 23);
			this.commandLauncherCommandSetter.TabIndex = 13;
			this.commandLauncherCommandSetter.Text = "{COMMAND}";
			this.commandLauncherCommandSetter.UseVisualStyleBackColor = true;
			// 
			// selectLauncherStdStream
			// 
			this.selectLauncherStdStream.Location = new System.Drawing.Point(332, 93);
			this.selectLauncherStdStream.Name = "selectLauncherStdStream";
			this.selectLauncherStdStream.Size = new System.Drawing.Size(124, 24);
			this.selectLauncherStdStream.TabIndex = 12;
			this.selectLauncherStdStream.Text = "{STD_STREAM}";
			this.selectLauncherStdStream.UseVisualStyleBackColor = true;
			this.selectLauncherStdStream.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// labelLauncherOption
			// 
			this.labelLauncherOption.Location = new System.Drawing.Point(13, 121);
			this.labelLauncherOption.Name = "labelLauncherOption";
			this.labelLauncherOption.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherOption.TabIndex = 10;
			this.labelLauncherOption.Text = "{OPTION}";
			// 
			// inputLauncherOption
			// 
			this.inputLauncherOption.Location = new System.Drawing.Point(119, 122);
			this.inputLauncherOption.Name = "inputLauncherOption";
			this.inputLauncherOption.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherOption.TabIndex = 9;
			this.inputLauncherOption.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// commandLauncherOptionDirPath
			// 
			this.commandLauncherOptionDirPath.Image = global::PeMain.Properties.Images.Dir;
			this.commandLauncherOptionDirPath.Location = new System.Drawing.Point(265, 121);
			this.commandLauncherOptionDirPath.Name = "commandLauncherOptionDirPath";
			this.commandLauncherOptionDirPath.Size = new System.Drawing.Size(34, 25);
			this.commandLauncherOptionDirPath.TabIndex = 7;
			this.commandLauncherOptionDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherOptionDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherOptionDirPath.Click += new System.EventHandler(this.CommandLauncherOptionDirPath_Click);
			// 
			// commandLauncherOptionFilePath
			// 
			this.commandLauncherOptionFilePath.Image = global::PeMain.Properties.Images.File;
			this.commandLauncherOptionFilePath.Location = new System.Drawing.Point(225, 121);
			this.commandLauncherOptionFilePath.Name = "commandLauncherOptionFilePath";
			this.commandLauncherOptionFilePath.Size = new System.Drawing.Size(34, 25);
			this.commandLauncherOptionFilePath.TabIndex = 8;
			this.commandLauncherOptionFilePath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherOptionFilePath.UseVisualStyleBackColor = true;
			this.commandLauncherOptionFilePath.Click += new System.EventHandler(this.CommandLauncherOptionFilePath_Click);
			// 
			// labelLauncherNote
			// 
			this.labelLauncherNote.Location = new System.Drawing.Point(13, 246);
			this.labelLauncherNote.Name = "labelLauncherNote";
			this.labelLauncherNote.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherNote.TabIndex = 6;
			this.labelLauncherNote.Text = "{NOTE}";
			// 
			// inputLauncherIconIndex
			// 
			this.inputLauncherIconIndex.Location = new System.Drawing.Point(225, 184);
			this.inputLauncherIconIndex.Name = "inputLauncherIconIndex";
			this.inputLauncherIconIndex.Size = new System.Drawing.Size(33, 23);
			this.inputLauncherIconIndex.TabIndex = 5;
			this.inputLauncherIconIndex.ValueChanged += new System.EventHandler(this.InputLauncherIconIndex_ValueChanged);
			// 
			// labelLauncherTag
			// 
			this.labelLauncherTag.Location = new System.Drawing.Point(13, 217);
			this.labelLauncherTag.Name = "labelLauncherTag";
			this.labelLauncherTag.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherTag.TabIndex = 4;
			this.labelLauncherTag.Text = "{TAG}";
			// 
			// labelLauncherIconPath
			// 
			this.labelLauncherIconPath.Location = new System.Drawing.Point(13, 181);
			this.labelLauncherIconPath.Name = "labelLauncherIconPath";
			this.labelLauncherIconPath.Size = new System.Drawing.Size(100, 23);
			this.labelLauncherIconPath.TabIndex = 4;
			this.labelLauncherIconPath.Text = "{ICON}";
			// 
			// labelLauncherWorkDirPath
			// 
			this.labelLauncherWorkDirPath.Location = new System.Drawing.Point(13, 152);
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
			this.inputLauncherNote.AcceptsReturn = true;
			this.inputLauncherNote.Location = new System.Drawing.Point(119, 246);
			this.inputLauncherNote.Multiline = true;
			this.inputLauncherNote.Name = "inputLauncherNote";
			this.inputLauncherNote.Size = new System.Drawing.Size(216, 47);
			this.inputLauncherNote.TabIndex = 3;
			this.inputLauncherNote.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherTag
			// 
			this.inputLauncherTag.Location = new System.Drawing.Point(119, 217);
			this.inputLauncherTag.Name = "inputLauncherTag";
			this.inputLauncherTag.Size = new System.Drawing.Size(216, 23);
			this.inputLauncherTag.TabIndex = 3;
			this.inputLauncherTag.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherIconPath
			// 
			this.inputLauncherIconPath.Location = new System.Drawing.Point(119, 184);
			this.inputLauncherIconPath.Name = "inputLauncherIconPath";
			this.inputLauncherIconPath.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherIconPath.TabIndex = 3;
			this.inputLauncherIconPath.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherWorkDirPath
			// 
			this.inputLauncherWorkDirPath.Location = new System.Drawing.Point(119, 153);
			this.inputLauncherWorkDirPath.Name = "inputLauncherWorkDirPath";
			this.inputLauncherWorkDirPath.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherWorkDirPath.TabIndex = 3;
			this.inputLauncherWorkDirPath.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherName
			// 
			this.inputLauncherName.Location = new System.Drawing.Point(119, 68);
			this.inputLauncherName.Name = "inputLauncherName";
			this.inputLauncherName.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherName.TabIndex = 3;
			this.inputLauncherName.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherCommand
			// 
			this.inputLauncherCommand.Location = new System.Drawing.Point(119, 95);
			this.inputLauncherCommand.Name = "inputLauncherCommand";
			this.inputLauncherCommand.Size = new System.Drawing.Size(100, 23);
			this.inputLauncherCommand.TabIndex = 3;
			this.inputLauncherCommand.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// commandLauncherIconPath
			// 
			this.commandLauncherIconPath.Image = global::PeMain.Properties.Images.File;
			this.commandLauncherIconPath.Location = new System.Drawing.Point(260, 182);
			this.commandLauncherIconPath.Name = "commandLauncherIconPath";
			this.commandLauncherIconPath.Size = new System.Drawing.Size(34, 25);
			this.commandLauncherIconPath.TabIndex = 2;
			this.commandLauncherIconPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherIconPath.UseVisualStyleBackColor = true;
			this.commandLauncherIconPath.Click += new System.EventHandler(this.CommandLauncherIconPath_Click);
			// 
			// commandLauncherWorkDirPath
			// 
			this.commandLauncherWorkDirPath.Image = global::PeMain.Properties.Images.Dir;
			this.commandLauncherWorkDirPath.Location = new System.Drawing.Point(225, 152);
			this.commandLauncherWorkDirPath.Name = "commandLauncherWorkDirPath";
			this.commandLauncherWorkDirPath.Size = new System.Drawing.Size(34, 25);
			this.commandLauncherWorkDirPath.TabIndex = 2;
			this.commandLauncherWorkDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherWorkDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherWorkDirPath.Click += new System.EventHandler(this.CommandLauncherWorkDirPath_Click);
			// 
			// commandLauncherDirPath
			// 
			this.commandLauncherDirPath.Image = global::PeMain.Properties.Images.Dir;
			this.commandLauncherDirPath.Location = new System.Drawing.Point(265, 94);
			this.commandLauncherDirPath.Name = "commandLauncherDirPath";
			this.commandLauncherDirPath.Size = new System.Drawing.Size(34, 25);
			this.commandLauncherDirPath.TabIndex = 2;
			this.commandLauncherDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherDirPath.Click += new System.EventHandler(this.CommandLauncherDirPath_Click);
			// 
			// commandLauncherFilePath
			// 
			this.commandLauncherFilePath.Image = global::PeMain.Properties.Images.File;
			this.commandLauncherFilePath.Location = new System.Drawing.Point(225, 94);
			this.commandLauncherFilePath.Name = "commandLauncherFilePath";
			this.commandLauncherFilePath.Size = new System.Drawing.Size(34, 25);
			this.commandLauncherFilePath.TabIndex = 2;
			this.commandLauncherFilePath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherFilePath.UseVisualStyleBackColor = true;
			this.commandLauncherFilePath.Click += new System.EventHandler(this.CommandLauncherFilePath_Click);
			// 
			// groupLauncherType
			// 
			this.groupLauncherType.Controls.Add(this.selectLauncherType_uri);
			this.groupLauncherType.Controls.Add(this.selectLauncherType_file);
			this.groupLauncherType.Location = new System.Drawing.Point(13, 3);
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
			this.selectLauncherType_uri.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
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
			this.selectLauncherType_file.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// pageCommand
			// 
			this.pageCommand.Controls.Add(this.labelCommandHotkey);
			this.pageCommand.Controls.Add(this.inputCommandHotkey);
			this.pageCommand.Controls.Add(this.labelCommandIcon);
			this.pageCommand.Controls.Add(this.selectCommandIcon);
			this.pageCommand.Controls.Add(this.inputCommandHideTime);
			this.pageCommand.Controls.Add(this.selectCommandTopmost);
			this.pageCommand.Controls.Add(this.titleCommandHideTime);
			this.pageCommand.Controls.Add(this.commandCommandFont);
			this.pageCommand.Controls.Add(this.titleCommandFont);
			this.pageCommand.Location = new System.Drawing.Point(4, 24);
			this.pageCommand.Name = "pageCommand";
			this.pageCommand.Padding = new System.Windows.Forms.Padding(3);
			this.pageCommand.Size = new System.Drawing.Size(734, 310);
			this.pageCommand.TabIndex = 2;
			this.pageCommand.Text = "{COMMAND}";
			this.pageCommand.UseVisualStyleBackColor = true;
			// 
			// labelCommandHotkey
			// 
			this.labelCommandHotkey.Location = new System.Drawing.Point(35, 109);
			this.labelCommandHotkey.Name = "labelCommandHotkey";
			this.labelCommandHotkey.Size = new System.Drawing.Size(100, 23);
			this.labelCommandHotkey.TabIndex = 16;
			this.labelCommandHotkey.Text = "{HOT KEY}";
			// 
			// inputCommandHotkey
			// 
			this.inputCommandHotkey.BackColor = System.Drawing.Color.White;
			this.inputCommandHotkey.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.inputCommandHotkey.Hotkey = System.Windows.Forms.Keys.None;
			this.inputCommandHotkey.Language = null;
			this.inputCommandHotkey.Location = new System.Drawing.Point(156, 105);
			this.inputCommandHotkey.Modifiers = System.Windows.Forms.Keys.None;
			this.inputCommandHotkey.Name = "inputCommandHotkey";
			this.inputCommandHotkey.ReadOnly = true;
			this.inputCommandHotkey.Size = new System.Drawing.Size(252, 27);
			this.inputCommandHotkey.TabIndex = 15;
			this.inputCommandHotkey.Text = "";
			// 
			// labelCommandIcon
			// 
			this.labelCommandIcon.Location = new System.Drawing.Point(40, 187);
			this.labelCommandIcon.Name = "labelCommandIcon";
			this.labelCommandIcon.Size = new System.Drawing.Size(100, 23);
			this.labelCommandIcon.TabIndex = 14;
			this.labelCommandIcon.Text = "{ICON}";
			// 
			// selectCommandIcon
			// 
			this.selectCommandIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectCommandIcon.FormattingEnabled = true;
			this.selectCommandIcon.Location = new System.Drawing.Point(146, 190);
			this.selectCommandIcon.Name = "selectCommandIcon";
			this.selectCommandIcon.Size = new System.Drawing.Size(121, 23);
			this.selectCommandIcon.TabIndex = 13;
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
			this.selectCommandTopmost.Location = new System.Drawing.Point(36, 160);
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
			this.commandCommandFont.Click += new System.EventHandler(this.CommandCommandFont_Click);
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
			this.pageToolbar.Controls.Add(this.selectToolbarShowText);
			this.pageToolbar.Controls.Add(this.splitContainer2);
			this.pageToolbar.Controls.Add(this.labelToolbarIcon);
			this.pageToolbar.Controls.Add(this.selectToolbarIcon);
			this.pageToolbar.Controls.Add(this.labelToolbarPosition);
			this.pageToolbar.Controls.Add(this.selectToolbarPosition);
			this.pageToolbar.Controls.Add(this.selectToolbarVisible);
			this.pageToolbar.Controls.Add(this.selectToolbarAutoHide);
			this.pageToolbar.Controls.Add(this.selectToolbarTopmost);
			this.pageToolbar.Controls.Add(this.commandToolbarFont);
			this.pageToolbar.Controls.Add(this.labelToolbarFonr);
			this.pageToolbar.Location = new System.Drawing.Point(4, 24);
			this.pageToolbar.Name = "pageToolbar";
			this.pageToolbar.Size = new System.Drawing.Size(734, 310);
			this.pageToolbar.TabIndex = 3;
			this.pageToolbar.Text = "{TOOLBAR}";
			this.pageToolbar.UseVisualStyleBackColor = true;
			// 
			// selectToolbarShowText
			// 
			this.selectToolbarShowText.Location = new System.Drawing.Point(133, 72);
			this.selectToolbarShowText.Name = "selectToolbarShowText";
			this.selectToolbarShowText.Size = new System.Drawing.Size(104, 24);
			this.selectToolbarShowText.TabIndex = 16;
			this.selectToolbarShowText.Text = "{SHOWTEXT}";
			this.selectToolbarShowText.UseVisualStyleBackColor = true;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Location = new System.Drawing.Point(269, 78);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.toolStripContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.selecterToolbar);
			this.splitContainer2.Size = new System.Drawing.Size(287, 179);
			this.splitContainer2.SplitterDistance = 171;
			this.splitContainer2.TabIndex = 15;
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.treeToolbarItemGroup);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(171, 154);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(171, 179);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolToolbarGroup);
			// 
			// treeToolbarItemGroup
			// 
			this.treeToolbarItemGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeToolbarItemGroup.HideSelection = false;
			this.treeToolbarItemGroup.LabelEdit = true;
			this.treeToolbarItemGroup.Location = new System.Drawing.Point(0, 0);
			this.treeToolbarItemGroup.Name = "treeToolbarItemGroup";
			this.treeToolbarItemGroup.ShowPlusMinus = false;
			this.treeToolbarItemGroup.Size = new System.Drawing.Size(171, 154);
			this.treeToolbarItemGroup.TabIndex = 13;
			this.treeToolbarItemGroup.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeToolbarItemGroup_BeforeLabelEdit);
			this.treeToolbarItemGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeToolbarItemGroup_AfterSelect);
			this.treeToolbarItemGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeToolbarItemGroup_KeyDown);
			// 
			// toolToolbarGroup
			// 
			this.toolToolbarGroup.Dock = System.Windows.Forms.DockStyle.None;
			this.toolToolbarGroup.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolToolbarGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolToolbarGroup_addGroup,
									this.toolToolbarGroup_addItem,
									this.toolStripSeparator1,
									this.toolToolbarGroup_up,
									this.toolToolbarGroup_down,
									this.toolStripSeparator2,
									this.toolToolbarGroup_remove});
			this.toolToolbarGroup.Location = new System.Drawing.Point(0, 0);
			this.toolToolbarGroup.Name = "toolToolbarGroup";
			this.toolToolbarGroup.Size = new System.Drawing.Size(171, 25);
			this.toolToolbarGroup.Stretch = true;
			this.toolToolbarGroup.TabIndex = 0;
			this.toolToolbarGroup.Text = "toolStrip1";
			// 
			// toolToolbarGroup_addGroup
			// 
			this.toolToolbarGroup_addGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_addGroup.Image = global::PeMain.Properties.Images.Group;
			this.toolToolbarGroup_addGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_addGroup.Name = "toolToolbarGroup_addGroup";
			this.toolToolbarGroup_addGroup.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_addGroup.Text = "toolStripButton1";
			this.toolToolbarGroup_addGroup.Click += new System.EventHandler(this.ToolToolbarGroup_addGroup_Click);
			// 
			// toolToolbarGroup_addItem
			// 
			this.toolToolbarGroup_addItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_addItem.Image = global::PeMain.Properties.Images.AddItem;
			this.toolToolbarGroup_addItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_addItem.Name = "toolToolbarGroup_addItem";
			this.toolToolbarGroup_addItem.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_addItem.Text = "toolStripButton2";
			this.toolToolbarGroup_addItem.Click += new System.EventHandler(this.ToolToolbarGroup_addItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolToolbarGroup_up
			// 
			this.toolToolbarGroup_up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_up.Image = global::PeMain.Properties.Images.Up;
			this.toolToolbarGroup_up.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_up.Name = "toolToolbarGroup_up";
			this.toolToolbarGroup_up.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_up.Text = "toolStripButton1";
			this.toolToolbarGroup_up.Click += new System.EventHandler(this.ToolToolbarGroup_up_Click);
			// 
			// toolToolbarGroup_down
			// 
			this.toolToolbarGroup_down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_down.Image = global::PeMain.Properties.Images.Down;
			this.toolToolbarGroup_down.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_down.Name = "toolToolbarGroup_down";
			this.toolToolbarGroup_down.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_down.Text = "toolStripButton2";
			this.toolToolbarGroup_down.Click += new System.EventHandler(this.ToolToolbarGroup_down_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolToolbarGroup_remove
			// 
			this.toolToolbarGroup_remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_remove.Image = global::PeMain.Properties.Images.Remove;
			this.toolToolbarGroup_remove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_remove.Name = "toolToolbarGroup_remove";
			this.toolToolbarGroup_remove.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_remove.Text = "toolStripButton3";
			this.toolToolbarGroup_remove.Click += new System.EventHandler(this.ToolToolbarGroup_remove_Click);
			// 
			// selecterToolbar
			// 
			this.selecterToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selecterToolbar.Filtering = false;
			this.selecterToolbar.FilterType = PeMain.UI.LauncherItemSelecterType.Full;
			this.selecterToolbar.IconSize = PeUtility.IconSize.Small;
			this.selecterToolbar.ItemEdit = false;
			this.selecterToolbar.Location = new System.Drawing.Point(0, 0);
			this.selecterToolbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterToolbar.Name = "selecterToolbar";
			this.selecterToolbar.SelectedItem = null;
			this.selecterToolbar.Size = new System.Drawing.Size(112, 179);
			this.selecterToolbar.TabIndex = 14;
			this.selecterToolbar.SelectChangedItem += new System.EventHandler<PeMain.UI.SelectedItemEventArg>(this.SelecterToolbar_SelectChangedItem);
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
			this.selectToolbarIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
			this.selectToolbarPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarPosition.FormattingEnabled = true;
			this.selectToolbarPosition.Location = new System.Drawing.Point(129, 102);
			this.selectToolbarPosition.Name = "selectToolbarPosition";
			this.selectToolbarPosition.Size = new System.Drawing.Size(121, 23);
			this.selectToolbarPosition.TabIndex = 9;
			// 
			// selectToolbarVisible
			// 
			this.selectToolbarVisible.Location = new System.Drawing.Point(23, 78);
			this.selectToolbarVisible.Name = "selectToolbarVisible";
			this.selectToolbarVisible.Size = new System.Drawing.Size(104, 24);
			this.selectToolbarVisible.TabIndex = 8;
			this.selectToolbarVisible.Text = "{VISIBLE}";
			this.selectToolbarVisible.UseVisualStyleBackColor = true;
			// 
			// selectToolbarAutoHide
			// 
			this.selectToolbarAutoHide.Location = new System.Drawing.Point(133, 47);
			this.selectToolbarAutoHide.Name = "selectToolbarAutoHide";
			this.selectToolbarAutoHide.Size = new System.Drawing.Size(104, 24);
			this.selectToolbarAutoHide.TabIndex = 7;
			this.selectToolbarAutoHide.Text = "{AUTOHIDE}";
			this.selectToolbarAutoHide.UseVisualStyleBackColor = true;
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
			this.commandToolbarFont.Click += new System.EventHandler(this.CommandToolbarFont_Click);
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
			this.pageNote.Size = new System.Drawing.Size(734, 310);
			this.pageNote.TabIndex = 6;
			this.pageNote.Text = "{NOTE}";
			this.pageNote.UseVisualStyleBackColor = true;
			// 
			// pageDisplay
			// 
			this.pageDisplay.Location = new System.Drawing.Point(4, 24);
			this.pageDisplay.Name = "pageDisplay";
			this.pageDisplay.Size = new System.Drawing.Size(734, 310);
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
			this.commandSubmit.Click += new System.EventHandler(this.CommandSubmit_Click);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(748, 389);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.commandSubmit);
			this.flowLayoutPanel1.Controls.Add(this.commandCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(559, 349);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 37);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// SettingForm
			// 
			this.AcceptButton = this.commandSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(748, 389);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "SettingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "{SETTING}";
			this.tabSetting.ResumeLayout(false);
			this.pageMain.ResumeLayout(false);
			this.groupMainLog.ResumeLayout(false);
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
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.toolToolbarGroup.ResumeLayout(false);
			this.toolToolbarGroup.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button commandLauncherCommandSetter;
		private System.Windows.Forms.Label labelCommandHotkey;
		private PeMain.UI.PeHotkeyControl inputCommandHotkey;
		private System.Windows.Forms.CheckBox selectLogAddShow;
		private System.Windows.Forms.CheckBox selectLogVisible;
		private System.Windows.Forms.GroupBox groupMainLog;
		private System.Windows.Forms.CheckBox selectToolbarShowText;
		private System.Windows.Forms.CheckBox selectToolbarAutoHide;
		private System.Windows.Forms.CheckBox selectLauncherStdStream;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_remove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_down;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_up;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_addItem;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_addGroup;
		private System.Windows.Forms.ToolStrip toolToolbarGroup;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.ComboBox selectCommandIcon;
		private System.Windows.Forms.Label labelCommandIcon;
		private System.Windows.Forms.Button commandLauncherOptionFilePath;
		private System.Windows.Forms.Button commandLauncherOptionDirPath;
		private System.Windows.Forms.TextBox inputLauncherOption;
		private System.Windows.Forms.Label labelLauncherOption;
		private System.Windows.Forms.Label labelMainLanguage;
		private System.Windows.Forms.ComboBox selectMainLanguage;
		private System.Windows.Forms.Button commandLauncherDirPath;
		private System.Windows.Forms.TextBox inputLauncherTag;
		private System.Windows.Forms.TextBox inputLauncherNote;
		private System.Windows.Forms.Label labelLauncherTag;
		private System.Windows.Forms.Label labelLauncherNote;
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
		private System.Windows.Forms.CheckBox selectToolbarVisible;
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
