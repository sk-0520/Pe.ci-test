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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PeMain.Data;
using PeSkin;
using ContentTypeTextNet.Pe.Library.Utility;

namespace PeMain.UI
{
	partial class StreamForm
	{
		public void SetParameter(Process process, LauncherItem launcherItem)
		{
			Process = process;
			LauncherItem = launcherItem;
			
			Process.EnableRaisingEvents = true;
			Process.Exited += new EventHandler(Process_Exited);
			
			// アイコン設定、アイテムに設定されているアイコンとは別に実行プロセスのアイコンを指定する
			try {
				var iconPath = Environment.ExpandEnvironmentVariables(LauncherItem.Command);
				if(PathUtility.HasIconPath(iconPath)) {
					Icon = IconUtility.Load(iconPath, IconScale.Normal, 0);
				} else {
					Icon = new Icon(iconPath);
				}
			} catch(Exception ex) {
				Debug.WriteLine(ex);
				Icon = global::PeMain.Properties.Images.App;
			}
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
			
			//this._inputStream = Process.StandardInput;
			
			this.inputOutput.Font = CommonData.MainSetting.Launcher.StreamFontSetting.Font;
		}
		
		void OutputStreamReceived(string line, bool stdOutput)
		{
			/* //#20 retry
			if(IsDisposed) {
				// #20
				return;
			}
			 */
			
			this.inputOutput.BeginInvoke(
				(MethodInvoker)delegate() {
					this.inputOutput.Text += line + Environment.NewLine;
					this.inputOutput.SelectionStart = this.inputOutput.TextLength;
					this.inputOutput.ScrollToCaret();
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
			/* //#20 retry
			if(IsDisposed) {
				// #20
				return;
			}
			 */
			
			this.toolStream_itemKill.Enabled = false;
			this.toolStream_itemClear.Enabled = false;
			this.toolStream_itemRefresh.Enabled = false;
			this.inputOutput.ReadOnly = true;
			RefreshProperty();
			
			Text += String.Format(": {0}", Process.ExitCode);
		}
		
		void KillProcess()
		{
			try {
				if(Process.HasExited) {
					return;
				}
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
				stream.Write(this.inputOutput.Text);
			}
		}
		
		void SwitchTopmost()
		{
			this.toolStream_itemTopmost.Checked = !this.toolStream_itemTopmost.Checked;
			TopMost = this.toolStream_itemTopmost.Checked;
		}
		
	}
}
