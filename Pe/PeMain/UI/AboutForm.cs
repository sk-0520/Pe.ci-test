/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of AboutForm.
	/// </summary>
	public partial class AboutForm : Form, ISetCommonData
	{
		public AboutForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void CommandOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
		
		void Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var linkLabel = (LinkLabel)sender;
			linkLabel.Visible = true;
			var link = linkLabel.Text;
			if(string.IsNullOrWhiteSpace(link)) {
				return;
			}
			
			try {
				Executer.RunCommand(link);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, ex.Message, new { Exception = ex, Link = link});
			}
		}
		
		void CommandExecuteDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Path.GetDirectoryName(Application.ExecutablePath));
		}
		
		void CommandDataDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Literal.UserSettingDirPath);
		}
		
		void CommandBackupDir_Click(object sender, EventArgs e)
		{
			OpenDirectory(Literal.UserBackupDirPath);
		}
	}
}
