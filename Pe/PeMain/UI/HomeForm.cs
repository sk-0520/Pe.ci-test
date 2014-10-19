/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 10/16/2014
 * 時刻: 20:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.IF;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of HomeForm.
	/// </summary>
	public partial class HomeForm : Form, ISetCommonData
	{
		public HomeForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void CommandNotify_Click(object sender, EventArgs e)
		{
			SystemExecuter.OpenNotificationAreaHistory(CommonData);
		}
		
		void CommandStartup_Click(object sender, EventArgs e)
		{
			var path = Literal.StartupShortcutPath;
			var icon = MessageBoxIcon.Information;
			string message;
			if(!File.Exists(path)) {
				try {
					AppUtility.MakeAppShortcut(path);
					message = CommonData.Language["home/startup/dialog/message"];
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
					message = ex.Message;
					icon = MessageBoxIcon.Error;
				}
			} else {
				message = CommonData.Language["home/startup/exists"];
				CommonData.Logger.Puts(LogType.Information, message, path);
			}
			MessageBox.Show(message, CommonData.Language["home/startup/dialog/caption"], MessageBoxButtons.OK, icon);
		}
		
		void CommandLauncher_Click(object sender, EventArgs e)
		{
			// TODO: がんばろう
			MakeDefaultLauncherItem();
		}
		
		void HomeForm_Shown(object sender, EventArgs e)
		{
			UIUtility.ShowFront(this);
			this.Activate();
		}
	}
}
