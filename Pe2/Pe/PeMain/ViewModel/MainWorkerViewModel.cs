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
		public MainWorkerViewModel(Constants constants)
		{
			Constants = constants;
			LoadSetting();
		}

		#region property

		#region setting

		MainSettingModel MainSetting { get; set; }
		LauncherItemSettingModel LauncherItemSetting { get; set; }
		LauncherGroupItemSettingModel LauncherGroupItemSetting { get; set; }

		#endregion

		Constants Constants { get; set; }

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
			MainSetting = AppUtility.LoadSetting<MainSettingModel>(Constants.UserSettingFileMainSettingPath);
			LauncherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(Constants.UserSettingFileLauncherItemSettingPath);
			LauncherGroupItemSetting = AppUtility.LoadSetting<LauncherGroupItemSettingModel>(Constants.UserSettingFileLauncherGroupItemSetting);
		}

		void SaveSetting()
		{
			AppUtility.SaveSetting(Constants.UserSettingFileMainSettingPath, MainSetting);
			AppUtility.SaveSetting(Constants.UserSettingFileLauncherItemSettingPath, LauncherItemSetting);
			AppUtility.SaveSetting(Constants.UserSettingFileLauncherGroupItemSetting, LauncherGroupItemSetting);
		}

		#endregion
	}
}
