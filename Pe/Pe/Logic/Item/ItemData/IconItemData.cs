/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 19:31
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	public class IconItemData: ItemData
	{
		const string AttributePath = "path";
		const string AttributeIndex = "index";
		
		public IconItemData()
		{
		}
		
		public override string Name { get { return "icon"; } }
		
		public string Path { get; set; }
		public int Index { get; set; }
		
		public override void Clear()
		{
			base.Clear();
			
			Path = default(string);
			Index = default(int);
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			result.SetAttribute(AttributePath, Path);
			result.SetAttribute(AttributeIndex, Index.ToString());
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var unsafePath = element.GetAttribute(AttributePath);
			var unsafeIndex = element.GetAttribute(AttributeIndex);
			
			int outInt;
			
			Path = unsafePath;
			if(int.TryParse(unsafeIndex, out outInt)) {
				Index = outInt;
			}
		}
	}
}
