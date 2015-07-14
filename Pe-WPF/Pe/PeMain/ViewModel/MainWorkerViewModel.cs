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

	public sealed class MainWorkerViewModel: ViewModelBase, IAppSender, IClipboardWatcher
	{
		public MainWorkerViewModel(VariableConstants variableConstants, ILogger logger)
		{
			CommonData = new CommonData() {
				Logger = logger,
				VariableConstants = variableConstants,
				AppSender = this,
				ClipboardWatcher = this,
			};
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

		LoggingWindow LoggingWindow { get; set; }
		public LoggingViewModel Logging { get { return LoggingWindow.ViewModel; } }

		List<LauncherToolbarWindow> LauncherToolbarWindowList { get; set; }
		public IEnumerable<LauncherToolbarViewModel> LauncherToolbar { get { return LauncherToolbarWindowList.Select(l => l.ViewModel); } }

		List<NoteWindow> NoteWindowList { get; set; }
		public IEnumerable<NoteViewModel> NoteShowItems { get { return NoteWindowList.Select(w => w.ViewModel); } }
		public IEnumerable<NoteMenuViewModel> NoteHiddenItems { get { return CommonData.NoteIndexSetting.Items.Where(n => !n.Visible).Select(n => new NoteMenuViewModel(n, CommonData)); } }

		MessageWindow MessageWindow { get; set; }
		List<Window> WindowList { get; set; }

		public ImageSource ApplicationIcon
		{
			get
			{
				//TODO: 自前で生成したいけどHardcodet.Wpf.TaskbarNotificationの都合上厳しい
#if DEBUG
				var path = "/Resources/Icon/Tasktray/App-debug.ico";
#elif BETA
				var path = "/Resources/Icon/Tasktray/App-beta.ico";
#else
				var path = "/Resources/Icon/Tasktray/App-release.ico";
#endif
				var uri = SharedConstants.GetEntryUri(path);
				return new BitmapImage(uri);
			}
		}

		#endregion

		#region command

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
						foreach(var window in NoteWindowList.Where(n => !n.ViewModel.IsLocked).ToArray()) {
							window.UserClose();
						}
					}
				);

				return result;
			}
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

		public void SendWindowAppend(Window window)
		{
			ReceiveWindowAppend(window);
		}

		public void SendWindowRemove(Window window)
		{
			ReceiveWindowRemove(window);
		}

		public void SendIndexRemove(IndexKind indexKind, Guid guid)
		{
			ReceiveIndexRemove(indexKind, guid);
		}

		public void SendIndexSave(IndexKind indexKind)
		{
			ReceiveIndexSave(indexKind);
		}

		public IndexBodyItemModelBase SendGetIndexBody(IndexKind indexKind, Guid guid)
		{
			return ReceiveGetIndexBody(indexKind, guid);
		}

		public void SendSaveIndexBody(IndexKind indexKind, Guid guid, IndexBodyItemModelBase indexBody)
		{
			ReceiveSaveIndexBody(indexKind, guid, indexBody);
		}

		public void SendDeviceChanged(ChangedDevice changedDevice)
		{
			ReceiveDeviceChanged(changedDevice);
		}

		public void SendClipboardChanged()
		{
			ReceiveClipboardChanged();
		}

		public void SendHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel)
		{
			ReceiveHotKey(hotKeyId, hotKeyModel);
		}

		#region IAppSender-Implement

		void ReceiveWindowAppend(Window window)
		{
			var toolbarWindow = window as LauncherToolbarWindow;
			if(toolbarWindow != null) {
				LauncherToolbarWindowList.Add(toolbarWindow);
			}
			var noteWindow = window as NoteWindow;
			if (noteWindow != null) {
				NoteWindowList.Add(noteWindow);
			}
		}

		void ReceiveWindowRemove(Window window)
		{
			var noteWindow = window as NoteWindow;
			if(noteWindow != null) {
				NoteWindowList.Remove(noteWindow);
				//var noteViewMode = noteWindow.ViewModel;

				OnPropertyChanged("NoteShowItems");
				OnPropertyChanged("NoteHiddenItems");
			}
		}

		void ReceiveIndexRemove(IndexKind indexKind, Guid guid)
		{
			switch(indexKind) {
				case IndexKind.Note: 
					{
						CommonData.NoteIndexSetting.Items.Remove(guid);
						SendIndexSave(indexKind);
					}
					break;

				default:
					throw new NotImplementedException();
			}
		}

		void ReceiveIndexSave(IndexKind indexKind)
		{
			switch(indexKind) {
				case IndexKind.Note: 
					{
						var path = Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingNoteIndexFilePath);
						AppUtility.SaveSetting(path, CommonData.NoteIndexSetting, CommonData.Logger);
					}
					break;

				default:
					throw new NotImplementedException();
			}
		}

		public IndexBodyItemModelBase ReceiveGetIndexBody(IndexKind indexKind, Guid guid)
		{
			switch(indexKind) {
				case IndexKind.Note: 
					{
						var dirPath = CommonData.VariableConstants.UserSettingNoteDirectoryPath;
						var fileName = IndexItemUtility.GetIndexBodyFileName(guid);
						var path = Environment.ExpandEnvironmentVariables(Path.Combine(dirPath, fileName));
						return AppUtility.LoadSetting<NoteBodyItemModel>(path, CommonData.Logger);
					}

				default:
					throw new NotImplementedException();
			}
		}

		void ReceiveSaveIndexBody(IndexKind indexKind, Guid guid, IndexBodyItemModelBase indexBody)
		{
			switch(indexKind) {
				case IndexKind.Note: {
						var dirPath = CommonData.VariableConstants.UserSettingNoteDirectoryPath;
						var fileName = IndexItemUtility.GetIndexBodyFileName(guid);
						var path = Environment.ExpandEnvironmentVariables(Path.Combine(dirPath, fileName));
						AppUtility.SaveSetting(path, (NoteBodyItemModel)indexBody, CommonData.Logger);
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
				bool changedScreenCount = LauncherToolbar.Count() != rawScreenCount;

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
		}

		public void ReceiveHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel)
		{
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

		#endregion


		#region function

		void LoadSetting()
		{
			// TODO: 環境変数展
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				// 各種設定の読込
				CommonData.MainSetting = AppUtility.LoadSetting<MainSettingModel>(CommonData.VariableConstants.UserSettingFileMainSettingPath, CommonData.Logger);
				CommonData.LauncherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.Logger);
				CommonData.LauncherGroupSetting = AppUtility.LoadSetting<LauncherGroupSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.Logger);
				// 言語ファイル
				CommonData.Language = AppUtility.LoadLanguageFile(CommonData.VariableConstants.ApplicationLanguageDirectoryPath, CommonData.MainSetting.Language.Name, CommonData.VariableConstants.LanguageCode, CommonData.Logger);
				// インデックスファイル読み込み
				CommonData.NoteIndexSetting = AppUtility.LoadSetting<NoteIndexSettingModel>(CommonData.VariableConstants.UserSettingNoteIndexFilePath, CommonData.Logger);
				CommonData.ClipboardIndexSetting = AppUtility.LoadSetting<ClipboardIndexSettingModel>(CommonData.VariableConstants.UserSettingClipboardIndexFilePath, CommonData.Logger);
				CommonData.TemplateIndexSetting = AppUtility.LoadSetting<TemplateIndexSettingModel>(CommonData.VariableConstants.UserSettingTemplateIndexFilePath, CommonData.Logger);
			}
		}

		void SaveSetting()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileMainSettingPath, CommonData.MainSetting, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.LauncherItemSetting, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.LauncherGroupSetting, CommonData.Logger);

				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingNoteIndexFilePath, CommonData.NoteIndexSetting, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingClipboardIndexFilePath, CommonData.ClipboardIndexSetting, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingTemplateIndexFilePath, CommonData.TemplateIndexSetting, CommonData.Logger);
			}
		}

		/// <summary>
		///プログラム実行を準備。
		/// </summary>
		public bool Initialize()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {

				LoadSetting();
				if(!InitializeAccept()) {
					return false;
				}
				InitializeSetting();

				InitializeStatic();

				CreateMessage();

				CreateLogger();

				CreateToolbar();

				CreateNote();

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


		void InitializeSetting()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				SettingUtility.InitializeMainSetting(CommonData.MainSetting, CommonData.NonProcess);
				SettingUtility.InitializeLauncherItemSetting(CommonData.LauncherItemSetting, CommonData.NonProcess);
				SettingUtility.InitializeLauncherGroupSetting(CommonData.LauncherGroupSetting, CommonData.NonProcess);
			}
		}

		void InitializeStatic()
		{
			LauncherListDisplayImageConverter.LauncherIconCaching = CommonData.LauncherIconCaching;
			LauncherListDisplayImageConverter.NonProcess = CommonData.NonProcess;
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
				LauncherToolbarWindowList = new List<LauncherToolbarWindow>();

				foreach(var screen in Screen.AllScreens.OrderBy(s => !s.Primary)) {
					//var toolbar = new LauncherToolbarWindow();
					//toolbar.SetCommonData(CommonData, screen);
					//LauncherToolbarWindowList.Add(toolbar);
					ViewUtility.CreateToolbarWindow(screen, CommonData);
				}
			}
		}

		void CreateNote()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				NoteWindowList = new List<NoteWindow>();

				foreach(var noteItem in CommonData.NoteIndexSetting.Items.Where(n => n.Visible)) {
					var window = CreateNoteWindow(noteItem, false);
				}
			}
		}

		/// <summary>
		/// ディスプレイ数に変更があった。
		/// </summary>
		void ChangedScreenCount()
		{
		}

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
				Name = "TODO: note title",
			};

			return CreateNoteWindow(noteItem, appendIndex);
		}

		NoteWindow CreateNoteWindow(NoteIndexItemModel noteItem, bool appendIndex)
		{
			var window = ViewUtility.CreateNoteWindow(noteItem, CommonData);
			if(appendIndex) {
				CommonData.NoteIndexSetting.Items.Add(noteItem);
			}

			return window;
		}

		IEnumerable<NoteViewModel> GetEnabledNoteItems()
		{
			return NoteShowItems
				.Where(n => !n.IsLocked)
				.Where(n => !n.IsCompacted)
			;
		}

		#endregion
	}
}
