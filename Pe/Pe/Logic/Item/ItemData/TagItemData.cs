/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 19:33
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	/// <summary>
	/// Description of TagItemData.
	/// </summary>
	public class TagItemData: ItemData
	{
		public TagItemData()
		{
			Tags = new List<string>();
		}
		
		public override string Name { get { return "tags"; } }
		
		public List<string> Tags { get; private set; }
		
		public override void Clear()
		{
			base.Clear();
			
			Tags.Clear();
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			foreach(var tag in Tags) {
				var dataElement = xml.CreateElement(DataElement.Tag);
				dataElement.SetAttribute(DataElement.Attribute, tag);
				result.AppendChild(dataElement);
			}
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			foreach(XmlElement childElement in element.GetElementsByTagName(DataElement.Tag)) {
				var tag = childElement.GetAttribute(DataElement.Attribute);
				Tags.Add(tag);
			}
		}
	}
}
