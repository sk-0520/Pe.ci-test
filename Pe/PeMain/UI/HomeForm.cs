/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 10/16/2014
 * 時刻: 20:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;
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
			AppUtility.MakeAppShortcut(path);
		}
	}
}
