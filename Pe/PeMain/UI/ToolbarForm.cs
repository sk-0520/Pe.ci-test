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
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.IF;
using PeSkin;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// ツールバー。
	/// </summary>
	public partial class ToolbarForm : AppbarForm, ISetCommonData
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
		
		void LauncherTypeFile_ButtonClick(object sender, EventArgs e)
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
			if(this._isRunning && UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UseToolbarItem.FloatSize = Size;
			}
		}
		void ToolbarForm_LocationChanged(object sender, EventArgs e)
		{
			if(this._isRunning && UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UseToolbarItem.FloatLocation = Location;
			}
		}
		
		void ToolLauncherDragEnter(object sender, DragEventArgs e)
		{
			ProcessDropEffect(sender, e);
		}
		
		void ToolLauncherDragOver(object sender, DragEventArgs e)
		{
			ProcessDropEffect(sender, e);
		}
		
		void ToolLauncherDragDrop(object sender, DragEventArgs e)
		{
			var dropData = ProcessDropEffect(sender, e);
			if(dropData.DropType == DropType.Files) {
				ExecuteDropData(dropData);
			} else {
				Debug.Assert(dropData.DropType == DropType.Button);
				ChnageDropDataLauncherItemPosition(dropData);
				this._dragStartItem = null;
			}
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
		
		void ToolbarForm_Activated(object sender, EventArgs e)
		{
			DrawFullActivaChanged(true);
		}
		
		void ToolbarForm_Deactivate(object sender, EventArgs e)
		{
			DrawFullActivaChanged(false);
		}
		
		void OpeningRootMenu(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
			
			this._menuOpening = true;
			this.tipsLauncher.Hide();
			var toolItem = sender as ToolStripDropDownItem;
			if(toolItem != null) {
				switch(UseToolbarItem.ToolbarPosition) {
					case ToolbarPosition.DesktopFloat:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Default;
						break;
						
					case ToolbarPosition.DesktopTop:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Default;
						break;
						
					case ToolbarPosition.DesktopBottom:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Default;
						break;
						
					case ToolbarPosition.DesktopLeft:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Right;
						break;
						
					case ToolbarPosition.DesktopRight:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Left;
						break;
						
					default:
						Debug.Assert(false, UseToolbarItem.ToolbarPosition.ToString());
						break;
				}
			}
		}
		
		void CloseRootMenu(object sender, EventArgs e)
		{
			this._menuOpening = false;
			SwitchHidden();
		}
		
		void ToolItem_MouseHover(object sender, EventArgs e)
		{
			var toolItem = (ToolStripItem)sender;
			/*
			var cursorPoint = Cursor.Position;
			cursorPoint.Offset(SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Height);
			var point = this.PointToClient(cursorPoint);
			Debug.WriteLine(toolItem.ToolTipText);
			this.tipsLauncher.Show(toolItem.ToolTipText, this, point);
			 */
			//this.tipsLauncher.SetToolTip(this.toolLauncher, toolItem.ToolTipText);
			//this.tipsLauncher.Show(toolItem.Text, this, Point.Empty);
			//this.tipsLauncher.RemoveAll();
			//this.tipsLauncher.SetToolTip(this, "#");
			//if(toolItem.OwnerItem == this.toolLauncher)
			//if(toolItem.OwnerItem == null)
			//var menuItem = toolItem as ToolStripDropDownItem;
			if(this._menuOpening) {
				// メニュー表示中はなんもしない
				return;
			}
			this.tipsLauncher.ShowItem(DockScreen, toolItem, SelectedGroupItem, UseToolbarItem);
		}

		void toolItem_MouseLeave(object sender, EventArgs e)
		{
			this.tipsLauncher.HideItem();
			//this.tipsLauncher.RemoveAll();
		}
		

		/*
		void ToolLauncher_MouseHover(object sender, EventArgs e)
		{
			var cursorPoint = Cursor.Position;
			cursorPoint.Offset(SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Height);
			var point = this.PointToClient(cursorPoint);
			var toolItem = this.toolLauncher.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Bounds.Contains(point));
			if(toolItem != null) {
				//this.tipsLauncher.SetToolTip(this.toolLauncher, toolItem.ToolTipText);
				Debug.WriteLine("ToolLauncher_MouseHover");
			} else {
				this.tipsLauncher.RemoveAll();
			}
		}
		 * */
		
		void ToolbarForm_AppbarFullScreen(object sender, AppbarFullScreenEvent e)
		{
			if(e.FullScreen) {
				TopMost = false;
				NativeMethods.SetWindowPos(Handle, (IntPtr)HWND.HWND_BOTTOM, 0, 0, 0, 0, SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOACTIVATE);
			} else {
				ApplySettingTopmost();
			}
		}
		
		void LauncherButton_MouseDown(object sender, MouseEventArgs e)
		{
			if(Control.ModifierKeys == Keys.Alt) {
				this._dragStartItem = (ToolStripItem)sender;
				this.toolLauncher.DoDragDrop(sender, DragDropEffects.Move);
			}
		}

	}
}
