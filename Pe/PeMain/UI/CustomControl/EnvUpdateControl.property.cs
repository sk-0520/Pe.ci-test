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

using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class EnvUpdateControl
	{
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
	}
}
