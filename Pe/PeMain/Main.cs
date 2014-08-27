/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/10/30
 * 時刻: 23:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PeMain
{
	public static class Startup
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			var commandLine = new PeUtility.CommandLine(args);
			Literal.Initialize(commandLine);
			PeMain.Logic.FileLogger fileLogger = new PeMain.Logic.FileLogger();
			if(commandLine.HasOption("log")) {
				var logPath = Path.Combine(Literal.LogFileDirPath, DateTime.Now.ToString(Literal.timestampFileName) + ".log");
				PeUtility.FileUtility.MakeFileParentDirectory(logPath);
				fileLogger = new PeMain.Logic.FileLogger(logPath);
				fileLogger.Puts(PeMain.Data.LogType.Information, "Information", new PeMain.Logic.PeInformation().ToString());
			}
			
			bool isFirstInstance;
			var mutexName = Literal.programName;
			#if DEBUG
			mutexName += "_debug";
			//mutexName += new Random().Next().ToString();
			#endif
			fileLogger.Puts(PeMain.Data.LogType.Information, "mutex name", mutexName);
			using(fileLogger) {
				using (Mutex mtx = new Mutex(true, mutexName, out isFirstInstance)) {
					if (isFirstInstance) {
						using(var context = new UI.Pe(commandLine, fileLogger)) {
							#if DEBUG
							context.DebugProcess();
							#endif
							
							Application.Run();
						}
					} else {
						fileLogger.Puts(PeMain.Data.LogType.Error, "dual boot", mutexName);
					}
				}
			}
		}
	}
}
