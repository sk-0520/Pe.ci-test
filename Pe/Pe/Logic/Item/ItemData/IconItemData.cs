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
	/// <summary>
	/// 
	/// </summary>
	public class IconItemData: ItemData
	{
		const string AttributePath = "path";
		const string AttributeIndex = "index";
		
		/// <summary>
		/// 
		/// </summary>
		public IconItemData()
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		public override string Name { get { return "icon"; } }
		/// <summary>
		/// 
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Index { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			
			Path = default(string);
			Index = default(int);
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
			
			result.SetAttribute(AttributePath, Path);
			result.SetAttribute(AttributeIndex, Index.ToString());
			
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
