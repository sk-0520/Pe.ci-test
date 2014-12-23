/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/10/30
 * 時刻: 23:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.PeMain
{
	public static class Startup
	{
		/// <summary>
		/// TODO: ちょっと分けたい^^;
		/// </summary>
		/// <param name="args"></param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			{
				var s = Literal.ApplicationSBinAppPath;
				var a = ContentTypeTextNet.Pe.Library.Utility.Serializer.LoadFile<ContentTypeTextNet.Pe.PeMain.Data.Applications>(s, true);
				ContentTypeTextNet.Pe.Library.Utility.Serializer.SaveFile(a, @"Z:\a.xml");
			}
			var commandLine = new ContentTypeTextNet.Pe.Library.Utility.CommandLine(args);
			Literal.Initialize(commandLine);
			var fileLogger = new ContentTypeTextNet.Pe.PeMain.Logic.FileLogger();
			if(commandLine.HasOption("log")) {
				var logPath = Path.Combine(Literal.LogFileDirPath, DateTime.Now.ToString(Literal.NowTimestampFileName) + ".log");
				ContentTypeTextNet.Pe.Library.Utility.FileUtility.MakeFileParentDirectory(logPath);
				fileLogger = new ContentTypeTextNet.Pe.PeMain.Logic.FileLogger(logPath);
				fileLogger.Puts(ContentTypeTextNet.Pe.PeMain.Data.LogType.Information, "Information", new ContentTypeTextNet.Pe.PeMain.Logic.AppInformation().ToString());
			}
			
			bool isFirstInstance;
			var mutexName = Literal.programName;
			#if DEBUG
			mutexName += "_debug";
			//mutexName += new Random().Next().ToString();
			#endif
			fileLogger.Puts(ContentTypeTextNet.Pe.PeMain.Data.LogType.Information, "mutex name", mutexName);
			using(fileLogger) {
				#if RELEASE
				try {
				#endif
					using (var mtx = new Mutex(true, mutexName, out isFirstInstance)) {
						if (isFirstInstance) {
							using(var app = new UI.App(commandLine, fileLogger)) {
								#if DEBUG
								app.DebugProcess();
								#endif
								if(!app.Initialized) {
									app.CloseApplication(false);
								} else {
									if(!app.ExistsSettingFilePath) {
										Task.Factory.StartNew(
											() => {
												Thread.Sleep(Literal.startHomeDialogWaitTime);
											}
										).ContinueWith(
											t => {
												app.ShowHomeDialog();
											},
											TaskScheduler.FromCurrentSynchronizationContext()
										);
									}
									Application.Run();
								}
							}
							fileLogger.Puts(ContentTypeTextNet.Pe.PeMain.Data.LogType.Information, "Close", Process.GetCurrentProcess());
						} else {
							fileLogger.Puts(ContentTypeTextNet.Pe.PeMain.Data.LogType.Error, "duplicate boot", mutexName);
						}
					}
				#if RELEASE
				} catch(Exception ex) {
					fileLogger.Puts(PeMain.Data.LogType.Error, ex.Message, ex);
					throw;
				}
				#endif
			}
		}
	}
}
