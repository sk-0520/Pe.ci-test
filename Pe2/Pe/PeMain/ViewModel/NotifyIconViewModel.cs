namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	/// <summary>
	/// Provides bindable properties and commands for the NotifyIcon. In this sample, the
	/// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
	/// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
	/// </summary>
	public class NotifyIconViewModel: ViewModelBase
	{
		#region property

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
