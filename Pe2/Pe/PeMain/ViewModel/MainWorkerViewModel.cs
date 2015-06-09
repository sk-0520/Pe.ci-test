namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.View;

	public sealed class MainWorkerViewModel: ViewModelBase
	{
		public MainWorkerViewModel(VariableConstants variableConstants)
		{
			VariableConstants = variableConstants;
			LoadSetting();
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
					var window = new SettingWindow();
					window.ShowDialog();
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
			var mainSetting = AppUtility.LoadSetting<MainSettingModel>(VariableConstants.UserSettingFileMainSettingPath);
			var launcherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(VariableConstants.UserSettingFileLauncherItemSettingPath);
			var launcherGroupItemSetting = AppUtility.LoadSetting<LauncherGroupItemSettingModel>(VariableConstants.UserSettingFileLauncherGroupItemSetting);
			var language = AppUtility.LoadLanguageFile(VariableConstants.ApplicationLanguageDirectoryPath, mainSetting.Language.Name);

			CommonData = new CommonData(mainSetting, launcherItemSetting, launcherGroupItemSetting, language);
		}

		void SaveSetting()
		{
			//AppUtility.SaveSetting(VariableConstants.UserSettingFileMainSettingPath, MainSetting);
			//AppUtility.SaveSetting(VariableConstants.UserSettingFileLauncherItemSettingPath, LauncherItemSetting);
			//AppUtility.SaveSetting(VariableConstants.UserSettingFileLauncherGroupItemSetting, LauncherGroupItemSetting);
		}

		#endregion
	}
}
