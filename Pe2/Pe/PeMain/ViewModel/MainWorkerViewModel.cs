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

		#region property

		CommonData CommonData { get; set; }
		public LanguageManager Language { get { return CommonData.Language; } }

		public bool Pause { get; set; }

		LoggingWindow LoggingWindow { get; set; }
		public LoggingViewModel Logging { get { return LoggingWindow.ViewModel; } }

		List<LauncherToolbarWindow> LauncherToolbarWindowList { get; set; }
		public IEnumerable<LauncherToolbarViewModel> LauncherToolbar { get { return LauncherToolbarWindowList.Select(l => l.ViewModel); } }

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
						window.SetCommonData(CommonData);
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

		public void SendDeviceChanged(ChangedDevice changedDevice)
		{
			ReceiveDeviceChanged(changedDevice);
		}

		public void SendClipboardChanged()
		{
			ReceiveClipboardChanged();
		}

		#region IAppSender-Implement

		void ReceiveWindowAppend(Window window)
		{ }

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
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				// 各種設定の読込
				CommonData.MainSetting = AppUtility.LoadSetting<MainSettingModel>(CommonData.VariableConstants.UserSettingFileMainSettingPath, CommonData.Logger);
				CommonData.LauncherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.Logger);
				CommonData.LauncherGroupSetting = AppUtility.LoadSetting<LauncherGroupSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.Logger);
				// 言語ファイル
				CommonData.Language = AppUtility.LoadLanguageFile(CommonData.VariableConstants.ApplicationLanguageDirectoryPath, CommonData.MainSetting.Language.Name, CommonData.VariableConstants.LanguageCode, CommonData.Logger);
			}
		}

		void SaveSetting()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileMainSettingPath, CommonData.MainSetting, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.LauncherItemSetting, CommonData.Logger);
				AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.LauncherGroupSetting, CommonData.Logger);
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

				CreateMessage();

				CreateLogger();

				CreateToolbar();

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
				window.SetCommonData(CommonData);
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

		/// <summary>
		/// メッセージウィンドウ作成
		/// </summary>
		void CreateMessage()
		{
			MessageWindow = new MessageWindow();
			MessageWindow.SetCommonData(CommonData);
			MessageWindow.Show();
		}

		/// <summary>
		/// ログの生成。
		/// </summary>
		void CreateLogger()
		{
			using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
				LoggingWindow = new LoggingWindow();
				LoggingWindow.SetCommonData(CommonData);

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
					var toolbar = new LauncherToolbarWindow(screen);
					toolbar.SetCommonData(CommonData);
					LauncherToolbarWindowList.Add(toolbar);
				}
			}
		}

		/// <summary>
		/// ディスプレイ数に変更があった。
		/// </summary>
		void ChangedScreenCount()
		{
		}

		#endregion
	}
}
