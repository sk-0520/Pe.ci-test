/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/08/30
 * 時刻: 20:40
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Text.RegularExpressions;
using System.Xml;

using Pe.IF;

namespace Pe.Logic
{
	/// <summary>
	/// 
	/// </summary>
	public class LanguageItemContainer: ItemContainer<LanguageItem>
	{
		/// <summary>
		/// 
		/// </summary>
		public LanguageItemContainer(): base("language") { }
		
		/// <summary>
		/// 
		/// </summary>
		public override LanguageItem this[string id] {
			get 
			{
				LanguageItem item = null;
				if(TryGetItem(id, out item)) {
					return item;
				}
				
				item = new LanguageItem();
				item.Container = this;
				item.Id = id;
				item.Word = string.Format("<{0}>", id);
				return item; 
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="impArg"></param>
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			foreach(var item in Items) {
				item.Container = this;
			}
		}
	}
}
