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
using System.Drawing;
using System.Windows.Forms;

namespace PeMain.UI
{
	/// <summary>
	/// Description of StreamForm.
	/// </summary>
	public partial class StreamForm : Form
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
			OutputStreamReceived(e.Data, true);
		}
		
	}
}
