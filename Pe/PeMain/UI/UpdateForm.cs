/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:19
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PeMain.IF;

namespace PeMain.UI
{
	/// <summary>
	/// アップデートチェック。
	/// 
	/// 実行は PeUpdater にお任せ。
	/// </summary>
	public partial class UpdateForm : Form, ISetCommonData
	{
		public UpdateForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			Initialize();
		}
		
		void CommandOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
