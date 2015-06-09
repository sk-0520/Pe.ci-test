namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.View;

	public sealed class MainWorkerViewModel: ViewModelBase
	{
		public MainWorkerViewModel(VariableConstants variableConstants, ILogger logger)
		{
			VariableConstants = variableConstants;
			CommonData = new CommonData() {
				Logger = logger,
			};
		}


		#region property

		CommonData CommonData { get; set; }

		VariableConstants VariableConstants { get; set; }

		public bool Pause { get; set; }

		List<Window> WindowList { get; set; }


		#endregion

		#region command

		/// <summary>
		/// Hides the main window. This command is only enabled if a window is open.
		/// </summary>
		public ICommand HideWindowCommand
		{
			get
			{
				return new DelegateCommand {
					Command = o => Application.Current.MainWindow.Close(),
					CanExecute = o => Application.Current.MainWindow != null
				};
			}
		}

		public ICommand ShowSettingWindowCommand
		{
			get
			{
				var result = new DelegateCommand();
				result.Command = o => {
					//var window = new SettingWindow();
					//window.ShowDialog();
				};

				return result;
			}
		}

		/// <summary>
		/// Shuts down the application.
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

		#region function

		void LoadSetting()
		{
			// 各種設定の読込
			CommonData.MainSetting= AppUtility.LoadSetting<MainSettingModel>(VariableConstants.UserSettingFileMainSettingPath, CommonData.Logger);
			CommonData.LauncherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.Logger);
			CommonData.LauncherGroupItemSetting = AppUtility.LoadSetting<LauncherGroupItemSettingModel>(VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.Logger);
			// 言語ファイル
			CommonData.Language = AppUtility.LoadLanguageFile(VariableConstants.ApplicationLanguageDirectoryPath, CommonData.MainSetting.Language.Name, VariableConstants.LanguageCode, CommonData.Logger);
		}

		void SaveSetting()
		{
			AppUtility.SaveSetting(VariableConstants.UserSettingFileMainSettingPath, CommonData.MainSetting, CommonData.Logger);
			AppUtility.SaveSetting(VariableConstants.UserSettingFileLauncherItemSettingPath, CommonData.LauncherItemSetting, CommonData.Logger);
			AppUtility.SaveSetting(VariableConstants.UserSettingFileLauncherGroupItemSetting, CommonData.LauncherGroupItemSetting, CommonData.Logger);
		}

		/// <summary>
		///プログラム実行を準備。
		/// </summary>
		public void Initialize()
		{
			CommonData.Logger.Information("MainWorkerViewModel initialize");

			LoadSetting();


		}

		#endregion
	}
}
