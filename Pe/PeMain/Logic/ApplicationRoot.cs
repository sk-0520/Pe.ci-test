namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	internal static class ApplicationRoot
	{
#if DEBUG
		static void DebugRootProcess()
		{
		}
#endif
		/// <summary>
		/// プログラム重複判定用のmutex名を取得。
		/// </summary>
		/// <param name="commandLine"></param>
		/// <returns></returns>
		static string GetMutexName(CommandLine commandLine)
		{
			string mutexName = string.Empty;
			
			if(commandLine.HasValue("mutex")) {
				mutexName = commandLine.GetValue("mutex");
			}
			if(string.IsNullOrWhiteSpace(mutexName)) {
				mutexName = Literal.programName;
#if DEBUG
				mutexName += "_debug";
				//mutexName += new Random().Next().ToString();
#endif
			}

			return mutexName;
		}

		internal static void Execute(string[] args)
		{
#if DEBUG
			DebugRootProcess();
#endif

			var commandLine = new CommandLine(args);
			Literal.Initialize(commandLine);
			var fileLogger = new ContentTypeTextNet.Pe.PeMain.Logic.FileLogger();
			if(commandLine.HasOption("log")) {
				var logPath = Path.Combine(Literal.LogFileDirPath, DateTime.Now.ToString(Literal.NowTimestampFileName) + ".log");
				FileUtility.MakeFileParentDirectory(logPath);
				fileLogger = new FileLogger(logPath);
				fileLogger.Puts(LogType.Information, "Information", new AppInformation().ToString());
			}

			string mutexName = GetMutexName(commandLine);

			bool isFirstInstance;
			fileLogger.Puts(LogType.Information, "mutex name", mutexName);
			using(fileLogger) {
#if RELEASE
				try {
#endif
				using(var mtx = new Mutex(true, mutexName, out isFirstInstance)) {
					if(isFirstInstance) {
						using(var app = new UI.App(commandLine, fileLogger)) {
							if(!app.Initialized) {
								app.CloseApplication(false);
							} else {
								if(!app.ExistsSettingFilePath) {
									Task.Run(() => {
										Thread.Sleep(Literal.startHomeDialogWaitTime);
									}).ContinueWith(t => {
										app.ShowHomeDialog();
									}, TaskScheduler.FromCurrentSynchronizationContext());
								}
								Application.Run();
							}
						}
						fileLogger.Puts(LogType.Information, "Close", Process.GetCurrentProcess().Id);
					} else {
						fileLogger.Puts(LogType.Error, "duplicate boot", mutexName);
					}
				}
#if RELEASE
				} catch(Exception ex) {
					fileLogger.Puts(LogType.Error, ex.Message, ex);
					throw;
				}
#endif
			}
		}
	}
}
