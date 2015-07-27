namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
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
		MainWorkerViewModel _mainWorker;

		#endregion

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

#if DEBUG
			DebugProcess();
#endif

			var commandLine = new CommandLine();
			var constants = new VariableConstants(commandLine);
			var systemLogger = AppUtility.CreateSystemLogger(constants.FileLogging, constants.LogDirectoryPath);
			systemLogger.IsStock = true;
			systemLogger.Information("start!", commandLine);
			systemLogger.Information("application", new AppInformationCollection().ToString());
			this._mainWorker = new MainWorkerViewModel(constants, systemLogger);
			if (this._mainWorker.Initialize()) {
				LanguageUtility.RecursiveSetLanguage(this._notifyIcon, this._mainWorker.Language);
				this._notifyIcon = (TaskbarIcon)FindResource("root");
				this._notifyIcon.DataContext = this._mainWorker;
				//var menu = (ContextMenu)FindResource("ContextMenu");
				//menu.PlacementTarget = this._notifyIcon;
				//menu.DataContext = this._notifyIcon.DataContext;
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
			if (this._mainWorker != null) {
				this._mainWorker.Dispose();
			}

			base.OnExit(e);
		}
	}
}
