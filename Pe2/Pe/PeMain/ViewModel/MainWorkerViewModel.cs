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
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public sealed class MainWorkerViewModel: ViewModelBase, IDisposable
	{
		public MainWorkerViewModel(VariableConstants variableConstants, ILogger logger)
		{
			CommonData = new CommonData() {
				Logger = logger,
				VariableConstants = variableConstants,
			};
		}

		#region property

		CommonData CommonData { get; set; }

		public bool Pause { get; set; }

		public LoggingViewModel LoggingVM { get; set; }
		List<Window> WindowList { get; set; }


		#endregion

		#region command

		/// <summary>
		/// 設定ウィンドウ表示。
		/// </summary>
		public ICommand ShowSettingWindowCommand
		{
			get
			{
				var result = new DelegateCommand();
				result.Command = o => {
					var window = new SettingWindow();
					window.SetCommonData(CommonData);
					window.ShowDialog();
				};

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
				var result = new DelegateCommand();
				result.Command += o => {
					Debug.Assert(LoggingVM != null);

					LoggingVM.Visible = !LoggingVM.Visible;
				};

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
				var result = new DelegateCommand();
				
				result.Command = o => {
					SaveSetting();
					Application.Current.Shutdown();
				};

				return result;
			}
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			CommonData.Dispose();
		}

		#endregion

		#region function

		void LoadSetting()
		{
			// 各種設定の読込
			CommonData.MainSetting= AppUtility.LoadSetting<MainSettingModel>(CommonData.VariableConstants.UserSettingFileMainSettingPath, CommonData.Logger);
			CommonData.LauncherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.Logger);
			CommonData.LauncherGroupItemSetting = AppUtility.LoadSetting<LauncherGroupItemSettingModel>(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.Logger);
			// 言語ファイル
			CommonData.Language = AppUtility.LoadLanguageFile(CommonData.VariableConstants.ApplicationLanguageDirectoryPath, CommonData.MainSetting.Language.Name, CommonData.VariableConstants.LanguageCode, CommonData.Logger);
		}

		void SaveSetting()
		{
			AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileMainSettingPath, CommonData.MainSetting, CommonData.Logger);
			AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.LauncherItemSetting, CommonData.Logger);
			AppUtility.SaveSetting(CommonData.VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.LauncherGroupItemSetting, CommonData.Logger);
		}

		/// <summary>
		///プログラム実行を準備。
		/// </summary>
		public bool Initialize()
		{
			CommonData.Logger.Information("MainWorkerViewModel initialize");

			LoadSetting();

			if(!InitializeAccept()) {
				return false;
			}

			InitializeSetting();

			CreateLogger();

			CreateToolbar();

			return true;
		}

		/// <summary>
		/// 使用許諾まわり。
		/// </summary>
		bool InitializeAccept()
		{
			if(CheckAccept()) {
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

		bool CheckAccept()
		{
			if(!CommonData.MainSetting.RunningInformation.Accept) {
				// 完全に初回
				CommonData.Logger.Debug("first");
				return false;
			}

			if(CommonData.MainSetting.RunningInformation.LastExecuteVersion == null) {
				// 何らかの理由で前回実行時のバージョン格納されていない
				CommonData.Logger.Debug("last version == null");
				return false;
			}

			if(CommonData.MainSetting.RunningInformation.LastExecuteVersion < Constants.acceptVersion) {
				// 前回バージョンから強制定期に使用許諾が必要
				CommonData.Logger.Debug("last version < accept version");
				return false;
			}

			return true;
		}

		void InitializeSetting()
		{
			InitializeMainSetting();
		}

		void InitializeMainSetting()
		{
		}

		/// <summary>
		/// ログの生成。
		/// </summary>
		void CreateLogger()
		{
			var loggingWindow = new LoggingWindow();
			loggingWindow.SetCommonData(CommonData);
			LoggingVM = loggingWindow.ViewModel;

			var systemLogger = (SystemLogger)CommonData.Logger;
			systemLogger.LogCollector = LoggingVM;
			if (systemLogger.IsStock) {
				// 溜まったログをViewにドバー
				foreach (var logItem in systemLogger.StockItems) {
					systemLogger.LogCollector.AddLog(logItem);
				}
				systemLogger.IsStock = false;
			}
		}

		/// <summary>
		/// ツールバーの生成。
		/// </summary>
		void CreateToolbar()
		{
			foreach (var screen in Screen.AllScreens.OrderBy(s => !s.Primary)) {
				var toolbar = new LauncherToolbarWindow(screen);
				toolbar.SetCommonData(CommonData);
			}
		}

		#endregion
	}
}
