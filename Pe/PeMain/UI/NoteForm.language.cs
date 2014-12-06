/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Linq;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
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
			
			/*
			foreach(var combo in new [] { this.contextMenu_itemForeColor, this.contextMenu_itemBackColor } ) {
				foreach(var item in (IEnumerable<ColorDisplayValue>)combo.ComboBox.DataSource) {
					item.SetLanguage(CommonData.Language);
				}
			}
			*/
		}
	}
}
