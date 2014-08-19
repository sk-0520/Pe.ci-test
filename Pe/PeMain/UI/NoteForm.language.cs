/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class NoteForm
	{
		void ApplyLanguageMenuItems(ToolStripItemCollection itemCollection)
		{
			if(itemCollection == null || itemCollection.Count == 0) {
				return;
			}
			
			foreach(ToolStripItem item in itemCollection) {
				var menuItem = item as ToolStripMenuItem;
				if(menuItem != null) {
					ApplyLanguageMenuItems(menuItem.DropDownItems);
				}
				item.SetLanguage(CommonData.Language);
			}
		}
		
		void ApplyLanguage()
		{
			ApplyLanguageMenuItems(this.contextMenu.Items);
			foreach(var combo in new [] { this.contextMenu_fore, this.contextMenu_back } ) {
				foreach(var item in (IEnumerable<ColorData>)combo.ComboBox.DataSource) {
					item.SetLanguage(CommonData.Language);
				}
			}
		}
	}
}
