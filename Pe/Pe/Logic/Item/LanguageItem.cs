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
	public abstract class LanguageBaseItem: Item
	{
		const string AttributeWord = "word";
		
		/// <summary>
		/// 
		/// </summary>
		public string Word { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			
			Word = default(string);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="impArg"></param>
		/// <returns></returns>
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs impArg)
		{
			var result = base.ToXmlElement(xml, impArg);
			
			result.SetAttribute(AttributeWord, Word);
			
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="impArg"></param>
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var word = element.GetAttribute(AttributeWord);
			Word = word;
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class LanguageDefineItem: LanguageBaseItem {	}
	
	/// <summary>
	/// 
	/// </summary>
	public class LanguageItem: LanguageBaseItem
	{
		const string AttributeWord = "word";
		
		private string text = null;
		
		/// <summary>
		/// 
		/// </summary>
		public ItemContainer<LanguageDefineItem> Container { get; internal protected set; }
		
		/*
		/// <summary>
		/// 
		/// </summary>
		public string Word { get; set; }
		*/
		
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
							return Container[id].Word;
						});
						word = repResult;
					}
					
					this.text = word;
				}
				
				return this.text;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
		}
		
		/// <summary>
		/// 
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			
			Word = default(string);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="impArg"></param>
		/// <returns></returns>
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs impArg)
		{
			var result = base.ToXmlElement(xml, impArg);
			
			result.SetAttribute(AttributeWord, Word);
			
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="impArg"></param>
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var word = element.GetAttribute(AttributeWord);
			Word = word;
		}
	}
	

}
