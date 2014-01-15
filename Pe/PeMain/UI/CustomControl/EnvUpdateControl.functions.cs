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
using System.Windows.Controls;
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	public partial class EnvUpdateControl
	{
		void SetLanguage(Language language)
		{
			ApplyLanguage(language);
			this._language = language;
		}
		
		void SetItem(Dictionary<string, string> map)
		{
			this.gridEnv.Rows.Clear();
			
			var rowList = new List<DataGridViewRow>(map.Count);
			foreach(var item in map) {
				var row = this.gridEnv.RowTemplate;
				row.Cells[this.headerKey.Index].Value = item.Key;
				row.Cells[this.headerValue.Index].Value = item.Value;
			}
			
			this.gridEnv.Rows.AddRange(rowList.ToArray());
		}
	}
}
