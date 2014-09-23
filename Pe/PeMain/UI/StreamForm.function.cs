/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:58
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.Logic;
using PeUtility;

namespace PeMain.UI
{
	public partial class StreamForm
	{
		public void SetParameter(Process process, LauncherItem launcherItem)
		{
			Process = process;
			LauncherItem = launcherItem;
			
			Process.EnableRaisingEvents = true;
			Process.Exited += new EventHandler(Process_Exited);
		}

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(Process != null);
			
			ApplyLanguage();
			
			this.propertyProcess.SelectedObject = Process;
			this.propertyProperty.SelectedObject = Process.StartInfo;

			Process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
			Process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
		}
		
		void OutputStreamReceived(string line, bool stdOutput)
		{
			if(IsDisposed) {
				// #20
				return;
			}
			
			this.viewOutput.BeginInvoke(
				(MethodInvoker)delegate() {
					this.viewOutput.Text += line + Environment.NewLine;
				}
			);
		}
		
		void RefreshProperty()
		{
			// #21
			Process.Refresh();
		}
		
		void ExitedProcess()
		{
			if(IsDisposed) {
				// #20
				return;
			}
			
			this.toolStream_itemKill.Enabled = false;
			this.toolStream_itemClear.Enabled = false;
			this.toolStream_itemRefresh.Enabled = false;
			RefreshProperty();
			
			Text += String.Format(": {0}", Process.ExitCode);
		}
		
		void KillProcess()
		{
			if(Process.HasExited) {
				return;
			}
			try {
				Process.Kill();
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
			}
		}
		
		/// <summary>
		/// #22
		/// </summary>
		/// <param name="path"></param>
		void SaveStream(string path)
		{
			using(var stream = new StreamWriter(new FileStream(path, FileMode.Create))) {
				stream.Write(this.viewOutput.Text);
			}
		}
		
		void SwitchTopmost()
		{
			this.toolStream_itemTopmost.Checked = !this.toolStream_itemTopmost.Checked;
			TopMost = this.toolStream_itemTopmost.Checked;
		}
		
	}
}
