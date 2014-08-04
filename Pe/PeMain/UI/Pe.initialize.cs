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
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

using Microsoft.Win32;
using PeMain.Data;
using PeMain.Data.DB;
using PeMain.Logic;
using PeMain.Properties;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_initialize.
	/// </summary>
	public partial class Pe
	{
		void InitializeLanguage(CommandLine commandLine, StartupLogger logger)
		{
			// 言語
			var langName = this._commonData.MainSetting.LanguageFileName;
			var languageFileName = "default.xml";
			if(!string.IsNullOrEmpty(langName)) {
				if(!Path.HasExtension(langName)) {
					languageFileName = Path.ChangeExtension(langName, "xml");
				} else {
					languageFileName = langName;
				}
			} else {
				var a = CultureInfo.CurrentCulture;
				languageFileName = Path.ChangeExtension(CultureInfo.CurrentCulture.Name, "xml");
			}
			var languageFilePath = Path.Combine(Literal.PeLanguageDirPath, languageFileName);
			if(logger != null) {
				logger.Puts(LogType.Information, "language", languageFilePath);
			}
			this._commonData.Language = LoadDeserialize<Language>(languageFilePath, false);
			if(this._commonData.Language == null) {
				if(logger != null) {
					logger.Puts(LogType.Warning, "not found language", languageFilePath);
				}
				this._commonData.Language = new Language();
			}
		}
		
		void InitializeNoteTableCreate(string tableName, StartupLogger logger)
		{
			logger.Puts(LogType.Information, tableName, "CREATE");
			
			var map = new Dictionary<string, string>() {
				{ DataTables.masterTableNote,           global::PeMain.Properties.SQL.CreateNoteMasterTable },
				{ DataTables.transactionTableNote,      global::PeMain.Properties.SQL.CreateNoteTransactionTable },
				{ DataTables.transactionTableNoteStyle, global::PeMain.Properties.SQL.CreateNoteStyleTransactionTable },
			};
			var command = map[tableName];
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
			logger.Puts(LogType.Information, tableName, "CHECK");
			
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
			logger.Puts(LogType.Information, "db-file", dbFilePath);

			if(!File.Exists(dbFilePath)) {
				logger.Puts(LogType.Information, "note-data", "dir cleate");
				
				FileUtility.MakeFileParentDirectory(dbFilePath);
			}
			var connection = new SQLiteConnection("Data Source=" + dbFilePath);
			this._commonData.Database = new PeDBManager(connection, false);

			// 
			var enabledVersionTable = this._commonData.Database.ExistsTable(DataTables.masterTableVersion);
			Debug.WriteLine(enabledVersionTable);
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
		{
			this._commonData.MainSetting.Note.setDatabase(this._commonData.Database);
		}
		
		/// <summary>
		/// 設定ファイル初期化
		/// </summary>
		/// <param name="args"></param>
		void InitializeSetting(CommandLine commandLine, StartupLogger logger)
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			logger.Puts(LogType.Information, "main-setting", mainSettingFilePath);
			
			this._commonData.MainSetting = LoadDeserialize<MainSetting>(mainSettingFilePath, true);
			
			var launcherItemsFilePath = Literal.UserLauncherItemsPath;
			logger.Puts(LogType.Information, "launcher-items", launcherItemsFilePath);
			this._commonData.MainSetting.Launcher.Items = LoadDeserialize<HashSet<LauncherItem>>(launcherItemsFilePath, true);
			
			InitializeLanguage(commandLine, logger);
			
			//#if DEBUG
			InitializeDB(commandLine, logger);
			InitializeNote(commandLine, logger);
			//#endif
		}
		
		void InitializeMessage(CommandLine commandLine, StartupLogger logger)
		{
			this._messageWindow = new MessageWindow(this);
			this._messageWindow.StartupLogger = logger;
			this._messageWindow.SetCommonData(this._commonData);
		}
		
		void AttachmentToolbarSubMenu(ToolStripMenuItem parentMenu)
		{
			var menuList = new List<ToolStripMenuItem>();
			foreach(var screen in Screen.AllScreens) {
				var menuItem = new ToolStripMenuItem();
				menuItem.Name = screen.DeviceName;
				menuItem.Text = ScreenUtility.ToScreenName(screen);
				menuItem.Click += (object sender, EventArgs e) => {
					var toolbar = this._toolbarForms[screen];
					toolbar.Visible = !toolbar.Visible;
					toolbar.UseToolbarItem.Visible = toolbar.Visible;
				};
				menuList.Add(menuItem);
			}
			
			// サブメニュー設定
			parentMenu.DropDownItems.AddRange(menuList.ToArray());
			
			parentMenu.DropDownOpening += (object sender, EventArgs e) => {
				foreach(var screen in Screen.AllScreens) {
					if(parentMenu.DropDownItems.ContainsKey(screen.DeviceName)) {
						var menuItem = (ToolStripMenuItem)parentMenu.DropDownItems[screen.DeviceName];
						menuItem.Checked = this._toolbarForms[screen].Visible;
					}
				}
			};
		}
		
		void AttachmentNoteSubMenu(ToolStripMenuItem parentMenu)
		{
			var menuList = new List<ToolStripItem>();
			var itemNoteCreate = new ToolStripMenuItem();
			var itemNoteHidden = new ToolStripMenuItem();
			var itemNoteCompact= new ToolStripMenuItem();
			menuList.Add(itemNoteCreate);
			menuList.Add(itemNoteHidden);
			menuList.Add(itemNoteCompact);
			
			// ノート作成
			itemNoteCreate.Name = menuNameWindowNoteCreate;
			itemNoteCreate.Click += delegate(object sender, EventArgs e) {
				CreateNote();
			};
			// ノート非表示
			itemNoteHidden.Name = menuNameWindowNoteHidden;
			// ノート作成
			itemNoteCompact.Name = menuNameWindowNoteCompact;
			
			// サブメニュー設定
			parentMenu.DropDownItems.AddRange(menuList.ToArray());
		}
		
		void AttachmentWindowSubMenu(ToolStripMenuItem parentMenu)
		{
			var menuList = new List<ToolStripMenuItem>();
			var itemToolbar = new ToolStripMenuItem();
			var itemNote = new ToolStripMenuItem();
			var itemLogger = new ToolStripMenuItem();
			
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
			parentMenu.DropDownItems.AddRange(menuList.ToArray());
			
			// ログ
			parentMenu.DropDownOpening += (object sender, EventArgs e) => {
				itemLogger.Checked = this._logForm.Visible;
			};
		}
		
		void AttachmentSystemEnvSubMenu(ToolStripMenuItem parentMenu)
		{
			var menuList = new List<ToolStripMenuItem>();
			var itemHiddenFile = new ToolStripMenuItem();
			var itemExtension = new ToolStripMenuItem();
			menuList.Add(itemHiddenFile);
			menuList.Add(itemExtension);
			
			// 隠しファイル
			itemHiddenFile.Name = menuNameSystemEnvHiddenFile;
			itemHiddenFile.Click += (object sender, EventArgs e) => {
				SystemEnv.SetHiddenFileShow(!SystemEnv.IsHiddenFileShow());
				SystemEnv.RefreshShell();
			};
			
			// 拡張子
			itemExtension.Name  = menuNameSystemEnvExtension;
			itemExtension.Click += (object sender, EventArgs e) => {
				SystemEnv.SetExtensionShow(!SystemEnv.IsExtensionShow());
				SystemEnv.RefreshShell();
			};
			
			// サブメニュー設定
			parentMenu.DropDownItems.AddRange(menuList.ToArray());
			
			parentMenu.DropDownOpening += (object sender, EventArgs e) => {
				itemHiddenFile.Checked = SystemEnv.IsHiddenFileShow();
				itemExtension.Checked = SystemEnv.IsExtensionShow();
			};

		}
		
		/// <summary>
		/// 本体メニュー初期化
		/// </summary>
		/// <returns></returns>
		private void AttachmentMainMenu()
		{
			var menuList = new List<ToolStripItem>();
			var itemWindow = new ToolStripMenuItem();
			var itemSystemEnv = new ToolStripMenuItem();
			var itemSetting = new ToolStripMenuItem();
			var itemAbout = new ToolStripMenuItem();
			var itemExit = new ToolStripMenuItem();
			
			menuList.Add(itemSetting);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(itemWindow);
			menuList.Add(itemSystemEnv);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(itemAbout);
			menuList.Add(itemExit);
			
			// 情報
			itemAbout.Name = menuNameAbout;
			itemAbout.Click += (object sender, EventArgs e) => {
				MessageBox.Show("デュン！");
			};
			
			// ウィンドウ
			itemWindow.Name = menuNameWindow;
			AttachmentWindowSubMenu(itemWindow);

			// システム環境
			itemSystemEnv.Name = menuNameSystemEnv;
			AttachmentSystemEnvSubMenu(itemSystemEnv);

			// 設定
			itemSetting.Name = menuNameSetting;
			itemSetting.Click += (object sender, EventArgs e) => {
				PauseOthers(() => OpenSetting());
			};
			
			// 終了
			itemExit.Name = menuNameExit;
			itemExit.Click += (object sender, EventArgs e) => {
				CloseApplication(true);
			};

			this._notificationMenu.Items.AddRange(menuList.ToArray());
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
			this._notificationMenu = new ContextMenuStrip();
			AttachmentMainMenu();
			
			this._notifyIcon.DoubleClick += IconDoubleClick;
			this._notifyIcon.Visible = true;
			
			this._notifyIcon.Icon = global::PeMain.Properties.Images.Pe;
			this._notifyIcon.ContextMenuStrip = this._notificationMenu;
		}
		
		void InitializeLogForm(CommandLine commandLine, StartupLogger logger)
		{
			this._logForm = new LogForm(logger.FileLogger);
			this._logForm.SetCommonData(this._commonData);
			
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
			foreach(var item in this._commonData.MainSetting.Note.GetNoteItemList(true).Where(item => item.Visibled)) {
				CreateNote(item);
			}
		}

		void InitializeUI(CommandLine commandLine, StartupLogger logger)
		{
			logger.Puts(LogType.Information, this._commonData.Language["log/init/ui"], this._commonData.Language["log/start"]);

			InitializeSkin(commandLine, logger);
			InitializeLogForm(commandLine, logger);
			InitializeMessage(commandLine, logger);
			InitializeMain(commandLine, logger);
			InitializeCommandForm(commandLine, logger);
			InitializeToolbarForm(commandLine, logger);
			InitializeNoteForm(commandLine, logger);
			
			logger.Puts(LogType.Information, this._commonData.Language["log/init/ui"], this._commonData.Language["log/end"]);
		}
		
		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="args"></param>
		void Initialize(CommandLine commandLine, StartupLogger logger)
		{
			logger.Puts(LogType.Information, "Initialize", commandLine.Options.ToArray());
			
			this._commonData = new CommonData();
			this._commonData.RootSender = this;
			
			InitializeSetting(commandLine, logger);
			InitializeUI(commandLine, logger);
			
			ApplyLanguage();
			
			SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
			
			this._logForm.PutsList(logger.GetList(), false);
		}

		void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
		{
			this._logForm.Puts(LogType.Information, "SessionEnding", e);
			SaveSetting();
		}
	}
}
