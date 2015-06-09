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
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;
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

			var commandLine = new CommandLine();
			var constants = new VariableConstants(commandLine);
			var systemLogger = AppUtility.CreateSystemLogger(constants.FileLogging, constants.LogDirectoryPath);
			systemLogger.Information("start!", commandLine);
			var workVm = new MainWorkerViewModel(constants, systemLogger);
			if(workVm.Initialize()) {
				this._notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
				this._notifyIcon.DataContext = workVm;
			} else {
				// 終了
			}
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
			base.OnExit(e);
		}
	}
}
