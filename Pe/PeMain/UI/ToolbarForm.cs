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

using PeMain.Data;
using PeMain.Logic;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm.
	/// </summary>
	public partial class ToolbarForm : AppbarForm, ISetSettingData
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
			var menuItem = (MenuItem)sender;
			var group = (ToolbarGroupItem)menuItem.Tag;
			SelectedGroup(group);
		}
		
		void button_ButtonClick(object sender, EventArgs e)
		{
			var toolItem = (ToolStripItem)sender;
			var launcherItem = (LauncherItem)toolItem.Tag;
			ExecuteItem(launcherItem);
		}
		
		void ToolbarForm_Paint(object sender, PaintEventArgs e)
		{
			DrawFull(e.Graphics, ClientRectangle, Form.ActiveForm == this);
		}
		
		void ToolbarForm_SizeChanged(object sender, EventArgs e)
		{
			if(UseToolbarItem != null && UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UseToolbarItem.FloatSize = Size;
			}
		}
		void ToolbarForm_LocationChanged(object sender, EventArgs e)
		{
			if(MainSetting != null && UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UseToolbarItem.FloatLocation = Location;
			}
		}
		
		void ToolLauncherDragEnter(object sender, DragEventArgs e)
		{
			ProcessDropEffect(e);
		}
		
		void ToolLauncherDragOver(object sender, DragEventArgs e)
		{
			ProcessDropEffect(e);
		}
		
		void ToolLauncherDragDrop(object sender, DragEventArgs e)
		{
			var dropData = ProcessDropEffect(e);
			ExecuteDropData(dropData);
		}
		
		void ToolbarFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
			}
		}
		
		
		void ToolbarFormShown(object sender, EventArgs e)
		{
			ApplySettingPosition();
		}
	}
}
