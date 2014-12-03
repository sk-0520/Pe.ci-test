/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/14
 * 時刻: 23:11
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// 指定して実行。
	/// </summary>
	public partial class ExecuteForm : Form, ISetCommonData
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
		
		void TabExecute_pageBasic_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
			}
		}
		
		void TabExecute_pageBasic_DragDrop(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var dragDatas = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
				var args = string.Join(" ", dragDatas.WhitespaceToQuotation());
				this.inputOption.Text = args;
			}
		}
		
		void InputWorkDirPath_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var dragDatas = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
				if(dragDatas.Count() == 1) {
					var path = dragDatas.First();
					
					e.Effect = DragDropEffects.Copy;
				}
			}
		}
		
		void InputWorkDirPath_DragDrop(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var dragDatas = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
				if(dragDatas.Count() == 1) {
					var path = dragDatas.First();
					if(FileUtility.Exists(path)) {
						var isDir = Directory.Exists(path);
						if(!isDir) {
							this.inputWorkDirPath.Text = Path.GetDirectoryName(path);
						} else {
							this.inputWorkDirPath.Text = path;
						}
					}
				}
			}
		}
	}
}
