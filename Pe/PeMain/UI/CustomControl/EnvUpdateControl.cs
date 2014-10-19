/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 21:35
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PeMain.IF;

namespace PeMain.UI
{
	/// <summary>
	/// 環境変数更新用コントロール。
	/// </summary>
	public partial class EnvUpdateControl : UserControl, ISetLanguage
	{
		public EnvUpdateControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void GridEnv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if(this._event && ValueChanged != null) {
				ValueChanged(this, new EventArgs());
			}
		}
		
		void GridEnv_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if(this._event && ValueChanged != null) {
				ValueChanged(this, new EventArgs());
			}
		}
	}
}
