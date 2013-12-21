/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 1:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl.
	/// </summary>
	public partial class LauncherItemSelectControl : UserControl
	{
		public LauncherItemSelectControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			ResizeInputArea();
		}
		
		void LauncherItemSelectControlResize(object sender, EventArgs e)
		{
			ResizeInputArea();
		}
		
		void LauncherItemSelectControlLoad(object sender, EventArgs e)
		{
			ResizeInputArea();
		}
	}
}
