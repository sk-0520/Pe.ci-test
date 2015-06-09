namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using Hardcodet.Wpf.TaskbarNotification;

	/// <summary>
	/// Simple application. Check the XAML for comments.
	/// </summary>
	public partial class App: Application
	{
		private TaskbarIcon _notifyIcon;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var constants = new VariableConstants(new CommandLine());
			this._notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
			this._notifyIcon.DataContext = AppUtility.CreateMainWorkerViewModel(constants);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
			base.OnExit(e);
		}
	}
}
