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
			
			bool isFirstInstance;
			var mutexName = Literal.programName;
			#if DEBUG
			mutexName += "_debug";
			//mutexName += new Random().Next().ToString();
			#endif
			using (Mutex mtx = new Mutex(true, mutexName, out isFirstInstance)) {
				if (isFirstInstance) {
					using(var context = new UI.Pe(args)) {
						#if DEBUG
						context.DebugProcess();
						#endif
						
						Application.Run();
					}
				} else {
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
				}
			} // releases the Mutex
		}
	}
}
