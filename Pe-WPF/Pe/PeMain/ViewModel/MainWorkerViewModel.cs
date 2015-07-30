namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using System.Threading.Tasks;
	using System.Threading;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Converter;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using System.IO;
	using System.Windows.Threading;
	using Microsoft.Win32;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using System.Globalization;

	public sealed class MainWorkerViewModel: ViewModelBase, IAppSender, IClipboardWatcher
	{
		#region define

		enum WindowSaveType
		{
			Temporary,
			Timer,
			System
		}

		#endregion

		#region variable

		bool _isContextMenuOpen;

		DateTime _clipboardPreviousTime = DateTime.MinValue;
		uint _clipboardPreviousSequenceNumber = 0;

		#endregion

		public MainWorkerViewModel(VariableConstants variableConstants, ILogger logger)
		{
			CommonData = new CommonData() {
				Logger = logger,
				VariableConstants = variableConstants,
				AppSender = this,
				ClipboardWatcher = this,
			};

			WindowSaveData = new WindowSaveData();

			StreamWindows = new HashSet<LauncherItemStreamWindow>();
			OtherWindows = new HashSet<Window>();

			IndexBodyCaching = new IndexBodyCaching(
				Constants.CacheIndexNote,
				Constants.CacheIndexTemplate,
				Constants.CacheIndexClipboard
			);
		}
		///// <summary>
		///// dummy init
		///// </summary>
		//public MainWorkerViewModel() 
		//{
		//	CommonData = new CommonData() {
		//		NoteIndexSetting = new NoteIndexSettingModel(),
		//		TemplateIndexSetting = new TemplateIndexSettingModel(),
		//		ClipboardIndexSetting = new ClipboardIndexSettingModel(),
		//	};
		//	LauncherToolbarWindowList = new List<LauncherToolbarWindow>();
		//	NoteWindowList = new List<NoteWindow>();
		//	LoggingWindow = new LoggingWindow();
		//}

		#region property

		CommonData CommonData { get; set; }
		public LanguageManager Language { get { return CommonData.Language; } }

		public bool Pause { get; set; }

		public bool IsContextMenuOpen 
		{
			get { return this._isContextMenuOpen; }
			set
			{
				if (SetVariableValue(ref this._isContextMenuOpen, value)) {
					Pause = IsContextMenuOpen;
				}
			}
		}

		HashSet<LauncherItemStreamWindow> StreamWindows { get; set; }
		HashSet<Window> OtherWindows { get; set; }

		LoggingWindow LoggingWindow { get; set; }
		public LoggingViewModel Logging { get { return LoggingWindow.ViewModel; } }

		List<LauncherToolbarWindow> LauncherToolbarWindows { get; set; }
		public IEnumerable<LauncherToolbarViewModel> LauncherToolbars { get { return LauncherToolbarWindows.Select(l => l.ViewModel); } }

		List<NoteWindow> NoteWindows { get; set; }
		public IEnumerable<NoteViewModel> NoteShowItems { get { return NoteWindows.Select(w => w.ViewModel); } }
		public IEnumerable<NoteMenuViewModel> NoteHiddenItems { get { return CommonData.NoteIndexSetting.Items.Where(n => !n.Visible).Select(n => new NoteMenuViewModel(n, CommonData.NonProcess, CommonData.AppSender)); } }

		MessageWindow MessageWindow { get; set; }

		WindowSaveData WindowSaveData { get; set; }

		public ImageSource ApplicationIcon
		{
			get
			{
				//TODO: 自前で生成したいけどHardcodet.Wpf.TaskbarNotificationの都合上厳しい
//#if DEBUG
//				var path = "/Resources/Icon/Tasktray/App-debug.ico";
//#elif BETA
//				var path = "/Resources/Icon/Tasktray/App-beta.ico";
//#else
//				var path = "/Resources/Icon/Tasktray/App-release.ico";
//#endif
				var uri = SharedConstants.GetEntryUri(AppResource.ApplicationTasktrayPath);
				return new BitmapImage(uri);
			}
		}

		DispatcherTimer WindowSaveTimer { get; set; }

		public IEnumerable<WindowItemCollectionViewModel> WindowTimerItems
		{
			get { return WindowSaveData.TimerItems.Select(w => new WindowItemCollectionViewModel(w)); }
		}
		public IEnumerable<WindowItemCollectionViewModel> WindowSystemItems
		{
			get { return WindowSaveData.SystemItems.Select(w => new WindowItemCollectionViewModel(w)); }
		}

		public bool IsVisibledShellHideFile { get { return SystemEnvironmentUtility.IsHideFileShow(); } }
		public bool IsVisibledShellExtension { get { return SystemEnvironmentUtility.IsExtensionShow(); } }

		TemplateWindow TemplateWindow { get; set; }
		public TemplateViewModel Template { get { return TemplateWindow.ViewModel; } }

		ClipboardWindow ClipboardWindow { get; set; }
		public ClipboardViewModel Clipboard { get { return ClipboardWindow.ViewModel; } }

		IndexBodyCaching IndexBodyCaching { get; set; }

		#endregion

		#region command

		public ICommand OpenContextMenuCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						LanguageUtility.RecursiveSetLanguage((ContextMenu)o, CommonData.Language);
					}
				);

				return result;
			}
		}

		/// <summary>
		/// 設定ウィンドウ表示。
		/// </summary>
		public ICommand ShowSettingWindowCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var window = new SettingWindow();
						window.SetCommonData(CommonData, null);
						window.ShowDialog();
					}
				);

				return result;
			}
		}

		public ICommand SwitchTemplateWindowCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Debug.Assert(Template != null);
						Template.Visible = !Template.Visible;
					}
				);

				return result;
			}
		}

		public ICommand SwitchCommandWindowCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
					}
				);

				return result;
			}
		}

		/// <summary>
		/// ログウィンドウ切り替え。
		/// </summary>
		public ICommand SwitchLoggingWindowCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Debug.Assert(Logging != null);
						Logging.Visible = !Logging.Visible;
					}
				);

				return result;
			}
		}

		public ICommand SwitchClipboardWindowCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Debug.Assert(Clipboard != null);
						Clipboard.Visible = !Clipboard.Visible;
					}
				);

				return result;
			}
		}

		/// <summary>
		/// プログラム終了。
		/// </summary>
		public ICommand ExitApplicationCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						SaveSetting();
						Application.Current.Shutdown();
					}
				);

				return result;
			}
		}

		public ICommand CreateNoteItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var point = new Point();
						var size = new Size(200, 200);
						CreateNoteItem(point, size, true); 
					}
				);

				return result;
			}
		}

		public ICommand CompactNoteItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						foreach(var vm in GetEnabledNoteItems()) {
							vm.IsCompacted = true;
						}
					}
				);

				return result;
			}
		}

		public ICommand HideNoteItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						foreach(var window in NoteWindows.Where(n => !n.ViewModel.IsLocked).ToArray()) {
							window.UserClose();
						}
					}
				);

				return result;
			}
		}

		public ICommand FrontNoteItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						foreach(var window in NoteWindows) {
							WindowsUtility.ShowNoActive(window.Handle);
						}
					}
				);

				return result;
			}
		}

		public ICommand SaveTemporaryWindow
		{
			get
			{
				var result = CreateCommand(
					o => {
						SaveWindowItemAsync(WindowSaveType.Temporary);
						//CommandManager.InvalidateRequerySuggested();
					}
				);

				return result;
			}
		}
		public ICommand LoadTemporaryWindow
		{
			get
			{
				var result = CreateCommand(
					o => {
						AppUtility.ChangeWindowFromWindowList(WindowSaveData.TemporaryItem);
					},
					o => {
						return WindowSaveData.TemporaryItem != null;
					}
				);

				return result;
			}
		}

		public ICommand SwitchShellHideFile
		{
			get
			{
				var result = CreateCommand(
					o => {
						SystemEnvironmentUtility.SetHideFileShow(!IsVisibledShellHideFile);
						SystemEnvironmentUtility.RefreshShell();
						OnPropertyChanged("IsVisibledShellHideFile");
					}
				);

				return result;
			}
		}

		public ICommand SwitchShellExtension
		{
			get
			{
				var result = CreateCommand(
					o => {
						SystemEnvironmentUtility.SetExtensionShow(!IsVisibledShellExtension);
						SystemEnvironmentUtility.RefreshShell();
						OnPropertyChanged("IsVisibledShellExtension");
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		void LoadSetting()
		{
			// TODO: 環境変数展
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				// 各種設定の読込
				CommonData.MainSetting = AppUtility.LoadSetting<MainSettingModel>(CommonData.VariableConstants.UserSettingFileMainSettingPath, FileType.Json, CommonData.Logger);
				CommonData.LauncherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, FileType.Json, CommonData.Logger);
				CommonData.LauncherGroupSetting = AppUtility.LoadSetting<LauncherGroupSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, FileType.Json, CommonData.Logger);
				// 言語ファイル
				CommonData.Language = AppUtility.LoadLanguageFile(CommonData.VariableConstants.ApplicationLanguageDirectoryPath, CommonData.MainSetting.Language.Name, CommonData.VariableConstants.LanguageCode, CommonData.Logger);
				// インデックスファイル読み込み
				CommonData.NoteIndexSetting = AppUtility.LoadSetting<NoteIndexSettingModel>(CommonData.VariableConstants.UserSettingNoteIndexFilePath, FileType.Json, CommonData.Logger);
				CommonData.ClipboardIndexSetting = AppUtility.LoadSetting<ClipboardIndexSettingModel>(CommonData.VariableConstants.UserSettingClipboardIndexFilePath, FileType.Json, CommonData.Logger);
				CommonData.TemplateIndexSetting = AppUtility.LoadSetting<TemplateIndexSettingModel>(CommonData.VariableConstants.UserSettingTemplateIndexFilePath, FileType.Json, CommonData.Logger);
			}
		}

		void SaveSetting()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileMainSettingPath, CommonData.MainSetting, FileType.Json, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.LauncherItemSetting, FileType.Json, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.LauncherGroupSetting, FileType.Json, CommonData.Logger);

				SendSaveIndex(IndexKind.Note);
				SendSaveIndex(IndexKind.Clipboard);
				SendSaveIndex(IndexKind.Template);
			}
		}

		/// <summary>
		///プログラム実行を準備。
		/// </summary>
		public bool Initialize()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {

				LoadSetting();
				// 前回バージョンが色々必要なのでインクリメント前の生情報を保持しておく。
				var previousVersion = (Version)CommonData.MainSetting.RunningInformation.LastExecuteVersion;
				ResetCulture(CommonData.NonProcess);
				if(!InitializeAccept()) {
					return false;
				}
				InitializeSetting(previousVersion);
				InitializeStatus();
				InitializeSystemEvent();
				InitializeStatic();

				CreateMessage();
				CreateLogger();

				CreateToolbar();
				CreateNote();
				CreateTemplate();
				CreateClipboard();

				return true;
			}
		}

		/// <summary>
		/// 使用許諾まわり。
		/// </summary>
		bool InitializeAccept()
		{
			if(SettingUtility.CheckAccept(CommonData.MainSetting.RunningInformation, CommonData.NonProcess)) {
				SettingUtility.IncrementRunningInformation(CommonData.MainSetting.RunningInformation);
			} else {
				// 使用許諾表示前に使用しない状態にしておく。
				CommonData.MainSetting.RunningInformation.Accept = false;
				var window = new AcceptWindow();
				window.SetCommonData(CommonData, null);
				window.ShowDialog();
				if(CommonData.MainSetting.RunningInformation.Accept) {
					CommonData.Logger.Information("accept: OK");
					SettingUtility.IncrementRunningInformation(CommonData.MainSetting.RunningInformation);
				} else {
					CommonData.Logger.Information("accept: NG");
					return false;
				}
			}

			return true;
		}

		void InitializeSetting(Version previousVersion)
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				SettingUtility.InitializeMainSetting(CommonData.MainSetting, previousVersion, CommonData.NonProcess);
				SettingUtility.InitializeLauncherItemSetting(CommonData.LauncherItemSetting, previousVersion, CommonData.NonProcess);
				SettingUtility.InitializeLauncherGroupSetting(CommonData.LauncherGroupSetting, previousVersion, CommonData.NonProcess);
			}
		}

		void InitializeStatus()
		{
			WindowSaveData.TimerItems.LimitSize = CommonData.MainSetting.WindowSave.SaveCount;
			WindowSaveData.SystemItems.LimitSize = CommonData.MainSetting.WindowSave.SaveCount;
			
			if (WindowSaveTimer != null) {
				WindowSaveTimer.Stop();
			}

			WindowSaveTimer = new DispatcherTimer();
			WindowSaveTimer.Tick += Timer_Tick;
			WindowSaveTimer.Interval = CommonData.MainSetting.WindowSave.SaveIntervalTime;
			//@ WindowSaveTimer.Start();
		}

		void InitializeSystemEvent()
		{
			SystemEvents.DisplaySettingsChanging += SystemEvents_DisplaySettingsChanging;
		}

		void InitializeStatic()
		{
			LauncherListDisplayImageConverter.LauncherIconCaching = CommonData.LauncherIconCaching;
			LauncherListDisplayImageConverter.NonProcess = CommonData.NonProcess;
			LauncherListDisplayImageConverter.AppSender = CommonData.AppSender;
		}

		/// <summary>
		/// メッセージウィンドウ作成
		/// </summary>
		void CreateMessage()
		{
			MessageWindow = new MessageWindow();
			MessageWindow.SetCommonData(CommonData, null);
			MessageWindow.Show();
		}

		/// <summary>
		/// ログの生成。
		/// </summary>
		void CreateLogger()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				LoggingWindow = new LoggingWindow();
				LoggingWindow.SetCommonData(CommonData, null);

				var appLogger = (AppLogger)CommonData.Logger;
				appLogger.LogCollector = Logging;
				if(appLogger.IsStock) {
					// 溜まったログをViewにドバー
					foreach(var logItem in appLogger.StockItems) {
						appLogger.LogCollector.AddLog(logItem);
					}
					appLogger.IsStock = false;
				}
			}
		}

		/// <summary>
		/// ツールバーの生成。
		/// </summary>
		void CreateToolbar()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				LauncherToolbarWindows = new List<LauncherToolbarWindow>();

				foreach(var screen in Screen.AllScreens.OrderBy(s => !s.Primary)) {
					//var toolbar = new LauncherToolbarWindow();
					//toolbar.SetCommonData(CommonData, screen);
					SendCreateWindow(WindowKind.LauncherToolbar, screen, null);
				}
			}
		}

		void CreateNote()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				NoteWindows = new List<NoteWindow>();

				foreach(var noteItem in CommonData.NoteIndexSetting.Items.Where(n => n.Visible)) {
					var window = CreateNoteWindow(noteItem, false);
				}
			}
		}

		void CreateTemplate()
		{
			TemplateWindow = new TemplateWindow();
			TemplateWindow.SetCommonData(CommonData, null);
		}

		void CreateClipboard()
		{
			ClipboardWindow = new ClipboardWindow();
			ClipboardWindow.SetCommonData(CommonData, null);
		}

		/// <summary>
		/// ディスプレイ数に変更があった。
		/// </summary>
		void ChangedScreenCount()
		{
		}

		/// <summary>
		/// <para>TODO: これで完結できるならSettingUtilityに移動する。</para>
		/// </summary>
		/// <param name="point"></param>
		/// <param name="size"></param>
		/// <param name="appendIndex"></param>
		/// <returns></returns>
		NoteWindow CreateNoteItem([PixelKind(Px.Logical)] Point point, [PixelKind(Px.Logical)] Size size, bool appendIndex)
		{
			var noteItem = new NoteIndexItemModel() {
				WindowLeft = point.X,
				WindowTop = point.Y,
				WindowWidth = size.Width,
				WindowHeight = size.Height,
				Visible = true,
				ForeColor = CommonData.MainSetting.Note.ForeColor,
				BackColor = CommonData.MainSetting.Note.BackColor,
				// TODO: note title
				Name = "TODO: note title",
			};
			noteItem.Font.Family = SystemFonts.MessageFontFamily.Source;

			return CreateNoteWindow(noteItem, appendIndex);
		}

		NoteWindow CreateNoteWindow(NoteIndexItemModel noteItem, bool appendIndex)
		{
			var window = (NoteWindow)SendCreateWindow(WindowKind.Note, noteItem, null);
			if(appendIndex) {
				CommonData.NoteIndexSetting.Items.Add(noteItem);
			}

			return window;
		}

		static void ResetCulture(INonProcess nonProcess)
		{
			try {
				var cultureInfo = new CultureInfo(nonProcess.Language.CultureCode);
				CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
				CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
			} catch (CultureNotFoundException ex) {
				nonProcess.Logger.Trace(ex);
			}
			
		}

		IEnumerable<NoteViewModel> GetEnabledNoteItems()
		{
			return NoteShowItems
				.Where(n => !n.IsLocked)
				.Where(n => !n.IsCompacted)
			;
		}

		WindowItemCollectionModel SaveWindowItem(WindowSaveType type)
		{
			var windowList = AppUtility.GetSystemWindowList(false);
			var windowCollection = new WindowItemCollectionModel();
			foreach (var window in windowList) {
				windowCollection.Add(window);
			}

			windowCollection.Name = "TODO:" + DateTime.Now.ToString();

			switch(type) {
				case WindowSaveType.Temporary:
					WindowSaveData.TemporaryItem = windowCollection;
					break;

				case WindowSaveType.Timer:
					WindowSaveData.TimerItems.Add(windowCollection);
					OnPropertyChanged("WindowTimerItems");
					break;

				case WindowSaveType.System:
					WindowSaveData.SystemItems.Add(windowCollection);
					OnPropertyChanged("WindowSystemItems");
					break;
			}
			CommonData.Logger.Information("save window", windowCollection);

			return windowCollection;
		}

		Task<WindowItemCollectionModel> SaveWindowItemAsync(WindowSaveType type)
		{
			return Task.Run(() => SaveWindowItem(type));
		}

		#endregion

		#region ViewModelBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				CommonData.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

		#region IAppSender

		public void SendAppendWindow(Window window)
		{
			ReceiveAppendWindow(window);
		}

		public Window SendCreateWindow(WindowKind kind, object extensionData, Window parent)
		{
			return ReceiveCreateWindow(kind, extensionData, parent);
		}

		public void SendRefreshView(WindowKind kind, Window fromView)
		{
			ReceiveRefreshView(kind, fromView);
		}

		public void SendRemoveIndex(IndexKind indexKind, Guid guid)
		{
			ReceiveRemoveIndex(indexKind, guid);
		}

		public void SendSaveIndex(IndexKind indexKind)
		{
			ReceiveSaveIndex(indexKind);
		}

		public IndexBodyItemModelBase SendLoadIndexBody(IndexKind indexKind, Guid guid)
		{
			return ReceiveLoadIndexBody(indexKind, guid);
		}

		public void SendSaveIndexBody(IndexBodyItemModelBase indexBody, Guid guid)
		{
			ReceiveSaveIndexBody(indexBody, guid);
		}

		public void SendDeviceChanged(ChangedDevice changedDevice)
		{
			ReceiveDeviceChanged(changedDevice);
		}

		public void SendClipboardChanged()
		{
			ReceiveClipboardChanged();
		}

		public void SendInputHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel)
		{
			ReceiveHotKey(hotKeyId, hotKeyModel);
		}

		#region IAppSender-Implement

		void ReceiveAppendWindow(Window window)
		{
			window.Closed += Window_Closed;

			var windowKind = window as IHavingWindowKind;
			if (windowKind != null) {
				switch(windowKind.WindowKind) {
					case WindowKind.LauncherToolbar:
						{
							var toolbarWindow = (LauncherToolbarWindow)window;
							LauncherToolbarWindows.Add(toolbarWindow);
						}
						break;

					case WindowKind.LauncherExecute:
					case WindowKind.LauncherCustomize:
						{
							OtherWindows.Add(window);
						}
						break;

					case WindowKind.LauncherStream: 
						{
							var streamWindow = (LauncherItemStreamWindow)window;
							StreamWindows.Add(streamWindow);
						}
						break;

					case WindowKind.Note: 
						{
							var noteWindow = (NoteWindow)window;
							NoteWindows.Add(noteWindow);

							OnPropertyChanged("NoteShowItems");
							OnPropertyChanged("NoteHiddenItems");
						}
						break;

					default:
						throw new NotImplementedException();
				}
			} else {
				OtherWindows.Add(window);
			}
		}

		void RemoveWindow(Window window)
		{
			var havingWindwKind = window as IHavingWindowKind;
			if(havingWindwKind != null) {
				switch(havingWindwKind.WindowKind) {
					case WindowKind.LauncherToolbar: 
						{
							var toolbarWindow = (LauncherToolbarWindow)window;
							LauncherToolbarWindows.Remove(toolbarWindow);
						}
						break;

					case WindowKind.LauncherExecute:
					case WindowKind.LauncherCustomize: 
						{
							OtherWindows.Remove(window);
						}
						break;

					case WindowKind.LauncherStream: 
						{
							var streamWindow = (LauncherItemStreamWindow)window;
							StreamWindows.Remove(streamWindow);
						}
						break;

					case WindowKind.Note:
						{
							var noteWindow = (NoteWindow)window;
							NoteWindows.Remove(noteWindow);

							OnPropertyChanged("NoteShowItems");
							OnPropertyChanged("NoteHiddenItems");
							break;
						}

					default:
						throw new NotImplementedException();
				}
			} else {
				OtherWindows.Remove(window);
			}
		}

		public Window ReceiveCreateWindow(WindowKind kind, object extensionData, Window parent)
		{
			CommonDataWindow window = null;

			switch(kind) {
				case WindowKind.LauncherToolbar:
					{
						window = new LauncherToolbarWindow();
						window.SetCommonData(CommonData, (ScreenModel)extensionData);
						break;
					}

				case WindowKind.LauncherExecute:
					{
						window = new LauncherItemExecuteWindow();
						window.SetCommonData(CommonData, extensionData);
						break;
					}

				case WindowKind.LauncherCustomize: 
					{
						window = new LauncherItemCustomizeWindow();
						window.SetCommonData(CommonData, extensionData);
						break;
					}

				case WindowKind.LauncherStream:
					{
						window = new LauncherItemStreamWindow();
						window.SetCommonData(CommonData, extensionData);
						break;
					}

				case WindowKind.Note: 
					{
						var noteItem = (NoteIndexItemModel)extensionData;
						if(!noteItem.Visible) {
							CommonData.Logger.Trace("hidden -> show", noteItem);
							noteItem.Visible = true;
						}
						window = new NoteWindow();
						window.SetCommonData(CommonData, noteItem);
						break;
					}

				default:
					throw new NotImplementedException();
			}

			SendAppendWindow(window);
			return window;
		}

		public void ReceiveRefreshView(WindowKind kind, Window fromView)
		{
			switch(kind) {
				case WindowKind.LauncherToolbar:
					{
						foreach(var toolbar in LauncherToolbars) {
							toolbar.Refresh();
						}
					}
					break;

				default:
					throw new NotImplementedException();
			}
		}

		void RemoveIndex<TItemModel, TIndexBody>(IndexKind indexKind, Guid guid, IndexItemCollectionModel<TItemModel> items, IndexBodyPairItemCollection<TIndexBody> cachingItems)
			where TItemModel : IndexItemModelBase
			where TIndexBody : IndexBodyItemModelBase
		{
			var index = cachingItems.IndexOf(guid);
			if (index != -1) {
				var pair = cachingItems[index];
				Debug.Assert(pair.Id == guid);
				cachingItems.RemoveAt(index);
				CommonData.Logger.Trace("remove cache dispose: " + pair.Id.ToString(), pair.Body);
				pair.Body.Dispose();
			}
			items.Remove(guid);

			SendSaveIndex(indexKind);
		}

		void ReceiveRemoveIndex(IndexKind indexKind, Guid guid)
		{
			switch(indexKind) {
				case IndexKind.Note: 
					{
						RemoveIndex(indexKind, guid, CommonData.NoteIndexSetting.Items, IndexBodyCaching.NoteItems);
					}
					break;

				case IndexKind.Template:
					{
						RemoveIndex(indexKind, guid, CommonData.TemplateIndexSetting.Items, IndexBodyCaching.TemplateItems);
					}
					break;

				case IndexKind.Clipboard: 
					{
						RemoveIndex(indexKind, guid, CommonData.ClipboardIndexSetting.Items, IndexBodyCaching.ClipboardItems);
					}
					break;

				default:
					throw new NotImplementedException();
			}
		}

		void SaveIndex<TIndexSetting>(IndexKind indexKind, TIndexSetting indexSetting, string filePath)
			where TIndexSetting : ModelBase
		{
			var path = Environment.ExpandEnvironmentVariables(filePath);
			AppUtility.SaveSetting(path, indexSetting, FileType.Json, CommonData.Logger);
		}

		void ReceiveSaveIndex(IndexKind indexKind)
		{
			switch(indexKind) {
				case IndexKind.Note:
					{
						SaveIndex(indexKind, CommonData.NoteIndexSetting, CommonData.VariableConstants.UserSettingNoteIndexFilePath);
					}
					break;

				case IndexKind.Template:
					{
						SaveIndex(indexKind, CommonData.TemplateIndexSetting, CommonData.VariableConstants.UserSettingTemplateIndexFilePath);
					}
					break;

				case IndexKind.Clipboard:
					{
						SaveIndex(indexKind, CommonData.ClipboardIndexSetting, CommonData.VariableConstants.UserSettingClipboardIndexFilePath);
					}
					break;

				default:
					throw new NotImplementedException();
			}
		}

		void AppendCachingItems<TIndexBody>(Guid guid, TIndexBody indexBody, IndexBodyPairItemCollection<TIndexBody> cachingItems)
			where TIndexBody : IndexBodyItemModelBase
		{
			if (!cachingItems.Any(p => p.Id == guid)) {
				var pairItem = new IndexBodyPairItem<TIndexBody>(guid, indexBody);
				cachingItems.Add(pairItem);
				if (cachingItems.StockItems.Any()) {
					var itemPairList = cachingItems.StockItems.ToArray();
					cachingItems.StockItems.Clear();
					foreach (var pair in itemPairList) {
						CommonData.Logger.Trace("cache dispose: " + pair.Id.ToString(), pair.Body);
						pair.Body.Dispose();
					}
				}
			}
		}

		IndexBodyItemModelBase GetIndexBody<TIndexBody>(IndexKind indexKind, Guid guid, IndexBodyPairItemCollection<TIndexBody> cachingItems, string dirPath, FileType fileType)
			where TIndexBody : IndexBodyItemModelBase, new()
		{
			var body = cachingItems.GetFromId(guid);
			if (body != null) {
				CommonData.Logger.Debug("load cache: " + guid.ToString(), body);
				return body;
			}
			var fileName = IndexItemUtility.GetIndexBodyFileName(indexKind, fileType, guid);
			var path = Environment.ExpandEnvironmentVariables(Path.Combine(dirPath, fileName));
			var result = AppUtility.LoadSetting<TIndexBody>(path, fileType, CommonData.Logger);
			AppendCachingItems(guid, result, cachingItems);
			return result;
		}

		public IndexBodyItemModelBase ReceiveLoadIndexBody(IndexKind indexKind, Guid guid)
		{
			switch(indexKind) {
				case IndexKind.Note: 
					{
						return GetIndexBody<NoteBodyItemModel>(indexKind, guid, IndexBodyCaching.NoteItems, CommonData.VariableConstants.UserSettingNoteDirectoryPath, FileType.Json);
					}

				case IndexKind.Template: 
					{
						return GetIndexBody<TemplateBodyItemModel>(indexKind, guid, IndexBodyCaching.TemplateItems, CommonData.VariableConstants.UserSettingTemplateDirectoryPath, FileType.Json);
					}

				case IndexKind.Clipboard: 
					{
						return GetIndexBody<ClipboardBodyItemModel>(indexKind, guid, IndexBodyCaching.ClipboardItems, CommonData.VariableConstants.UserSettingClipboardDirectoryPath, FileType.Binary);
					}

				default:
					throw new NotImplementedException();
			}
		}

		void SaveIndexBody<TIndexBody>(IndexBodyItemModelBase indexBody, Guid guid, IndexBodyPairItemCollection<TIndexBody> cachingItems, string dirPath, FileType fileType)
			where TIndexBody : IndexBodyItemModelBase
		{
			var fileName = IndexItemUtility.GetIndexBodyFileName(indexBody.IndexKind, fileType, guid);
			var path = Environment.ExpandEnvironmentVariables(Path.Combine(dirPath, fileName));
			var bodyItem = (TIndexBody)indexBody;
			AppUtility.SaveSetting(path, bodyItem, fileType, CommonData.Logger);
			AppendCachingItems(guid, bodyItem, cachingItems);
		}

		void ReceiveSaveIndexBody(IndexBodyItemModelBase indexBody, Guid guid)
		{
			switch (indexBody.IndexKind) {
				case IndexKind.Note:
					{
						SaveIndexBody<NoteBodyItemModel>(indexBody, guid, IndexBodyCaching.NoteItems, CommonData.VariableConstants.UserSettingNoteDirectoryPath, FileType.Json);
					}
					break;

				case IndexKind.Template: 
					{
						SaveIndexBody<TemplateBodyItemModel>(indexBody, guid, IndexBodyCaching.TemplateItems, CommonData.VariableConstants.UserSettingTemplateDirectoryPath, FileType.Json);
					}
					break;

				case IndexKind.Clipboard: 
					{
						SaveIndexBody<ClipboardBodyItemModel>(indexBody, guid, IndexBodyCaching.ClipboardItems, CommonData.VariableConstants.UserSettingClipboardDirectoryPath, FileType.Binary);
					}
					break;

				default:
					throw new NotImplementedException();
			}
		}

		void ReceiveDeviceChanged(ChangedDevice changedDevice)
		{
			CommonData.Logger.Information("catch: changed device");
			// TODO: まだ作ってないので暫定的に。
			var Initialized = true;

			// デバイス状態が変更されたか
			if(changedDevice.DBT == DBT.DBT_DEVNODES_CHANGED && Initialized && !Pause) {
				// デバイス変更前のスクリーン数が異なっていればディスプレイの抜き差しが行われたと判定する
				// 現在生成されているツールバーの数が前回ディスプレイ数となる

				// 変更通知から現在数をAPIでまともに取得する
				var rawScreenCount = NativeMethods.GetSystemMetrics(SM.SM_CMONITORS);
				bool changedScreenCount = LauncherToolbars.Count() != rawScreenCount;

				Task.Run(() => {
					// Forms で取得するディスプレイ数の合計値は少し遅れる
					const int waitMax = Constants.screenCountChangeRetryCount;
					int waitCount = 0;

					var managedScreenCount = Screen.AllScreens.Count();
					while(rawScreenCount != managedScreenCount) {
						if(waitMax < ++waitCount) {
							// タイムアウト
							break;
						}
						Thread.Sleep(Constants.screenCountChangeWaitTime);
						managedScreenCount = Screen.AllScreens.Count();
					}
				}).ContinueWith(t => {
					if(changedScreenCount) {
						ChangedScreenCount();
					}
				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
		}

		void ReceiveClipboardChanged()
		{
			if(!CommonData.MainSetting.Clipboard.Enabled) {
				return;
			}

			var seq = NativeMethods.GetClipboardSequenceNumber();
			if (this._clipboardPreviousSequenceNumber == seq) {
				return;
			}
			this._clipboardPreviousSequenceNumber = seq;

			var now = DateTime.Now;
			if (now - this._clipboardPreviousTime  <= CommonData.MainSetting.Clipboard.WaitTime) {
				CommonData.Logger.Information("clipboard time");
				return;
			}
			this._clipboardPreviousTime = now;

			try {
				var clipboardItem = ClipboardUtility.CreateClipboardItem(CommonData.MainSetting.Clipboard.EnabledClipboardTypes, MessageWindow.Handle, CommonData.Logger);
				if (clipboardItem.Type != ClipboardType.None) {
					Task.Run(() => {
						//clipboardItem.Name = displayText;
						if (Clipboard.IndexItems.Any()) {
							if (CommonData.MainSetting.Clipboard.DuplicationCount == 0) {
								// 範囲チェックを行わないのであれば無条件で追加
								return true;
							}

							// 毎回ファイル読むのもなぁ
							//// 指定範囲内に同じデータがあれば追加しない
							//IEnumerable<ClipboardItem> clipboardItems = this._commonData.MainSetting.Clipboard.HistoryItems;
							//if (this._commonData.MainSetting.Clipboard.ClipboardRepeated != Constants.clipboardDuplicationCount.minimum) {
							//	clipboardItems = clipboardItems.Take(this._commonData.MainSetting.Clipboard.ClipboardRepeated);
							//}
							//var hitItem = clipboardItems.FirstOrDefault(c => ClipboardUtility.EqualClipboardItem(c, clipboardItem));
							//return hitItem == null;
						}
						return true;
					}).ContinueWith(t => {
						Debug.WriteLine("Clipboard: " + t.Result);
						if (t.Result) {
							try {
								//this._commonData.MainSetting.Clipboard.HistoryItems.Insert(0, clipboardItem);
								//Clipboard.IndexItems.Insert();
								//var body = ReceiveGetIndexBody(IndexKind.Template)
								var displayText = DisplayTextUtility.MakeClipboardName(clipboardItem, CommonData.NonProcess);
								var index = new ClipboardIndexItemModel() {
									Name = displayText,
									Type = clipboardItem.Type,
								};
								SendSaveIndexBody(clipboardItem.Body, index.Id);
								Clipboard.IndexPairList.Add(index, null);
							} catch (Exception ex) {
								CommonData.Logger.Error(ex);
							}
						} else {
							CommonData.Logger.Information("clipboard dup");
						}

						t.Dispose();
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			} catch (AccessViolationException ex) {
				// #251
				CommonData.Logger.Error(ex);
				//this._commonData.Logger.Puts(LogType.Error, ex.Message, ex);
			}
		}

		public void ReceiveHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel)
		{
			if (Pause) {
				CommonData.Logger.Information("pause");
				return;
			}
			CommonData.Logger.Trace(hotKeyId.ToString(), hotKeyModel);
		}

		#endregion

		#endregion

		#region IClipboardWatcher

		public void ClipboardWatchingChange(bool watch)
		{
			if(watch) {
				MessageWindow.RegistClipboardListener();
			} else {
				MessageWindow.UnregistClipboardListener();
			}
		}

		public bool ClipboardWatching { get { return MessageWindow.ClipboardListenerRegisted; } }

		public bool ClipboardEnabledApplicationCopy { get { return CommonData.MainSetting.Clipboard.EnabledApplicationCopy; } }

		public bool UsingClipboard
		{
			get { return CommonData.MainSetting.Clipboard.UsingClipboard; }
			set { SetPropertyValue(CommonData.MainSetting.Clipboard, value); }
		}

		#endregion


		void Timer_Tick(object sender, EventArgs e)
		{
			if (Pause) {
				CommonData.Logger.Information("pause");
				return;
			}

			var timer = (DispatcherTimer)sender;
			timer.Stop();
			try {
				if (timer == WindowSaveTimer) {
					SaveWindowItemAsync(WindowSaveType.Timer);
				}
			} finally {
				timer.Start();
			}
		}

		void SystemEvents_DisplaySettingsChanging(object sender, EventArgs e)
		{
			SaveWindowItem(WindowSaveType.System);
		}

		void Window_Closed(object sender, EventArgs e)
		{
			RemoveWindow(sender as Window);
		}


	}
}
