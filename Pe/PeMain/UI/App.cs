﻿namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data.SQLite;
	using System.Diagnostics;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Timers;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.DB;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.DB;
	using Microsoft.Win32;

	/// <summary>
	/// プログラム本体。
	/// </summary>
	public sealed class App: IDisposable, IRootSender
	{
		#region Define

		const string menuNameNote = "menu_note";
		const string menuNameSystemEnv = "menu_systemenv";
		const string menuNameSetting = "menu_setting";
		const string menuNameAbout = "menu_about";
		const string menuNameHelp = "menu_help";
		const string menuNameExit = "menu_exit";

		const string menuNameWindowToolbar = "menu_window_toolbar";
		const string menuNameWindowNote = "menu_window_note";
		const string menuNameWindowLogger = "menu_window_logger";
		const string menuNameApplications = "menu_applications";

		const string menuNameWindowNoteCreate = "menu_window_note_create";
		const string menuNameWindowNoteHidden = "menu_window_note_hidden";
		const string menuNameWindowNoteCompact = "menu_window_note_compact";
		const string menuNameWindowNoteShowFront = "menu_window_note_show_front";
		const string menuNameWindowNoteSeparator = "menu_window_note_separator";

		const string menuNameSystemEnvHiddenFile = "menu_systemenv_hidden";
		const string menuNameSystemEnvExtension = "menu_systemenv_ext";
		const string menuNameSystemEnvWindow = "menu_systemenv_window";
		const string menuNameSystemEnvWindowSave = "menu_systemenv_window_save";
		const string menuNameSystemEnvWindowLoad = "menu_systemenv_window_load";
		const string menuNameSystemEnvWindowSeparator = "menu_systemenv_window_separator";
		const string menuNameSystemEnvClipboard = "menu_systemenv_clipboard";

		#endregion //////////////////////////////////////////

		#region Variable

		private NotifyIcon _notifyIcon;
		//private ContextMenu _contextMenu;
		private AppContextMenuStrip _contextMenu;
		private MessageWindow _messageWindow;
		private LogForm _logForm;
		private ClipboardForm _clipboardWindow;
		
		private List<NoteForm> _noteWindowList = new List<NoteForm>();
		
		private CommonData _commonData;
		private bool _pause;
		
		private Dictionary<Screen, ToolbarForm> _toolbarForms = new Dictionary<Screen, ToolbarForm>();
		
		private WindowListItem _tempWindowListItem;
		FixedSizedList<WindowListItem> _windowListItems; //= new List<WindowListItem>();
		//private List<WindowListItem> _windowListItemList = new List<WindowListItem>();
		
		System.Timers.Timer _windowTimer;

		Listener _listener;

		DateTime _clipboardPrevTime = DateTime.MinValue;
		uint _clipboardPrevSeq = 0;

		HashSet<Form> _otherWindows = new HashSet<Form>();

		HashSet<ISkin> _skins = new HashSet<ISkin>();
	
		#endregion //////////////////////////////////////////

		public App(CommandLine commandLine, FileLogger fileLogger)
		{
			Initialized = true;
			
			var logger = new StartupLogger(fileLogger);
			logger.PutsDebug("DebugLogging", "Startup: force logging");
			
			ExistsSettingFilePath = Initialize(commandLine, logger);
			logger.PutsDebug("ExistsSettingFilePath", ExistsSettingFilePath);

			#if !DISABLED_UPDATE_CHECK
			CheckUpdateProcessAsync(false);
			#endif
		}

		#region property

		public bool Initialized { get; private set; }
		public bool ExistsSettingFilePath { get; private set; }

		#endregion //////////////////////////////////////////

		#region IDisoise

		public void Dispose()
		{
			DetachmentSystemEvent();

			this._windowTimer.ToDispose();

			this._commonData.ToDispose();
			this._messageWindow.ToDispose();
			this._logForm.ToDispose();
			foreach(var w in this._noteWindowList) {
				w.ToDispose();
			}
			foreach(var w in this._toolbarForms.Values) {
				w.ToDispose();
			}
			this._contextMenu.ToDispose();
			this._notifyIcon.ToDispose();

#if DEBUG
			if(File.Exists(Literal.StartupShortcutPath)) {
				File.Delete(Literal.StartupShortcutPath);
			}
#endif
		}

		#endregion //////////////////////////////////////////

		#region IRootSender

		public void ShowBalloon(ToolTipIcon icon, string title, string message)
		{
			this._notifyIcon.ShowBalloonTip(0, title, message, icon);
		}

		public void ChangeLauncherGroupItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem)
		{
			foreach(var toolbar in this._toolbarForms.Values.Where(t => t.UseToolbarItem != toolbarItem)) {
				toolbar.ReceiveChangedLauncherItems(toolbarItem, toolbarGroupItem);
			}
		}

		public void AppendWindow(Form window)
		{
			this._otherWindows.Add(window);
			window.FormClosed += window_FormClosed;
		}

		public void ReceiveDeviceChanged(ChangeDevice changeDevice)
		{
			//this._commonData.Logger.Puts(LogType.Warning, "ReceiveDeviceChanged", changeDevice);
			// デバイス状態が変更されたか
			if(changeDevice.DBT == DBT.DBT_DEVNODES_CHANGED && Initialized && !this._pause) {
				// デバイス変更前のスクリーン数が異なっていればディスプレイの抜き差しが行われたと判定する
				// 現在生成されているツールバーの数が前回ディスプレイ数となる

				// 変更通知から現在数をAPIでまともに取得する
				var rawScreenCount = NativeMethods.GetSystemMetrics(SM.SM_CMONITORS);
				bool changedScreenCount = this._toolbarForms.Count != rawScreenCount;
				//bool isTimeout = false;
				Task.Factory.StartNew(() => {
					const int waitMax = Literal.waitCountForGetScreenCount;
					int waitCount = 0;

					var managedScreenCount = Screen.AllScreens.Count();
					while(rawScreenCount != managedScreenCount) {
						//Debug.WriteLine("waitCount" + waitCount);
						if(waitMax < ++waitCount) {
							// タイムアウト
							//isTimeout = true;
							break;
						}
						Thread.Sleep(Literal.screenCountWaitTime);
						managedScreenCount = Screen.AllScreens.Count();
					}
				}).ContinueWith(t => {
					if(changedScreenCount) {
						ChangedScreenCount();
					}
				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
		}

		public void ChangeClipboard()
		{
			if(!this._commonData.MainSetting.Clipboard.Enabled) {
				return;
			}

			var seq = NativeMethods.GetClipboardSequenceNumber();
			//Debug.WriteLine("{0} -> {1} - {2}", this._commonData.MainSetting.Clipboard.DisabledCopy, seq, _clipboardPrevSeq);
			if(this._commonData.MainSetting.Clipboard.DisabledCopy || seq == this._clipboardPrevSeq) {
				return;
			}
			this._clipboardPrevSeq = seq;
			var now = DateTime.Now;
			if(now - this._clipboardPrevTime <= this._commonData.MainSetting.Clipboard.WaitTime) {
				var map = new Dictionary<string, string>() {
					{ AppLanguageName.clipboardPrevTime, this._clipboardPrevTime.ToString() },
				};
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["clipboard/wait/title"], this._commonData.Language["clipboard/wait/message", map]);
				return;
			}
			Task.Factory.StartNew(() => {
				var time = Literal.clipboardThreadWaitTime.median;
				this._clipboardPrevTime = now;
				Thread.Sleep(time);
			}).ContinueWith(t => {
				var clipboardItem = new ClipboardItem();
				if(!this._commonData.MainSetting.Clipboard.DisabledCopy && clipboardItem.SetClipboardData(this._commonData.MainSetting.Clipboard.EnabledTypes)) {
					//this._clipboardPrevTime = DateTime.Now;
					try {
						var displayText = LanguageUtility.ClipboardItemToDisplayText(this._commonData.Language, clipboardItem, this._commonData.Logger);
						clipboardItem.Name = displayText;

						this._commonData.MainSetting.Clipboard.Items.Insert(0, clipboardItem);
					} catch(Exception ex) {
						this._commonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		#endregion //////////////////////////////////////////

		#region initilize

		void InitializeLanguage(CommandLine commandLine, StartupLogger logger)
		{
			Func<string, string> getLangPath = delegate(string name) {
				var fileName = string.Format("{0}.xml", name);
				var filePath = Path.Combine(Literal.ApplicationLanguageDirPath, fileName);
				if(logger != null) {
					logger.Puts(LogType.Information, "load language", filePath);
				}
				return filePath;
			};
			// 言語
			var langName = this._commonData.MainSetting.LanguageName;
			if(string.IsNullOrEmpty(langName)) {
				langName = CultureInfo.CurrentCulture.Name;
			}
			/*
			var languageFileName = string.Format("{0}.xml", langName);
			var languageFilePath = Path.Combine(Literal.ApplicationLanguageDirPath, languageFileName);
			if(logger != null) {
				logger.Puts(LogType.Information, "load language", languageFilePath);
			}
			this._commonData.Language = Serializer.LoadFile<Language>(languageFilePath, false);
			*/
			var languageFilePath = getLangPath(langName);
			this._commonData.Language = Serializer.LoadFile<Language>(languageFilePath, false);

			if(this._commonData.Language == null) {
				if(logger != null) {
					logger.Puts(LogType.Warning, "not found language", languageFilePath);
				}
				// #110, デフォルトの言語ファイル名
				langName = Literal.defaultLanguage;
				languageFilePath = getLangPath(langName);
				this._commonData.Language = Serializer.LoadFile<Language>(languageFilePath, true);
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
			var prevVersion = new Tuple<ushort, ushort, ushort>(prev.VersionMajor, prev.VersionMinor, prev.VersionRevision);
			// バージョンが一定未満なら強制的に使用承諾
			if(Functions.VersionCheck(prevVersion, Literal.AcceptVersion) < 0) {
				this._commonData.MainSetting.RunningInfo.Running = false;
			}

			if(commandLine.HasOption("accept") && commandLine.GetValue("accept") == "force") {
				// 強制的に使用許諾を表示し、次回実行時も使用許諾を表示できるようデータ保存
				this._commonData.MainSetting.RunningInfo.Running = false;
				Serializer.SaveFile(this._commonData.MainSetting, Literal.UserMainSettingPath);
			}
		}

		void InitializeNoteTableCreate(string tableName, StartupLogger logger)
		{
			var map = new Dictionary<string, string>() {
				{ DataTables.masterTableNote,           global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_CreateNoteMasterTable },
				{ DataTables.transactionTableNote,      global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_CreateNoteTransactionTable },
				{ DataTables.transactionTableNoteStyle, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_CreateNoteStyleTransactionTable },
			};
			var langMap = new Dictionary<string, string>() {
				{ "TABLE-NAME", tableName },
			};

			var command = map[tableName];
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/create", langMap], command);
			using(var query = this._commonData.Database.CreateQuery()) {
				query.ExecuteCommand(command);
			}

			var entity = new MVersionEntity();
			entity.Name = tableName;
			entity.Version = DataTables.map[tableName];
			using(var query = this._commonData.Database.CreateQuery()) {
				query.ExecuteInsert(new[] { entity });
			}
		}

		void InitializeWindowList(CommandLine commandLine, StartupLogger logger)
		{
			this._windowListItems = new FixedSizedList<WindowListItem>(this._commonData.MainSetting.WindowSaveCount);
		}
		/// <summary>
		/// NOTE: 将来的な予約
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="version"></param>
		/// <param name="logger"></param>
		void InitializeNoteTableChange(string tableName, int version, StartupLogger logger)
		{
			var langMap = new Dictionary<string, string>() {
				{ "TABLE-NAME", tableName },
			};
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/check", langMap], new { TableName = tableName, Version = version, });

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
		/// <param name="logger"></param>
		void InitializeDB(CommandLine commandLine, StartupLogger logger)
		{
			var dbFilePath = Literal.UserDBPath;
			var usePath = dbFilePath;
			if(NativeMethods.PathIsUNC(usePath)) {
				usePath = @"\\" + usePath;
			}
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/load"], usePath);

			if(!File.Exists(dbFilePath)) {
				logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/mkdir"], Path.GetDirectoryName(dbFilePath));

				FileUtility.MakeFileParentDirectory(dbFilePath);
			}
			var connection = new SQLiteConnection("Data Source=" + usePath);
			this._commonData.Database = new AppDBManager(connection, false);

			// 
			var enabledVersionTable = this._commonData.Database.ExistsTable(DataTables.masterTableVersion);
			//Debug.WriteLine(enabledVersionTable);
			logger.Puts(LogType.Information, this._commonData.Language["log/init/db-data/version"], enabledVersionTable);
			if(!enabledVersionTable) {
				// バージョンテーブルが存在しなければ作成
				using(var query = this._commonData.Database.CreateQuery()) {
					query.ExecuteCommand(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_CreateVersionMasterTable);
				}
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

		void InitializeNote(CommandLine commandLine, ILogger logger)
		{ }

		void InitializeApplicationExecutor(CommandLine commandLine, ILogger logger)
		{
			this._commonData.ApplicationSetting = Serializer.LoadFile<ApplicationSetting>(Literal.ApplicationBinAppPath, false);
		}
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
		/// <param name="commandLine"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		bool InitializeSetting(CommandLine commandLine, StartupLogger logger)
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			logger.Puts(LogType.Information, "load main-setting", mainSettingFilePath);

			var existsSettingFilePath = File.Exists(mainSettingFilePath);
			this._commonData.MainSetting = Serializer.LoadFile<MainSetting>(mainSettingFilePath, true);
			this._commonData.MainSetting.CorrectionValue();

			var launcherItemsFilePath = Literal.UserLauncherItemsPath;
			logger.Puts(LogType.Information, "load launcher-item", launcherItemsFilePath);
			this._commonData.MainSetting.Launcher.Items = Serializer.LoadFile<HashSet<LauncherItem>>(launcherItemsFilePath, true);
			foreach(var item in this._commonData.MainSetting.Launcher.Items) {
				item.CorrectionValue();
			}

			//var clipboardItemsFilePath = Literal.UserClipboardItemsPath;
			//logger.Puts(LogType.Information, "load clipboard-item", clipboardItemsFilePath);
			//this._commonData.MainSetting.Clipboard.Items = Serializer.LoadFile<Queue<ClipboardItem>>(clipboardItemsFilePath, true);

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

			InitializeApplicationExecutor(commandLine, logger);

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
			parentItem.Image = this._commonData.Skin.GetImage(SkinImage.Toolbar);
			// 表示
			parentItem.DropDownOpened += (object sender, EventArgs e) => {
				var screens = Screen.AllScreens.ToArray();
				var basePos = new Point(Math.Abs(screens.Min(s => s.Bounds.Left)), Math.Abs(screens.Min(s => s.Bounds.Top)));
				var iconSize = IconScale.Small.ToSize();
				var drawSize = (SizeF)iconSize;
				var maxArea = new RectangleF() {
					X = screens.Min(s => s.Bounds.Left),
					Y = screens.Min(s => s.Bounds.Top)
				};
				maxArea.Width = Math.Abs(maxArea.X) + screens.Max(s => s.Bounds.Right);
				maxArea.Height = Math.Abs(maxArea.Y) + screens.Max(s => s.Bounds.Bottom);

				var percentage = new SizeF(
					drawSize.Width / maxArea.Width * 100.0f,
					drawSize.Height / maxArea.Height * 100.0f
				);

				foreach(var screen in screens) {
					if(parentItem.DropDownItems.ContainsKey(screen.DeviceName)) {
						var menuItem = (ToolStripMenuItem)parentItem.DropDownItems[screen.DeviceName];
						// 各エリアの描画
						var alpha = 80;
						var baseImage = new Bitmap(iconSize.Width, iconSize.Height);
						using(var g = Graphics.FromImage(baseImage)) {
							foreach(var inScreen in screens) {
								var useScreen = inScreen == screen;
								var backColor = useScreen ? SystemColors.ActiveCaption : Color.FromArgb(alpha, SystemColors.InactiveCaption);
								var foreColor = useScreen ? SystemColors.ActiveCaptionText : Color.FromArgb(alpha, SystemColors.InactiveCaptionText);

								var baseArea = inScreen.Bounds;
								baseArea.Offset(basePos);

								var drawArea = new RectangleF(
									baseArea.X / 100.0f * percentage.Width,
									baseArea.Y / 100.0f * percentage.Height,
									baseArea.Width / 100.0f * percentage.Width,
									baseArea.Height / 100.0f * percentage.Height
								);

								using(var img = this._commonData.Skin.CreateColorBoxImage(foreColor, backColor, drawArea.Size.ToSize())) {
									g.DrawImage(img, drawArea.Location);
								}
							}
						}
						menuItem.Image.ToDispose();
						menuItem.Image = baseImage;
						menuItem.Checked = this._toolbarForms[screen].Visible;
					}
				}
			};
		}

		void AttachmentNoteSubMenu(ToolStripMenuItem parentItem)
		{
			var menuList = new List<ToolStripItem>();
			var itemNoteCreate = new ToolStripMenuItem();
			var itemNoteHidden = new ToolStripMenuItem();
			var itemNoteCompact = new ToolStripMenuItem();
			var itemNoteShowFront = new ToolStripMenuItem();
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
			parentItem.Image = this._commonData.Skin.GetImage(SkinImage.Note);
			// 表示
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				var hasNote = this._noteWindowList.Count > 0;
				itemNoteHidden.Enabled = hasNote;
				itemNoteCompact.Enabled = hasNote;
				itemNoteShowFront.Enabled = hasNote;

				//itemNoteCreate.ShortcutKeys = this._commonData.MainSetting.Note.CreateHotKey.GetShorcutKey();
				//itemNoteHidden.ShortcutKeys = this._commonData.MainSetting.Note.HiddenHotKey.GetShorcutKey();
				//itemNoteCompact.ShortcutKeys = this._commonData.MainSetting.Note.CompactHotKey.GetShorcutKey();
				//itemNoteShowFront.ShortcutKeys = this._commonData.MainSetting.Note.ShowFrontHotKey.GetShorcutKey();
				ToolStripUtility.SetSafeShortcutKeysAndDisplayKey(itemNoteCreate, this._commonData.MainSetting.Note.CreateHotKey, this._commonData.Language, this._commonData.Logger);
				ToolStripUtility.SetSafeShortcutKeysAndDisplayKey(itemNoteHidden, this._commonData.MainSetting.Note.HiddenHotKey, this._commonData.Language, this._commonData.Logger);
				ToolStripUtility.SetSafeShortcutKeysAndDisplayKey(itemNoteCompact, this._commonData.MainSetting.Note.CompactHotKey, this._commonData.Language, this._commonData.Logger);
				ToolStripUtility.SetSafeShortcutKeysAndDisplayKey(itemNoteShowFront, this._commonData.MainSetting.Note.ShowFrontHotKey, this._commonData.Language, this._commonData.Logger);

				OpeningNoteMenu();
			};
		}

		void AttachmentApplicationsSubMenu(ToolStripMenuItem parentItem)
		{
			var menuList = new List<ToolStripItem>();
			foreach(var applicationItem in this._commonData.ApplicationSetting.Items) {
				var launcherItem = new LauncherItem();
				launcherItem.Name = applicationItem.Name;
				launcherItem.Command = applicationItem.Name;
				launcherItem.LauncherType = LauncherType.Embedded;

				var icon = launcherItem.GetIcon(IconScale.Small, 0, this._commonData.ApplicationSetting);
#if DEBUG
				if(icon == null) {
					throw new NullReferenceException("rebuild solution!");
				}
#endif
				var menuItem = new ToolStripMenuItem();

				menuItem.Tag = applicationItem;
				menuItem.Image = IconUtility.ImageFromIcon(icon, IconScale.Small);

				menuItem.Click += (object sender, EventArgs e) => {
					if(this._commonData.ApplicationSetting.IsExecutingItem(launcherItem.Command)) {
						this._commonData.ApplicationSetting.KillApplicationItem(launcherItem);
					} else {
						Executor.RunItem(launcherItem, this._commonData);
					}
				};

				menuList.Add(menuItem);
			}

			parentItem.DropDownItems.AddRange(menuList.ToArray());

			parentItem.Name = menuNameApplications;
			parentItem.Image = this._commonData.Skin.GetImage(SkinImage.Applications);

			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				var menuItems = parentItem.DropDownItems.OfType<ToolStripMenuItem>();
				foreach(var menuItem in menuItems) {
					var applicationItem = menuItem.Tag as ApplicationItem;
					if(applicationItem != null) {
						menuItem.Checked = this._commonData.ApplicationSetting.IsExecutingItem(applicationItem.Name);
					}
				}
			};
		}

		void AttachmentSystemEnvWindowSubMenu(ToolStripMenuItem parentItem)
		{
			var menuList = new List<ToolStripItem>();
			var itemSave = new ToolStripMenuItem();
			var itemLoad = new ToolStripMenuItem();
			//var itemSeparator = new ToolStripSeparator();
			menuList.Add(itemSave);
			menuList.Add(itemLoad);
			//menuList.Add(itemSeparator);

			// 保存
			itemSave.Name = menuNameSystemEnvWindowSave;
			itemSave.Image = this._commonData.Skin.GetImage(SkinImage.WindowSave);
			itemSave.Click += (object sender, EventArgs e) => {
				var windowListItem = GetWindowListItem(false);
				this._tempWindowListItem = windowListItem;
			};

			// 読込
			itemLoad.Name = menuNameSystemEnvWindowLoad;
			itemLoad.Image = this._commonData.Skin.GetImage(SkinImage.WindowLoad);
			itemLoad.Click += (object sender, EventArgs e) => {
				ChangeWindow(this._tempWindowListItem);
				//this._tempWindowListItem = null;
			};

			// サブメニュー設定
			parentItem.DropDownItems.AddRange(menuList.ToArray());
			parentItem.Image = this._commonData.Skin.GetImage(SkinImage.WindowList);
			parentItem.DropDownOpened += (object sender, EventArgs e) => {
				itemLoad.Enabled = this._tempWindowListItem != null;

				OpeningWindowMenu();
			};
		}

		void AttachmentSystemEnvSubMenu(ToolStripMenuItem parentItem)
		{
			var menuList = new List<ToolStripItem>();
			var itemHiddenFile = new ToolStripMenuItem();
			var itemExtension = new ToolStripMenuItem();
			var itemWindow = new ToolStripMenuItem();
			var itemClipboard = new ToolStripMenuItem();
			menuList.Add(itemHiddenFile);
			menuList.Add(itemExtension);
			menuList.Add(itemWindow);
			menuList.Add(itemClipboard);

			// 隠しファイル
			itemHiddenFile.Name = menuNameSystemEnvHiddenFile;
			itemHiddenFile.Click += (object sender, EventArgs e) => {
				SystemEnvironment.SetHiddenFileShow(!SystemEnvironment.IsHiddenFileShow());
				SystemEnvironment.RefreshShell();
			};

			// 拡張子
			itemExtension.Name = menuNameSystemEnvExtension;
			itemExtension.Click += (object sender, EventArgs e) => {
				SystemEnvironment.SetExtensionShow(!SystemEnvironment.IsExtensionShow());
				SystemEnvironment.RefreshShell();
			};

			// ウィンドウ
			itemWindow.Name = menuNameSystemEnvWindow;
			AttachmentSystemEnvWindowSubMenu(itemWindow);

			// クリップボード
			itemClipboard.Name = menuNameSystemEnvClipboard;
			itemClipboard.Image = this._commonData.Skin.GetImage(SkinImage.Clipboard);
			
			itemClipboard.Click += (object sender, EventArgs e) => {
				SwitchShowClipboard();
			};

			// サブメニュー設定
			parentItem.DropDownItems.AddRange(menuList.ToArray());

			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				itemHiddenFile.Checked = SystemEnvironment.IsHiddenFileShow();
				itemExtension.Checked = SystemEnvironment.IsExtensionShow();
				itemClipboard.Checked = this._commonData.MainSetting.Clipboard.Visible;

				//itemHiddenFile.ShortcutKeys = this._commonData.MainSetting.SystemEnv.HiddenFileShowHotKey.GetShorcutKey();
				//itemExtension.ShortcutKeys = this._commonData.MainSetting.SystemEnv.ExtensionShowHotKey.GetShorcutKey();
				ToolStripUtility.SetSafeShortcutKeysAndDisplayKey(itemHiddenFile, this._commonData.MainSetting.SystemEnv.HiddenFileShowHotKey, this._commonData.Language, this._commonData.Logger);
				ToolStripUtility.SetSafeShortcutKeysAndDisplayKey(itemExtension, this._commonData.MainSetting.SystemEnv.ExtensionShowHotKey, this._commonData.Language, this._commonData.Logger);
				ToolStripUtility.SetSafeShortcutKeysAndDisplayKey(itemClipboard, this._commonData.MainSetting.Clipboard.ToggleHotKeySetting, this._commonData.Language, this._commonData.Logger);
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
			var itemApplications = new ToolStripMenuItem();
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
			menuList.Add(itemApplications);
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

			AttachmentApplicationsSubMenu(itemApplications);
			
			// ログ
			itemLogger.Name = menuNameWindowLogger;
			itemLogger.Image = this._commonData.Skin.GetImage(SkinImage.Log);
			itemLogger.Click += (object sender, EventArgs e) => {
				this._logForm.Visible = !this._logForm.Visible;
				this._commonData.MainSetting.Log.Visible = this._logForm.Visible;
			};

			// システム環境
			itemSystemEnv.Name = menuNameSystemEnv;
			itemSystemEnv.Image = this._commonData.Skin.GetImage(SkinImage.SystemEnvironment);
			AttachmentSystemEnvSubMenu(itemSystemEnv);

			// 設定
			itemSetting.Name = menuNameSetting;
			itemSetting.Image = this._commonData.Skin.GetImage(SkinImage.Config);
			itemSetting.Click += (object sender, EventArgs e) => PauseOthers(OpenSettingDialog);

			// 情報
			itemAbout.Name = menuNameAbout;

			itemAbout.Image = IconUtility.ImageFromIcon(this._commonData.Skin.GetIcon(SkinIcon.App), IconScale.Small);
			itemAbout.Click += (object sender, EventArgs e) => PauseOthers(() => {
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
			});

			// ヘルプ
			itemHelp.Name = menuNameHelp;
			itemHelp.Image = this._commonData.Skin.GetImage(SkinImage.Help);
			itemHelp.Click += (object sender, EventArgs e) => Executor.RunCommand(Literal.HelpDocumentURI, this._commonData);

			// 終了
			itemExit.Name = menuNameExit;
			itemExit.Image = this._commonData.Skin.GetImage(SkinImage.Close);
			itemExit.Click += (object sender, EventArgs e) => CloseApplication(true);

			// メインメニュー
			this._contextMenu.Opening += (object sender, CancelEventArgs e) => {
				itemLogger.Checked = this._logForm.Visible;
			};

			this._contextMenu.Items.AddRange(menuList.ToArray());
		}

		void InitializeSkin(CommandLine commandLine, ILogger logger)
		{
			ResetSkin(logger);

			var skinName = this._commonData.MainSetting.Skin.Name;
			var isDefault = string.IsNullOrWhiteSpace(skinName);
			if(!isDefault) {
				var hasSkin = this._skins.Any(s => s.About.Name == skinName);
				isDefault = !hasSkin;
			}
			if(isDefault) {
				var defSkin = new SystemSkin();
				defSkin.Load();
				skinName = defSkin.About.Name;
			}
			var skin = this._skins.Single(s => s.About.Name == skinName);

			this._commonData.Skin = skin;
			this._commonData.Skin.Initialize();

			LauncherItem.SetSkin(this._commonData.Skin);
		}

		/// <summary>
		/// 本体UI初期化
		/// </summary>
		/// <param name = "commandLine"></param>
		/// <param name = "logger"></param>
		void InitializeMain(CommandLine commandLine, StartupLogger logger)
		{
			this._notifyIcon = new NotifyIcon();
			this._contextMenu = new AppContextMenuStrip();
			AttachmentMainMenu();

			this._notifyIcon.DoubleClick += IconDoubleClick;
			this._notifyIcon.Visible = true;

			// タスクトレイアイコン構築
			var iconSize = IconScale.Small.ToSize();
			var iconRect = new Rectangle(Point.Empty, iconSize);
			using(var img = new Bitmap(iconSize.Width, iconSize.Height)) {
				using(var g = Graphics.FromImage(img)) {
					g.DrawImage(IconUtility.ImageFromIcon(this._commonData.Skin.GetIcon(SkinIcon.Tasktray), IconScale.Small), iconRect);
#if DEBUG
					DrawUtility.MarkingDebug(g, iconRect);
#endif
				}
				this._notifyIcon.Icon = Icon.FromHandle(img.GetHicon());
			}
			foreach(var toolItem in this._contextMenu.Items.OfType<ToolStripMenuItem>()) {
				ToolStripUtility.AttachmentOpeningMenuInScreen(toolItem);
			}
			this._contextMenu.Opening += (object sender, CancelEventArgs e) => HideAutoHiddenToolbar();
			this._notifyIcon.ContextMenuStrip = this._contextMenu;

		}

		void InitializeLogForm(CommandLine commandLine, StartupLogger logger)
		{
			this._logForm = new LogForm(logger.FileLogger);
			this._logForm.SetCommonData(this._commonData);
			/*
			this._logForm.Closing += (object sender, CancelEventArgs e) => {
				this._commonData.MainSetting.Log.Visible = false;
			};
			*/

			this._commonData.Logger = this._logForm;
		}

		void InitializeClipboardWindow(CommandLine commandLine, StartupLogger logger)
		{
			this._clipboardWindow = new ClipboardForm();
			this._clipboardWindow.SetCommonData(this._commonData);
			ChangeClipboard();
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
			InitializeClipboardWindow(commandLine, logger);
			InitializeMessage(commandLine, logger);
			InitializeMain(commandLine, logger);
			InitializeCommandForm(commandLine, logger);
			InitializeToolbarForm(commandLine, logger);
			InitializeNoteForm(commandLine, logger);
			InitializeWindowList(commandLine, logger);

			logger.Puts(LogType.Information, this._commonData.Language["log/init/ui/end"], string.Empty);
		}

		void InitializeTimer(CommandLine commandLine, StartupLogger logger)
		{
			Debug.Assert(this._commonData != null);

			// ウィンドウ一覧取得
			if(this._windowTimer == null) {
				this._windowTimer = new System.Timers.Timer();
				this._windowTimer.Elapsed += Timer_Elapsed;
			}
			this._windowTimer.Enabled = false;
			this._windowTimer.Interval = this._commonData.MainSetting.WindowSaveTime.TotalMilliseconds;
			this._windowTimer.Enabled = true;
		}

		void InitializeListener(CommandLine commandLine, StartupLogger logger)
		{
			Debug.Assert(this._commonData != null);

			this._listener = new Listener();
			this._listener.Enabled = true;
			this._listener.Keyboard.KeyPress += Keyboard_KeyPress;
		}

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name = "commandLine"></param>
		/// <param name = "logger"></param>
		bool Initialize(CommandLine commandLine, StartupLogger logger)
		{
			logger.Puts(LogType.Information, "Initialize Start", commandLine.Options.ToArray());

			this._pause = false;

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

			InitializeTimer(commandLine, logger);

			AttachmentSystemEvent();

			InitializeListener(commandLine, logger);
			//this._commonData.RootSender.EnabledClipboard = true;

			Debug.Assert(Initialized);
			logger.Puts(LogType.Information, "Initialize End", string.Empty);
			this._logForm.PutsList(logger.GetList(), false);

			return existsSettingFilePath;
		}

		#endregion //////////////////////////////////////////

		#region language

		void ApplyLanguageNoteMenu(ToolStripDropDownItem parentItem)
		{
			var keyItems = new[] {
				new {
					Name = menuNameWindowNoteCreate,
					Lang = "main/menu/window/note/create",
					//Key  = this._commonData.MainSetting.Note.CreateHotKey
				},
				new {
					Name = menuNameWindowNoteHidden,
					Lang = "main/menu/window/note/hidden",
					//Key  = this._commonData.MainSetting.Note.HiddenHotKey
				},
				new {
					Name = menuNameWindowNoteCompact,
					Lang = "main/menu/window/note/compact",
					//Key  = this._commonData.MainSetting.Note.CompactHotKey
				},
				new {
					Name = menuNameWindowNoteShowFront,
					Lang = "main/menu/window/note/show-front",
					//Key  = this._commonData.MainSetting.Note.ShowFrontHotKey
				},
			};

			foreach(var keyItem in keyItems) {
				var menuItem = (ToolStripMenuItem)parentItem.DropDownItems[keyItem.Name];
				menuItem.Text = this._commonData.Language[keyItem.Lang];
				//if(keyItem.Key.Enabled) {
				//	menuItem.ShortcutKeyDisplayString = LanguageUtility.HotkeySettingToDisplayText(this._commonData.Language, keyItem.Key);
				//}
			}
		}

		void ApplyLanguageSystemEnvWindowMenu(ToolStripDropDownItem parentItem)
		{
			parentItem.DropDownItems[menuNameSystemEnvWindowSave].Text = this._commonData.Language["main/menu/system-env/window/save"];
			parentItem.DropDownItems[menuNameSystemEnvWindowLoad].Text = this._commonData.Language["main/menu/system-env/window/load"];
		}

		void ApplyLanguageSystemEnvMenu(ToolStripDropDownItem parentItem)
		{
			var keyItems = new[] {
				new {
					Name = menuNameSystemEnvHiddenFile,
					Lang = "main/menu/system-env/show-hiddne-file",
					//Key  = this._commonData.MainSetting.SystemEnv.HiddenFileShowHotKey
				},
				new {
					Name = menuNameSystemEnvExtension,
					Lang = "main/menu/system-env/show-extension",
					//Key  = this._commonData.MainSetting.SystemEnv.ExtensionShowHotKey
				},
				new {
					Name = menuNameSystemEnvClipboard,
					Lang = "main/menu/system-env/clipboard",
					//Key  = this._commonData.MainSetting.Clipboard.ToggleHotKeySetting
				},
			};

			foreach(var keyItem in keyItems) {
				var menuItem = (ToolStripMenuItem)parentItem.DropDownItems[keyItem.Name];
				menuItem.Text = this._commonData.Language[keyItem.Lang];
				//if(keyItem.Key.Enabled) {
				//	menuItem.ShortcutKeyDisplayString = LanguageUtility.HotkeySettingToDisplayText(this._commonData.Language, keyItem.Key);
				//}
			}

			// ウィンドウ
			var itemWindow = (ToolStripDropDownItem)parentItem.DropDownItems[menuNameSystemEnvWindow];
			itemWindow.Text = this._commonData.Language["main/menu/system-env/window"];
			ApplyLanguageSystemEnvWindowMenu(itemWindow);
		}

		void ApplyLanguageApplicationsMenu(ToolStripDropDownItem parentItem)
		{
			var menuItems = parentItem.DropDownItems.Cast<ToolStripItem>();
			foreach(var menuItem in menuItems) {
				var applicationItem = menuItem.Tag as ApplicationItem;
				if(applicationItem != null) {
					menuItem.Text = LanguageUtility.ApplicationItemToTitle(this._commonData.Language, applicationItem);
				}
			}
		}

		void ApplyLanguageMainMenu()
		{
			var rootMenu = this._contextMenu.Items;

			rootMenu[menuNameWindowToolbar].Text = this._commonData.Language["main/menu/window/toolbar"];
			rootMenu[menuNameWindowNote].Text = this._commonData.Language["main/menu/window/note"];
			rootMenu[menuNameApplications].Text = this._commonData.Language["main/menu/applications"];
			rootMenu[menuNameWindowLogger].Text = this._commonData.Language["main/menu/window/logger"];
			rootMenu[menuNameSystemEnv].Text = this._commonData.Language["main/menu/system-env"];

			var noteMenu = (ToolStripDropDownItem)rootMenu[menuNameWindowNote];
			ApplyLanguageNoteMenu(noteMenu);

			var systemEnvMenu = (ToolStripDropDownItem)rootMenu[menuNameSystemEnv];
			ApplyLanguageSystemEnvMenu(systemEnvMenu);

			var applicationsMenu = (ToolStripDropDownItem)rootMenu[menuNameApplications];
			ApplyLanguageApplicationsMenu(applicationsMenu);

			rootMenu[menuNameSetting].Text = this._commonData.Language["main/menu/setting"];
			rootMenu[menuNameAbout].Text = this._commonData.Language["main/menu/about"];
			rootMenu[menuNameHelp].Text = this._commonData.Language["main/menu/help"];
			rootMenu[menuNameExit].Text = this._commonData.Language["common/menu/exit"];
		}

		void ApplyLanguage()
		{
			Debug.Assert(this._commonData.Language != null);

			ApplyLanguageMainMenu();
		}

		#endregion //////////////////////////////////////////

		#region function

		[Conditional("DEBUG")]
		public void DebugProcess()
		{ }

		void AttachmentSystemEvent()
		{
			SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
			SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
			SystemEvents.SessionEnding += SystemEvents_SessionEnding;
			SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
			SystemEvents.DisplaySettingsChanging += SystemEvents_DisplaySettingsChanging;

		}
		void DetachmentSystemEvent()
		{
			SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
			SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
			SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
			SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
			SystemEvents.DisplaySettingsChanging -= SystemEvents_DisplaySettingsChanging;
		}

		/// <summary>
		/// 保持するウィンドウ(Form)をすべて取得する。
		/// </summary>
		/// <returns></returns>
		IEnumerable<Form> GetWindows()
		{
			var result = new List<Form>();
			result.AddRange(this._toolbarForms.Values);
			result.Add(this._logForm);
			result.Add(this._clipboardWindow);

			/*
			foreach(var f in this._toolbarForms.Values.Where(f => f.OwnedForms.Length > 0)) {
				result.AddRange(f.OwnedForms);
			}
			*/
			result.AddRange(this._otherWindows);

			result.AddRange(this._noteWindowList);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="func">ウィンドウ再構築を独自に行う場合は真を返す処理を返す。</param>
		void PauseOthers(Func<Func<bool>> func)
		{
			var recursion = this._pause;
			var windowVisible = new Dictionary<Form, bool>();
			if(!recursion) {
				foreach(var window in GetWindows()) {
					windowVisible[window] = window.Visible;
					window.Visible = false;
				}
				// #82
				//this._notifyIcon.Visible = false;
				this._notifyIcon.ContextMenuStrip = null;
				this._pause = true;
			}
			var action = func();
			var customWindow = false;
			if(action != null) {
				customWindow = action();
			}
			if(!recursion) {
				if(!customWindow) {
					foreach(var pair in windowVisible) {
						// すでに表示している場合はポーズ中に処理が走ったため復帰は無視する
						if(!pair.Key.Visible) {
							pair.Key.Visible = pair.Value;
						}
					}
				}
				this._pause = false;
				// #82
				//this._notifyIcon.Visible = true;
				this._notifyIcon.ContextMenuStrip = this._contextMenu;
			}
		}

		/// <summary>
		/// 終了する。
		/// </summary>
		/// <param name="save"></param>
		public void CloseApplication(bool save)
		{
			if(this._commonData.ApplicationSetting != null) {
				this._commonData.ApplicationSetting.KillAllApplication();
			}

			if(save) {
				AppUtility.SaveSetting(this._commonData);
			}

			Application.Exit();
		}

		void ResetSkin(ILogger logger)
		{
			foreach(var skin in this._skins) {
				skin.Unload();
			}

			this._skins = AppUtility.GetSkins(logger);

		}

		void ResetMain()
		{
			this._contextMenu.ToDispose();
			this._notifyIcon.ToDispose();

			InitializeMain(null, null);
		}

		/// <summary>
		/// ツール―バー状態のリセット。
		/// </summary>
		void ResetToolbar()
		{
			//Debug.WriteLine("ResetToolbar");
			foreach(var toolbar in this._toolbarForms.Values) {
				//toolbar.SetCommonData(this._commonData);
				toolbar.ToDispose();
			}
			this._toolbarForms.Clear();

			InitializeToolbarForm(null, null);

			// メニュー構築
			var menuItem = (ToolStripMenuItem)this._contextMenu.Items[menuNameWindowToolbar];
			foreach(var subItem in menuItem.DropDownItems.Cast<ToolStripItem>().ToArray()) {
				subItem.ToDispose();
			}
			menuItem.DropDownItems.Clear();

			AttachmentToolbarSubMenu(menuItem);
		}

		/// <summary>
		/// ノート状態をリセット。
		/// </summary>
		void ResetNote()
		{
			foreach(var note in this._noteWindowList.ToArray()) {
				//note.Close();
				note.Dispose();
			}
			this._noteWindowList.Clear();
			InitializeNoteForm(null, null);
		}

		void ResetClipboard()
		{
			this._clipboardWindow.ClearEvent();
			this._clipboardWindow.ToDispose();
			this._clipboardWindow = new ClipboardForm();
			this._clipboardWindow.SetCommonData(this._commonData);
		}

		/// <summary>
		/// 表示コンポーネントをリセット。
		/// </summary>
		void ResetUI()
		{
			foreach(var window in this._otherWindows.ToArray()) {
				var streamWindow = window as StreamForm;
				if(streamWindow != null && streamWindow.ProcessRunning) {
					this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/reset/stream/skip"], streamWindow.LauncherItem);
					continue;
				}
				this._otherWindows.Remove(window);
				window.Dispose();
			}

			ResetMain();
			ResetToolbar();
			ResetNote();
			ResetClipboard();

			ApplyLanguage();
		}

		/// <summary>
		/// 設定ダイアログを開く。
		/// </summary>
		/// <returns></returns>
		Func<bool> OpenSettingDialog()
		{
			using(var settingForm = new SettingForm(this._commonData.Language, this._commonData.Skin, this._commonData.MainSetting, this._commonData.Database, this._commonData.ApplicationSetting)) {
				if(settingForm.ShowDialog() == DialogResult.OK) {
					/*
					foreach(var note in this._noteWindowList) {
						note.Close();
						note.Dispose();
					}
					this._noteWindowList.Clear();
					InitializeNoteForm(null, null);
					 */

					var mainSetting = settingForm.MainSetting;
					// ログ
					mainSetting.Log.Point = this._commonData.MainSetting.Log.Point;
					mainSetting.Log.Size = this._commonData.MainSetting.Log.Size;
					// クリップボード
					mainSetting.Clipboard.Location = this._commonData.MainSetting.Clipboard.Location;
					mainSetting.Clipboard.Size = this._commonData.MainSetting.Clipboard.Size;
					mainSetting.Clipboard.Items = this._commonData.MainSetting.Clipboard.Items;
					mainSetting.Clipboard.Items.LimitSize = mainSetting.Clipboard.Limit;

					var check = mainSetting.RunningInfo.CheckUpdate != mainSetting.RunningInfo.CheckUpdate || mainSetting.RunningInfo.CheckUpdate;
					var oldSetting = this._commonData.MainSetting;
					this._commonData.MainSetting = mainSetting;
					oldSetting.ToDispose();
					settingForm.SaveFiles();
					settingForm.SaveDB(this._commonData.Database);
					AppUtility.SaveSetting(this._commonData);
					InitializeLanguage(null, null);
					InitializeSkin(null, this._commonData.Logger);

					return () => {
						this._logForm.SetCommonData(this._commonData);
						this._messageWindow.SetCommonData(this._commonData);

						ResetUI();

						foreach(var window in this._otherWindows.OfType<StreamForm>().ToArray()) {
							window.Visible = true;
						}

						if(check) {
#if !DISABLED_UPDATE_CHECK
							CheckUpdateProcessAsync(false);
#endif
						}

						return true;
					};
				}
			}
			return null;
		}

		/// <summary>
		/// システム環境を変更する。
		/// </summary>
		/// <param name="nowValueDg"></param>
		/// <param name="changeValueDg"></param>
		/// <param name="messageTitleName"></param>
		/// <param name="showMessageName"></param>
		/// <param name="hiddenMessageName"></param>
		/// <param name="errorMessageName"></param>
		void ChangeShowSystemEnvironment(Func<bool> nowValueDg, Action<bool> changeValueDg, string messageTitleName, string showMessageName, string hiddenMessageName, string errorMessageName)
		{
			var prevValue = nowValueDg();
			changeValueDg(!prevValue);
			var nowValue = nowValueDg();
			SystemEnvironment.RefreshShell();

			ToolTipIcon icon;
			string messageName;
			if(prevValue != nowValue) {
				if(nowValue) {
					messageName = showMessageName;
				} else {
					messageName = hiddenMessageName;
				}
				icon = ToolTipIcon.Info;
				ResetLauncherFileList();
			} else {
				messageName = errorMessageName;
				icon = ToolTipIcon.Error;
			}
			var title = this._commonData.Language[messageTitleName];
			var message = this._commonData.Language[messageName];
			if(icon == ToolTipIcon.Error) {
				this._commonData.Logger.Puts(LogType.Error, title, message);
			}

			ShowBalloon(icon, title, message);
		}

		/// <summary>
		/// ホットキー受信。
		/// </summary>
		/// <param name="hotKeyId"></param>
		/// <param name="mod"></param>
		/// <param name="key"></param>
		public void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key)
		{
			if(this._pause) {
				return;
			}

			switch(hotKeyId) {
				case HotKeyId.HiddenFile:
					ChangeShowSystemEnvironment(SystemEnvironment.IsHiddenFileShow, SystemEnvironment.SetHiddenFileShow, "balloon/hidden-file/title", "balloon/hidden-file/show", "balloon/hidden-file/hide", "balloon/hidden-file/error");
					break;

				case HotKeyId.Extension:
					ChangeShowSystemEnvironment(SystemEnvironment.IsExtensionShow, SystemEnvironment.SetExtensionShow, "balloon/extension/title", "balloon/extension/show", "balloon/extension/hide", "balloon/extension/error");
					break;

				case HotKeyId.CreateNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/create"]);
					CreateNote(Point.Empty);
					break;

				case HotKeyId.HiddenNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/hidden"]);
					HiddenNote();
					break;

				case HotKeyId.CompactNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/compact"]);
					CompactNote();
					break;

				case HotKeyId.ShowFrontNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/show-front"]);
					ShowFrontNote();
					break;

				case HotKeyId.SwitchClipboardShow:
					SwitchShowClipboard();
					break;

				default:
					break;
			}
		}

		NoteForm CreateNote(Point point)
		{
			// アイテムをデータ設定
			var item = new NoteItem();
			item.Title = DateTime.Now.ToString();
			if(point.IsEmpty) {
				item.Location = Cursor.Position;
			} else {
				item.Location = point;
			}
			var noteDB = new NoteDB(this._commonData.Database);
			noteDB.InsertMaster(item);

			var noteForm = CreateNote(item);
			UIUtility.ShowFrontActive(noteForm);

			return noteForm;
		}

		NoteForm CreateNote(NoteItem noteItem)
		{
			var noteForm = new NoteForm();
			noteForm.NoteItem = noteItem;
			noteForm.SetCommonData(this._commonData);
			noteForm.Show();
			noteForm.Closed += (object sender, EventArgs e) => {
				if(noteForm.Visible) {
					this._noteWindowList.Remove(noteForm);
				}
			};
			this._noteWindowList.Add(noteForm);
			return noteForm;
		}

		void HiddenNote()
		{
			var list = this._noteWindowList
				.Where(note => !note.NoteItem.Locked)
				;
			foreach(var note in list.ToArray()) {
				note.ToClose(false);
			}
		}

		void CompactNote()
		{
			var list = this._noteWindowList
				.Where(note => !note.NoteItem.Compact)
				.Where(note => !note.NoteItem.Locked)
				;
			foreach(var note in list) {
				note.ToCompact();
			}
		}

		void ShowFrontNote()
		{
			var list = this._noteWindowList;
			foreach(var note in list) {
				UIUtility.ShowFront(note);
			}
		}

		void SwitchShowClipboard()
		{
			this._commonData.MainSetting.Clipboard.Visible = !this._commonData.MainSetting.Clipboard.Visible;
			this._clipboardWindow.Visible = this._commonData.MainSetting.Clipboard.Visible;
		}

		UpdateData CheckUpdate(bool force)
		{
			var updateData = new UpdateData(Literal.UserDownloadDirPath, this._commonData.MainSetting.RunningInfo.CheckUpdateRC, this._commonData);
			if(force || !this._pause && this._commonData.MainSetting.RunningInfo.CheckUpdate) {
				var updateInfo = updateData.Check();
			}
			return updateData;
		}

		void CheckedUpdate(bool force, UpdateData updateData)
		{
			if(force || !this._pause && this._commonData.MainSetting.RunningInfo.CheckUpdate) {
				if(updateData != null && updateData.Info != null) {
					if(updateData.Info.IsUpdate) {
						ShowUpdateDialog(updateData);
					} else if(updateData.Info.IsError) {
						this._commonData.Logger.Puts(LogType.Warning, this._commonData.Language["log/update/error"], updateData.Info.Log);
					} else {
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/newest"], updateData.Info.Log);
					}
				} else {
					this._commonData.Logger.Puts(LogType.Error, this._commonData.Language["log/update/error"], "info is null");
				}
			} else if(this._pause) {
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/check-stop"], "this._pause => true");
			}
		}

		void CheckUpdateProcessAsync(bool force)
		{
			Task.Factory.StartNew(() => {
				if(!force) {
					Thread.Sleep(Literal.updateWaitTime);
				}
				return CheckUpdate(force);
			}).ContinueWith(t => {
				CheckedUpdate(force, t.Result);
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		void CheckUpdateProcessWait(bool force)
		{
			var updateData = CheckUpdate(force);
			CheckedUpdate(force, updateData);
		}

		/// <summary>
		/// アップデートダイアログ表示。
		/// </summary>
		/// <param name="updateData"></param>
		void ShowUpdateDialog(UpdateData updateData)
		{
			PauseOthers(() => {
				try {
					using(var dialog = new UpdateForm()) {
						dialog.UpdateData = updateData;
						dialog.SetCommonData(this._commonData);
						if(dialog.ShowDialog() == DialogResult.OK) {
							// 現在設定を保持する
							AppUtility.SaveSetting(this._commonData);
							if(updateData.Execute()) {
								return () => {
									CloseApplication(false);
									return false;
								};
							}
						}
					}
				} catch(Exception ex) {
					// #96
					this._commonData.Logger.Puts(LogType.Error, ex.Message, ex);
				}
				return null;
			});
		}

		void ResetLauncherFileList()
		{
			// なんかこれそもそもが変な気がするんです
		}

		/// <summary>
		/// ディスプレイ数に変更があった。
		/// </summary>
		void ChangedScreenCount()
		{
			this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/screen/count-change"], string.Empty);
			ResetUI();
		}

		/// <summary>
		/// ホームダイアログ表示。
		/// </summary>
		public void ShowHomeDialog()
		{
			PauseOthers(() => {
				using(var dialog = new HomeForm()) {
					dialog.SetCommonData(this._commonData);
					dialog.ShowDialog();
					if(dialog.ItemFound) {
						return () => {
							// ログがあれば構築
							if(dialog.LogList.Count != 0) {
								this._logForm.PutsList(dialog.LogList, true);
							}
							// 初期化
							ResetUI();
							return true;
						};
					} else {
						return null;
					}
				}
			});
		}

		void OpeningNoteMenu()
		{
			var parentItem = (ToolStripMenuItem)this._contextMenu.Items[menuNameWindowNote];

			if(parentItem.DropDownItems.ContainsKey(menuNameWindowNoteSeparator)) {
				var separatorItem = parentItem.DropDownItems[menuNameWindowNoteSeparator];
				var itemMenus = parentItem.DropDownItems.Cast<ToolStripItem>().SkipWhile(t => t != separatorItem);
				foreach(var itemMenu in itemMenus.ToArray()) {
					parentItem.DropDownItems.Remove(itemMenu);
					itemMenu.ToDispose();
				}
			}
			var noteDB = new NoteDB(this._commonData.Database);
			var noteItems = noteDB.GetNoteItemList(true);
			var isStart = true;
			var itemNoteMenuList = new List<ToolStripItem>();
			var noteImageSize = IconScale.Small.ToSize();
			var noteSmallSize = new Size(noteImageSize.Width, noteImageSize.Height / 2);
			foreach(var noteItem in noteItems) {
				if(isStart) {
					var itemSeparator = new ToolStripSeparator();
					itemSeparator.Name = menuNameWindowNoteSeparator;
					itemNoteMenuList.Add(itemSeparator);
					isStart = false;
				}

				var menuItem = new ToolStripMenuItem();
				menuItem.Text = noteItem.Title;
				if(noteItem.Compact) {
					menuItem.ImageScaling = ToolStripItemImageScaling.None;
					menuItem.Image = this._commonData.Skin.CreateColorBoxImage(noteItem.Style.ForeColor, noteItem.Style.BackColor, noteSmallSize);
				} else {
					menuItem.Image = this._commonData.Skin.CreateColorBoxImage(noteItem.Style.ForeColor, noteItem.Style.BackColor, noteImageSize);
				}
				menuItem.Checked = noteItem.Visible;
				menuItem.Click += (object sender, EventArgs e) => {
					if(noteItem.Visible) {
						_noteWindowList.Single(n => n.NoteItem.NoteId == noteItem.NoteId).ToClose(false);
					} else {
						noteItem.Visible = true;
						var noteWindow = CreateNote(noteItem);
						noteWindow.SaveItem();
					}
				};

				itemNoteMenuList.Add(menuItem);
			}

			if(itemNoteMenuList.Count > 0) {
				parentItem.DropDownItems.AddRange(itemNoteMenuList.ToArray());
			}
			/*
				var noteDB = new NoteDB(this._commonData.Database);
					Debug.WriteLine(noteDB);
				foreach(var item in noteDB.GetNoteItemList(true).Where(item => item.Visible)) {
					Debug.WriteLine(item);
				}
			 */
		}

		/// <summary>
		/// ウィンドウ位置を取得する。
		/// </summary>
		/// <param name="getAppWindow"></param>
		/// <returns></returns>
		WindowListItem GetWindowListItem(bool getAppWindow)
		{
			var windowItemList = new WindowListItem();

			// http://msdn.microsoft.com/en-us/library/windows/desktop/ms633574(v=vs.85).aspx
			var skipClassName = new[] {
				"Shell_TrayWnd", // タスクバー
				"Button",
				"Progman", // プログラムマネージャ
				"#32769", // デスクトップ
				"WorkerW",
				"SysShadow",
				"SideBar_HTMLHostWindow",
			};

			var myProcess = Process.GetCurrentProcess();

			NativeMethods.EnumWindows(
				(hWnd, lParam) => {
					int processId;
					NativeMethods.GetWindowThreadProcessId(hWnd, out processId);
					var process = Process.GetProcessById(processId);
					if(!getAppWindow) {
						if(myProcess.Id == process.Id) {
							return true;
						}
					}

					if(!NativeMethods.IsWindowVisible(hWnd)) {
						return true;
					}

					var classBuffer = new StringBuilder(WindowsUtility.classNameLength);
					NativeMethods.GetClassName(hWnd, classBuffer, classBuffer.Capacity);
					var className = classBuffer.ToString();
					if(skipClassName.Any(s => s == className)) {
						return true;
					}

					var titleLength = NativeMethods.GetWindowTextLength(hWnd);
					var titleBuffer = new StringBuilder(titleLength + 1);
					NativeMethods.GetWindowText(hWnd, titleBuffer, titleBuffer.Capacity);
					var rawRect = new RECT();
					NativeMethods.GetWindowRect(hWnd, out rawRect);
					var windowItem = new WindowItem();
					windowItem.Name = titleBuffer.ToString();
					windowItem.Process = process;
					windowItem.WindowHandle = hWnd;
					windowItem.Rectangle = new Rectangle(rawRect.X, rawRect.Y, rawRect.Width, rawRect.Height);
					//Debug.WriteLine("{0}, {1}, {2}", className, windowItem.Name, windowItem.Rectangle);
					windowItemList.Items.Add(windowItem);
					return true;
				},
				IntPtr.Zero
			);

			return windowItemList;
		}

		/// <summary>
		/// ウィンドウ位置を設定。
		/// </summary>
		/// <param name="windowListItem"></param>
		void ChangeWindow(WindowListItem windowListItem)
		{
			foreach(var windowItem in windowListItem.Items) {
				var reslut = NativeMethods.MoveWindow(windowItem.WindowHandle, windowItem.Rectangle.X, windowItem.Rectangle.Y, windowItem.Rectangle.Width, windowItem.Rectangle.Height, true);
			}
		}

		void OpeningWindowMenu()
		{
			var parentItem = (ToolStripMenuItem)((ToolStripMenuItem)this._contextMenu.Items[menuNameSystemEnv]).DropDownItems[menuNameSystemEnvWindow];
			//_windowListItems
			if(parentItem.DropDownItems.ContainsKey(menuNameSystemEnvWindowSeparator)) {
				var separatorItem = parentItem.DropDownItems[menuNameSystemEnvWindowSeparator];
				var itemMenus = parentItem.DropDownItems.Cast<ToolStripItem>().SkipWhile(t => t != separatorItem);
				foreach(var itemMenu in itemMenus.ToArray()) {
					parentItem.DropDownItems.Remove(itemMenu);
					itemMenu.ToDispose();
				}
			}

			var itemWindowMenuList = new List<ToolStripItem>();
			var isStart = true;
			foreach(var windowListItem in this._windowListItems) {
				if(isStart) {
					var itemSeparator = new ToolStripSeparator();
					itemSeparator.Name = menuNameSystemEnvWindowSeparator;
					itemWindowMenuList.Add(itemSeparator);
					isStart = false;
				}

				var menuItem = new ToolStripMenuItem();
				menuItem.Text = windowListItem.Name;
				menuItem.Click += (object sender, EventArgs e) => ChangeWindow(windowListItem);
				itemWindowMenuList.Add(menuItem);

				if(itemWindowMenuList.Count > 0) {
					parentItem.DropDownItems.AddRange(itemWindowMenuList.ToArray());
				}
			}
		}

		public void PushWindowListItem(WindowListItem windowListItem)
		{
			/*
			if(this._commonData.MainSetting.WindowSaveCount <= this._windowListItems.Count) {
				this._windowListItems.RemoveRange(0, this._windowListItems.Count - this._commonData.MainSetting.WindowSaveCount + 1);
			}
			*/
			this._windowListItems.Add(windowListItem);
		}

		/// <summary>
		/// 自動的に隠すツールバーを強制的に隠す。
		/// </summary>
		void HideAutoHiddenToolbar()
		{
			foreach(var toolbar in this._toolbarForms.Values.Where(t => t.Visible && t.AutoHide).ToArray()) {
				toolbar.Hidden();
			}
		}


		#endregion //////////////////////////////////////////

		private void IconDoubleClick(object sender, EventArgs e)
		{
			if(!this._pause) {
				ShowHomeDialog();
			}
		}
		
		void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			this._logForm.Puts(LogType.Information, "SessionSwitch", e);
			if(e.Reason == SessionSwitchReason.ConsoleConnect) {
				ResetUI();
			} else if(e.Reason == SessionSwitchReason.ConsoleDisconnect) {
				AppUtility.SaveSetting(this._commonData);
			}
		}

		void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if(e.Category.IsIn(UserPreferenceCategory.VisualStyle, UserPreferenceCategory.Color)) {
				this._logForm.Puts(LogType.Information, "UserPreferenceChanged", e);
				ResetUI();
			}
		}

		void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
		{
			this._logForm.Puts(LogType.Information, "SessionEnding", e);
			AppUtility.SaveSetting(this._commonData);
		}
		
		void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			if(e.Mode == PowerModes.Resume) {
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/power/resume"], e);
#if !DISABLED_UPDATE_CHECK
				CheckUpdateProcessAsync(false);
#endif
			}
		}
		
		void SystemEvents_DisplaySettingsChanging(object sender, EventArgs e)
		{
			var windowItemList = GetWindowListItem(false);
			windowItemList.Name = this._commonData.Language["save-window/display"];
			PushWindowListItem(windowItemList);
			this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/save-window/display"], windowItemList);
			// #56
			ResetToolbar();
		}
		
		void NoteMenu_DropDownOpening(object sender, EventArgs e)
		{
			OpeningNoteMenu();
		}
		
		/// <summary>
		/// タイマー関連はここにまとめておこうと思う。
		/// 
		/// よって将来的な拡張に対応できるよう実装。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var timer = (System.Timers.Timer)sender;
			try {
				timer.Enabled = false;
				if(timer == this._windowTimer) {
					// 停止状態やメニュー表示状態では無視しとく
					if(!(this._pause || this._contextMenu.ShowContextMenu)) {
						var windowItemList = GetWindowListItem(false);
						windowItemList.Name = this._commonData.Language["save-window/timer"];
						PushWindowListItem(windowItemList);
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/save-window/timer"], windowItemList);
					} else {
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/save-window/skip"], new { Pause = this._pause, ShowContextMenu = this._contextMenu.ShowContextMenu});
					}
				}
			} finally {
				if(timer.AutoReset) {
					timer.Enabled = true;
				}
			}
		}

		void Keyboard_KeyPress(object sender, KeyPressEventArgs e)
		{
			const char esc = (char)27;

			if(e.KeyChar == esc) {
				var nowTime = DateTime.Now;
				// ダブルクリック時間だけど分かりやすいのでよし
				var time = NativeMethods.GetDoubleClickTime();
				if(nowTime - this._listener.PrevToolbarHiddenTime <= TimeSpan.FromMilliseconds(time)) {
					this._listener.Keyboard.Enabled = false;
					try {
						this._listener.PrevToolbarHiddenTime = nowTime;
						HideAutoHiddenToolbar();
					} finally {
						this._listener.PrevToolbarHiddenTime = DateTime.MinValue;
						this._listener.Keyboard.Enabled = true;
					}
				} else {
					this._listener.PrevToolbarHiddenTime = nowTime;
				}
			}
		}

		void window_FormClosed(object sender, FormClosedEventArgs e)
		{
			var window = (Form)sender;
			Debug.WriteLine(window.Text);
			this._otherWindows.Remove(window);
			this._commonData.Logger.Puts(LogType.Information, sender.ToString(), e);
			window.Dispose();
		}
		

	}
	
}

