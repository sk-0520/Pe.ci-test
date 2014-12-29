/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 21:35
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// 環境変数更新用コントロール。
	/// </summary>
	public partial class EnvUpdateControl : UserControl, ISetLanguage
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		Language _language;
		bool _event = false;
		#endregion ////////////////////////////////////

		#region event
		public event EventHandler<EventArgs> ValueChanged;
		#endregion ////////////////////////////////////

		public EnvUpdateControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		public IEnumerable<TPair<string, string>> Items
		{
			get
			{
				foreach(DataGridViewRow row in this.gridEnv.Rows) {
					if(row.IsNewRow) {
						continue;
					}
					var cellKey = row.Cells[this.gridEnv_columnKey.Index];
					var cellValue = row.Cells[this.gridEnv_columnValue.Index];
					if(cellKey.Value != null && cellValue != null) {
						var pair = new TPair<string, string>();
						pair.First = (string)cellKey.Value;
						pair.Second = (string)cellValue.Value;
						yield return pair;
					}
				}
			}
		}
		#endregion ////////////////////////////////////

		#region ISetCommonData
		#endregion ////////////////////////////////////

		#region ISetLanguage
		public void SetLanguage(Language language)
		{
			ApplyLanguage(language);
			this._language = language;
		}
		#endregion ////////////////////////////////////

		#region override
		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			this._event = true;
		}
		#endregion ////////////////////////////////////

		#region language
		void ApplyLanguage(Language language)
		{
			this.gridEnv_columnKey.SetLanguage(language);
			this.gridEnv_columnValue.SetLanguage(language);
		}
		#endregion ////////////////////////////////////

		#region function
		#endregion ////////////////////////////////////

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
