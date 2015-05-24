namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Skin.SystemSkin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.DB;
	using ContentTypeTextNet.Pe.PeMain.UI.CustomControl;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;
	using ContentTypeTextNet.Pe.PeMain.UI.Skin;

	/// <summary>
	/// 設定。
	/// 
	/// 一気にUIへ設定して一気にUIから取得する気分だったけど完全に設計ミスだわ。
	/// バインドするなりしておけばよかった。
	/// </summary>
	public partial class SettingForm: CommonForm
	{
		#region define
		//const int TREE_LEVEL_GROUP = 0;
		//const int TREE_LEVEL_ITEM = 1;

		const int TREE_TYPE_NONE = 0;
		const int TREE_TYPE_GROUP = 1;

		const string ddTreeNode = "tree-node";

		class NoteWrapItem
		{
			public NoteWrapItem(NoteItem item)
			{
				Remove = false;
				NewItem = false;

				NoteItem = item;
			}

			public NoteItem NoteItem { get; set; }

			public bool NewItem { get; set; }

			#region property name
			public bool Remove { get; set; }
			public long Id
			{
				get { return NoteItem.NoteId; }
				set { NoteItem.NoteId = value; }
			}
			public bool Visible
			{
				get { return NoteItem.Visible; }
				set { NoteItem.Visible = value; }
			}
			public bool Locked
			{
				get { return NoteItem.Locked; }
				set { NoteItem.Locked = value; }
			}
			public string Title
			{
				get { return NoteItem.Title; }
				set { NoteItem.Title = value; }
			}
			public string Body
			{
				get { return NoteItem.Body; }
				set { NoteItem.Body = value; }
			}
			public FontSetting Font
			{
				get { return NoteItem.Style.FontSetting; }
				set { NoteItem.Style.FontSetting = value; }
			}
			public Color Fore
			{
				get { return NoteItem.Style.Color.Fore.Color; }
				set { NoteItem.Style.Color.Fore.Color = value; }
			}
			public Color Back
			{
				get { return NoteItem.Style.Color.Back.Color; }
				set { NoteItem.Style.Color.Back.Color = value; }
			}
			#endregion
		}
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		//HashSet<LauncherItem> _launcherItems = null;
		//FontSetting _commandFont = null;
		//FontSetting _toolbarFont = null;
		LauncherItem _launcherSelectedItem = null;

		TabPage _nowSelectedTabPage = null;
		ImageList _imageToolbarItemGroup = null;
		bool _launcherItemEvent = false;

		List<NoteWrapItem> _noteItemList;

		/*
		Point _toolbarLocation;
		Size _toolbarSize;
		*/
		/*
		Dictionary<string, Rectangle> _toolbarFloat = new Dictionary<string, Rectangle>();
		Dictionary<string, FontSetting> _toolbarFont = new Dictionary<string, FontSetting>();
		*/
		ToolbarItem _toolbarSelectedToolbarItem = null;

		//ApplicationSetting _applicationSetting;

		string[] _launcherCommandList;
		#endregion ////////////////////////////////////

		#region event
		#endregion ////////////////////////////////////

		public SettingForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			Initialize();
		}

		#region property
		/// <summary>
		/// 使用言語データ
		/// </summary>
		//public Language Language { get; private set; }
		//public ISkin CommonData.Skin { get; private set; }

		//public MainSetting MainSetting { get; private set; }

		#endregion ////////////////////////////////////

		#region ISetCommonData
		#endregion ////////////////////////////////////

		#region override

		protected override void ApplySetting()
		{
			base.ApplySetting();

			ApplyMainSetting();
			ApplyLauncher();
			ApplyToolbar();
			ApplyCommand();
			ApplyNote();
			ApplyClipboard();
		}
		#endregion ////////////////////////////////////

		#region initialize


		void InitializeToolbar()
		{
			this.inputToolbarTextWidth.SetRange(Literal.toolbarTextWidth);
		}

		void InitializeUI()
		{
			InitializeToolbar();

			UIUtility.ShowCenterInPrimaryScreen(this);
		}

		void InitializeCommand()
		{
			//_commandList
			var osDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
			var systemDirPath = Environment.SystemDirectory;

			var pathDirList = Environment.GetEnvironmentVariable("PATH")
				.Split(';')
				.Select(Environment.ExpandEnvironmentVariables)
				.Where(s => string.Compare(s, osDirPath, true) == 0 || string.Compare(s, systemDirPath, true) == 0)
				.Distinct()
			;

			var dirList = new List<string>(new[] { osDirPath, systemDirPath });
			dirList.AddRange(pathDirList);

			this._launcherCommandList = dirList
				.Where(Directory.Exists)
				.Select(s => Directory.EnumerateFiles(s, "*.exe"))
				.SelectMany(list => list)
				.Select(Path.GetFileNameWithoutExtension)
				.OrderBy(s => s)
				.Distinct()
				.ToArray()
			;
		}

		void Initialize()
		{
			//this._launcherItems = new HashSet<LauncherItem>();

			//Language = language;
			//Skin = skin;
			//this._applicationSetting = applicationSetting;

			InitializeCommand();

			InitializeUI();
		}

		#endregion ////////////////////////////////////

		#region apply

		void ApplyLog(LogSetting setting)
		{
			this.selectLogVisible.DataBindings.Add("Checked", setting, "Visible", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectLogAddShow.DataBindings.Add("Checked", setting, "AddShow", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectLogFullDetail.DataBindings.Add("Checked", setting, "FullDetail", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectLogDebugging.DataBindings.Add("Checked", setting, "Debugging", false, DataSourceUpdateMode.OnPropertyChanged);

			this.selectLogTrigger_information.Checked = (setting.AddShowTrigger & LogType.Information) == LogType.Information;
			this.selectLogTrigger_warning.Checked = (setting.AddShowTrigger & LogType.Warning) == LogType.Warning;
			this.selectLogTrigger_error.Checked = (setting.AddShowTrigger & LogType.Error) == LogType.Error;
		}

		void ApplySystemEnv(SystemEnvironmentSetting setting)
		{
			/*
			this.inputSystemEnvHiddenFile.Hotkey = systemEnvSetting.HiddenFileShowHotKey.Key;
			this.inputSystemEnvHiddenFile.Modifiers = systemEnvSetting.HiddenFileShowHotKey.Modifiers;
			this.inputSystemEnvHiddenFile.Registered = systemEnvSetting.HiddenFileShowHotKey.Registered;
			
			this.inputSystemEnvExt.Hotkey = systemEnvSetting.ExtensionShowHotKey.Key;
			this.inputSystemEnvExt.Modifiers = systemEnvSetting.ExtensionShowHotKey.Modifiers;
			this.inputSystemEnvExt.Registered = systemEnvSetting.ExtensionShowHotKey.Registered;
			 */
			this.inputSystemEnvHiddenFile.HotKeySetting = setting.HiddenFileShowHotKey;
			this.inputSystemEnvExt.HotKeySetting = setting.ExtensionShowHotKey;
		}

		void ApplyRunningInfo(RunningSetting setting)
		{
			this.selectUpdateCheck.DataBindings.Add("Checked", setting, "CheckUpdate", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectUpdateCheckRC.DataBindings.Add("Checked", setting, "CheckUpdateRC", false, DataSourceUpdateMode.OnPropertyChanged);
		}

		void ApplySkin(SkinSetting setting)
		{
			var skins = AppUtility.GetSkins(new NullLogger());

			var skinDisplayValues = skins.Select(s => new SkinDisplayValue(s));
			var selectSkin = skinDisplayValues.SingleOrDefault(s => s.Value.About.Name == setting.Name);
			if(selectSkin != null) {
				this.selectSkinName.Attachment(skinDisplayValues, selectSkin.Value);
			} else {
				var defSkin = new SystemSkin();
				defSkin.Load();
				var skin = skinDisplayValues.Single(s => s.Value.About.Name == defSkin.About.Name);
				this.selectSkinName.Attachment(skinDisplayValues, skin.Value);
				defSkin.Unload();
			}

		}

		void ApplyLanguageSetting(string languageName)
		{
			var langFileName = string.Format("{0}.xml", languageName);
			var languageFilePath = Path.Combine(Literal.ApplicationLanguageDirPath, langFileName);

			// TODO: 泥臭い
			var languageTempList = Directory.GetFiles(Literal.ApplicationLanguageDirPath, "*.xml")
				.Where(s => string.Compare(Path.GetFileName(s), string.Format("{0}.xml", Literal.defaultLanguage), true) != 0)
				.Select(f => new {
					Language = Serializer.LoadXmlFile<Language>(f, false),
					BaseName = Path.GetFileNameWithoutExtension(f),
				})
				.ToArray()
				;
			var languagePairList = new List<Language>(languageTempList.Length);
			foreach(var lang in languageTempList) {
				lang.Language.BaseName = lang.BaseName;
				languagePairList.Add(lang.Language);
			}
			var langList = languagePairList
				.Select(l => new {
					DisplayValue = new LanguageDisplayValue(l),
					Language = l,
				})
				;
			var selectedItem = langList.SingleOrDefault(l => l.Language.BaseName == languageName);
			Language selectedLang = null;
			if(selectedItem != null) {
				selectedLang = selectedItem.Language;
			}
			if(selectedLang != null) {
				this.selectMainLanguage.Attachment(langList.Select(l => l.DisplayValue), selectedLang);
			} else {
				this.selectMainLanguage.Attachment(langList.Select(l => l.DisplayValue));
			}
		}

		void ApplyStream(StreamSetting setting)
		{
			this.commandStreamFont.FontSetting.Import(setting.FontSetting);
			this.commandStreamFont.RefreshView();

			this.commnadStreamGeneralForeColor.Color = setting.GeneralColor.Fore.Color;
			this.commnadStreamGeneralBackColor.Color = setting.GeneralColor.Back.Color;
			this.commnadStreamInputForeColor.Color = setting.InputColor.Fore.Color;
			this.commnadStreamInputBackColor.Color = setting.InputColor.Back.Color;
			this.commnadStreamErrorForeColor.Color = setting.ErrorColor.Fore.Color;
			this.commnadStreamErrorBackColor.Color = setting.ErrorColor.Back.Color;
		}

		void ApplyMainSetting()
		{
			var linkPath = Literal.StartupShortcutPath;
			this.selectMainStartup.Checked = File.Exists(linkPath);

			ApplyLog(CommonData.MainSetting.Log);
			ApplySystemEnv(CommonData.MainSetting.SystemEnvironment);
			ApplyRunningInfo(CommonData.MainSetting.Running);
			ApplyLanguageSetting(CommonData.MainSetting.LanguageName);
			ApplySkin(CommonData.MainSetting.Skin);
			ApplyStream(CommonData.MainSetting.Stream);
		}

		void ApplyLauncher()
		{
			this.selecterLauncher.SetItems(CommonData.MainSetting.Launcher.Items, CommonData.ApplicationSetting);
		}

		void ApplyToolbar()
		{
			this.selecterToolbar.SetItems(CommonData.MainSetting.Launcher.Items, CommonData.ApplicationSetting);

			// ツールーバー位置の項目構築
			var toolbarPosList = new List<ToolbarPositionDisplayValue>();
			foreach(var value in new[] { ToolbarPosition.DesktopFloat, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopBottom, ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopRight, }) {
				var data = new ToolbarPositionDisplayValue(value);
				data.SetLanguage(CommonData.Language);
				toolbarPosList.Add(data);
			}
			this.selectToolbarPosition.Attachment(toolbarPosList);

			// アイコンサイズ文言の項目構築
			AttachmentIconScale(this.selectToolbarIcon, IconScale.Small);

			ToolbarItem initToolbarItem = null;
			var toolbarItemDataList = new List<ToolbarDisplayValue>();
			foreach(var toolbarItem in CommonData.MainSetting.Toolbar.Items) {
				if(initToolbarItem == null && toolbarItem.IsNameEqual(Screen.PrimaryScreen.DeviceName)) {
					initToolbarItem = toolbarItem;
				}
				var toolbarItemData = new ToolbarDisplayValue(toolbarItem);
				//toolbarItemData.SetLanguage(Language);
				toolbarItemDataList.Add(toolbarItemData);
			}
			//this.selectToolbarItem.Attachment(toolbarItemDataList, initToolbarItem);
			this.selectToolbarItem.Attachment(toolbarItemDataList, initToolbarItem);
			this.selectToolbarItem.SelectedIndex = 0;
			
			// グループ情報設定
			var toolbarGroupList = new List<ToolbarGroupNameDisplayValue>();
			var rootNode = treeToolbarItemGroup.Nodes.Cast<TreeNode>();
			toolbarGroupList.Add(new ToolbarGroupNameDisplayValue(string.Empty));
			foreach(var groupItem in CommonData.MainSetting.Toolbar.ToolbarGroup.Groups) {
				var displayValue = new ToolbarGroupNameDisplayValue(groupItem.Name);
				toolbarGroupList.Add(displayValue);
			}
			this.selectToolbarGroup.Attachment(toolbarGroupList);

			ToolbarSelectedChangeToolbarItem(initToolbarItem);

			// グループ用項目
			this._imageToolbarItemGroup = new ImageList();
			this._imageToolbarItemGroup.ColorDepth = ColorDepth.Depth32Bit;

			// 各グループ構築
			foreach(var groupItem in CommonData.MainSetting.Toolbar.ToolbarGroup.Groups) {
				// メイングループ
				var parentNode = ToolbarAddGroup(groupItem.Name);
				// メイングループに紐付くアイテム
				foreach(var itemName in groupItem.ItemNames) {
					var relItem = CommonData.MainSetting.Launcher.Items.SingleOrDefault(item => item.IsNameEqual(itemName));
					if(relItem != null) {
						ToolbarAddItem(parentNode, relItem);
					}
				}
			}
		}

		void ApplyCommand()
		{
			//selectCommandFindTag
			this.selectCommandFindTag.DataBindings.Add("Checked", CommonData.MainSetting.Command, "EnabledFindTag");
			this.selectCommandFindFile.DataBindings.Add("Checked", CommonData.MainSetting.Command, "EnabledFindFile");

			//this._commandFont = commandSetting.FontSetting;
			this.commandCommandFont.FontSetting.Import(CommonData.MainSetting.Command.FontSetting);
			this.commandCommandFont.RefreshView();

			// アイコンサイズ文言の項目構築
			AttachmentIconScale(this.selectCommandIcon, CommonData.MainSetting.Command.IconScale);

			// ホットキー
			this.inputCommandHotkey.HotKeySetting = CommonData.MainSetting.Command.HotKey;

			// 消える時間
			this.inputCommandHideTime.SetValue(Literal.commandHiddenTime, CommonData.MainSetting.Command.HiddenTime);
		}

		void ApplyNote()
		{
			// ホットキー
			this.inputNoteCreate.HotKeySetting = CommonData.MainSetting.Note.CreateHotKey;
			this.inputNoteCompact.HotKeySetting = CommonData.MainSetting.Note.CompactHotKey;
			this.inputNoteHidden.HotKeySetting = CommonData.MainSetting.Note.HiddenHotKey;
			this.inputNoteShowFront.HotKeySetting = CommonData.MainSetting.Note.ShowFrontHotKey;

			this.commandNoteCaptionFont.FontSetting.Import(CommonData.MainSetting.Note.CaptionFontSetting);
			this.commandNoteCaptionFont.RefreshView();

			// 全リスト
			this.gridNoteItems.AutoGenerateColumns = false;
			var noteDB = new NoteDB(CommonData.Database);
			var noteRawList = noteDB.GetNoteItemList(true);
			this._noteItemList = new List<NoteWrapItem>(noteRawList.Count());
			foreach(var item in noteRawList) {
				var wrap = new NoteWrapItem(item);
				this._noteItemList.Add(wrap);
			}
			this.gridNoteItems_columnRemove.DataPropertyName = "Remove";
			this.gridNoteItems_columnId.DataPropertyName = "Id";
			this.gridNoteItems_columnVisible.DataPropertyName = "Visible";
			this.gridNoteItems_columnLocked.DataPropertyName = "Locked";
			this.gridNoteItems_columnTitle.DataPropertyName = "Title";
			this.gridNoteItems_columnBody.DataPropertyName = "Body";
			this.gridNoteItems_columnFont.DataPropertyName = "Font";
			this.gridNoteItems_columnFore.DataPropertyName = "Fore";
			this.gridNoteItems_columnBack.DataPropertyName = "Back";
			this.gridNoteItems.DataSource = new BindingSource(this._noteItemList, string.Empty);
		}

		void ApplyClipboard()
		{
			this.inputClipboardLimit.SetValue(Literal.clipboardLimit, CommonData.MainSetting.Clipboard.Limit);
			this.inputClipboardWaitTime.SetValue(Literal.clipboardWaitTime, CommonData.MainSetting.Clipboard.WaitTime);
			this.inputClipboardRepeated.SetValue(Literal.clipboardRepeated, CommonData.MainSetting.Clipboard.ClipboardRepeated);

			this.inputClipboardLimit.DataBindings.Add("Value", CommonData.MainSetting.Clipboard, "Limit", false, DataSourceUpdateMode.OnPropertyChanged);
			this.inputClipboardRepeated.DataBindings.Add("Value", CommonData.MainSetting.Clipboard, "ClipboardRepeated", false, DataSourceUpdateMode.OnPropertyChanged);

			this.selectClipboardEnabled.DataBindings.Add("Checked", CommonData.MainSetting.Clipboard, "Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectClipboardAppEnabled.DataBindings.Add("Checked", CommonData.MainSetting.Clipboard, "EnabledApplicationCopy", false, DataSourceUpdateMode.OnPropertyChanged);

			this.selectClipboardVisible.DataBindings.Add("Checked", CommonData.MainSetting.Clipboard, "Visible", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectClipboardTopMost.DataBindings.Add("Checked", CommonData.MainSetting.Clipboard, "TopMost", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectClipboardItemWClickToOutput.DataBindings.Add("Checked", CommonData.MainSetting.Clipboard, "DoubleClickToOutput", false, DataSourceUpdateMode.OnPropertyChanged);
			this.selectClipboardOutputUsingClipboard.DataBindings.Add("Checked", CommonData.MainSetting.Clipboard, "OutputUsingClipboard", false, DataSourceUpdateMode.OnPropertyChanged);

			this.selectClipboardSave.DataBindings.Add("Checked", CommonData.MainSetting.Clipboard, "SaveHistory", false, DataSourceUpdateMode.OnPropertyChanged);

			this.selectClipboardType_text.Checked = CommonData.MainSetting.Clipboard.EnabledTypes.HasFlag(ClipboardType.Text);
			this.selectClipboardType_rtf.Checked = CommonData.MainSetting.Clipboard.EnabledTypes.HasFlag(ClipboardType.Rtf);
			this.selectClipboardType_html.Checked = CommonData.MainSetting.Clipboard.EnabledTypes.HasFlag(ClipboardType.Html);
			this.selectClipboardType_image.Checked = CommonData.MainSetting.Clipboard.EnabledTypes.HasFlag(ClipboardType.Image);
			this.selectClipboardType_file.Checked = CommonData.MainSetting.Clipboard.EnabledTypes.HasFlag(ClipboardType.File);

			this.selectClipboardSaveType_text.Checked = CommonData.MainSetting.Clipboard.SaveTypes.HasFlag(ClipboardType.Text);
			this.selectClipboardSaveType_rtf.Checked = CommonData.MainSetting.Clipboard.SaveTypes.HasFlag(ClipboardType.Rtf);
			this.selectClipboardSaveType_html.Checked = CommonData.MainSetting.Clipboard.SaveTypes.HasFlag(ClipboardType.Html);
			this.selectClipboardSaveType_image.Checked = CommonData.MainSetting.Clipboard.SaveTypes.HasFlag(ClipboardType.Image);
			this.selectClipboardSaveType_file.Checked = CommonData.MainSetting.Clipboard.SaveTypes.HasFlag(ClipboardType.File);

			this.inputClipboardHotkey.HotKeySetting = CommonData.MainSetting.Clipboard.ToggleHotKeySetting;

			this.commandClipboardTextFont.FontSetting.Import(CommonData.MainSetting.Clipboard.TextFont);
			this.commandClipboardTextFont.RefreshView();

			var clipboardListTypeValues = new List<ClipboardListTypeDisplayValue>();
			foreach(var type in new[] { ClipboardListType.History, ClipboardListType.Template }) {
				var dv = new ClipboardListTypeDisplayValue(type);
				dv.SetLanguage(CommonData.Language);
				clipboardListTypeValues.Add(dv);
			}
			this.selectClipboardListType.Attachment(clipboardListTypeValues, CommonData.MainSetting.Clipboard.ClipboardListType);
		}

		#endregion

		#region language

		void ApplyLanguageTab()
		{
			this.tabSetting_pageMain.SetLanguage(CommonData.Language);
			this.tabSetting_pageLauncher.SetLanguage(CommonData.Language);
			this.tabSetting_pageToolbar.SetLanguage(CommonData.Language);
			this.tabSetting_pageCommand.SetLanguage(CommonData.Language);
			this.tabSetting_pageNote.SetLanguage(CommonData.Language);
			this.tabSetting_pageDisplay.SetLanguage(CommonData.Language);
			this.tabSetting_pageClipboard.SetLanguage(CommonData.Language);
		}
		
		void ApplyLanguageLog()
		{
			this.selectLogVisible.SetLanguage(CommonData.Language);
			this.selectLogAddShow.SetLanguage(CommonData.Language);
			this.selectLogFullDetail.SetLanguage(CommonData.Language);
			this.selectLogDebugging.SetLanguage(CommonData.Language);
			this.selectLogTrigger_information.Text = LogType.Information.ToText(CommonData.Language);
			this.selectLogTrigger_warning.Text = LogType.Warning.ToText(CommonData.Language);
			this.selectLogTrigger_error.Text = LogType.Error.ToText(CommonData.Language);
		}
		
		void ApplyLanguageSystemEnv()
		{
			this.inputSystemEnvExt.SetLanguage(CommonData.Language);
			this.inputSystemEnvHiddenFile.SetLanguage(CommonData.Language);

			this.labelSystemEnvExt.SetLanguage(CommonData.Language);
			this.labelSystemEnvHiddenFile.SetLanguage(CommonData.Language);
		}

		void ApplyLanguageSkin()
		{
			this.groupMainSkin.SetLanguage(CommonData.Language);
			this.commandSkinAbout.SetLanguage(CommonData.Language);
		}

		void ApplyLanguageStream()
		{
			this.groupStream.SetLanguage(CommonData.Language);
			this.commandStreamFont.SetLanguage(CommonData.Language);

			this.labelStreamFont.SetLanguage(CommonData.Language);
			this.labelStreamFore.SetLanguage(CommonData.Language);
			this.labelStreamBack.SetLanguage(CommonData.Language);
			this.labelStreamGeneral.SetLanguage(CommonData.Language);
			this.labelStreamInput.SetLanguage(CommonData.Language);
			this.labelStreamError.SetLanguage(CommonData.Language);

			UIUtility.ResizeAutoSize(this.groupStream, true);
		}

		void ApplyLanguageRunningInfo()
		{
			this.groupUpdateCheck.SetLanguage(CommonData.Language);
			this.selectUpdateCheck.SetLanguage(CommonData.Language);
			this.selectUpdateCheckRC.SetLanguage(CommonData.Language);
		}
		
		void ApplyLanguageMain()
		{
			this.groupMainLog.SetLanguage(CommonData.Language);
			this.groupMainSystemEnv.SetLanguage(CommonData.Language);
			this.labelMainLanguage.SetLanguage(CommonData.Language);
			this.selectMainStartup.SetLanguage(CommonData.Language);
			
			ApplyLanguageLog();
			ApplyLanguageSystemEnv();
			ApplyLanguageRunningInfo();
			ApplyLanguageSkin();
			ApplyLanguageStream();
		}
		
		void ApplyLanguageLauncher()
		{
			this.selecterLauncher.SetLanguage(CommonData.Language);
			this.envLauncherUpdate.SetLanguage(CommonData.Language);
			this.envLauncherRemove.SetLanguage(CommonData.Language);

			this.tabLauncher_pageCommon.SetLanguage(CommonData.Language);
			this.tabLauncher_pageEnv.SetLanguage(CommonData.Language);
			this.tabLauncher_pageOthers.SetLanguage(CommonData.Language);

			this.groupLauncherType.SetLanguage(CommonData.Language);
			this.selectLauncherType_file.Text = LauncherType.File.ToText(CommonData.Language);
			this.selectLauncherType_directory.Text = LauncherType.Directory.ToText(CommonData.Language);
			this.selectLauncherType_command.Text = LauncherType.Command.ToText(CommonData.Language);
			this.selectLauncherType_embedded.Text = LauncherType.Embedded.ToText(CommonData.Language);

			this.labelLauncherName.SetLanguage(CommonData.Language);
			this.labelLauncherCommand.SetLanguage(CommonData.Language);
			this.labelLauncherOption.SetLanguage(CommonData.Language);
			this.labelLauncherWorkDirPath.SetLanguage(CommonData.Language);
			this.labelLauncherIconPath.SetLanguage(CommonData.Language);

			this.selectLauncherEnv.SetLanguage(CommonData.Language);

			this.labelLauncherTag.SetLanguage(CommonData.Language);
			this.labelLauncherNote.SetLanguage(CommonData.Language);

			this.selectLauncherStdStream.SetLanguage(CommonData.Language);
			this.selectLauncherAdmin.SetLanguage(CommonData.Language);
		}
		
		void ApplyLanguageToolbar()
		{
			this.selecterToolbar.SetLanguage(CommonData.Language);
			this.commandToolbarFont.SetLanguage(CommonData.Language);
			this.commandToolbarScreens.SetLanguage(CommonData.Language);

			this.selectToolbarTopmost.SetLanguage(CommonData.Language);
			this.selectToolbarVisible.SetLanguage(CommonData.Language);
			this.selectToolbarAutoHide.SetLanguage(CommonData.Language);
			this.selectToolbarShowText.SetLanguage(CommonData.Language);
			this.labelToolbarGroup.SetLanguage(CommonData.Language);
			this.labelToolbarTextWidth.SetLanguage(CommonData.Language);
			this.labelToolbarPosition.SetLanguage(CommonData.Language);
			this.labelToolbarIcon.SetLanguage(CommonData.Language);
			this.labelToolbarFont.SetLanguage(CommonData.Language);

			this.toolToolbarGroup_addGroup.SetLanguage(CommonData.Language);
			this.toolToolbarGroup_addItem.SetLanguage(CommonData.Language);
			this.toolToolbarGroup_up.SetLanguage(CommonData.Language);
			this.toolToolbarGroup_down.SetLanguage(CommonData.Language);
			this.toolToolbarGroup_remove.SetLanguage(CommonData.Language);

			this.inputToolbarTextWidth.SetLanguage(CommonData.Language);
		}
		
		void ApplyLanguageCommand()
		{
			this.commandCommandFont.SetLanguage(CommonData.Language);
			this.inputCommandHotkey.SetLanguage(CommonData.Language);
			this.selectCommandFindTag.SetLanguage(CommonData.Language);
			this.selectCommandFindFile.SetLanguage(CommonData.Language);
			this.groupCommandFind.SetLanguage(CommonData.Language);
			this.groupCommandFind.Size = Size.Empty;

			this.labelCommandFont.SetLanguage(CommonData.Language);
			this.labelCommandIcon.SetLanguage(CommonData.Language);
			this.labelCommandHideTime.SetLanguage(CommonData.Language);
			this.labelCommandHotkey.SetLanguage(CommonData.Language);
		}
		
		void ApplyLanguageNote()
		{
			this.groupNoteKey.SetLanguage(CommonData.Language);

			this.inputNoteCreate.SetLanguage(CommonData.Language);
			this.inputNoteHidden.SetLanguage(CommonData.Language);
			this.inputNoteCompact.SetLanguage(CommonData.Language);
			this.inputNoteShowFront.SetLanguage(CommonData.Language);

			this.commandNoteCaptionFont.SetLanguage(CommonData.Language);

			this.groupNoteItem.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnRemove.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnId.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnVisible.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnLocked.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnBody.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnTitle.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnFont.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnFore.SetLanguage(CommonData.Language);
			this.gridNoteItems_columnBack.SetLanguage(CommonData.Language);

			this.labelNoteCreate.SetLanguage(CommonData.Language);
			this.labelNoteHiddent.SetLanguage(CommonData.Language);
			this.labelNoteCompact.SetLanguage(CommonData.Language);
			this.labelNoteShowFront.SetLanguage(CommonData.Language);
			this.labelNoteCaptionFont.SetLanguage(CommonData.Language);
		}
		
		void ApplyLanguageDisplay()
		{
			
		}

		void ApplyLanguageClipboard()
		{
			this.inputClipboardHotkey.SetLanguage(CommonData.Language);

			this.commandClipboardTextFont.SetLanguage(CommonData.Language);
			this.labelClipboardFont.SetLanguage(CommonData.Language);

			this.labelClipboardLimit.SetLanguage(CommonData.Language);
			this.labelClipboardWaitTaime.SetLanguage(CommonData.Language);
			//this.labelClipboardSleepTime.SetLanguage(Language);
			this.labelClipboardHotkey.SetLanguage(CommonData.Language);
			this.selectClipboardEnabled.SetLanguage(CommonData.Language);
			this.selectClipboardAppEnabled.SetLanguage(CommonData.Language);
			this.selectClipboardTopMost.SetLanguage(CommonData.Language);
			this.selectClipboardVisible.SetLanguage(CommonData.Language);
			this.selectClipboardItemWClickToOutput.SetLanguage(CommonData.Language);
			this.selectClipboardOutputUsingClipboard.SetLanguage(CommonData.Language);
			this.selectClipboardSave.SetLanguage(CommonData.Language);
			this.groupClipboardType.SetLanguage(CommonData.Language);
			this.groupClipboardSaveType.SetLanguage(CommonData.Language);
			this.groupClipboardOutput.SetLanguage(CommonData.Language);
			this.labelClipboardListType.SetLanguage(CommonData.Language);
			this.labelClipboardRepeated.SetLanguage(CommonData.Language, new Dictionary<string, string>() {
				{ ProgramLanguageName.clipboardRepeatedAll, Literal.clipboardRepeated.minimum.ToString("D") },
			});

			this.selectClipboardType_text.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.selectClipboardType_rtf.Text = ClipboardType.Rtf.ToText(CommonData.Language);
			this.selectClipboardType_html.Text = ClipboardType.Html.ToText(CommonData.Language);
			this.selectClipboardType_image.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.selectClipboardType_file.Text = ClipboardType.File.ToText(CommonData.Language);

			this.selectClipboardSaveType_text.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.selectClipboardSaveType_rtf.Text = ClipboardType.Rtf.ToText(CommonData.Language);
			this.selectClipboardSaveType_html.Text = ClipboardType.Html.ToText(CommonData.Language);
			this.selectClipboardSaveType_image.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.selectClipboardSaveType_file.Text = ClipboardType.File.ToText(CommonData.Language);

			this.inputClipboardLimit.SetLanguage(CommonData.Language);
			this.inputClipboardWaitTime.SetLanguage(CommonData.Language);
			this.inputClipboardRepeated.SetLanguage(CommonData.Language);
		}

		override protected void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			
			ApplyLanguageTab();
			ApplyLanguageMain();
			ApplyLanguageLauncher();
			ApplyLanguageToolbar();
			ApplyLanguageCommand();
			ApplyLanguageNote();
			ApplyLanguageDisplay();
			ApplyLanguageClipboard();
		}
		#endregion ////////////////////////////////////

		#region skin

		void ApplySkinLauncher()
		{
			this.selecterLauncher.SetSkin(CommonData.Skin);
			this.envLauncherUpdate.SetSkin(CommonData.Skin);
			this.envLauncherRemove.SetSkin(CommonData.Skin);

			this.toolToolbarGroup_addGroup.Image = CommonData.Skin.GetImage(SkinImage.Group);
			this.toolToolbarGroup_addItem.Image = CommonData.Skin.GetImage(SkinImage.AddItem);
			this.toolToolbarGroup_up.Image = CommonData.Skin.GetImage(SkinImage.Up);
			this.toolToolbarGroup_down.Image = CommonData.Skin.GetImage(SkinImage.Down);
			this.toolToolbarGroup_remove.Image = CommonData.Skin.GetImage(SkinImage.Remove);

			this.commandLauncherFilePath.Image = CommonData.Skin.GetImage(SkinImage.File);
			this.commandLauncherDirPath.Image = CommonData.Skin.GetImage(SkinImage.Dir);
			this.commandLauncherOptionFilePath.Image = CommonData.Skin.GetImage(SkinImage.File);
			this.commandLauncherOptionDirPath.Image = CommonData.Skin.GetImage(SkinImage.Dir);
			this.commandLauncherWorkDirPath.Image = CommonData.Skin.GetImage(SkinImage.Dir);
			this.commandLauncherIconPath.Image = CommonData.Skin.GetImage(SkinImage.File);
		}

		void ApplySkinToolbar()
		{
			this.selecterToolbar.SetSkin(CommonData.Skin);
		}

		protected override void ApplySkin()
		{
			ApplySkinLauncher();
			ApplySkinToolbar();
		}

		#endregion ////////////////////////////////////

		#region function
		void AttachmentIconScale(ComboBox control, IconScale defaultData)
		{
			var iconSizeDataList = new List<IconScaleDisplayValue>();
			//foreach(var value in new [] { IconScale.Small, IconScale.Normal, IconScale.Big, IconScale.Large }) {
			foreach(var value in new[] { IconScale.Small, IconScale.Normal, IconScale.Big }) {
				var data = new IconScaleDisplayValue(value);
				data.SetLanguage(CommonData.Language);
				iconSizeDataList.Add(data);
			}
			control.Attachment(iconSizeDataList, defaultData);
		}

		bool CheckValidate()
		{
			var checkResult = true;
			this.errorProvider.Clear();

			if(!LauncherItemValid()) {
				checkResult = false;
			}

			if(!NoteValid()) {
				checkResult = false;
			}

			return checkResult;
		}

		public void SaveFiles()
		{
			SaveFileMainStartup();
		}

		public void SaveDB()
		{
			using(var tran = CommonData.Database.BeginTransaction()) {
				try {
					SaveDBNoteItems(CommonData.Database);
					tran.Commit();
				} catch(Exception) {
					tran.Rollback();
					throw;
				}
			}
		}
		//void CreateSettingData()
		//{
		//	var mainSetting = new MainSetting();

		//	// 現在状況
		//	mainSetting.Running.Running = true;
		//	mainSetting.Running.SetDefaultVersion();

		//	// 本体
		//	ExportMainSetting(mainSetting);

		//	// ランチャ
		//	ExportLauncherSetting(mainSetting.Launcher);

		//	// コマンド
		//	ExportCommandSetting(mainSetting.Command);

		//	// ツールバー
		//	ExportToolbarSetting(mainSetting.Toolbar);

		//	// ノート
		//	ExportNoteSetting(mainSetting.Note);

		//	// ディスプレイ

		//	// クリップボード
		//	ExportClipboardSetting(mainSetting.Clipboard);

		//	// プロパティ設定
		//	MainSetting = mainSetting;
		//}

		void ExportUnbindData()
		{
			ExportLogShowTrigger(CommonData.MainSetting.Log);
			ExportSystemEnvSetting(CommonData.MainSetting.SystemEnvironment);
			ExportSkinSetting(CommonData.MainSetting.Skin);
			ExportLanguageSetting(CommonData.MainSetting);
			ExportLauncherSetting(CommonData.MainSetting.Launcher);
			ExportStreamSetting(CommonData.MainSetting.Stream);
			ExportNoteSetting(CommonData.MainSetting.Note);
			ExportToolbarSetting(CommonData.MainSetting.Toolbar);
			ExportClipboardSetting(CommonData.MainSetting.Clipboard);
			ExportCommandSetting(CommonData.MainSetting.Command);

			ExportToolbarGroup(CommonData.MainSetting.Toolbar);
		}


		/*
		string GetStartupAppLinkPath()
		{
			var startupDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			var appLinkPath = Path.Combine(startupDirPath, Literal.shortcutName);

			return appLinkPath;
		}
		*/


		void CloseScreenWindow(object sender, EventArgs e)
		{
			foreach(var w in OwnedForms.OfType<ScreenForm>().ToArray()) {
				w.Dispose();
			}
		}

		void KeyUpScreen(object sender, KeyEventArgs e)
		{
			// イベント強制
			CloseScreenWindow(sender, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="commonData"></param>
		public void ShowScreenWindow()
		{
			var pairs = Screen.AllScreens.Select(s => new { Screen = s, Window = new ScreenForm() }).ToList();
			foreach(var pair in pairs) {
				//pair.Window.SetCommonData(CommonData);
				pair.Window.SetLanguage(CommonData.Language);
				pair.Window.SetSkin(CommonData.Skin);
				pair.Window.Screen = pair.Screen;
				pair.Window.Click += CloseScreenWindow;
				// KeyDownでESCだと勢い余って設定画面が閉じるのであげたときのみ取得する
				pair.Window.KeyUp += KeyUpScreen;
			}

			foreach(var window in pairs.Select(p => p.Window)) {
				window.Show(this);
			}
			foreach(var window in pairs.Select(p => p.Window)) {
				window.Refresh();
			}
		}

		#endregion ////////////////////////////////////

		#region export
		//void ExportCommandSetting(CommandSetting commandSetting)
		//{
		//	/*
		//	commandSetting.HotKey.Key = this.inputCommandHotkey.Hotkey;
		//	commandSetting.HotKey.Modifiers = this.inputCommandHotkey.Modifiers;
		//	commandSetting.HotKey.Registered = this.inputCommandHotkey.Registered;
		//	 */
		//	commandSetting.HotKey = this.inputCommandHotkey.HotKeySetting;

		//	commandSetting.FontSetting = this.commandCommandFont.FontSetting;
		//}

		void ExportLauncherSetting(LauncherSetting setting)
		{
			setting.Items.Clear();
			foreach(var item in this.selecterLauncher.Items) {
				setting.Items.Add(item);
			}
		}

		//void ExportLogSetting(LogSetting logSetting)
		//{
		//	logSetting.Visible = this.selectLogVisible.Checked;
		//	logSetting.AddShow = this.selectLogAddShow.Checked;
		//	logSetting.FullDetail = this.selectLogFullDetail.Checked;
		//	logSetting.Debugging = this.selectLogDebugging.Checked;

		//	var trigger = new Dictionary<CheckBox, LogType>() {
		//		{ this.selectLogTrigger_information, LogType.Information },
		//		{ this.selectLogTrigger_warning,     LogType.Warning },
		//		{ this.selectLogTrigger_error,       LogType.Error },
		//	};
		//	var logType = LogType.None;
		//	foreach(var t in trigger) {
		//		if(t.Key.Checked) {
		//			logType |= t.Value;
		//		}
		//	}
		//	logSetting.AddShowTrigger = logType;
		//}
		void ExportLogShowTrigger(LogSetting setting)
		{
			var trigger = new Dictionary<CheckBox, LogType>() {
				{ this.selectLogTrigger_information, LogType.Information },
				{ this.selectLogTrigger_warning,     LogType.Warning },
				{ this.selectLogTrigger_error,       LogType.Error },
			};
			var logType = LogType.None;
			foreach(var t in trigger.Where(t => t.Key.Checked)) {
				logType |= t.Value;
			}

			setting.AddShowTrigger = logType;
		}

		void ExportSystemEnvSetting(SystemEnvironmentSetting setting)
		{
		//	/*
		//	systemEnvSetting.HiddenFileShowHotKey.Key = this.inputSystemEnvHiddenFile.Hotkey;
		//	systemEnvSetting.HiddenFileShowHotKey.Modifiers = this.inputSystemEnvHiddenFile.Modifiers;
		//	systemEnvSetting.HiddenFileShowHotKey.Registered = this.inputSystemEnvHiddenFile.Registered;
			
		//	systemEnvSetting.ExtensionShowHotKey.Key = this.inputSystemEnvExt.Hotkey;
		//	systemEnvSetting.ExtensionShowHotKey.Modifiers = this.inputSystemEnvExt.Modifiers;
		//	systemEnvSetting.ExtensionShowHotKey.Registered = this.inputSystemEnvExt.Registered;
		//	 */
			setting.HiddenFileShowHotKey = this.inputSystemEnvHiddenFile.HotKeySetting;
			setting.ExtensionShowHotKey = this.inputSystemEnvExt.HotKeySetting;
		}

		//void ExportRunningInfoSetting(RunningSetting setting)
		//{
		//	setting.CheckUpdate = this.selectUpdateCheck.Checked;
		//	setting.CheckUpdateRC = this.selectUpdateCheckRC.Checked;
		//}

		void ExportLanguageSetting(MainSetting setting)
		{
			var lang = this.selectMainLanguage.SelectedValue as Language;
			if(lang != null) {
				setting.LanguageName = lang.BaseName;
			}
		}

		void ExportSkinSetting(SkinSetting setting)
		{
			var skin = (ISkin)this.selectSkinName.SelectedValue;
			setting.Name = skin.About.Name;
		}

		void ExportStreamSetting(StreamSetting setting)
		{
			setting.FontSetting = this.commandStreamFont.FontSetting;

			setting.GeneralColor.Fore.Color = this.commnadStreamGeneralForeColor.Color;
			setting.GeneralColor.Back.Color = this.commnadStreamGeneralBackColor.Color;
			setting.InputColor.Fore.Color = this.commnadStreamInputForeColor.Color;
			setting.InputColor.Back.Color = this.commnadStreamInputBackColor.Color;
			setting.ErrorColor.Fore.Color = this.commnadStreamErrorForeColor.Color;
			setting.ErrorColor.Back.Color = this.commnadStreamErrorBackColor.Color;
		}

		//void ExportMainSetting(MainSetting mainSetting)
		//{
		//	ExportLogSetting(mainSetting.Log);
		//	ExportSystemEnvSetting(mainSetting.SystemEnvironment);
		//	ExportRunningInfoSetting(mainSetting.Running);

		//	ExportLanguageSetting(mainSetting);
		//	ExportSkinSetting(mainSetting.Skin);
		//	ExportStreamSetting(mainSetting.Stream);
		//}

		void ExportNoteSetting(NoteSetting noteSetting)
		{
			// ホットキー
			noteSetting.CreateHotKey = this.inputNoteCreate.HotKeySetting;
			noteSetting.HiddenHotKey = this.inputNoteHidden.HotKeySetting;
			noteSetting.CompactHotKey = this.inputNoteCompact.HotKeySetting;
			noteSetting.ShowFrontHotKey = this.inputNoteShowFront.HotKeySetting;

			// フォント
			noteSetting.CaptionFontSetting = this.commandNoteCaptionFont.FontSetting;
		}

		void ExportToolbarSetting(ToolbarSetting setting)
		{
			ToolbarSetSelectedItem(this._toolbarSelectedToolbarItem);
		}

		void ExportToolbarGroup(ToolbarSetting setting)
		{
			setting.ToolbarGroup = new ToolbarGroup();
			// ツリーからグループ項目構築
			foreach(TreeNode groupNode in this.treeToolbarItemGroup.Nodes) {
				var toolbarGroupItem = new ToolbarGroupItem();

				// グループ項目
				var groupName = groupNode.Text;
				toolbarGroupItem.Name = groupName;

				// グループに紐付くアイテム名
				toolbarGroupItem.ItemNames.AddRange(groupNode.Nodes.OfType<LauncherItemTreeNode>().Select(node => node.LauncherItem.Name));

				setting.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
		}


		void ExportClipboardSetting(ClipboardSetting setting)
		{
			//setting.Limit = (int)this.inputClipboardLimit.Value;
			setting.WaitTime = TimeSpan.FromMilliseconds((int)this.inputClipboardWaitTime.Value);
			//setting.SleepTime = TimeSpan.FromMilliseconds((int)this.inputClipboardSleepTime.Value);
			//setting.ClipboardRepeated = (int)this.inputClipboardRepeated.Value;

			//setting.Enabled = this.selectClipboardEnabled.Checked;
			//setting.EnabledApplicationCopy = this.selectClipboardAppEnabled.Checked;
			//setting.Visible = this.selectClipboardVisible.Checked;
			//setting.TopMost = this.selectClipboardTopMost.Checked;
			//setting.DoubleClickToOutput = this.selectClipboardItemWClickToOutput.Checked;
			//setting.OutputUsingClipboard = this.selectClipboardOutputUsingClipboard.Checked;

			setting.ClipboardListType = (ClipboardListType)this.selectClipboardListType.SelectedValue;

			var enabledTypeMap = new Dictionary<ClipboardType, bool>() {
				{ ClipboardType.Text, this.selectClipboardType_text.Checked },
				{ ClipboardType.Rtf,  this.selectClipboardType_rtf.Checked },
				{ ClipboardType.Html, this.selectClipboardType_html.Checked },
				{ ClipboardType.Image,this.selectClipboardType_image.Checked },
				{ ClipboardType.File, this.selectClipboardType_file.Checked },
			};
			var enabledClipboardTypes = ClipboardType.None;
			foreach(var type in enabledTypeMap.Where(p => p.Value).Select(p => p.Key)) {
				enabledClipboardTypes |= type;
			}
			setting.EnabledTypes = enabledClipboardTypes;

			var saveTypeMap = new Dictionary<ClipboardType, bool>() {
				{ ClipboardType.Text, this.selectClipboardSaveType_text.Checked },
				{ ClipboardType.Rtf,  this.selectClipboardSaveType_rtf.Checked },
				{ ClipboardType.Html, this.selectClipboardSaveType_html.Checked },
				{ ClipboardType.Image,this.selectClipboardSaveType_image.Checked },
				{ ClipboardType.File, this.selectClipboardSaveType_file.Checked },
			};
			var saveClipboardTypes = ClipboardType.None;
			foreach(var type in saveTypeMap.Where(p => p.Value).Select(p => p.Key)) {
				saveClipboardTypes |= type;
			}
			setting.SaveTypes = saveClipboardTypes;
			//setting.SaveHistory = this.selectClipboardSave.Checked;

			setting.ToggleHotKeySetting = this.inputClipboardHotkey.HotKeySetting;

			// フォント
			setting.TextFont = this.commandClipboardTextFont.FontSetting;

		}

		void ExportCommandSetting(CommandSetting setting)
		{
			setting.FontSetting = this.commandCommandFont.FontSetting;
			setting.HotKey = this.inputCommandHotkey.HotKeySetting;
			setting.IconScale = (IconScale)this.selectCommandIcon.SelectedValue;
			setting.HiddenTime = TimeSpan.FromMilliseconds((int)this.inputCommandHideTime.Value);
		}

		#endregion ////////////////////////////////////

		#region save

		void SaveFileMainStartup()
		{
			var linkPath = Literal.StartupShortcutPath;
			if(this.selectMainStartup.Checked) {
				if(!File.Exists(linkPath)) {
					// 生成
					AppUtility.MakeAppShortcut(linkPath);
				}
			} else {
				if(File.Exists(linkPath)) {
					// 削除
					File.Delete(linkPath);
				}
			}
		}

		void SaveDBNoteItems(AppDBManager db)
		{
			if(this._noteItemList.Count > 0) {
				var removeList = this._noteItemList.Where(note => note.Remove).Select(note => note.NoteItem);
				var saveList = this._noteItemList.Where(note => !note.Remove).Select(note => note.NoteItem);

				var noteDB = new NoteDB(db);
				noteDB.ToDisabled(removeList);
				noteDB.Resist(saveList);
			}
		}

		#endregion

		#region page
		#region page/main
		#endregion ////////////////////////////////////
		#region page/launcher
		LauncherType LauncherGetSelectedType()
		{
			var map = new Dictionary<RadioButton, LauncherType>() {
				{ this.selectLauncherType_file, LauncherType.File },
				{ this.selectLauncherType_directory, LauncherType.Directory },
				{ this.selectLauncherType_command, LauncherType.Command },
				{ this.selectLauncherType_embedded, LauncherType.Embedded },
			};
			return map.Single(m => m.Key.Checked).Value;
			/*
			if(this.selectLauncherType_file.Checked) {
				return LauncherType.File;
			} else {
				Debug.Assert(this.selectLauncherType_uri.Checked);
				return LauncherType.URI;
			}
			 */
		}
		
		void LauncherSetSelectedType(LauncherType type)
		{
			this.selectLauncherType_file.Checked = type == LauncherType.File;
			this.selectLauncherType_directory.Checked  = type == LauncherType.Directory;
			this.selectLauncherType_command.Checked = type == LauncherType.URI || type == LauncherType.Command;
			this.selectLauncherType_embedded.Checked  = type == LauncherType.Embedded;
			
			LauncherApplyType(type);
		}
		
		void LauncherInputClear()
		{
			this._launcherSelectedItem = null;
			this._launcherItemEvent = false;
			
			var textList = new Control[] {
				this.inputLauncherName,
				this.inputLauncherCommand,
				this.inputLauncherOption,
				this.inputLauncherWorkDirPath,
				this.inputLauncherIconPath,
				this.inputLauncherTag,
				this.inputLauncherNote,
			};
			foreach(var text in textList) {
				text.Text = string.Empty;
			}
			this.inputLauncherIconPath.IconIndex = 0;
			LauncherSetSelectedType(LauncherType.File);
			var checkList = new CheckBox[] {
				this.selectLauncherStdStream,
				this.selectLauncherAdmin,
			};
			foreach(var check in checkList) {
				check.Checked = false;
			}
			
			this.envLauncherUpdate.Clear();
			this.envLauncherRemove.Clear();
			
			this._launcherItemEvent = true;
		}
		
		void LauncherSelectItem(LauncherItem item)
		{
			LauncherInputClear();
			this._launcherItemEvent = false;
			
			this._launcherSelectedItem = item;
			
			LauncherSetSelectedType(item.LauncherType);
			this.inputLauncherName.Text = item.Name;

			this.inputLauncherCommand.DataSource = null;
			if(item.LauncherType == LauncherType.Embedded) {
				this.inputLauncherCommand.DropDownStyle = ComboBoxStyle.DropDownList;
				var displayValueList = CommonData.ApplicationSetting.Items
					.OrderBy(i => i.Name)
					.Select(i => new ApplicationDisplayValue(i))
					.ToArray()
				;
				foreach(var dv in displayValueList) {
					dv.SetLanguage(CommonData.Language);
				}
				var applicationItem = CommonData.ApplicationSetting.Items.SingleOrDefault(i => i.Name == item.Command);
				if(applicationItem != null){
					this.inputLauncherCommand.Attachment(displayValueList, applicationItem);
				} else {
					this.inputLauncherCommand.Attachment(displayValueList);
				} 
				//this.inputLauncherCommand.Text = item.Command;
			} else {
				this.inputLauncherCommand.DropDownStyle = ComboBoxStyle.DropDown;
				if(item.LauncherType == LauncherType.Command) {
					/*
					var commandList = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Text_CommandList
						.SplitLines()
						.Select(s => s.Trim())
						.Where(s => !string.IsNullOrWhiteSpace(s))
						.OrderBy(s => s)
						.ToArray()
					;
					this.inputLauncherCommand.DataSource = commandList;
					*/
					this.inputLauncherCommand.Items.AddRange(this._launcherCommandList);
				} else {
					this.inputLauncherCommand.Items.Clear();
				}

				this.inputLauncherCommand.Text = item.Command;
			}

			this.inputLauncherOption.Text = item.Option;
			this.inputLauncherWorkDirPath.Text = item.WorkDirPath;
			/*
			this.inputLauncherIconPath.Text = item.IconPath;
			this.inputLauncherIconPath.Tag = item.IconIndex;
			 */
			this.inputLauncherIconPath.Text = item.IconItem.Path;
			//this.inputLauncherIconPath.Tag = item.IconItem.Index;
			this.inputLauncherIconPath.IconIndex = item.IconItem.Index;
			
			this.inputLauncherTag.Text = string.Join(", ", item.Tag.ToArray());
			this.inputLauncherNote.Text = item.Note;
			this.selectLauncherStdStream.Checked = item.StdOutputWatch;
			this.selectLauncherAdmin.Checked = item.Administrator;
			this.selectLauncherEnv.Checked = !this.selectLauncherEnv.Checked;
			this.selectLauncherEnv.Checked = item.EnvironmentSetting.EditEnvironment;
			this.envLauncherUpdate.SetItem(item.EnvironmentSetting.Update.ToDictionary(pair => pair.First, pair => pair.Second));
			this.envLauncherRemove.SetItem(item.EnvironmentSetting.Remove);
			
			this._launcherItemEvent = true;
			
			//if(item.LauncherType == LauncherType.File) {
			//	this.selectLauncherAdmin.Enabled = true;
			//}
		}
		
		void LauncherInputValueToItem(LauncherItem item)
		{
			Debug.Assert(item != null);
			/*
			var oldIcon = new {
				Path = item.IconPath,
				Index= item.IconIndex
			};
			 */
			var oldIcon = new IconItem(item.IconItem.Path, item.IconItem.Index);
			item.LauncherType = LauncherGetSelectedType();
			//item.Name = this.inputLauncherName.Text.Trim();
			var name = this.inputLauncherName.Text.Trim();
			if(CommonData.MainSetting.Launcher.Items.Count > 1) {
				// 重複している場合はちょっと細工
				var uniqName = TextUtility.ToUniqueDefault(name, CommonData.MainSetting.Launcher.Items.Where(i => i != item).Select(i => i.Name));
				if(!item.IsNameEqual(uniqName)) {
					var prevEvent = this._launcherItemEvent;
					this._launcherItemEvent = false;
					this.inputLauncherName.Text = uniqName;
					this._launcherItemEvent = prevEvent;
					name = uniqName;
				}
			}
			item.Name = name;
			if(item.LauncherType == LauncherType.Embedded) {
				var applicationItem = this.inputLauncherCommand.SelectedValue as ApplicationItem;
				if(applicationItem != null) {
					item.Command = applicationItem.Name;
				} else {
					item.Command = CommonData.ApplicationSetting.Items.Single().Name;
				}
			} else {
				item.Command = this.inputLauncherCommand.Text.Trim();
			}
			item.Option = this.inputLauncherOption.Text.Trim();
			item.WorkDirPath = this.inputLauncherWorkDirPath.Text.Trim();
			/*
			item.IconPath = this.inputLauncherIconPath.Text.Trim();
			item.IconIndex = this.inputLauncherIconPath.Tag != null ? (int)this.inputLauncherIconPath.Tag: 0;
			 */
			item.IconItem.Path = this.inputLauncherIconPath.Text.Trim();
			item.IconItem.Index = this.inputLauncherIconPath.IconIndex;
			
			item.Tag = this.inputLauncherTag.Text.Split(',').Select(s => s.Trim()).ToList();
			item.Note = this.inputLauncherNote.Text.Trim();
			item.StdOutputWatch = this.selectLauncherStdStream.Checked;
			item.Administrator = this.selectLauncherAdmin.Checked;
			item.EnvironmentSetting.EditEnvironment = this.selectLauncherEnv.Checked;
			item.EnvironmentSetting.Update.Clear();
			item.EnvironmentSetting.Update.AddRange(this.envLauncherUpdate.Items);
			item.EnvironmentSetting.Remove.Clear();
			item.EnvironmentSetting.Remove.AddRange(this.envLauncherRemove.Items);

			item.HasError = this.selecterLauncher.Items.Where(i => i != item).Any(i => i.Equals(item));
			
			if(!oldIcon.Equals(item)) {
				item.ClearIcon();
			}
			
			LauncherApplyType(item.LauncherType);
			
			//if(item.LauncherType == LauncherType.File) {
			//	this.selectLauncherAdmin.Enabled = true;
			//}
		}
		
		void LauncherApplyType(LauncherType type)
		{
			var enabledControls = new Control [] {
				this.commandLauncherFilePath,
				this.commandLauncherDirPath,
				this.commandLauncherOptionDirPath,
				this.commandLauncherOptionFilePath,
				this.commandLauncherWorkDirPath,
				this.commandLauncherIconPath,
				this.inputLauncherName,
				this.inputLauncherCommand,
				this.inputLauncherOption,
				this.inputLauncherWorkDirPath,
				this.inputLauncherIconPath,
				this.inputLauncherTag,
				this.inputLauncherNote,
				this.selectLauncherStdStream,
				this.selectLauncherAdmin,
				this.selectLauncherEnv,
				this.envLauncherUpdate,
				this.envLauncherRemove,
			};
			IEnumerable<Control> disabledControls = null;
			switch(type) {
				case LauncherType.File:
					break;
					
				case LauncherType.Directory:
					{
						disabledControls = new Control[] {
							this.commandLauncherFilePath,
							this.commandLauncherOptionFilePath,
							this.commandLauncherOptionDirPath,
							this.commandLauncherWorkDirPath,
							this.inputLauncherOption,
							this.inputLauncherWorkDirPath,
							this.selectLauncherStdStream,
							this.selectLauncherAdmin,
							this.selectLauncherEnv,
							this.envLauncherUpdate,
							this.envLauncherRemove,
						};
					}
					break;

				case LauncherType.URI:
				case LauncherType.Command:
					disabledControls = new Control[] {
						this.selectLauncherAdmin,
					};
					break;
					
				case LauncherType.Embedded: 
					{
						disabledControls = new Control[] {
							this.commandLauncherFilePath,
							this.commandLauncherDirPath,
							this.commandLauncherOptionFilePath,
							this.commandLauncherOptionDirPath,
							this.commandLauncherWorkDirPath,
							this.inputLauncherOption,
							this.inputLauncherWorkDirPath,
							this.inputLauncherIconPath,
							this.selectLauncherStdStream,
							this.selectLauncherAdmin,
							this.selectLauncherEnv,
							this.envLauncherUpdate,
							this.envLauncherRemove,
							this.inputLauncherNote,
						};
					}
					break;
			}

			foreach(var control in enabledControls) {
				control.Enabled = true;
			}
			if(disabledControls != null) {
				foreach(var control in disabledControls) {
					control.Enabled = false;
				}
			}
		}
		
		bool LauncherItemValid()
		{
			if(!this.selecterLauncher.Items.Any(item => item.HasError)) {
				return true;
			} else {
				this.errorProvider.SetError(this.selecterLauncher, CommonData.Language["setting/check/item-name-dup"]);
				return false;
			}
		}
		
		void LauncherOpenIcon()
		{
			var iconPath = Environment.ExpandEnvironmentVariables(this.inputLauncherIconPath.Text.Trim());
			var iconIndex= this.inputLauncherIconPath.IconIndex;
			using(var dialog = new OpenIconDialog()) {
				if(iconPath.Length > 0 && File.Exists(iconPath)) {
					dialog.IconPath.Path  = iconPath;
					dialog.IconPath.Index = iconIndex;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					this.inputLauncherIconPath.Text = dialog.IconPath.Path;
					this.inputLauncherIconPath.IconIndex = dialog.IconPath.Index;
				}
			}
		}
		
		void LauncherAddFile(string filePath)
		{
			var checkPath = LauncherItemUtility.InquiryUseShocutTarget(filePath, CommonData.Language, new NullLogger());
			var useShortcut = checkPath == filePath;
			var path = checkPath;

			var item = LauncherItemUtility.LoadFile(path, useShortcut);
			var uniqueName = LauncherItemUtility.GetUniqueName(item, this.selecterLauncher.Items);
			item.Name = uniqueName;
			this.selecterLauncher.AddItem(item);
		}
		
		void LauncherInputChange()
		{
			if(this._launcherSelectedItem != null) {
				LauncherInputValueToItem(this._launcherSelectedItem);

				this.inputLauncherCommand.DataSource = null;

				this.selecterLauncher.Refresh();
			}
		}
		#endregion ////////////////////////////////////
		#region page/toolbar
		void ToolbarSelectingPage()
		{
			this.selecterToolbar.SetItems(this.selecterLauncher.Items, CommonData.ApplicationSetting);
			this._imageToolbarItemGroup.Images.Clear();
			var treeImage = new Dictionary<int, Image>() {
				{ TREE_TYPE_NONE, CommonData.Skin.GetImage(SkinImage.NotImpl) },
				{ TREE_TYPE_GROUP, CommonData.Skin.GetImage(SkinImage.Group) },
			};
			this._imageToolbarItemGroup.Images.AddRange(treeImage.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToArray());

			var seq = this.selecterLauncher.Items.Select(item => new { Name = item.Name, Icon = item.GetIcon(IconScale.Small, item.IconItem.Index, CommonData.ApplicationSetting, new NullLogger()) }).Where(item => item.Icon != null);
			foreach(var elemet in seq) {
				this._imageToolbarItemGroup.Images.Add(elemet.Name, elemet.Icon);
			}

			// 各ランチャーアイテムノードを更新
			foreach(var node in this.treeToolbarItemGroup.GetChildrenNodes().OfType<LauncherItemTreeNode>()) {
				node.Text = node.LauncherItem.Name;
				node.ImageKey = node.LauncherItem.Name;
			}

			// イメージリスト再設定のために一度null初期化
			this.treeToolbarItemGroup.ImageList = null;
			this.treeToolbarItemGroup.StateImageList = null;
			// イメージリスト再設定
			this.treeToolbarItemGroup.ImageList = this._imageToolbarItemGroup;
			this.treeToolbarItemGroup.StateImageList = this._imageToolbarItemGroup;
		}

		void ToolbarSetSelectedItem(ToolbarItem toolbarItem)
		{
			toolbarItem.ToolbarPosition = (ToolbarPosition)this.selectToolbarPosition.SelectedValue;
			toolbarItem.Topmost = this.selectToolbarTopmost.Checked;
			toolbarItem.AutoHide = this.selectToolbarAutoHide.Checked;
			toolbarItem.Visible = this.selectToolbarVisible.Checked;
			toolbarItem.ShowText = this.selectToolbarShowText.Checked;
			toolbarItem.TextWidth = (int)this.inputToolbarTextWidth.Value;
			toolbarItem.FontSetting = this.commandToolbarFont.FontSetting;

			toolbarItem.IconScale = (IconScale)this.selectToolbarIcon.SelectedValue;

			toolbarItem.DefaultGroup = this.selectToolbarGroup.SelectedValue as string;
		}

		void ToolbarSelectedChangeToolbarItem(ToolbarItem toolbarItem)
		{
			Debug.Assert(toolbarItem != null);
			//this._toolbarLocation = toolbarSetting.FloatLocation;
			//this._toolbarSize = toolbarSetting.FloatSize;

			this.selectToolbarPosition.SelectedValue = toolbarItem.ToolbarPosition;
			this.selectToolbarIcon.SelectedValue = toolbarItem.IconScale;
			this.commandToolbarFont.FontSetting.Import(toolbarItem.FontSetting);
			this.commandToolbarFont.RefreshView();

			this.inputToolbarTextWidth.Value = toolbarItem.TextWidth;

			// 各ON/OFF
			this.selectToolbarAutoHide.Checked = toolbarItem.AutoHide;
			this.selectToolbarVisible.Checked = toolbarItem.Visible;
			this.selectToolbarTopmost.Checked = toolbarItem.Topmost;
			this.selectToolbarShowText.Checked = toolbarItem.ShowText;

			this.selectToolbarGroup.SelectedValue = toolbarItem.DefaultGroup ?? string.Empty;

			this._toolbarSelectedToolbarItem = toolbarItem;
		}

		GroupItemTreeNode ToolbarAddGroup(string groupName)
		{
			var node = new GroupItemTreeNode();
			node.Text = TextUtility.ToUniqueDefault(groupName, this.treeToolbarItemGroup.Nodes.Cast<TreeNode>().Select(n => n.Text));
			node.ImageIndex = TREE_TYPE_GROUP;
			node.SelectedImageIndex = TREE_TYPE_GROUP;
			this.treeToolbarItemGroup.Nodes.Add(node);

			return node;
		}

		void ToolbarSetItem(LauncherItemTreeNode node, LauncherItem item)
		{
			Debug.Assert(node != null);
			Debug.Assert(item != null);

			node.Text = item.Name;
			//if(this._imageToolbarItemGroup.Images.ContainsKey(item.Name)) {
			node.ImageKey = item.Name;
			node.SelectedImageKey = item.Name;
			//} else {
			//	node.ImageIndex = TREE_TYPE_NONE;
			//	node.SelectedImageIndex = TREE_TYPE_NONE;
			//}
			//node.Tag = item;
			node.LauncherItem = item;
		}

		void ToolbarAddItem(GroupItemTreeNode parentNode, LauncherItem item)
		{
			Debug.Assert(parentNode != null);
			/*
			var items = this.selecterToolbar.Items;
			if(items != null && items.Count() > 0) {
				var item = this.selecterToolbar.SelectedItem;
				if(item == null) {
					item = items.First();
				}
				var node = new TreeNode();
				ToolbarSetItem(node, item);
				parentNode.Nodes.Add(node);
				if(!parentNode.IsExpanded) {
					parentNode.Expand();
				}
			}
			*/
			var node = new LauncherItemTreeNode();
			ToolbarSetItem(node, item);
			parentNode.Nodes.Add(node);
			if(!parentNode.IsExpanded) {
				parentNode.Expand();
			}
		}

		void ToolbarSelectedChangeGroupItem(LauncherItem item)
		{
			Debug.Assert(item != null);
			var showItem = this.selecterToolbar.ViewItems.Any(i => i == item);
			if(!showItem) {
				this.selecterToolbar.Filtering = false;
			}
			this.selecterToolbar.SelectedItem = item;
		}

		void ToolbarChangedGroupCount()
		{
			var toolbarItems = this.selectToolbarItem.Items.Cast<ToolbarDisplayValue>().Select(dv => dv.Value);
			var rootNode = treeToolbarItemGroup.Nodes.Cast<TreeNode>();
			var groupNames = rootNode.Select(n => n.Text);
			foreach(var toolbarItem in toolbarItems) {
				if(!groupNames.Any(g => ToolbarItem.CheckNameEqual(g, toolbarItem.DefaultGroup))) {
					toolbarItem.DefaultGroup = string.Empty;
				}
			}

			var groupList = new List<ToolbarGroupNameDisplayValue>();
			groupList.Add(new ToolbarGroupNameDisplayValue(string.Empty));
			foreach(var node in rootNode) {
				groupList.Add(new ToolbarGroupNameDisplayValue(node.Text));
			}

			this.selectToolbarGroup.Attachment(groupList);
			this.selectToolbarGroup.SelectedValue = _toolbarSelectedToolbarItem.DefaultGroup;
		}
		#endregion ////////////////////////////////////
		#region page/command
		#endregion ////////////////////////////////////
		#region page/note
		bool NoteValid()
		{
			return true;
		}
		#endregion ////////////////////////////////////
		#region page/clipboard
		#endregion ////////////////////////////////////
		#endregion ////////////////////////////////////

		void SelecterLauncher_SelectChangedItem(object sender, SelectedItemEventArg e)
		{
			if(this._launcherSelectedItem != null) {
				// 現在アイテムに入力内容を退避
				LauncherInputValueToItem(this._launcherSelectedItem);
			}
			if(e.Item == null) {
				// 未選択状態
				LauncherInputClear();
				this.splitContainer1.Panel2.Enabled = false; // NOTE: 暫定対応
				return;
			}
			if(e.Item == this._launcherSelectedItem) {
				// 現在選択中アイテム
				return;
			}
			this.splitContainer1.Panel2.Enabled = true; // NOTE: 暫定対応
			LauncherSelectItem(e.Item);
		}
		
		void SelecterLauncher_CreateItem(object sender, CreateItemEventArg e)
		{
			if(this._launcherSelectedItem != null) {
				// 現在アイテムに入力内容を退避
				LauncherInputValueToItem(this._launcherSelectedItem);
			}
			LauncherSelectItem(e.Item);
		}
		
		void TabSetting_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if(this._nowSelectedTabPage == this.tabSetting_pageLauncher) {
				e.Cancel = !LauncherItemValid();
			}
			if(!e.Cancel) {
				if(e.TabPage == this.tabSetting_pageToolbar) {
					ToolbarSelectingPage();
				}
				this._nowSelectedTabPage =  e.TabPage;
			}
		}
		
		void CommandLauncherFilePath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogWithFilePath(this.inputLauncherCommand);
		}
		
		void CommandLauncherDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogWithDirectoryPath(this.inputLauncherCommand);
		}
		
		void CommandLauncherWorkDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogWithDirectoryPath(this.inputLauncherWorkDirPath);
		}
		
		void CommandLauncherIconPath_Click(object sender, EventArgs e)
		{
			LauncherOpenIcon();
		}
		
		void CommandLauncherOptionFilePath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogWithFilePath(this.inputLauncherOption);
		}
		
		void CommandLauncherOptionDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogWithDirectoryPath(this.inputLauncherOption);
		}
		
		void ToolToolbarGroup_addGroup_Click(object sender, EventArgs e)
		{
			ToolbarAddGroup(CommonData.Language["new/group-item"]);
			ToolbarChangedGroupCount();
		}
		
		void ToolToolbarGroup_addItem_Click(object sender, EventArgs e)
		{
			var selectedNode = this.treeToolbarItemGroup.SelectedNode;
			if(selectedNode != null) {
				var parentNode = selectedNode as GroupItemTreeNode;
				if(parentNode == null) {
					parentNode = (GroupItemTreeNode)selectedNode.Parent;
				}
				
				var items = this.selecterToolbar.Items;
				if(items != null && items.Count() > 0) {
					var item = this.selecterToolbar.SelectedItem;
					if(item == null) {
						item = items.First();
					}
					ToolbarAddItem(parentNode, item);
				}
			}
		}
		
		void ToolToolbarGroup_up_Click(object sender, EventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node != null) {
				node.MoveToUp(true);
			}
		}
		
		void ToolToolbarGroup_down_Click(object sender, EventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node != null) {
				node.MoveToDown(true);
			}
		}
		
		void ToolToolbarGroup_remove_Click(object sender, EventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node != null) {
				node.Remove();
				ToolbarChangedGroupCount();
			}
		}
		
		void TreeToolbarItemGroup_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			var launcherItemNode = node as LauncherItemTreeNode;
			if(launcherItemNode != null) {
				ToolbarSelectedChangeGroupItem(launcherItemNode.LauncherItem);
			}
		}
		
		void SelecterToolbar_SelectChangedItem(object sender, SelectedItemEventArg e)
		{
			var item = this.selecterToolbar.SelectedItem;
			if(item != null) {
				var launcherItemNode = this.treeToolbarItemGroup.SelectedNode as LauncherItemTreeNode;
				if(launcherItemNode != null) {
					// 選択中ノードのランチャーアイテムを切り替える
					ToolbarSetItem(launcherItemNode, item);
				}
			}
		}

		private void selecterToolbar_ListDoubleClick(object sender, LauncherItemSelecterEventArgs e)
		{
			var item = this.selecterToolbar.SelectedItem;
			if(item != null) {
				var groupItemNode = this.treeToolbarItemGroup.SelectedNode as GroupItemTreeNode;
				if(groupItemNode != null) {
					// 選択中グループにランチャーアイテムを設定する
					ToolbarAddItem(groupItemNode, item);
				}
			}
		}

		void PageLauncher_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var datas = (string[])e.Data.GetData(DataFormats.FileDrop, false);
				if(datas.Length == 1) {
					e.Effect = DragDropEffects.Copy;
				} else {
					e.Effect = DragDropEffects.None;
				}
			} else {
				e.Effect = DragDropEffects.None;
			}
		}
		
		void PageLauncher_DragDrop(object sender, DragEventArgs e)
		{
			var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop, false)).First();
			LauncherAddFile(filePath);
		}
		
		
		void TreeToolbarItemGroup_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node == null) {
				// 到達不可のはず
				Debug.Assert(false);
				return;
			}
			
			//e.CancelEdit = node.Level != TREE_LEVEL_GROUP;
			e.CancelEdit = !(node is GroupItemTreeNode);
		}
		
		
		void TreeToolbarItemGroup_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.F2) {
				var node = this.treeToolbarItemGroup.SelectedNode;
				if(node != null) {
					node.BeginEdit();
				}
			}
		}
		
		void CommandSubmit_Click(object sender, System.EventArgs e)
		{
			if(CheckValidate()) {
				//// 設定データ生成
				//CreateSettingData();
				ExportUnbindData();

				DialogResult = DialogResult.OK;
			}
		}

		
		void InputLauncherName_TextChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void SelectLauncherType_file_CheckedChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
				LauncherSelectItem(this._launcherSelectedItem);
			}

			/*
			if(sender == this.selectLauncherEnv) {
				//this.panelEnv.Enabled = this.selectLauncherEnv.Checked;
				var enabled = this.selectLauncherEnv.Checked;
				this.envLauncherUpdate.Enabled = enabled;
				this.envLauncherRemove.Enabled = enabled;
			}
			 */
		}
		
		void InputLauncherIconIndex_ValueChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void SelectToolbarItem_SelectedValueChanged(object sender, EventArgs e)
		{
			var toolbarItem = this.selectToolbarItem.SelectedValue as ToolbarItem;
			if(this._toolbarSelectedToolbarItem != null && toolbarItem != null) {
				ToolbarSetSelectedItem(this._toolbarSelectedToolbarItem);
				ToolbarSelectedChangeToolbarItem(toolbarItem);
			}
		}
		
		void EnvLauncherUpdate_ValueChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void EnvLauncherRemove_ValueChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void GridNoteItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == this.gridNoteItems_columnFont.Index) {
				// フォント変更
				// TODO: ダイアログ表示を一元化する必要あり
				// ↑
				// 2015/01/15: なんのこっちゃ……
				using(var dialog = new FontDialog()) {
					var row = this._noteItemList[e.RowIndex];
					var fontSetting = row.Font;
					dialog.SetFontSetting(fontSetting);
					
					if(dialog.ShowDialog() == DialogResult.OK) {
						fontSetting.Import(dialog.Font);
					}
				}
			} else if(e.ColumnIndex == this.gridNoteItems_columnFore.Index || e.ColumnIndex == this.gridNoteItems_columnBack.Index) {
				// 前景色・背景色
				var row = this._noteItemList[e.RowIndex];
				var isFore = e.ColumnIndex == this.gridNoteItems_columnFore.Index;
				using(var dialog = new ColorDialog()) {
					if(dialog.ShowDialog() == DialogResult.OK) {
						var color = dialog.Color;
						if(isFore) {
							row.Fore = color;
						} else {
							row.Back = color;
						}
					}
				}
			}
			
		}
		
		void GridNoteItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.ColumnIndex == this.gridNoteItems_columnFont.Index) {
				// フォント
				var row = this._noteItemList[e.RowIndex];
				e.Value = LanguageUtility.FontSettingToDisplayText(CommonData.Language, row.Font);
				e.FormattingApplied = true;
			}
		}
		
		void selectLauncherEnv_CheckedChanged(object sender, EventArgs e)
		{
			var enabled = this.selectLauncherEnv.Checked;
			this.envLauncherUpdate.Enabled = enabled;
			this.envLauncherRemove.Enabled = enabled;
		}
		
		void inputLauncherIconPath_IconIndexChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}	
		}
		
		void treeToolbarItemGroup_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			//Debug.Assert(e.Node.Level == TREE_LEVEL_GROUP);
			if(e.Label == null) {
				// なんもしてない
				return;
			}
			// 編集されたのでラベル名が変更されているか
			if(e.Node.Text.Trim() == e.Label.Trim()) {
				e.CancelEdit = true;
				return;
			}
			
			// グループ名が空白は変更対象としない
			if(string.IsNullOrWhiteSpace(e.Label)) {
				e.CancelEdit = true;
				return;
			}
			
			var oldName = e.Node.Text;
			var nodes = this.treeToolbarItemGroup.Nodes.Cast<TreeNode>();
			//var changedNowSelectedToolbarItem = ToolbarItem.CheckNameEqual(this._toolbarSelectedToolbarItem.DefaultGroup, e.Node.Text);
			// グループ名重複は色々まずい
			var uniqName = TextUtility.ToUniqueDefault(e.Label.Trim(), nodes.Where(n => n != e.Node).Select(n => n.Text.Trim()));
			var useName = e.Label.Trim();
			if(uniqName != useName) {
				// 変更データを無視して採番値を設定
				e.CancelEdit = true;
				useName = e.Node.Text = uniqName;
			}
			
			
			// 変更値をツールバーの初期グループ名に反映
			var toolbarItems = this.selectToolbarItem.Items.Cast<ToolbarDisplayValue>().Select(dv => dv.Value);
			var groupList = new List<ToolbarGroupNameDisplayValue>();
			var changedGroupToolbarItem = toolbarItems.SingleOrDefault(t => t.DefaultGroup == oldName);
			if(changedGroupToolbarItem != null) {
				changedGroupToolbarItem.DefaultGroup = e.CancelEdit ? uniqName:  e.Label.Trim();
			}
			groupList.Add(new ToolbarGroupNameDisplayValue(string.Empty));
			foreach(var node in nodes) {
				if(node == e.Node) {
					groupList.Add(new ToolbarGroupNameDisplayValue(useName));
				} else {
					groupList.Add(new ToolbarGroupNameDisplayValue(node.Text));
				}
			}
			var nowIndex = this.selectToolbarGroup.SelectedIndex;
			this.selectToolbarGroup.Attachment(groupList);
			this.selectToolbarGroup.SelectedIndex = nowIndex;
		}

		private void selectSkinName_SelectedValueChanged(object sender, EventArgs e)
		{
			var skin = this.selectSkinName.SelectedValue as ISkin;
			if(skin != null) {
				this.commandSkinAbout.Enabled = skin.About.Setting;
			}
		}

		private void commandSkinAbout_Click(object sender, EventArgs e)
		{
			var skin = (ISkin)this.selectSkinName.SelectedValue;
			var about = skin.About;
			MessageBox.Show(about.Name);
		}

		private void commandToolbarScreens_Click(object sender, EventArgs e)
		{
			ShowScreenWindow();
		}

		private void treeToolbarItemGroup_ItemDrag(object sender, ItemDragEventArgs e)
		{
			this.treeToolbarItemGroup.SelectedNode = (TreeNode)e.Item;
			var data = new DataObject(ddTreeNode, e.Item);
			this.treeToolbarItemGroup.DoDragDrop(data, DragDropEffects.All);
		}

		private void treeToolbarItemGroup_DragOver(object sender, DragEventArgs e)
		{
			var treeNode = e.Data.GetData(ddTreeNode) as TreeNode;
			if(treeNode == null) {
				e.Effect = DragDropEffects.None;
				return;
			}
			var clientPoint = this.treeToolbarItemGroup.PointToClient(new Point(e.X, e.Y));
			var overNode = this.treeToolbarItemGroup.GetNodeAt(clientPoint);

			if(overNode == treeNode) {
				// 自分自身は無視
				e.Effect = DragDropEffects.None;
				return;
			}
			// 子を持つのであれば展開する
			if(overNode != null && overNode.Nodes.Count > 0 && !overNode.IsExpanded) {
				overNode.Expand();
			}

			if(treeNode is LauncherItemTreeNode) {
				// ランチャーアイテム
				if(overNode != null && overNode != treeNode.Parent) {
					e.Effect = DragDropEffects.Move;
				} else {
					e.Effect = DragDropEffects.None;
				}
			} else {
				// グループアイテム
				if(overNode == null) {
					e.Effect = DragDropEffects.Move;
				} else {
					if(overNode is GroupItemTreeNode) {
						e.Effect = DragDropEffects.Move;
					} else {
						// ランチャーアイテムノードには移動できない
						e.Effect = DragDropEffects.None;
					}
				}
			}
		}

		private void treeToolbarItemGroup_DragDrop(object sender, DragEventArgs e)
		{
			var treeNode = e.Data.GetData(ddTreeNode) as TreeNode;
			if(treeNode == null) {
				return;
			}
			var clientPoint = this.treeToolbarItemGroup.PointToClient(new Point(e.X, e.Y));
			var overNode = this.treeToolbarItemGroup.GetNodeAt(clientPoint);

			if(overNode == treeNode) {
				// 自分自身は無視
				return;
			}

			this.treeToolbarItemGroup.BeginUpdate();
			treeNode.Remove();
			if(treeNode is LauncherItemTreeNode) {
				// 指定ノードの下に移動
				Debug.Assert(overNode != null);
				Debug.Assert(overNode != treeNode.Parent);
				if(overNode is GroupItemTreeNode) {
					// グループの下
					overNode.Nodes.Add(treeNode);
				} else {
					// ランチャーアイテムの位置
					var groupNode = overNode.Parent;
					groupNode.Nodes.Insert(overNode.Index, treeNode);
				}
			} else {
				// グループアイテム
				if(overNode == null) {
					// 一番下に移動
					this.treeToolbarItemGroup.Nodes.Add(treeNode);
				} else {
					Debug.Assert(overNode is GroupItemTreeNode);
					// 指定グループの上に移動
					if(overNode.Index == 0) {
						this.treeToolbarItemGroup.Nodes.Insert(0, treeNode);
					} else {
						this.treeToolbarItemGroup.Nodes.Insert(overNode.Index, treeNode);
					}
				}
			}
			this.treeToolbarItemGroup.SelectedNode = treeNode;
			this.treeToolbarItemGroup.EndUpdate();
		}

		private void commnadStreamGeneralForeColor_Click(object sender, EventArgs e)
		{
			var button = (ColorButton)sender;
			using(var dialog = new ColorDialog()) {
				dialog.CustomColors = new [] { button.Color }
					.Select(c => ColorTranslator.ToWin32(c))
					.ToArray()
				;
				if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					button.Color = dialog.Color;
				}
			}
		}

	}
}
