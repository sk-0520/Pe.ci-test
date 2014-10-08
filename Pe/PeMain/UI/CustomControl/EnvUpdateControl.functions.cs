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
	partial class EnvUpdateControl
	{
		public void SetLanguage(Language language)
		{
			ApplyLanguage(language);
			this._language = language;
		}
		
		public void SetItem(IDictionary<string, string> map)
		{
			this._event = false;
			try {
				var rowList = new List<DataGridViewRow>(map.Count);
				foreach(var item in map) {
					var row = new DataGridViewRow();
					row.CreateCells(this.gridEnv);
					row.Cells[this.gridEnv_columnKey.Index].Value = item.Key;
					row.Cells[this.gridEnv_columnValue.Index].Value = item.Value;
					
					rowList.Add(row);
				}
				this.gridEnv.Rows.Clear();
				this.gridEnv.Rows.AddRange(rowList.ToArray());
			} finally {
				this._event = true;
			}
		}
		
		public void Clear()
		{
			this.gridEnv.Rows.Clear();
		}
	}
}
