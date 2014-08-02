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
			var logger = new StartupLogger(fileLogger);
			Initialize(commandLine, logger);
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
			MessageBox.Show("でゅん・・・");
		}
	}
}
