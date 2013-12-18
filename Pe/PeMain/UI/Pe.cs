/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:22
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using System.Windows.Forms;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	public partial class Pe: IDisposable
	{
		public Pe(string[] args)
		{
			Initialize(args);
			var f = new BaseToolbarForm();
			f.Show();
			f.DockType = DockType.Left;
		}
		
		public void Dispose()
		{
			if(this.notifyIcon != null) {
				this.notifyIcon.Dispose();
			}
		}
		
		private void menuAboutClick(object sender, EventArgs e)
		{
			MessageBox.Show("About This Application");
		}
		
		private void menuExitClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show("The icon was double clicked");
		}
	}
}
