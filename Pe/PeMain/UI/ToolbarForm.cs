/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:15
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm.
	/// </summary>
	public partial class ToolbarForm : AppbarForm
	{
		public ToolbarForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void ToolbarForm_MenuItem_Click(object sender, EventArgs e)
		{
			var menuItem = (ToolStripItem)sender;
			var group = (ToolbarGroupItem)menuItem.Tag;
			SelectedGroup(group);
		}
		
		void button_ButtonClick(object sender, EventArgs e)
		{
			Debug.WriteLine("click: " + sender.ToString());
		}

		
		void button_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			Debug.WriteLine("menu: " + e.ClickedItem.ToString());
		}

		
	}
}
