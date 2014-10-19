/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:38
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;

using PeMain.IF;

namespace PeMain.UI
{
	/// <summary>
	/// 標準出力取得。
	/// </summary>
	public partial class StreamForm : Form, ISetCommonData
	{
		public StreamForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			OutputStreamReceived(e.Data, true);
		}

		void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			OutputStreamReceived(e.Data, false);
		}
		
		
		void ToolStream_refresh_Click(object sender, EventArgs e)
		{
			RefreshProperty();
		}
		
		void Process_Exited(object sender, EventArgs e)
		{
			if(InvokeRequired) {
				Invoke((MethodInvoker)delegate() { ExitedProcess(); });
			} else {
				ExitedProcess();
			}
		}
		
		void ToolStream_kill_Click(object sender, EventArgs e)
		{
			KillProcess();
		}
		
		void ViewOutput_TextChanged(object sender, EventArgs e)
		{
			var hasText = this.viewOutput.TextLength > 0;
			this.toolStream_itemSave.Enabled = hasText;
			this.toolStream_itemClear.Enabled = hasText;
		}
		
		void ToolStream_clear_Click(object sender, EventArgs e)
		{
			// #22
			this.viewOutput.Clear();
		}
		
		void ToolStream_save_Click(object sender, EventArgs e)
		{
			using(var dialog = new SaveFileDialog()) {
				dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				dialog.FileName = Literal.NowTimestampFileName + ".output.log";
				dialog.Filter = "*.output.log|*.output.log";
				if(dialog.ShowDialog() == DialogResult.OK) {
					var path = dialog.FileName;
					SaveStream(path);
				}
			}
		}
		
		void ToolStream_itemTopmost_Click(object sender, EventArgs e)
		{
			SwitchTopmost();
		}

	}
}
