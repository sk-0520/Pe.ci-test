/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 13:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
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
	}
}
