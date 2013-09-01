/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 10:51
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	public abstract class NameItem: Item
	{
		const string AttributeName = "name";
		
		public NameItem()
		{
		}
		
		/// <summary>
		/// アイテム名
		/// </summary>
		public string Name { get; set; }
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			result.SetAttribute(AttributeName, Name);
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var name = element.GetAttribute(AttributeName);
			Name = name;
		}
	}
}
