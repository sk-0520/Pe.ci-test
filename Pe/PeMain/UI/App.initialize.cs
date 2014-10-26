/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:47
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Win32;
using PeMain.Data;
using PeMain.Data.DB;
using PeMain.Logic;
using PeMain.Logic.DB;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_initialize.
	/// </summary>
	partial class App
	{
		void InitializeLanguage(CommandLine commandLine, StartupLogger logger)
		{
			// 言語
			var langName = this._commonData.MainSetting.LanguageName;
			if(string.IsNullOrEmpty(langName)) {
				langName = CultureInfo.CurrentCulture.Name;
			}
			var languageFileName = string.Format("{0}.xml", langName);
			var languageFilePath = Path.Combine(Literal.ApplicationLanguageDirPath, languageFileName);
			if(logger != null) {
				logger.Puts(LogType.Information, "load language", languageFilePath);
			}
			this._commonData.Language = Serializer.LoadFile<Language>(languageFilePath, false);
			if(this._commonData.Language == null) {
				if(logger != null) {
					logger.Puts(LogType.Warning, "not found language", languageFilePath);
				}
				this._commonData.Language = new Language();
			}
			this._commonData.Language.BaseName = langName;
		}
		
		void InitializeRunningInfo(CommandLine commandLine, StartupLogger logger)
		{
			var prev = new {
				VersionMajor = this._commonData.MainSetting.RunningInfo.VersionMajor,
				VersionMinor = this._commonData.MainSetting.RunningInfo.VersionMinor,
				VersionRevision = this._commonData.MainSetting.RunningInfo.VersionRevision,
				VersionBuild = this._commonData.MainSetting.RunningInfo.VersionBuild,
			};
			this._commonData.MainSetting.RunningInfo.SetDefaultVersion();
			var prevVersion = new Tuple<ushort,ushort,ushort>(prev.VersionMajor, prev.VersionMinor, prev.VersionRevision);
			// バージョンが一定未満なら強制的に使用承諾
			if(Functions.VersionCheck(prevVersion, Literal.AcceptVersion) < 0) {
				this._commonData.MainSetting.RunningInfo.Running = false;
			}
		}
		
		void InitializeNoteTableCreate(string tableName, StartupLogger logger)
		{
			var map = new Dictionary<string, string>() {
				{ DataTables.masterTableNote,           global::PeMain.Properties.SQL.CreateNoteMasterTable },
				{ DataTables.transactionTableNote,      global::PeMain.Properties.SQL.CreateNoteTransactionTable },
				{ DataTables.transactionTableNoteStyle, global::PeMain.Properties.SQL.CreateNoteStyleTransactionTable },
			};
			var langMap = new Dictionary<string, string>() {
				{ "TABLE-NAME", tableName },
			};
			
			var command = map[tableName];
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/create", langMap] , command);
			this._commonData.Database.ExecuteCommand(command);

			var entity = new MVersionEntity();
			entity.Name = tableName;
			entity.Version = DataTables.map[tableName];
			this._commonData.Database.ExecuteInsert(new [] { entity });
		}
		
		
		/// <summary>
		/// NOTE: 将来的な予約
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="version"></param>
		/// <param name="initLog"></param>
		void InitializeNoteTableChange(string tableName, int version, StartupLogger logger)
		{
			var langMap = new Dictionary<string, string>() {
				{ "TABLE-NAME", tableName },
			};
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/check", langMap] , new { TableName = tableName, Version = version, });
			
			var map = new Dictionary<string, string>() {
				{ DataTables.masterTableNote,           string.Empty },
				{ DataTables.transactionTableNote,      string.Empty },
				{ DataTables.transactionTableNoteStyle, string.Empty },
			};
		}
		
		/// <summary>
		/// テーブル一覧の確認と不足分作成・バージョン修正
		/// </summary>
		/// <param name="commandLine"></param>
		/// <param name="initLog"></param>
		void InitializeDB(CommandLine commandLine, StartupLogger logger)
		{
			var dbFilePath = Literal.UserDBPath;
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/load"], dbFilePath);

			if(!File.Exists(dbFilePath)) {
				logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/mkdir"], Path.GetDirectoryName(dbFilePath));
				
				FileUtility.MakeFileParentDirectory(dbFilePath);
			}
			var connection = new SQLiteConnection("Data Source=" + dbFilePath);
			this._commonData.Database = new AppDBManager(connection, false);

			// 
			var enabledVersionTable = this._commonData.Database.ExistsTable(DataTables.masterTableVersion);
			//Debug.WriteLine(enabledVersionTable);
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/version"], enabledVersionTable);
			if(!enabledVersionTable) {
				// バージョンテーブルが存在しなければ作成
				this._commonData.Database.ExecuteCommand(global::PeMain.Properties.SQL.CreateVersionMasterTable);
			}
			
			// プログラムの知っているテーブルが存在しない、またはバージョンが異なる場合に調整する
			foreach(var pair in DataTables.map.Where(pair => pair.Key != DataTables.masterTableVersion)) {
				if(!this._commonData.Database.ExistsTable(pair.Key)) {
					InitializeNoteTableCreate(pair.Key, logger);
				} else {
					InitializeNoteTableChange(pair.Key, pair.Value, logger);
				}
			}
			
		}
		
		void InitializeNote(CommandLine commandLine, StartupLogger logger)
		{ }
		
		/// <summary>
		/// Peを使用使用するかユーザーに問い合わせる。
		/// </summary>
		/// <param name="logger"></param>
		/// <returns>使用する場合は真</returns>
		bool CheckAccept(StartupLogger logger)
		{
			var accept = this._commonData.MainSetting.RunningInfo.Running;
			if(!accept) {
				// TODO: ここから
				var dialog = new AcceptForm();
				dialog.SetCommonData(this._commonData);
				accept = dialog.ShowDialog() == DialogResult.OK;
			}
			
			return accept;
		}
		
		/// <summary>
		/// 設定ファイル初期化
		/// </summary>
		/// <param name="args"></param>
		bool InitializeSetting(CommandLine commandLine, StartupLogger logger)
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			logger.Puts(LogType.Information, "load main-setting", mainSettingFilePath);
			
			var existsSettingFilePath = File.Exists(mainSettingFilePath);
			this._commonData.MainSetting = Serializer.LoadFile<MainSetting>(mainSettingFilePath, true);
			
			var launcherItemsFilePath = Literal.UserLauncherItemsPath;
			logger.Puts(LogType.Information, "load launcher-item", launcherItemsFilePath);
			this._commonData.MainSetting.Launcher.Items = Serializer.LoadFile<HashSet<LauncherItem>>(launcherItemsFilePath, true);
			
			InitializeLanguage(commandLine, logger);
			InitializeRunningInfo(commandLine, logger);
			var acceptProgram = CheckAccept(logger);
			
			if(!acceptProgram) {
				// 使用許可が下りないのでさようなら
				Initialized = false;
				return existsSettingFilePath;
			}
			this._commonData.MainSetting.RunningInfo.Running = true;
			
			InitializeDB(commandLine, logger);
			InitializeNote(commandLine, logger);
			
			return existsSettingFilePath;
		}
		
		void InitializeMessage(CommandLine commandLine, StartupLogger logger)
		{
			this._messageWindow = new MessageWindow(this);
			this._messageWindow.StartupLogger = logger;
			this._messageWindow.SetCommonData(this._commonData);
		}
		
		void AttachmentToolbarSubMenu(ToolStripMenuItem parentItem)
		{
			var menuList = new List<ToolStripMenuItem>();
			foreach(var screen in Screen.AllScreens) {
				var menuItem = new ToolStripMenuItem();
				menuItem.Name = screen.DeviceName;
				menuItem.Text = ScreenUtility.GetScreenName(screen);
				menuItem.Click += (object sender, EventArgs e) => {
					var toolbar = this._toolbarForms[screen];
					/*
					toolbar.Visible = !toolbar.Visible;
					toolbar.UseToolbarItem.Visible = toolbar.Visible;
					 */
					toolbar.UseToolbarItem.Visible = !toolbar.Visible;
					toolbar.ApplySettingVisible();
				};
				menuList.Add(menuItem);
			}
			
			// サブメニュー設定
			parentItem.DropDownItems.AddRange(menuList.ToArray());
			
			// 親アイテム
			parentItem.Name = menuNameWindowToolbar;
			parentItem.Image = global::PeMain.Properties.Images.Toolbar;
			// 表示
			parentItem.DropDownOpened += (object sender, EventArgs e) => {
				/*
				var screens = Screen.AllScreens;
				var area = new Rectangle(
					screens.Min(s => s.Bounds.Left),
					screens.Min(s => s.Bounds.Top),
					screens.Max(s => s.Bounds.Right),
					screens.Max(s => s.Bounds.Bottom)
				);
				var iconSize = IconScale.Small.ToSize();
				*/
				foreach(var screen in Screen.AllScreens) {
					if(parentItem.DropDownItems.ContainsKey(screen.DeviceName)) {
						var menuItem = (ToolStripMenuItem)parentItem.DropDownItems[screen.DeviceName];
						menuItem.Checked = this._toolbarForms[screen].Visible;
					}
				}
			};
		}
		
		void AttachmentNoteSubMenu(ToolStripMenuItem parentItem)
		{
			var menuList = new List<ToolStripItem>();
			var itemNoteCreate  = new ToolStripMenuItem();
			var itemNoteHidden  = new ToolStripMenuItem();
			var itemNoteCompact = new ToolStripMenuItem();
			var itemNoteShowFront   = new ToolStripMenuItem();
			menuList.Add(itemNoteCreate);
			menuList.Add(itemNoteHidden);
			menuList.Add(itemNoteCompact);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(itemNoteShowFront);
			
			// ノート作成
			itemNoteCreate.Name = menuNameWindowNoteCreate;
			itemNoteCreate.Click += (object sender, EventArgs e) => {
				var screen = ScreenUtility.GetCurrentCursor();
				var area = screen.Bounds;
				var point = new Point(
					area.Left + area.Width / 2 - Literal.noteSize.Width / 2,
					area.Top + area.Height / 2 - Literal.noteSize.Width / 2
				);
				CreateNote(point);
			};
			// ノート非表示
			itemNoteHidden.Name = menuNameWindowNoteHidden;
			itemNoteHidden.Click += (object sender, EventArgs e) => {
				HiddenNote();
			};
			// ノート最小化
			itemNoteCompact.Name = menuNameWindowNoteCompact;
			itemNoteCompact.Click += (object sender, EventArgs e) => {
				CompactNote();
			};
			
			// ノートを前面へ
			itemNoteShowFront.Name = menuNameWindowNoteShowFront;
			itemNoteShowFront.Click += (object sender, EventArgs e) => {
				ShowFrontNote();
			};
			
			// サブメニュー設定
			parentItem.DropDownItems.AddRange(menuList.ToArray());
			
			// 親アイテム
			parentItem.Name = menuNameWindowNote;
			parentItem.Image = global::PeMain.Properties.Images.Note;
			// 表示
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				var hasNote = this._noteWindowList.Count > 0;
				itemNoteHidden.Enabled = hasNote;
				itemNoteCompact.Enabled = hasNote;
				itemNoteShowFront.Enabled = hasNote;
			};
		}
		
		/*
		void AttachmentWindowSubMenu(MenuItem parentMenu)
		{
			var menuList = new List<MenuItem>();
			var itemToolbar = new MenuItem();
			var itemNote = new MenuItem();
			var itemLogger = new MenuItem();
			
			menuList.Add(itemToolbar);
			menuList.Add(itemNote);
			menuList.Add(itemLogger);
			
			itemToolbar.Name = menuNameWindowToolbar;
			AttachmentToolbarSubMenu(itemToolbar);
			
			itemNote.Name = menuNameWindowNote;
			AttachmentNoteSubMenu(itemNote);
			
			itemLogger.Name = menuNameWindowLogger;
			itemLogger.Click += (object sender, EventArgs e) => {
				this._logForm.Visible = !this._logForm.Visible;
				this._commonData.MainSetting.Log.Visible = this._logForm.Visible;
			};
			
			// サブメニュー設定
			parentMenu.MenuItems.AddRange(menuList.ToArray());
			
			// ログ
			parentMenu.Popup += (object sender, EventArgs e) => {
				itemLogger.Checked = this._logForm.Visible;
			};
		}
		 */
		
		void AttachmentSystemEnvSubMenu(ToolStripMenuItem parentItem)
		{
			var menuList = new List<ToolStripItem>();
			var itemHiddenFile = new ToolStripMenuItem();
			var itemExtension = new ToolStripMenuItem();
			menuList.Add(itemHiddenFile);
			menuList.Add(itemExtension);
			
			// 隠しファイル
			itemHiddenFile.Name = menuNameSystemEnvHiddenFile;
			itemHiddenFile.Click += (object sender, EventArgs e) => {
				SystemEnvironment.SetHiddenFileShow(!SystemEnvironment.IsHiddenFileShow());
				SystemEnvironment.RefreshShell();
			};
			
			// 拡張子
			itemExtension.Name  = menuNameSystemEnvExtension;
			itemExtension.Click += (object sender, EventArgs e) => {
				SystemEnvironment.SetExtensionShow(!SystemEnvironment.IsExtensionShow());
				SystemEnvironment.RefreshShell();
			};
			
			// サブメニュー設定
			parentItem.DropDownItems.AddRange(menuList.ToArray());
			
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				itemHiddenFile.Checked = SystemEnvironment.IsHiddenFileShow();
				itemExtension.Checked = SystemEnvironment.IsExtensionShow();
			};

		}
		
		/// <summary>
		/// 本体メニュー初期化
		/// </summary>
		/// <returns></returns>
		private void AttachmentMainMenu()
		{
			var menuList = new List<ToolStripItem>();
			//var itemWindow = new MenuItem();
			var itemToolbar = new ToolStripMenuItem();
			var itemNote = new ToolStripMenuItem();
			var itemLogger = new ToolStripMenuItem();
			var itemSystemEnv = new ToolStripMenuItem();
			var itemSetting = new ToolStripMenuItem();
			var itemAbout = new ToolStripMenuItem();
			var itemHelp = new ToolStripMenuItem();
			var itemExit = new ToolStripMenuItem();
			
			menuList.Add(itemSetting);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(itemToolbar);
			menuList.Add(itemNote);
			menuList.Add(itemLogger);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(itemSystemEnv);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(itemAbout);
			menuList.Add(itemHelp);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(itemExit);
			
			// ウィンドウ
			//itemWindow.Name = menuNameWindow;
			//AttachmentWindowSubMenu(itemWindow);
			AttachmentToolbarSubMenu(itemToolbar);
			
			AttachmentNoteSubMenu(itemNote);
			
			// ログ
			itemLogger.Name = menuNameWindowLogger;
			itemLogger.Image = global::PeMain.Properties.Images.Log;
			itemLogger.Click += (object sender, EventArgs e) => {
				this._logForm.Visible = !this._logForm.Visible;
				this._commonData.MainSetting.Log.Visible = this._logForm.Visible;
			};
			
			// システム環境
			itemSystemEnv.Name = menuNameSystemEnv;
			itemSystemEnv.Image = global::PeMain.Properties.Images.SystemEnvironment;
			AttachmentSystemEnvSubMenu(itemSystemEnv);

			// 設定
			itemSetting.Name = menuNameSetting;
			itemSetting.Image = global::PeMain.Properties.Images.Config;
			itemSetting.Click += (object sender, EventArgs e) => {
				PauseOthers(() => OpenSettingDialog());
			};
			
			// 情報
			itemAbout.Name = menuNameAbout;
			itemAbout.Image = AppUtility.GetAppIcon(IconScale.Small);
			itemAbout.Click += (object sender, EventArgs e) => {
				PauseOthers(
					() => {
						var checkUpdate = false;
						using(var dialog = new AboutForm()) {
							dialog.SetCommonData(this._commonData);
							dialog.ShowDialog();
							checkUpdate = dialog.CheckUpdate;
						}
						if(checkUpdate) {
							CheckUpdateProcessWait(true);
						}
						
						return null;
					}
				);
			};
			
			// ヘルプ
			itemHelp.Name = menuNameHelp;
			itemHelp.Image = global::PeMain.Properties.Images.Help;
			itemHelp.Click += (object sender, EventArgs e) => {
				Executer.RunCommand(Literal.HelpDocumentURI, this._commonData);
			};
			
			// 終了
			itemExit.Name = menuNameExit;
			itemExit.Image = global::PeMain.Properties.Images.Close;
			itemExit.Click += (object sender, EventArgs e) => {
				CloseApplication(true);
			};
			
			// メインメニュー
			this._contextMenu.Opening += (object sender, CancelEventArgs e) => {
				itemLogger.Checked = this._logForm.Visible;
			};

			this._contextMenu.Items.AddRange(menuList.ToArray());
		}
		
		void InitializeSkin(CommandLine commandLine, StartupLogger logger)
		{
			this._commonData.Skin = new SystemSkin();
		}
		
		/// <summary>
		/// 本体UI初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeMain(CommandLine commandLine, StartupLogger logger)
		{
			this._notifyIcon = new NotifyIcon();
			this._contextMenu = new ContextMenuStrip();
			AttachmentMainMenu();
			
			this._notifyIcon.DoubleClick += IconDoubleClick;
			this._notifyIcon.Visible = true;
			
			var iconSize = IconScale.Small.ToSize();
			var iconRect = new Rectangle(Point.Empty, iconSize);
			using(var img = new Bitmap(iconSize.Width, iconSize.Height)) {
				using(var g = Graphics.FromImage(img)) {
					using(var icon = AppUtility.GetAppIcon(IconScale.Small)) {
						g.DrawImage(icon, iconRect);
					}
					#if DEBUG
					/*
					using(var b = new SolidBrush(Color.FromArgb(128, Color.Red))) {
						g.FillRectangle(b, iconRect);
					}
					 */
					DrawUtility.MarkingDebug(g, iconRect);
					#endif
				}
				this._notifyIcon.Icon = Icon.FromHandle(img.GetHicon());
			}
			foreach(ToolStripMenuItem toolItem in this._contextMenu.Items.Cast<ToolStripItem>().Where(t => t is ToolStripMenuItem)) {
				ToolStripUtility.AttachmentOpeningMenuInScreen(toolItem);
			}
			this._notifyIcon.ContextMenuStrip = this._contextMenu;
			
		}
		
		void InitializeLogForm(CommandLine commandLine, StartupLogger logger)
		{
			this._logForm = new LogForm(logger.FileLogger);
			this._logForm.SetCommonData(this._commonData);
			this._logForm.Closing += (object sender, CancelEventArgs e) => {
				this._commonData.MainSetting.Log.Visible = false;
			};
			
			this._commonData.Logger = this._logForm;
		}
		
		void InitializeCommandForm(CommandLine commandLine, StartupLogger logger)
		{
			
		}
		
		void InitializeToolbarForm(CommandLine commandLine, StartupLogger logger)
		{
			Debug.Assert(this._commonData != null);
			
			// ディスプレイ分生成
			foreach(var screen in Screen.AllScreens.OrderBy(s => !s.Primary)) {
				var toolbar = new ToolbarForm();
				toolbar.DockScreen = screen;
				//toolbar.MessageString +=  screen.DeviceName;
				toolbar.SetCommonData(this._commonData);
				this._toolbarForms.Add(screen, toolbar);
			}
			
			/*
			this._toolbarForms = new ToolbarForm();
			this._toolbarForms.Logger = this._logForm;
			this._toolbarForms.SetCommonData(this._commonData.Language, this._mainSetting);
			 */
		}
		
		void InitializeNoteForm(CommandLine commandLine, StartupLogger logger)
		{
			var noteDB = new NoteDB(this._commonData.Database);
			foreach(var item in noteDB.GetNoteItemList(true).Where(item => item.Visible)) {
				CreateNote(item);
			}
		}

		void InitializeUI(CommandLine commandLine, StartupLogger logger)
		{
			logger.Puts(LogType.Information, this._commonData.Language["log/init/ui/start"], string.Empty);

			InitializeSkin(commandLine, logger);
			InitializeLogForm(commandLine, logger);
			InitializeMessage(commandLine, logger);
			InitializeMain(commandLine, logger);
			InitializeCommandForm(commandLine, logger);
			InitializeToolbarForm(commandLine, logger);
			InitializeNoteForm(commandLine, logger);
			
			logger.Puts(LogType.Information, this._commonData.Language["log/init/ui/end"], string.Empty);
		}
		
		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="args"></param>
		bool Initialize(CommandLine commandLine, StartupLogger logger)
		{
			logger.Puts(LogType.Information, "Initialize Start", commandLine.Options.ToArray());
			
			this._commonData = new CommonData();
			this._commonData.RootSender = this;
			
			Debug.Assert(Initialized);
			var existsSettingFilePath = InitializeSetting(commandLine, logger);
			if(!Initialized) {
				logger.Puts(LogType.Information, "Initialize Cancel", string.Empty);
				return existsSettingFilePath;
			}
			Debug.Assert(Initialized);
			InitializeUI(commandLine, logger);
			
			Debug.Assert(Initialized);
			ApplyLanguage();
			
			//SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
			SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
			SystemEvents.SessionEnding += SystemEvents_SessionEnding;
			SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
			
			Debug.Assert(Initialized);
			this._logForm.PutsList(logger.GetList(), false);
			logger.Puts(LogType.Information, "Initialize End", string.Empty);
			
			return existsSettingFilePath;
		}


	}
}
