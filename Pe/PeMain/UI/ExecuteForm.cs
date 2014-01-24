/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/14
 * 時刻: 23:11
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ExecuteForm.
	/// </summary>
	public partial class ExecuteForm : Form
	{
		public ExecuteForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void CommandOption_file_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogFilePath(this.inputOption);
		}
		
		void CommandOption_dir_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputOption);
		}
		
		void CommandWorkDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputWorkDirPath);
		}
		
		void SelectUserDefault_CheckedChanged(object sender, EventArgs e)
		{
			var enabled = this.selectEnvironment.Checked;
			envUpdate.Enabled = enabled;
			envRemove.Enabled = enabled;
		}
		
		void CommandSubmit_Click(object sender, EventArgs e)
		{
			SubmitInput();
			DialogResult = DialogResult.OK;
		}
	}
}
