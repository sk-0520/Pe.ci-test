using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.PeMain.Logic.DB;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// 設定。
	/// </summary>
	public partial class SettingForm : Form
	{
		#region define
		const int TREE_LEVEL_GROUP = 0;
		const int TREE_LEVEL_ITEM = 1;

		const int TREE_TYPE_NONE = 0;
		const int TREE_TYPE_GROUP = 1;

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
				get { return NoteItem.Style.ForeColor; }
				set { NoteItem.Style.ForeColor = value; }
			}
			public Color Back
			{
				get { return NoteItem.Style.BackColor; }
				set { NoteItem.Style.BackColor = value; }
			}
			#endregion
		}
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		HashSet<LauncherItem> _launcherItems = null;
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

		ApplicationSetting _applicationSetting;

		string[] _commandList;
		#endregion ////////////////////////////////////

		#region event
		#endregion ////////////////////////////////////

		public SettingForm(Language language, ISkin skin, MainSetting setting, AppDBManager db, ApplicationSetting applicationSetting)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			Initialize(language, skin, setting, db, applicationSetting);
		}

		#region property
		/// <summary>
		/// 使用言語データ
		/// </summary>
		public Language Language { get; private set; }
		public ISkin Skin { get; private set; }

		public MainSetting MainSetting { get; private set; }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		#endregion ////////////////////////////////////

		#region override
		#endregion ////////////////////////////////////

		#region initialize
		void InitializeLog(LogSetting logSetting)
		{
			this.selectLogVisible.Checked = logSetting.Visible;
			this.selectLogAddShow.Checked = logSetting.AddShow;
			this.selectLogFullDetail.Checked = logSetting.FullDetail;

			this.selectLogTrigger_information.Checked = (logSetting.AddShowTrigger & LogType.Information) == LogType.Information;
			this.selectLogTrigger_warning.Checked = (logSetting.AddShowTrigger & LogType.Warning) == LogType.Warning;
			this.selectLogTrigger_error.Checked = (logSetting.AddShowTrigger & LogType.Error) == LogType.Error;
		}

		void InitializeSystemEnv(SystemEnvSetting systemEnvSetting)
		{
			/*
			this.inputSystemEnvHiddenFile.Hotkey = systemEnvSetting.HiddenFileShowHotKey.Key;
			this.inputSystemEnvHiddenFile.Modifiers = systemEnvSetting.HiddenFileShowHotKey.Modifiers;
			this.inputSystemEnvHiddenFile.Registered = systemEnvSetting.HiddenFileShowHotKey.Registered;
			
			this.inputSystemEnvExt.Hotkey = systemEnvSetting.ExtensionShowHotKey.Key;
			this.inputSystemEnvExt.Modifiers = systemEnvSetting.ExtensionShowHotKey.Modifiers;
			this.inputSystemEnvExt.Registered = systemEnvSetting.ExtensionShowHotKey.Registered;
			 */
			this.inputSystemEnvHiddenFile.HotKeySetting = systemEnvSetting.HiddenFileShowHotKey;
			this.inputSystemEnvExt.HotKeySetting = systemEnvSetting.ExtensionShowHotKey;
		}

		void InitializeRunningInfo(RunningInfo setting)
		{
			this.selectUpdateCheck.Checked = setting.CheckUpdate;
			this.selectUpdateCheckRC.Checked = setting.CheckUpdateRC;
		}

		void InitializeSkin(SkinSetting setting)
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

		void InitializeLanguage(string languageName, Language language)
		{
			var langFileName = string.Format("{0}.xml", languageName);
			var languageFilePath = Path.Combine(Literal.ApplicationLanguageDirPath, langFileName);

			// TODO: 泥臭い
			var languageTempList = Directory.GetFiles(Literal.ApplicationLanguageDirPath, "*.xml")
				.Where(s => string.Compare(Path.GetFileName(s), string.Format("{0}.xml", Literal.defaultLanguage), true) != 0)
				.Select(
					f => new {
						Language = Serializer.LoadFile<Language>(f, false),
						BaseName = Path.GetFileNameWithoutExtension(f),
					}
				)
				.ToArray()
				;
			var languagePairList = new List<Language>(languageTempList.Length);
			foreach(var lang in languageTempList) {
				lang.Language.BaseName = lang.BaseName;
				languagePairList.Add(lang.Language);
			}
			var langList = languagePairList
				.Select(
					l => new {
						DisplayValue = new LanguageDisplayValue(l),
						Language = l,
					}
				)
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

		void InitializeMainSetting(MainSetting mainSetting)
		{
			var linkPath = Literal.StartupShortcutPath;
			this.selectMainStartup.Checked = File.Exists(linkPath);

			InitializeLog(mainSetting.Log);
			InitializeSystemEnv(mainSetting.SystemEnv);
			InitializeRunningInfo(mainSetting.RunningInfo);
			InitializeLanguage(mainSetting.LanguageName, Language);
			InitializeSkin(mainSetting.Skin);
		}

		void InitializeLauncher(LauncherSetting launcherSetting)
		{
			this._launcherItems.Clear();
			foreach(var item in launcherSetting.Items) {
				this._launcherItems.Add((LauncherItem)item.Clone());
			}
			this.selecterLauncher.SetItems(this._launcherItems, this._applicationSetting);
		}

		void InitializeCommand(CommandSetting commandSetting)
		{
			//this._commandFont = commandSetting.FontSetting;
			this.commandCommandFont.FontSetting.Import(commandSetting.FontSetting);
			this.commandCommandFont.RefreshView();

			// アイコンサイズ文言の項目構築
			AttachmentIconScale(this.selectCommandIcon, commandSetting.IconScale);

			// ホットキー
			/*
			this.inputCommandHotkey.Hotkey = commandSetting.HotKey.Key;
			this.inputCommandHotkey.Modifiers = commandSetting.HotKey.Modifiers;
			this.inputCommandHotkey.Registered = commandSetting.HotKey.Registered;
			 */
			this.inputCommandHotkey.HotKeySetting = commandSetting.HotKey;
		}

		void InitializeNote(NoteSetting noteSetting, AppDBManager db)
		{
			// ホットキー
			this.inputNoteCreate.HotKeySetting = noteSetting.CreateHotKey;
			this.inputNoteCompact.HotKeySetting = noteSetting.CompactHotKey;
			this.inputNoteHidden.HotKeySetting = noteSetting.HiddenHotKey;
			this.inputNoteShowFront.HotKeySetting = noteSetting.ShowFrontHotKey;

			this.commandNoteCaptionFont.FontSetting.Import(noteSetting.CaptionFontSetting);
			this.commandNoteCaptionFont.RefreshView();

			// 全リスト
			this.gridNoteItems.AutoGenerateColumns = false;
			var noteDB = new NoteDB(db);
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

			//			this.gridNoteItems.GetRowDisplayRectangle = noteList;
		}

		void InitializeToolbar(ToolbarSetting toolbarSetting)
		{
			//this.inputToolbarTextWidth.Minimum = Literal.toolbarTextWidth.minimum;
			//this.inputToolbarTextWidth.Maximum = Literal.toolbarTextWidth.maximum;
			this.inputToolbarTextWidth.SetRange(Literal.toolbarTextWidth);

			this.selecterToolbar.SetItems(this._launcherItems, this._applicationSetting);

			// ツールーバー位置の項目構築
			var toolbarPosList = new List<ToolbarPositionDisplayValue>();
			foreach(var value in new[] { ToolbarPosition.DesktopFloat, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopBottom, ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopRight, }) {
				var data = new ToolbarPositionDisplayValue(value);
				data.SetLanguage(Language);
				toolbarPosList.Add(data);
			}
			this.selectToolbarPosition.Attachment(toolbarPosList);

			// アイコンサイズ文言の項目構築
			AttachmentIconScale(this.selectToolbarIcon, IconScale.Small);

			ToolbarItem initToolbarItem = null;
			var toolbarItemDataList = new List<ToolbarDisplayValue>();
			foreach(var toolbarItem in toolbarSetting.Items) {
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
			foreach(var groupItem in toolbarSetting.ToolbarGroup.Groups) {
				var displayValue = new ToolbarGroupNameDisplayValue(groupItem.Name);
				toolbarGroupList.Add(displayValue);
			}
			this.selectToolbarGroup.Attachment(toolbarGroupList);

			ToolbarSelectedChangeToolbarItem(initToolbarItem);

			// グループ用項目
			this._imageToolbarItemGroup = new ImageList();
			this._imageToolbarItemGroup.ColorDepth = ColorDepth.Depth32Bit;

			// 各グループ構築
			foreach(var groupItem in toolbarSetting.ToolbarGroup.Groups) {
				// メイングループ
				var parentNode = ToolbarAddGroup(groupItem.Name);
				// メイングループに紐付くアイテム
				foreach(var itemName in groupItem.ItemNames) {
					var relItem = this._launcherItems.SingleOrDefault(item => item.IsNameEqual(itemName));
					if(relItem != null) {
						ToolbarAddItem(parentNode, relItem);
					}
				}
			}
		}

		void InitializeClipboard(ClipboardSetting setting)
		{
			this.inputClipboardLimit.SetValue(Literal.clipboardLimit, setting.Limit);
			this.inputClipboardWaitTime.SetValue(Literal.clipboardWaitTime, setting.WaitTime);
			this.inputClipboardSleepTime.SetValue(Literal.clipboardSleepTime, setting.SleepTime);

			this.selectClipboardEnabled.Checked = setting.Enabled;
			this.selectClipboardAppEnabled.Checked = setting.EnabledApplicationCopy;

			this.selectClipboardVisible.Checked = setting.Visible;
			this.selectClipboardTopMost.Checked = setting.TopMost;

			this.selectClipboardType_text.Checked = setting.EnabledTypes.HasFlag(ClipboardType.Text);
			this.selectClipboardType_rtf.Checked = setting.EnabledTypes.HasFlag(ClipboardType.Rtf);
			this.selectClipboardType_html.Checked = setting.EnabledTypes.HasFlag(ClipboardType.Html);
			this.selectClipboardType_image.Checked = setting.EnabledTypes.HasFlag(ClipboardType.Image);
			this.selectClipboardType_file.Checked = setting.EnabledTypes.HasFlag(ClipboardType.File);

			this.inputClipboardHotkey.HotKeySetting = setting.ToggleHotKeySetting;
		}

		void InitializeUI(MainSetting mainSetting, AppDBManager db)
		{
			ApplyLanguage();
			ApplySkin();

			InitializeMainSetting(mainSetting);
			InitializeLauncher(mainSetting.Launcher);
			InitializeToolbar(mainSetting.Toolbar);
			InitializeCommand(mainSetting.Command);
			InitializeNote(mainSetting.Note, db);
			InitializeClipboard(mainSetting.Clipboard);

#if RELEASE
			var debugPage = new [] { this.tabSetting_pageCommand, this.tabSetting_pageDisplay };
			foreach(var page in debugPage) {
				this.tabSetting.TabPages.Remove(page);
			}
#endif
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

			this._commandList = dirList
				.Where(Directory.Exists)
				.Select(s => Directory.EnumerateFiles(s, "*.exe"))
				.SelectMany(list => list)
				.Select(Path.GetFileNameWithoutExtension)
				.OrderBy(s => s)
				.Distinct()
				.ToArray()
			;
		}

		void Initialize(Language language, ISkin skin, MainSetting mainSetting, AppDBManager db, ApplicationSetting applicationSetting)
		{
			this._launcherItems = new HashSet<LauncherItem>();

			Language = language;
			Skin = skin;
			this._applicationSetting = applicationSetting;

			InitializeCommand();

			InitializeUI(mainSetting, db);
		}

		#endregion ////////////////////////////////////

		#region language

		void ApplyLanguageTab()
		{
			this.tabSetting_pageMain.SetLanguage(Language);
			this.tabSetting_pageLauncher.SetLanguage(Language);
			this.tabSetting_pageToolbar.SetLanguage(Language);
			this.tabSetting_pageCommand.SetLanguage(Language);
			this.tabSetting_pageNote.SetLanguage(Language);
			this.tabSetting_pageDisplay.SetLanguage(Language);
			this.tabSetting_pageClipboard.SetLanguage(Language);
		}
		
		void ApplyLanguageLog()
		{
			this.selectLogVisible.SetLanguage(Language);
			this.selectLogAddShow.SetLanguage(Language);
			this.selectLogFullDetail.SetLanguage(Language);
			this.selectLogTrigger_information.Text = LogType.Information.ToText(Language);
			this.selectLogTrigger_warning.Text = LogType.Warning.ToText(Language);
			this.selectLogTrigger_error.Text = LogType.Error.ToText(Language);
		}
		
		void ApplyLanguageSystemEnv()
		{
			this.inputSystemEnvExt.SetLanguage(Language);
			this.inputSystemEnvHiddenFile.SetLanguage(Language);
			
			this.labelSystemEnvExt.SetLanguage(Language);
			this.labelSystemEnvHiddenFile.SetLanguage(Language);
		}

		void ApplyLanguageSkin()
		{
			this.groupMainSkin.SetLanguage(Language);
			this.commandSkinAbout.SetLanguage(Language);
		}

		void ApplyLanguageRunningInfo()
		{
			this.groupUpdateCheck.SetLanguage(Language);
			this.selectUpdateCheck.SetLanguage(Language);
			this.selectUpdateCheckRC.SetLanguage(Language);
		}
		
		void ApplyLanguageMain()
		{
			this.groupMainLog.SetLanguage(Language);
			this.groupMainSystemEnv.SetLanguage(Language);
			this.labelMainLanguage.SetLanguage(Language);
			this.selectMainStartup.SetLanguage(Language);
			
			ApplyLanguageLog();
			ApplyLanguageSystemEnv();
			ApplyLanguageRunningInfo();
		}
		
		void ApplyLanguageLauncher()
		{
			this.selecterLauncher.SetLanguage(Language);
			this.envLauncherUpdate.SetLanguage(Language);
			this.envLauncherRemove.SetLanguage(Language);
			
			this.tabLauncher_pageCommon.SetLanguage(Language);
			this.tabLauncher_pageEnv.SetLanguage(Language);
			this.tabLauncher_pageOthers.SetLanguage(Language);
			
			this.groupLauncherType.SetLanguage(Language);
			this.selectLauncherType_file.Text = LauncherType.File.ToText(Language);
			this.selectLauncherType_directory.Text = LauncherType.Directory.ToText(Language);
			this.selectLauncherType_command.Text = LauncherType.Command.ToText(Language);
			this.selectLauncherType_embedded.Text = LauncherType.Embedded.ToText(Language);
			
			this.labelLauncherName.SetLanguage(Language);
			this.labelLauncherCommand.SetLanguage(Language);
			this.labelLauncherOption.SetLanguage(Language);
			this.labelLauncherWorkDirPath.SetLanguage(Language);
			this.labelLauncherIconPath.SetLanguage(Language);
			
			this.selectLauncherEnv.SetLanguage(Language);
			
			this.labelLauncherTag.SetLanguage(Language);
			this.labelLauncherNote.SetLanguage(Language);
			
			this.selectLauncherStdStream.SetLanguage(Language);
			this.selectLauncherAdmin.SetLanguage(Language);
		}
		
		void ApplyLanguageToolbar()
		{
			this.selecterToolbar.SetLanguage(Language);
			this.commandToolbarFont.SetLanguage(Language);
			
			this.selectToolbarTopmost.SetLanguage(Language);
			this.selectToolbarVisible.SetLanguage(Language);
			this.selectToolbarAutoHide.SetLanguage(Language);
			this.selectToolbarShowText.SetLanguage(Language);
			this.labelToolbarGroup.SetLanguage(Language);
			this.labelToolbarTextWidth.SetLanguage(Language);
			this.labelToolbarPosition.SetLanguage(Language);
			this.labelToolbarIcon.SetLanguage(Language);
			this.labelToolbarFont.SetLanguage(Language);
			
			this.toolToolbarGroup_addGroup.SetLanguage(Language);
			this.toolToolbarGroup_addItem.SetLanguage(Language);
			this.toolToolbarGroup_up.SetLanguage(Language);
			this.toolToolbarGroup_down.SetLanguage(Language);
			this.toolToolbarGroup_remove.SetLanguage(Language);

		}
		
		void ApplyLanguageCommand()
		{
			this.commandCommandFont.SetLanguage(Language);
			this.inputCommandHotkey.SetLanguage(Language);
			
			this.selectCommandTopmost.SetLanguage(Language);
			this.labelCommandFont.SetLanguage(Language);
			this.labelCommandIcon.SetLanguage(Language);
		}
		
		void ApplyLanguageNote()
		{
			this.groupNoteKey.SetLanguage(Language);
			
			this.inputNoteCreate.SetLanguage(Language);
			this.inputNoteHidden.SetLanguage(Language);
			this.inputNoteCompact.SetLanguage(Language);
			this.inputNoteShowFront.SetLanguage(Language);
			
			this.commandNoteCaptionFont.SetLanguage(Language);

			this.groupNoteItem.SetLanguage(Language);
			this.gridNoteItems_columnRemove.SetLanguage(Language);
			this.gridNoteItems_columnId.SetLanguage(Language);
			this.gridNoteItems_columnVisible.SetLanguage(Language);
			this.gridNoteItems_columnLocked.SetLanguage(Language);
			this.gridNoteItems_columnBody.SetLanguage(Language);
			this.gridNoteItems_columnTitle.SetLanguage(Language);
			this.gridNoteItems_columnFont.SetLanguage(Language);
			this.gridNoteItems_columnFore.SetLanguage(Language);
			this.gridNoteItems_columnBack.SetLanguage(Language);
			
			this.labelNoteCreate.SetLanguage(Language);
			this.labelNoteHiddent.SetLanguage(Language);
			this.labelNoteCompact.SetLanguage(Language);
			this.labelNoteShowFront.SetLanguage(Language);
			this.labelNoteCaptionFont.SetLanguage(Language);
		}
		
		void ApplyLanguageDisplay()
		{
			
		}

		void ApplyLanguageClipboard()
		{
			this.inputClipboardHotkey.SetLanguage(Language);

			this.labelClipboardLimit.SetLanguage(Language);
			this.labelClipboardWaitTaime.SetLanguage(Language);
			this.labelClipboardSleepTime.SetLanguage(Language);
			this.labelClipboardHotkey.SetLanguage(Language);
			this.selectClipboardEnabled.SetLanguage(Language);
			this.selectClipboardAppEnabled.SetLanguage(Language);
			this.selectClipboardTopMost.SetLanguage(Language);
			this.selectClipboardVisible.SetLanguage(Language);
			this.groupClipboardType.SetLanguage(Language);
			this.selectClipboardType_text.Text = ClipboardType.Text.ToText(Language);
			this.selectClipboardType_rtf.Text = ClipboardType.Rtf.ToText(Language);
			this.selectClipboardType_html.Text = ClipboardType.Html.ToText(Language);
			this.selectClipboardType_image.Text = ClipboardType.Image.ToText(Language);
			this.selectClipboardType_file.Text = ClipboardType.File.ToText(Language);
		}

		void ApplyLanguage()
		{
			Debug.Assert(Language != null);
			
			UIUtility.SetDefaultText(this, Language);
			
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
			this.selecterLauncher.SetSkin(Skin);
			this.envLauncherUpdate.SetSkin(Skin);
			this.envLauncherRemove.SetSkin(Skin);

			toolToolbarGroup_addGroup.Image = Skin.GetImage(SkinImage.Group);
			toolToolbarGroup_addItem.Image = Skin.GetImage(SkinImage.AddItem);
			toolToolbarGroup_up.Image = Skin.GetImage(SkinImage.Up);
			toolToolbarGroup_down.Image = Skin.GetImage(SkinImage.Down);
			toolToolbarGroup_remove.Image = Skin.GetImage(SkinImage.Remove);
		}

		void ApplySkinToolbar()
		{
			this.selecterToolbar.SetSkin(Skin);
		}

		void ApplySkin()
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
				data.SetLanguage(Language);
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

		public void SaveDB(AppDBManager db)
		{
			using(var tran = db.BeginTransaction()) {
				try {
					SaveDBNoteItems(db);
					tran.Commit();
				} catch(Exception) {
					tran.Rollback();
					throw;
				}
			}
		}
		void CreateSettingData()
		{
			var mainSetting = new MainSetting();

			// 現在状況
			mainSetting.RunningInfo.Running = true;
			mainSetting.RunningInfo.SetDefaultVersion();

			// 本体
			ExportMainSetting(mainSetting);

			// ランチャ
			ExportLauncherSetting(mainSetting.Launcher);

			// コマンド
			ExportCommandSetting(mainSetting.Command);

			// ツールバー
			ExportToolbarSetting(mainSetting.Toolbar);

			// ノート
			ExportNoteSetting(mainSetting.Note);

			// ディスプレイ

			// クリップボード
			ExportClipboardSetting(mainSetting.Clipboard);

			// プロパティ設定
			MainSetting = mainSetting;
		}

		/*
		string GetStartupAppLinkPath()
		{
			var startupDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			var appLinkPath = Path.Combine(startupDirPath, Literal.shortcutName);

			return appLinkPath;
		}
		*/
		#endregion ////////////////////////////////////

		#region export
		void ExportCommandSetting(CommandSetting commandSetting)
		{
			/*
			commandSetting.HotKey.Key = this.inputCommandHotkey.Hotkey;
			commandSetting.HotKey.Modifiers = this.inputCommandHotkey.Modifiers;
			commandSetting.HotKey.Registered = this.inputCommandHotkey.Registered;
			 */
			commandSetting.HotKey = this.inputCommandHotkey.HotKeySetting;

			commandSetting.FontSetting = this.commandCommandFont.FontSetting;
		}

		void ExportLauncherSetting(LauncherSetting setting)
		{
			setting.Items.Clear();
			foreach(var item in this.selecterLauncher.Items) {
				setting.Items.Add(item);
			}
		}

		void ExportLogSetting(LogSetting logSetting)
		{
			logSetting.Visible = this.selectLogVisible.Checked;
			logSetting.AddShow = this.selectLogAddShow.Checked;
			logSetting.FullDetail = this.selectLogFullDetail.Checked;

			var trigger = new Dictionary<CheckBox, LogType>() {
				{ this.selectLogTrigger_information, LogType.Information },
				{ this.selectLogTrigger_warning,     LogType.Warning },
				{ this.selectLogTrigger_error,       LogType.Error },
			};
			var logType = LogType.None;
			foreach(var t in trigger) {
				if(t.Key.Checked) {
					logType |= t.Value;
				}
			}
			logSetting.AddShowTrigger = logType;
		}

		void ExportSystemEnvSetting(SystemEnvSetting systemEnvSetting)
		{
			/*
			systemEnvSetting.HiddenFileShowHotKey.Key = this.inputSystemEnvHiddenFile.Hotkey;
			systemEnvSetting.HiddenFileShowHotKey.Modifiers = this.inputSystemEnvHiddenFile.Modifiers;
			systemEnvSetting.HiddenFileShowHotKey.Registered = this.inputSystemEnvHiddenFile.Registered;
			
			systemEnvSetting.ExtensionShowHotKey.Key = this.inputSystemEnvExt.Hotkey;
			systemEnvSetting.ExtensionShowHotKey.Modifiers = this.inputSystemEnvExt.Modifiers;
			systemEnvSetting.ExtensionShowHotKey.Registered = this.inputSystemEnvExt.Registered;
			 */
			systemEnvSetting.HiddenFileShowHotKey = this.inputSystemEnvHiddenFile.HotKeySetting;
			systemEnvSetting.ExtensionShowHotKey = this.inputSystemEnvExt.HotKeySetting;
		}

		void ExportRunningInfoSetting(RunningInfo setting)
		{
			setting.CheckUpdate = this.selectUpdateCheck.Checked;
			setting.CheckUpdateRC = this.selectUpdateCheckRC.Checked;
		}

		void ExportLanguageSetting(MainSetting setting)
		{
			var lang = this.selectMainLanguage.SelectedValue as Language;
			if(lang != null) {
				setting.LanguageName = lang.BaseName;
			}
		}

		void ExportMainSetting(MainSetting mainSetting)
		{
			ExportLogSetting(mainSetting.Log);
			ExportSystemEnvSetting(mainSetting.SystemEnv);
			ExportRunningInfoSetting(mainSetting.RunningInfo);

			ExportLanguageSetting(mainSetting);
		}

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

		void ExportToolbarSetting(ToolbarSetting toolbarSetting)
		{
			ToolbarSetSelectedItem(this._toolbarSelectedToolbarItem);
			foreach(var itemData in this.selectToolbarItem.Items.Cast<ToolbarDisplayValue>()) {
				var item = itemData.Value;
				if(toolbarSetting.Items.Contains(item)) {
					toolbarSetting.Items.Remove(item);
				}
				toolbarSetting.Items.Add(item);
			}

			// ツリーからグループ項目構築
			foreach(TreeNode groupNode in this.treeToolbarItemGroup.Nodes) {
				var toolbarGroupItem = new ToolbarGroupItem();

				// グループ項目
				var groupName = groupNode.Text;
				toolbarGroupItem.Name = groupName;

				// グループに紐付くアイテム名
				toolbarGroupItem.ItemNames.AddRange(groupNode.Nodes.Cast<TreeNode>().Select(node => node.Text));

				toolbarSetting.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
		}


		void ExportClipboardSetting(ClipboardSetting setting)
		{
			setting.Limit = (int)this.inputClipboardLimit.Value;
			setting.WaitTime = TimeSpan.FromMilliseconds((int)this.inputClipboardWaitTime.Value);
			setting.SleepTime = TimeSpan.FromMilliseconds((int)this.inputClipboardSleepTime.Value);

			setting.Enabled = this.selectClipboardEnabled.Checked;
			setting.EnabledApplicationCopy = this.selectClipboardAppEnabled.Checked;
			setting.Visible = this.selectClipboardVisible.Checked;
			setting.TopMost = this.selectClipboardTopMost.Checked;

			var map = new Dictionary<ClipboardType, bool>() {
				{ ClipboardType.Text, this.selectClipboardType_text.Checked },
				{ ClipboardType.Rtf,  this.selectClipboardType_rtf.Checked },
				{ ClipboardType.Html, this.selectClipboardType_html.Checked },
				{ ClipboardType.Image,this.selectClipboardType_image.Checked },
				{ ClipboardType.File, this.selectClipboardType_file.Checked },
			};
			var clipboardType = ClipboardType.None;
			foreach(var type in map.Where(p => p.Value).Select(p => p.Key)) {
				clipboardType |= type;
			}
			setting.EnabledTypes = clipboardType;

			setting.ToggleHotKeySetting = this.inputClipboardHotkey.HotKeySetting;
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
				var displayValueList = _applicationSetting.Items
					.OrderBy(i => i.Name)
					.Select(i => new ApplicationDisplayValue(i))
					.ToArray()
				;
				foreach(var dv in displayValueList) {
					dv.SetLanguage(Language);
				}
				var applicationItem = this._applicationSetting.Items.SingleOrDefault(i => i.Name == item.Command);
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
					this.inputLauncherCommand.Items.AddRange(this._commandList);
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
			
			if(item.LauncherType == LauncherType.File) {
				this.selectLauncherAdmin.Enabled = true;
			}
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
			item.Name = this.inputLauncherName.Text.Trim();
			if(item.LauncherType == LauncherType.Embedded) {
				var applicationItem = this.inputLauncherCommand.SelectedValue as ApplicationItem;
				if(applicationItem != null) {
					item.Command = applicationItem.Name;
				} else {
					item.Command = this._applicationSetting.Items.Single().Name;
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
			
			item.Tag = this.inputLauncherTag.Text.Split(',').Map(s => s.Trim()).ToList();
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
			
			if(item.LauncherType == LauncherType.File) {
				this.selectLauncherAdmin.Enabled = true;
			}
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
			var disabledControls = new Control[]{};
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
					{
						disabledControls = new Control[] {
							this.commandLauncherFilePath,
							this.commandLauncherDirPath,
							this.commandLauncherOptionFilePath,
							this.commandLauncherOptionDirPath,
							this.commandLauncherWorkDirPath,
							//this.inputLauncherOption,
							this.inputLauncherWorkDirPath,
							this.selectLauncherStdStream,
							this.selectLauncherAdmin,
							this.selectLauncherEnv,
							this.envLauncherUpdate,
							this.envLauncherRemove,
						};
					}
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
				this.errorProvider.SetError(this.selecterLauncher, Language["setting/check/item-name-dup"]);
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
			var path = filePath;
			var useShortcut = false;
			// TODO: 処理重複
			if(PathUtility.IsShortcutPath(filePath)) {
				var result = MessageBox.Show(Language["common/dialog/d-d/shortcut/message"], Language["common/dialog/d-d/shortcut/caption"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				switch(result) {
					case DialogResult.Yes:
						try {
							var sf = new ShortcutFile(filePath, false);
							path = sf.TargetPath;
						} catch(ArgumentException ex) {
							Debug.WriteLine(ex);
						}
						break;
						
					case DialogResult.No:
						useShortcut = true;
						break;
						
					default:
						return;
				}
			}
			var item = LauncherItem.LoadFile(path, useShortcut);
			var uniqueName = LauncherItem.GetUniqueName(item, this.selecterLauncher.Items);
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
			this.selecterToolbar.SetItems(this.selecterLauncher.Items, this._applicationSetting);
			this._imageToolbarItemGroup.Images.Clear();
			var treeImage = new Dictionary<int, Image>() {
				{ TREE_TYPE_NONE, Skin.GetImage(SkinImage.NotImpl) },
				{ TREE_TYPE_GROUP, Skin.GetImage(SkinImage.Group) },
			};
			this._imageToolbarItemGroup.Images.AddRange(treeImage.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToArray());

			var seq = this.selecterLauncher.Items.Select(item => new { Name = item.Name, Icon = item.GetIcon(IconScale.Small, item.IconItem.Index, this._applicationSetting) }).Where(item => item.Icon != null);
			foreach(var elemet in seq) {
				this._imageToolbarItemGroup.Images.Add(elemet.Name, elemet.Icon);
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

		TreeNode ToolbarAddGroup(string groupName)
		{
			var node = new TreeNode();
			node.Text = TextUtility.ToUnique(groupName, this.treeToolbarItemGroup.Nodes.Cast<TreeNode>().Select(n => n.Text), null);
			node.ImageIndex = TREE_TYPE_GROUP;
			node.SelectedImageIndex = TREE_TYPE_GROUP;
			this.treeToolbarItemGroup.Nodes.Add(node);

			return node;
		}

		void ToolbarSetItem(TreeNode node, LauncherItem item)
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
			node.Tag = item;
		}

		void ToolbarAddItem(TreeNode parentNode, LauncherItem item)
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
			var node = new TreeNode();
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

		void SelecterLauncher_SelectChnagedItem(object sender, SelectedItemEventArg e)
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
			DialogUtility.OpenDialogFilePath(this.inputLauncherCommand);
		}
		
		void CommandLauncherDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputLauncherCommand);
		}
		
		void CommandLauncherWorkDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputLauncherWorkDirPath);
		}
		
		void CommandLauncherIconPath_Click(object sender, EventArgs e)
		{
			LauncherOpenIcon();
		}
		
		void CommandLauncherOptionFilePath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogFilePath(this.inputLauncherOption);
		}
		
		void CommandLauncherOptionDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputLauncherOption);
		}
		
		void ToolToolbarGroup_addGroup_Click(object sender, EventArgs e)
		{
			ToolbarAddGroup(Language["group/new"]);
			ToolbarChangedGroupCount();
		}
		
		void ToolToolbarGroup_addItem_Click(object sender, EventArgs e)
		{
			var selectedNode = this.treeToolbarItemGroup.SelectedNode;
			if(selectedNode != null) {
				var parentNode = selectedNode;
				if(selectedNode.Level == TREE_LEVEL_ITEM) {
					parentNode = selectedNode.Parent;
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
			if(node.Level == TREE_LEVEL_ITEM) {
				ToolbarSelectedChangeGroupItem((LauncherItem)node.Tag);
			}
		}
		
		void SelecterToolbar_SelectChangedItem(object sender, SelectedItemEventArg e)
		{
			var item = this.selecterToolbar.SelectedItem;
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(item != null && node != null && node.Level == TREE_LEVEL_ITEM) {
				ToolbarSetItem(node, item);
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
			e.CancelEdit = node.Level != TREE_LEVEL_GROUP;
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
				// 設定データ生成
				CreateSettingData();
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
				using(var dialog = new FontDialog()) {
					var row = this._noteItemList[e.RowIndex];
					var fontSetting = row.Font;
					if(fontSetting.IsDefault) {
						dialog.Font = fontSetting.Font;
					}
					
					if(dialog.ShowDialog() == DialogResult.OK) {
						var result = new FontSetting();
						var font = dialog.Font;
						fontSetting.Family = font.FontFamily.Name;
						fontSetting.Height = font.Size;
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
				e.Value = LanguageUtility.FontSettingToDisplayText(Language, row.Font);
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
			Debug.Assert(e.Node.Level == TREE_LEVEL_GROUP);
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
	}
}
