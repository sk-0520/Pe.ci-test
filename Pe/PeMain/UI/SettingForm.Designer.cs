using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.UI
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tabSetting = new System.Windows.Forms.TabControl();
			this.tabSetting_pageMain = new System.Windows.Forms.TabPage();
			this.groupLauncherStream = new System.Windows.Forms.GroupBox();
			this.commandLauncherStreamFont = new ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton();
			this.groupMainSkin = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.selectSkinName = new System.Windows.Forms.ComboBox();
			this.commandSkinAbout = new System.Windows.Forms.Button();
			this.panelMainOthers = new System.Windows.Forms.FlowLayoutPanel();
			this.labelMainLanguage = new System.Windows.Forms.Label();
			this.selectMainLanguage = new System.Windows.Forms.ComboBox();
			this.selectMainStartup = new System.Windows.Forms.CheckBox();
			this.groupUpdateCheck = new System.Windows.Forms.GroupBox();
			this.panelUpdate = new System.Windows.Forms.FlowLayoutPanel();
			this.selectUpdateCheck = new System.Windows.Forms.CheckBox();
			this.selectUpdateCheckRC = new System.Windows.Forms.CheckBox();
			this.groupMainSystemEnv = new System.Windows.Forms.GroupBox();
			this.panelMainSystemEnv = new System.Windows.Forms.TableLayoutPanel();
			this.inputSystemEnvExt = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.labelSystemEnvExt = new System.Windows.Forms.Label();
			this.inputSystemEnvHiddenFile = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.labelSystemEnvHiddenFile = new System.Windows.Forms.Label();
			this.groupMainLog = new System.Windows.Forms.GroupBox();
			this.selectLogDebugging = new System.Windows.Forms.CheckBox();
			this.selectLogFullDetail = new System.Windows.Forms.CheckBox();
			this.selectLogAddShow = new System.Windows.Forms.CheckBox();
			this.groupLogTrigger = new System.Windows.Forms.GroupBox();
			this.selectLogTrigger_error = new System.Windows.Forms.CheckBox();
			this.selectLogTrigger_warning = new System.Windows.Forms.CheckBox();
			this.selectLogTrigger_information = new System.Windows.Forms.CheckBox();
			this.selectLogVisible = new System.Windows.Forms.CheckBox();
			this.tabSetting_pageLauncher = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.selecterLauncher = new ContentTypeTextNet.Pe.PeMain.UI.CustomControl.LauncherItemSelectControl();
			this.tabLauncher = new System.Windows.Forms.TabControl();
			this.tabLauncher_pageCommon = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.groupLauncherType = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.selectLauncherType_file = new System.Windows.Forms.RadioButton();
			this.selectLauncherType_directory = new System.Windows.Forms.RadioButton();
			this.selectLauncherType_command = new System.Windows.Forms.RadioButton();
			this.selectLauncherType_embedded = new System.Windows.Forms.RadioButton();
			this.commandLauncherOptionDirPath = new System.Windows.Forms.Button();
			this.inputLauncherOption = new System.Windows.Forms.TextBox();
			this.commandLauncherDirPath = new System.Windows.Forms.Button();
			this.commandLauncherFilePath = new System.Windows.Forms.Button();
			this.inputLauncherName = new System.Windows.Forms.TextBox();
			this.commandLauncherOptionFilePath = new System.Windows.Forms.Button();
			this.labelLauncherName = new System.Windows.Forms.Label();
			this.labelLauncherCommand = new System.Windows.Forms.Label();
			this.commandLauncherWorkDirPath = new System.Windows.Forms.Button();
			this.labelLauncherOption = new System.Windows.Forms.Label();
			this.commandLauncherIconPath = new System.Windows.Forms.Button();
			this.labelLauncherWorkDirPath = new System.Windows.Forms.Label();
			this.inputLauncherIconPath = new ContentTypeTextNet.Pe.PeMain.UI.Ex.IconTextBox();
			this.inputLauncherWorkDirPath = new System.Windows.Forms.TextBox();
			this.labelLauncherIconPath = new System.Windows.Forms.Label();
			this.inputLauncherCommand = new System.Windows.Forms.ComboBox();
			this.tabLauncher_pageEnv = new System.Windows.Forms.TabPage();
			this.panelLauncherEnv = new System.Windows.Forms.TableLayoutPanel();
			this.envLauncherRemove = new ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvRemoveControl();
			this.envLauncherUpdate = new ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvUpdateControl();
			this.selectLauncherEnv = new System.Windows.Forms.CheckBox();
			this.tabLauncher_pageOthers = new System.Windows.Forms.TabPage();
			this.panelLauncherOthers = new System.Windows.Forms.TableLayoutPanel();
			this.selectLauncherStdStream = new System.Windows.Forms.CheckBox();
			this.inputLauncherNote = new System.Windows.Forms.TextBox();
			this.inputLauncherTag = new System.Windows.Forms.TextBox();
			this.labelLauncherTag = new System.Windows.Forms.Label();
			this.selectLauncherAdmin = new System.Windows.Forms.CheckBox();
			this.labelLauncherNote = new System.Windows.Forms.Label();
			this.tabSetting_pageToolbar = new System.Windows.Forms.TabPage();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.groupToolbar = new System.Windows.Forms.GroupBox();
			this.commandToolbarScreens = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.selectToolbarVisible = new System.Windows.Forms.CheckBox();
			this.selectToolbarTopmost = new System.Windows.Forms.CheckBox();
			this.labelToolbarTextWidth = new System.Windows.Forms.Label();
			this.inputToolbarTextWidth = new System.Windows.Forms.NumericUpDown();
			this.labelToolbarFont = new System.Windows.Forms.Label();
			this.commandToolbarFont = new ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton();
			this.selectToolbarIcon = new System.Windows.Forms.ComboBox();
			this.labelToolbarIcon = new System.Windows.Forms.Label();
			this.labelToolbarPosition = new System.Windows.Forms.Label();
			this.selectToolbarPosition = new System.Windows.Forms.ComboBox();
			this.labelToolbarGroup = new System.Windows.Forms.Label();
			this.selectToolbarGroup = new System.Windows.Forms.ComboBox();
			this.selectToolbarAutoHide = new System.Windows.Forms.CheckBox();
			this.selectToolbarShowText = new System.Windows.Forms.CheckBox();
			this.selectToolbarItem = new System.Windows.Forms.ComboBox();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.treeToolbarItemGroup = new System.Windows.Forms.TreeView();
			this.toolToolbarGroup = new System.Windows.Forms.ToolStrip();
			this.toolToolbarGroup_addGroup = new System.Windows.Forms.ToolStripButton();
			this.toolToolbarGroup_addItem = new System.Windows.Forms.ToolStripButton();
			this.DisableCloseToolStripSeparator1 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.toolToolbarGroup_up = new System.Windows.Forms.ToolStripButton();
			this.toolToolbarGroup_down = new System.Windows.Forms.ToolStripButton();
			this.DisableCloseToolStripSeparator2 = new ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator();
			this.toolToolbarGroup_remove = new System.Windows.Forms.ToolStripButton();
			this.selecterToolbar = new ContentTypeTextNet.Pe.PeMain.UI.CustomControl.LauncherItemSelectControl();
			this.tabSetting_pageCommand = new System.Windows.Forms.TabPage();
			this.labelCommandHotkey = new System.Windows.Forms.Label();
			this.inputCommandHotkey = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.labelCommandIcon = new System.Windows.Forms.Label();
			this.selectCommandIcon = new System.Windows.Forms.ComboBox();
			this.inputCommandHideTime = new System.Windows.Forms.NumericUpDown();
			this.selectCommandTopmost = new System.Windows.Forms.CheckBox();
			this.labelCommandHideTime = new System.Windows.Forms.Label();
			this.commandCommandFont = new ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton();
			this.labelCommandFont = new System.Windows.Forms.Label();
			this.tabSetting_pageNote = new System.Windows.Forms.TabPage();
			this.panelNote = new System.Windows.Forms.TableLayoutPanel();
			this.panelNoteOthers = new System.Windows.Forms.TableLayoutPanel();
			this.commandNoteCaptionFont = new ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton();
			this.labelNoteCaptionFont = new System.Windows.Forms.Label();
			this.groupNoteKey = new System.Windows.Forms.GroupBox();
			this.panelNoteKey = new System.Windows.Forms.FlowLayoutPanel();
			this.labelNoteCreate = new System.Windows.Forms.Label();
			this.inputNoteCreate = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.labelNoteHiddent = new System.Windows.Forms.Label();
			this.inputNoteHidden = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.labelNoteCompact = new System.Windows.Forms.Label();
			this.inputNoteCompact = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.labelNoteShowFront = new System.Windows.Forms.Label();
			this.inputNoteShowFront = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.groupNoteItem = new System.Windows.Forms.GroupBox();
			this.gridNoteItems = new System.Windows.Forms.DataGridView();
			this.gridNoteItems_columnRemove = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.gridNoteItems_columnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridNoteItems_columnVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.gridNoteItems_columnLocked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.gridNoteItems_columnTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridNoteItems_columnBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridNoteItems_columnFont = new ContentTypeTextNet.Pe.PeMain.UI.Ex.NoteFontDataGridViewButtonColumn();
			this.gridNoteItems_columnFore = new ContentTypeTextNet.Pe.PeMain.UI.Ex.NoteColorDataGridViewButtonColumn();
			this.gridNoteItems_columnBack = new ContentTypeTextNet.Pe.PeMain.UI.Ex.NoteColorDataGridViewButtonColumn();
			this.tabSetting_pageDisplay = new System.Windows.Forms.TabPage();
			this.tabSetting_pageClipboard = new System.Windows.Forms.TabPage();
			this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.selectClipboardEnabled = new System.Windows.Forms.CheckBox();
			this.selectClipboardAppEnabled = new System.Windows.Forms.CheckBox();
			this.selectClipboardSave = new System.Windows.Forms.CheckBox();
			this.selectClipboardTopMost = new System.Windows.Forms.CheckBox();
			this.selectClipboardVisible = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.panelClipboardTypes = new System.Windows.Forms.FlowLayoutPanel();
			this.groupClipboardType = new System.Windows.Forms.GroupBox();
			this.panelClipboardType = new System.Windows.Forms.FlowLayoutPanel();
			this.selectClipboardType_text = new System.Windows.Forms.CheckBox();
			this.selectClipboardType_rtf = new System.Windows.Forms.CheckBox();
			this.selectClipboardType_html = new System.Windows.Forms.CheckBox();
			this.selectClipboardType_image = new System.Windows.Forms.CheckBox();
			this.selectClipboardType_file = new System.Windows.Forms.CheckBox();
			this.groupClipboardSaveType = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.selectClipboardSaveType_text = new System.Windows.Forms.CheckBox();
			this.selectClipboardSaveType_rtf = new System.Windows.Forms.CheckBox();
			this.selectClipboardSaveType_html = new System.Windows.Forms.CheckBox();
			this.selectClipboardSaveType_image = new System.Windows.Forms.CheckBox();
			this.selectClipboardSaveType_file = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.labelClipboardListType = new System.Windows.Forms.Label();
			this.labelClipboardHotkey = new System.Windows.Forms.Label();
			this.inputClipboardHotkey = new ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl();
			this.labelClipboardFont = new System.Windows.Forms.Label();
			this.commandClipboardTextFont = new ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton();
			this.selectClipboardListType = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.labelClipboardLimit = new System.Windows.Forms.Label();
			this.labelClipboardWaitTaime = new System.Windows.Forms.Label();
			this.labelClipboardSleepTime = new System.Windows.Forms.Label();
			this.inputClipboardLimit = new System.Windows.Forms.NumericUpDown();
			this.inputClipboardSleepTime = new System.Windows.Forms.NumericUpDown();
			this.inputClipboardWaitTime = new System.Windows.Forms.NumericUpDown();
			this.commandCancel = new System.Windows.Forms.Button();
			this.commandSubmit = new System.Windows.Forms.Button();
			this.panelSetting = new System.Windows.Forms.TableLayoutPanel();
			this.panelCommand = new System.Windows.Forms.FlowLayoutPanel();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.tabSetting.SuspendLayout();
			this.tabSetting_pageMain.SuspendLayout();
			this.groupLauncherStream.SuspendLayout();
			this.groupMainSkin.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.panelMainOthers.SuspendLayout();
			this.groupUpdateCheck.SuspendLayout();
			this.panelUpdate.SuspendLayout();
			this.groupMainSystemEnv.SuspendLayout();
			this.panelMainSystemEnv.SuspendLayout();
			this.groupMainLog.SuspendLayout();
			this.groupLogTrigger.SuspendLayout();
			this.tabSetting_pageLauncher.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabLauncher.SuspendLayout();
			this.tabLauncher_pageCommon.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.groupLauncherType.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.tabLauncher_pageEnv.SuspendLayout();
			this.panelLauncherEnv.SuspendLayout();
			this.tabLauncher_pageOthers.SuspendLayout();
			this.panelLauncherOthers.SuspendLayout();
			this.tabSetting_pageToolbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.groupToolbar.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputToolbarTextWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.toolToolbarGroup.SuspendLayout();
			this.tabSetting_pageCommand.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).BeginInit();
			this.tabSetting_pageNote.SuspendLayout();
			this.panelNote.SuspendLayout();
			this.panelNoteOthers.SuspendLayout();
			this.groupNoteKey.SuspendLayout();
			this.panelNoteKey.SuspendLayout();
			this.groupNoteItem.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridNoteItems)).BeginInit();
			this.tabSetting_pageClipboard.SuspendLayout();
			this.flowLayoutPanel9.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.panelClipboardTypes.SuspendLayout();
			this.groupClipboardType.SuspendLayout();
			this.panelClipboardType.SuspendLayout();
			this.groupClipboardSaveType.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel8.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputClipboardLimit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.inputClipboardSleepTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.inputClipboardWaitTime)).BeginInit();
			this.panelSetting.SuspendLayout();
			this.panelCommand.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// tabSetting
			// 
			this.tabSetting.Controls.Add(this.tabSetting_pageMain);
			this.tabSetting.Controls.Add(this.tabSetting_pageLauncher);
			this.tabSetting.Controls.Add(this.tabSetting_pageToolbar);
			this.tabSetting.Controls.Add(this.tabSetting_pageCommand);
			this.tabSetting.Controls.Add(this.tabSetting_pageNote);
			this.tabSetting.Controls.Add(this.tabSetting_pageDisplay);
			this.tabSetting.Controls.Add(this.tabSetting_pageClipboard);
			this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSetting.Location = new System.Drawing.Point(3, 4);
			this.tabSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting.Name = "tabSetting";
			this.tabSetting.SelectedIndex = 0;
			this.tabSetting.Size = new System.Drawing.Size(778, 331);
			this.tabSetting.TabIndex = 0;
			this.tabSetting.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabSetting_Selecting);
			// 
			// tabSetting_pageMain
			// 
			this.tabSetting_pageMain.Controls.Add(this.groupLauncherStream);
			this.tabSetting_pageMain.Controls.Add(this.groupMainSkin);
			this.tabSetting_pageMain.Controls.Add(this.panelMainOthers);
			this.tabSetting_pageMain.Controls.Add(this.groupUpdateCheck);
			this.tabSetting_pageMain.Controls.Add(this.groupMainSystemEnv);
			this.tabSetting_pageMain.Controls.Add(this.groupMainLog);
			this.tabSetting_pageMain.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageMain.Name = "tabSetting_pageMain";
			this.tabSetting_pageMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageMain.Size = new System.Drawing.Size(770, 303);
			this.tabSetting_pageMain.TabIndex = 0;
			this.tabSetting_pageMain.Text = ":setting/page/main";
			this.tabSetting_pageMain.UseVisualStyleBackColor = true;
			// 
			// groupLauncherStream
			// 
			this.groupLauncherStream.Controls.Add(this.commandLauncherStreamFont);
			this.groupLauncherStream.Location = new System.Drawing.Point(224, 194);
			this.groupLauncherStream.Name = "groupLauncherStream";
			this.groupLauncherStream.Size = new System.Drawing.Size(288, 75);
			this.groupLauncherStream.TabIndex = 4;
			this.groupLauncherStream.TabStop = false;
			this.groupLauncherStream.Text = ":setting/group/stream";
			// 
			// commandLauncherStreamFont
			// 
			this.commandLauncherStreamFont.AutoSize = true;
			this.commandLauncherStreamFont.Location = new System.Drawing.Point(9, 22);
			this.commandLauncherStreamFont.Name = "commandLauncherStreamFont";
			this.commandLauncherStreamFont.Size = new System.Drawing.Size(141, 25);
			this.commandLauncherStreamFont.TabIndex = 0;
			this.commandLauncherStreamFont.Text = "{FAMILY} {PT} ...";
			this.commandLauncherStreamFont.UseVisualStyleBackColor = true;
			// 
			// groupMainSkin
			// 
			this.groupMainSkin.AutoSize = true;
			this.groupMainSkin.Controls.Add(this.flowLayoutPanel1);
			this.groupMainSkin.Location = new System.Drawing.Point(224, 42);
			this.groupMainSkin.Name = "groupMainSkin";
			this.groupMainSkin.Size = new System.Drawing.Size(288, 51);
			this.groupMainSkin.TabIndex = 2;
			this.groupMainSkin.TabStop = false;
			this.groupMainSkin.Text = ":setting/group/skin";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.selectSkinName);
			this.flowLayoutPanel1.Controls.Add(this.commandSkinAbout);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 19);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(282, 29);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// selectSkinName
			// 
			this.selectSkinName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectSkinName.FormattingEnabled = true;
			this.selectSkinName.Location = new System.Drawing.Point(3, 3);
			this.selectSkinName.Name = "selectSkinName";
			this.selectSkinName.Size = new System.Drawing.Size(176, 23);
			this.selectSkinName.TabIndex = 0;
			this.selectSkinName.SelectedValueChanged += new System.EventHandler(this.selectSkinName_SelectedValueChanged);
			// 
			// commandSkinAbout
			// 
			this.commandSkinAbout.Location = new System.Drawing.Point(185, 3);
			this.commandSkinAbout.Name = "commandSkinAbout";
			this.commandSkinAbout.Size = new System.Drawing.Size(84, 23);
			this.commandSkinAbout.TabIndex = 5;
			this.commandSkinAbout.Text = ":setting/command/skin";
			this.commandSkinAbout.UseVisualStyleBackColor = true;
			this.commandSkinAbout.Click += new System.EventHandler(this.commandSkinAbout_Click);
			// 
			// panelMainOthers
			// 
			this.panelMainOthers.AutoSize = true;
			this.panelMainOthers.Controls.Add(this.labelMainLanguage);
			this.panelMainOthers.Controls.Add(this.selectMainLanguage);
			this.panelMainOthers.Controls.Add(this.selectMainStartup);
			this.panelMainOthers.Location = new System.Drawing.Point(15, 7);
			this.panelMainOthers.Name = "panelMainOthers";
			this.panelMainOthers.Size = new System.Drawing.Size(559, 29);
			this.panelMainOthers.TabIndex = 0;
			// 
			// labelMainLanguage
			// 
			this.labelMainLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelMainLanguage.AutoSize = true;
			this.labelMainLanguage.Location = new System.Drawing.Point(3, 7);
			this.labelMainLanguage.Name = "labelMainLanguage";
			this.labelMainLanguage.Size = new System.Drawing.Size(143, 15);
			this.labelMainLanguage.TabIndex = 0;
			this.labelMainLanguage.Text = ":setting/label/language";
			// 
			// selectMainLanguage
			// 
			this.selectMainLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectMainLanguage.FormattingEnabled = true;
			this.selectMainLanguage.Location = new System.Drawing.Point(152, 3);
			this.selectMainLanguage.Name = "selectMainLanguage";
			this.selectMainLanguage.Size = new System.Drawing.Size(121, 23);
			this.selectMainLanguage.TabIndex = 0;
			// 
			// selectMainStartup
			// 
			this.selectMainStartup.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.selectMainStartup.AutoSize = true;
			this.selectMainStartup.Location = new System.Drawing.Point(336, 5);
			this.selectMainStartup.Margin = new System.Windows.Forms.Padding(60, 3, 3, 3);
			this.selectMainStartup.Name = "selectMainStartup";
			this.selectMainStartup.Size = new System.Drawing.Size(158, 19);
			this.selectMainStartup.TabIndex = 1;
			this.selectMainStartup.Text = ":setting/check/startup";
			this.selectMainStartup.UseVisualStyleBackColor = true;
			// 
			// groupUpdateCheck
			// 
			this.groupUpdateCheck.AutoSize = true;
			this.groupUpdateCheck.Controls.Add(this.panelUpdate);
			this.groupUpdateCheck.Location = new System.Drawing.Point(518, 42);
			this.groupUpdateCheck.Name = "groupUpdateCheck";
			this.groupUpdateCheck.Size = new System.Drawing.Size(226, 81);
			this.groupUpdateCheck.TabIndex = 5;
			this.groupUpdateCheck.TabStop = false;
			this.groupUpdateCheck.Text = ":setting/group/update-check";
			// 
			// panelUpdate
			// 
			this.panelUpdate.AutoSize = true;
			this.panelUpdate.Controls.Add(this.selectUpdateCheck);
			this.panelUpdate.Controls.Add(this.selectUpdateCheckRC);
			this.panelUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelUpdate.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.panelUpdate.Location = new System.Drawing.Point(3, 19);
			this.panelUpdate.Name = "panelUpdate";
			this.panelUpdate.Size = new System.Drawing.Size(220, 59);
			this.panelUpdate.TabIndex = 4;
			// 
			// selectUpdateCheck
			// 
			this.selectUpdateCheck.AutoSize = true;
			this.selectUpdateCheck.Location = new System.Drawing.Point(3, 3);
			this.selectUpdateCheck.Name = "selectUpdateCheck";
			this.selectUpdateCheck.Size = new System.Drawing.Size(194, 19);
			this.selectUpdateCheck.TabIndex = 0;
			this.selectUpdateCheck.Text = ":setting/check/update-check";
			this.selectUpdateCheck.UseVisualStyleBackColor = true;
			// 
			// selectUpdateCheckRC
			// 
			this.selectUpdateCheckRC.AutoSize = true;
			this.selectUpdateCheckRC.Location = new System.Drawing.Point(3, 28);
			this.selectUpdateCheckRC.Name = "selectUpdateCheckRC";
			this.selectUpdateCheckRC.Size = new System.Drawing.Size(214, 19);
			this.selectUpdateCheckRC.TabIndex = 1;
			this.selectUpdateCheckRC.Text = ":setting/check/update-check.RC";
			this.selectUpdateCheckRC.UseVisualStyleBackColor = true;
			// 
			// groupMainSystemEnv
			// 
			this.groupMainSystemEnv.AutoSize = true;
			this.groupMainSystemEnv.Controls.Add(this.panelMainSystemEnv);
			this.groupMainSystemEnv.Location = new System.Drawing.Point(224, 99);
			this.groupMainSystemEnv.Name = "groupMainSystemEnv";
			this.groupMainSystemEnv.Size = new System.Drawing.Size(288, 89);
			this.groupMainSystemEnv.TabIndex = 3;
			this.groupMainSystemEnv.TabStop = false;
			this.groupMainSystemEnv.Text = ":setting/group/system-env";
			// 
			// panelMainSystemEnv
			// 
			this.panelMainSystemEnv.ColumnCount = 2;
			this.panelMainSystemEnv.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.panelMainSystemEnv.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelMainSystemEnv.Controls.Add(this.inputSystemEnvExt, 1, 1);
			this.panelMainSystemEnv.Controls.Add(this.labelSystemEnvExt, 0, 1);
			this.panelMainSystemEnv.Controls.Add(this.inputSystemEnvHiddenFile, 1, 0);
			this.panelMainSystemEnv.Controls.Add(this.labelSystemEnvHiddenFile, 0, 0);
			this.panelMainSystemEnv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMainSystemEnv.Location = new System.Drawing.Point(3, 19);
			this.panelMainSystemEnv.Name = "panelMainSystemEnv";
			this.panelMainSystemEnv.RowCount = 3;
			this.panelMainSystemEnv.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelMainSystemEnv.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelMainSystemEnv.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.panelMainSystemEnv.Size = new System.Drawing.Size(282, 67);
			this.panelMainSystemEnv.TabIndex = 5;
			// 
			// inputSystemEnvExt
			// 
			this.inputSystemEnvExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.inputSystemEnvExt.BackColor = System.Drawing.Color.White;
			this.inputSystemEnvExt.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.inputSystemEnvExt.Hotkey = System.Windows.Forms.Keys.None;
			this.inputSystemEnvExt.HotKeySetting = null;
			this.inputSystemEnvExt.Location = new System.Drawing.Point(156, 32);
			this.inputSystemEnvExt.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputSystemEnvExt.Name = "inputSystemEnvExt";
			this.inputSystemEnvExt.ReadOnly = true;
			this.inputSystemEnvExt.Registered = false;
			this.inputSystemEnvExt.Size = new System.Drawing.Size(123, 23);
			this.inputSystemEnvExt.TabIndex = 1;
			this.inputSystemEnvExt.Text = "None";
			// 
			// labelSystemEnvExt
			// 
			this.labelSystemEnvExt.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelSystemEnvExt.AutoSize = true;
			this.labelSystemEnvExt.Location = new System.Drawing.Point(3, 36);
			this.labelSystemEnvExt.Name = "labelSystemEnvExt";
			this.labelSystemEnvExt.Size = new System.Drawing.Size(147, 15);
			this.labelSystemEnvExt.TabIndex = 17;
			this.labelSystemEnvExt.Text = ":setting/label/extension";
			// 
			// inputSystemEnvHiddenFile
			// 
			this.inputSystemEnvHiddenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.inputSystemEnvHiddenFile.BackColor = System.Drawing.Color.White;
			this.inputSystemEnvHiddenFile.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.inputSystemEnvHiddenFile.Hotkey = System.Windows.Forms.Keys.None;
			this.inputSystemEnvHiddenFile.HotKeySetting = null;
			this.inputSystemEnvHiddenFile.Location = new System.Drawing.Point(156, 3);
			this.inputSystemEnvHiddenFile.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputSystemEnvHiddenFile.Name = "inputSystemEnvHiddenFile";
			this.inputSystemEnvHiddenFile.ReadOnly = true;
			this.inputSystemEnvHiddenFile.Registered = false;
			this.inputSystemEnvHiddenFile.Size = new System.Drawing.Size(123, 23);
			this.inputSystemEnvHiddenFile.TabIndex = 0;
			this.inputSystemEnvHiddenFile.Text = "None";
			// 
			// labelSystemEnvHiddenFile
			// 
			this.labelSystemEnvHiddenFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelSystemEnvHiddenFile.AutoSize = true;
			this.labelSystemEnvHiddenFile.Location = new System.Drawing.Point(3, 7);
			this.labelSystemEnvHiddenFile.Name = "labelSystemEnvHiddenFile";
			this.labelSystemEnvHiddenFile.Size = new System.Drawing.Size(146, 15);
			this.labelSystemEnvHiddenFile.TabIndex = 17;
			this.labelSystemEnvHiddenFile.Text = ":setting/label/hiddenfile";
			// 
			// groupMainLog
			// 
			this.groupMainLog.AutoSize = true;
			this.groupMainLog.Controls.Add(this.selectLogDebugging);
			this.groupMainLog.Controls.Add(this.selectLogFullDetail);
			this.groupMainLog.Controls.Add(this.selectLogAddShow);
			this.groupMainLog.Controls.Add(this.groupLogTrigger);
			this.groupMainLog.Controls.Add(this.selectLogVisible);
			this.groupMainLog.Location = new System.Drawing.Point(15, 42);
			this.groupMainLog.Name = "groupMainLog";
			this.groupMainLog.Size = new System.Drawing.Size(200, 227);
			this.groupMainLog.TabIndex = 1;
			this.groupMainLog.TabStop = false;
			this.groupMainLog.Text = ":setting/group/log";
			// 
			// selectLogDebugging
			// 
			this.selectLogDebugging.AutoSize = true;
			this.selectLogDebugging.Location = new System.Drawing.Point(16, 183);
			this.selectLogDebugging.Name = "selectLogDebugging";
			this.selectLogDebugging.Size = new System.Drawing.Size(175, 19);
			this.selectLogDebugging.TabIndex = 3;
			this.selectLogDebugging.Text = ":setting/check/debugging";
			this.selectLogDebugging.UseVisualStyleBackColor = true;
			// 
			// selectLogFullDetail
			// 
			this.selectLogFullDetail.AutoSize = true;
			this.selectLogFullDetail.Location = new System.Drawing.Point(16, 158);
			this.selectLogFullDetail.Name = "selectLogFullDetail";
			this.selectLogFullDetail.Size = new System.Drawing.Size(170, 19);
			this.selectLogFullDetail.TabIndex = 2;
			this.selectLogFullDetail.Text = ":setting/check/full-detail";
			this.selectLogFullDetail.UseVisualStyleBackColor = true;
			// 
			// selectLogAddShow
			// 
			this.selectLogAddShow.AutoSize = true;
			this.selectLogAddShow.Location = new System.Drawing.Point(16, 47);
			this.selectLogAddShow.Name = "selectLogAddShow";
			this.selectLogAddShow.Size = new System.Drawing.Size(172, 19);
			this.selectLogAddShow.TabIndex = 1;
			this.selectLogAddShow.Text = ":setting/check/add-show";
			this.selectLogAddShow.UseVisualStyleBackColor = true;
			// 
			// groupLogTrigger
			// 
			this.groupLogTrigger.Controls.Add(this.selectLogTrigger_error);
			this.groupLogTrigger.Controls.Add(this.selectLogTrigger_warning);
			this.groupLogTrigger.Controls.Add(this.selectLogTrigger_information);
			this.groupLogTrigger.Location = new System.Drawing.Point(6, 45);
			this.groupLogTrigger.Name = "groupLogTrigger";
			this.groupLogTrigger.Size = new System.Drawing.Size(188, 106);
			this.groupLogTrigger.TabIndex = 0;
			this.groupLogTrigger.TabStop = false;
			// 
			// selectLogTrigger_error
			// 
			this.selectLogTrigger_error.AutoSize = true;
			this.selectLogTrigger_error.Location = new System.Drawing.Point(17, 75);
			this.selectLogTrigger_error.Name = "selectLogTrigger_error";
			this.selectLogTrigger_error.Size = new System.Drawing.Size(118, 19);
			this.selectLogTrigger_error.TabIndex = 2;
			this.selectLogTrigger_error.Text = "#LogType.Error";
			this.selectLogTrigger_error.UseVisualStyleBackColor = true;
			// 
			// selectLogTrigger_warning
			// 
			this.selectLogTrigger_warning.AutoSize = true;
			this.selectLogTrigger_warning.Location = new System.Drawing.Point(17, 50);
			this.selectLogTrigger_warning.Name = "selectLogTrigger_warning";
			this.selectLogTrigger_warning.Size = new System.Drawing.Size(136, 19);
			this.selectLogTrigger_warning.TabIndex = 1;
			this.selectLogTrigger_warning.Text = "#LogType.Warning";
			this.selectLogTrigger_warning.UseVisualStyleBackColor = true;
			// 
			// selectLogTrigger_information
			// 
			this.selectLogTrigger_information.AutoSize = true;
			this.selectLogTrigger_information.Location = new System.Drawing.Point(17, 25);
			this.selectLogTrigger_information.Name = "selectLogTrigger_information";
			this.selectLogTrigger_information.Size = new System.Drawing.Size(158, 19);
			this.selectLogTrigger_information.TabIndex = 0;
			this.selectLogTrigger_information.Text = "#LogType.Information";
			this.selectLogTrigger_information.UseVisualStyleBackColor = true;
			// 
			// selectLogVisible
			// 
			this.selectLogVisible.AutoSize = true;
			this.selectLogVisible.Location = new System.Drawing.Point(16, 22);
			this.selectLogVisible.Name = "selectLogVisible";
			this.selectLogVisible.Size = new System.Drawing.Size(157, 19);
			this.selectLogVisible.TabIndex = 0;
			this.selectLogVisible.Text = ":common/label/visible";
			this.selectLogVisible.UseVisualStyleBackColor = true;
			// 
			// tabSetting_pageLauncher
			// 
			this.tabSetting_pageLauncher.AllowDrop = true;
			this.tabSetting_pageLauncher.Controls.Add(this.splitContainer1);
			this.tabSetting_pageLauncher.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageLauncher.Name = "tabSetting_pageLauncher";
			this.tabSetting_pageLauncher.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageLauncher.Size = new System.Drawing.Size(770, 283);
			this.tabSetting_pageLauncher.TabIndex = 1;
			this.tabSetting_pageLauncher.Text = ":setting/page/launcher";
			this.tabSetting_pageLauncher.UseVisualStyleBackColor = true;
			this.tabSetting_pageLauncher.DragDrop += new System.Windows.Forms.DragEventHandler(this.PageLauncher_DragDrop);
			this.tabSetting_pageLauncher.DragEnter += new System.Windows.Forms.DragEventHandler(this.PageLauncher_DragEnter);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(3, 4);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.selecterLauncher);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabLauncher);
			this.splitContainer1.Panel2.Enabled = false;
			this.splitContainer1.Size = new System.Drawing.Size(764, 275);
			this.splitContainer1.SplitterDistance = 193;
			this.splitContainer1.TabIndex = 0;
			// 
			// selecterLauncher
			// 
			this.selecterLauncher.ApplicationSetting = null;
			this.selecterLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selecterLauncher.Filtering = false;
			this.selecterLauncher.FilterType = ContentTypeTextNet.Pe.PeMain.UI.CustomControl.LauncherItemSelecterType.Full;
			this.selecterLauncher.IconScale = ContentTypeTextNet.Pe.Library.Skin.IconScale.Small;
			this.selecterLauncher.ItemEdit = true;
			this.selecterLauncher.Location = new System.Drawing.Point(0, 0);
			this.selecterLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterLauncher.Name = "selecterLauncher";
			this.selecterLauncher.SelectedItem = null;
			this.selecterLauncher.Size = new System.Drawing.Size(193, 275);
			this.selecterLauncher.TabIndex = 0;
			this.selecterLauncher.ItemCreate += new System.EventHandler<ContentTypeTextNet.Pe.PeMain.UI.CustomControl.CreateItemEventArg>(this.SelecterLauncher_CreateItem);
			this.selecterLauncher.SelectItemChanged += new System.EventHandler<ContentTypeTextNet.Pe.PeMain.UI.CustomControl.SelectedItemEventArg>(this.SelecterLauncher_SelectChnagedItem);
			// 
			// tabLauncher
			// 
			this.tabLauncher.Controls.Add(this.tabLauncher_pageCommon);
			this.tabLauncher.Controls.Add(this.tabLauncher_pageEnv);
			this.tabLauncher.Controls.Add(this.tabLauncher_pageOthers);
			this.tabLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabLauncher.Location = new System.Drawing.Point(0, 0);
			this.tabLauncher.Name = "tabLauncher";
			this.tabLauncher.SelectedIndex = 0;
			this.tabLauncher.Size = new System.Drawing.Size(567, 275);
			this.tabLauncher.TabIndex = 0;
			// 
			// tabLauncher_pageCommon
			// 
			this.tabLauncher_pageCommon.Controls.Add(this.tableLayoutPanel4);
			this.tabLauncher_pageCommon.Location = new System.Drawing.Point(4, 24);
			this.tabLauncher_pageCommon.Name = "tabLauncher_pageCommon";
			this.tabLauncher_pageCommon.Padding = new System.Windows.Forms.Padding(3);
			this.tabLauncher_pageCommon.Size = new System.Drawing.Size(559, 247);
			this.tabLauncher_pageCommon.TabIndex = 0;
			this.tabLauncher_pageCommon.Text = ":setting/page/launcher/basic";
			this.tabLauncher_pageCommon.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 4;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
			this.tableLayoutPanel4.Controls.Add(this.groupLauncherType, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.commandLauncherOptionDirPath, 3, 3);
			this.tableLayoutPanel4.Controls.Add(this.inputLauncherOption, 1, 3);
			this.tableLayoutPanel4.Controls.Add(this.commandLauncherDirPath, 3, 2);
			this.tableLayoutPanel4.Controls.Add(this.commandLauncherFilePath, 2, 2);
			this.tableLayoutPanel4.Controls.Add(this.inputLauncherName, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.commandLauncherOptionFilePath, 2, 3);
			this.tableLayoutPanel4.Controls.Add(this.labelLauncherName, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.labelLauncherCommand, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.commandLauncherWorkDirPath, 2, 4);
			this.tableLayoutPanel4.Controls.Add(this.labelLauncherOption, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.commandLauncherIconPath, 2, 5);
			this.tableLayoutPanel4.Controls.Add(this.labelLauncherWorkDirPath, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.inputLauncherIconPath, 1, 5);
			this.tableLayoutPanel4.Controls.Add(this.inputLauncherWorkDirPath, 1, 4);
			this.tableLayoutPanel4.Controls.Add(this.labelLauncherIconPath, 0, 5);
			this.tableLayoutPanel4.Controls.Add(this.inputLauncherCommand, 1, 2);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 8;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(553, 241);
			this.tableLayoutPanel4.TabIndex = 11;
			// 
			// groupLauncherType
			// 
			this.tableLayoutPanel4.SetColumnSpan(this.groupLauncherType, 4);
			this.groupLauncherType.Controls.Add(this.flowLayoutPanel3);
			this.groupLauncherType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupLauncherType.Location = new System.Drawing.Point(3, 3);
			this.groupLauncherType.Name = "groupLauncherType";
			this.groupLauncherType.Size = new System.Drawing.Size(547, 56);
			this.groupLauncherType.TabIndex = 0;
			this.groupLauncherType.TabStop = false;
			this.groupLauncherType.Text = ":setting/group/item-type";
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.selectLauncherType_file);
			this.flowLayoutPanel3.Controls.Add(this.selectLauncherType_directory);
			this.flowLayoutPanel3.Controls.Add(this.selectLauncherType_command);
			this.flowLayoutPanel3.Controls.Add(this.selectLauncherType_embedded);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 19);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(541, 34);
			this.flowLayoutPanel3.TabIndex = 2;
			// 
			// selectLauncherType_file
			// 
			this.selectLauncherType_file.AutoSize = true;
			this.selectLauncherType_file.Location = new System.Drawing.Point(3, 3);
			this.selectLauncherType_file.Name = "selectLauncherType_file";
			this.selectLauncherType_file.Size = new System.Drawing.Size(140, 19);
			this.selectLauncherType_file.TabIndex = 0;
			this.selectLauncherType_file.TabStop = true;
			this.selectLauncherType_file.Text = "#LauncherType.File";
			this.selectLauncherType_file.UseVisualStyleBackColor = true;
			this.selectLauncherType_file.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// selectLauncherType_directory
			// 
			this.selectLauncherType_directory.AutoSize = true;
			this.selectLauncherType_directory.Location = new System.Drawing.Point(149, 3);
			this.selectLauncherType_directory.Name = "selectLauncherType_directory";
			this.selectLauncherType_directory.Size = new System.Drawing.Size(137, 19);
			this.selectLauncherType_directory.TabIndex = 1;
			this.selectLauncherType_directory.TabStop = true;
			this.selectLauncherType_directory.Text = "#LauncherType.Dir";
			this.selectLauncherType_directory.UseVisualStyleBackColor = true;
			this.selectLauncherType_directory.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// selectLauncherType_command
			// 
			this.selectLauncherType_command.AutoSize = true;
			this.selectLauncherType_command.Location = new System.Drawing.Point(292, 3);
			this.selectLauncherType_command.Name = "selectLauncherType_command";
			this.selectLauncherType_command.Size = new System.Drawing.Size(180, 19);
			this.selectLauncherType_command.TabIndex = 2;
			this.selectLauncherType_command.TabStop = true;
			this.selectLauncherType_command.Text = "#LauncherType.Command";
			this.selectLauncherType_command.UseVisualStyleBackColor = true;
			this.selectLauncherType_command.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// selectLauncherType_embedded
			// 
			this.selectLauncherType_embedded.AutoSize = true;
			this.selectLauncherType_embedded.Location = new System.Drawing.Point(3, 28);
			this.selectLauncherType_embedded.Name = "selectLauncherType_embedded";
			this.selectLauncherType_embedded.Size = new System.Drawing.Size(181, 19);
			this.selectLauncherType_embedded.TabIndex = 3;
			this.selectLauncherType_embedded.TabStop = true;
			this.selectLauncherType_embedded.Text = "#LauncherType.Embedded";
			this.selectLauncherType_embedded.UseVisualStyleBackColor = true;
			this.selectLauncherType_embedded.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// commandLauncherOptionDirPath
			// 
			this.commandLauncherOptionDirPath.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandLauncherOptionDirPath.Location = new System.Drawing.Point(512, 125);
			this.commandLauncherOptionDirPath.Name = "commandLauncherOptionDirPath";
			this.commandLauncherOptionDirPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherOptionDirPath.TabIndex = 7;
			this.commandLauncherOptionDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherOptionDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherOptionDirPath.Click += new System.EventHandler(this.CommandLauncherOptionDirPath_Click);
			// 
			// inputLauncherOption
			// 
			this.inputLauncherOption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputLauncherOption.Location = new System.Drawing.Point(165, 125);
			this.inputLauncherOption.Name = "inputLauncherOption";
			this.inputLauncherOption.Size = new System.Drawing.Size(297, 23);
			this.inputLauncherOption.TabIndex = 5;
			this.inputLauncherOption.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// commandLauncherDirPath
			// 
			this.commandLauncherDirPath.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandLauncherDirPath.Location = new System.Drawing.Point(512, 94);
			this.commandLauncherDirPath.Name = "commandLauncherDirPath";
			this.commandLauncherDirPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherDirPath.TabIndex = 4;
			this.commandLauncherDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherDirPath.Click += new System.EventHandler(this.CommandLauncherDirPath_Click);
			// 
			// commandLauncherFilePath
			// 
			this.commandLauncherFilePath.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandLauncherFilePath.Location = new System.Drawing.Point(468, 94);
			this.commandLauncherFilePath.Name = "commandLauncherFilePath";
			this.commandLauncherFilePath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherFilePath.TabIndex = 3;
			this.commandLauncherFilePath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherFilePath.UseVisualStyleBackColor = true;
			this.commandLauncherFilePath.Click += new System.EventHandler(this.CommandLauncherFilePath_Click);
			// 
			// inputLauncherName
			// 
			this.tableLayoutPanel4.SetColumnSpan(this.inputLauncherName, 3);
			this.inputLauncherName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputLauncherName.Location = new System.Drawing.Point(165, 65);
			this.inputLauncherName.Name = "inputLauncherName";
			this.inputLauncherName.Size = new System.Drawing.Size(385, 23);
			this.inputLauncherName.TabIndex = 1;
			this.inputLauncherName.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// commandLauncherOptionFilePath
			// 
			this.commandLauncherOptionFilePath.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandLauncherOptionFilePath.Location = new System.Drawing.Point(468, 125);
			this.commandLauncherOptionFilePath.Name = "commandLauncherOptionFilePath";
			this.commandLauncherOptionFilePath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherOptionFilePath.TabIndex = 6;
			this.commandLauncherOptionFilePath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherOptionFilePath.UseVisualStyleBackColor = true;
			this.commandLauncherOptionFilePath.Click += new System.EventHandler(this.CommandLauncherOptionFilePath_Click);
			// 
			// labelLauncherName
			// 
			this.labelLauncherName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelLauncherName.AutoSize = true;
			this.labelLauncherName.Location = new System.Drawing.Point(3, 69);
			this.labelLauncherName.Name = "labelLauncherName";
			this.labelLauncherName.Size = new System.Drawing.Size(156, 15);
			this.labelLauncherName.TabIndex = 4;
			this.labelLauncherName.Text = ":setting/label/item-name";
			// 
			// labelLauncherCommand
			// 
			this.labelLauncherCommand.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelLauncherCommand.AutoSize = true;
			this.labelLauncherCommand.Location = new System.Drawing.Point(3, 99);
			this.labelLauncherCommand.Name = "labelLauncherCommand";
			this.labelLauncherCommand.Size = new System.Drawing.Size(149, 15);
			this.labelLauncherCommand.TabIndex = 4;
			this.labelLauncherCommand.Text = ":setting/label/command";
			// 
			// commandLauncherWorkDirPath
			// 
			this.commandLauncherWorkDirPath.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandLauncherWorkDirPath.Location = new System.Drawing.Point(468, 156);
			this.commandLauncherWorkDirPath.Name = "commandLauncherWorkDirPath";
			this.commandLauncherWorkDirPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherWorkDirPath.TabIndex = 9;
			this.commandLauncherWorkDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherWorkDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherWorkDirPath.Click += new System.EventHandler(this.CommandLauncherWorkDirPath_Click);
			// 
			// labelLauncherOption
			// 
			this.labelLauncherOption.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelLauncherOption.AutoSize = true;
			this.labelLauncherOption.Location = new System.Drawing.Point(3, 130);
			this.labelLauncherOption.Name = "labelLauncherOption";
			this.labelLauncherOption.Size = new System.Drawing.Size(127, 15);
			this.labelLauncherOption.TabIndex = 10;
			this.labelLauncherOption.Text = ":setting/label/option";
			// 
			// commandLauncherIconPath
			// 
			this.commandLauncherIconPath.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.commandLauncherIconPath.Location = new System.Drawing.Point(468, 187);
			this.commandLauncherIconPath.Name = "commandLauncherIconPath";
			this.commandLauncherIconPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherIconPath.TabIndex = 11;
			this.commandLauncherIconPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherIconPath.UseVisualStyleBackColor = true;
			this.commandLauncherIconPath.Click += new System.EventHandler(this.CommandLauncherIconPath_Click);
			// 
			// labelLauncherWorkDirPath
			// 
			this.labelLauncherWorkDirPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelLauncherWorkDirPath.AutoSize = true;
			this.labelLauncherWorkDirPath.Location = new System.Drawing.Point(3, 161);
			this.labelLauncherWorkDirPath.Name = "labelLauncherWorkDirPath";
			this.labelLauncherWorkDirPath.Size = new System.Drawing.Size(139, 15);
			this.labelLauncherWorkDirPath.TabIndex = 4;
			this.labelLauncherWorkDirPath.Text = ":setting/label/work-dir";
			// 
			// inputLauncherIconPath
			// 
			this.inputLauncherIconPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputLauncherIconPath.IconIndex = 0;
			this.inputLauncherIconPath.Location = new System.Drawing.Point(165, 187);
			this.inputLauncherIconPath.Name = "inputLauncherIconPath";
			this.inputLauncherIconPath.Size = new System.Drawing.Size(297, 23);
			this.inputLauncherIconPath.TabIndex = 10;
			this.inputLauncherIconPath.IconIndexChanged += new System.EventHandler(this.inputLauncherIconPath_IconIndexChanged);
			this.inputLauncherIconPath.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherWorkDirPath
			// 
			this.inputLauncherWorkDirPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputLauncherWorkDirPath.Location = new System.Drawing.Point(165, 156);
			this.inputLauncherWorkDirPath.Name = "inputLauncherWorkDirPath";
			this.inputLauncherWorkDirPath.Size = new System.Drawing.Size(297, 23);
			this.inputLauncherWorkDirPath.TabIndex = 8;
			this.inputLauncherWorkDirPath.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// labelLauncherIconPath
			// 
			this.labelLauncherIconPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelLauncherIconPath.AutoSize = true;
			this.labelLauncherIconPath.Location = new System.Drawing.Point(3, 192);
			this.labelLauncherIconPath.Name = "labelLauncherIconPath";
			this.labelLauncherIconPath.Size = new System.Drawing.Size(145, 15);
			this.labelLauncherIconPath.TabIndex = 4;
			this.labelLauncherIconPath.Text = ":setting/label/icon-path";
			// 
			// inputLauncherCommand
			// 
			this.inputLauncherCommand.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputLauncherCommand.Location = new System.Drawing.Point(165, 94);
			this.inputLauncherCommand.Name = "inputLauncherCommand";
			this.inputLauncherCommand.Size = new System.Drawing.Size(297, 23);
			this.inputLauncherCommand.TabIndex = 2;
			this.inputLauncherCommand.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// tabLauncher_pageEnv
			// 
			this.tabLauncher_pageEnv.Controls.Add(this.panelLauncherEnv);
			this.tabLauncher_pageEnv.Location = new System.Drawing.Point(4, 24);
			this.tabLauncher_pageEnv.Name = "tabLauncher_pageEnv";
			this.tabLauncher_pageEnv.Padding = new System.Windows.Forms.Padding(3);
			this.tabLauncher_pageEnv.Size = new System.Drawing.Size(559, 277);
			this.tabLauncher_pageEnv.TabIndex = 1;
			this.tabLauncher_pageEnv.Text = ":common/page/env";
			this.tabLauncher_pageEnv.UseVisualStyleBackColor = true;
			// 
			// panelLauncherEnv
			// 
			this.panelLauncherEnv.ColumnCount = 2;
			this.panelLauncherEnv.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this.panelLauncherEnv.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.panelLauncherEnv.Controls.Add(this.envLauncherRemove, 1, 1);
			this.panelLauncherEnv.Controls.Add(this.envLauncherUpdate, 0, 1);
			this.panelLauncherEnv.Controls.Add(this.selectLauncherEnv, 0, 0);
			this.panelLauncherEnv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelLauncherEnv.Location = new System.Drawing.Point(3, 3);
			this.panelLauncherEnv.Name = "panelLauncherEnv";
			this.panelLauncherEnv.RowCount = 2;
			this.panelLauncherEnv.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.panelLauncherEnv.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelLauncherEnv.Size = new System.Drawing.Size(553, 271);
			this.panelLauncherEnv.TabIndex = 19;
			// 
			// envLauncherRemove
			// 
			this.envLauncherRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envLauncherRemove.Location = new System.Drawing.Point(390, 32);
			this.envLauncherRemove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.envLauncherRemove.Name = "envLauncherRemove";
			this.envLauncherRemove.Size = new System.Drawing.Size(160, 267);
			this.envLauncherRemove.TabIndex = 0;
			this.envLauncherRemove.ValueChanged += new System.EventHandler<System.EventArgs>(this.EnvLauncherRemove_ValueChanged);
			// 
			// envLauncherUpdate
			// 
			this.envLauncherUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envLauncherUpdate.Location = new System.Drawing.Point(3, 32);
			this.envLauncherUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.envLauncherUpdate.Name = "envLauncherUpdate";
			this.envLauncherUpdate.Size = new System.Drawing.Size(381, 267);
			this.envLauncherUpdate.TabIndex = 0;
			this.envLauncherUpdate.ValueChanged += new System.EventHandler<System.EventArgs>(this.EnvLauncherUpdate_ValueChanged);
			// 
			// selectLauncherEnv
			// 
			this.selectLauncherEnv.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.selectLauncherEnv.AutoSize = true;
			this.panelLauncherEnv.SetColumnSpan(this.selectLauncherEnv, 2);
			this.selectLauncherEnv.Location = new System.Drawing.Point(3, 5);
			this.selectLauncherEnv.Name = "selectLauncherEnv";
			this.selectLauncherEnv.Size = new System.Drawing.Size(164, 19);
			this.selectLauncherEnv.TabIndex = 0;
			this.selectLauncherEnv.Text = ":setting/check/edit-env";
			this.selectLauncherEnv.UseVisualStyleBackColor = true;
			this.selectLauncherEnv.CheckedChanged += new System.EventHandler(this.selectLauncherEnv_CheckedChanged);
			// 
			// tabLauncher_pageOthers
			// 
			this.tabLauncher_pageOthers.Controls.Add(this.panelLauncherOthers);
			this.tabLauncher_pageOthers.Location = new System.Drawing.Point(4, 24);
			this.tabLauncher_pageOthers.Name = "tabLauncher_pageOthers";
			this.tabLauncher_pageOthers.Size = new System.Drawing.Size(559, 277);
			this.tabLauncher_pageOthers.TabIndex = 2;
			this.tabLauncher_pageOthers.Text = ":setting/page/launcher/others";
			this.tabLauncher_pageOthers.UseVisualStyleBackColor = true;
			// 
			// panelLauncherOthers
			// 
			this.panelLauncherOthers.ColumnCount = 2;
			this.panelLauncherOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.panelLauncherOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelLauncherOthers.Controls.Add(this.selectLauncherStdStream, 0, 0);
			this.panelLauncherOthers.Controls.Add(this.inputLauncherNote, 1, 3);
			this.panelLauncherOthers.Controls.Add(this.inputLauncherTag, 1, 2);
			this.panelLauncherOthers.Controls.Add(this.labelLauncherTag, 0, 2);
			this.panelLauncherOthers.Controls.Add(this.selectLauncherAdmin, 0, 1);
			this.panelLauncherOthers.Controls.Add(this.labelLauncherNote, 0, 3);
			this.panelLauncherOthers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelLauncherOthers.Location = new System.Drawing.Point(0, 0);
			this.panelLauncherOthers.Name = "panelLauncherOthers";
			this.panelLauncherOthers.RowCount = 5;
			this.panelLauncherOthers.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelLauncherOthers.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelLauncherOthers.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelLauncherOthers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelLauncherOthers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.panelLauncherOthers.Size = new System.Drawing.Size(559, 277);
			this.panelLauncherOthers.TabIndex = 0;
			// 
			// selectLauncherStdStream
			// 
			this.selectLauncherStdStream.AutoSize = true;
			this.panelLauncherOthers.SetColumnSpan(this.selectLauncherStdStream, 2);
			this.selectLauncherStdStream.Location = new System.Drawing.Point(3, 3);
			this.selectLauncherStdStream.Name = "selectLauncherStdStream";
			this.selectLauncherStdStream.Size = new System.Drawing.Size(181, 19);
			this.selectLauncherStdStream.TabIndex = 0;
			this.selectLauncherStdStream.Text = ":setting/check/std-stream";
			this.selectLauncherStdStream.UseVisualStyleBackColor = true;
			this.selectLauncherStdStream.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// inputLauncherNote
			// 
			this.inputLauncherNote.AcceptsReturn = true;
			this.inputLauncherNote.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputLauncherNote.Location = new System.Drawing.Point(126, 82);
			this.inputLauncherNote.Multiline = true;
			this.inputLauncherNote.Name = "inputLauncherNote";
			this.inputLauncherNote.Size = new System.Drawing.Size(430, 172);
			this.inputLauncherNote.TabIndex = 3;
			this.inputLauncherNote.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherTag
			// 
			this.inputLauncherTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherTag.Location = new System.Drawing.Point(126, 53);
			this.inputLauncherTag.Name = "inputLauncherTag";
			this.inputLauncherTag.Size = new System.Drawing.Size(430, 23);
			this.inputLauncherTag.TabIndex = 2;
			this.inputLauncherTag.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// labelLauncherTag
			// 
			this.labelLauncherTag.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelLauncherTag.AutoSize = true;
			this.labelLauncherTag.Location = new System.Drawing.Point(3, 57);
			this.labelLauncherTag.Name = "labelLauncherTag";
			this.labelLauncherTag.Size = new System.Drawing.Size(116, 15);
			this.labelLauncherTag.TabIndex = 4;
			this.labelLauncherTag.Text = ":setting/label/tags";
			// 
			// selectLauncherAdmin
			// 
			this.selectLauncherAdmin.AutoSize = true;
			this.panelLauncherOthers.SetColumnSpan(this.selectLauncherAdmin, 2);
			this.selectLauncherAdmin.Location = new System.Drawing.Point(3, 28);
			this.selectLauncherAdmin.Name = "selectLauncherAdmin";
			this.selectLauncherAdmin.Size = new System.Drawing.Size(163, 19);
			this.selectLauncherAdmin.TabIndex = 1;
			this.selectLauncherAdmin.Text = ":common/check/admin";
			this.selectLauncherAdmin.UseVisualStyleBackColor = true;
			this.selectLauncherAdmin.CheckedChanged += new System.EventHandler(this.InputLauncherIconIndex_ValueChanged);
			// 
			// labelLauncherNote
			// 
			this.labelLauncherNote.AutoSize = true;
			this.labelLauncherNote.Location = new System.Drawing.Point(3, 82);
			this.labelLauncherNote.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.labelLauncherNote.Name = "labelLauncherNote";
			this.labelLauncherNote.Size = new System.Drawing.Size(117, 15);
			this.labelLauncherNote.TabIndex = 6;
			this.labelLauncherNote.Text = ":setting/label/note";
			// 
			// tabSetting_pageToolbar
			// 
			this.tabSetting_pageToolbar.Controls.Add(this.splitContainer3);
			this.tabSetting_pageToolbar.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageToolbar.Name = "tabSetting_pageToolbar";
			this.tabSetting_pageToolbar.Size = new System.Drawing.Size(770, 283);
			this.tabSetting_pageToolbar.TabIndex = 3;
			this.tabSetting_pageToolbar.Text = ":setting/page/toolbar";
			this.tabSetting_pageToolbar.UseVisualStyleBackColor = true;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer3.IsSplitterFixed = true;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.groupToolbar);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer3.Size = new System.Drawing.Size(770, 283);
			this.splitContainer3.SplitterDistance = 270;
			this.splitContainer3.TabIndex = 19;
			// 
			// groupToolbar
			// 
			this.groupToolbar.Controls.Add(this.commandToolbarScreens);
			this.groupToolbar.Controls.Add(this.panel1);
			this.groupToolbar.Controls.Add(this.selectToolbarItem);
			this.groupToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupToolbar.Location = new System.Drawing.Point(0, 0);
			this.groupToolbar.Name = "groupToolbar";
			this.groupToolbar.Size = new System.Drawing.Size(270, 283);
			this.groupToolbar.TabIndex = 0;
			this.groupToolbar.TabStop = false;
			this.groupToolbar.Text = "☃";
			// 
			// commandToolbarScreens
			// 
			this.commandToolbarScreens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.commandToolbarScreens.Location = new System.Drawing.Point(193, 0);
			this.commandToolbarScreens.Name = "commandToolbarScreens";
			this.commandToolbarScreens.Size = new System.Drawing.Size(71, 23);
			this.commandToolbarScreens.TabIndex = 22;
			this.commandToolbarScreens.Text = ":setting/command/screens";
			this.commandToolbarScreens.UseVisualStyleBackColor = true;
			this.commandToolbarScreens.Click += new System.EventHandler(this.commandToolbarScreens_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tableLayoutPanel2);
			this.panel1.Location = new System.Drawing.Point(6, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(235, 235);
			this.panel1.TabIndex = 21;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.selectToolbarVisible, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.selectToolbarTopmost, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.labelToolbarTextWidth, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.inputToolbarTextWidth, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.labelToolbarFont, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.commandToolbarFont, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.selectToolbarIcon, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.labelToolbarIcon, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.labelToolbarPosition, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.selectToolbarPosition, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.labelToolbarGroup, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.selectToolbarGroup, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.selectToolbarAutoHide, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.selectToolbarShowText, 0, 5);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 10;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(235, 252);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// selectToolbarVisible
			// 
			this.selectToolbarVisible.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.selectToolbarVisible, 2);
			this.selectToolbarVisible.Location = new System.Drawing.Point(3, 225);
			this.selectToolbarVisible.Name = "selectToolbarVisible";
			this.selectToolbarVisible.Size = new System.Drawing.Size(157, 19);
			this.selectToolbarVisible.TabIndex = 8;
			this.selectToolbarVisible.Text = ":common/label/visible";
			this.selectToolbarVisible.UseVisualStyleBackColor = true;
			// 
			// selectToolbarTopmost
			// 
			this.selectToolbarTopmost.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.selectToolbarTopmost.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.selectToolbarTopmost, 2);
			this.selectToolbarTopmost.Location = new System.Drawing.Point(3, 200);
			this.selectToolbarTopmost.Name = "selectToolbarTopmost";
			this.selectToolbarTopmost.Size = new System.Drawing.Size(170, 19);
			this.selectToolbarTopmost.TabIndex = 7;
			this.selectToolbarTopmost.Text = ":common/label/topmost";
			this.selectToolbarTopmost.UseVisualStyleBackColor = true;
			// 
			// labelToolbarTextWidth
			// 
			this.labelToolbarTextWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelToolbarTextWidth.AutoSize = true;
			this.labelToolbarTextWidth.Location = new System.Drawing.Point(3, 125);
			this.labelToolbarTextWidth.Name = "labelToolbarTextWidth";
			this.labelToolbarTextWidth.Size = new System.Drawing.Size(152, 15);
			this.labelToolbarTextWidth.TabIndex = 7;
			this.labelToolbarTextWidth.Text = ":setting/label/text-width";
			// 
			// inputToolbarTextWidth
			// 
			this.inputToolbarTextWidth.Location = new System.Drawing.Point(161, 121);
			this.inputToolbarTextWidth.Name = "inputToolbarTextWidth";
			this.inputToolbarTextWidth.Size = new System.Drawing.Size(142, 23);
			this.inputToolbarTextWidth.TabIndex = 4;
			this.inputToolbarTextWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelToolbarFont
			// 
			this.labelToolbarFont.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelToolbarFont.AutoSize = true;
			this.labelToolbarFont.Location = new System.Drawing.Point(3, 95);
			this.labelToolbarFont.Name = "labelToolbarFont";
			this.labelToolbarFont.Size = new System.Drawing.Size(125, 15);
			this.labelToolbarFont.TabIndex = 5;
			this.labelToolbarFont.Text = ":common/label/font";
			// 
			// commandToolbarFont
			// 
			this.commandToolbarFont.AutoSize = true;
			this.commandToolbarFont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.commandToolbarFont.Location = new System.Drawing.Point(161, 90);
			this.commandToolbarFont.Name = "commandToolbarFont";
			this.commandToolbarFont.Size = new System.Drawing.Size(142, 25);
			this.commandToolbarFont.TabIndex = 3;
			this.commandToolbarFont.Text = "{FAMILY} {PT} ...";
			this.commandToolbarFont.UseVisualStyleBackColor = true;
			// 
			// selectToolbarIcon
			// 
			this.selectToolbarIcon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectToolbarIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarIcon.FormattingEnabled = true;
			this.selectToolbarIcon.Location = new System.Drawing.Point(161, 61);
			this.selectToolbarIcon.Name = "selectToolbarIcon";
			this.selectToolbarIcon.Size = new System.Drawing.Size(142, 23);
			this.selectToolbarIcon.TabIndex = 2;
			// 
			// labelToolbarIcon
			// 
			this.labelToolbarIcon.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelToolbarIcon.AutoSize = true;
			this.labelToolbarIcon.Location = new System.Drawing.Point(3, 65);
			this.labelToolbarIcon.Name = "labelToolbarIcon";
			this.labelToolbarIcon.Size = new System.Drawing.Size(101, 15);
			this.labelToolbarIcon.TabIndex = 3;
			this.labelToolbarIcon.Text = ":enum/icon-size";
			// 
			// labelToolbarPosition
			// 
			this.labelToolbarPosition.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelToolbarPosition.AutoSize = true;
			this.labelToolbarPosition.Location = new System.Drawing.Point(3, 36);
			this.labelToolbarPosition.Name = "labelToolbarPosition";
			this.labelToolbarPosition.Size = new System.Drawing.Size(142, 15);
			this.labelToolbarPosition.TabIndex = 1;
			this.labelToolbarPosition.Text = ":enum/toolbar-position";
			// 
			// selectToolbarPosition
			// 
			this.selectToolbarPosition.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectToolbarPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarPosition.FormattingEnabled = true;
			this.selectToolbarPosition.Location = new System.Drawing.Point(161, 32);
			this.selectToolbarPosition.Name = "selectToolbarPosition";
			this.selectToolbarPosition.Size = new System.Drawing.Size(142, 23);
			this.selectToolbarPosition.TabIndex = 1;
			// 
			// labelToolbarGroup
			// 
			this.labelToolbarGroup.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelToolbarGroup.AutoSize = true;
			this.labelToolbarGroup.Location = new System.Drawing.Point(3, 7);
			this.labelToolbarGroup.Name = "labelToolbarGroup";
			this.labelToolbarGroup.Size = new System.Drawing.Size(124, 15);
			this.labelToolbarGroup.TabIndex = 13;
			this.labelToolbarGroup.Text = ":setting/label/group";
			// 
			// selectToolbarGroup
			// 
			this.selectToolbarGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectToolbarGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarGroup.FormattingEnabled = true;
			this.selectToolbarGroup.Location = new System.Drawing.Point(161, 3);
			this.selectToolbarGroup.Name = "selectToolbarGroup";
			this.selectToolbarGroup.Size = new System.Drawing.Size(142, 23);
			this.selectToolbarGroup.TabIndex = 0;
			// 
			// selectToolbarAutoHide
			// 
			this.selectToolbarAutoHide.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.selectToolbarAutoHide.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.selectToolbarAutoHide, 2);
			this.selectToolbarAutoHide.Location = new System.Drawing.Point(3, 175);
			this.selectToolbarAutoHide.Name = "selectToolbarAutoHide";
			this.selectToolbarAutoHide.Size = new System.Drawing.Size(171, 19);
			this.selectToolbarAutoHide.TabIndex = 6;
			this.selectToolbarAutoHide.Text = ":setting/check/auto-hide";
			this.selectToolbarAutoHide.UseVisualStyleBackColor = true;
			// 
			// selectToolbarShowText
			// 
			this.selectToolbarShowText.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.selectToolbarShowText.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.selectToolbarShowText, 2);
			this.selectToolbarShowText.Location = new System.Drawing.Point(3, 150);
			this.selectToolbarShowText.Name = "selectToolbarShowText";
			this.selectToolbarShowText.Size = new System.Drawing.Size(175, 19);
			this.selectToolbarShowText.TabIndex = 5;
			this.selectToolbarShowText.Text = ":setting/check/show-text";
			this.selectToolbarShowText.UseVisualStyleBackColor = true;
			// 
			// selectToolbarItem
			// 
			this.selectToolbarItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarItem.FormattingEnabled = true;
			this.selectToolbarItem.Location = new System.Drawing.Point(6, 0);
			this.selectToolbarItem.Name = "selectToolbarItem";
			this.selectToolbarItem.Size = new System.Drawing.Size(186, 23);
			this.selectToolbarItem.TabIndex = 0;
			this.selectToolbarItem.SelectedValueChanged += new System.EventHandler(this.SelectToolbarItem_SelectedValueChanged);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.toolStripContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.selecterToolbar);
			this.splitContainer2.Size = new System.Drawing.Size(496, 283);
			this.splitContainer2.SplitterDistance = 291;
			this.splitContainer2.TabIndex = 15;
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.treeToolbarItemGroup);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(291, 258);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(291, 283);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolToolbarGroup);
			// 
			// treeToolbarItemGroup
			// 
			this.treeToolbarItemGroup.AllowDrop = true;
			this.treeToolbarItemGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeToolbarItemGroup.HideSelection = false;
			this.treeToolbarItemGroup.LabelEdit = true;
			this.treeToolbarItemGroup.Location = new System.Drawing.Point(0, 0);
			this.treeToolbarItemGroup.Name = "treeToolbarItemGroup";
			this.treeToolbarItemGroup.ShowPlusMinus = false;
			this.treeToolbarItemGroup.Size = new System.Drawing.Size(291, 258);
			this.treeToolbarItemGroup.TabIndex = 0;
			this.treeToolbarItemGroup.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeToolbarItemGroup_BeforeLabelEdit);
			this.treeToolbarItemGroup.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeToolbarItemGroup_AfterLabelEdit);
			this.treeToolbarItemGroup.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeToolbarItemGroup_ItemDrag);
			this.treeToolbarItemGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeToolbarItemGroup_AfterSelect);
			this.treeToolbarItemGroup.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeToolbarItemGroup_DragDrop);
			this.treeToolbarItemGroup.DragOver += new System.Windows.Forms.DragEventHandler(this.treeToolbarItemGroup_DragOver);
			this.treeToolbarItemGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeToolbarItemGroup_KeyDown);
			// 
			// toolToolbarGroup
			// 
			this.toolToolbarGroup.Dock = System.Windows.Forms.DockStyle.None;
			this.toolToolbarGroup.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolToolbarGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolToolbarGroup_addGroup,
            this.toolToolbarGroup_addItem,
            this.DisableCloseToolStripSeparator1,
            this.toolToolbarGroup_up,
            this.toolToolbarGroup_down,
            this.DisableCloseToolStripSeparator2,
            this.toolToolbarGroup_remove});
			this.toolToolbarGroup.Location = new System.Drawing.Point(0, 0);
			this.toolToolbarGroup.Name = "toolToolbarGroup";
			this.toolToolbarGroup.Size = new System.Drawing.Size(291, 25);
			this.toolToolbarGroup.Stretch = true;
			this.toolToolbarGroup.TabIndex = 0;
			this.toolToolbarGroup.Text = "toolStrip1";
			// 
			// toolToolbarGroup_addGroup
			// 
			this.toolToolbarGroup_addGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_addGroup.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolToolbarGroup_addGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_addGroup.Name = "toolToolbarGroup_addGroup";
			this.toolToolbarGroup_addGroup.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_addGroup.ToolTipText = ":setting/tips/add-group";
			this.toolToolbarGroup_addGroup.Click += new System.EventHandler(this.ToolToolbarGroup_addGroup_Click);
			// 
			// toolToolbarGroup_addItem
			// 
			this.toolToolbarGroup_addItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_addItem.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolToolbarGroup_addItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_addItem.Name = "toolToolbarGroup_addItem";
			this.toolToolbarGroup_addItem.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_addItem.ToolTipText = ":setting/tips/add-item";
			this.toolToolbarGroup_addItem.Click += new System.EventHandler(this.ToolToolbarGroup_addItem_Click);
			// 
			// DisableCloseToolStripSeparator1
			// 
			this.DisableCloseToolStripSeparator1.Name = "DisableCloseToolStripSeparator1";
			this.DisableCloseToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolToolbarGroup_up
			// 
			this.toolToolbarGroup_up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_up.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolToolbarGroup_up.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_up.Name = "toolToolbarGroup_up";
			this.toolToolbarGroup_up.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_up.ToolTipText = ":setting/tips/up-item";
			this.toolToolbarGroup_up.Click += new System.EventHandler(this.ToolToolbarGroup_up_Click);
			// 
			// toolToolbarGroup_down
			// 
			this.toolToolbarGroup_down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_down.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolToolbarGroup_down.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_down.Name = "toolToolbarGroup_down";
			this.toolToolbarGroup_down.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_down.ToolTipText = ":setting/tips/down-item";
			this.toolToolbarGroup_down.Click += new System.EventHandler(this.ToolToolbarGroup_down_Click);
			// 
			// DisableCloseToolStripSeparator2
			// 
			this.DisableCloseToolStripSeparator2.Name = "DisableCloseToolStripSeparator2";
			this.DisableCloseToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolToolbarGroup_remove
			// 
			this.toolToolbarGroup_remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_remove.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ReplaceSkin;
			this.toolToolbarGroup_remove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_remove.Name = "toolToolbarGroup_remove";
			this.toolToolbarGroup_remove.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_remove.ToolTipText = ":setting/tips/remove-item";
			this.toolToolbarGroup_remove.Click += new System.EventHandler(this.ToolToolbarGroup_remove_Click);
			// 
			// selecterToolbar
			// 
			this.selecterToolbar.ApplicationSetting = null;
			this.selecterToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selecterToolbar.Filtering = false;
			this.selecterToolbar.FilterType = ContentTypeTextNet.Pe.PeMain.UI.CustomControl.LauncherItemSelecterType.Full;
			this.selecterToolbar.IconScale = ContentTypeTextNet.Pe.Library.Skin.IconScale.Small;
			this.selecterToolbar.ItemEdit = false;
			this.selecterToolbar.Location = new System.Drawing.Point(0, 0);
			this.selecterToolbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterToolbar.Name = "selecterToolbar";
			this.selecterToolbar.SelectedItem = null;
			this.selecterToolbar.Size = new System.Drawing.Size(201, 283);
			this.selecterToolbar.TabIndex = 0;
			this.selecterToolbar.SelectItemChanged += new System.EventHandler<ContentTypeTextNet.Pe.PeMain.UI.CustomControl.SelectedItemEventArg>(this.SelecterToolbar_SelectChangedItem);
			this.selecterToolbar.ListDoubleClick += new System.EventHandler<ContentTypeTextNet.Pe.PeMain.UI.CustomControl.LauncherItemSelecterEventArgs>(this.selecterToolbar_ListDoubleClick);
			// 
			// tabSetting_pageCommand
			// 
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandHotkey);
			this.tabSetting_pageCommand.Controls.Add(this.inputCommandHotkey);
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandIcon);
			this.tabSetting_pageCommand.Controls.Add(this.selectCommandIcon);
			this.tabSetting_pageCommand.Controls.Add(this.inputCommandHideTime);
			this.tabSetting_pageCommand.Controls.Add(this.selectCommandTopmost);
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandHideTime);
			this.tabSetting_pageCommand.Controls.Add(this.commandCommandFont);
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandFont);
			this.tabSetting_pageCommand.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageCommand.Name = "tabSetting_pageCommand";
			this.tabSetting_pageCommand.Padding = new System.Windows.Forms.Padding(3);
			this.tabSetting_pageCommand.Size = new System.Drawing.Size(770, 283);
			this.tabSetting_pageCommand.TabIndex = 2;
			this.tabSetting_pageCommand.Text = ":setting/page/command";
			this.tabSetting_pageCommand.UseVisualStyleBackColor = true;
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
			this.inputCommandHotkey.HotKeySetting = null;
			this.inputCommandHotkey.Location = new System.Drawing.Point(156, 105);
			this.inputCommandHotkey.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputCommandHotkey.Name = "inputCommandHotkey";
			this.inputCommandHotkey.ReadOnly = true;
			this.inputCommandHotkey.Registered = false;
			this.inputCommandHotkey.Size = new System.Drawing.Size(252, 23);
			this.inputCommandHotkey.TabIndex = 15;
			this.inputCommandHotkey.Text = "None";
			// 
			// labelCommandIcon
			// 
			this.labelCommandIcon.Location = new System.Drawing.Point(40, 187);
			this.labelCommandIcon.Name = "labelCommandIcon";
			this.labelCommandIcon.Size = new System.Drawing.Size(100, 23);
			this.labelCommandIcon.TabIndex = 14;
			this.labelCommandIcon.Text = ":enum/icon-size";
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
			this.selectCommandTopmost.AutoSize = true;
			this.selectCommandTopmost.Location = new System.Drawing.Point(36, 160);
			this.selectCommandTopmost.Name = "selectCommandTopmost";
			this.selectCommandTopmost.Size = new System.Drawing.Size(175, 19);
			this.selectCommandTopmost.TabIndex = 4;
			this.selectCommandTopmost.Text = "::common/label/topmost";
			this.selectCommandTopmost.UseVisualStyleBackColor = true;
			// 
			// labelCommandHideTime
			// 
			this.labelCommandHideTime.Location = new System.Drawing.Point(35, 78);
			this.labelCommandHideTime.Name = "labelCommandHideTime";
			this.labelCommandHideTime.Size = new System.Drawing.Size(100, 23);
			this.labelCommandHideTime.TabIndex = 2;
			this.labelCommandHideTime.Text = "{HIDE TIME}";
			// 
			// commandCommandFont
			// 
			this.commandCommandFont.AutoSize = true;
			this.commandCommandFont.Location = new System.Drawing.Point(146, 23);
			this.commandCommandFont.Name = "commandCommandFont";
			this.commandCommandFont.Size = new System.Drawing.Size(171, 25);
			this.commandCommandFont.TabIndex = 1;
			this.commandCommandFont.Text = "{FAMILY} {PT} ...";
			this.commandCommandFont.UseVisualStyleBackColor = true;
			// 
			// labelCommandFont
			// 
			this.labelCommandFont.Location = new System.Drawing.Point(35, 28);
			this.labelCommandFont.Name = "labelCommandFont";
			this.labelCommandFont.Size = new System.Drawing.Size(100, 23);
			this.labelCommandFont.TabIndex = 0;
			this.labelCommandFont.Text = ":common/label/font";
			// 
			// tabSetting_pageNote
			// 
			this.tabSetting_pageNote.Controls.Add(this.panelNote);
			this.tabSetting_pageNote.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageNote.Name = "tabSetting_pageNote";
			this.tabSetting_pageNote.Size = new System.Drawing.Size(770, 283);
			this.tabSetting_pageNote.TabIndex = 6;
			this.tabSetting_pageNote.Text = ":setting/page/note";
			this.tabSetting_pageNote.UseVisualStyleBackColor = true;
			// 
			// panelNote
			// 
			this.panelNote.ColumnCount = 2;
			this.panelNote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.panelNote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelNote.Controls.Add(this.panelNoteOthers, 0, 1);
			this.panelNote.Controls.Add(this.groupNoteKey, 0, 0);
			this.panelNote.Controls.Add(this.groupNoteItem, 1, 0);
			this.panelNote.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelNote.Location = new System.Drawing.Point(0, 0);
			this.panelNote.Name = "panelNote";
			this.panelNote.RowCount = 2;
			this.panelNote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelNote.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelNote.Size = new System.Drawing.Size(770, 283);
			this.panelNote.TabIndex = 14;
			// 
			// panelNoteOthers
			// 
			this.panelNoteOthers.ColumnCount = 2;
			this.panelNoteOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.panelNoteOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.panelNoteOthers.Controls.Add(this.commandNoteCaptionFont, 1, 1);
			this.panelNoteOthers.Controls.Add(this.labelNoteCaptionFont, 0, 1);
			this.panelNoteOthers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelNoteOthers.Location = new System.Drawing.Point(3, 230);
			this.panelNoteOthers.Name = "panelNoteOthers";
			this.panelNoteOthers.RowCount = 3;
			this.panelNoteOthers.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelNoteOthers.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelNoteOthers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelNoteOthers.Size = new System.Drawing.Size(229, 50);
			this.panelNoteOthers.TabIndex = 12;
			// 
			// commandNoteCaptionFont
			// 
			this.commandNoteCaptionFont.AutoSize = true;
			this.commandNoteCaptionFont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.commandNoteCaptionFont.Location = new System.Drawing.Point(134, 3);
			this.commandNoteCaptionFont.Name = "commandNoteCaptionFont";
			this.commandNoteCaptionFont.Size = new System.Drawing.Size(141, 25);
			this.commandNoteCaptionFont.TabIndex = 0;
			this.commandNoteCaptionFont.Text = "{FAMILY} {PT} ...";
			this.commandNoteCaptionFont.UseVisualStyleBackColor = true;
			// 
			// labelNoteCaptionFont
			// 
			this.labelNoteCaptionFont.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelNoteCaptionFont.AutoSize = true;
			this.labelNoteCaptionFont.Location = new System.Drawing.Point(3, 8);
			this.labelNoteCaptionFont.Name = "labelNoteCaptionFont";
			this.labelNoteCaptionFont.Size = new System.Drawing.Size(125, 15);
			this.labelNoteCaptionFont.TabIndex = 9;
			this.labelNoteCaptionFont.Text = ":common/label/font";
			// 
			// groupNoteKey
			// 
			this.groupNoteKey.Controls.Add(this.panelNoteKey);
			this.groupNoteKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupNoteKey.Location = new System.Drawing.Point(3, 3);
			this.groupNoteKey.Name = "groupNoteKey";
			this.groupNoteKey.Size = new System.Drawing.Size(229, 221);
			this.groupNoteKey.TabIndex = 0;
			this.groupNoteKey.TabStop = false;
			this.groupNoteKey.Text = ":setting/group/key";
			// 
			// panelNoteKey
			// 
			this.panelNoteKey.Controls.Add(this.labelNoteCreate);
			this.panelNoteKey.Controls.Add(this.inputNoteCreate);
			this.panelNoteKey.Controls.Add(this.labelNoteHiddent);
			this.panelNoteKey.Controls.Add(this.inputNoteHidden);
			this.panelNoteKey.Controls.Add(this.labelNoteCompact);
			this.panelNoteKey.Controls.Add(this.inputNoteCompact);
			this.panelNoteKey.Controls.Add(this.labelNoteShowFront);
			this.panelNoteKey.Controls.Add(this.inputNoteShowFront);
			this.panelNoteKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelNoteKey.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.panelNoteKey.Location = new System.Drawing.Point(3, 19);
			this.panelNoteKey.Name = "panelNoteKey";
			this.panelNoteKey.Size = new System.Drawing.Size(223, 199);
			this.panelNoteKey.TabIndex = 8;
			// 
			// labelNoteCreate
			// 
			this.labelNoteCreate.AutoSize = true;
			this.labelNoteCreate.Location = new System.Drawing.Point(3, 0);
			this.labelNoteCreate.Name = "labelNoteCreate";
			this.labelNoteCreate.Size = new System.Drawing.Size(159, 15);
			this.labelNoteCreate.TabIndex = 1;
			this.labelNoteCreate.Text = ":setting/label/note-create";
			// 
			// inputNoteCreate
			// 
			this.inputNoteCreate.BackColor = System.Drawing.Color.White;
			this.inputNoteCreate.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputNoteCreate.Hotkey = System.Windows.Forms.Keys.None;
			this.inputNoteCreate.HotKeySetting = null;
			this.inputNoteCreate.Location = new System.Drawing.Point(3, 18);
			this.inputNoteCreate.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputNoteCreate.Name = "inputNoteCreate";
			this.inputNoteCreate.ReadOnly = true;
			this.inputNoteCreate.Registered = false;
			this.inputNoteCreate.Size = new System.Drawing.Size(211, 23);
			this.inputNoteCreate.TabIndex = 0;
			this.inputNoteCreate.Text = "None";
			// 
			// labelNoteHiddent
			// 
			this.labelNoteHiddent.AutoSize = true;
			this.labelNoteHiddent.Location = new System.Drawing.Point(3, 44);
			this.labelNoteHiddent.Name = "labelNoteHiddent";
			this.labelNoteHiddent.Size = new System.Drawing.Size(160, 15);
			this.labelNoteHiddent.TabIndex = 4;
			this.labelNoteHiddent.Text = ":setting/label/note-hidden";
			// 
			// inputNoteHidden
			// 
			this.inputNoteHidden.BackColor = System.Drawing.Color.White;
			this.inputNoteHidden.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputNoteHidden.Hotkey = System.Windows.Forms.Keys.None;
			this.inputNoteHidden.HotKeySetting = null;
			this.inputNoteHidden.Location = new System.Drawing.Point(3, 62);
			this.inputNoteHidden.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputNoteHidden.Name = "inputNoteHidden";
			this.inputNoteHidden.ReadOnly = true;
			this.inputNoteHidden.Registered = false;
			this.inputNoteHidden.Size = new System.Drawing.Size(211, 23);
			this.inputNoteHidden.TabIndex = 1;
			this.inputNoteHidden.Text = "None";
			// 
			// labelNoteCompact
			// 
			this.labelNoteCompact.AutoSize = true;
			this.labelNoteCompact.Location = new System.Drawing.Point(3, 88);
			this.labelNoteCompact.Name = "labelNoteCompact";
			this.labelNoteCompact.Size = new System.Drawing.Size(172, 15);
			this.labelNoteCompact.TabIndex = 4;
			this.labelNoteCompact.Text = ":setting/label/note-compact";
			// 
			// inputNoteCompact
			// 
			this.inputNoteCompact.BackColor = System.Drawing.Color.White;
			this.inputNoteCompact.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputNoteCompact.Hotkey = System.Windows.Forms.Keys.None;
			this.inputNoteCompact.HotKeySetting = null;
			this.inputNoteCompact.Location = new System.Drawing.Point(3, 106);
			this.inputNoteCompact.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputNoteCompact.Name = "inputNoteCompact";
			this.inputNoteCompact.ReadOnly = true;
			this.inputNoteCompact.Registered = false;
			this.inputNoteCompact.Size = new System.Drawing.Size(213, 23);
			this.inputNoteCompact.TabIndex = 2;
			this.inputNoteCompact.Text = "None";
			// 
			// labelNoteShowFront
			// 
			this.labelNoteShowFront.AutoSize = true;
			this.labelNoteShowFront.Location = new System.Drawing.Point(3, 132);
			this.labelNoteShowFront.Name = "labelNoteShowFront";
			this.labelNoteShowFront.Size = new System.Drawing.Size(185, 15);
			this.labelNoteShowFront.TabIndex = 5;
			this.labelNoteShowFront.Text = ":setting/label/note-show-front";
			// 
			// inputNoteShowFront
			// 
			this.inputNoteShowFront.BackColor = System.Drawing.Color.White;
			this.inputNoteShowFront.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputNoteShowFront.Hotkey = System.Windows.Forms.Keys.None;
			this.inputNoteShowFront.HotKeySetting = null;
			this.inputNoteShowFront.Location = new System.Drawing.Point(3, 150);
			this.inputNoteShowFront.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputNoteShowFront.Name = "inputNoteShowFront";
			this.inputNoteShowFront.ReadOnly = true;
			this.inputNoteShowFront.Registered = false;
			this.inputNoteShowFront.Size = new System.Drawing.Size(211, 23);
			this.inputNoteShowFront.TabIndex = 3;
			this.inputNoteShowFront.Text = "None";
			// 
			// groupNoteItem
			// 
			this.groupNoteItem.Controls.Add(this.gridNoteItems);
			this.groupNoteItem.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupNoteItem.Location = new System.Drawing.Point(238, 3);
			this.groupNoteItem.Name = "groupNoteItem";
			this.panelNote.SetRowSpan(this.groupNoteItem, 2);
			this.groupNoteItem.Size = new System.Drawing.Size(529, 277);
			this.groupNoteItem.TabIndex = 1;
			this.groupNoteItem.TabStop = false;
			this.groupNoteItem.Text = ":setting/group/item";
			// 
			// gridNoteItems
			// 
			this.gridNoteItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.gridNoteItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridNoteItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridNoteItems_columnRemove,
            this.gridNoteItems_columnId,
            this.gridNoteItems_columnVisible,
            this.gridNoteItems_columnLocked,
            this.gridNoteItems_columnTitle,
            this.gridNoteItems_columnBody,
            this.gridNoteItems_columnFont,
            this.gridNoteItems_columnFore,
            this.gridNoteItems_columnBack});
			this.gridNoteItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridNoteItems.Location = new System.Drawing.Point(3, 19);
			this.gridNoteItems.MultiSelect = false;
			this.gridNoteItems.Name = "gridNoteItems";
			this.gridNoteItems.RowTemplate.Height = 21;
			this.gridNoteItems.Size = new System.Drawing.Size(523, 255);
			this.gridNoteItems.TabIndex = 0;
			this.gridNoteItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridNoteItems_CellContentClick);
			this.gridNoteItems.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GridNoteItems_CellFormatting);
			// 
			// gridNoteItems_columnRemove
			// 
			this.gridNoteItems_columnRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gridNoteItems_columnRemove.FillWeight = 30F;
			this.gridNoteItems_columnRemove.HeaderText = ":setting/column/note/remove";
			this.gridNoteItems_columnRemove.Name = "gridNoteItems_columnRemove";
			this.gridNoteItems_columnRemove.Width = 189;
			// 
			// gridNoteItems_columnId
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.gridNoteItems_columnId.DefaultCellStyle = dataGridViewCellStyle1;
			this.gridNoteItems_columnId.FillWeight = 40F;
			this.gridNoteItems_columnId.HeaderText = ":setting/column/note/id";
			this.gridNoteItems_columnId.Name = "gridNoteItems_columnId";
			this.gridNoteItems_columnId.ReadOnly = true;
			this.gridNoteItems_columnId.Width = 40;
			// 
			// gridNoteItems_columnVisible
			// 
			this.gridNoteItems_columnVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gridNoteItems_columnVisible.FillWeight = 30F;
			this.gridNoteItems_columnVisible.HeaderText = ":setting/column/note/visible";
			this.gridNoteItems_columnVisible.Name = "gridNoteItems_columnVisible";
			this.gridNoteItems_columnVisible.Width = 180;
			// 
			// gridNoteItems_columnLocked
			// 
			this.gridNoteItems_columnLocked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gridNoteItems_columnLocked.HeaderText = ":setting/column/note/locked";
			this.gridNoteItems_columnLocked.Name = "gridNoteItems_columnLocked";
			this.gridNoteItems_columnLocked.Width = 181;
			// 
			// gridNoteItems_columnTitle
			// 
			this.gridNoteItems_columnTitle.HeaderText = ":setting/column/note/title";
			this.gridNoteItems_columnTitle.Name = "gridNoteItems_columnTitle";
			// 
			// gridNoteItems_columnBody
			// 
			this.gridNoteItems_columnBody.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridNoteItems_columnBody.DefaultCellStyle = dataGridViewCellStyle2;
			this.gridNoteItems_columnBody.HeaderText = ":setting/column/note/body";
			this.gridNoteItems_columnBody.MinimumWidth = 100;
			this.gridNoteItems_columnBody.Name = "gridNoteItems_columnBody";
			this.gridNoteItems_columnBody.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.gridNoteItems_columnBody.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// gridNoteItems_columnFont
			// 
			this.gridNoteItems_columnFont.FillWeight = 80F;
			this.gridNoteItems_columnFont.HeaderText = ":setting/column/note/font";
			this.gridNoteItems_columnFont.Name = "gridNoteItems_columnFont";
			this.gridNoteItems_columnFont.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.gridNoteItems_columnFont.Width = 80;
			// 
			// gridNoteItems_columnFore
			// 
			this.gridNoteItems_columnFore.FillWeight = 80F;
			this.gridNoteItems_columnFore.HeaderText = ":setting/column/note/fore";
			this.gridNoteItems_columnFore.Name = "gridNoteItems_columnFore";
			this.gridNoteItems_columnFore.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.gridNoteItems_columnFore.Width = 80;
			// 
			// gridNoteItems_columnBack
			// 
			this.gridNoteItems_columnBack.FillWeight = 80F;
			this.gridNoteItems_columnBack.HeaderText = ":setting/column/note/back";
			this.gridNoteItems_columnBack.Name = "gridNoteItems_columnBack";
			this.gridNoteItems_columnBack.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.gridNoteItems_columnBack.Width = 80;
			// 
			// tabSetting_pageDisplay
			// 
			this.tabSetting_pageDisplay.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageDisplay.Name = "tabSetting_pageDisplay";
			this.tabSetting_pageDisplay.Size = new System.Drawing.Size(770, 283);
			this.tabSetting_pageDisplay.TabIndex = 5;
			this.tabSetting_pageDisplay.Text = ":setting/page/display";
			this.tabSetting_pageDisplay.UseVisualStyleBackColor = true;
			// 
			// tabSetting_pageClipboard
			// 
			this.tabSetting_pageClipboard.Controls.Add(this.flowLayoutPanel9);
			this.tabSetting_pageClipboard.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageClipboard.Name = "tabSetting_pageClipboard";
			this.tabSetting_pageClipboard.Padding = new System.Windows.Forms.Padding(3);
			this.tabSetting_pageClipboard.Size = new System.Drawing.Size(770, 283);
			this.tabSetting_pageClipboard.TabIndex = 7;
			this.tabSetting_pageClipboard.Text = ":setting/page/clipboard";
			this.tabSetting_pageClipboard.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel9
			// 
			this.flowLayoutPanel9.AutoScroll = true;
			this.flowLayoutPanel9.Controls.Add(this.flowLayoutPanel7);
			this.flowLayoutPanel9.Controls.Add(this.flowLayoutPanel8);
			this.flowLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel9.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel9.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel9.Name = "flowLayoutPanel9";
			this.flowLayoutPanel9.Size = new System.Drawing.Size(764, 277);
			this.flowLayoutPanel9.TabIndex = 22;
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.AutoSize = true;
			this.flowLayoutPanel7.Controls.Add(this.flowLayoutPanel5);
			this.flowLayoutPanel7.Controls.Add(this.flowLayoutPanel6);
			this.flowLayoutPanel7.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(642, 165);
			this.flowLayoutPanel7.TabIndex = 20;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.AutoSize = true;
			this.flowLayoutPanel5.Controls.Add(this.flowLayoutPanel4);
			this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(256, 131);
			this.flowLayoutPanel5.TabIndex = 18;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.AutoSize = true;
			this.flowLayoutPanel4.Controls.Add(this.selectClipboardEnabled);
			this.flowLayoutPanel4.Controls.Add(this.selectClipboardAppEnabled);
			this.flowLayoutPanel4.Controls.Add(this.selectClipboardSave);
			this.flowLayoutPanel4.Controls.Add(this.selectClipboardTopMost);
			this.flowLayoutPanel4.Controls.Add(this.selectClipboardVisible);
			this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(250, 125);
			this.flowLayoutPanel4.TabIndex = 0;
			// 
			// selectClipboardEnabled
			// 
			this.selectClipboardEnabled.AutoSize = true;
			this.selectClipboardEnabled.Location = new System.Drawing.Point(3, 3);
			this.selectClipboardEnabled.Name = "selectClipboardEnabled";
			this.selectClipboardEnabled.Size = new System.Drawing.Size(218, 19);
			this.selectClipboardEnabled.TabIndex = 0;
			this.selectClipboardEnabled.Text = ":setting/check/clipboard-enabled";
			this.selectClipboardEnabled.UseVisualStyleBackColor = true;
			// 
			// selectClipboardAppEnabled
			// 
			this.selectClipboardAppEnabled.AutoSize = true;
			this.selectClipboardAppEnabled.Location = new System.Drawing.Point(3, 28);
			this.selectClipboardAppEnabled.Name = "selectClipboardAppEnabled";
			this.selectClipboardAppEnabled.Size = new System.Drawing.Size(244, 19);
			this.selectClipboardAppEnabled.TabIndex = 1;
			this.selectClipboardAppEnabled.Text = ":setting/check/clipboard-app-enabled";
			this.selectClipboardAppEnabled.UseVisualStyleBackColor = true;
			// 
			// selectClipboardSave
			// 
			this.selectClipboardSave.AutoSize = true;
			this.selectClipboardSave.Location = new System.Drawing.Point(3, 53);
			this.selectClipboardSave.Name = "selectClipboardSave";
			this.selectClipboardSave.Size = new System.Drawing.Size(200, 19);
			this.selectClipboardSave.TabIndex = 14;
			this.selectClipboardSave.Text = ":setting/check/clipboard-save";
			this.selectClipboardSave.UseVisualStyleBackColor = true;
			// 
			// selectClipboardTopMost
			// 
			this.selectClipboardTopMost.AutoSize = true;
			this.selectClipboardTopMost.Location = new System.Drawing.Point(3, 78);
			this.selectClipboardTopMost.Name = "selectClipboardTopMost";
			this.selectClipboardTopMost.Size = new System.Drawing.Size(222, 19);
			this.selectClipboardTopMost.TabIndex = 6;
			this.selectClipboardTopMost.Text = ":setting/check/clipboard-topmost";
			this.selectClipboardTopMost.UseVisualStyleBackColor = true;
			// 
			// selectClipboardVisible
			// 
			this.selectClipboardVisible.AutoSize = true;
			this.selectClipboardVisible.Location = new System.Drawing.Point(3, 103);
			this.selectClipboardVisible.Name = "selectClipboardVisible";
			this.selectClipboardVisible.Size = new System.Drawing.Size(209, 19);
			this.selectClipboardVisible.TabIndex = 5;
			this.selectClipboardVisible.Text = ":setting/check/clipboard-visible";
			this.selectClipboardVisible.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.panelClipboardTypes);
			this.flowLayoutPanel6.Location = new System.Drawing.Point(265, 3);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(374, 159);
			this.flowLayoutPanel6.TabIndex = 19;
			// 
			// panelClipboardTypes
			// 
			this.panelClipboardTypes.AutoSize = true;
			this.panelClipboardTypes.Controls.Add(this.groupClipboardType);
			this.panelClipboardTypes.Controls.Add(this.groupClipboardSaveType);
			this.panelClipboardTypes.Location = new System.Drawing.Point(3, 3);
			this.panelClipboardTypes.Name = "panelClipboardTypes";
			this.panelClipboardTypes.Size = new System.Drawing.Size(368, 153);
			this.panelClipboardTypes.TabIndex = 15;
			// 
			// groupClipboardType
			// 
			this.groupClipboardType.AutoSize = true;
			this.groupClipboardType.Controls.Add(this.panelClipboardType);
			this.groupClipboardType.Location = new System.Drawing.Point(9, 3);
			this.groupClipboardType.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
			this.groupClipboardType.Name = "groupClipboardType";
			this.groupClipboardType.Size = new System.Drawing.Size(172, 147);
			this.groupClipboardType.TabIndex = 9;
			this.groupClipboardType.TabStop = false;
			this.groupClipboardType.Text = ":setting/group/clipboard-type";
			// 
			// panelClipboardType
			// 
			this.panelClipboardType.AutoSize = true;
			this.panelClipboardType.Controls.Add(this.selectClipboardType_text);
			this.panelClipboardType.Controls.Add(this.selectClipboardType_rtf);
			this.panelClipboardType.Controls.Add(this.selectClipboardType_html);
			this.panelClipboardType.Controls.Add(this.selectClipboardType_image);
			this.panelClipboardType.Controls.Add(this.selectClipboardType_file);
			this.panelClipboardType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelClipboardType.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.panelClipboardType.Location = new System.Drawing.Point(3, 19);
			this.panelClipboardType.Name = "panelClipboardType";
			this.panelClipboardType.Size = new System.Drawing.Size(166, 125);
			this.panelClipboardType.TabIndex = 0;
			// 
			// selectClipboardType_text
			// 
			this.selectClipboardType_text.AutoSize = true;
			this.selectClipboardType_text.Location = new System.Drawing.Point(3, 3);
			this.selectClipboardType_text.Name = "selectClipboardType_text";
			this.selectClipboardType_text.Size = new System.Drawing.Size(148, 19);
			this.selectClipboardType_text.TabIndex = 0;
			this.selectClipboardType_text.Text = "#ClipboardType.Text";
			this.selectClipboardType_text.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardType_text.UseVisualStyleBackColor = true;
			// 
			// selectClipboardType_rtf
			// 
			this.selectClipboardType_rtf.AutoSize = true;
			this.selectClipboardType_rtf.Location = new System.Drawing.Point(3, 28);
			this.selectClipboardType_rtf.Name = "selectClipboardType_rtf";
			this.selectClipboardType_rtf.Size = new System.Drawing.Size(139, 19);
			this.selectClipboardType_rtf.TabIndex = 0;
			this.selectClipboardType_rtf.Text = "#ClipboardType.Rtf";
			this.selectClipboardType_rtf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardType_rtf.UseVisualStyleBackColor = true;
			// 
			// selectClipboardType_html
			// 
			this.selectClipboardType_html.AutoSize = true;
			this.selectClipboardType_html.Location = new System.Drawing.Point(3, 53);
			this.selectClipboardType_html.Name = "selectClipboardType_html";
			this.selectClipboardType_html.Size = new System.Drawing.Size(151, 19);
			this.selectClipboardType_html.TabIndex = 0;
			this.selectClipboardType_html.Text = "#ClipboardType.Html";
			this.selectClipboardType_html.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardType_html.UseVisualStyleBackColor = true;
			// 
			// selectClipboardType_image
			// 
			this.selectClipboardType_image.AutoSize = true;
			this.selectClipboardType_image.Location = new System.Drawing.Point(3, 78);
			this.selectClipboardType_image.Name = "selectClipboardType_image";
			this.selectClipboardType_image.Size = new System.Drawing.Size(160, 19);
			this.selectClipboardType_image.TabIndex = 0;
			this.selectClipboardType_image.Text = "#ClipboardType.Image";
			this.selectClipboardType_image.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardType_image.UseVisualStyleBackColor = true;
			// 
			// selectClipboardType_file
			// 
			this.selectClipboardType_file.AutoSize = true;
			this.selectClipboardType_file.Location = new System.Drawing.Point(3, 103);
			this.selectClipboardType_file.Name = "selectClipboardType_file";
			this.selectClipboardType_file.Size = new System.Drawing.Size(142, 19);
			this.selectClipboardType_file.TabIndex = 0;
			this.selectClipboardType_file.Text = "#ClipboardType.File";
			this.selectClipboardType_file.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardType_file.UseVisualStyleBackColor = true;
			// 
			// groupClipboardSaveType
			// 
			this.groupClipboardSaveType.AutoSize = true;
			this.groupClipboardSaveType.Controls.Add(this.flowLayoutPanel2);
			this.groupClipboardSaveType.Location = new System.Drawing.Point(193, 3);
			this.groupClipboardSaveType.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
			this.groupClipboardSaveType.Name = "groupClipboardSaveType";
			this.groupClipboardSaveType.Size = new System.Drawing.Size(172, 147);
			this.groupClipboardSaveType.TabIndex = 10;
			this.groupClipboardSaveType.TabStop = false;
			this.groupClipboardSaveType.Text = ":setting/group/save-clipboard-type";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.Controls.Add(this.selectClipboardSaveType_text);
			this.flowLayoutPanel2.Controls.Add(this.selectClipboardSaveType_rtf);
			this.flowLayoutPanel2.Controls.Add(this.selectClipboardSaveType_html);
			this.flowLayoutPanel2.Controls.Add(this.selectClipboardSaveType_image);
			this.flowLayoutPanel2.Controls.Add(this.selectClipboardSaveType_file);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 19);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(166, 125);
			this.flowLayoutPanel2.TabIndex = 0;
			// 
			// selectClipboardSaveType_text
			// 
			this.selectClipboardSaveType_text.AutoSize = true;
			this.selectClipboardSaveType_text.Location = new System.Drawing.Point(3, 3);
			this.selectClipboardSaveType_text.Name = "selectClipboardSaveType_text";
			this.selectClipboardSaveType_text.Size = new System.Drawing.Size(148, 19);
			this.selectClipboardSaveType_text.TabIndex = 0;
			this.selectClipboardSaveType_text.Text = "#ClipboardType.Text";
			this.selectClipboardSaveType_text.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardSaveType_text.UseVisualStyleBackColor = true;
			// 
			// selectClipboardSaveType_rtf
			// 
			this.selectClipboardSaveType_rtf.AutoSize = true;
			this.selectClipboardSaveType_rtf.Location = new System.Drawing.Point(3, 28);
			this.selectClipboardSaveType_rtf.Name = "selectClipboardSaveType_rtf";
			this.selectClipboardSaveType_rtf.Size = new System.Drawing.Size(139, 19);
			this.selectClipboardSaveType_rtf.TabIndex = 0;
			this.selectClipboardSaveType_rtf.Text = "#ClipboardType.Rtf";
			this.selectClipboardSaveType_rtf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardSaveType_rtf.UseVisualStyleBackColor = true;
			// 
			// selectClipboardSaveType_html
			// 
			this.selectClipboardSaveType_html.AutoSize = true;
			this.selectClipboardSaveType_html.Location = new System.Drawing.Point(3, 53);
			this.selectClipboardSaveType_html.Name = "selectClipboardSaveType_html";
			this.selectClipboardSaveType_html.Size = new System.Drawing.Size(151, 19);
			this.selectClipboardSaveType_html.TabIndex = 0;
			this.selectClipboardSaveType_html.Text = "#ClipboardType.Html";
			this.selectClipboardSaveType_html.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardSaveType_html.UseVisualStyleBackColor = true;
			// 
			// selectClipboardSaveType_image
			// 
			this.selectClipboardSaveType_image.AutoSize = true;
			this.selectClipboardSaveType_image.Location = new System.Drawing.Point(3, 78);
			this.selectClipboardSaveType_image.Name = "selectClipboardSaveType_image";
			this.selectClipboardSaveType_image.Size = new System.Drawing.Size(160, 19);
			this.selectClipboardSaveType_image.TabIndex = 0;
			this.selectClipboardSaveType_image.Text = "#ClipboardType.Image";
			this.selectClipboardSaveType_image.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardSaveType_image.UseVisualStyleBackColor = true;
			// 
			// selectClipboardSaveType_file
			// 
			this.selectClipboardSaveType_file.AutoSize = true;
			this.selectClipboardSaveType_file.Location = new System.Drawing.Point(3, 103);
			this.selectClipboardSaveType_file.Name = "selectClipboardSaveType_file";
			this.selectClipboardSaveType_file.Size = new System.Drawing.Size(142, 19);
			this.selectClipboardSaveType_file.TabIndex = 0;
			this.selectClipboardSaveType_file.Text = "#ClipboardType.File";
			this.selectClipboardSaveType_file.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectClipboardSaveType_file.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel8
			// 
			this.flowLayoutPanel8.AutoSize = true;
			this.flowLayoutPanel8.Controls.Add(this.tableLayoutPanel1);
			this.flowLayoutPanel8.Controls.Add(this.tableLayoutPanel3);
			this.flowLayoutPanel8.Location = new System.Drawing.Point(651, 3);
			this.flowLayoutPanel8.Name = "flowLayoutPanel8";
			this.flowLayoutPanel8.Size = new System.Drawing.Size(347, 204);
			this.flowLayoutPanel8.TabIndex = 21;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.labelClipboardListType, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelClipboardHotkey, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.inputClipboardHotkey, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelClipboardFont, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.commandClipboardTextFont, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.selectClipboardListType, 1, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(341, 97);
			this.tableLayoutPanel1.TabIndex = 16;
			// 
			// labelClipboardListType
			// 
			this.labelClipboardListType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelClipboardListType.AutoSize = true;
			this.labelClipboardListType.Location = new System.Drawing.Point(3, 67);
			this.labelClipboardListType.Name = "labelClipboardListType";
			this.labelClipboardListType.Size = new System.Drawing.Size(165, 15);
			this.labelClipboardListType.TabIndex = 14;
			this.labelClipboardListType.Text = ":setting/label/clipboard-list";
			// 
			// labelClipboardHotkey
			// 
			this.labelClipboardHotkey.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelClipboardHotkey.AutoSize = true;
			this.labelClipboardHotkey.Location = new System.Drawing.Point(3, 7);
			this.labelClipboardHotkey.Name = "labelClipboardHotkey";
			this.labelClipboardHotkey.Size = new System.Drawing.Size(188, 15);
			this.labelClipboardHotkey.TabIndex = 11;
			this.labelClipboardHotkey.Text = ":setting/label/clipboard-hotkey";
			// 
			// inputClipboardHotkey
			// 
			this.inputClipboardHotkey.BackColor = System.Drawing.Color.White;
			this.inputClipboardHotkey.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputClipboardHotkey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputClipboardHotkey.Hotkey = System.Windows.Forms.Keys.None;
			this.inputClipboardHotkey.HotKeySetting = null;
			this.inputClipboardHotkey.Location = new System.Drawing.Point(197, 3);
			this.inputClipboardHotkey.Modifiers = ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows.MOD.None;
			this.inputClipboardHotkey.Name = "inputClipboardHotkey";
			this.inputClipboardHotkey.ReadOnly = true;
			this.inputClipboardHotkey.Registered = false;
			this.inputClipboardHotkey.Size = new System.Drawing.Size(141, 23);
			this.inputClipboardHotkey.TabIndex = 7;
			this.inputClipboardHotkey.Text = "None";
			// 
			// labelClipboardFont
			// 
			this.labelClipboardFont.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelClipboardFont.AutoSize = true;
			this.labelClipboardFont.Location = new System.Drawing.Point(3, 37);
			this.labelClipboardFont.Name = "labelClipboardFont";
			this.labelClipboardFont.Size = new System.Drawing.Size(125, 15);
			this.labelClipboardFont.TabIndex = 13;
			this.labelClipboardFont.Text = ":common/label/font";
			// 
			// commandClipboardTextFont
			// 
			this.commandClipboardTextFont.AutoSize = true;
			this.commandClipboardTextFont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.commandClipboardTextFont.Location = new System.Drawing.Point(197, 32);
			this.commandClipboardTextFont.Name = "commandClipboardTextFont";
			this.commandClipboardTextFont.Size = new System.Drawing.Size(141, 25);
			this.commandClipboardTextFont.TabIndex = 8;
			this.commandClipboardTextFont.Text = "{FAMILY} {PT} ...";
			this.commandClipboardTextFont.UseVisualStyleBackColor = true;
			// 
			// selectClipboardListType
			// 
			this.selectClipboardListType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectClipboardListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectClipboardListType.FormattingEnabled = true;
			this.selectClipboardListType.Location = new System.Drawing.Point(197, 63);
			this.selectClipboardListType.Name = "selectClipboardListType";
			this.selectClipboardListType.Size = new System.Drawing.Size(141, 23);
			this.selectClipboardListType.TabIndex = 15;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.labelClipboardLimit, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.labelClipboardWaitTaime, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.labelClipboardSleepTime, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.inputClipboardLimit, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.inputClipboardSleepTime, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.inputClipboardWaitTime, 1, 1);
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 106);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 4;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(310, 95);
			this.tableLayoutPanel3.TabIndex = 17;
			// 
			// labelClipboardLimit
			// 
			this.labelClipboardLimit.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelClipboardLimit.AutoSize = true;
			this.labelClipboardLimit.Location = new System.Drawing.Point(3, 7);
			this.labelClipboardLimit.Name = "labelClipboardLimit";
			this.labelClipboardLimit.Size = new System.Drawing.Size(174, 15);
			this.labelClipboardLimit.TabIndex = 6;
			this.labelClipboardLimit.Text = ":setting/label/clipboard-limit";
			// 
			// labelClipboardWaitTaime
			// 
			this.labelClipboardWaitTaime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelClipboardWaitTaime.AutoSize = true;
			this.labelClipboardWaitTaime.Location = new System.Drawing.Point(3, 36);
			this.labelClipboardWaitTaime.Name = "labelClipboardWaitTaime";
			this.labelClipboardWaitTaime.Size = new System.Drawing.Size(173, 15);
			this.labelClipboardWaitTaime.TabIndex = 7;
			this.labelClipboardWaitTaime.Text = ":setting/label/clipboard-wait";
			// 
			// labelClipboardSleepTime
			// 
			this.labelClipboardSleepTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelClipboardSleepTime.AutoSize = true;
			this.labelClipboardSleepTime.Location = new System.Drawing.Point(3, 65);
			this.labelClipboardSleepTime.Name = "labelClipboardSleepTime";
			this.labelClipboardSleepTime.Size = new System.Drawing.Size(178, 15);
			this.labelClipboardSleepTime.TabIndex = 9;
			this.labelClipboardSleepTime.Text = ":setting/label/clipboard-sleep";
			// 
			// inputClipboardLimit
			// 
			this.inputClipboardLimit.Location = new System.Drawing.Point(187, 3);
			this.inputClipboardLimit.Name = "inputClipboardLimit";
			this.inputClipboardLimit.Size = new System.Drawing.Size(120, 23);
			this.inputClipboardLimit.TabIndex = 2;
			this.inputClipboardLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// inputClipboardSleepTime
			// 
			this.inputClipboardSleepTime.Location = new System.Drawing.Point(187, 61);
			this.inputClipboardSleepTime.Name = "inputClipboardSleepTime";
			this.inputClipboardSleepTime.Size = new System.Drawing.Size(120, 23);
			this.inputClipboardSleepTime.TabIndex = 4;
			this.inputClipboardSleepTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// inputClipboardWaitTime
			// 
			this.inputClipboardWaitTime.Location = new System.Drawing.Point(187, 32);
			this.inputClipboardWaitTime.Name = "inputClipboardWaitTime";
			this.inputClipboardWaitTime.Size = new System.Drawing.Size(120, 23);
			this.inputClipboardWaitTime.TabIndex = 3;
			this.inputClipboardWaitTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
			// panelSetting
			// 
			this.panelSetting.ColumnCount = 1;
			this.panelSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelSetting.Controls.Add(this.panelCommand, 0, 1);
			this.panelSetting.Controls.Add(this.tabSetting, 0, 0);
			this.panelSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSetting.Location = new System.Drawing.Point(0, 0);
			this.panelSetting.Name = "panelSetting";
			this.panelSetting.RowCount = 2;
			this.panelSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelSetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.panelSetting.Size = new System.Drawing.Size(784, 382);
			this.panelSetting.TabIndex = 2;
			// 
			// panelCommand
			// 
			this.panelCommand.AutoSize = true;
			this.panelCommand.Controls.Add(this.commandSubmit);
			this.panelCommand.Controls.Add(this.commandCancel);
			this.panelCommand.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelCommand.Location = new System.Drawing.Point(595, 342);
			this.panelCommand.Name = "panelCommand";
			this.panelCommand.Size = new System.Drawing.Size(186, 37);
			this.panelCommand.TabIndex = 3;
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
			this.ClientSize = new System.Drawing.Size(784, 382);
			this.Controls.Add(this.panelSetting);
			this.Icon = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_App;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(800, 420);
			this.Name = "SettingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = ":window/setting";
			this.tabSetting.ResumeLayout(false);
			this.tabSetting_pageMain.ResumeLayout(false);
			this.tabSetting_pageMain.PerformLayout();
			this.groupLauncherStream.ResumeLayout(false);
			this.groupLauncherStream.PerformLayout();
			this.groupMainSkin.ResumeLayout(false);
			this.groupMainSkin.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.panelMainOthers.ResumeLayout(false);
			this.panelMainOthers.PerformLayout();
			this.groupUpdateCheck.ResumeLayout(false);
			this.groupUpdateCheck.PerformLayout();
			this.panelUpdate.ResumeLayout(false);
			this.panelUpdate.PerformLayout();
			this.groupMainSystemEnv.ResumeLayout(false);
			this.panelMainSystemEnv.ResumeLayout(false);
			this.panelMainSystemEnv.PerformLayout();
			this.groupMainLog.ResumeLayout(false);
			this.groupMainLog.PerformLayout();
			this.groupLogTrigger.ResumeLayout(false);
			this.groupLogTrigger.PerformLayout();
			this.tabSetting_pageLauncher.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabLauncher.ResumeLayout(false);
			this.tabLauncher_pageCommon.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.groupLauncherType.ResumeLayout(false);
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.tabLauncher_pageEnv.ResumeLayout(false);
			this.panelLauncherEnv.ResumeLayout(false);
			this.panelLauncherEnv.PerformLayout();
			this.tabLauncher_pageOthers.ResumeLayout(false);
			this.panelLauncherOthers.ResumeLayout(false);
			this.panelLauncherOthers.PerformLayout();
			this.tabSetting_pageToolbar.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.groupToolbar.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputToolbarTextWidth)).EndInit();
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
			this.tabSetting_pageCommand.ResumeLayout(false);
			this.tabSetting_pageCommand.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).EndInit();
			this.tabSetting_pageNote.ResumeLayout(false);
			this.panelNote.ResumeLayout(false);
			this.panelNoteOthers.ResumeLayout(false);
			this.panelNoteOthers.PerformLayout();
			this.groupNoteKey.ResumeLayout(false);
			this.panelNoteKey.ResumeLayout(false);
			this.panelNoteKey.PerformLayout();
			this.groupNoteItem.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridNoteItems)).EndInit();
			this.tabSetting_pageClipboard.ResumeLayout(false);
			this.flowLayoutPanel9.ResumeLayout(false);
			this.flowLayoutPanel9.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.panelClipboardTypes.ResumeLayout(false);
			this.panelClipboardTypes.PerformLayout();
			this.groupClipboardType.ResumeLayout(false);
			this.groupClipboardType.PerformLayout();
			this.panelClipboardType.ResumeLayout(false);
			this.panelClipboardType.PerformLayout();
			this.groupClipboardSaveType.ResumeLayout(false);
			this.groupClipboardSaveType.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flowLayoutPanel8.ResumeLayout(false);
			this.flowLayoutPanel8.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputClipboardLimit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.inputClipboardSleepTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.inputClipboardWaitTime)).EndInit();
			this.panelSetting.ResumeLayout(false);
			this.panelSetting.PerformLayout();
			this.panelCommand.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.FlowLayoutPanel panelMainOthers;
		private System.Windows.Forms.TableLayoutPanel panelMainSystemEnv;
		private System.Windows.Forms.TableLayoutPanel panelLauncherOthers;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputNoteShowFront;
		private System.Windows.Forms.Label labelNoteShowFront;
		private System.Windows.Forms.TableLayoutPanel panelNote;
		private System.Windows.Forms.TableLayoutPanel panelLauncherEnv;
		private System.Windows.Forms.CheckBox selectMainStartup;
		private System.Windows.Forms.RadioButton selectLauncherType_embedded;
		private System.Windows.Forms.RadioButton selectLauncherType_directory;
		private System.Windows.Forms.CheckBox selectLogFullDetail;
		private System.Windows.Forms.CheckBox selectUpdateCheckRC;
		private System.Windows.Forms.CheckBox selectUpdateCheck;
		private System.Windows.Forms.FlowLayoutPanel panelUpdate;
		private System.Windows.Forms.GroupBox groupUpdateCheck;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel panelNoteOthers;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.DataGridView gridNoteItems;
		private System.Windows.Forms.GroupBox groupNoteItem;
		private System.Windows.Forms.GroupBox groupNoteKey;
		private System.Windows.Forms.Label labelNoteCaptionFont;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton commandNoteCaptionFont;
		private System.Windows.Forms.FlowLayoutPanel panelNoteKey;
		private System.Windows.Forms.Label labelNoteCompact;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputNoteCompact;
		private System.Windows.Forms.Label labelNoteHiddent;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputNoteHidden;
		private System.Windows.Forms.Label labelNoteCreate;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputNoteCreate;
		private System.Windows.Forms.CheckBox selectLogTrigger_information;
		private System.Windows.Forms.CheckBox selectLogTrigger_warning;
		private System.Windows.Forms.CheckBox selectLogTrigger_error;
		private System.Windows.Forms.GroupBox groupLogTrigger;
		private ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvRemoveControl envLauncherRemove;
		private ContentTypeTextNet.Pe.PeMain.UI.CustomControl.EnvUpdateControl envLauncherUpdate;
		private System.Windows.Forms.CheckBox selectLauncherEnv;
		private System.Windows.Forms.CheckBox selectLauncherAdmin;
		private System.Windows.Forms.TabPage tabLauncher_pageOthers;
		private System.Windows.Forms.TabPage tabLauncher_pageEnv;
		private System.Windows.Forms.TabPage tabLauncher_pageCommon;
		private System.Windows.Forms.TabControl tabLauncher;
		private System.Windows.Forms.Label labelSystemEnvExt;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputSystemEnvHiddenFile;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputSystemEnvExt;
		private System.Windows.Forms.Label labelSystemEnvHiddenFile;
		private System.Windows.Forms.GroupBox groupMainSystemEnv;
		private System.Windows.Forms.ComboBox selectToolbarItem;
		private System.Windows.Forms.GroupBox groupToolbar;
		private System.Windows.Forms.Label labelToolbarTextWidth;
		private System.Windows.Forms.NumericUpDown inputToolbarTextWidth;
		private System.Windows.Forms.Label labelCommandHotkey;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputCommandHotkey;
		private System.Windows.Forms.CheckBox selectLogAddShow;
		private System.Windows.Forms.CheckBox selectLogVisible;
		private System.Windows.Forms.GroupBox groupMainLog;
		private System.Windows.Forms.CheckBox selectToolbarShowText;
		private System.Windows.Forms.CheckBox selectToolbarAutoHide;
		private System.Windows.Forms.CheckBox selectLauncherStdStream;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_remove;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_down;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_up;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.DisableCloseToolStripSeparator DisableCloseToolStripSeparator1;
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
		private System.Windows.Forms.ComboBox inputLauncherCommand;
		private System.Windows.Forms.TextBox inputLauncherName;
		private System.Windows.Forms.TextBox inputLauncherWorkDirPath;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.IconTextBox inputLauncherIconPath;
		private System.Windows.Forms.Label labelLauncherCommand;
		private System.Windows.Forms.Label labelLauncherName;
		private System.Windows.Forms.Label labelLauncherWorkDirPath;
		private System.Windows.Forms.Label labelLauncherIconPath;
		private System.Windows.Forms.RadioButton selectLauncherType_file;
		private System.Windows.Forms.RadioButton selectLauncherType_command;
		private System.Windows.Forms.GroupBox groupLauncherType;
		private ContentTypeTextNet.Pe.PeMain.UI.CustomControl.LauncherItemSelectControl selecterToolbar;
		private ContentTypeTextNet.Pe.PeMain.UI.CustomControl.LauncherItemSelectControl selecterLauncher;
		private System.Windows.Forms.TreeView treeToolbarItemGroup;
		private System.Windows.Forms.ComboBox selectToolbarPosition;
		private System.Windows.Forms.Label labelToolbarPosition;
		private System.Windows.Forms.ComboBox selectToolbarIcon;
		private System.Windows.Forms.Label labelToolbarIcon;
		private System.Windows.Forms.CheckBox selectToolbarVisible;
		private System.Windows.Forms.Label labelToolbarFont;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton commandToolbarFont;
		private System.Windows.Forms.CheckBox selectToolbarTopmost;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabPage tabSetting_pageNote;
		private System.Windows.Forms.TabPage tabSetting_pageDisplay;
		private System.Windows.Forms.TabPage tabSetting_pageToolbar;
		private System.Windows.Forms.NumericUpDown inputCommandHideTime;
		private System.Windows.Forms.Label labelCommandHideTime;
		private System.Windows.Forms.CheckBox selectCommandTopmost;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton commandCommandFont;
		private System.Windows.Forms.Label labelCommandFont;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.TabPage tabSetting_pageCommand;
		private System.Windows.Forms.TableLayoutPanel panelSetting;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.Button commandSubmit;
		private System.Windows.Forms.FlowLayoutPanel panelCommand;
		private System.Windows.Forms.TabPage tabSetting_pageLauncher;
		private System.Windows.Forms.TabPage tabSetting_pageMain;
		private System.Windows.Forms.TabControl tabSetting;
		private System.Windows.Forms.Label labelToolbarGroup;
		private System.Windows.Forms.ComboBox selectToolbarGroup;
		private System.Windows.Forms.TabPage tabSetting_pageClipboard;
		private System.Windows.Forms.CheckBox selectClipboardEnabled;
		private System.Windows.Forms.CheckBox selectClipboardType_file;
		private System.Windows.Forms.CheckBox selectClipboardType_image;
		private System.Windows.Forms.CheckBox selectClipboardType_html;
		private System.Windows.Forms.CheckBox selectClipboardType_rtf;
		private System.Windows.Forms.CheckBox selectClipboardType_text;
		private System.Windows.Forms.CheckBox selectClipboardTopMost;
		private System.Windows.Forms.CheckBox selectClipboardVisible;
		private System.Windows.Forms.CheckBox selectClipboardAppEnabled;
		private System.Windows.Forms.NumericUpDown inputClipboardWaitTime;
		private System.Windows.Forms.NumericUpDown inputClipboardLimit;
		private System.Windows.Forms.Label labelClipboardWaitTaime;
		private System.Windows.Forms.Label labelClipboardLimit;
		private System.Windows.Forms.GroupBox groupClipboardType;
		private System.Windows.Forms.FlowLayoutPanel panelClipboardType;
		private System.Windows.Forms.Label labelClipboardSleepTime;
		private System.Windows.Forms.NumericUpDown inputClipboardSleepTime;
		private System.Windows.Forms.Label labelClipboardHotkey;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.PeHotkeyControl inputClipboardHotkey;
		private System.Windows.Forms.GroupBox groupMainSkin;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.ComboBox selectSkinName;
		private System.Windows.Forms.Button commandSkinAbout;
		private System.Windows.Forms.CheckBox selectLogDebugging;
		private System.Windows.Forms.Label labelClipboardFont;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton commandClipboardTextFont;
		private System.Windows.Forms.GroupBox groupLauncherStream;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.FontSplitButton commandLauncherStreamFont;
		private System.Windows.Forms.Button commandToolbarScreens;
		private System.Windows.Forms.DataGridViewCheckBoxColumn gridNoteItems_columnRemove;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridNoteItems_columnId;
		private System.Windows.Forms.DataGridViewCheckBoxColumn gridNoteItems_columnVisible;
		private System.Windows.Forms.DataGridViewCheckBoxColumn gridNoteItems_columnLocked;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridNoteItems_columnTitle;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridNoteItems_columnBody;
		private Ex.NoteFontDataGridViewButtonColumn gridNoteItems_columnFont;
		private Ex.NoteColorDataGridViewButtonColumn gridNoteItems_columnFore;
		private Ex.NoteColorDataGridViewButtonColumn gridNoteItems_columnBack;
		private System.Windows.Forms.GroupBox groupClipboardSaveType;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.CheckBox selectClipboardSaveType_text;
		private System.Windows.Forms.CheckBox selectClipboardSaveType_rtf;
		private System.Windows.Forms.CheckBox selectClipboardSaveType_html;
		private System.Windows.Forms.CheckBox selectClipboardSaveType_image;
		private System.Windows.Forms.CheckBox selectClipboardSaveType_file;
		private System.Windows.Forms.CheckBox selectClipboardSave;
		private System.Windows.Forms.FlowLayoutPanel panelClipboardTypes;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label labelClipboardListType;
		private System.Windows.Forms.ComboBox selectClipboardListType;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel8;
	}
}
