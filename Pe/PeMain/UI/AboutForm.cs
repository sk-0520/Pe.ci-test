/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
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
			OpenDirectory(Literal.ApplicationRootDirPath);
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
			if(e.ColumnIndex == this.gridComponents_columnName.Index && e.RowIndex != -1) {
				var cell = this.gridComponents.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewLinkCell;
				if(cell != null) {
					var rowIndex = cell.RowIndex;
					if(0 <= rowIndex && rowIndex < ComponentInfoList.Count) {
						var componentInfo = ComponentInfoList[rowIndex];
						var link = componentInfo.URI;
						Executer.RunCommand(link, CommonData);
						cell.LinkVisited = true;
					}
				}
			}
		}
		
		void linkCopyShort_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var list = new List<string>();
			list.Add("Software: " + Literal.Version.ProductName);
			list.Add("Version: " + Literal.ApplicationVersion);
			list.Add("Type: " +
				#if DEBUG
				"DEBUG"
				#else
				"RELEASE"
				#endif
			);
			list.Add("Process: " + (Environment.Is64BitProcess ? "64": "32"));
			list.Add("Platform: " + (Environment.Is64BitOperatingSystem ? "64": "32"));
			list.Add("OS: " + System.Environment.OSVersion);
			list.Add("CLI: " + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());

			ClipboardUtility.CopyText(Environment.NewLine + Separator + Environment.NewLine + string.Join(Environment.NewLine, list.Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine, CommonData.MainSetting.Clipboard);
		}
		
		void linkCopyLong_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ClipboardUtility.CopyText(Environment.NewLine + Separator + Environment.NewLine + string.Join(Environment.NewLine, new ContentTypeTextNet.Pe.PeMain.Logic.AppInformation().ToString().SplitLines().Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine, CommonData.MainSetting.Clipboard);
		}
	}
}
