/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 13:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PeUtility
{
	/// <summary>
	/// Description of ToolbarForm.
	/// </summary>
	public partial class AppbarForm : Form
	{
		public AppbarForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			if(AutoHide && ClientRectangle.Contains(this.PointToClient(Control.MousePosition))) {
				return;
			} else {
				base.OnMouseLeave(e);
			}
		}
		
		protected override void OnControlAdded(ControlEventArgs e)
		{
			e.Control.MouseLeave += new EventHandler(e_Control_MouseLeave);
			base.OnControlAdded(e);
		}
		
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			e.Control.MouseLeave -= e_Control_MouseLeave;
			base.OnControlRemoved(e);
		}

		void e_Control_MouseLeave(object sender, EventArgs e)
		{
			SwitchHidden();
		}
		
		void AppbarFormVisibleChanged(object sender, EventArgs e)
		{
			var visible = Visible;
			if(visible) {
				DesktopDockType = this._prevDesktopDockType;
			} else {
				_prevDesktopDockType = DesktopDockType;
				DesktopDockType = DesktopDockType.None;
			}
		}
		
		void TimerAutoHide_Tick(object sender, EventArgs e)
		{
			if(IsDocking) {
				ToHidden(false);
			} else {
				this.timerAutoHidden.Stop();
			}
		}
		
		
		void AppbarForm_MouseEnter(object sender, EventArgs e)
		{
			if(AutoHide) {
				StopHidden();
			}
		}
		
		void AppbarForm_MouseLeave(object sender, EventArgs e)
		{
			if(AutoHide) {
				WaitHidden();
			}
		}
	}
}
