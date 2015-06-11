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
		#region variable
		
		TaskbarIcon _notifyIcon;
		SystemLogger _systemLogger;

		#endregion

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var commandLine = new CommandLine();
			var constants = new VariableConstants(commandLine);
			this._systemLogger = AppUtility.CreateSystemLogger(constants.FileLogging, constants.LogDirectoryPath);
			this._systemLogger.IsStock = true;
			this._systemLogger.Information("start!", commandLine);
			var workVm = new MainWorkerViewModel(constants, this._systemLogger);
			if(workVm.Initialize()) {
				this._notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
				this._notifyIcon.DataContext = workVm;
			} else {
				// 終了
				Application.Current.Shutdown();
			}
		}

		protected override void OnExit(ExitEventArgs e)
		{
			if (this._notifyIcon != null) {
				this._notifyIcon.Dispose();
			}
			if (this._systemLogger != null) {
				//this._systemLogger.LoggerConfig.PutsCustom = false;
			}
			base.OnExit(e);
		}
	}
}
