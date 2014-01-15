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
using PI.Windows;

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
			var toolItem = (ToolStripItem)sender;
			var launcherItem = (LauncherItem)toolItem.Tag;
			ExecuteItem(launcherItem);
		}
		
		void ToolbarForm_Paint(object sender, PaintEventArgs e)
		{
			DrawFull(e.Graphics, ClientRectangle, Form.ActiveForm == this);
		}
		
		void ToolbarForm_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				// タイトルバーっぽければ移動させとく
				if(ToolbarSetting.ToolbarPosition == ToolbarPosition.DesktopFloat) {
					var hitTest = HT.HTNOWHERE;
					var captionArea = GetCaptionArea(ToolbarSetting.ToolbarPosition);
					if(captionArea.Contains(e.Location)) {
						hitTest = HT.HTCAPTION;
					} else {
						var leftArea = new Rectangle(0, 0, Padding.Left, Height);
						var rightArea = new Rectangle(Width - Padding.Right, 0, Padding.Right, Height);
						if(leftArea.Contains(e.Location)) {
							hitTest = HT.HTLEFT;
						} else if(rightArea.Contains(e.Location)) {
							hitTest = HT.HTRIGHT;
						}
					}
					if(hitTest != HT.HTNOWHERE) {
						API.ReleaseCapture();
						API.SendMessage(Handle, WM.WM_NCLBUTTONDOWN, (IntPtr)hitTest, IntPtr.Zero);
					}
				}
			}
		}
		
		void ToolbarForm_SizeChanged(object sender, EventArgs e)
		{
			if(ToolbarSetting != null && ToolbarSetting.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				ToolbarSetting.FloatSize = Size;
			}
		}
		void ToolbarForm_LocationChanged(object sender, EventArgs e)
		{
			if(ToolbarSetting != null && ToolbarSetting.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				ToolbarSetting.FloatLocation = Location;
			}
		}
	}
}
