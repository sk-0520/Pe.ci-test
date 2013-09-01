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
	public class LanguageItem: Item
	{
		const string AttributeWord = "word";
		
		private string text = null;
		
		public LanguageItem(LanguageItemContainer lang)
		{
			LanguageItemContainer = lang;
		}
		
		public LanguageItemContainer LanguageItemContainer { get; internal protected set; }
		
		/// <summary>
		/// 
		/// </summary>
		public string Word { get; set; }
		
		/// <summary>
		/// Wordからの変換値
		/// </summary>
		public string Text
		{
			get
			{
				if(this.text == null) {
					
					var word = Word;
					
					if(word.IndexOf('$') != -1) {
						var repResult = Regex.Replace(word, @"\$\{(.+)\}", (Match m) => {
							string s = m.Value;
							var id = s.Substring("${".Length, s.Length - "${}".Length);
							return LanguageItemContainer[id].Text;
						});
						word = repResult;
					}
					
					this.text = word;
				}
				
				return this.text;
			}
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs impArg)
		{
			var result = base.ToXmlElement(xml, impArg);
			
			result.SetAttribute(AttributeWord, Word);
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var word = element.GetAttribute(AttributeWord);
			Word = word;
		}
	}
	
	
	public class LanguageItemContainer: ItemContainer<LanguageItem>, IImportExportXmlElement
	{
		public LanguageItemContainer()
		{
		}
		
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
				
				item = new LanguageItem(this);
				item.Id = id;
				item.Word = string.Format("<{0}>", id);
				return item; 
			}
		}
		
		public XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			throw new NotImplementedException();
		}
		public void FromXmlElement(XmlElement xml, ImportArgs impArg)
		{
			foreach(XmlElement element in xml.GetElementsByTagName("item")) {
				var item = new LanguageItem(this);
				item.FromXmlElement(element, impArg);
				Set(item);
			}
		}
	}
}
