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
	/// 
	/// </summary>
	public class TagItemData: ItemData
	{
		/// <summary>
		/// 
		/// </summary>
		public override string Name { get { return "tags"; } }
		/// <summary>
		/// 
		/// </summary>
		public List<string> Tags { get; private set; }
		
		/// <summary>
		/// 
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			
			Tags = new List<string>();
		}
		/// <summary>
		/// 
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			
			Tags.Clear();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="expArg"></param>
		/// <returns></returns>
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="impArg"></param>
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
