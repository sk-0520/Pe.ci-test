/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 21:41
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PeMain.UI
{
	public partial class EnvUpdateControl
	{
		public IEnumerable<KeyValuePair<string, string>> Items
		{
			get
			{
				foreach(DataGridViewRow row in this.gridEnv.Rows) {
					if(row.IsNewRow) {
						continue;
					}
					var cellKey = row.Cells[this.headerKey.Index];
					var cellValue = row.Cells[this.headerValue.Index];
					if(cellKey.Value != null && cellValue != null) {
						yield return new KeyValuePair<string, string>((string)cellKey.Value, (string)cellValue.Value);
					}
				}
			}
		}
	}
}
