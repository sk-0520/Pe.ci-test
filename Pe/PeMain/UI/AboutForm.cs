/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.IF;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// 情報。
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
			linkLabel.LinkVisited = true;
			var link = linkLabel.Text;
			if(string.IsNullOrWhiteSpace(link)) {
				return;
			}
			
			try {
				Executer.RunCommand(link, CommonData);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, link, ex);
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
		
		void CommandUpdate_Click(object sender, EventArgs e)
		{
			var caption = CommonData.Language["about/update/check/dialog/caption"];
			var message = CommonData.Language["about/update/check/dialog/message"];
			var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			if(result == DialogResult.Yes) {
				CheckUpdate = true;
				Close();
			}
		}
		
		void CommandChangelog_Click(object sender, EventArgs e)
		{
			var path = Path.Combine(Literal.ApplicationDocumentDirPath, "changelog.xml");
			Executer.OpenFile(path, CommonData);
		}
		
		void GridComponents_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == this.gridComponents_columnURI.Index && e.RowIndex != -1) {
				var cell = this.gridComponents.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewLinkCell;
				if(cell != null) {
					var link = (string)cell.Value;
					Executer.RunCommand(link, CommonData);
					cell.LinkVisited = true;
				}
			}
		}
	}
}
