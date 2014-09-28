/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:22
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PeMain.Logic;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	public partial class Pe: IDisposable, IRootSender
	{
		public Pe(CommandLine commandLine, FileLogger fileLogger)
		{
			Initialized = true;
			
			var logger = new StartupLogger(fileLogger);
			Initialize(commandLine, logger);
			
			CheckUpdateProcess();
		}
		
		public void Dispose()
		{
			if(this._messageWindow != null) {
				this._messageWindow.Dispose();
			}
			if(this._notifyIcon != null) {
				this._notifyIcon.Dispose();
			}
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			/*
			var update = new Update(@"Z:temp", false);
			var info = update.Check();
			if(info.IsUpdate) {
				var s = string.Format("{0} {1}", info.Version, info.IsRcVersion ? "RC": "RELEASE");
				if(MessageBox.Show(s, "UPDATE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes) {
					update.Execute();
				}
			}
			 */
			MessageBox.Show("PON!");
		}
		

		
	}
}
