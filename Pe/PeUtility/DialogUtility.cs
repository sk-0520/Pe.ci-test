/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/13
 * 時刻: 12:11
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PeUtility
{
	public class DialogFilterItem
	{
		public DialogFilterItem()
		{
			Wildcard = new List<string>();
		}
		
		public DialogFilterItem(string display, params string[] wildcard)
		{
			Display = display;
			Wildcard = new List<string>(wildcard);
		}
		public DialogFilterItem(string display, IEnumerable<string> wildcard)
		{
			Display = display;
			Wildcard = new List<string>(wildcard);
		}
		public string Display { get; set; }
		public List<string> Wildcard { get; set; }
		
		public override string ToString()
		{
			return string.Format("{0}|{1}", Display, string.Join(";", Wildcard));
		}
	}
	
	public class DialogFilter
	{
		public DialogFilter()
		{
			Items = new List<DialogFilterItem>();
		}
		
		public List<DialogFilterItem> Items { get; private set; }
		
		public void Attachment(FileDialog dialog)
		{
			dialog.Filter = ToString();
		}
		
		public override string ToString()
		{
			return string.Join("|", Items.Select(i => i.ToString()));
		}

	}

}
