namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
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

			//create the notifyicon (it's a resource declared in NotifyIconResources.xaml
			this._notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
			this._notifyIcon.DataContext = AppUtility.CreateMainWorkerViewModel();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
			base.OnExit(e);
		}
	}
}
