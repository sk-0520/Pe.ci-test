namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.View;

	public sealed class MainWorkerViewModel: ViewModelBase
	{
		#region property

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
				return new DelegateCommand { Command = o => Application.Current.Shutdown() };
			}
		}

		#endregion

	}
}
