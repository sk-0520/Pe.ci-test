/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:58
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	public partial class StreamForm
	{
		public void SetSettingData(Language language, Process process, LauncherItem launcherItem)
		{
			Language = language;
			Process = process;
			LauncherItem = launcherItem;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(Process != null);
			
			ApplyLanguage();
			
			this.propertyProcess.SelectedObject = Process;

			Process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e) {
				if(this.viewOutput != null)
				this.viewOutput.BeginInvoke((MethodInvoker)delegate() {
					var s = e.Data;
					this.viewOutput.Text += s;
				});
			};
		}
	}
}
